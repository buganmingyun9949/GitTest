using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Framework.Logging;
using Plugin.Exam.Qs.Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using NAudio.Wave;
using ST.Common;
using ST.Common.WebApi;
using ST.Models.Paper;
using VoiceRecorder.Audio;

namespace Plugin.Exam.Qs.ViewModel
{
    public class QsBaseViewModel : ViewModelBase, IDisposable
    {
        public QuestionType QsType { get; set; }


        #region << 属性 字段 >>

        public Button _BtnSkipNext;

        public int QsIndex { get; set; }

        public DispatcherTimer _dTimer { get; set; }

        public string _waveFileName; //临时录音文件

        public RecordState _Recording { get; set; }//是够 录音

        public ExamFlowType _NextFlowType = ExamFlowType.TitleAudio;

        public Paper_DetailItem PaperDetailItem;

        public List<Paper_InfoItem> PaperItems;

        public string QsID;

        private string qsTitle;
        /// <summary>
        /// 题目 一级标题
        /// </summary>
        public string QsTitle
        {
            get
            {
                return qsTitle;
            }
            set
            {
                qsTitle = value;
                RaisePropertyChanged("QsTitle");
            }
        }


        private string qsTitleContent;
        /// <summary>
        /// 题目 一级标题 独白内容
        /// </summary>
        public string QsTitleContent
        {
            get
            {
                return qsTitleContent;
            }
            set
            {
                qsTitleContent = value;
                RaisePropertyChanged("QsTitleContent");
            }
        }


        private string qsTitleAudio;
        /// <summary>
        /// 题目 一级标题 MP3
        /// </summary>
        public string QsTitleAudio
        {
            get
            {
                return qsTitleAudio;
            }
            set
            {
                qsTitleAudio = value;
                RaisePropertyChanged("QsTitleAudio");
            }
        }


        private string promptCommandText;
        /// <summary>
        /// 考试 命令 提示文本
        /// </summary>
        public string PromptCommandText
        {
            get;
            set;
        }


        private int totalTime;
        /// <summary>
        /// 播放总时间
        /// 时间 显示
        /// </summary>
        public int TotalTime
        {
            get;
            set;
        }


        private int playTime;
        /// <summary>
        /// 播放 时间
        /// 时间 显示
        /// </summary>
        public int PlayTime
        {
            get;
            set;
        }

        #endregion

        #region << 自定义方法 >>

        public virtual void BeginExam(ExamFlowType nextFlowType)
        {

        }

        public void BtnSkipNext_Click(object sender, RoutedEventArgs e)
        {
            CleanUp();

            _BtnSkipNext.IsEnabled = false;
            BeginExam(_NextFlowType);
        }


        /// <summary>
        /// 打开下一大题窗口
        /// </summary>
        public void NextQsView()
        {
            CleanUp();

            _BtnSkipNext.Click -= BtnSkipNext_Click;
            _BtnSkipNext = null;

            Messenger.Default.Send(new ExamNextQsNavigateMessage(++QsIndex, 0, 0),
                "ExamNextQsView");
        }

        /// <summary>
        /// 发送进度消息
        /// </summary>
        public void SendProgress()
        {
            Messenger.Default.Send(new ExamQsCmdMessage(PlayTime, TotalTime, PromptCommandText),
                "ExamQsMainWinCmdView");
        }

        /// <summary>
        /// 发送 录音
        /// </summary>
        public void SendSampleAggregator()
        {
            Messenger.Default.Send(new ExamQsSampleAggregatorMessage(GlobalUser.Recorder?.SampleAggregator),
                "ExamQsSampleAggregatorView");
        }

        public void PlayAudio(string audioFile)
        {
            GlobalUser.WavePlayer?.Stop();

            try
            {
                GlobalUser.WavePlayer = CreateWavePlayer();

                MediaFoundationReader mfr = new MediaFoundationReader($"{WebApiProxy.MEDIAURL}{audioFile}");
                GlobalUser.WavePlayer.Init(mfr);//存入 网络音频地址
                GlobalUser.WavePlayer.PlaybackStopped += OnPlaybackStopped;

                TotalTime = Convert.ToInt32(mfr.TotalTime.Minutes * 60 + mfr.TotalTime.Seconds);
                PlayTime = 0;

                SendProgress();
                //int totalTime = GlobalUser.AudioFileReader.TotalTime.Seconds;
                //AudioPanelVM.SetSpeakerEnabled(TotalTime);

                //PlayingTime = 0;
                //_IsAudioPlay = true;
                _dTimer.Start();
                GlobalUser.WavePlayer.Play();
            }
            catch (Exception e)
            {
                Log4NetHelper.Error(
                    $"Play Audio Error ---- {e.Message} ----  {e.StackTrace} -------- url: {WebApiProxy.MEDIAURL}{audioFile}");

                BeginExam(_NextFlowType);
            }
        }

        public void PlaySysAudio(string audioFile)
        {
            GlobalUser.WavePlayer?.Stop();
            GlobalUser.WavePlayer?.Dispose();
            try
            {
                GlobalUser.WavePlayer = CreateWavePlayer();

                AudioFileReader afr = new AudioFileReader(audioFile);
                GlobalUser.WavePlayer.Init(afr);//存入 网络音频地址
                GlobalUser.WavePlayer.PlaybackStopped += OnPlaybackBySysStopped;

                //TotalTime = Convert.ToInt32(afr.TotalTime.TotalSeconds);
                //TotalTime = Convert.ToInt32(afr.TotalTime.Minutes * 60 + afr.TotalTime.Seconds) + 1;
                //PlayTime = 0;
                if (_NextFlowType == ExamFlowType.NextQs)
                {
                    TotalTime = Convert.ToInt32(afr.TotalTime.Minutes * 60 + afr.TotalTime.Seconds) + 1;
                    PlayTime = 0;
                    _dTimer.Start();
                }

                GlobalUser.WavePlayer.Play();
            }
            catch (Exception e)
            {
                Log4NetHelper.Error(String.Format("Play Audio Error {0}", e.Message));
            }
        }

        private IWavePlayer CreateWavePlayer()
        {
            return new WaveOut();
        }

        void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            //CleanUp();

            //BeginExam(_NextFlowType);

            if (e.Exception != null)
            {
                Log4NetHelper.Error(String.Format("Playback Stopped due to an error {0}", e.Exception.Message));
            }
        }

        void OnPlaybackBySysStopped(object sender, StoppedEventArgs e)
        {
            if (_Recording == RecordState.Recording)
                PlayRecorder();
            else if (_Recording == RecordState.StopRecord)
            {

            }

            if (e.Exception != null)
            {
                Log4NetHelper.Error(String.Format("Playback Stopped due to an error {0}", e.Exception.Message));
            }
        }


        public void PlayRecorderWithSysAduio(string audioName)
        {
            PlaySysAudio(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", $"{audioName}.mp3"));
        }

        public void PlayRecorder()
        {
            GlobalUser.WavePlayer?.Stop();

            GlobalUser.Recorder = new AudioRecorder();
            GlobalUser.Recorder.Stopped += OnRecorderStopped;
            BeginMonitoring();
            BeginRecording();
            //ShowWaveForm = true;
            //SampleAggregator = GlobalUser.Recorder.SampleAggregator;//同步 波形
        }

        private void BeginMonitoring()
        {
            GlobalUser.Recorder?.Stop();
            GlobalUser.Recorder?.SetRecordTime(TotalTime);
            GlobalUser.Recorder?.BeginMonitoring();
            SendProgress();
        }

        private void BeginRecording()
        {
            _waveFileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".wav");
            GlobalUser.Recorder.BeginRecording(_waveFileName);
            _dTimer.Start();
            SendSampleAggregator();
        }

        void OnRecorderStopped(object sender, EventArgs e)
        {
            //录音完成
            //直接评分
            //todo
            if (_Recording == RecordState.Recording || GlobalUser.Recorder?.RecordingState == RecordingState.Stopped)
            {
                _Recording = RecordState.StopRecord;
                //PlayRecorderWithSysAduio("stoprecord");
                SendSampleAggregator();
            }

            Log4NetHelper.Info($"开始评分 -- 录音文件: {_waveFileName}");

            if (e != null)
            {
                Log4NetHelper.Error(String.Format("Record Stopped due to an error {0}", e));
            }
        }


        public void CleanUp()
        {
            if (GlobalUser.AudioFileReader != null)
            {
                GlobalUser.AudioFileReader?.Dispose();
                GlobalUser.AudioFileReader = null;
            }
            if (GlobalUser.WavePlayer != null)
            {
                GlobalUser.WavePlayer?.Stop();
                GlobalUser.WavePlayer?.Dispose();
                GlobalUser.WavePlayer = null;
            }
            if (GlobalUser.Recorder != null)
            {
                GlobalUser.Recorder?.SampleAggregator?.RaiseRestart();
                GlobalUser.Recorder?.Stop();
                GlobalUser.Recorder?.Dispose();
                GlobalUser.Recorder = null;
            }

            _dTimer?.Stop();
        }

        public void Dispose()
        {
            CleanUp();

            GC.Collect(0);
        }

        #endregion

    }
}
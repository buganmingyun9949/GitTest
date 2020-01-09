using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
using ST.Common.ToolsHelper;
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
        public int QsCount { get; set; }

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


        private bool _PlayingIconEnable;
        /// <summary>
        ///  播放按钮 转圈
        /// </summary>
        public bool PlayingIconEnable
        {
            get
            {
                return _PlayingIconEnable;
            }
            set
            {
                _PlayingIconEnable = value;
                RaisePropertyChanged("PlayingIconEnable");
            }
        }

        private bool _RecordingEnable;
        /// <summary>
        ///  录音按钮 转圈
        /// </summary>
        public bool RecordingEnable
        {
            get
            {
                return _RecordingEnable;
            }
            set
            {
                _RecordingEnable = value;
                RaisePropertyChanged("RecordingEnable");
            }
        }

        #endregion

        #region << 自定义方法 >>

        public virtual void OnScoreCallback(SyncScoreCallbckMessage msg)
        {
        }

        public virtual void ShowPlayingResult(string result)
        {
        }

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

            if (GlobalUser.DoAnswer)
            {

                _BtnSkipNext.Click -= BtnSkipNext_Click;
                _BtnSkipNext = null;

                if (GlobalUser.MenuType == MenuType.Task)
                {
                    //作业模式触发
                    string zyId = GlobalUser.SelectPaperNumber.Split('#')[0];

                    foreach (var taskInfo in GlobalUser.USER.UserZy.Where(w =>
                        w.UserPhone == GlobalUser.USER.Mobile && w.ZyID == zyId))
                    {
                        foreach (var taskSubInfo
                            in taskInfo.ZySubs.Where(w => w.SubId == PaperDetailItem.qs_id))
                        {
                            taskSubInfo.Done = true;
                        }
                    }
                }

                Messenger.Default.Send(new ExamNextQsNavigateMessage(++QsIndex, 0, 0),
                    "ExamNextQsView");
            }
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
            string reueUrl = "";
            try
            {
                GlobalUser.WavePlayer = CreateWavePlayer();

                //reueUrl = WebApiProxy.GetRedirectUrl($"{WebApiProxy.MEDIAURL}{audioFile}");

                string pfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER, SecurityHelper
                        .HmacMd5Encrypt(GlobalUser.SelectPaperName + GlobalUser.SelectPaperNumber,
                            GlobalUser.FILEPWD, Encoding.UTF8).ToUpper(),
                    SecurityHelper.HmacMd5Encrypt(Path.GetFileNameWithoutExtension(audioFile), GlobalUser.FILEPWD,
                        Encoding.UTF8).ToLower() + ".qf");

                if(!File.Exists(pfile))
                {
                    throw new Exception("无效的本地内容文件");
                }

                var sfile = FileSecretHelper.DecryptFile0(pfile);

                //if (!audioFile.ToLower().Contains(".mp3"))
                //    reueUrl = $"{audioFile}.mp3";
                Mp3FileReader mfr = new Mp3FileReader(sfile); 
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
            catch (Exception ex)
            {
                _dTimer?.Stop();
                Log4NetHelper.Error($"Play Audio Error ---- ---- {ex.Message} ----  {ex.StackTrace}");
                try
                {
                    GlobalUser.WavePlayer?.Stop();
                    GlobalUser.WavePlayer?.Dispose();

                    GlobalUser.WavePlayer = CreateWavePlayer();
                    if (audioFile.ToLower().Contains("records."))
                        reueUrl = $"http://{audioFile}.mp3";
                    else
                        reueUrl = WebApiProxy.GetRedirectUrl($"{WebApiProxy.MEDIAURL}{audioFile}"); 

                    MediaFoundationReader mfr = new MediaFoundationReader(reueUrl);
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
                        $"Play Audio Error ---- {e.Message} ----  {e.StackTrace} \r\n-------- url: {WebApiProxy.MEDIAURL}{audioFile} \r\n-------- True Url:{reueUrl}");

                    BeginExam(_NextFlowType);
                }
            }
        }

        public void PlayAudio1(string audioFile, bool isSend = true)
        {
            GlobalUser.WavePlayer?.Stop();
            string reueUrl = "";
            try
            {
                GlobalUser.WavePlayer = CreateWavePlayer();

                string pfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER, SecurityHelper
                        .HmacMd5Encrypt(GlobalUser.SelectPaperName + GlobalUser.SelectPaperNumber,
                            GlobalUser.FILEPWD, Encoding.UTF8).ToUpper(),
                    SecurityHelper.HmacMd5Encrypt(Path.GetFileNameWithoutExtension(audioFile), GlobalUser.FILEPWD,
                        Encoding.UTF8).ToLower() + ".qf");

                if (!File.Exists(pfile))
                {
                    throw new Exception("无效的本地内容文件");
                }

                var sfile = FileSecretHelper.DecryptFile0(pfile);

                //if (!audioFile.ToLower().Contains(".mp3"))
                //    reueUrl = $"{audioFile}.mp3";
                Mp3FileReader mfr = new Mp3FileReader(sfile);
                //reueUrl = WebApiProxy.GetRedirectUrl($"{WebApiProxy.MEDIAURL}{audioFile}");

                //if (!audioFile.ToLower().Contains(".mp3"))
                //    reueUrl = $"{audioFile}.mp3";

                //AudioFileReader afr =
                //    new AudioFileReader(Path.Combine(GlobalUser.AUDIODATAFOLDER, Path.GetFileName(audioFile)));
                GlobalUser.WavePlayer.Init(mfr);//存入 网络音频地址
                GlobalUser.WavePlayer.PlaybackStopped += OnPlaybackStopped1;

                //TotalTime = Convert.ToInt32(mfr.TotalTime.Minutes * 60 + mfr.TotalTime.Seconds);
                //PlayTime = 0;

                if (isSend)
                    SendProgress();

                GlobalUser.WavePlayer.Play();
            }
            catch (Exception)
            {
                try
                {
                    GlobalUser.WavePlayer?.Stop();
                    GlobalUser.WavePlayer?.Dispose();

                    GlobalUser.WavePlayer = CreateWavePlayer();
                    if (audioFile.ToLower().Contains("records."))
                        reueUrl = $"http://{audioFile}.mp3";
                    else
                        reueUrl = WebApiProxy.GetRedirectUrl($"{WebApiProxy.MEDIAURL}{audioFile}");

                    MediaFoundationReader mfr = new MediaFoundationReader(reueUrl);
                    GlobalUser.WavePlayer.Init(mfr);//存入 网络音频地址
                    GlobalUser.WavePlayer.PlaybackStopped += OnPlaybackStopped1;

                    TotalTime = Convert.ToInt32(mfr.TotalTime.Minutes * 60 + mfr.TotalTime.Seconds);
                    PlayTime = 0;

                    SendProgress();

                    GlobalUser.WavePlayer.Play();
                }
                catch (Exception e)
                {
                    Log4NetHelper.Error(
                        $"Play Audio Error ---- {e.Message} ----  {e.StackTrace} \r\n-------- url: {WebApiProxy.MEDIAURL}{audioFile} \r\n-------- True Url:{reueUrl}");

                    BeginExam(_NextFlowType);
                }
            }
        }

        public void PlaySysAudio(string audioFile)
        {
            GlobalUser.WavePlayer?.Stop();
            GlobalUser.WavePlayer?.Dispose();
            try
            {
                GlobalUser.WavePlayer = CreateWavePlayer();

                //string pfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER, SecurityHelper
                //        .HmacMd5Encrypt(GlobalUser.SelectPaperName + GlobalUser.SelectPaperNumber,
                //            GlobalUser.FILEPWD, Encoding.UTF8).ToUpper(),
                //    SecurityHelper.HmacMd5Encrypt(Path.GetFileNameWithoutExtension(audioFile), GlobalUser.FILEPWD,
                //        Encoding.UTF8).ToLower() + ".qf");

                //if (!File.Exists(pfile))
                //{
                //    throw new Exception("无效的本地内容文件");
                //}

                //var sfile = FileSecretHelper.DecryptFile0(pfile);

                //if (!audioFile.ToLower().Contains(".mp3"))
                //    reueUrl = $"{audioFile}.mp3";
                //Mp3FileReader mfr = new Mp3FileReader(sfile);

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

        void OnPlaybackStopped1(object sender, StoppedEventArgs e)
        {
            PlayingIconEnable = false;

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

        public void PlayRecorder1()
        {
            GlobalUser.WavePlayer?.Stop();

            GlobalUser.Recorder = new AudioRecorder();
            GlobalUser.Recorder.Stopped += OnRecorderStopped1;
            BeginMonitoring(false);
            BeginRecording(false);
        }

        private void BeginMonitoring(bool isSend = true)
        {
            GlobalUser.Recorder?.Stop();
            GlobalUser.Recorder?.SetRecordTime(TotalTime);
            GlobalUser.Recorder?.BeginMonitoring();
            if (isSend)
                SendProgress();
        }

        private void BeginRecording(bool isView = true)
        {
            _waveFileName = Path.Combine(GlobalUser.AUDIODATAFOLDER, Guid.NewGuid() + ".wav");
            GlobalUser.Recorder.BeginRecording(_waveFileName);
            _dTimer.Start();
            if (isView)
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

        void OnRecorderStopped1(object sender, EventArgs e)
        {
            //录音完成
            //直接评分
            //todo
            if (_Recording == RecordState.Recording || GlobalUser.Recorder?.RecordingState == RecordingState.Stopped)
            {
                _Recording = RecordState.StopRecord;
            }
            RecordingEnable = false;
            //PlayingIconEnable = false;
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

           // _dTimer?.Stop();

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                _dTimer?.Stop(); 
            }));    

        }

        public void Dispose()
        {
            CleanUp();

            GC.Collect(0); 
        }

        #endregion

    }
}
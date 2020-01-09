using System;
using System.Collections.Generic;
using System.Windows.Threading;
using Framework.Logging;
using GalaSoft.MvvmLight;
using NAudio.Wave;
using ST.Common;
using ST.Common.WebApi;
using ST.Models.Paper;

namespace Plugin.Exam.Report.ViewModel
{
    public class QsBaseRVM : ViewModelBase, IDisposable
    {
        public QsBaseRVM()
        {
            Dispose();
        }

        #region << 属性 字段 >>
        public int QsIndex { get; set; }

        public DispatcherTimer _dTimer { get; set; }

        public string _waveFileName; //临时录音文件

        /// <summary>
        /// 题目 大题
        /// </summary>
        public Paper_DetailItem PaperDetail;

        /// <summary>
        /// 答题记录
        /// </summary>
        public Exam_Attend_Result ExamResult;

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

        private string _PaperDetailUserScore;
        /// <summary>
        /// 我的 答题 分数
        /// </summary>
        public string PaperDetailUserScore
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_PaperDetailUserScore))
                    return "0";
                return _PaperDetailUserScore;
            }
            set
            {
                _PaperDetailUserScore = value;
                RaisePropertyChanged("PaperDetailUserScore");
            }
        }

        private string _PaperDetailTotalScore;
        /// <summary>
        /// 试卷 题目 满分
        /// </summary>
        public string PaperDetailTotalScore
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_PaperDetailTotalScore))
                    return "0";
                return _PaperDetailTotalScore;
            }
            set
            {
                _PaperDetailTotalScore = value;
                RaisePropertyChanged("PaperDetailTotalScore");
            }
        }

        private string _PlayIcon;

        /// <summary>
        /// 播放按钮图标
        /// </summary>
        public string PlayIcon
        {
            get
            {
                return _PlayIcon;
            }
            set
            {
                _PlayIcon = value;
                RaisePropertyChanged("PlayIcon");
            }
        }

        private string _UserCoherence;
        public string UserCoherence
        {
            get => _UserCoherence;
            set
            {
                if (_UserCoherence != value)
                {
                    _UserCoherence = value;
                    RaisePropertyChanged("UserCoherence");
                }
            }
        }
        private string _UserPronunciation;
        public string UserPronunciation
        {
            get => _UserPronunciation;
            set
            {
                if (_UserPronunciation != value)
                {
                    _UserPronunciation = value;
                    RaisePropertyChanged("UserPronunciation");
                }
            }
        }
        private string _UserFluency;
        public string UserFluency
        {
            get => _UserFluency;
            set
            {
                if (_UserFluency != value)
                {
                    _UserFluency = value;
                    RaisePropertyChanged("UserFluency");
                }
            }
        }
        private string _UserIntegrity;
        public string UserIntegrity
        {
            get => _UserIntegrity;
            set
            {
                if (_UserIntegrity != value)
                {
                    _UserIntegrity = value;
                    RaisePropertyChanged("UserIntegrity");
                }
            }
        }
        private string _UserSpeed;
        public string UserSpeed
        {
            get => _UserSpeed;
            set
            {
                if (_UserSpeed != value)
                {
                    _UserSpeed = value;
                    RaisePropertyChanged("UserSpeed");
                }
            }
        }

        #endregion

        #region << 自定义方法 >>


        #region << 播放 >>

        public void PlayAudio(string url)
        {

            GlobalUser.WavePlayer?.Stop();

            try
            {
                GlobalUser.WavePlayer = CreateWavePlayer();
                MediaFoundationReader mfr = new MediaFoundationReader($"{WebApiProxy.MEDIAURL}{url}");
                GlobalUser.WavePlayer.Init(mfr);
                GlobalUser.WavePlayer.PlaybackStopped += OnPlaybackStopped;
                GlobalUser.WavePlayer.Play();

            }
            catch (Exception e)
            {
                Log4NetHelper.Error(String.Format("Play Error {0}", e.Message));
            }
        }

        private IWavePlayer CreateWavePlayer()
        {
            return new WaveOut();
        }

        void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {

            GlobalUser.WavePlayer?.Stop();
            PlayIcon = "Play";

            // we want to be always on the GUI thread and be able to change GUI components
            //Debug.Assert(!InvokeRequired, "PlaybackStopped on wrong thread");
            // we want it to be safe to clean up input stream and playback device in the handler for PlaybackStopped
            CleanUp();
            //labelNowTime.Text = "0";
            if (e.Exception != null)
            {
                Log4NetHelper.Error(String.Format("Playback Stopped due to an error {0}", e.Exception.Message));
            }
        }

        #endregion


        public string GetRefText(string answerText)
        {
            var answerList = answerText.Split(new string[] {"|"}, StringSplitOptions.None);

            string showRefText = "";

            //for (int i = 0; i < answerList.Length; i++)
            //{
            //    if (i < 2)
            //    {
            //        showRefText += answerList[i] + "|";
            //    }
            //}

            if (answerList.Length > 2)
            {
                return $"{answerList[0]}|{answerList[1]}";
            }


            return answerText;
        }

        #endregion

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
        }

        public void Dispose()
        {
            CleanUp();

            GC.Collect(0);
        }
    }
}
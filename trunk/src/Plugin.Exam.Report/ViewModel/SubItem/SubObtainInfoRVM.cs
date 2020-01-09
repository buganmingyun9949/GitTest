using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using NAudio.Wave;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Paper;
using ST.Models.Score;

namespace Plugin.Exam.Report.ViewModel.SubItem
{
    /// <summary>
    /// 信息获取 小题
    /// </summary>
    public class SubObtainInfoRVM : QsBaseRVM
    {

        #region << 属性 >>

        private const string MediaPic = ".JPG .GIF .PNG .BMP .JPEG";

        private Exam_Attend_Result_Item ExamItemResult;

        private string sourceAudio = "";

        private string infotitleAudio = "";

        private string _QsInfoContent;
        public string QsInfoContent
        {
            get => _QsInfoContent;
            set
            {
                if (_QsInfoContent != value)
                {
                    _QsInfoContent = value;
                    RaisePropertyChanged("QsInfoContent");
                }
            }
        }

        private string _QsItemContent;
        public string QsItemContent
        {
            get => _QsItemContent;
            set
            {
                if (_QsItemContent != value)
                {
                    _QsItemContent = value;
                    RaisePropertyChanged("QsItemContent");
                }
            }
        }

        private string _QsTitle2;
        public string QsTitle2
        {
            get => _QsTitle2;
            set
            {
                if (_QsTitle2 != value)
                {
                    _QsTitle2 = value;
                    RaisePropertyChanged("QsTitle2");
                }
            }
        }

        private string _SpeakRefText;
        public string SpeakRefText
        {
            get => _SpeakRefText;
            set
            {
                if (_SpeakRefText != value)
                {
                    _SpeakRefText = value;
                    RaisePropertyChanged("SpeakRefText");
                }
            }
        }

        private Visibility _ShowInfoTitlePlayBtn;
        /// <summary>
        /// 音频 播放按钮
        /// </summary>
        public Visibility ShowInfoTitlePlayBtn
        {
            get
            {
                return _ShowInfoTitlePlayBtn;
            }
            set
            {
                _ShowInfoTitlePlayBtn = value;
                RaisePropertyChanged("ShowInfoTitlePlayBtn");
            }
        }

        private Visibility _ShowPlayBtn;
        /// <summary>
        /// 音频 播放按钮
        /// </summary>
        public Visibility ShowPlayBtn
        {
            get
            {
                return _ShowPlayBtn;
            }
            set
            {
                _ShowPlayBtn = value;
                RaisePropertyChanged("ShowPlayBtn");
            }
        }

        private string _PlayUserAudioUrl;
        /// <summary>
        /// 我的录音音频
        /// </summary>
        public string PlayUserAudioUrl
        {
            get
            {
                return _PlayUserAudioUrl;
            }
            set
            {
                _PlayUserAudioUrl = value;
                RaisePropertyChanged("PlayUserAudioUrl");
            }
        }

        private string _UserItemScore;
        public string UserItemScore
        {
            get => _UserItemScore;
            set
            {
                if (_UserItemScore != value)
                {
                    _UserItemScore = value;
                    RaisePropertyChanged("UserItemScore");
                }
            }
        }

        private string _ItemTotalScore;
        public string ItemTotalScore
        {
            get => _ItemTotalScore;
            set
            {
                if (_ItemTotalScore != value)
                {
                    _ItemTotalScore = value;
                    RaisePropertyChanged("ItemTotalScore");
                }
            }
        }

        private object _QsItemImage;
        public object QsItemImage
        {
            get => _QsItemImage;
            set
            {
                if (_QsItemImage != value)
                {
                    _QsItemImage = value;
                    RaisePropertyChanged("QsItemImage");
                }
            }
        }

        #endregion

        #region << 按钮 command >>


        private RelayCommand _PlayQsItemAudio;//播放 音频

        public RelayCommand PlayQsItemAudio
        {
            get
            {
                return _PlayQsItemAudio ?? (_PlayQsItemAudio = new RelayCommand(
                           () =>
                           {
                               if (GlobalUser.WavePlayer?.PlaybackState == PlaybackState.Playing)
                               {
                                   GlobalUser.WavePlayer?.Stop();
                               }

                               if (!string.IsNullOrEmpty(sourceAudio))
                               {
                                   PlayAudio(sourceAudio);
                                   PlayIcon = "18-1-1";
                               }

                           }));
            }
        }


        private RelayCommand _PlayInfoTitleAudio;//播放 音频

        public RelayCommand PlayInfoTitleAudio
        {
            get
            {
                return _PlayInfoTitleAudio ?? (_PlayInfoTitleAudio = new RelayCommand(
                           () =>
                           {
                               if (GlobalUser.WavePlayer?.PlaybackState == PlaybackState.Playing)
                               {
                                   GlobalUser.WavePlayer?.Stop();
                               }

                               if (!string.IsNullOrEmpty(infotitleAudio))
                               {
                                   PlayAudio(infotitleAudio);
                                   PlayIcon = "18-1";
                               }

                           }));
            }
        }
        #endregion

        public SubObtainInfoRVM(Paper_InfoItem paperInfoItem, int itemIndex, Exam_Attend_Result_Item examItemResult, string infotitle, string infotitleaudio, bool showPlayBtn = true)
        {
            CleanUp();
            if (paperInfoItem.items.Count > 1)
                QsTitle2 = infotitle;

            infotitleAudio = infotitleaudio;
            if (string.IsNullOrEmpty(infotitleaudio))
            {
                ShowInfoTitlePlayBtn = Visibility.Collapsed;
            }


            if (!showPlayBtn)
            {
                ShowPlayBtn = Visibility.Collapsed;
            }

            if (!string.IsNullOrEmpty(paperInfoItem.items[itemIndex].source_content))
            {
                ShowPlayBtn = Visibility.Visible;
                sourceAudio = paperInfoItem.items[itemIndex].source_content;
            }

            ExamItemResult = examItemResult;

            if (paperInfoItem.items.Count == 1)
                QsInfoContent = infotitle;

            QsItemContent = $"{paperInfoItem.items[itemIndex].item_content}";

            if (!string.IsNullOrEmpty(paperInfoItem.items[itemIndex].img_source_content) &&
                MediaPic.Contains(paperInfoItem.items[itemIndex].img_source_content.ToUpper().Split('.').LastOrDefault()))
            {
                string url = WebApiProxy.GetRedirectUrl($"{WebApiProxy.MEDIAURL}{paperInfoItem.items[itemIndex].img_source_content}");
                QsItemImage = new BitmapImage(new Uri(url));
            }

            ItemTotalScore = $"{paperInfoItem.items[itemIndex].item_score}分";

            if (string.IsNullOrEmpty(sourceAudio))
            {
                ShowPlayBtn = Visibility.Collapsed;
            }

            PlayUserAudioUrl = examItemResult.user_answer;


            ScoreRoot score = JsonHelper.FromJson<ScoreRoot>(examItemResult.score_result.Replace("\"params\"", "\"param\""));

            SpeakRefText = GetRefText(score.param.request.refText);
            UserItemScore = $"{score.result.overall}分";
        }
    }
}
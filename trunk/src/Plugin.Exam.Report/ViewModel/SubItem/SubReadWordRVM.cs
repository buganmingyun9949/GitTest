using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Framework.Logging;
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
    /// 句子
    /// </summary>
    public class SubReadWordRVM : QsBaseRVM
    {

        #region << 属性 >>

        private const string MediaPic = ".JPG .GIF .PNG .BMP .JPEG";

        private Exam_Attend_Result_Item ExamItemResult;

        private string sourceAudio = "";

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

        private string _SyncQsYbContent;

        /// <summary>
        /// 音标
        /// </summary>
        public string SyncQsYbContent
        {
            get
            {
                return _SyncQsYbContent;
            }
            set
            {
                _SyncQsYbContent = value;
                RaisePropertyChanged("SyncQsYbContent");
            }
        }

        private string _SyncQsZhContent;

        /// <summary>
        /// 单词 中文
        /// </summary>
        public string SyncQsZhContent
        {
            get
            {
                return _SyncQsZhContent;
            }
            set
            {
                _SyncQsZhContent = value;
                RaisePropertyChanged("SyncQsZhContent");
            }
        }

        private object _QsInfoImage;
        /// <summary>
        /// 单词 图片
        /// </summary>
        public object QsInfoImage
        {
            get
            {
                return _QsInfoImage;
            }
            set
            {
                _QsInfoImage = value;
                RaisePropertyChanged("QsInfoImage");
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
                                   PlayIcon = "16-1-1";
                               }

                           }));
            }
        }

        #endregion

        public SubReadWordRVM(Paper_ItemsItem paperItemsItem, int itemIndex, Exam_Attend_Result_Item examItemResult, bool showPlayBtn = true)
        {
            CleanUp();

            ExamItemResult = examItemResult;

            QsItemContent = $"{itemIndex + 1}、{paperItemsItem.item_content}";

            try
            {
                KeyWordEx keyWord = JsonHelper.FromJson<KeyWordEx>(paperItemsItem.item_keyword);
                SyncQsYbContent = $"[ {keyWord.yb} ]";
                SyncQsZhContent = keyWord.desc;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error($"单词 KeyWord 绑定异常:{paperItemsItem.item_keyword}", ex);
            }

            if (!string.IsNullOrEmpty(paperItemsItem.img_source_content) &&
                MediaPic.Contains(paperItemsItem.img_source_content.ToUpper().Split('.').LastOrDefault()))
            {
                string url = WebApiProxy.GetRedirectUrl($"{WebApiProxy.MEDIAURL}{paperItemsItem.img_source_content}");
                QsInfoImage = new BitmapImage(new Uri(url));
            }

            sourceAudio = paperItemsItem.source_content;
            ItemTotalScore = $"{paperItemsItem.item_score}分";

            if (string.IsNullOrEmpty(sourceAudio))
            {
                ShowPlayBtn = Visibility.Collapsed;
            }

            PlayUserAudioUrl = examItemResult.user_answer;

            ScoreRoot score = JsonHelper.FromJson<ScoreRoot>(examItemResult.score_result.Replace("\"params\"", "\"param\""));
            UserItemScore = $"{score.result.overall}分";
        }
    }
}
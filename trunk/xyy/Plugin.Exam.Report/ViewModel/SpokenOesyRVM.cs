using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using NAudio.Wave;
using Plugin.Exam.Report.View.SubItem;
using Plugin.Exam.Report.ViewModel.SubItem;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Paper;
using ST.Models.Score;

namespace Plugin.Exam.Report.ViewModel
{
    public class SpokenOesyRVM : QsBaseRVM
    {
        #region << 属性 >>

        private const string MediaPic = ".JPG .GIF .PNG .BMP .JPEG";

        private StackPanel _qsItemContent;

        private string userAudio = "";

        private string paperContentAudio = "";

        private int _SelectedChoiceExamShow;

        public int SelectedChoiceExamShow
        {
            get => _SelectedChoiceExamShow;
            set
            {
                if (_SelectedChoiceExamShow != value)
                {
                    _SelectedChoiceExamShow = value;
                    RaisePropertyChanged("SelectedChoiceExamShow");
                    BindSubItem();
                }
            }
        }

        private string titleTwo;

        /// <summary>
        /// 二级 题目 标题
        /// </summary>
        public string TitleTwo
        {
            get
            {
                return titleTwo;
            }
            set
            {
                titleTwo = value;
                RaisePropertyChanged("TitleTwo");
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

        private string _SpeakRefText;
        /// <summary>
        /// 参考文本内容 1
        /// </summary>
        public string SpeakRefText
        {
            get
            {
                return _SpeakRefText;
            }
            set
            {
                _SpeakRefText = value;
                RaisePropertyChanged("SpeakRefText");
            }
        }

        private object _QsInfoImage;
        /// <summary>
        /// Image
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


        private RelayCommand _PlayQsAudio;//播放 音频

        public RelayCommand PlayQsAudio
        {
            get
            {
                return _PlayQsAudio ?? (_PlayQsAudio = new RelayCommand(
                           () =>
                           {
                               if (GlobalUser.WavePlayer?.PlaybackState == PlaybackState.Playing)
                               {
                                   GlobalUser.WavePlayer?.Stop();
                               }

                               if (!string.IsNullOrEmpty(paperContentAudio))
                               {
                                   PlayAudio(paperContentAudio);
                                   PlayIcon = "12-1";
                               }

                           }));
            }
        }

        #endregion

        public SpokenOesyRVM(StackPanel qsItemContent, Paper_DetailItem paperDetail, Exam_Attend_Result examResult)
        {
            _qsItemContent = qsItemContent;
            PaperDetail = paperDetail;
            ExamResult = examResult;

            QsTitle = PaperDetail.qs_title;
            QsTitleContent = PaperDetail.qs_content;
            QsTitleAudio = PaperDetail.source_content;

            SelectedChoiceExamShow = 0;

            //绑定分数
            BindUserTotalScore();
            //绑定选项内容
            BindSubItem();
        }

        #region << 自定义方法 >>

        /// <summary>
        /// 绑定 选择题 内容
        /// </summary>
        private void BindSubItem()
        {
            var detailItem = PaperDetail.info[0].items[0];
            var itemAnswerResult = ExamResult.result_items.Where(w => w.item_id == detailItem.item_id).ToList()[0];

            _qsItemContent.Children.Clear();

            TitleTwo = detailItem.item_content;
            if (!string.IsNullOrEmpty(PaperDetail.info[0].source_content) &&
                MediaPic.Contains(PaperDetail.info[0].source_content.ToUpper().Split('.').LastOrDefault()))
            {
                QsInfoImage = new BitmapImage(new Uri($"{WebApiProxy.MEDIAURL}{PaperDetail.info[0].source_content}"));
            }
            if (!string.IsNullOrEmpty(detailItem.img_source_content) &&
                MediaPic.Contains(detailItem.img_source_content.ToUpper().Split('.').LastOrDefault()))
            {
                QsInfoImage = new BitmapImage(new Uri($"{WebApiProxy.MEDIAURL}{detailItem.img_source_content}"));
            }

            paperContentAudio = detailItem.source_content;
            PlayUserAudioUrl = itemAnswerResult.user_answer;

            if (string.IsNullOrEmpty(paperContentAudio))
            {
                ShowPlayBtn = Visibility.Collapsed;
            }

            SpeakRefText = GetRefText(detailItem.answers[0].answer_content);//detailItem.answers[0].answer_content);


            ScoreRoot score = JsonHelper.FromJson<ScoreRoot>(itemAnswerResult.score_result.Replace("\"params\"", "\"param\""));

            UserPronunciation = (100 * score.result.pronunciation / Convert.ToSingle(PaperDetailTotalScore)).ToString("0");
            UserFluency = (100 * score.result.fluency / Convert.ToSingle(PaperDetailTotalScore)).ToString("0");
            UserCoherence = (100 * score.result.coherence / Convert.ToSingle(PaperDetailTotalScore)).ToString("0");
            UserSpeed = $"{score.result.speed.ToString("0")} 词/分钟";
        }

        /// <summary>
        /// 绑定分数
        /// </summary>
        private void BindUserTotalScore()
        {
            float totaolScore = 0;
            float? userScore = 0;

            var myAllItemId = string.Join(" ", (ExamResult.result_items.Select(s => s.item_id).ToArray()));

            for (int i = 0; i < PaperDetail.info[0].items.Count; i++)
            {
                totaolScore = totaolScore + PaperDetail.info[0].items[i].item_score;

                userScore = userScore + ExamResult.result_items
                                .Where(w => w.item_id == PaperDetail.info[0].items[i].item_id).ToList()[0].exam_score;
            }

            PaperDetailUserScore = $"{userScore}";
            PaperDetailTotalScore = $"{totaolScore}";
        } 
        
        #endregion

    }
}
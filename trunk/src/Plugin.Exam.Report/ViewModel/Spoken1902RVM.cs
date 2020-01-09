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
    public class Spoken1902RVM : QsBaseRVM
    {
        #region << 属性 >>

        private StackPanel _qsItemContent;
        private StackPanel _qsItemContent1;

        private string sourceAudio = "";
        private string sourceAudio1= "";

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
        private string _TitleTwo1;

        /// <summary>
        /// 二级 题目 标题
        /// </summary>
        public string TitleTwo1
        {
            get
            {
                return _TitleTwo1;
            }
            set
            {
                _TitleTwo1 = value;
                RaisePropertyChanged("TitleTwo1");
            }
        }

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
                    BindSubItemChoice();
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

        private Visibility _ShowPlayBtn1;
        /// <summary>
        /// 音频 播放按钮
        /// </summary>
        public Visibility ShowPlayBtn1
        {
            get
            {
                return _ShowPlayBtn1;
            }
            set
            {
                _ShowPlayBtn1 = value;
                RaisePropertyChanged("ShowPlayBtn1");
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

                               if (!string.IsNullOrEmpty(sourceAudio))
                               {
                                   PlayAudio(sourceAudio);
                                   PlayIcon = "3-1";
                               }

                           }));
            }
        }

        private RelayCommand _PlayQsAudio1;//播放 音频

        public RelayCommand PlayQsAudio1
        {
            get
            {
                return _PlayQsAudio1 ?? (_PlayQsAudio1 = new RelayCommand(
                           () =>
                           {
                               if (GlobalUser.WavePlayer?.PlaybackState == PlaybackState.Playing)
                               {
                                   GlobalUser.WavePlayer?.Stop();
                               }

                               if (!string.IsNullOrEmpty(sourceAudio1))
                               {
                                   PlayAudio(sourceAudio1);
                                   PlayIcon = "3-2";
                               }

                           }));
            }
        }

        #endregion


        public Spoken1902RVM(StackPanel qsItemContent, StackPanel qsItemContent1, Paper_DetailItem paperDetail, Exam_Attend_Result examResult)
        {
            _qsItemContent = qsItemContent;
            _qsItemContent1 = qsItemContent1;
            PaperDetail = paperDetail;
            ExamResult = examResult;

            QsTitle = PaperDetail.qs_title;
            QsTitleContent = PaperDetail.qs_content;
            QsTitleAudio = PaperDetail.source_content;

            SelectedChoiceExamShow = 0;

            //绑定分数
            BindUserTotalScore();
            //绑定选项内容
            BindSubItemChoice();
        }

        #region << 自定义方法 >>

        /// <summary>
        /// 绑定 选择题 内容
        /// </summary>
        private void BindSubItemChoice()
        {
            CleanUp();

            var paperInfos = PaperDetail.info[0];
            var paperInfos1 = PaperDetail.info[1];

            TitleTwo = paperInfos.info_content;
            TitleTwo1 = paperInfos1.info_content;

            if (!string.IsNullOrEmpty(paperInfos.info_content_img))
            {
                string url = WebApiProxy.GetRedirectUrl($"{WebApiProxy.MEDIAURL}{paperInfos.info_content_img}");
                QsInfoImage = new BitmapImage(new Uri(url));
            }

            sourceAudio = paperInfos.info_content_source_content;
            sourceAudio1 = paperInfos1.info_content_source_content;
            ShowPlayBtn = Visibility.Visible;
            ShowPlayBtn1 = Visibility.Visible;

            _qsItemContent.Children.Clear();

            for (int i = 0; i < paperInfos.items.Count; i++)
            {
                var detailItem = paperInfos.items[i];
                var itemAnswerResult = ExamResult.result_items.Where(w => w.item_id == detailItem.item_id).ToList()[0];

                var itemView = new SubTextChoice1RV();
                itemView.DataContext =
                    new SubTextChoice1RVM(detailItem, (2 * SelectedChoiceExamShow + i), itemAnswerResult, false);
                _qsItemContent.Children.Add(itemView);
            }

            _qsItemContent1.Children.Clear();
            for (int i = 0; i < paperInfos1.items.Count; i++)
            {
                var detailItem = paperInfos1.items[i];
                var itemAnswerResult = ExamResult.result_items.Where(w => w.item_id == detailItem.item_id).ToList()[0];

                var itemView = new SubTextChoice1RV();
                itemView.DataContext =
                    new SubTextChoice1RVM(detailItem, (2 * SelectedChoiceExamShow + i + paperInfos1.items.Count), itemAnswerResult, false);
                _qsItemContent1.Children.Add(itemView);
            }
        }

        /// <summary>
        /// 绑定分数
        /// </summary>
        private void BindUserTotalScore()
        {
            float totaolScore = 0;
            float? userScore = 0;

            var myAllItemId = string.Join(" ", (ExamResult.result_items.Select(s => s.item_id).ToArray()));
            for (int j = 0; j < PaperDetail.info.Count; j++)
            {
                for (int i = 0; i < PaperDetail.info[j].items.Count; i++)
                {
                    totaolScore = totaolScore + PaperDetail.info[j].items[i].item_score;

                    userScore = userScore + ExamResult.result_items
                                    .Where(w => w.item_id == PaperDetail.info[j].items[i].item_id).ToList()[0]
                                    .exam_score;
                }

            }

            PaperDetailUserScore = $"{userScore}";
            PaperDetailTotalScore = $"{totaolScore}";
        }

        #endregion

    }
}
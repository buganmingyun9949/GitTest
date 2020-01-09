using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using NAudio.Wave;
using Plugin.Exam.Report.View;
using Plugin.Exam.Report.View.SubItem;
using Plugin.Exam.Report.ViewModel.SubItem;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Paper;

namespace Plugin.Exam.Report.ViewModel
{
    public class DiaChoiceRVM : QsBaseRVM
    {
        #region << 属性 >>

        private const string MediaPic = ".JPG .GIF .PNG .BMP .JPEG";
        private DiaChoiceRV _DiaChoiceRV;
        private StackPanel _qsItemContent;
        private ListBox _qsTabTitle;

        private string sourceAudio = "";

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

        private object _QsInfoImage;
        /// <summary>
        /// 图片
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
                                   PlayIcon = "2-1";
                               }

                           }));
            }
        }

        #endregion

        public DiaChoiceRVM(DiaChoiceRV uc, Paper_DetailItem paperDetail, Exam_Attend_Result examResult)
        {
            _DiaChoiceRV = uc;
            _qsItemContent = uc.QsItemContent;
            _qsTabTitle = uc.QsTabTitle;
            PaperDetail = paperDetail;
            ExamResult = examResult;

            QsTitle = PaperDetail.qs_title;
            QsTitleContent = PaperDetail.qs_content;
            QsTitleAudio = PaperDetail.source_content;

            SelectedChoiceExamShow = 0;

            //绑定分数
            BindUserTotalScore();
            //绑定选项Tab按钮
            BindTabTitle();
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

            var paperInfos = PaperDetail.info[SelectedChoiceExamShow];

            TitleTwo = paperInfos.info_content;
            sourceAudio = paperInfos.info_content_source_content;
            ShowPlayBtn = Visibility.Visible; 
             
            if (!string.IsNullOrEmpty(paperInfos.info_content_img) &&
                MediaPic.Contains(paperInfos.info_content_img.ToUpper().Split('.').LastOrDefault()))
            {
                string url = WebApiProxy.GetRedirectUrl($"{WebApiProxy.MEDIAURL}{paperInfos.info_content_img}");
                QsInfoImage = new BitmapImage(new Uri(url));
            }
            else
            {
                QsInfoImage = null;
            }

            _qsItemContent.Children.Clear();

            for (int i = 0; i < paperInfos.items.Count; i++)
            {
                var detailItem = paperInfos.items[i];
                var itemAnswerResult = ExamResult.result_items.Where(w => w.item_id == detailItem.item_id).ToList()[0];

                var itemView = new SubTextChoiceRV();
                itemView.DataContext =
                    new SubTextChoiceRVM(detailItem, (2 * SelectedChoiceExamShow + i + 5), itemAnswerResult, false);
                _qsItemContent.Children.Add(itemView);
            }
        }

        /// <summary>
        /// 绑定选项Tab按钮
        /// </summary>
        private void BindTabTitle()
        {
            List<ListViewItem> lsTabs = new List<ListViewItem>();

            for (int i = 0; i < PaperDetail.info.Count; i++)
            {
                ListViewItem item = new ListViewItem();

                item.Content = $"第{i + 1}部分";
                item.Style = (Style)_DiaChoiceRV.FindResource("ExamResultCenterItemToolListBoxItem");

                if (i == 0)
                {
                    item.Style = (Style)_DiaChoiceRV.FindResource("ExamResultLeftItemToolListBoxItem");
                }

                if (i == (PaperDetail.info.Count - 1))
                {
                    item.Style = (Style)_DiaChoiceRV.FindResource("ExamResultRightItemToolListBoxItem");
                }

                lsTabs.Add(item);
            }
            _qsTabTitle.Items.Clear(); 
            _qsTabTitle.ItemsSource = null;
            _qsTabTitle.ItemsSource = lsTabs;
        }

        /// <summary>
        /// 绑定分数
        /// </summary>
        private void BindUserTotalScore()
        {
            float totaolScore = 0;
            float? userScore = 0;

            var myAllItemId = string.Join(" ", (ExamResult.result_items.Select(s => s.item_id).ToArray()));
            for (int k = 0; k < PaperDetail.info.Count; k++)
            {
                for (int i = 0; i < PaperDetail.info[k].items.Count; i++)
                {
                    totaolScore = totaolScore + PaperDetail.info[k].items[i].item_score;

                    userScore = userScore + ExamResult.result_items
                                    .Where(w => w.item_id == PaperDetail.info[k].items[i].item_id).ToList()[0].exam_score;
                }
            }

            PaperDetailUserScore = $"{userScore}";
            PaperDetailTotalScore = $"{totaolScore}";
        }

        #endregion

    }
}
using System.Linq;
using System.Windows.Controls;
using Plugin.Exam.Report.View.SubItem;
using Plugin.Exam.Report.ViewModel.SubItem;
using ST.Common.ToolsHelper;
using ST.Models.Paper;

namespace Plugin.Exam.Report.ViewModel
{
    public class SingleChoiceRVM : QsBaseRVM
    {
        #region << 属性 >>

        private StackPanel _qsItemContent;

        private int _SelectedChoiceExamShow;

        private const string MediaPic = ".JPG .GIF .PNG .BMP .JPEG";

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

        #endregion


        public SingleChoiceRVM(StackPanel qsItemContent, Paper_DetailItem paperDetail, Exam_Attend_Result examResult)
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
            BindSubItemChoice();
        }

        #region << 自定义方法 >>

        /// <summary>
        /// 绑定 选择题 内容
        /// </summary>
        private void BindSubItemChoice()
        {
            Paper_ItemsItem detailItem = new Paper_ItemsItem();

            if (PaperDetail.info.Count > 1)
            {
                detailItem = PaperDetail.info[SelectedChoiceExamShow].items[0];
            }
            else
            {
                detailItem = PaperDetail.info[0].items[SelectedChoiceExamShow];
            }
            var itemAnswerResult = ExamResult.result_items.Where(w => w.item_id == detailItem.item_id).ToList()[0];

            _qsItemContent.Children.Clear();
            if (MediaPic.Contains(detailItem.answers[0].answer_content.ToUpper()) &&
                MediaPic.Contains(detailItem.answers[1].answer_content.ToUpper()) &&
                MediaPic.Contains(detailItem.answers[2].answer_content.ToUpper()))
            {
                var itemView = new SubImageChoiceRV();
                itemView.DataContext = new SubImageChoiceRVM(detailItem, SelectedChoiceExamShow, itemAnswerResult);
                _qsItemContent.Children.Add(itemView);
            }
            else
            {
                var itemView = new SubTextChoiceRV();
                itemView.DataContext = new SubTextChoiceRVM(detailItem, SelectedChoiceExamShow, itemAnswerResult);
                _qsItemContent.Children.Add(itemView);
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
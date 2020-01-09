using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Framework.Logging;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using NAudio.Wave;
using Plugin.Exam.Qs.Common;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using ST.Models.Paper;

namespace Plugin.Exam.Qs.ViewModel.SubItem
{
    /// <summary>
    /// 听小对话 回答问题
    /// </summary>
    public class SubImageChoiceViewModel : QsBaseViewModel
    {
        public new QuestionType QsType = QuestionType.HearingSingleChoice;
        public const string ViewName = "SubTextChoiceViewModel";
        private Paper_ItemsItem _paperSubItemDetail;
        private int _subIndex = 0;
        private int _showIndexNum = 6;

        public SubImageChoiceViewModel()
        {
        }

        public SubImageChoiceViewModel(Paper_ItemsItem paperSubItemDetail, int subIndex, int showIndexNum) : this()
        {
            _paperSubItemDetail = paperSubItemDetail;
            _subIndex = subIndex;
            _showIndexNum = showIndexNum;

            BindQsSubItemInfo();
        }

        #region << 属性 字段 >>
        
        private string qsItemContent;

        /// <summary>
        /// 选择题内容
        /// </summary>
        public string QsItemContent
        {
            get
            {
                return qsItemContent;
            }
            set
            {
                qsItemContent = value;
                RaisePropertyChanged("QsItemContent");
            }
        }


        private object qsItemOptionA;

        /// <summary>
        /// 选择题 选项A
        /// </summary>
        public object QsItemOptionA
        {
            get
            {
                return qsItemOptionA;
            }
            set
            {
                qsItemOptionA = value;
                RaisePropertyChanged("QsItemOptionA");
            }
        }

        private object qsItemOptionB;

        /// <summary>
        /// 选择题 选项B
        /// </summary>
        public object QsItemOptionB
        {
            get
            {
                return qsItemOptionB;
            }
            set
            {
                qsItemOptionB = value;
                RaisePropertyChanged("QsItemOptionB");
            }
        }

        private object qsItemOptionC;

        /// <summary>
        /// 选择题 选项C
        /// </summary>
        public object QsItemOptionC
        {
            get
            {
                return qsItemOptionC;
            }
            set
            {
                qsItemOptionC = value;
                RaisePropertyChanged("QsItemOptionC");
            }
        }


        private string _AnswerOrder = "0";

        private string currentOption;
        public string CurrentOption
        {
            //get
            //{
            //    return currentOption;
            //}
            set
            {
                if (value != null) //要判断一下是否为 null
                {
                    if (value.Contains("1-"))
                    {
                        _AnswerOrder = value.Replace("1-", "");
                    }
                    currentOption = value;
                    RaisePropertyChanged("CurrentOption");
                }
            }
        }

        #endregion

        #region << 按钮方法 >>

        private RelayCommand<string> _CloseExamCommand;//录音 正常,打开 考试

        public RelayCommand<string> CloseExamCommand
        {
            get { return _CloseExamCommand ?? (_CloseExamCommand = new RelayCommand<string>(s =>
            {
                
            })); }
        }


        #endregion

        #region << 自定义方法 >>

        /// <summary>
        /// 加载题目
        /// </summary>
        /// <param name="item_id"></param>
        private void BindQsSubItemInfo()
        {
            QsItemContent = $"{_showIndexNum}、{_paperSubItemDetail.item_content}";
            QsItemOptionA = new BitmapImage(new Uri($"{WebApiProxy.MEDIAURL}{_paperSubItemDetail.answers[0].source_content}"));
            QsItemOptionB = new BitmapImage(new Uri($"{WebApiProxy.MEDIAURL}{_paperSubItemDetail.answers[1].source_content}"));
            QsItemOptionC = new BitmapImage(new Uri($"{WebApiProxy.MEDIAURL}{_paperSubItemDetail.answers[2].source_content}"));
            CurrentOption = "0";
        }


        #endregion

        #region << 考试 >>

        public void UpdateSubChoiceAnswer(string qsId,string qsType)
        {
            float myScore = 0f;
            string myAnswer = "";

            //评分
            for (int i = 0; i < _paperSubItemDetail.answers.Count; i++)
            {
                if ((i + 1) == Int32.Parse(_AnswerOrder))
                {
                    if (_paperSubItemDetail.answers[i].answer_is_right == 1)
                        myScore = _paperSubItemDetail.item_score;

                    myAnswer = _paperSubItemDetail.answers[i].source_content;
                    break;
                }
            }

            // 异步请求，防止界面假死
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                SubmitRecordModel model = new SubmitRecordModel
                {
                    exam_attend_id = GlobalUser.AttendPaperItemId,
                    item_id = _paperSubItemDetail.item_id,
                    item_score = _paperSubItemDetail.item_score,
                    exam_score = myScore,
                    user_answer = GetChoiceName(myAnswer),
                    item_answer = _paperSubItemDetail.answers.Where(w => w.answer_is_right == 1).ToList()[0].source_content,
                    item_answer_type = 1,
                    qs_id = qsId,
                    qs_type = qsType,
                    item_no = _paperSubItemDetail.item_no
                };

                var result = WebApiProxy.GetHtmlRespInfo(model, ApiType.SubmitRecord, GlobalUser.USER.Token);

                GlobalUser.DoneItemExam = true;
                GlobalUser.DoneScore = ScoreType.ScoreSuccess; //评分
                if (result?.retCode != 0)
                {
                    GlobalUser.DoneScore = ScoreType.ScoreFailure; //提交评分 失败了
                    Log4NetHelper.Error("UpdateSubChoiceAnswer: Error");
                    Log4NetHelper.Error("上传评分失败 -- result: 提交评分失败");
                }
            }));
        }

        private string GetChoiceName(string answer)
        {
            string name = "";

            var answerItem = _paperSubItemDetail.answers.Where(w => w.source_content == answer).ToList();

            if (answerItem.Count == 0)
            {
                return name;
            }

            if (answerItem[0].answer_sort == 1)
            {
                name = "A";
            }
            else if (answerItem[0].answer_sort == 2)
            {
                name = "B";
            }
            else if (answerItem[0].answer_sort == 3)
            {
                name = "C";
            }
            else if (answerItem[0].answer_sort == 4)
            {
                name = "D";
            }

            return name;
        }

        #endregion


    }
}

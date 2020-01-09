using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
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
    public class SubTextChoice1ViewModel : QsBaseViewModel
    {
        public new QuestionType QsType = QuestionType.HearingSingleChoice;
        public const string ViewName = "SubTextChoiceViewModel";
        private Paper_ItemsItem _paperSubItemDetail;
        private int _subIndex = 0;
        private int _showIndexNum = 6;

        public SubTextChoice1ViewModel()
        {
        }

        public SubTextChoice1ViewModel(Paper_ItemsItem paperSubItemDetail, int subIndex, int showIndexNum) : this()
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


        private string qsItemOptionA;

        /// <summary>
        /// 选择题 选项A
        /// </summary>
        public string QsItemOptionA
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

        private string qsItemOptionB;

        /// <summary>
        /// 选择题 选项B
        /// </summary>
        public string QsItemOptionB
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

        private string qsItemOptionC;

        /// <summary>
        /// 选择题 选项C
        /// </summary>
        public string QsItemOptionC
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

        private string qsItemOptionD;

        /// <summary>
        /// 选择题 选项D
        /// </summary>
        public string QsItemOptionD
        {
            get
            {
                return qsItemOptionD;
            }
            set
            {
                qsItemOptionD = value;
                RaisePropertyChanged("QsItemOptionD");
            }
        }

        private string qsItemOptionE;

        /// <summary>
        /// 选择题 选项E
        /// </summary>
        public string QsItemOptionE
        {
            get
            {
                return qsItemOptionE;
            }
            set
            {
                qsItemOptionE = value;
                RaisePropertyChanged("QsItemOptionE");
            }
        }

        private string qsItemOptionF;

        /// <summary>
        /// 选择题 选项F
        /// </summary>
        public string QsItemOptionF
        {
            get
            {
                return qsItemOptionF;
            }
            set
            {
                qsItemOptionF = value;
                RaisePropertyChanged("QsItemOptionF");
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
            QsItemContent = $"{_paperSubItemDetail.item_content}";//{_showIndexNum}、
            QsItemOptionA = $" A.{_paperSubItemDetail.answers[0].answer_content}";
            QsItemOptionB = $" B.{_paperSubItemDetail.answers[1].answer_content}";

            if (_paperSubItemDetail.answers.Count > 2)
                QsItemOptionC = $" C.{_paperSubItemDetail.answers[2].answer_content}";
            else
                QsItemOptionC = "";

            if (_paperSubItemDetail.answers.Count > 3)
                QsItemOptionD = $" D.{_paperSubItemDetail.answers[3].answer_content}";
            else
                QsItemOptionD = "";

            if (_paperSubItemDetail.answers.Count > 4)
                QsItemOptionE = $" E.{_paperSubItemDetail.answers[4].answer_content}";
            else
                QsItemOptionE = "";

            if (_paperSubItemDetail.answers.Count > 5)
                QsItemOptionF= $" F.{_paperSubItemDetail.answers[5].answer_content}";
            else
                QsItemOptionF = "";



            CurrentOption = "0";
        }


        #endregion

        #region << 考试 >>
         
        public void UpdateSubChoiceAnswer(string qsId,string qsType)
        {
            float myScore = 0f;
            int myAnswer = -1;

            //评分
            for (int i = 0; i < _paperSubItemDetail.answers.Count; i++)
            {
                if ((i + 1) == Int32.Parse(_AnswerOrder))
                {
                    if (_paperSubItemDetail.answers[i].answer_is_right == 1)
                        myScore = _paperSubItemDetail.item_score;

                    myAnswer = i;
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
                    item_answer = _paperSubItemDetail.answers.Where(w=>w.answer_is_right == 1).ToList()[0].answer_content,
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
                    Log4NetHelper.Error("SubTextChoice1ViewModel: Error");
                    Log4NetHelper.Error("上传评分失败 -- result: 提交评分失败");
                }
            }));
        }

        private string GetChoiceName(int answer)
        {
            string name = "";

            switch (answer)
            {
                case 0:
                    name = "A";
                    break;
                case 1:
                    name = "B";
                    break;
                case 2:
                    name = "C";
                    break;
                case 3:
                    name = "D";
                    break;
                case 4:
                    name = "E";
                    break;
                case 5:
                    name = "F";
                    break;
            }

            return name;
            //var answerItem = _paperSubItemDetail.answers[answer];

            //if (answerItem == null)
            //{
            //    return name;
            //}

            //if (answerItem.answer_sort == 1)
            //{
            //    name = "A";
            //}
            //else if (answerItem.answer_sort == 2)
            //{
            //    name = "B";
            //}
            //else if (answerItem.answer_sort == 3)
            //{
            //    name = "C";
            //}
            //else if (answerItem.answer_sort == 4)
            //{
            //    name = "D";
            //}
            //else if (answerItem.answer_sort == 5)
            //{
            //    name = "E";
            //}
            //else if (answerItem.answer_sort == 6)
            //{
            //    name = "F";
            //}

            //return name;
        }

        #endregion


    }
}

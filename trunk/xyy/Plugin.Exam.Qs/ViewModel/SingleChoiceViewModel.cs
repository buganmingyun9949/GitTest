using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Framework.Logging;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using NAudio.Wave;
using Plugin.Exam.Qs.Common;
using Plugin.Exam.Qs.View;
using Plugin.Exam.Qs.View.SubItem;
using Plugin.Exam.Qs.ViewModel.SubItem;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using ST.Models.Paper;

namespace Plugin.Exam.Qs.ViewModel
{
    /// <summary>
    /// 听小对话 回答问题
    /// </summary>
    public class SingleChoiceViewModel : QsBaseViewModel
    {
        public new QuestionType QsType = QuestionType.HearingSingleChoice;
        public const string ViewName = "SingleChoiceViewModel";
        private const string MediaPic = ".JPG .GIF .PNG .BMP .JPEG";
        private int _itemIndex = 0;
        private int _infoIndex = 0;
        private int _audioPlayTimes=0;
        private int _prepareTime = 0;
        private int _answerTime = 0;
        private int _item_repet_times = 1;
        private int _showIndexNum = 0;
        private StackPanel _subChoiceQsContent;

        public SingleChoiceViewModel(int qsIndex, int infoIndex, int itemIndex)
        {
            QsIndex = qsIndex;
            _infoIndex = infoIndex;
            _itemIndex = itemIndex;
            _dTimer = new DispatcherTimer();
            _dTimer.Interval = TimeSpan.FromSeconds(1);
            _dTimer.Tick += DTimerOnTick;
        }

        public SingleChoiceViewModel(StackPanel subChoiceQsContent, Paper_DetailItem paperDetail, Button btn, int qsIndex, int infoIndex, int itemIndex, string item_id = null) : this(qsIndex, infoIndex, itemIndex)
        {
            _subChoiceQsContent = subChoiceQsContent;

            _BtnSkipNext = btn;
            _BtnSkipNext.IsEnabled = false;
            _BtnSkipNext.Click += BtnSkipNext_Click;

            PaperDetailItem = paperDetail;
            QsID = paperDetail.qs_id;
            QsTitle = paperDetail.qs_title;
            QsTitleContent = paperDetail.qs_content;
            QsTitleAudio = paperDetail.source_content;
            PaperItems = paperDetail.info.OrderBy(o => o.info_sort).ToList();

            BindQsItemInfo(item_id);
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
        private void BindQsItemInfo(string item_id = null)
        {
            if (_itemIndex >= PaperItems[_infoIndex].items.Count)
            {
                _itemIndex = 0;
                _infoIndex++;
            }

            if (_infoIndex >= PaperItems.Count)
            {
                //进入 下一道大题
                NextQsView();

            }
            else
            {
                _subChoiceQsContent.Children.Clear();

               Paper_ItemsItem itemInfo;

                itemInfo = PaperItems[_infoIndex].items[_itemIndex];
                if ((!string.IsNullOrEmpty(itemInfo.answers[0].source_content) &&
                     !string.IsNullOrEmpty(itemInfo.answers[0].source_content) &&
                     !string.IsNullOrEmpty(itemInfo.answers[1].source_content)) &&
                    MediaPic.Contains(itemInfo.answers[2].source_content.ToUpper().Split('.')
                        .LastOrDefault()) &&
                    MediaPic.Contains(itemInfo.answers[1].source_content.ToUpper().Split('.')
                        .LastOrDefault()) &&
                    MediaPic.Contains(itemInfo.answers[2].source_content.ToUpper().Split('.')
                        .LastOrDefault()))
                {
                    var subChoiceView = new SubImageChoiceView();
                    subChoiceView.DataContext =
                        new SubImageChoiceViewModel(itemInfo, 0, (_infoIndex + _itemIndex + 1));

                    _subChoiceQsContent.Children.Add(subChoiceView);
                }
                else
                {
                    var subChoiceView = new SubTextChoiceView();
                    subChoiceView.DataContext =
                        new SubTextChoiceViewModel(itemInfo, 0, (_infoIndex + _itemIndex + 1));

                    _subChoiceQsContent.Children.Add(subChoiceView);
                }


                //QsItemContent = $"{_itemIndex + 1}、{itemInfo.item_content}";
                //QsItemOptionA = $" A.{itemInfo.answers[0].answer_content}";
                //QsItemOptionB = $" B.{itemInfo.answers[1].answer_content}";
                //QsItemOptionC = $" C.{itemInfo.answers[2].answer_content}";
                //CurrentOption = _AnswerOrder = "0";

                //初始化
                PlayTime = 0;
                TotalTime = 0;
                _audioPlayTimes = 0;
                _prepareTime = 0;
                _answerTime = 0;
                _item_repet_times = 1;

                BeginExam(_NextFlowType);
            }
        }

        private void DTimerOnTick(object sender, EventArgs e)
        {
            if (_BtnSkipNext != null&& !_BtnSkipNext.IsEnabled)
                _BtnSkipNext.IsEnabled = (PlayTime >= 1);

            if (PlayTime >= TotalTime)
            {
                PlayTime = TotalTime;
                CleanUp();
                BeginExam(_NextFlowType);
            }
            else
            {
                PlayTime++;

                SendProgress();
            }
        }

        #endregion

        #region << 考试 >>

        public override void BeginExam(ExamFlowType nextFlowType)
        {
            Paper_ItemsItem itemInfo = PaperItems[_infoIndex].items[_itemIndex];


            switch (nextFlowType)
            {
                case ExamFlowType.TitleAudio:
                    //1.播放大题题干
                    PromptCommandText = "听指令";
                    _NextFlowType = ExamFlowType.PromptAudio;
                    PlayAudio(QsTitleAudio);
                    break;
                case ExamFlowType.PromptAudio:
                    //2.播放大题题干 提示音频,
                    PromptCommandText = "听指令";
                    if (string.IsNullOrEmpty(PaperItems[_infoIndex].source_content))
                    {
                        _NextFlowType = ExamFlowType.PrepareTime;
                        BeginExam(_NextFlowType);
                        break;
                    }

                    _NextFlowType = ExamFlowType.PromptAudio;
                    PlayAudio(PaperItems[_infoIndex].source_content);
                    break;
                case ExamFlowType.PrepareTime:
                    //3.阅读题目
                    PromptCommandText = "请看题";
                    _prepareTime = TotalTime = itemInfo.item_prepare_second;
                    PlayTime = 0;
                    SendProgress();
                    _NextFlowType = ExamFlowType.QuestionAudio;
                    _dTimer.Start();
                    break;
                case ExamFlowType.QuestionAudio:
                    //3.播放 题目音频
                    PromptCommandText = "请听录音";
                    if (string.IsNullOrEmpty(itemInfo.source_content))
                    {
                        _NextFlowType = ExamFlowType.AnswerTime;
                        BeginExam(_NextFlowType);
                        break;
                    }

                    PlayAudio(itemInfo.source_content);
                    if (itemInfo.item_repet_times > 1 &&
                        itemInfo.item_repet_times > _item_repet_times)
                    {
                        _item_repet_times++;
                        _NextFlowType = ExamFlowType.QuestionAudio;
                    }
                    else
                        _NextFlowType = ExamFlowType.AnswerTime;

                    break;
                case ExamFlowType.AnswerTime:
                    //3.阅读题目
                    PromptCommandText = "开始答题";
                    _answerTime = itemInfo.item_answer_second;
                    TotalTime = itemInfo.item_answer_second;
                    PlayTime = 0;
                    _NextFlowType = ExamFlowType.NextQs;
                    _dTimer.Start();
                    break;
                case ExamFlowType.NextQs:
                    //记录答案
                    UpdateAnswer();
                    //3.下一题
                    _itemIndex++;
                    _NextFlowType = ExamFlowType.PrepareTime;
                    BindQsItemInfo();
                    break;
            }

        }

        private void UpdateAnswer()
        {
            if (_subChoiceQsContent.Children.Count > 0)
            {
                for (int i = 0; i < _subChoiceQsContent.Children.Count; i++)
                {
                    if (_subChoiceQsContent.Children[i].GetType().Name == "SubImageChoiceView")
                    {
                        var subView = _subChoiceQsContent.Children[i] as SubImageChoiceView;

                        var subVM = subView.DataContext as SubImageChoiceViewModel;

                        subVM.UpdateSubChoiceAnswer(PaperDetailItem.qs_id, PaperDetailItem.qs_type);

                    }
                    else
                    {
                        var subView = _subChoiceQsContent.Children[i] as SubTextChoiceView;

                        var subVM = subView.DataContext as SubTextChoiceViewModel;

                        subVM.UpdateSubChoiceAnswer(PaperDetailItem.qs_id, PaperDetailItem.qs_type);

                    }
                }
            }


            //var paperItemsItem = PaperItems[0].items[_itemIndex];
            //float myScore = 0f;
            //string myAnswer = "";

            ////评分
            //for (int i = 0; i < paperItemsItem.answers.Count; i++)
            //{
            //    if ((i + 1) == Int32.Parse(_AnswerOrder))
            //    {
            //        if (paperItemsItem.answers[i].answer_is_right == 1)
            //            myScore = paperItemsItem.item_score;

            //        myAnswer = paperItemsItem.answers[i].answer_content;
            //        break;
            //    }
            //}

            //// 异步请求，防止界面假死
            //Application.Current.Dispatcher.Invoke(new Action(() =>
            //{
            //    SubmitRecordModel model = new SubmitRecordModel
            //    {
            //        exam_attend_id = GlobalUser.AttendPaperItemId,
            //        item_id = paperItemsItem.item_id,
            //        item_score = paperItemsItem.item_score,
            //        exam_score = myScore,
            //        user_answer = myAnswer,
            //        item_answer = paperItemsItem.answers.Where(w => w.answer_is_right == 1).ToList()[0].answer_content,
            //        item_answer_type = 1,
            //        qs_id = PaperDetailItem.qs_id,
            //        qs_type = PaperDetailItem.qs_type,
            //        item_no = paperItemsItem.item_no
            //    };

            //    var result = WebApiProxy.GetHtmlRespInfo(model, ApiType.SubmitRecord, GlobalUser.USER.Token);

            //    GlobalUser.DoneItemExam = true;
            //    GlobalUser.DoneScore = ScoreType.ScoreSuccess; //评分
            //    if (result?.retCode != 0)
            //    {
            //        GlobalUser.DoneScore = ScoreType.ScoreFailure; //提交评分 失败了
            //    }
            //}));
        }

        #endregion


    }
}

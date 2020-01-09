
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using ST.Common.WebApi;
using ST.Models.Api;
using ST.Models.Paper;
using Personal_App.Common;
using Personal_App.Domain;
using Personal_App.Domain.Exam;
using Plugin.Exam.Qs.Common;
using Plugin.Exam.Qs.View;
using Plugin.Exam.Qs.ViewModel;
using ST.Common;
using ST.Common.ToolsHelper;
using VoiceRecorder.Audio;

namespace Personal_App.ViewModel.Exam
{
    public class ExamQsMainWinVM : ExamCommonVM
    {
        private WrapPanel _mainQsContent;
        private static Button _BtnSkipNext;
        public const string ViewName = "ExamQsMainWinView";

        public ExamQsMainWinVM()
        {
            Messenger.Default.Unregister<ExamNavigateMessage>(this);
            Messenger.Default.Unregister<ExamNavigateMessage>(this, "ExamQsMainWinView");
            Messenger.Default.Unregister<ExamNavigateMessage>(this, "ExamNextQsView");
            Messenger.Default.Unregister<ExamNavigateMessage>(this, "ExamQsMainWinCmdView");

            Messenger.Default.Register<ExamQsNavigateMessage>(this, "ExamQsMainWinView",
                (message) => OnExamQsNavigate(message));
            Messenger.Default.Register<ExamNextQsNavigateMessage>(this, "ExamNextQsView",
                (message) => OnExamNextQsNavigate(message));
            Messenger.Default.Register<ExamQsCmdMessage>(this, "ExamQsMainWinCmdView",
                (message) => OnExamQsCmd(message));
            Messenger.Default.Register<ExamQsSampleAggregatorMessage>(this, "ExamQsSampleAggregatorView",
                (message) => OnExamQsSampleAggregator(message));
            Messenger.Default.Register<ExamScoreNavigateMessage>(this, "ExamScoreNavi",
                (message) => OnExamScoreNavigate(message));
        }

        public ExamQsMainWinVM(WrapPanel mainQsContent, Button btnSkipNext) : this()
        {
            _mainQsContent = mainQsContent;
            _BtnSkipNext = btnSkipNext;

            BindQs();
        }

        private void OnExamScoreNavigate(ExamScoreNavigateMessage message)
        {
            ExamSubItemScoreVM examScore = new ExamSubItemScoreVM(message);
            examScore.EngineScore();
            examScore.AutoScore();
        }

        private void OnExamQsNavigate(ExamQsNavigateMessage message)
        {
            message.viewElement.DataContext = message.ViewModel;

            //if (_mainQsContent.Children.Count > 0)
            //{
            //    _mainQsContent.Children.RemoveAt(0);
            //}
            //else
            _mainQsContent.Children.Clear();
            _mainQsContent.Children.Add(message.viewElement);
        }

        private void OnExamNextQsNavigate(ExamNextQsNavigateMessage message)
        {
            var paperDetails = GlobalUser.SelectPaperInfo.paper_detail.OrderBy(o => o.qs_sort).ToList();

            if (message.NextQsIndex >= paperDetails.Count)
            {
                //试卷 题目已经做完
                //进入完成界面
                //todo
                CleanUp();
                //完成考试
                BindExamCompleteUC();
            }
            else
            {
                NextQsView(message.NextQsIndex, message.InfoIndex, message.ItemIndex, paperDetails[message.NextQsIndex]);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void BindQs()
        {
            var paperDetails = GlobalUser.SelectPaperInfo.paper_detail.OrderBy(o => o.qs_sort).ToList();

            int itemIndex = 0;
            int infoIndex = 0;
            int qsIndex = 0;

            var paperDetail = paperDetails[qsIndex];

            if (!string.IsNullOrEmpty(GlobalUser.SelectExamAttendResult))
            {
                List<Exam_Attend_Result_Item> myAnswerResults =
                    JsonHelper.FromJson<List<Exam_Attend_Result_Item>>(GlobalUser.SelectExamAttendResult);

                var myAllItemId = string.Join(" ", myAnswerResults.Select(s => s.item_id).ToArray());

                for (int i = 0; i < paperDetails.Count; i++)
                {
                    if (myAllItemId == "")
                    {
                        continue;
                    }
                    paperDetail = paperDetails[i];

                    for (int j = 0; j < paperDetails[i].info.Count; j++)
                    {
                        for (int k = 0; k < paperDetails[i].info[j].items.Count; k++)
                        {
                            if (myAllItemId.Contains(paperDetails[i].info[j].items[k].item_id))
                            {
                                myAllItemId = myAllItemId.Replace(paperDetails[i].info[j].items[k].item_id, "");

                                itemIndex = k;
                                infoIndex = j;

                                if (paperDetails[i].info.Count <= (infoIndex+1))
                                {
                                    if (paperDetails[i].info[j].items.Count<=(itemIndex+1))
                                    {
                                        qsIndex = i + 1;

                                        if (qsIndex >= paperDetails.Count)
                                        {
                                            break;
                                        }
                                        paperDetail = paperDetails[qsIndex];
                                        infoIndex = 0;
                                        itemIndex = 0;
                                    }
                                    else
                                    {
                                        itemIndex = k + 1;
                                    }
                                }
                                else
                                {
                                    itemIndex = k + 1;

                                    if (paperDetails[i].info[infoIndex].items.Count <= itemIndex)
                                    {
                                        infoIndex = j + 1;
                                        itemIndex = 0;
                                    }
                                    //if (infoIndex == paperDetails[i].info.Count)
                                    //{
                                    //    qsIndex = i + 1;
                                    //    paperDetail = paperDetails[qsIndex];
                                    //    itemIndex = 0;
                                    //}
                                }

                                if (string.IsNullOrWhiteSpace(myAllItemId))
                                {
                                    break;
                                }
                            }
                            else
                            {
                                myAllItemId = "";
                                continue;
                            }
                        }
                    }
                }
            }

            //显示 题目 信息
            NextQsView(qsIndex, infoIndex, itemIndex, paperDetail);
        }

        /// <summary>
        /// 显示 题目 信息
        /// </summary>
        /// <param name="itemIndex"></param>
        /// <param name="qsIndex"></param>
        /// <param name="paperDetail"></param>
        private static void NextQsView(int qsIndex, int infoIndex, int itemIndex, Paper_DetailItem paperDetail)
        {
            QuestionType qsType = (QuestionType)Enum.ToObject(typeof(QuestionType),
                int.Parse(paperDetail.qs_type));

            switch (qsType)
            {
                case QuestionType.HearingSingleChoice:
                    var view0 = new SingleChoiceView();
                    //听力选答 小对话
                    Messenger.Default.Send(new ExamQsNavigateMessage(SingleChoiceViewModel.ViewName,
                            view0,
                            new SingleChoiceViewModel(view0.SubChoiceQsContent, paperDetail, _BtnSkipNext, qsIndex, infoIndex, itemIndex)),
                        "ExamQsMainWinView");
                    break;
                case QuestionType.HearingDiaChoice:
                    var view1 = new DiaChoiceView();
                    //听力选答 长对话
                    Messenger.Default.Send(new ExamQsNavigateMessage(DiaChoiceViewModel.ViewName,
                            view1,
                            new DiaChoiceViewModel(view1.SubChoiceQsContent, paperDetail, _BtnSkipNext, qsIndex, infoIndex)),
                        "ExamQsMainWinView");
                    break;
                case QuestionType.HearingEssayChoice:
                    var view2 = new EssayChoiceView();
                    //听力选答 短文
                    Messenger.Default.Send(new ExamQsNavigateMessage(EssayChoiceViewModel.ViewName,
                            view2,
                            new EssayChoiceViewModel(view2.SubChoiceQsContent, paperDetail, _BtnSkipNext, qsIndex, infoIndex)),
                        "ExamQsMainWinView");
                    break;
                case QuestionType.RepeatImitate:
                    //ReadSentenceView
                    if (paperDetail.qs_true_type == "32")
                    {
                        //朗读句子
                        Messenger.Default.Send(new ExamQsNavigateMessage(ReadSentenceViewModel.ViewName,
                                new ReadSentenceView(),
                                new ReadSentenceViewModel(paperDetail, _BtnSkipNext, qsIndex, infoIndex)),
                            "ExamQsMainWinView");
                    }
                    else
                    {
                        //跟读模仿(短文 句子跟读)
                        //朗读句子
                        Messenger.Default.Send(new ExamQsNavigateMessage(ReadSentenceViewModel.ViewName,
                                new ReadSentenceView(),
                                new ReadSentenceViewModel(paperDetail, _BtnSkipNext, qsIndex, infoIndex)),
                            "ExamQsMainWinView");
                    }

                    break;
                case QuestionType.SpokenPred:
                    //朗读短文
                    Messenger.Default.Send(new ExamQsNavigateMessage(SpokenPredViewModel.ViewName, new SpokenPredView(),
                            new SpokenPredViewModel(paperDetail, _BtnSkipNext, qsIndex, infoIndex)),
                        "ExamQsMainWinView");
                    break;
                case QuestionType.SpokenScne:
                case QuestionType.ScneQA_G:
                    //情景问答
                    Messenger.Default.Send(new ExamQsNavigateMessage(SpokenScneViewModel.ViewName, new SpokenScneView(),
                            new SpokenScneViewModel(paperDetail, _BtnSkipNext, qsIndex, itemIndex)),
                        "ExamQsMainWinView");
                    break;
                case QuestionType.SpokenRightAnswer:
                case QuestionType.SpokenRightAnswer2:
                    //听句子 选择说出正确内容
                    Messenger.Default.Send(new ExamQsNavigateMessage(SpokenRightAnswerViewModel.ViewName,
                            new SpokenRightAnswerView(),
                            new SpokenRightAnswerViewModel(paperDetail, _BtnSkipNext, qsIndex, infoIndex)),
                        "ExamQsMainWinView");
                    break;
                case QuestionType.SpokenScnePic:
                case QuestionType.ObtainInfo:
                    //信息获取
                    Messenger.Default.Send(new ExamQsNavigateMessage(ObtainInfoViewModel.ViewName, new ObtainInfoView(),
                            new ObtainInfoViewModel(paperDetail, _BtnSkipNext, qsIndex, infoIndex, itemIndex)),
                        "ExamQsMainWinView");
                    break;
                case QuestionType.ObtainInfoAsk:
                    //信息获取及转述
                    Messenger.Default.Send(new ExamQsNavigateMessage(ObtainInfoAskViewModel.ViewName, new ObtainInfoAskView(),
                            new ObtainInfoAskViewModel(paperDetail, _BtnSkipNext, qsIndex, infoIndex, itemIndex)),
                        "ExamQsMainWinView");
                    break;
                case QuestionType.SpokenOesy:
                    //口头作文  故事复述  话题简述
                    Messenger.Default.Send(new ExamQsNavigateMessage(SpokenOesyViewModel.ViewName, new SpokenOesyView(),
                            new SpokenOesyViewModel(paperDetail, _BtnSkipNext, qsIndex, infoIndex)),
                        "ExamQsMainWinView");
                    break;
            }
        }

        private int GetQsIndex()
        {
            int qsIndex = 0;

            if (GlobalUser.DoneItemExam)
            {



            }

            return qsIndex;
        }

        #region << 窗口 命令 信息 >>


        private string _PromptCommandText;

        /// <summary>
        /// 命令提示 
        /// </summary>
        public string PromptCommandText
        {
            get
            {
                return _PromptCommandText;
            }
            set
            {
                _PromptCommandText = value;
                RaisePropertyChanged("PromptCommandText");
            }
        }

        private int _PlayingTime;

        public int PlayingTime
        {
            get
            {
                return _PlayingTime;
            }
            set
            {
                _PlayingTime = value;
                RaisePropertyChanged("PlayingTime");
            }
        }

        private int _TotalTime;

        public int TotalTime
        {
            get
            {
                return _TotalTime;
            }
            set
            {
                _TotalTime = value;
                RaisePropertyChanged("TotalTime");
            }
        }

        private SampleAggregator _QsSampleAggregator;

        /// <summary>
        /// 波形
        /// </summary>
        public SampleAggregator QsSampleAggregator
        {
            get { return _QsSampleAggregator; }
            set
            {
                _QsSampleAggregator = value;
                RaisePropertyChanged("QsSampleAggregator");
            }
        }

        private void OnExamQsCmd(ExamQsCmdMessage message)
        {
            //Application.Current.Dispatcher.Invoke(new Action(() =>
            //{
            PlayingTime = message.PlayingTime;
            TotalTime = message.TotalTime;
            PromptCommandText = message.PromptCommandText;
            //}));
        }

        private void OnExamQsSampleAggregator(ExamQsSampleAggregatorMessage message)
        {
            //Application.Current.Dispatcher.Invoke(new Action(() =>
            //{
            QsSampleAggregator = message.SampleAggregator;
            //}));
        }


        #endregion
    }
}

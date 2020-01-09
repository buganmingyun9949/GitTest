using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Framework.Logging;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using ST.Models.Paper;
using Personal_App.Common;
using Personal_App.Domain;
using NAudio.Wave;
using Application = System.Windows.Application;
using MaterialDesignThemes.Wpf;
using GalaSoft.MvvmLight;
using Plugin.Exam.Qs.Common;
using Plugin.Exam.Report.View;
using Plugin.Exam.Report.ViewModel;
using ST.Common;

namespace Personal_App.ViewModel.Exam
{
    public class ExamResultVM : ExamCommonVM//, IRequestHandler
    {
        #region << 字段 >>

        public const string ViewName = "ExamResultView";

        private Grid _paperDetailView;
        private Button _btnReturnHome;

        //private List<ExamDetailListItem> ExamChoiceDetailList;

        //private List<ExamDetailListItem> ExamSpeakDetailList;

        //private List<ExamDetailListItem> ExamRecordAndRelateDetailList;

        //private List<ExamDetailListItem> ExamReadDetailList;

        #endregion

        #region << 属性 >>

        public SimulationPaper SPaper { get; set; }
        //public ShowItem[] ShowItems { get; }

        private string _PlayIcon;

        /// <summary>
        /// 播放按钮图标
        /// </summary>
        public string PlayIcon
        {
            get
            {
                return _PlayIcon;
            }
            set
            {
                _PlayIcon = value;
                RaisePropertyChanged("PlayIcon");
            }
        }
        private string _userTotalScoreText;

        /// <summary>
        /// 用户总分文本。
        /// </summary>
        public string UserTotalScoreText
        {
            get
            {
                return _userTotalScoreText;
            }
            set
            {
                _userTotalScoreText = value;
                RaisePropertyChanged("UserTotalScoreText");
            }
        }
        private string _examScoreForeground;

        /// <summary>
        /// 零分文本颜色。
        /// </summary>
        public string ExamScoreForeground
        {
            get
            {
                return _examScoreForeground;
            }
            set
            {
                _examScoreForeground = value;
                RaisePropertyChanged("ExamScoreForeground");
            }
        }
        private string _totalScoreForeground;

        /// <summary>
        /// 用户总分颜色。
        /// </summary>
        public string TotalScoreForeground
        {
            get
            {
                return _totalScoreForeground;
            }
            set
            {
                _totalScoreForeground = value;
                RaisePropertyChanged("TotalScoreForeground");
            }
        }

        private object _QsNaviName;

        /// <summary>
        /// 题目导航
        /// </summary>
        public object QsNaviName
        {
            get
            {
                return _QsNaviName;
            }
            set
            {
                _QsNaviName = value;
                RaisePropertyChanged("QsNaviName");
            }
        }
        private object _TotalStarScore;

        /// <summary>
        /// 总成绩 星星
        /// </summary>
        public object TotalStarScore
        {
            get
            {
                return _TotalStarScore;
            }
            set
            {
                _TotalStarScore = value;
                RaisePropertyChanged("TotalStarScore");
            }
        }

        private Visibility _ShowTotalScore;

        /// <summary>
        /// 总成绩
        /// </summary>
        public Visibility ShowTotalScore
        {
            get
            {
                return _ShowTotalScore;
            }
            set
            {
                _ShowTotalScore = value;
                RaisePropertyChanged("ShowTotalScore");
            }
        }

        private Visibility _ShowDetailScore;
        /// <summary>
        /// 其他题目 显示
        /// </summary>
        public Visibility ShowDetailScore
        {
            get
            {
                return _ShowDetailScore;
            }
            set
            {
                _ShowDetailScore = value;
                RaisePropertyChanged("ShowDetailScore");
            }
        }

        private int _SelectedExamShow;

        public int SelectedExamShow
        {
            get => _SelectedExamShow;
            set
            {
                if (_SelectedExamShow != value)
                {
                    _SelectedExamShow = value;
                    ShowExamItemInfo();
                    RaisePropertyChanged("SelectedExamShow");
                }
            }
        }

        #region << 总成绩 >>


        private string _DefeatPercent;
        /// <summary>
        /// 击败全国***
        /// </summary>
        public string DefeatPercent
        {
            get
            {
                return _DefeatPercent;
            }
            set
            {
                _DefeatPercent = value;
                RaisePropertyChanged("DefeatPercent");
            }
        }

        private string _UserTotalScore;
        /// <summary>
        /// 我的满分
        /// </summary>
        public string UserTotalScore
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_UserTotalScore))
                    return "0";
                return _UserTotalScore;
            }
            set
            {
                _UserTotalScore = value;
                RaisePropertyChanged("UserTotalScore");
            }
        }

        private string _ReadTotalScore;
        /// <summary>
        /// 试卷 朗读短文 满分
        /// </summary>
        public string ReadTotalScore
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_ReadTotalScore))
                    return "10";
                return _ReadTotalScore;
            }
            set
            {
                _ReadTotalScore = value;
                RaisePropertyChanged(ReadTotalScore);
            }
        }
        #endregion

        #endregion

        public ExamResultVM()
        {
            BindPaperInfo();//更新做题信息

            BindTotalInfo();

            //BindQs();
        }

        public ExamResultVM(Grid paperDetailView, Button btnReturnHome) : this()
        {
            _paperDetailView = paperDetailView;

            _btnReturnHome = btnReturnHome;
        }

        #region << 自定义 方法 >>

        private void BindQs()
        {
            var paperDetails = GlobalUser.SelectPaperInfo.paper_detail.OrderBy(o => o.qs_sort).ToList();
            var paperDetail = paperDetails[SelectedExamShow - 1];

            var myAnswerResults =
                JsonHelper.FromJson<List<Exam_Attend_Result_Item>>(GlobalUser.SelectExamAttendResult);
            var examResult = new  Exam_Attend_Result();
            examResult.result_items = myAnswerResults;

            QuestionType qsType = (QuestionType)Enum.ToObject(typeof(QuestionType),
                int.Parse(paperDetail.qs_type));


            UserControl itemView = new UserControl();

            switch (qsType)
            {
                case QuestionType.HearingSingleChoice:
                    //听力选答 小对话
                    SingleChoiceRV view1 = new SingleChoiceRV();
                    itemView = view1;
                    itemView.DataContext = new SingleChoiceRVM(view1.QsItemContent, paperDetail, examResult);
                    break;
                case QuestionType.HearingDiaChoice:
                    //听力选答 长对话
                    DiaChoiceRV view2 = new DiaChoiceRV();
                    itemView = view2;
                    itemView.DataContext = new DiaChoiceRVM(view2.QsItemContent, paperDetail, examResult);
                    break;
                case QuestionType.HearingEssayChoice:
                    //听力选答 短文
                    EssayChoiceRV view3 = new EssayChoiceRV();
                    itemView = view3;
                    itemView.DataContext = new EssayChoiceRVM(view3.QsItemContent, paperDetail, examResult);
                    break;
                case QuestionType.RepeatImitate:
                    //ReadSentenceView
                    if (paperDetail.qs_true_type == "32")
                    {
                        //朗读句子
                        ReadSentenceRV view16 = new ReadSentenceRV();
                        itemView = view16;
                        itemView.DataContext = new ReadSentenceRVM(view16.QsItemContent, paperDetail, examResult);
                    }
                    else
                    {
                        //跟读模仿(短文 句子跟读)
                        //朗读句子
                        ReadSentenceRV view16 = new ReadSentenceRV();
                        itemView = view16;
                        itemView.DataContext = new ReadSentenceRVM(view16.QsItemContent, paperDetail, examResult);
                    }

                    break;
                case QuestionType.SpokenPred:
                    //情景问答
                    SpokenPredRV view5 = new SpokenPredRV();
                    itemView = view5;
                    itemView.DataContext = new SpokenPredRVM(view5.QsItemContent, paperDetail, examResult);
                    break;
                case QuestionType.SpokenScne:
                case QuestionType.ScneQA_G:
                    //情景问答
                    SpokenScneRV view9 = new SpokenScneRV();
                    itemView = view9;
                    itemView.DataContext = new SpokenScneRVM(view9.QsItemContent, paperDetail, examResult);
                    break;
                case QuestionType.SpokenRightAnswer:
                case QuestionType.SpokenRightAnswer2:
                    //听句子 选择说出正确内容
                    SpokenRightAnswerRV view15 = new SpokenRightAnswerRV();
                    itemView = view15;
                    itemView.DataContext = new SpokenRightAnswerRVM(view15.QsItemContent, paperDetail, examResult);
                    break;
                case QuestionType.SpokenScnePic:
                case QuestionType.ObtainInfo:
                    //信息获取
                    ObtainInfoRV view18 = new ObtainInfoRV();
                    itemView = view18;
                    itemView.DataContext = new ObtainInfoRVM(view18.QsItemContent, paperDetail, examResult);
                    break;
                case QuestionType.SpokenOesy:
                    //口头作文  故事复述  话题简述
                    SpokenOesyRV view12 = new SpokenOesyRV();
                    itemView = view12;
                    itemView.DataContext = new SpokenOesyRVM(view12.QsItemContent, paperDetail, examResult);
                    break;
                case QuestionType.ObtainInfoAsk:
                    //信息获取及转述
                    ObtainInfoAskRV view37 = new ObtainInfoAskRV();
                    itemView = view37;
                    itemView.DataContext = new ObtainInfoAskRVM(view37.QsItemContent, paperDetail, examResult);
                    break;
            }

            _paperDetailView.Children.Clear();
            _paperDetailView.Children.Add(itemView);
        }

        private void BindTotalInfo()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                ObservableCollection<object> obs = new ObservableCollection<object>();
                obs.Add(new {TitleName = $"模考总成绩",});
                for (int i = 0; i < GlobalUser.SelectPaperInfo.paper_detail.Count; i++)
                {
                    obs.Add(new {TitleName = $"第{GetQsItemIndex(i)}大题",});
                }

                QsNaviName = obs;


                ShowTotalScore = Visibility.Visible;
                ShowDetailScore = Visibility.Collapsed;

                UserTotalScore = GlobalUser.SelectExamAttend.score.ToString();

                var currentPoint = GlobalUser.SelectExamAttend.score /
                                   Convert.ToDouble(GlobalUser.SelectPaperInfo.paper_score) * 100;

                var point = currentPoint;//GlobalUser.SelectPaperAvgScore5Points * 20;

                if (currentPoint >= 96)
                {
                    UserTotalScoreText = $"本次试卷总分 {GlobalUser.SelectPaperTotalScore}，您的得分是 ";
                    //ExamScoreForeground = "#3CB371";
                    TotalScoreForeground = "#3CB371";
                    DefeatPercent = $"太棒了，保持住！";
                }
                else if (point >= 85 && point < 96)
                {
                    UserTotalScoreText = $"本次试卷总分 {GlobalUser.SelectPaperTotalScore}，您的得分是 ";
                    TotalScoreForeground = "#3CB371";
                    DefeatPercent = $"哟，不错哦，再努把力～";
                }
                else if (point >= 75 && point < 85)
                {
                    UserTotalScoreText = $"本次试卷总分 {GlobalUser.SelectPaperTotalScore}，您的得分是 ";
                    TotalScoreForeground = "#3CB371";
                    DefeatPercent = $"还有上升空间，再接再厉～";
                }
                else if (point >= 61 && point < 75)
                {
                    UserTotalScoreText = $"本次试卷总分 {GlobalUser.SelectPaperTotalScore}，您的得分是 ";
                    TotalScoreForeground = "#3CB371";
                    DefeatPercent = $"多多努力，加强练习～";
                }
                else if (point >= 31 && point < 61)
                {
                    UserTotalScoreText = $"本次试卷总分 {GlobalUser.SelectPaperTotalScore}，您的得分是 ";
                    TotalScoreForeground = "#3CB371";
                    DefeatPercent = $"别灰心，努力练习～";
                }
                else
                {
                    UserTotalScoreText = $"本次试卷总分 {GlobalUser.SelectPaperTotalScore}，您的得分是 ";
                    TotalScoreForeground = "#3CB371";
                    DefeatPercent = $"是不是没发挥好？";
                }

                Calc5Score();
                TotalStarScore = SetStarScore();
            }));
        }

        private void Calc5Score()
        {
            float? AvgScore5Points = 5 * (GlobalUser.SelectExamAttend.score / GlobalUser.SelectPaperTotalScore);

            string strScore = Convert.ToDouble(AvgScore5Points).ToString("0.00");

            int leftPoint = int.Parse(strScore.Substring(0, 1));
            int rightPoint = int.Parse(strScore.Substring(2, 2));


            if (AvgScore5Points > 0)
            {

                if (rightPoint > 0 && rightPoint <= 30)
                {
                    rightPoint = 25;
                }
                else if (rightPoint > 30 && rightPoint <= 60)
                {
                    rightPoint = 50;
                }
                else if (rightPoint > 60 && rightPoint <= 99)
                {
                    rightPoint = 75;
                }
                else if (rightPoint > 99)
                {
                    if (leftPoint < 5)
                        leftPoint = leftPoint + 1;

                    rightPoint = 00;
                }

                AvgScore5Points = float.Parse($"{leftPoint}.{rightPoint}");
            }

            GlobalUser.SelectPaperAvgScore5Points = AvgScore5Points;
        }

        private ImageSource SetStarScore()
        {
            switch (GlobalUser.SelectPaperAvgScore5Points)
            {
                case 0.0f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_1_0);
                case 0.25f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_1_14);
                case 0.5f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_1_24);
                case 0.75f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_1_34);
                case 1.0f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_1_44);
                case 1.25f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_2_14);
                case 1.5f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_2_24);
                case 1.75f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_2_34);
                case 2.0f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_2_44);
                case 2.25f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_3_14);
                case 2.5f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_3_24);
                case 2.75f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_3_34);
                case 3.0f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_3_44);
                case 3.25f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_4_14);
                case 3.5f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_4_24);
                case 3.75f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_4_34);
                case 4.0f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_4_44);
                case 4.25f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_5_14);
                case 4.5f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_5_24);
                case 4.75f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_5_34);
                case 5.0f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_5_44);
                default:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_1_0);
            }

        }

        private string GetQsItemIndex(int index)
        {
            string strIndex = "一";

            switch (index)
            {
                case 0:
                    strIndex = "一";
                    break;
                case 1:
                    strIndex = "二";
                    break;
                case 2:
                    strIndex = "三";
                    break;
                case 3:
                    strIndex = "四";
                    break;
                case 4:
                    strIndex = "五";
                    break;
                case 5:
                    strIndex = "六";
                    break;
                case 6:
                    strIndex = "七";
                    break;
                case 7:
                    strIndex = "八";
                    break;
                case 8:
                    strIndex = "九";
                    break;
                case 9:
                    strIndex = "十";
                    break;
                case 10:
                    strIndex = "十一";
                    break;
                case 11:
                    strIndex = "十二";
                    break;
                case 12:
                    strIndex = "十三";
                    break;
                case 13:
                    strIndex = "十四";
                    break;
                case 14:
                    strIndex = "十五";
                    break;
                default:
                    strIndex = "一";
                    break;
            }

            return strIndex;
        }

        /// <summary>
        /// 显示 隐藏内容
        /// </summary>
        /// <param name="selectIndex"></param>
        private void ShowExamItemInfo()
        {
            if (SelectedExamShow == 0)
            {
                ShowTotalScore = Visibility.Visible;
                ShowDetailScore = Visibility.Collapsed;

                if (GlobalUser.WavePlayer != null)
                {
                    GlobalUser.WavePlayer?.Stop();
                    GlobalUser.WavePlayer = null;
                }
                if (GlobalUser.AudioFileReader != null)
                {
                    GlobalUser.AudioFileReader?.Dispose();
                    GlobalUser.AudioFileReader = null;
                }

                return;
            }

            //显示内容
            ShowTotalScore = Visibility.Collapsed;
            ShowDetailScore = Visibility.Visible;

            BindQs();
            
        }
        
        #endregion

        #region << 按钮事件 >>
        
        private RelayCommand _windowMinimizeCommand; // 窗口最小化
        /// <summary>
        /// 最小化窗口。
        /// </summary>
        public RelayCommand WindowMinimizeCommand
        {
            get
            {
                return _windowMinimizeCommand ?? (_windowMinimizeCommand = new RelayCommand(
                           (action) =>
                           {
                               var windows = Application.Current.Windows;
                               var mianWondow = new Window();
                               foreach (Window window in windows)
                               {
                                   if (window.Name == "MetroWindowMain")
                                   {
                                       CleanUp();
                                       PlayIcon = "Play";
                                       mianWondow = window;
                                   }
                               }
                               mianWondow.WindowState = WindowState.Minimized;
                           })
                        );
            }
        }

        private RelayCommand _windowCloseCommand; // 窗口关闭
        /// <summary>
        /// 关闭窗口。
        /// </summary>
        public RelayCommand WindowCloseCommand
        {
            get
            {
                return _windowCloseCommand ?? (_windowCloseCommand = new RelayCommand(
                           (action) =>
                           {
                               var view = new SignOutDialog()
                               {
                                   DataContext = new SignOutDialogVM()
                               };

                               var result = DialogHost.Show(view, "ExamMainDialog", WindowClosingEventHandler);
                           })
                        );
            }
        }

        private void WindowClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();

            //...now, lets update the "session" with some new content!
            eventArgs.Session.UpdateContent(new SampleProgressDialog());
            //note, you can also grab the session when the dialog opens via the DialogOpenedEventHandler

            //lets run a fake operation for 3 seconds then close this baby.
            //Task.Delay(TimeSpan.FromSeconds(1))
            //    .ContinueWith((t, _) => Environment.Exit(0), null,
            //        TaskScheduler.FromCurrentSynchronizationContext()); // 关闭程序

            Task.Factory.StartNew(() => Thread.Sleep(1000))
                .ContinueWith(t => Environment.Exit(0), TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion
        
    }
}

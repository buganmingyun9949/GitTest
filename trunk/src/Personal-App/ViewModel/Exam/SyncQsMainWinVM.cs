
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Framework.Logging;
using GalaSoft.MvvmLight.Command;
//using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
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
    public class SyncQsMainWinVM : ExamCommonVM
    {
        private DataGrid _dgSyncQsListItems;
        private WrapPanel _qsUcContent;
        private WrapPanel _mainQsContent;
        private static Button _BtnSkipNext;
        public const string ViewName = "SyncQsMainWinView";

        public SyncQsMainWinVM()
        {
            Messenger.Default.Unregister<ExamNavigateMessage>(this);
            Messenger.Default.Unregister<ExamNavigateMessage>(this, "SyncQsMainWinView");
            Messenger.Default.Unregister<ExamNavigateMessage>(this, "SyncNextQsView");
            Messenger.Default.Unregister<ExamNavigateMessage>(this, "SyncQsMainWinCmdView");
            Messenger.Default.Unregister<ExamNavigateMessage>(this, "SyncQsSampleAggregatorView");
            Messenger.Default.Unregister<ExamNavigateMessage>(this, "SyncScoreNavi");
            Messenger.Default.Unregister<ExamNavigateMessage>(this, "SyncUploadScoreNavi");
            Messenger.Default.Unregister<ExamNavigateMessage>(this, "SyncLoginFailure");

            Messenger.Default.Register<ExamQsNavigateMessage>(this, "SyncQsMainWinView", (message) => OnExamQsNavigate(message));
            Messenger.Default.Register<ExamNextQsNavigateMessage>(this, "SyncNextQsView", (message) => OnExamNextQsNavigate(message));
            Messenger.Default.Register<ExamQsCmdMessage>(this, "SyncQsMainWinCmdView", (message) => OnExamQsCmd(message));
            Messenger.Default.Register<ExamQsSampleAggregatorMessage>(this, "SyncQsSampleAggregatorView", (message) => OnExamQsSampleAggregator(message));
            Messenger.Default.Register<ExamScoreNavigateMessage>(this, "SyncScoreNavi", (message) => OnExamScoreNavigate(message));
            Messenger.Default.Register<SyncUploadScoreCallbckMessage>(this, "SyncUploadScoreNavi", (message) => OnSyncUploadScoreNavi(message));
            Messenger.Default.Register<ExamScoreNavigateMessage>(this, "SyncLoginFailure", (message) => OnLoginFailureNavigate(message));
        }

        public SyncQsMainWinVM(WrapPanel mainQsContent, Button btnSkipNext) : this()
        {
            _mainQsContent = mainQsContent;
            _BtnSkipNext = btnSkipNext;

            BindQs();
        }

        public void LoadSyncQsMainWinVM(DataGrid dgSyncQsListItems,  WrapPanel mainQsContent)
        {
            //UserPanelVM.ExamTitle = "中考英语听说模考";
            //UserPanelVM.SelectPaperNumber = GlobalUser.SelectPaperNumber.Split('#')[1];
            SyncQsMainTitleName = GlobalUser.SelectPaperName;

            _dgSyncQsListItems = dgSyncQsListItems;
            _mainQsContent = mainQsContent;
            //_BtnSkipNext = btnSkipNext;
            SelectedSyncQsShow = 0;

            var lsType = GlobalUser.SelectPaperNumber.Split('#');
            if (lsType[2] == "4")
                SyncQsHeaderContent = "单词";
            if (lsType[2] == "5")
                SyncQsHeaderContent = "句子";

            _dgSyncQsListItems.Columns[0].Header = SyncQsHeaderContent;
            _dgSyncQsListItems.SelectedIndex = SelectedSyncQsShow;
            BindQs();
        }


        #region <<  属性 字段>>


        private ObservableCollection<SelectableViewModel> _syncQsListItems;

        /// <summary>
        /// 题目内容列表。 
        /// </summary>
        public ObservableCollection<SelectableViewModel> SyncQsListItems
        {
            get
            {
                return _syncQsListItems;
            }
            set
            {
                _syncQsListItems = value;
                RaisePropertyChanged("SyncQsListItems");
            }
        }

        private string _syncQsHeaderContent;

        /// <summary>
        /// 单词/句子。 
        /// </summary>
        public string SyncQsHeaderContent
        {
            get
            {
                return _syncQsHeaderContent;
            }
            set
            {
                _syncQsHeaderContent = value;
                RaisePropertyChanged("SyncQsHeaderContent");
            }
        }

        private int _SelectedSyncQsShow;

        /// <summary>
        /// 选定项。 
        /// </summary>
        public int SelectedSyncQsShow
        {
            get
            {
                return _SelectedSyncQsShow;
            }
            set
            {
                if (_SelectedSyncQsShow != value)
                {
                    _SelectedSyncQsShow = value;
                    ShowSyncQsInfo();
                }

                RaisePropertyChanged("SelectedSyncQsShow");
            }
        }

        private string _SyncQsMainTitleName;

        /// <summary>
        /// 选定项。 
        /// </summary>
        public string SyncQsMainTitleName
        {
            get
            {
                return _SyncQsMainTitleName;
            }
            set
            {
                _SyncQsMainTitleName = value; 
                RaisePropertyChanged("SyncQsMainTitleName");
            }
        }

        #endregion

        #region << 自定义方法 >>

        private void OnExamScoreNavigate(ExamScoreNavigateMessage message)
        {
            ExamSubItemScoreVM examScore = new ExamSubItemScoreVM(message);
            examScore.EngineScore();
            examScore.AutoScore();
        }

        /// <summary>
        /// 更新 菜单 显示的最高评分
        /// </summary>
        /// <param name="message"></param>
        private void OnSyncUploadScoreNavi(SyncUploadScoreCallbckMessage msg)
        {
            try
            {
                if (msg.Overall >= 0 || SyncQsListItems.Count > 0)
                {
                    //var x = SyncQsListItems[SelectedSyncQsShow];
                    //if (x.item_id == msg.Item_Id)
                    //{
                    //    //MessageBox.Show($"{msg.Overall}");
                    //    if (x.Item_score == null || x.Item_score < msg.Overall)
                    //    {
                    //        x.Item_score = msg.Overall;
                    //        SyncQsListItems[SelectedSyncQsShow] = x;
                    //        RaisePropertyChanged("SyncQsListItems");
                    //    }
                    //}
                    for (int i = 0; i < SyncQsListItems.Count; i++)
                    {
                        if (SyncQsListItems[i].item_id == msg.Item_Id)
                        {
                            if (SyncQsListItems[i].Item_score == null || SyncQsListItems[i].Item_score < msg.Overall)
                            {
                                SyncQsListItems[i].Item_score = msg.Overall;
                                RaisePropertyChanged("SyncQsListItems");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("同步到列表 最新得分 失败", ex);
            }
        }


        private RelayCommand<string> _btncmd;//录音 正常,打开 考试

        public RelayCommand<string> btncmd
        {
            get
            {
                return _btncmd ?? (_btncmd = new RelayCommand<string>(s =>
                {
                    //var xx = _dgSyncQsListItems.Items[SelectedSyncQsShow] as Paper_InfoItem;

                    //xx.qs_show_score = new Random().Next(10, 100);

                    //for (int i = 0; i < SyncQsListItems.Count; i++)
                    //{
                    //    if (i == 1)
                    //    {
                    //        SyncQsListItems[i].Item_score = new Random().Next(10, 100);
                    //        RaisePropertyChanged("SyncQsListItems");
                    //    }
                    //}

                    //var x = SyncQsListItems[SelectedSyncQsShow < 1 ? 0 : _SelectedSyncQsShow];
                    ////MessageBox.Show($"{msg.Overall}");
                    ////if (x.qs_show_score == null || x.qs_show_score < msg.Overall)
                    ////{
                    //x.Item_score = new Random().Next(10, 100);
                    //SyncQsListItems[SelectedSyncQsShow < 1 ? 0 : _SelectedSyncQsShow] = x;
                    //RaisePropertyChanged("SyncQsListItems");
                    //_dgSyncQsListItems.ItemsSource = SyncQsListItems;
                    ////_dgSyncQsListItems.Items.Clear();
                    ////_dgSyncQsListItems.ItemsSource = null;
                    ////_dgSyncQsListItems.ItemsSource = SyncQsListItems;
                }));
            }
        }

        /// <summary>
        /// 登录异常,回到登录页
        /// </summary>
        /// <param name="message"></param>
        private void OnLoginFailureNavigate(ExamScoreNavigateMessage message)
        {
            foreach (var subUCItem in _mainQsContent.Children)
            {
                var subUC = subUCItem as FrameworkElement;
                var subVM = subUC.DataContext as QsBaseViewModel;

                subVM.CleanUp();
            }

            DialogHost.CloseAllShow();
            
            LoginFailure();
        }

        private void OnExamQsNavigate(ExamQsNavigateMessage message)
        {
            if (message.ViewModel != null)
                message.viewElement.DataContext = message.ViewModel;
            try
            {
                if (_mainQsContent.Children.Count > 0)
                {
                    var control = _mainQsContent.Children[0] as FrameworkElement;
                    var vm = control.DataContext as QsBaseViewModel;
                    vm.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("移除 QsBaseViewModel Dispose ", ex);
            }
            _mainQsContent.Children.Clear();
            _mainQsContent.Children.Add(message.viewElement);
        }

        private void OnExamNextQsNavigate(ExamNextQsNavigateMessage message)
        {
            if (SyncQsListItems.Count == (SelectedSyncQsShow + 1))
            {
                SelectedSyncQsShow = 0; 
            }
            else
            {
                SelectedSyncQsShow++;
            }

            _dgSyncQsListItems.SelectedIndex = SelectedSyncQsShow;
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindQs()
        {
            //模考
            BindSyncQs();
        }

        /// <summary>
        /// 绑定 模考
        /// </summary>
        private void BindSyncQs()
        {
            List<Exam_Attend_Result_Item> myAnswerResults =
                JsonHelper.FromJson<List<Exam_Attend_Result_Item>>(GlobalUser.SelectExamAttendResult);

            //var myAllItemId = string.Join(" ", myAnswerResults.Select(s => s.item_id).ToArray());
            var paperDetails = GlobalUser.SelectPaperInfo.paper_detail;

            if(myAnswerResults!=null)
            { 
                foreach (Exam_Attend_Result_Item item in myAnswerResults)
                {
                    paperDetails[0].info.ForEach(x =>
                    {
                        if (x.items[0].item_id == item.item_id)
                        {
                            x.qs_show_score = Convert.ToInt32(item.exam_score);
                            x.qs_show_score_result = item.score_result;
                        }
                    });
                }
            }

            SyncQsListItems = CreateData(paperDetails[0].info);

            ShowSyncQsInfo();
        }
        private static ObservableCollection<SelectableViewModel> CreateData(List<Paper_InfoItem> info)
        {
            var ls = new ObservableCollection<SelectableViewModel>();

            foreach (var v in info)
            {
                var item = new SelectableViewModel()
                {
                    item_id = v.items[0].item_id,
                    Item_content = v.items[0].item_content,
                    Item_score = v.qs_show_score,
                    item = v,
                };

                ls.Add(item);
            }

            return ls;
        }

        private void ShowSyncQsInfo()
        {

            Paper_InfoItem item = new Paper_InfoItem();

            if (_dgSyncQsListItems.Items == null || _dgSyncQsListItems.Items.Count <=1)
            {
                item = SyncQsListItems[SelectedSyncQsShow < 1 ? 0 : _SelectedSyncQsShow].item;
            }
            else
            {
                item = (_dgSyncQsListItems.Items[SelectedSyncQsShow < 1 ? 0 : _SelectedSyncQsShow] as SelectableViewModel).item;
            }

            var ml = new HistoryItemModel()
            {
                exam_attend_id = GlobalUser.AttendPaperItemId,
                item_id = item.items[0].item_id,
                token = GlobalUser.USER.Token
            };
            var result1 = WebProxy(ml, ApiType.GetSyncHistory, null, "get");

            SyncScoreHistoryItem historyItem = new SyncScoreHistoryItem();
            try
            {
                if (result1?.retCode == 0)
                {
                    historyItem = JsonHelper.FromJson<SyncScoreHistoryItem>(result1.retData.ToString());
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error($"抓取历史 单题答题记录 异常", e);
            }

            var lsType = GlobalUser.SelectPaperNumber.Split('#');

            if (lsType[2] == "4")
            {
                var view4 = new SyncWordView();
                view4.ScoreBoxView.Children.Clear();
                view4.DataContext = null;
                view4.DataContext =
                    new SyncWordViewModel(item, historyItem, view4,
                        (SelectedSyncQsShow < 1 ? 1 : _SelectedSyncQsShow + 1),
                        SyncQsListItems.Count);
                Messenger.Default.Send(new ExamQsNavigateMessage(SyncWordViewModel.ViewName, view4, null),
                    "SyncQsMainWinView");
            }
            else if (lsType[2] == "5")
            {
                var view5 = new SyncPredView();

                view5.ScoreBoxView.Children.Clear();
                view5.DataContext = null;
                view5.DataContext =
                    new SyncPredViewModel(item, historyItem, view5,
                        (SelectedSyncQsShow < 1 ? 1 : _SelectedSyncQsShow + 1),
                        SyncQsListItems.Count);
                Messenger.Default.Send(new ExamQsNavigateMessage(SyncPredViewModel.ViewName, view5,
                        null),
                    "SyncQsMainWinView");
            }
        }

        #endregion

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

    public class SelectableViewModel: MainViewModelBase
    {
        public string item_id { get; set; }

        public string Item_content { get; set; }

        private int? _item_score;
        public int? Item_score
        {
            get
            {
                return _item_score;
            }
            set
            {
                _item_score = value;
                RaisePropertyChanged("Item_score");
                RaisePropertyChanged("Item_score_view");
            }
        }
        private string _Item_score_view;
        public string Item_score_view
        {
            get
            {
                if (Item_score == null || Item_score < 0)
                {
                    return "未练习";
                }
                return $"{Item_score} 分";
            }
            set
            {
                _Item_score_view = value;
                RaisePropertyChanged("Item_score_view");
            }
        } 

        public Paper_InfoItem item { get; set; }
    }
}

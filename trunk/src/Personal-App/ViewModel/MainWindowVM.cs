using System;
using System.Collections.Generic;
using System.Configuration;
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
using Framework.Logging;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using Personal_App.Common;
using Personal_App.Domain;
using MaterialDesignThemes.Wpf;
using Personal_App.Controls;
using Personal_App.Domain.Menu;
using Personal_App.ViewModel.Menu;
using ST.Common;

namespace Personal_App.ViewModel
{
    public class MainWindowVM : MainViewModel
    {
        private WrapPanel _mainContentPanel;

        private MainWindow _metroWindow;

        public MainWindowVM()
        {
            Messenger.Reset();
            Messenger.Default.Unregister<NavigateMessage>(this);
            Messenger.Default.Unregister<NavigateMessage>(this, "ShowUserpapers");
            Messenger.Default.Unregister<ExamNavigateMessage>(this, "MainExamMainWin");
            Messenger.Default.Unregister<MainDialogMessage>(this, "MainMessageDialog");
            Messenger.Default.Unregister<MainDialogMessage>(this, "MainDownloadFile");
            Messenger.Default.Unregister<NavigateMessage>(this, "ReloadMenu");

            Messenger.Default.Register<NavigateMessage>(this, "ShowUserpapers", (message) => OnNavigate(message));
            Messenger.Default.Register<MainDialogMessage>(this, "MainMessageDialog", (msg) => ShowMsgDialog(msg));
            Messenger.Default.Register<MainDialogMessage>(this, "MainDownloadFile", (msg) => DownLoadFile(msg));
            Messenger.Default.Register<NavigateMessage>(this, "ReloadMenu", (message) => OnReloadMenu(message));
        }

        public MainWindowVM(MainWindow metroWindow, WrapPanel mainContentPanel) : this()
        {
            LoadMainWindowVm(metroWindow, mainContentPanel);
        }


        #region << 属性 字段 >>

        List<string> menuViewTargets = new List<string>();
        List<ListViewItem> menuItems = new List<ListViewItem>();
        Dictionary<string, ListViewItem> dicMenu = new Dictionary<string, ListViewItem>();

        private int _SelectedMenuShow;

        public int SelectedMenuShow
        {
            get => _SelectedMenuShow;
            set
            {
                if (_SelectedMenuShow != value)
                {
                    _SelectedMenuShow = value;
                    ShowSelectedMenuInfo();
                    RaisePropertyChanged("SelectedMenuShow");
                }
            }
        }

        private Visibility _ShowService;

        /// <summary>
        /// 是否显示客服按钮
        /// </summary>
        public Visibility ShowService
        {
            get => _ShowService;
            set
            {
                _ShowService = value;
                RaisePropertyChanged("ShowService");
            }
        }

        private Visibility _ShowOperationloading;

        public Visibility ShowOperationloading
        {
            get => _ShowOperationloading;
            set
            {
                _ShowOperationloading = value;
                RaisePropertyChanged("ShowOperationloading");
            }
        }

        private object _QrCode;

        public object QrCode
        {
            get => _QrCode;
            set
            {
                _QrCode = value;
                RaisePropertyChanged("QrCode");
            }
        }

        #endregion

        private void ShowSelectedMenuInfo()
        {
            if(_metroWindow.MenuListBox.SelectedIndex == SelectedMenuShow) return;

            //未绑卡
            if (GlobalUser.STUDYCARD == null || GlobalUser.STUDYCARD.expire_status == -1)
            {
                Messenger.Default.Send(new NavigateMessage(BindingCardVM.ViewName, null), "ShowUserpapers");
            }

            if (GlobalUser.STUDYCARD.expire_status == 1)
            {
                //Messenger.Default.Send(new NavigateMessage("ChildrenClear", null), "ShowUserpapers"); //清空重复

                _metroWindow.MenuListBox.SelectedIndex = SelectedMenuShow;

                if (SelectedMenuShow < 0) return;
                var lsUc = GetChildObjects<TextBlock>(menuItems[SelectedMenuShow], typeof(TextBlock));

                var key = "";
                if (lsUc.Count < 1)
                    key = dicMenu.Keys.FirstOrDefault();
                else
                    key = lsUc[0].Text;

                switch (key)
                {
                    case "作业":
                        BindUserZy();
                        return;
                    case "同步":
                        BindUserSync();
                        return;
                    case "专项":
                        BindUserTrain();
                        return;
                    case "模考":
                        BindUserPapers();
                        return;
                    case "报纸同步":
                        BindNewPaperVM();
                        return;
                    case "完整版本":
                        BindFullVer();
                        return;
                    default:
                        BindMainPaper();
                        return;
                }
            }
        }

        void ChangeCardEvent(string cardValidity)
        {
            //User.Card = GlobalUser.USER.Card;
            Validity = GlobalUser.STUDYCARD.expire_time;
        }

        #region << Btn Command >>

        #region 最小化窗口命令...

        /// <summary>
        /// 最小化窗口。
        /// </summary>
        public RelayCommand<Window> MinimizeWindowCommand { get; private set; }

        private void MinimizeWindow(Window window)
        {
            if (window != null)
            {
                window.WindowState = WindowState.Minimized;
            }
        }

        #endregion

        /// <summary>
        /// 注销登录。
        /// </summary>
        private RelayCommand userLogoutCommand;

        public RelayCommand UserLogoutCommand
        {
            get
            {
                return userLogoutCommand ?? (userLogoutCommand = new RelayCommand((action) =>
                {
                    var view = new LogoffDialog()
                    {
                    };

                    var result = DialogHostEx.ShowDialog(GlobalUser.MainWin, view, LogoutEventHandler);
                }));
            }
        }

        private RelayCommand openUserInfoWinCommand;

        public RelayCommand OpenUserInfoWinCommand
        {
            get
            {
                return openUserInfoWinCommand ?? (openUserInfoWinCommand = new RelayCommand((action) =>
                {
                    Messenger.Default.Send(new NavigateMessage(UserInfoWinVM.ViewName, null, true),
                        "ShowUserpapers");
                }));
            }
        }

        private void LogoutEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool) eventArgs.Parameter == false) return;

            Loginout();
        }


        private void ExtendedPayOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Task.Factory.StartNew(() => Thread.Sleep(5000))
                .ContinueWith(t =>
                {
                    eventargs.Session.Close();
                    //打开 学习卡 绑定 
                    MainLoadoadMenu(true);
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// 注销登录。
        /// </summary>
        private RelayCommand _ShowServiceDialog;

        public RelayCommand ShowServiceDialog
        {
            get
            {
                return _ShowServiceDialog ?? (_ShowServiceDialog = new RelayCommand((action) =>
                {
                    var view = new CustomerServiceDialog();
                    view.DataContext = new CustomerServiceDialogVM();

                    var result = DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ShowServiceDialogCloseEventHandler);
                }));
            }
        }

        private void ShowServiceDialogCloseEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            //if ((bool)eventArgs.Parameter == false) return;

            //Application.Current.Dispatcher.Invoke(new Action(() =>
            //{


            //}));
        }

        #endregion

        /// <summary>
        /// 获取或设置用户信息。
        /// </summary>
        //public RespObject User { get; set; }

        #region << 自定义方法 >>

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                    if (child != null && child is T)
                        yield return (T) child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }

        /// <summary>
        /// 查找父控件的子控件集合
        /// </summary>
        /// <typeparam name="T">子控件类型</typeparam>
        /// <param name="obj">父控件</param>
        /// <param name="typename">子控件类型名</param>
        /// <returns></returns>
        private List<T> GetChildObjects<T>(DependencyObject obj, Type typename) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();


            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);


                if (child is T && (((T) child).GetType() == typename) &&
                    ((T) child).Tag?.ToString() == "ListBox")
                {
                    childList.Add((T) child);
                }

                if (child != null)
                    childList.AddRange(GetChildObjects<T>(child, typename));
            }

            return childList;
        }

        public void LoadMainWindowVm(MainWindow metroWindow, WrapPanel mainContentPanel)
        {
            _metroWindow = metroWindow;
            _mainContentPanel = mainContentPanel;

            views = new Dictionary<string, FrameworkElement>();

            User = GlobalUser.USER;
            Classinfo = GlobalUser.CLASSINFO;

            #region << 绑定 窗口 >>

            BindDefaultWin("");

            #endregion

            //if (GlobalUser.USER?.Expire_status != -1)
            //{
            //    Validity = Convert.ToDateTime(GlobalUser.USER.Card?.expire_time).ToString("yyyy年MM月dd日 HH时mm分 到期");
            //    //Validity = String.IsNullOrEmpty(GlobalUser.USER.Card?.ValidityEnd) ? "尚未绑定学习卡" : $"{TimeHelper.ConvertStringToDateTime(GlobalUser.USER.Card?.ValidityEnd).ToString("yyyy 年 MM 月 dd 日到期")}";
            //}

            //if (!string.IsNullOrEmpty(GlobalUser.USER.Card?.expire_time))
            //{
            //    if (Convert.ToDateTime(GlobalUser.USER.Card?.expire_time).AddDays(-7) < DateTime.Now)
            //    {
            //        //过期7天提醒
            //        ShowPayBox = Visibility.Visible;
            //        TxtCardTimeout =
            //            $"学习卡将于{Convert.ToDateTime(GlobalUser.USER.Card?.expire_time).ToString("yyyy-MM-dd")}过期";
            //    }
            //}

            //if (GlobalUser.STUDYCARD.card_type == 2)
            //{
            //    //试用卡永久
            //    ShowPayBox = Visibility.Visible;
            //    TxtCardTimeout =
            //        $"学习卡将于{Convert.ToDateTime(GlobalUser.USER.Card?.expire_time).ToString("yyyy-MM-dd")}过期";
            //}

            //未绑卡
            if (GlobalUser.STUDYCARD == null)
            {
                //ShowPayBox = Visibility.Collapsed;
                Messenger.Default.Send(new NavigateMessage(BindingCardRemindVM.ViewName, null), "ShowUserpapers");
                //Validity = "尚未绑定学习卡";
            }
            //卡过期
            else if (GlobalUser.STUDYCARD.expire_status == 0)
            {
                //ShowPayBox = Visibility.Collapsed;
                //TxtCardTimeout =
                //    $"学习卡将于{Convert.ToDateTime(GlobalUser.USER.Card?.expire_time).ToString("yyyy-MM-dd")}过期";
                Messenger.Default.Send(new NavigateMessage(BindingCardRemindVM.ViewName, null), "ShowUserpapers");
                BindKeFu();
            }
            else if (GlobalUser.STUDYCARD.expire_status == -1)
            {
                //ShowPayBox = Visibility.Collapsed;
                Messenger.Default.Send(new NavigateMessage(BindingCardRemindVM.ViewName, null), "ShowUserpapers");
                BindKeFu();
            }
            else
            {
                ShowOperationloading = Visibility.Collapsed;
                if (!GlobalUser.USER.UnFirstOpen && GlobalUser.STUDYCARD?.card_type != 3)
                {
                    ShowOperationloading = Visibility.Visible;
                }

                BindKeFu();

                MainLoadoadMenu(false);

                SelectedMenuShow = 0;

                ShowSelectedMenuInfo();
            }

            MinimizeWindowCommand = new RelayCommand<Window>(MinimizeWindow);
        }

        public override void MainLoadoadMenu(bool isReload)
        {
            if (isReload)
            {
                BindDefaultWin("");

                //ShowPayBox = Visibility.Collapsed;
                //if (!string.IsNullOrEmpty(GlobalUser.USER.Card?.expire_time))
                //{
                //    if (Convert.ToDateTime(GlobalUser.USER.Card?.expire_time).AddDays(-7) < DateTime.Now)
                //    {
                //        //过期7天提醒
                //        ShowPayBox = Visibility.Visible;
                //        TxtCardTimeout =
                //            $"学习卡将于{Convert.ToDateTime(GlobalUser.USER.Card?.expire_time).ToString("yyyy-MM-dd")}过期";
                //    }
                //}

                //if (GlobalUser.STUDYCARD.card_type == 2)
                //{
                //    //试用卡永久
                //    ShowPayBox = Visibility.Visible;
                //    TxtCardTimeout =
                //        $"学习卡将于{Convert.ToDateTime(GlobalUser.USER.Card?.expire_time).ToString("yyyy-MM-dd")}过期";
                //}
            }

            var ct = JsonHelper.FromJsonTo<Card_Setting>(GlobalUser.STUDYCARD.card_setting.ToString());

            if (ct.module_type == 1)
            {
                int card_module_num = Convert.ToInt32(ct.card_modules.ToString());

                menuItems?.Clear();
                dicMenu?.Clear();
                menuViewTargets?.Clear();

                if (GlobalUser.STUDYCARD.card_type == 3)
                {
                    #region << 报纸同步 >>

                    GetMenuItemUc("\xe65b", "报纸同步");
                    menuViewTargets.Add(MenuNewPaperVM.ViewName);

                    #endregion

                    #region << 完整版本 >>

                    GetMenuItemUc("\xe630", "完整版本");
                    menuViewTargets.Add(MenuFullVerVM.ViewName);

                    #endregion
                }
                else
                {
                    //动态本地菜单
                    //_metroWindow.MenuListBox.ItemsSource = null;
                    //_metroWindow.MenuListBox.Items.Clear();

                    #region << 首页 >>

                    GetMenuItemUc("\ue688", "首页");
                    menuViewTargets.Add(MainPageVM.ViewName);

                    #endregion

                    //if ((card_module_num & (1 << 5)) > 0)
                    //{

                    #region << 作业 >>

                    GetMenuItemUc("\ue613", "作业");
                    menuViewTargets.Add(MenuHomeworkVM.ViewName);

                    #endregion

                    //}

                    #region << 同步 >>

                    //GetMenuItemUc("\ue600", "同步");
                    //menuViewTargets.Add(MenuSyncVM.ViewName);

                    #endregion

                    #region << 专项 >>

                    GetMenuItemUc("\ue61a", "专项");
                    menuViewTargets.Add(MenuTrainVM.ViewName);

                    #endregion

                    #region << 模考 >>

                    GetMenuItemUc("\ue623", "模考");
                    menuViewTargets.Add(MenuExamVM.ViewName);

                    #endregion
                }

                _metroWindow.MenuListBox.ItemsSource = null;
                _metroWindow.MenuListBox.ItemsSource = dicMenu.Values.ToList();
            }

            if (ct.module_type == 2)
            {
                //动态菜单
            }

            if (isReload)
            {
                _metroWindow.MenuListBox.SelectedIndex = 0;

                //ShowSelectedMenuInfo();
            }
        }

        private void GetMenuItemUc(string titleIcon, string titleName)
        {
            ListViewItem item = new ListViewItem();
            Grid gd = new Grid();
            TextBlock tbIcon = new TextBlock();
            TextBlock tbContent = new TextBlock();

            item = new ListViewItem();
            item.Height = 90;
            item.VerticalContentAlignment = VerticalAlignment.Center;
            item.HorizontalContentAlignment = HorizontalAlignment.Center;
            gd = new Grid();
            gd.VerticalAlignment = VerticalAlignment.Center;
            gd.HorizontalAlignment = HorizontalAlignment.Center;
            gd.RowDefinitions.Add(new RowDefinition());
            gd.RowDefinitions.Add(new RowDefinition()); 

            tbIcon = new TextBlock();
            tbContent = new TextBlock();
            tbIcon.Text = titleIcon;
            tbIcon.FontFamily = Application.Current.TryFindResource("IconFont") as FontFamily;
            tbIcon.FontSize = 30;
            tbIcon.HorizontalAlignment = HorizontalAlignment.Center;
            tbContent.Text = titleName;
            tbContent.FontFamily = new FontFamily("Microsoft YaHei");
            tbContent.Tag = "ListBox";
            tbContent.Margin = new Thickness(0, 8, 0, 0);
            tbContent.HorizontalAlignment = HorizontalAlignment.Center;

            tbIcon.SetValue(Grid.RowProperty, 0);
            tbContent.SetValue(Grid.RowProperty, 1);
            gd.Children.Add(tbIcon);
            gd.Children.Add(tbContent);
            item.Content = gd;

            menuItems.Add(item);
            dicMenu.Add(tbContent.Text, item);
        }

        /// <summary>
        /// 绑定客服
        /// </summary>
        private void BindKeFu()
        {
            ShowService = Visibility.Collapsed;

            var result1 = WebProxy(ApiType.GetAgentInfo, GlobalUser.USER.Token);

            if (result1.retCode == 0)
            {
                AgentInfo info = JsonHelper.FromJson<AgentInfo>(result1.retData.ToString());
                //if (!info.qrcode.ToLower().Contains("http"))
                //    info.qrcode = $"{WebApiProxy.STATICURL}{info.qrcode}";
                GlobalUser.AGENTINFO = info;
                ShowService = Visibility.Visible;

                //BitmapImage bi = new BitmapImage();
                //bi.BeginInit();
                //bi.CacheOption = BitmapCacheOption.OnLoad;
                //bi.UriSource = new Uri(info.qrcode);
                //bi.EndInit();
                //QrCode = bi;
            }
        }

        /// <summary>
        /// 绑定 默认窗口
        /// </summary>
        private void BindDefaultWin(string viewName)
        {
            ClearMemory();
            var viewBC = new BindingCard();
            var view0 = new MainPage();
            var view1 = new MenuTrain();
            var view2 = new MenuExam();
            var view3 = new MenuHomework();
            var view4 = new MenuSync();
            var view10 = new TrainListControl();
            var view11 = new SyncListControl();
            var view12 = new SyncUnitListControl();
            var view13 = new MenuFullVer();
            var view14 = new MenuNewPaper();
            var view15 = new TyHomeworkControl();

            switch (viewName)
            {
                case BindingCardVM.ViewName:
                    ClearOneBindWin(viewName);
                    BindingCardVM bindingCardVM = new BindingCardVM(viewBC);
                    bindingCardVM.ChangeCardEvent += ChangeCardEvent;
                    SetupView(BindingCardVM.ViewName, viewBC, bindingCardVM);
                    break;
                case BindingCardRemindVM.ViewName:
                    ClearOneBindWin(viewName);
                    SetupView(BindingCardRemindVM.ViewName, new BindingCardRemind(), new BindingCardRemindVM());

                    break;
                //case ExpireCardVM.ViewName:
                //SetupView(ExpireCardVM.ViewName, new ExpireCard(), new ExpireCardVM());//卡片过期

                //break;
                case BindingClassVM.ViewName:
                    ClearOneBindWin(viewName);
                    SetupView(BindingClassVM.ViewName, new BindingClass(), new BindingClassVM());
                    break;
                case UserInfoWinVM.ViewName:
                    ClearOneBindWin(viewName);
                    SetupView(UserInfoWinVM.ViewName, new UserInfoWin(), new UserInfoWinVM());
                    break;
                case MainPageVM.ViewName:
                    ClearOneBindWin(MainPageVM.ViewName);
                    SetupView(MainPageVM.ViewName, view0, new MainPageVM(view0.FPPanel, view0.NewZyPanel));
                    break;
                case MenuTrainVM.ViewName:
                    ClearOneBindWin(viewName);
                    SetupView(MenuTrainVM.ViewName, view1, new MenuTrainVM(view1.TrainContentPanel));
                    break;
                case MenuExamVM.ViewName:
                    ClearOneBindWin(viewName);
                    SetupView(MenuExamVM.ViewName, view2, new MenuExamVM(view2.PaperContentPanel));
                    break;
                case MenuHomeworkVM.ViewName:
                    ClearOneBindWin(viewName);
                    SetupView(MenuHomeworkVM.ViewName, view3,
                        new MenuHomeworkVM(view3.NewContentPanel, view3.CompleteContentPanel));
                    break;
                case MenuSyncVM.ViewName:
                    ClearOneBindWin(viewName);
                    SetupView(MenuSyncVM.ViewName, view4, new MenuSyncVM());
                    break;
                case TrainListControlVM.ViewName:
                    ClearOneBindWin(viewName);
                    SetupView(TrainListControlVM.ViewName, view10, null);
                    break;
                case SyncListControlVM.ViewName:
                    ClearOneBindWin(viewName);
                    SetupView(SyncListControlVM.ViewName, view11, null);
                    break;
                case SyncUnitListControlVM.ViewName:
                    ClearOneBindWin(viewName);
                    SetupView(SyncUnitListControlVM.ViewName, view12, null);
                    break;
                case MenuFullVerVM.ViewName:
                    ClearOneBindWin(viewName);
                    SetupView(MenuFullVerVM.ViewName, view13, new MenuFullVerVM());
                    break;
                case MenuNewPaperVM.ViewName:
                    ClearOneBindWin(viewName);
                    SetupView(MenuNewPaperVM.ViewName, view14, new MenuNewPaperVM(view14.NewPaperContentPanel));
                    break;
                case TyHomeworkControlVM.ViewName:
                    ClearOneBindWin(viewName);
                    SetupView(TyHomeworkControlVM.ViewName, view15, new TyHomeworkControlVM());
                    break;
                    ;
                default:
                {
                    ClearDefaultBindWin();

                    BindingCardVM bindingCardVM0 = new BindingCardVM(viewBC);
                    bindingCardVM0.ChangeCardEvent += ChangeCardEvent;
                    SetupView(BindingCardVM.ViewName, viewBC, bindingCardVM0);

                    SetupView(BindingCardRemindVM.ViewName, new BindingCardRemind(), new BindingCardRemindVM());

                    //SetupView(ExpireCardVM.ViewName, new ExpireCard(), new ExpireCardVM());//卡片过期

                    SetupView(BindingClassVM.ViewName, new BindingClass(), new BindingClassVM());

                    SetupView(UserInfoWinVM.ViewName, new UserInfoWin(), new UserInfoWinVM());

                    SetupView(MainPageVM.ViewName, view0, new MainPageVM(view0.FPPanel, view0.NewZyPanel));

                    SetupView(MenuTrainVM.ViewName, view1, new MenuTrainVM(view1.TrainContentPanel));

                    SetupView(MenuExamVM.ViewName, view2, new MenuExamVM(view2.PaperContentPanel));

                    SetupView(MenuHomeworkVM.ViewName, view3,
                        new MenuHomeworkVM(view3.NewContentPanel, view3.CompleteContentPanel));

                    SetupView(MenuSyncVM.ViewName, view4, new MenuSyncVM());

                    SetupView(TrainListControlVM.ViewName, view10, null);

                    SetupView(SyncListControlVM.ViewName, view11, null);

                    SetupView(SyncUnitListControlVM.ViewName, view12, null);

                    SetupView(MenuFullVerVM.ViewName, view13, new MenuFullVerVM());

                    SetupView(MenuNewPaperVM.ViewName, view14, new MenuNewPaperVM(view14.NewPaperContentPanel));

                    SetupView(TyHomeworkControlVM.ViewName, view15, new TyHomeworkControlVM());
                }
                    break;
            }

            //var viewBC = new BindingCard();
            //BindingCardVM bindingCardVM = new BindingCardVM(viewBC);
            //bindingCardVM.ChangeCardEvent += ChangeCardEvent;
            //SetupView(BindingCardVM.ViewName, viewBC, bindingCardVM);

            //SetupView(BindingCardRemindVM.ViewName, new BindingCardRemind(), new BindingCardRemindVM());

            ////SetupView(ExpireCardVM.ViewName, new ExpireCard(), new ExpireCardVM());//卡片过期

            //SetupView(BindingClassVM.ViewName, new BindingClass(), new BindingClassVM());

            //SetupView(UserInfoWinVM.ViewName, new UserInfoWin(), new UserInfoWinVM());

            //var view0 = new MainPage();
            //SetupView(MainPageVM.ViewName, view0, new MainPageVM(view0.FPPanel, view0.NewZyPanel));
            //var view1 = new MenuTrain();
            //SetupView(MenuTrainVM.ViewName, view1, new MenuTrainVM(view1.TrainContentPanel));
            //var view2 = new MenuExam();
            //SetupView(MenuExamVM.ViewName, view2, new MenuExamVM(view2.PaperContentPanel));
            //var view3 = new MenuHomework();
            //SetupView(MenuHomeworkVM.ViewName, view3,
            //    new MenuHomeworkVM(view3.NewContentPanel, view3.CompleteContentPanel));
            //var view4 = new MenuSync();
            //SetupView(MenuSyncVM.ViewName, view4, new MenuSyncVM());

            //var view10 = new TrainListControl();
            //SetupView(TrainListControlVM.ViewName, view10, null);
            //var view11 = new SyncListControl();
            //SetupView(SyncListControlVM.ViewName, view11, null);
            //var view12 = new SyncUnitListControl();
            //SetupView(SyncUnitListControlVM.ViewName, view12, null);

            //var view13 = new MenuFullVer();
            //SetupView(MenuFullVerVM.ViewName, view13, new MenuFullVerVM());

            //var view14 = new MenuNewPaper();
            //SetupView(MenuNewPaperVM.ViewName, view14, new MenuNewPaperVM(view14.NewPaperContentPanel));

            //var view15 = new TyHomeworkControl();
            //SetupView(TyHomeworkControlVM.ViewName, view15, new TyHomeworkControlVM());
        }

        private void OnNavigate(NavigateMessage message)
        {
            if (message.State?.ToString() == "NewCard" && message.TargetView == null)
            {
                MainLoadoadMenu(true);
                //Messenger.Default.Send(new NavigateMessage(null, "NewCard", true), "ShowUserpapers");

                //ClearDefaultBindWin();
                BindDefaultWin("");

                this.CurrentView = views[menuViewTargets[0]];
                this.currentViewName = menuViewTargets[0];

                if (message.IsClear)
                    _mainContentPanel.Children.Clear();

                _mainContentPanel.Children.Add(CurrentView);
                //_metroWindow.MainContentSV.ScrollToVerticalOffset(0);

                SelectedMenuShow = 0;
                _metroWindow.MenuListBox.SelectedIndex = 0;

                return;
            }

            if (!(message.TargetView == MenuTrainVM.ViewName || message.TargetView == MenuExamVM.ViewName ||
                  message.TargetView == MenuHomeworkVM.ViewName ||
                  message.TargetView == MenuSyncVM.ViewName ||
                  message.TargetView == MainPageVM.ViewName ||
                  message.TargetView == MenuNewPaperVM.ViewName ||
                  message.TargetView == MenuFullVerVM.ViewName))
                _metroWindow.MenuListBox.SelectedItem = null; //非菜单内容时 取消左侧选中样式

            var lsUc = GetChildObjects<TextBlock>(_metroWindow.MenuListBox, typeof(TextBlock)).Select(s => s.Text)
                .ToList();
            switch (message.TargetView)
            {
                case MenuHomeworkVM.ViewName:
                    _metroWindow.MenuListBox.SelectedIndex = lsUc.ToList().IndexOf("作业");
                    break;
                case MenuSyncVM.ViewName:
                    _metroWindow.MenuListBox.SelectedIndex = lsUc.ToList().IndexOf("同步");
                    break;
                case MenuTrainVM.ViewName:
                    _metroWindow.MenuListBox.SelectedIndex = lsUc.ToList().IndexOf("专项");
                    break;
                case MenuExamVM.ViewName:
                    _metroWindow.MenuListBox.SelectedIndex = lsUc.ToList().IndexOf("模考");
                    break;
                case MainPageVM.ViewName:
                    _metroWindow.MenuListBox.SelectedIndex = lsUc.ToList().IndexOf("首页");
                    break;
                case MenuNewPaperVM.ViewName:
                    _metroWindow.MenuListBox.SelectedIndex = lsUc.ToList().IndexOf("报纸同步");
                    break;
                case MenuFullVerVM.ViewName:
                    _metroWindow.MenuListBox.SelectedIndex = lsUc.ToList().IndexOf("完整版本");
                    break;
                default:
                    break;
            }

            if (message.IsClear && message.TargetView == "ChildrenClear")
            {
                _mainContentPanel.Children.Clear();
            }
            else if (message.IsClear && message.TargetView == BindingClassVM.ViewName &&
                     message.State?.ToString() == BindingClassVM.ViewName)
            {
                _mainContentPanel.Children.Clear();

                this.CurrentView = views[message.TargetView];
                this.currentViewName = message.TargetView;
                _mainContentPanel.Children.Add(CurrentView);

                //_metroWindow.MainContentSV.ScrollToVerticalOffset(0);
                //SelectedMenuShow = -1;
            }
            else if (message.IsClear && message.State?.ToString() == "N") //new
            {
                _mainContentPanel.Children.Clear();

                //this.CurrentView = views[message.TargetView];
                //this.currentViewName = message.TargetView;
                _mainContentPanel.Children.Add(message.Fe);

                //_metroWindow.MainContentSV.ScrollToVerticalOffset(0);
                //SelectedMenuShow = -1;
            }
            else
            {
                if (views.ContainsKey(message.TargetView))
                {
                    GlobalUser.ErrScoreInfo?.Clear();
                    GlobalUser.ErrScoreInfo = null;
                       User = GlobalUser.USER;
                    Classinfo = GlobalUser.CLASSINFO;

                    if (message.State?.ToString() == "R")
                    {
                        //ClearDefaultBindWin();
                        BindDefaultWin(message.TargetView);
                    }

                    this.CurrentView = views[message.TargetView];
                    this.currentViewName = message.TargetView;
                    //((IView)this.CurrentView.DataContext).Activated(message.State);
                    //if (message.IsClear)
                        _mainContentPanel.Children.Clear();

                    _mainContentPanel.Children.Add(CurrentView);
                    //_metroWindow.MainContentSV.ScrollToVerticalOffset(0);
                }
            }
        }

        private void OnReloadMenu(NavigateMessage message)
        {
            MainLoadoadMenu(true);
            SelectedMenuShow = 0;
        }

        private void DownLoadFile(MainDialogMessage message)
        {
            try
            {
                var fileByte = WebApiProxy.GetAudioFile($"{WebApiProxy.MEDIAURL}{message.MessageText}");
                if (fileByte != null)
                    File.WriteAllBytes(Path.Combine(GlobalUser.AUDIODATAFOLDER, Path.GetFileName(message.MessageText)),
                        fileByte);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error($"下载 音频失败, err:{ex}");
            }
        }

        private void ShowMsgDialog(MainDialogMessage message)
        {
            var view = new MessageDialog();
            view.DataContext = new MessageDialogVM()
            {
                MsgTitle = "提示消息",
                MsgContent = message.MessageText,
            };

            if (_metroWindow.IsActive)
            {
                if (message.ControlElement != null)
                {
                    DialogHostEx.ShowDialog(message.ControlElement, view, MessageDialogClose);
                }
                else
                {
                    if (GlobalUser.MainWin != null)
                        DialogHostEx.ShowDialog(GlobalUser.MainWin, view, MessageDialogClose);
                }
            }
        }

        public void BindMainPaper()
        {
            GlobalUser.MenuType = MenuType.Main;
            Messenger.Default.Send(new NavigateMessage(MainPageVM.ViewName, "R", false), "ShowUserpapers");
        }

        public void BindFullVer()
        {
            GlobalUser.MenuType = MenuType.TyFullVer;
            Messenger.Default.Send(new NavigateMessage(MenuFullVerVM.ViewName, null, false), "ShowUserpapers");
        }

        public void BindNewPaperVM()
        {
            GlobalUser.MenuType = MenuType.PaperSync;
            Messenger.Default.Send(new NavigateMessage(MenuNewPaperVM.ViewName, null, false), "ShowUserpapers");
        }

        public void BindUserPapers()
        {
            GlobalUser.MenuType = MenuType.Exam;
            Messenger.Default.Send(new NavigateMessage(MenuExamVM.ViewName, "R", false), "ShowUserpapers");
        }

        public void BindUserZy()
        {
            if (string.IsNullOrEmpty(GlobalUser.CLASSINFO?.Class_name))
            {
                Messenger.Default.Send(new NavigateMessage(BindingClassVM.ViewName, BindingClassVM.ViewName, true),
                    "ShowUserpapers");
                return;
            }
            else
            {
                GlobalUser.MenuType = MenuType.Task;
                Messenger.Default.Send(new NavigateMessage(MenuHomeworkVM.ViewName, "R", true), "ShowUserpapers");
            }
        }

        public void BindUserTrain()
        {
            GlobalUser.MenuType = MenuType.Train;
            Messenger.Default.Send(new NavigateMessage(MenuTrainVM.ViewName, "R", true), "ShowUserpapers");
        }

        public void BindUserSync()
        {
            GlobalUser.MenuType = MenuType.Sync;
            Messenger.Default.Send(new NavigateMessage(MenuSyncVM.ViewName, null, true), "ShowUserpapers");
        }

        #endregion
    }
}
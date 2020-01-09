using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Framework.Logging;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using Personal_App.Common;
using Personal_App.Domain;
using MaterialDesignThemes.Wpf;
using ST.Common;

namespace Personal_App.ViewModel
{
    public class MainWindowVM : MainViewModel
    {
        private WrapPanel _mainContentPanel;

        private Window _metroWindow;

        public MainWindowVM() { }

        void ChangeCardEvent(string cardValidity)
        {
            User.Card = GlobalUser.USER.Card;
            Validity = GlobalUser.USER.Card.expire_time;
        }

        public MainWindowVM(Window metroWindow, WrapPanel mainContentPanel)
        {
            _metroWindow = metroWindow;
            _mainContentPanel = mainContentPanel;

            Messenger.Default.Unregister<NavigateMessage>(this);
            Messenger.Default.Unregister<NavigateMessage>(this, "ShowUserpapers");

            Messenger.Default.Register<NavigateMessage>(this, (message) => OnNavigate(message));
            Messenger.Default.Register<NavigateMessage>(this, "ShowUserpapers", (action) => BindUserPapers());

            //Messenger.Default.Register<CardExpireDialogMessage>(this, "MainMessageDialog", (message) => OnShowCardExpireDialog(message));

            views = new Dictionary<string, FrameworkElement>();

            BindingCardVM bindingCardVM = new BindingCardVM();
            bindingCardVM.ChangeCardEvent += ChangeCardEvent;

            SetupView(BindingCardVM.ViewName, new BindingCard(), bindingCardVM);

            SetupView(ExpireCardVM.ViewName, new ExpireCard(), new ExpireCardVM());

            if (GlobalUser.USER?.Expire_status != -1)
            {
                User = GlobalUser.USER;
                Validity = Convert.ToDateTime(GlobalUser.USER.Card?.expire_time).ToString("yyyy年MM月dd日 HH时mm分 到期");
                //Validity = String.IsNullOrEmpty(GlobalUser.USER.Card?.ValidityEnd) ? "尚未绑定学习卡" : $"{TimeHelper.ConvertStringToDateTime(GlobalUser.USER.Card?.ValidityEnd).ToString("yyyy 年 MM 月 dd 日到期")}";
            }

            //未绑卡
            if (GlobalUser.USER.Card == null || GlobalUser.USER.Expire_status == -1)
            {
                Messenger.Default.Send(new NavigateMessage(BindingCardVM.ViewName, null));
                Validity = "尚未绑定学习卡";
            }
            //卡过期
            else if (GlobalUser.USER.Expire_status == 0)
            {
                Messenger.Default.Send(new NavigateMessage(ExpireCardVM.ViewName, null));
            }
            else
            {
                BindUserPapers();
            }

            MinimizeWindowCommand = new RelayCommand<Window>(MinimizeWindow);
        }

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


        private void LogoutEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                //var logoutResult = WebProxy(ApiType.UserLogout, GlobalUser.USER.AccessToken);//暂时 不用调用接口  移除token记录

                string userFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER);
                string userDataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER, $"user.dat");

                UserDataModel userDataModel = new UserDataModel
                {
                    UserName = GlobalUser.USER.UserName,
                    Phone = GlobalUser.USER.Moblie,
                    AccessToken = "",
                    Data = new UserInfo()
                };
                GlobalUser.USER = null;
                string userEncoded = Base64Provider.AESEncrypt(userDataModel.ToJsonItem(), Base64Provider.KEY, Base64Provider.IV);
                try
                {
                    if (!Directory.Exists(userFolder))
                    {
                        Directory.CreateDirectory(userFolder);
                    }
                    if (!File.Exists(userDataFile))
                    {
                        var file = File.Create(userDataFile);
                        file.Close();
                    }
                    using (StreamWriter sw = new StreamWriter(userDataFile, false))
                    {
                        sw.Write(userEncoded);
                    }
                }
                catch (Exception e)
                {
                    Log4NetHelper.Error("写入用户信息异常：", e);
                }

                var winLogin = new MainLogin();
                winLogin.Show();
                winLogin.Focus();

                GlobalUser.MainWin?.Hide();
                //GlobalUser.MainWin?.Close();
                GlobalUser.MainWin = null;

                //_metroWindow.Hide();
                //_metroWindow.Close();
                //_metroWindow = null;

            }));

        }

        /// <summary>
        /// 获取或设置用户信息。
        /// </summary>
        //public RespObject User { get; set; }


        private void OnNavigate(NavigateMessage message)
        {
            if (message.IsClear && message.TargetView == "ChildrenClear")
            {
                _mainContentPanel.Children.Clear();
            }
            else
            {
                if (views.ContainsKey(message.TargetView))
                {
                    this.CurrentView = views[message.TargetView];
                    this.currentViewName = message.TargetView;
                    //((IView)this.CurrentView.DataContext).Activated(message.State);
                    if (message.IsClear)
                        _mainContentPanel.Children.Clear();

                    _mainContentPanel.Children.Add(CurrentView);
                }
            }
        }

        public void BindUserPapers()
        {
            Messenger.Default.Send(new NavigateMessage("ChildrenClear", null));//清空重复

            // 异步请求，防止界面假死
            //Task.Run(() =>
            //{
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {

                var ml = new GetSimulationListModel()
                {
                    paper_type = 2,
                    pagesize = 20,
                    token= GlobalUser.USER.Token
                };

                var result = WebProxy(ml, ApiType.GetSimulationList, null, "get");
                
                if (result.retData.total > 0)
                {
                    List<SimulationItem> lsPapers =
                        JsonHelper.FromJson<List<SimulationItem>>(result.retData.list.ToString());

                    //Messenger.OverrideDefault();
                    if (lsPapers != null && lsPapers.Count > 0)
                    {
                        var sList = lsPapers;

                        for (int i = 0; i < sList.Count; i++)
                        {
                            string keyName = $"{sList[i].exam_id}_{sList[i].paper_title}_{i}";
                            sList[i].exam_id = $"{sList[i].exam_id}#{i + 1}";
                            SetupView(keyName, new PaperControl(), new PaperControlVM(sList[i]));

                            //SetupView(keyName, new PaperControl(), new PaperControlVM());
                            Messenger.Default.Send(new NavigateMessage(keyName, null, false));
                        }
                    }
                }
                else
                {
                    //无试卷可用
                    //todo
                    MessageBox.Show("无试卷可用!");
                }
            }));
            //});
        }

    }
}
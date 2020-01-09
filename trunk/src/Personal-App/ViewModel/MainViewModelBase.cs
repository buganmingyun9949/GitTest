using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Personal_App.Domain;
using ST.Models.Api;
using ST.Common.WebApi;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Personal_App.Common;
using MaterialDesignThemes.Wpf;
using ST.Common.ToolsHelper;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Data;
using System.Windows.Controls;
using Framework.Logging;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
using ST.Common;

namespace Personal_App.ViewModel
{
    public class MainViewModelBase : ViewModelBase// INotifyPropertyChanged
    {
        public MainViewModelBase()
        {
            //Task task = Task.Factory.StartNew(() => ListeningNetwork());
        }

        DispatcherTimer networkTimer = new DispatcherTimer();
        private void ListeningNetwork()
        {
            //networkTimer.Interval = TimeSpan.FromSeconds(1000);
            //networkTimer.Tick += networkTimer_Tick;
            //networkTimer.Start();
        }

        void networkTimer_Tick(object sender, EventArgs e)
        {
            if (!NetworkHelper.IsConnectInternet())
            {
                networkTimer.Stop();
                //MessageBox.Show("无网络连接");

                Messenger.Default.Send(new MainDialogMessage("无网络连接!"), "MainMessageDialog");
                //networkTimer.Stop();
                //var view = new NotNetworkDialog();
                //var dialog = DialogHostEx.ShowDialog(view, sender, networkTimerOpenedEventHandler);
            }
        }

        private void networkTimerOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }

        #region << 关闭 弹出 提示 >>

        public ICommand RunExtendedDialogCommand => new RelayCommand(ExecuteRunExtendedDialog);


        private void ExecuteRunExtendedDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new SignOutDialog()
            {
                DataContext = new SignOutDialogVM()
            };

            //show the dialog
            DialogHost.Show(view, o, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);

            //check the result...
            //Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
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
            //        TaskScheduler.FromCurrentSynchronizationContext());//关闭程序

            Task.Factory.StartNew(() => Thread.Sleep(1000))
                .ContinueWith(t => Environment.Exit(0), TaskScheduler.FromCurrentSynchronizationContext());
        }


        #endregion

        /// <summary>
        ///  Web 代理（提供 Api 请求操作）。
        /// </summary>
        /// <param name="apiType">Api 请求的类型。</param>
        /// <param name="accessToken">访问令牌。</param>
        /// <returns>Api 响应对象。</returns>
        public RespCode WebProxy(ApiType apiType, string accessToken = null, string _Method = "Post")
        {
            var result = WebApiProxy.GetHtmlRespInfo(apiType, accessToken, _Method);
            // Token失效    4001
            if (result?.retCode == 4001)
            {
                LoginFailure();
                //var windows = Application.Current.Windows;
                //var loginWondow = new Window();
                //var mainWondow = new Window();
                //foreach (Window window in windows)
                //{
                //    if (window.Name == "ml")
                //    {
                //        loginWondow = window;
                //    }
                //    if (window.Name == "MetroWindowMain")
                //    {
                //        mainWondow = window;
                //    }
                //}
                //GlobalUser.LoginWin = loginWondow;
                //GlobalUser.LoginWin.Show();
                //GlobalUser.LoginWin.Focus();

                ////DialogHost.CloseAllShow();

                ////mainWondow.Hide();
                ////mainWondow.Close();

                ////GlobalUser.MainWin?.Hide();
                ////GlobalUser.MainWin?.Close();
                ////GlobalUser.MainWin = null;

                //GlobalUser.LoginExpired = true;
                //GlobalUser.AutoLoggedIn = false;
                //GlobalUser.USER = null;
                //LoginExpired(loginWondow);
            }
            return result;
        }

        /// <summary>
        /// Web 代理（提供 Api 请求操作）。
        /// </summary>
        /// <typeparam name="T">Api 请求的参数对象类型。</typeparam>
        /// <param name="model">Api 请求的参数对象。</param>
        /// <param name="apiType">Api 请求的类型。</param>
        /// <param name="accessToken">访问令牌。</param>
        /// <returns>Api 响应对象。</returns>
        public RespCode WebProxy<T>(T model, ApiType apiType, string accessToken = null, string _Method = "Post")
        {
            var result = WebApiProxy.GetHtmlRespInfo(model, apiType, accessToken, _Method);
            // 验证错误码
            if (result?.retCode == 4001)
            {
                LoginFailure();
            }

            return result;
        }

        /// <summary>
        /// Web 代理（提供 Api 请求操作）。
        /// </summary>
        /// <typeparam name="T">Api 请求的参数对象类型。</typeparam>
        /// <param name="model">Api 请求的参数对象。</param>
        /// <param name="apiType">Api 请求的类型。</param>
        /// <param name="accessToken">访问令牌。</param>
        /// <returns>Api 响应对象。</returns>
        public RespCode WebProxy(string filrPath, ApiType apiType, string accessToken = null, string _Method = "Post")
        {
            var result = WebApiProxy.GetHtmlFileRespInfo(filrPath, apiType, accessToken, _Method);
            // 验证错误码
            if (result?.retCode == 4001)
            {
                LoginFailure();
                //return;
            }

            return result;
        }

        public void LoginFailure()
        {
            var windows = Application.Current.Windows;
            var loginWondow = new Window();
            var mainWondow = new Window();
            foreach (Window window in windows)
            {
                if (window.Name == "ml")
                {
                    loginWondow = window;
                }

                if (window.Name == "MetroWindowMain")
                {
                    mainWondow = window;
                }
            }

            mainWondow.Hide();
            //mainWondow.Close();
            GlobalUser.MainWin?.Hide();

            DialogHost.CloseAllShow();
            //GlobalUser.MainWin?.Close();
            mainWondow = null;
            GlobalUser.MainWin = null;
            ClearMemory();
            GlobalUser.LoginWin = loginWondow;
            GlobalUser.LoginWin.DataContext = new MainLoginVM(false);
            GlobalUser.LoginWin.Show();
            GlobalUser.LoginWin.Focus();

            GlobalUser.LoginExpired = true;
            GlobalUser.AutoLoggedIn = false;
            //GlobalUser.USER = null;
            LoginExpired(loginWondow);
        }

        /// <summary>
        /// 登录过期。
        /// </summary>
        private void LoginExpired(Window window)
        {
            LoginExpiredDialog loginExpiredDialog = new LoginExpiredDialog();
            DialogHostEx.ShowDialog(window, loginExpiredDialog, LoginExpiredOpenedEventHandler);
        }

        private void LoginExpiredOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {

            //Task.Delay(TimeSpan.FromSeconds(3))
            //.ContinueWith(
            //  (t, _) =>
            //  {
            //      eventArgs.Session.Close();

            //  }, null, TaskScheduler.FromCurrentSynchronizationContext());//关闭程序


            Task.Factory.StartNew(() => Thread.Sleep(3000))
                .ContinueWith(t =>
                {
                    if (eventArgs.Session == null || eventArgs.Session.IsEnded == true)
                    {
                        GlobalUser.LoginWin.Focus();
                        return;
                    }

                    eventArgs.Session.Close();

                    GlobalUser.LoginWin.Focus();

                }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        public void MessageDialogClose(object sender, DialogOpenedEventArgs eventArgs)
        {

            //Task.Delay(TimeSpan.FromSeconds(3))
            //.ContinueWith(
            //  (t, _) =>
            //  {
            //      eventArgs.Session.Close();

            //  }, null, TaskScheduler.FromCurrentSynchronizationContext());//关闭程序


            Task.Factory.StartNew(() => Thread.Sleep(4000))
                .ContinueWith(t =>
                {
                    eventArgs.Session.Close();

                    GlobalUser.LoginWin?.Focus();

                }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        /// <summary>
        /// 注销登录提示。
        /// </summary>
        private void Logout()
        {

        }



        #region 用户信息弹出窗口...

        /// <summary>
        /// 打开用户信息对话框命令。
        /// </summary>
        public ICommand OpenUserInfoDialogCommand => new RelayCommand(OpenUserInfoDialog);

        /// <summary>
        /// 打开用户信息对话框。
        /// </summary>
        /// <param name="o"></param>
        private async void OpenUserInfoDialog(object o)
        {
            var userInfo = GlobalUser.USER;
            var cardInfo = GlobalUser.STUDYCARD;
            var userInfoDialogVM = new UserInfoDialogVM();

            if (userInfo != null)
            {
                //userInfoDialogVM.User = GlobalUser.USER;
                //userInfoDialogVM.UserName = GlobalUser.USER.UserName;
                if (cardInfo != null)
                {
                    //cardInfo.Validity = $"{Convert.ToDateTime(GlobalUser.STUDYCARD.expire_time).ToString("yyyy年MM月dd日")} 到期";
                }
            }
            else
            {
                MessageBox.Show("获取用户信息失败，请重新登录！");
            }
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new UserInfoDialog(true)
            {
                DataContext = userInfoDialogVM,
                Height = 500,
                Width = 370
            };

            #region 验证用户头像是否存在...
            //GlobalUser.USER.Avatar = "http://www.baid2u.com/123.png";
            if (!string.IsNullOrEmpty(GlobalUser.USER.Avatar))
            {
                //GlobalUser.USER.Avatar = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "head.png");
                view.AvatarImage.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.head);
            }

            #endregion
            //show the dialog
            await DialogHostEx.ShowDialog(GlobalUser.MainWin, view, UserInfoDialogOpenedEventHandler,
                UserInfoDialogClosingEventHandler);

            //check the result...
            //Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }
        private void UserInfoDialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }

        private void UserInfoDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            var userInfoDialog = eventArgs.Session.Content as UserInfoDialog;
            var dataContent = userInfoDialog.DataContext as UserInfoDialogVM;

            var newName = userInfoDialog.txtUsername.Text;
            var oldName = dataContent.UserName;

            if ((bool)eventArgs.Parameter)
            {
                if (string.IsNullOrWhiteSpace(newName))
                {
                    userInfoDialog.rtbErrBox.Visibility = Visibility.Visible;
                    userInfoDialog.errMsg.Content = "请输入您的用户名";
                    eventArgs.Cancel();
                }
                else
                {
                    userInfoDialog.rtbErrBox.Visibility = Visibility.Collapsed;

                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {

                        ModifyUserInfoModel modifyUserInfoModel = new ModifyUserInfoModel()
                        {
                            user_name = newName
                        };

                        var reqResult = WebProxy(modifyUserInfoModel, ApiType.ModifyUserInfo, GlobalUser.USER.Token);

                        if (reqResult?.retCode == 0)
                        {
                            GlobalUser.USER.UserName = newName;
                            //userInfoDialog.txtUsername.Text = newName;                    
                            RememberUser();
                            userInfoDialog.errMsg.Content = "保存成功";
                        }
                        else
                        {
                            //MessageDialogVM messageDialogVM = new MessageDialogVM()
                            //{
                            //    MessageContent = "保存失败！"
                            //};
                            //eventArgs.Session.UpdateContent(new MessageDialog());

                            userInfoDialog.rtbErrBox.Visibility = Visibility.Visible;
                            userInfoDialog.errMsg.Content = "保存失败";
                            eventArgs.Cancel();
                        }
                    }));
                }
            }
            else
            {
                userInfoDialog.txtUsername.Text = oldName;
            }
        }

        public void RememberUser()
        {
            string userFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER);
            string userDataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER, $"user.dat");
            UserDataModel userDataModel = new UserDataModel
            {
                UserName = GlobalUser.USER.UserName,
                Phone = GlobalUser.USER.Mobile,
                Avatar = GlobalUser.USER.Avatar,
                AccessToken = GlobalUser.USER.Token,
                Password = GlobalUser.USER.Password,
                Data = GlobalUser.USER,
                ClassData = GlobalUser.CLASSINFO,
                UserZy = GlobalUser.USER.UserZy,
                StudyCard = GlobalUser.STUDYCARD,
                GradeData = GlobalUser.GRADEINFO,
                UnFirstOpen = GlobalUser.USER.UnFirstOpen
            };

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
                    file.Dispose();
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
        }

        #endregion

        #region 窗口关闭...

        public ICommand WindowClosingCommand => new RelayCommand(WindowClosing);

        private async void WindowClosing(object obj)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new SignOutDialog()
            {
                DataContext = new SignOutDialogVM()
            };

            //show the dialog
            await DialogHost.Show(view, (object)"MainDialog", WindowClosingEventHandler, WindowClosingEventHandler);

            //check the result...
            //Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }

        private void WindowClosingEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
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
            //        TaskScheduler.FromCurrentSynchronizationContext());//关闭程序

            Task.Factory.StartNew(() => Thread.Sleep(1000))
                .ContinueWith(t => Environment.Exit(0), TaskScheduler.FromCurrentSynchronizationContext());
        }


        #endregion

        private RelayCommand runWindowMinimizeCommand;//下载 试卷

        public RelayCommand RunWindowMinimizeCommand
        {
            get
            {
                return runWindowMinimizeCommand ?? (runWindowMinimizeCommand = new RelayCommand(
                           (action) =>
                           {
                               var win = action as Window;
                               if (win.WindowState != WindowState.Minimized)
                               {
                                   win.WindowState = WindowState.Minimized;
                               }
                           }));
            }
        }
        //public event PropertyChangedEventHandler PropertyChanged;

        //internal Action<PropertyChangedEventArgs> RaisePropertyChanged()
        //{
        //    return args => PropertyChanged?.Invoke(this, args);
        //}
        //internal virtual void RaisePropertyChanged(string propertyName)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        //}


        //[DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        //public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>    
        /// 释放内存    
        /// </summary>    
        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                //SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

    }

}
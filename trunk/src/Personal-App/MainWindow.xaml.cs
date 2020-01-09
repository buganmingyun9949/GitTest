using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ST.Common.ToolsHelper;
using Personal_App.Common;
using Personal_App.ViewModel;
using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using Personal_App.Domain;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Threading;
using Framework.Logging;
using FSLib.App.SimpleUpdater;
using GalaSoft.MvvmLight.Messaging;
using ST.Common;
using ST.Common.WebApi;
using ST.Models.Api;

namespace Personal_App
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //更新 初始化
        private Updater updater;
        public MainWindow()
        {
            InitializeComponent();


            //TopLogoImage.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.logo1);

            //TopBgImage.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.topbg2);

        }

        private void MainWindow_OnActivated(object sender, EventArgs e)
        {
        }

        private void MainLogin_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;

            //let's set up a little MVVM, cos that's what the cool kids are doing:
            //var view = new Domain.SignOutDialog()
            //{
            //    DataContext = new SignOutDialogVM()
            //};

            //var result = DialogHostEx.ShowDialog(this, view, MainWindowClosingEventHandler);
            Environment.Exit(0);
        }

        private void MainWindowClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            //OK, lets cancel the close...
            eventArgs.Cancel();

            //...now, lets update the "session" with some new content!
            eventArgs.Session.UpdateContent(new Domain.SampleProgressDialog());
            //note, you can also grab the session when the dialog opens via the DialogOpenedEventHandler

            //lets run a fake operation for 3 seconds then close this baby.
            //Task.Delay(TimeSpan.FromSeconds(1))
            //    .ContinueWith((t, _) => Environment.Exit(0), null,
            //        TaskScheduler.FromCurrentSynchronizationContext()); // 关闭程序

            Task.Factory.StartNew(() => Thread.Sleep(1000))
                .ContinueWith(t => Environment.Exit(0), TaskScheduler.FromCurrentSynchronizationContext());
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            ChangeWindowSize changeWindowSize = new ChangeWindowSize(this);
            changeWindowSize.RegisterHook();
        }

        private void BtnLogOff_Click(object sender, RoutedEventArgs e)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new LogoffDialog()
            {

            };

            var result = DialogHostEx.ShowDialog(this, view, LogOffEventHandler);
        }

        private void LogOffEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;

            string userFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER);
            string userDataFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER, $"user.dat");

            UserDataModel userDataModel = new UserDataModel
            {
                UserName = GlobalUser.USER.UserName,
                Phone = GlobalUser.USER.Mobile,
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

            //Application.Current.Dispatcher.Invoke(() =>
            //{
            //    var cardResult = WebProxy(ApiType.UserLogout, GlobalUser.USER.AccessToken);
            //});

            var winLogin = new MainLogin();
            winLogin.Show();
            winLogin.Focus();
            Hide();
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            if (!GlobalUser.LoginExpired)
            {
                var view = new SignOutDialog()
                {
                    DataContext = new SignOutDialogVM()
                };

                var result = DialogHostEx.ShowDialog(this, view, MainWindowClosingEventHandler);
            }
        }

        //private void ChbTopMast_OnClick(object sender, RoutedEventArgs e)
        //{
        //    if (ChbTopMast.IsChecked == true)
        //    {
        //        this.Topmost = true;
        //    }
        //    else
        //    {
        //        this.Topmost = false;
        //    }
        //}
        //private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (MenuListBox.SelectedIndex == 0)
        //    {

        //    }
        //    else if (MenuListBox.SelectedIndex == 1)
        //    {

        //    }
        //    else if (MenuListBox.SelectedIndex == 2)
        //    {

        //    }
        //}
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //ViewModelLocator.MainWindow = ViewModelLocator.MainWindow == null
            //    ? new MainWindowVM(this, MainContentPanel)
            //    : ViewModelLocator.MainWindow;
            var vm = ViewModelLocator.MainWindow;
            MenuListBox.SelectedItem = null;
            MenuListBox.SelectedIndex = -1;
            vm.LoadMainWindowVm(this, MainContentPanel);

            this.DataContext = vm;//new MainWindowVM(this, MainContentPanel);

            var ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            //txtAppVersion.Text = $"V{ver}";

            this.TxtAPPName.Text = $"英语说  V{ver}";

            //更新 初始化
            updater = Updater.Instance;

            //当检查发生错误时,这个事件会触发 
            updater.Error += updater_Error;
            //没有找到更新的事件
            updater.NoUpdatesFound += updater_NoUpdatesFound;
            //找到更新的事件.但在此实例中,找到更新会自动进行处理,所以这里并不需要操作
            //updater.UpdatesFound += new EventHandler(updater_UpdatesFound);
            //开始检查更新-这是最简单的模式.请现在 assemblyInfo.cs 中配置更新地址,参见对应的文件.
            //Updater.CheckUpdateSimple();
            if (GlobalUser.AutoLoggedIn)
            {
                Updater.CheckUpdateSimple(Properties.Settings.Default.UpdateUrl,
                    Properties.Settings.Default.UpdateFileName); //http://192.168.0.2/xyy-yys.xml
            }

            //monitorIntert.Tick += MonitorIntertOnTick;
            //monitorIntert.Interval = new TimeSpan(0, 0, 0, 30);
            //monitorIntert.Start();

        }

        void updater_Error(object sender, EventArgs e)
        {
            if ((!GlobalUser.AutoLoggedIn || IsCheckUpdateBtn) && this.IsActive)
            {
                IsCheckUpdateBtn = false;
                var updater = sender as FSLib.App.SimpleUpdater.Updater;
                //System.Windows.Forms.MessageBox.Show("更新服务连接失败");
                Messenger.Default.Send(new MainDialogMessage("没有找到可更新的版本!"), "MainMessageDialog");
                //Log4NetHelper.ErrorFormat($"updater_Error: {updater.Context.Exception.Message}");
            }
        }

        void updater_NoUpdatesFound(object sender, EventArgs e)
        {
            if ((!GlobalUser.AutoLoggedIn || IsCheckUpdateBtn) && this.IsActive)
            {
                IsCheckUpdateBtn = false;
                var updater = sender as FSLib.App.SimpleUpdater.Updater;
                //System.Windows.Forms.MessageBox.Show("没有找到更新");
                Messenger.Default.Send(new MainDialogMessage("当前已经是最新版本啦!"), "MainMessageDialog");
                //Log4NetHelper.Error($"updater_NoUpdatesFound: {updater.Context.Exception.Message}");
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //ChkTopmast.IsChecked = false;

            /*
             * 如果您希望更加简单的使用而不用去加这样的属性，或者您想程序运行的时候自定义，您可以通过下列方式的任何一种方式取代上面的属性声明：
             * 使用Updater.CheckUpdateSimple 的重载方法。这个重载方法允许你传入一个升级包的地址；
             * 在检查前手动设置 FSLib.App.SimpleUpdater.Updater.UpdateUrl 属性。这是一个静态属性，也就是说，您并不需要创建 FSLib.App.SimpleUpdater.Updater.UpdateUrl 的对象实例就可以修改它。
             */
            IsCheckUpdateBtn = true;
            Updater.CheckUpdateSimple(Properties.Settings.Default.UpdateUrl,
                Properties.Settings.Default.UpdateFileName); //http://192.168.0.2/xyy-yys.xml
        }

        private bool IsCheckUpdateBtn = false;

        #region << 网络监测 >>

        private DispatcherTimer monitorIntert = new DispatcherTimer();

        private int showErrIntentCount = 0;

        private void MonitorIntertOnTick(object sender, EventArgs e)
        {
            string[] urls = new string[] {WebApiProxy.APIURL, WebApiProxy.MEDIAURL, "www.baidu.com"};
            CheckServeStatus(urls);
        }


        /// <summary>
        /// 检测网络连接状态
        /// </summary>
        /// <param name="urls"></param>
        public void CheckServeStatus(string[] urls)
        {
            int errCount = 0;//ping时连接失败个数

            string errMsg = "";

            if (!LocalConnectionStatus())
            {
                Console.WriteLine("网络异常~无连接");
                errMsg = "网络异常~无连接";
            }
            else if (!MyPing(urls, out errCount))
            {
                if ((double)errCount / urls.Length >= 0.3)
                {
                    Console.WriteLine("网络异常~连接多次服务器无响应");
                    errMsg = "网络异常~连接多次服务器无响应";
                }
                else
                {
                    Console.WriteLine("网络不稳定");
                    errMsg = "网络不稳定";
                }
            }
            else
            {
                Console.WriteLine("网络正常");
                errMsg = "网络正常";

                showErrIntentCount = 0;

                return;
            }

            if (!string.IsNullOrEmpty(errMsg) && showErrIntentCount == 0)
            {
                showErrIntentCount = 1;
                //Messenger.Default.Send(new MainDialogMessage(errMsg), "MainMessageDialog");

            }

        }

        private const int INTERNET_CONNECTION_MODEM = 1;
        private const int INTERNET_CONNECTION_LAN = 2;

        [DllImport("winInet.dll")]
        private static extern bool InternetGetConnectedState(ref int dwFlag, int dwReserved);

        /// <summary>
        /// 判断本地的连接状态
        /// </summary>
        /// <returns></returns>
        private bool LocalConnectionStatus()
        {
            System.Int32 dwFlag = new Int32();
            if (!InternetGetConnectedState(ref dwFlag, 0))
            {
                Console.WriteLine("LocalConnectionStatus--未连网!");
                return false;
            }
            else
            {
                if ((dwFlag & INTERNET_CONNECTION_MODEM) != 0)
                {
                    Console.WriteLine("LocalConnectionStatus--采用调制解调器上网。");
                    return true;
                }
                else if ((dwFlag & INTERNET_CONNECTION_LAN) != 0)
                {
                    Console.WriteLine("LocalConnectionStatus--采用网卡上网。");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Ping命令检测网络是否畅通
        /// </summary>
        /// <param name="urls">URL数据</param>
        /// <param name="errorCount">ping时连接失败个数</param>
        /// <returns></returns>
        public bool MyPing(string[] urls, out int errorCount)
        {
            bool isconn = true;
            Ping ping = new Ping();
            errorCount = 0;
            try
            {
                PingReply pr;
                for (int i = 0; i < urls.Length; i++)
                {
                    pr = ping.Send(urls[i]);
                    if (pr.Status != IPStatus.Success)
                    {
                        isconn = false;
                        errorCount++;
                    }
                    Console.WriteLine("Ping " + urls[i] + "    " + pr.Status.ToString());
                }
            }
            catch
            {
                isconn = false;
                errorCount = urls.Length;
            }
            //if (errorCount > 0 && errorCount < 3)
            //  isconn = true;
            return isconn;
        }

        public int GetNetStatus()
        {
            Ping ping;
            PingReply ret;
            ping = new Ping();
            try
            {
                //ret = ping.Send("www.baidu.com");
                ret = ping.Send(WebApiProxy.APIURL);

                if (ret.Status != IPStatus.Success)
                {
                    //没网
                    return 1;
                }
                else
                {
                    //有网
                    return 0;
                }
            }
            catch (Exception err)
            {
                //MessageBox.Show("获取网络状态异常：" + err.ToString());
                //MessageBox.Show("获取网络状态异常");
                return 1;
            }
        }


        #endregion
    }
}

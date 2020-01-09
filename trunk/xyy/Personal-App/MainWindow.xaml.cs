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
using System.Threading;
using Framework.Logging;
using ST.Common;
using ST.Models.Api;

namespace Personal_App
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            //TopLogoImage.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.logo1);

            //TopBgImage.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.topbg2);

            this.DataContext = new MainWindowVM(this, MainContentPanel);

            var ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            txtAppVersion.Text = $"V{ver.Major}.{ver.Minor}.{ver.Build}";

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
            //ChangeWindowSize changeWindowSize = new ChangeWindowSize(this);
            //changeWindowSize.RegisterHook();
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
    }
}

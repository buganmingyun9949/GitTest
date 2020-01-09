using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using ST.Common.ToolsHelper;
using Personal_App.Common;
using Personal_App.ViewModel;
using MaterialDesignThemes.Wpf;
using Personal_App.Domain;
using System.ComponentModel;
using System.Threading;
using Framework.Logging;
using FSLib.App.SimpleUpdater;
using GalaSoft.MvvmLight.Messaging;

namespace Personal_App
{
    /// <summary>
    /// MainLogin.xaml 的交互逻辑
    /// </summary>
    public partial class MainLogin : Window
    {
        public MainLogin()
        {
            InitializeComponent();

            //LoginImage.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.loginpic);

            //FlipView2Nd.ItemsSource =
            //    new []
            //    {
            //        ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.loginpic)
            //    };
            //new Uri[]
            //{
            //    new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory), UriKind.RelativeOrAbsolute),
            //};

            //DataContext = new MainLoginVM();
            //if (GlobalUser.AutoLoggedIn)
            //{
            //    Hide();
            //}
        }

        /// <summary>
        /// 手机号短信验证码输入验证(只能输入数字)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                || (e.Key >= Key.D0 && e.Key <= Key.D9)
                || e.Key == Key.Left || e.Key == Key.Right
                || e.Key == Key.Back || e.Key == Key.Tab
                )
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 手机号短信验证码输入验证屏蔽空格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUsername_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        /// <summary>
        /// 图形验证码输入验证(只能输入数字，英文)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtImageCode_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                || (e.Key >= Key.D0 && e.Key <= Key.D9)
                || e.Key == Key.Left || e.Key == Key.Right
                || e.Key == Key.Back || e.Key == Key.Tab
                || (e.Key >= Key.A && e.Key <= Key.Z)
                )
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 图形验证码输入验证屏蔽空格
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtImageCode_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void MainLogin_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        //private MessageDialogResult ShowModalMessageConfirmExternal(string v1, string v2, MessageDialogStyle affirmativeAndNegative, MetroDialogSettings mySettings, int v3)
        //{
        //    return MessageDialogResult.Negative;
        //}

        //private async void ShowDialogOutside(object sender, RoutedEventArgs e)
        //{
        //    var dialog = (BaseMetroDialog)this.Resources["CustomDialogTest"];
        //    dialog.DialogSettings.ColorScheme = MetroDialogColorScheme.Theme;
        //    dialog = dialog.ShowDialogExternally();

        //    await Task.Delay(5000);

        //    await dialog.RequestCloseAsync();
        //}

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            ChangeWindowSize changeWindowSize = new ChangeWindowSize(this);
            changeWindowSize.RegisterHook();
        }


        private void MainLogin_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;

            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new SignOutDialog()
            {
                DataContext = new SignOutDialogVM()
            };

            var result = DialogHostEx.ShowDialog(this, view, MainLoginClosingEventHandler);
        }

        private void MainLoginClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
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

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            var view = new SignOutDialog()
            {
                DataContext = new SignOutDialogVM()
            };

            var result = DialogHostEx.ShowDialog(this, view, MainLoginClosingEventHandler);
        }

        private void MainLogin_OnLoaded(object sender, RoutedEventArgs e)
        {

            //更新 初始化
            var updater = Updater.Instance;

            //当检查发生错误时,这个事件会触发 
            updater.Error += updater_Error;
            //没有找到更新的事件
            updater.NoUpdatesFound += updater_NoUpdatesFound;
            //找到更新的事件.但在此实例中,找到更新会自动进行处理,所以这里并不需要操作
            //updater.UpdatesFound += new EventHandler(updater_UpdatesFound);
            //开始检查更新-这是最简单的模式.请现在 assemblyInfo.cs 中配置更新地址,参见对应的文件.
            //Updater.CheckUpdateSimple();

            /*
             * 如果您希望更加简单的使用而不用去加这样的属性，或者您想程序运行的时候自定义，您可以通过下列方式的任何一种方式取代上面的属性声明：
             * 使用Updater.CheckUpdateSimple 的重载方法。这个重载方法允许你传入一个升级包的地址；
             * 在检查前手动设置 FSLib.App.SimpleUpdater.Updater.UpdateUrl 属性。这是一个静态属性，也就是说，您并不需要创建 FSLib.App.SimpleUpdater.Updater.UpdateUrl 的对象实例就可以修改它。
             */
            Updater.CheckUpdateSimple(Properties.Settings.Default.UpdateUrl,
                Properties.Settings.Default.UpdateFileName); //http://192.168.0.2/xyy-yys.xml
        }

        static void updater_Error(object sender, EventArgs e)
        {
            var updater = sender as FSLib.App.SimpleUpdater.Updater;
            //System.Windows.Forms.MessageBox.Show("更新服务连接失败");
            //Messenger.Default.Send(new MainDialogMessage("没有找到可更新的版本!"), "MainMessageDialog");
            //Log4NetHelper.ErrorFormat($"updater_Error: {updater.Context.Exception.Message}");
        }

        static void updater_NoUpdatesFound(object sender, EventArgs e)
        {
            var updater = sender as FSLib.App.SimpleUpdater.Updater;
            //System.Windows.Forms.MessageBox.Show("没有找到更新");
            //Messenger.Default.Send(new MainDialogMessage("当前已经是最新版本啦!"), "MainMessageDialog");
            //Log4NetHelper.Error($"updater_NoUpdatesFound: {updater.Context.Exception.Message}");
        }
    }
}

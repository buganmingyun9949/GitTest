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

            DataContext = new MainLoginVM();
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
            //ChangeWindowSize changeWindowSize = new ChangeWindowSize(this);
            //changeWindowSize.RegisterHook();
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
    }
}

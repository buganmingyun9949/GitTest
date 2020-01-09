using Personal_App.ViewModel;
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
//using System.Windows.Shapes;
using ST.Models.Api;
using ST.Common.WebApi;
using Personal_App.Common;
using System.IO;
using MaterialDesignThemes.Wpf;
using ST.Common;
using ST.Common.ToolsHelper;

namespace Personal_App.Domain
{
    /// <summary>
    /// UserInfoDialog.xaml 的交互逻辑
    /// </summary>
    public partial class UserInfoDialog : UserControl
    {
        public UserInfoDialog(bool isBind = false)
        {
            InitializeComponent();

            if (isBind)
            {
                Binding binding = new Binding()
                {
                    //Source = newName,
                    Path = new PropertyPath("User.UserName"),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
                BindingOperations.SetBinding(txtUsername, TextBox.TextProperty, binding);
            }
            else
            {
                this.txtUsername.Text = GlobalUser.USER?.UserName;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //var userInfoDialog = eventArgs.Session.Content as UserInfoDialog;

            //var dataContent = userInfoDialog.DataContext as UserInfoDialogVM;

            //ModifyUserNameModel modifyUserNameModel = new ModifyUserNameModel()
            //{
            //    newUserName = dataContent.UserName
            //};
            //DependencyObject
            //var reqResult = WebApiProxy.GetHtmlRespInfo(modifyUserNameModel, ApiType.ModifyUserName, GlobalUser.USER.AccessToken);

            //if ((bool)eventArgs.Parameter == false) return;

            ////OK, lets cancel the close...
            //eventArgs.Cancel();

            ////...now, lets update the "session" with some new content!
            //eventArgs.Session.UpdateContent(new SampleProgressDialog());
            ////note, you can also grab the session when the dialog opens via the DialogOpenedEventHandler

            ////lets run a fake operation for 3 seconds then close this baby.
            //Task.Delay(TimeSpan.FromSeconds(1))
            //    .ContinueWith((t, _) => Environment.Exit(0), null,
            //        TaskScheduler.FromCurrentSynchronizationContext());//关闭程序

        }

        private void txtUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.rtbErrBox != null)
            {
                if (txtUsername.Text.Trim().Length > 0)
                {
                    this.rtbErrBox.Visibility = Visibility.Collapsed;
                }
                else
                {
                    this.rtbErrBox.Visibility = Visibility.Visible;
                }
            }
        }
    }
}

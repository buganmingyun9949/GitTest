using GalaSoft.MvvmLight.Messaging;
using ST.Models.Api;
using Personal_App.Domain;
using ST.Common.WebApi;
using ST.Common;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Personal_App.ViewModel
{
    /// <summary>
    /// 用户账户对象模型。
    /// </summary>
    public class UserInfoDialogVM : MainViewModel
    {
        private Visibility _editModeStatus;

        private Visibility _saveButtonStatus;

        private Visibility _userNameStatus;

        /// <summary>
        /// 初始化 <see cref="UserInfoDialogVM"/> 类的新实例。
        /// </summary>
        public UserInfoDialogVM()
        {
            UserNameStatus = Visibility.Visible;
            EditModeStatus = Visibility.Collapsed;
            SaveButtonStatus = Visibility.Collapsed;

            UserName = GlobalUser.USER.UserName;

            //if (GlobalUser.USER.Expire_status == 1)
            //{
            //    CardStatus = "正常";
            //}
            //else if (GlobalUser.USER.Expire_status == 0)
            //{
            //    CardStatus = "过期";
            //}
            //else
            //{
            //    CardStatus = "尚未绑定学习卡";
            //}

            User = GlobalUser.USER;
            RaisePropertyChanged("User");

            if (GlobalUser.USER.Card != null)
            {
                CardStatus = $"{GlobalUser.USER.Card.card_key}";

                GradeName = $"{GlobalUser.USER.Card.grade}年级";

                ValidityTime = $"{Convert.ToDateTime(GlobalUser.USER.Card.expire_time).ToString("yyyy年MM月dd日")} 到期";
            }
            else
            {
                CardStatus = "尚未绑定学习卡";
                GradeName = "尚未绑定学习卡";
                ValidityTime = "尚未绑定学习卡";
            }
        }

        public DependencyObject DependencyObject;

        private UserInfo _User;
        /// <summary>
        /// 获取或设置用户信息。
        /// </summary>
        public UserInfo User
        {
            get
            {
                return _User;
            }
            set
            {
                _User = value;
                RaisePropertyChanged("User");
            }
        }

        /// <summary>
        /// 有效期。
        /// </summary>
        public string Validity { get; set; }

        #region Btn Command...

        #region 编辑用户按钮...

        public ICommand EditUserButtonCommand => new RelayCommand(EditUserInfoCommand);

        /// <summary>
        /// 编辑用户信息事件。
        /// </summary>
        /// <param name="o"></param>
        private void EditUserInfoCommand(object o)
        {
            // 显示
            if (UserNameStatus == Visibility.Visible)
            {
                UserNameStatus = Visibility.Hidden;
                EditModeStatus = Visibility.Visible;
                SaveButtonStatus = Visibility.Visible;
            }
            // 编辑
            else
            {
                UserNameStatus = Visibility.Visible;
                EditModeStatus = Visibility.Hidden;
                SaveButtonStatus = Visibility.Hidden;
            }
        }

        private RelayCommand userEditButtonCommand;
        /// <summary>
        /// 编辑按钮命令。
        /// </summary>
        public RelayCommand EditUserCommand
        {
            get
            {
                return userEditButtonCommand ?? (userEditButtonCommand = new RelayCommand(
                        (action) =>
                           {
                               // 显示
                               if (UserNameStatus == Visibility.Visible)
                               {
                                   UserNameStatus = Visibility.Hidden;
                                   EditModeStatus = Visibility.Visible;
                                   SaveButtonStatus = Visibility.Visible;
                               }
                               // 编辑
                               else
                               {
                                   UserNameStatus = Visibility.Visible;
                                   EditModeStatus = Visibility.Hidden;
                                   SaveButtonStatus = Visibility.Hidden;
                               }
                           })
                        );
            }
        }



        #endregion

        //private RelayCommand userSaveBtnCommand;

        ///// <summary>
        ///// 保存按钮命令。
        ///// </summary>
        //public RelayCommand UserSaveBtnCommand
        //{
        //    get
        //    {
        //        return userSaveBtnCommand ?? (userSaveBtnCommand = new RelayCommand(
        //                async (action) =>
        //                   {
        //                       //var result = await DialogHost.Show(new SampleProgressDialog(), ClosingEventHandler);
        //                       //显示
        //                       var view = new SampleProgressDialog();
        //                       //打开 对话框
        //                       //DialogHost.Show(view, ExtendedClosingEventHandler);
        //                       var result = await DialogHost.Show(view, action, ClosingEventHandler);

        //                       Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        //                   })
        //                );
        //    }
        //}

        //private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        //{
        //    if ((bool)eventArgs.Parameter == false) return;

        //    //OK, lets cancel the close...
        //    eventArgs.Cancel();

        //    //...now, lets update the "session" with some new content!
        //    eventArgs.Session.UpdateContent(new SampleProgressDialog());
        //}

        #region 保存用户信息...

        public ICommand SaveUserButtonCommand => new RelayCommand(SaveUserCommand);

        private void SaveUserCommand(object o)
        {

            // 异步请求，防止界面假死
            //await Task.Run(() =>
            //{
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                ModifyUserInfoModel modifyUserInfoModel = new ModifyUserInfoModel()
                {
                    user_name = UserName
                };

                var reqResult = WebProxy(modifyUserInfoModel, ApiType.ModifyUserInfo, GlobalUser.USER.AccessToken);

                if (reqResult?.retCode == 0)
                {
                    UserNameStatus = Visibility.Visible;
                    EditModeStatus = Visibility.Hidden;
                    SaveButtonStatus = Visibility.Hidden;
                }
                else
                {
                    UserNameStatus = Visibility.Hidden;
                    EditModeStatus = Visibility.Visible;
                    SaveButtonStatus = Visibility.Visible;
                }
            }));
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new SampleProgressDialog();

            //show the dialog
            DialogHost.Show(view, o, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
        }

        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {

            Task.Factory.StartNew(() => Thread.Sleep(1000))
                .ContinueWith(t => {
                    eventargs.Session.Close();

                    //跳转到 列表
                    Messenger.Default.Send(new NavigateMessage(ExpireCardVM.ViewName, null));
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {

        }

        #endregion

        #endregion

        #region EventHandler...

        #endregion

        private string _username;

        /// <summary>
        /// 用户名。
        /// </summary>
        public string UserName
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                RaisePropertyChanged("UserName");
            }
        }

        /// <summary>
        /// 编辑显示状态。
        /// </summary>
        public Visibility UserNameStatus
        {
            get
            {
                return _userNameStatus;
            }
            set
            {
                _userNameStatus = value;
                RaisePropertyChanged("UserNameStatus");
            }
        }
        /// <summary>
        /// 编辑显示状态。
        /// </summary>
        public Visibility EditModeStatus
        {
            get
            {
                return _editModeStatus;
            }
            set
            {
                _editModeStatus = value;
                RaisePropertyChanged("EditModeStatus");
            }
        }

        /// <summary>
        /// 保存按钮显示状态。
        /// </summary>
        public Visibility SaveButtonStatus
        {
            get
            {
                return _saveButtonStatus;
            }
            set
            {
                _saveButtonStatus = value;
                RaisePropertyChanged("SaveButtonStatus");
            }
        }



        private string _CardStatus;
        /// <summary>
        /// 卡片状态。
        /// </summary>
        public string CardStatus
        {
            get
            {
                return _CardStatus;
            }
            set
            {
                _CardStatus = value;
                RaisePropertyChanged("CardStatus");
            }
        }

        private string _GradeName;
        /// <summary>
        /// 年级。
        /// </summary>
        public string GradeName
        {
            get
            {
                return _GradeName;
            }
            set
            {
                _GradeName = value;
                RaisePropertyChanged("GradeName");
            }
        }

        private string _ValidityTime;
        /// <summary>
        ///  有效期。
        /// </summary>
        public string ValidityTime
        {
            get
            {
                return _ValidityTime;
            }
            set
            {
                _ValidityTime = value;
                RaisePropertyChanged("ValidityTime");
            }
        }

    }
}

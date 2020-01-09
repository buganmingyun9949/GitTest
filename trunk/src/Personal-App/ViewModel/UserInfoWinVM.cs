using GalaSoft.MvvmLight.Messaging;
using ST.Models.Api;
using Personal_App.Domain;
using ST.Common.WebApi;
using ST.Common;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Personal_App.ViewModel.Menu;
using ST.Common.ToolsHelper;

namespace Personal_App.ViewModel
{
    /// <summary>
    /// 用户账户对象模型。
    /// </summary>
    public class UserInfoWinVM : MainViewModel
    {

        public const string ViewName = "UserInfoWin";

        /// <summary>
        /// 初始化 <see cref="UserInfoDialogVM"/> 类的新实例。
        /// </summary>
        public UserInfoWinVM()
        {
            UserName = GlobalUser.USER.UserName;

            User1 = GlobalUser.USER;

            UserPhoneInfo =
                $"您当前验证手机:{User.Mobile.Substring(0, 3) + "***" + User.Mobile.Substring(7)} 可使用此手机号登录，找回密码";
            if (GlobalUser.STUDYCARD != null)
            {
                CardStatus = $"{GlobalUser.STUDYCARD.card_key?.ToUpper()}";

                GradeName = $"{GlobalUser.STUDYCARD.grade}年级";

                ValidityTime = $"({Convert.ToDateTime(GlobalUser.STUDYCARD.expire_time).ToString("yyyy年MM月dd日 hh时mm分ss秒")} 到期)";
            }
            else
            {
                CardStatus = "尚未绑定学习卡";
                GradeName = "尚未绑定学习卡";
                ValidityTime = "尚未绑定学习卡";
            }
        }

        #region << 属性 字段 >>

        private int dialogNum = -1;

        public DependencyObject DependencyObject;

        private UserInfo _User1;
        /// <summary>
        /// 获取或设置用户信息。
        /// </summary>
        public UserInfo User1
        {
            get
            {
                return _User1;
            }
            set
            {
                _User1 = value;
                UserPhoneInfo =
                    $"您当前验证手机:{_User1.Mobile.Substring(0, 3) + "***" + _User1.Mobile.Substring(7)} 可使用此手机号登录，找回密码";
                RaisePropertyChanged("User");
            }
        }

        /// <summary>
        /// 有效期。
        /// </summary>
        public string Validity { get; set; }

        private string _UserPhoneInfo;
        /// <summary>
        /// 有效期。
        /// </summary>
        public string UserPhoneInfo
        {
            get
            {
                return _UserPhoneInfo;
            }
            set
            {
                _UserPhoneInfo = value;
                RaisePropertyChanged("UserPhoneInfo");
            }
        }

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


        #endregion

        #region << Btn Command  >>
        
        private RelayCommand _BtnLoginoutCmd;//注销

        public RelayCommand BtnLoginoutCmd
        {
            get
            {
                return _BtnLoginoutCmd ?? (_BtnLoginoutCmd = new RelayCommand(
                           (action) =>
                           {
                               //打开班级列表
                               //show the dialog 
                               Loginout();
                           }));
            }
        }

        private RelayCommand _BindingChangeNameCmd;//绑定 修改姓名

        public RelayCommand BindingChangeNameCmd
        {
            get
            {
                return _BindingChangeNameCmd ?? (_BindingChangeNameCmd = new RelayCommand(
                           (action) =>
                           {
                               //打开班级列表
                               //show the dialog
                               ExecuteRunNameExtendedDialog(action);
                           }));
            }
        }

        private RelayCommand _BtnReBindingCardCmd;//重新绑定学习卡

        public RelayCommand BtnReBindingCardCmd
        {
            get
            {
                return _BtnReBindingCardCmd ?? (_BtnReBindingCardCmd = new RelayCommand(
                           (action) =>
                           {
                               //打开绑卡
                               //show the dialog
                               ExecuteReBindingCardExtendedDialog(action);
                           }));
            }
        }

        private RelayCommand _BindingChangePhoneCmd;//绑定 修改手机号

        public RelayCommand BindingChangePhoneCmd
        {
            get
            {
                return _BindingChangePhoneCmd ?? (_BindingChangePhoneCmd = new RelayCommand(
                           (action) =>
                           {
                               //打开班级列表
                               //show the dialog
                               ExecuteRunPhoneExtendedDialog(action);

                           }));
            }
        }

        private RelayCommand _BindingChangePwdCmd;//绑定 修改密码

        public RelayCommand BindingChangePwdCmd
        {
            get
            {
                return _BindingChangePwdCmd ?? (_BindingChangePwdCmd = new RelayCommand(
                           (action) =>
                           {
                               //打开班级列表
                               //show the dialog
                               ExecuteRunPwdExtendedDialog(action);

                           }));
            }
        }

        private RelayCommand _BtnSelectNewHeadCmd;//绑定 修改头像

        public RelayCommand BtnSelectNewHeadCmd
        {
            get
            {
                return _BtnSelectNewHeadCmd ?? (_BtnSelectNewHeadCmd = new RelayCommand(
                           (action) =>
                           {
                               OpenFileDialog openFile = new OpenFileDialog();
                               openFile.Title = "选择一个新头像";
                               openFile.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.gif";
                               openFile.FileName = string.Empty;
                               openFile.FilterIndex = 1;
                               openFile.RestoreDirectory = true;
                               openFile.DefaultExt = "jpg";

                               if (openFile.ShowDialog() == true)
                               {
                                   //Image img = new Image();
                                   //BitmapImage bi = new BitmapImage();
                                   //bi.BeginInit();
                                   //bi.UriSource = new Uri(openFile.FileName, UriKind.RelativeOrAbsolute);
                                   //bi.EndInit();

                                   if (string.IsNullOrEmpty(openFile.FileName))
                                   {
                                       //ErrMsgOut = "无效的姓名！";
                                       Messenger.Default.Send(new MainDialogMessage("无效的头像"), "MainMessageDialog");
                                   }
                                   else
                                   {

                                       var reqResult = WebProxy(openFile.FileName, ApiType.ModifyUserHead,
                                           GlobalUser.USER.Token);

                                       if (reqResult?.retCode == 0)
                                       {
                                           string newPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data",
                                               $"{Guid.NewGuid().ToString()}{Path.GetExtension(openFile.FileName)}");


                                           ModifyUserInfoModel modifyUserInfoModel = new ModifyUserInfoModel()
                                           {
                                               user_avatar = reqResult.retData.static1,
                                           };

                                           reqResult = WebProxy(modifyUserInfoModel, ApiType.ModifyUserInfo,
                                               GlobalUser.USER.Token);
                                           if (reqResult?.retCode == 0)
                                           {
                                               File.Copy(openFile.FileName, newPath);

                                               GlobalUser.USER.Avatar = newPath;
                                               //userInfoDialog.txtUsername.Text = newName;                    
                                               RememberUser();
                                               User = GlobalUser.USER;

                                               Messenger.Default.Send(
                                                   new NavigateMessage(UserInfoWinVM.ViewName, "R", true),
                                                   "ShowUserpapers");

                                               //Messenger.Default.Send(new MainDialogMessage(reqResult.retMsg),
                                               //    "MainMessageDialog");
                                           }
                                           else
                                           {
                                               Messenger.Default.Send(new MainDialogMessage(reqResult.retMsg),
                                                   "MainMessageDialog");
                                           }
                                       }
                                       else
                                       {
                                           Messenger.Default.Send(new MainDialogMessage("连接服务器失败,请检查您的网络"),
                                               "MainMessageDialog");
                                       }
                                   }
                               }
                           }));
            }
        }


        #endregion

        #region << 自定义 方法 >>

        private async void ExecuteRunNameExtendedDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new UserChangeName();
            var viewVM = new UserChangeNameVM(view,"");
            view.DataContext = viewVM;
            //show the dialog
            //await DialogHost.Show(view, o, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
            await DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);

            dialogNum = 1;
        }

        private async void ExecuteRunPhoneExtendedDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new UserChangePhone();
            var viewVM = new UserChangePhoneVM(view,  "");
            view.DataContext = viewVM;
            //show the dialog
            //await DialogHost.Show(view, o, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
            await DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);

            dialogNum = 2;
        }

        private async void ExecuteRunPwdExtendedDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new UserChangePwd();
            var viewVM = new UserChangePwdVM(view,  "");
            view.DataContext = viewVM;
            //show the dialog
            //await DialogHost.Show(view, o, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
            await DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);

            dialogNum = 3;
        }

        private async void ExecuteReBindingCardExtendedDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new UserReBandingCard();
            //show the dialog
            //await DialogHost.Show(view, o, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
            await DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler, ExtendedReBandingClosingEventHandler);

            dialogNum = 4;
        }

        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {

            //Task.Factory.StartNew(() => Thread.Sleep(1000))
            //    .ContinueWith(t => {
            //        eventargs.Session.Close();

            //        //跳转到 列表
            //        Messenger.Default.Send(new NavigateMessage(ExpireCardVM.ViewName, null));
            //    }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter != null)
            {
                if ((bool)eventArgs.Parameter)
                {
                    Messenger.Default.Send(new NavigateMessage(UserInfoWinVM.ViewName, "R", true), "ShowUserpapers");
                }
            }
        }

        private void ExtendedReBandingClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter != null)
            {
                //if ((Int32) eventArgs.Parameter == 5)
                //{
                //    var view = new BindingCard();
                //    view.DataContext = new BindingCardVM(view, true);
                //    Messenger.Default.Send(new NavigateMessage(BindingCardVM.ViewName, "N", view, true),
                //        "ShowUserpapers");
                //}

                if (eventArgs.Parameter != null)
                {
                    if ((Int32) eventArgs.Parameter == 1)
                    {
                        //打开 支付宝 二维码  
                        var view = new UserPayQRCodeDialog(1);
                        //打开 对话框
                        eventArgs.Session.UpdateContent(view);
                        eventArgs.Cancel();
                    }
                    else if ((Int32) eventArgs.Parameter == 2)
                    {
                        //打开 微信 二维码
                        var view = new UserPayQRCodeDialog(2);
                        //打开 对话框
                        eventArgs.Session.UpdateContent(view);
                        eventArgs.Cancel();
                    }
                    else if ((Int32) eventArgs.Parameter == 3)
                    {
                        //打开 学习卡 绑定 
                        Messenger.Default.Send(new NavigateMessage(BindingCardVM.ViewName, null), "ShowUserpapers");
                    }
                    else if ((Int32) eventArgs.Parameter == 4 || (Int32) eventArgs.Parameter == 6)
                    {
                        if ((Int32)eventArgs.Parameter == 6)
                        {
                            return;
                        }
                        //var view = new BindingCard();
                        //view.DataContext = new BindingCardVM(view, true);
                        //Messenger.Default.Send(new NavigateMessage(BindingCardVM.ViewName, "N", view, true),
                        //    "ShowUserpapers");
                        var userResult = WebProxy(ApiType.UserInfo, GlobalUser.USER.Token);
                        if (!string.IsNullOrEmpty(userResult.retData.study_card.ToString()))
                        {

                            if (GlobalUser.STUDYCARD?.card_key == userResult.retData.study_card?.card_key.ToString())
                            {

                                var view0 = new BindNewCardErrDialog();
                                eventArgs.Session.UpdateContent(view0);
                                eventArgs.Cancel();

                                return;
                            }

                            GlobalUser.STUDYCARD = new Study_Card
                            {
                                used_time = userResult.retData.study_card?.used_time,
                                card_key = userResult.retData.study_card?.card_key,
                                expire_status = userResult.retData.study_card?.expire_status,
                                expire_time = userResult.retData.study_card?.expire_time,
                                agent_id = userResult.retData.study_card?.agent_id,
                                grade = userResult.retData.study_card?.grade,
                                card_type = userResult.retData.study_card?.card_type,
                                card_auth = userResult.retData.study_card?.card_auth,
                                card_price = userResult.retData.study_card?.card_price,
                                card_name = userResult.retData.study_card?.card_name,
                                card_setting = userResult.retData.study_card?.card_setting
                            };
                        }

                        // 绑定成功,卡片内容
                        //GlobalUser.USER.Card.card_key = GlobalUser.STUDYCARD.card_key;
                        //GlobalUser.USER.Card.used_time = GlobalUser.STUDYCARD.used_time;
                        //GlobalUser.USER.Card.expire_time = GlobalUser.STUDYCARD.expire_time;

                        User = GlobalUser.USER;
                        Validity = Convert.ToDateTime(GlobalUser.STUDYCARD?.expire_time)
                            .ToString("yyyy年MM月dd日 HH时mm分 到期");

                        RememberUser();

                        var view = new BindCardOKDialog()
                        {
                            DataContext = new BindCardOKVM()
                            {
                                CardNo = GlobalUser.STUDYCARD.card_key,
                                CardName = $"{GlobalUser.STUDYCARD.grade}年级",
                                Validity =
                                    $"{Convert.ToDateTime(GlobalUser.STUDYCARD.used_time).ToString("yyyy年MM月dd日")} - {Convert.ToDateTime(GlobalUser.STUDYCARD.expire_time).ToString("yyyy年MM月dd日")}"
                            }
                        };

                        eventArgs.Session.UpdateContent(view);
                        eventArgs.Cancel();

                        Messenger.Default.Send(new NavigateMessage(null, "NewCard", true), "ShowUserpapers");
                    }
                    else if ((Int32) eventArgs.Parameter == 5)
                    {
                        int card_auth_num = Convert.ToInt32(GlobalUser.STUDYCARD.card_auth);
                        if ((card_auth_num & (1 << 2)) > 0 ) //可在线续费
                        {
                            var view = new UserPayBoxDialog();
                            //var result = DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedClosing2PayEventHandler);

                            eventArgs.Session.UpdateContent(view);
                            eventArgs.Cancel();
                            return;
                        }
                        else if ((card_auth_num & (1 << 1)) > 0 || card_auth_num == 0) //可绑卡续费
                        {
                            //打开 学习卡 绑定 
                            Messenger.Default.Send(new NavigateMessage(BindingCardVM.ViewName, null), "ShowUserpapers");
                        }
                    }
                    else
                    {
                        //关闭
                    }
                }
            }
        }

        #endregion

    }
}

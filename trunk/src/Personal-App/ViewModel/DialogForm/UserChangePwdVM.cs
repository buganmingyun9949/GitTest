
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using Personal_App.Common;
using Personal_App.Domain;
using MaterialDesignThemes.Wpf;
using ST.Common;
using ST.Models.Paper;
using Card = ST.Models.Api.Card;

namespace Personal_App.ViewModel
{
    public class UserChangePwdVM : MainViewModel, IView
    {
        public void Activated(object state)
        {
            throw new NotImplementedException();
        }

        public const string ViewName = "UserChangePwd";

        public UserChangePwdVM()
        {
            ErrMsgOut = "";

            User = GlobalUser.USER;
        }

        public UserChangePwdVM(UserControl uc, string phoneNum):this()
        {
            this._uc = uc as UserChangePwd;
        }

        #region << 字段 属性 >>

        private UserChangePwd _uc;

        private WrapPanel _classPanel;

        private string _showUserPhone;

        public string ShowUserPhone
        {
            get
            {
                return _showUserPhone;
            }
            set
            {
                if (string.IsNullOrEmpty(GlobalUser.USER?.Mobile))
                    _showUserPhone = User.Mobile.Substring(0, 3) + "***" + User.Mobile.Substring(7);
                RaisePropertyChanged("ShowUserPhone");
            }
        }

        private string _errMsgOut;

        public string ErrMsgOut
        {
            get
            {
                return _errMsgOut;
            }
            set
            {
                _errMsgOut = value;
                RaisePropertyChanged("ErrMsgOut");
            }
        }

        private string _TxtUserPhoneCode;

        /// <summary>
        /// 验证码
        /// </summary>
        public string TxtUserPhoneCode
        {
            get
            {
                return _TxtUserPhoneCode;
            }
            set
            {
                _TxtUserPhoneCode = value;
                RaisePropertyChanged("TxtUserPhoneCode");
            }
        }

        private string _TxtUserNewPwd;

        /// <summary>
        /// 新密码
        /// </summary>
        public string TxtUserNewPwd
        {
            get
            {
                return _TxtUserNewPwd;
            }
            set
            {
                _TxtUserNewPwd = value;
                RaisePropertyChanged("TxtUserNewPwd");
            }
        }

        PhoneVerificationCode pvc = new PhoneVerificationCode();

        /// <summary>
        /// 验证码
        /// </summary>
        public PhoneVerificationCode Pvc
        {
            get { return pvc; }

            set
            {
                pvc = value;
                RaisePropertyChanged("Pvc");
            }
        }


        #endregion

        #region << Btn Command >>

        private RelayCommand _ChangeCmd;//加入 更新

        public RelayCommand ChangeCmd
        {
            get
            {
                return _ChangeCmd ?? (_ChangeCmd = new RelayCommand(
                           (action) =>
                           {
                               //打开班级列表
                               //show the dialog
                               ExecuteRunExtendedDialog(action);
                           }));
            }
        }

        private RelayCommand sendCode;//获取短信验证码

        public RelayCommand SendCode
        {
            get
            {
                return sendCode ?? (sendCode = new RelayCommand(
                           () =>
                           {
                               if (!string.IsNullOrEmpty(GlobalUser.USER.Mobile))
                               {
                                   pvc.GetCode(GlobalUser.USER.Mobile);

                                   SendCodeModel lm = new SendCodeModel()
                                   {
                                       phone = GlobalUser.USER.Mobile
                                   };
                                   // 异步获取验证码
                                   Application.Current.Dispatcher.Invoke(new Action(() =>
                                   {
                                       var result = WebProxy(lm, ApiType.SendCode);
                                       if (result != null)
                                       {
                                           if (result.retCode == 0)
                                           {
                                               ErrMsgOut = "";
                                           }
                                           else
                                           {
                                               ErrMsgOut = result.retMsg;
                                               Pvc.ClearCode();
                                           }
                                       }
                                       else
                                       {
                                           ErrMsgOut = "与服务器通讯失败，请稍候重试！";
                                       }
                                   }));
                               }
                               else
                               {
                                   ErrMsgOut = "手机号不能为空！";
                               }

                           }));
            }
        }

        #endregion

        #region << 自定义 >>

        private async void ExecuteRunExtendedDialog(object o)
        {
            bool isPass = true;

            if (string.IsNullOrEmpty(TxtUserNewPwd))
            {
                ErrMsgOut = "无效的密码！";
                isPass = false;
            }

            if (string.IsNullOrEmpty(TxtUserPhoneCode))
            {
                ErrMsgOut = "无效的验证码！";
                isPass = false;
            }


            if (isPass)
            {
                UserPwdFindModel lm = new UserPwdFindModel()
                {
                    phone = GlobalUser.USER.Mobile,
                    password = TxtUserNewPwd,
                    phone_code = TxtUserPhoneCode,
                };

                var reqResult = WebProxy(lm, ApiType.UserPwdFind);

                if (reqResult?.retCode == 0)
                {
                    //GlobalUser.USER.Mobile = TxtUserNewPwd;                 
                    RememberUser();
                    ErrMsgOut = "更新成功";
                    //_uc.CurrentCloseBtn.CommandParameter = true;

                    await Task.Factory.StartNew(() => Thread.Sleep(333))
                        .ContinueWith(t =>
                        {
                            _uc.CurrentCloseBtn.CommandParameter = true;
                            ButtonAutomationPeer peer =
                                new ButtonAutomationPeer(_uc.CurrentCloseBtn);

                            IInvokeProvider invokeProv =
                                peer.GetPattern(PatternInterface.Invoke)
                                    as IInvokeProvider;
                            invokeProv.Invoke();

                        }, TaskScheduler.FromCurrentSynchronizationContext());
                }
                else
                {
                    ErrMsgOut = reqResult.retMsg; //"更新失败";
                }
            }
        }

        #endregion
    }
}
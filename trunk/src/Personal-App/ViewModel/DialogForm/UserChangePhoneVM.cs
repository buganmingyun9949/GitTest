
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
    public class UserChangePhoneVM : MainViewModel, IView
    {
        public void Activated(object state)
        {
            throw new NotImplementedException();
        }

        public const string ViewName = "UserChangePhone";

        public UserChangePhoneVM()
        {
            ErrMsgOut = "";

            User = GlobalUser.USER;
        }

        public UserChangePhoneVM(UserControl uc, string phoneNum):this()
        {

            this._uc = uc as UserChangePhone;
        }

        #region << 字段 属性 >>

        private UserChangePhone _uc;

        private WrapPanel _classPanel;

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

        private string _TxtUserPhone;

        /// <summary>
        /// 新手机号
        /// </summary>
        public string TxtUserPhone
        {
            get
            {
                return _TxtUserPhone;
            }
            set
            {
                _TxtUserPhone = value;
                RaisePropertyChanged("TxtUserPhone");
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

                               if (GlobalUser.USER.Mobile == TxtUserPhone)
                               {
                                   ErrMsgOut = "新手机号与原手机号相同！";
                                   return;
                               }

                               if (!RegexHelper.IsHandset(TxtUserPhone))
                               {
                                   ErrMsgOut = "请输入有效手机号！";
                                   return;
                               }

                               if (!string.IsNullOrEmpty(TxtUserPhone))
                               {
                                   pvc.GetCode(TxtUserPhone);
                                   
                                   UserPwdFindModel lm = new UserPwdFindModel()
                                   {
                                       phone = TxtUserPhone,
                                       role = 1
                                   };

                                   var reqResult1 = WebProxy(lm, ApiType.CheckUserPhone, null);

                                   string retData1 = reqResult1?.retData.ToString();

                                   if (reqResult1?.retCode == 0 &&
                                       (retData1.ToLower().Contains("check") && !retData1.Contains("1")))
                                   {

                                       SendCodeModel lm1 = new SendCodeModel()
                                       {
                                           phone = TxtUserPhone
                                       };
                                       // 异步获取验证码
                                       Application.Current.Dispatcher.Invoke(new Action(() =>
                                       {
                                           var result = WebProxy(lm1, ApiType.SendCode);
                                           if (result != null)
                                           {
                                               if (result.retCode == 0)
                                               {
                                                   ErrMsgOut = "";
                                               }
                                               else
                                               {
                                                   ErrMsgOut = result.retMsg;
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
                                       if (reqResult1.retCode == 40400)
                                           ErrMsgOut = reqResult1.retMsg;
                                       else
                                           ErrMsgOut = Properties.Settings.Default.UsedPhoneNum;
                                   }
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

            if (string.IsNullOrEmpty(TxtUserPhone)|| !RegexHelper.IsHandset(TxtUserPhone))
            {
                ErrMsgOut = "请输入有效手机号！";
                isPass = false;
                return;
            }

            if (GlobalUser.USER.Mobile == TxtUserPhone)
            {
                ErrMsgOut = "新手机号与原手机号相同！";
                isPass = false;
                return;
            }
            if (string.IsNullOrEmpty(TxtUserPhoneCode))
            {
                ErrMsgOut = "无效的验证码！";
                isPass = false;
                return;
            }


            if (isPass)
            {
                UserPwdFindModel lm = new UserPwdFindModel()
                {
                    phone = TxtUserPhone,
                    role = 1
                };

                var reqResult1 = WebProxy(lm, ApiType.CheckUserPhone, null);

                string retData1 = reqResult1?.retData.ToString();

                if (reqResult1?.retCode == 0 && (retData1.ToLower().Contains("check") && !retData1.Contains("1")))
                {
                    UserPwdFindModel lm1 = new UserPwdFindModel()
                    {
                        phone = TxtUserPhone,
                        phone_code = TxtUserPhoneCode
                    };

                    var reqResult = WebProxy(lm1, ApiType.ModifyUserPhone, GlobalUser.USER.Token);

                    if (reqResult?.retCode == 0)
                    {
                        GlobalUser.USER.Mobile = TxtUserPhone;
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
                        ErrMsgOut = reqResult.retMsg;
                    }
                }
                else
                {
                    ErrMsgOut = Properties.Settings.Default.UsedPhoneNum;
                }
            }
        }

        #endregion
    }
}
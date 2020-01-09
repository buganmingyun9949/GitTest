
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
    public class UserChangeNameVM : MainViewModel, IView
    {
        public void Activated(object state)
        {
            throw new NotImplementedException();
        }

        public const string ViewName = "UserChangeName";

        public UserChangeNameVM()
        {
            ErrMsgOut = "";

            TxtUserName = GlobalUser.USER.UserName;
        }

        public UserChangeNameVM(UserControl uc, string phoneNum):this()
        {
            this._uc = uc as UserChangeName;
        }

        #region << 字段 属性 >>

        private UserChangeName _uc;

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

        private string _TxtUserName;

        /// <summary>
        /// 新名字
        /// </summary>
        public string TxtUserName
        {
            get
            {
                return _TxtUserName;
            }
            set
            {
                _TxtUserName = value;
                RaisePropertyChanged("TxtUserName");
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

        #endregion

        #region << 自定义 >>


        private async void ExecuteRunExtendedDialog(object o)
        {
            if (string.IsNullOrEmpty(TxtUserName))
            {
                ErrMsgOut = "无效的姓名！";
            }
            else
            {
                ModifyUserInfoModel modifyUserInfoModel = new ModifyUserInfoModel()
                {
                    user_name = TxtUserName
                };

                var reqResult = WebProxy(modifyUserInfoModel, ApiType.ModifyUserInfo, GlobalUser.USER.Token);

                if (reqResult?.retCode == 0)
                {
                    GlobalUser.USER.UserName = TxtUserName;
                    //userInfoDialog.txtUsername.Text = newName;                    
                    RememberUser();
                    ErrMsgOut = "更新成功";
                    //_uc.CurrentCloseBtn.CommandParameter = true;

                    //Thread.Sleep(2000);
                    //_uc.CurrentCloseBtn.Click

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
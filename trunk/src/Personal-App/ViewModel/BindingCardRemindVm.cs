using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using Personal_App.Common;
using Personal_App.Domain;
using MaterialDesignThemes.Wpf;
using Personal_App.ViewModel.Menu;
using ST.Common;
using Card = ST.Models.Api.Card;

namespace Personal_App.ViewModel
{
    public class BindingCardRemindVM : MainViewModel, IView
    {
        public const string ViewName = "BindingCardRemindVM";

        public BindingCardRemindVM()
        {
            ShowBackBtn = Visibility.Collapsed;
        }


        public BindingCardRemindVM(BindingCardRemind bindingCard, bool reBanding = false) : this()
        {
            _BindingCard = bindingCard;

            if (reBanding)
            {
                ShowBackBtn = Visibility.Visible;
            }
        }

        #region << 属性 字段 >>


        public delegate void ChangeCardHandler(string cardValidity);
        //定义事件
        public event ChangeCardHandler ChangeCardEvent;

        //定义事件
        public BindingCardRemind _BindingCard;

        private string pattern = @"^[a-zA-Z0-9-]{4}(\-)[a-zA-Z0-9-]{4}(\-)[a-zA-Z0-9-]{4}(\-)[a-zA-Z0-9-]{4}$";

        private bool BandingOK = false;

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


        private Visibility _ShowBackBtn;

        /// <summary>
        /// 显示返回按钮
        /// </summary>
        public Visibility ShowBackBtn
        {
            get
            {
                return _ShowBackBtn;
            }
            set
            {
                _ShowBackBtn = value;
                RaisePropertyChanged("ShowBackBtn");
            }
        }

        #endregion

        #region << Btn Command >>

        private RelayCommand _BindingCardReminCmd;//绑定新卡

        public RelayCommand BindingCardReminCmd => _BindingCardReminCmd ?? (_BindingCardReminCmd = new RelayCommand(
                           (action) =>
                           {
                               //打开绑卡
                               //show the dialog
                               ExecuteReBindingCardExtendedDialog(action);
                           }));


        private RelayCommand backCmd;//返回按钮

        public RelayCommand BackCmd
        {
            get
            {
                return backCmd ?? (backCmd = new RelayCommand(
                           (action) =>
                           {
                               Messenger.Default.Send(
                                   new NavigateMessage(UserInfoWinVM.ViewName, BandingOK ? "R" : "", true),
                                   "ShowUserpapers");
                           }));
            }
        }
        #endregion

        #region << 自定义方法 >>

        public void Activated(object state)
        {
            throw new NotImplementedException();
        }

        private async void ExecuteReBindingCardExtendedDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new UserReBandingCard();
            //show the dialog
            //await DialogHost.Show(view, o, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
            await DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler, ExtendedBandingClosingEventHandler);

        }

        private async void ExecuteRunExtendedDialog(object o)
        {
        }
        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
        }

        private void ExtendedBandingClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
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
                    if ((Int32)eventArgs.Parameter == 1)
                    {
                        //打开 支付宝 二维码  
                        var view = new UserPayQRCodeDialog(1);
                        //打开 对话框
                        eventArgs.Session.UpdateContent(view);
                        eventArgs.Cancel();
                    }
                    else if ((Int32)eventArgs.Parameter == 2)
                    {
                        //打开 微信 二维码
                        var view = new UserPayQRCodeDialog(2);
                        //打开 对话框
                        eventArgs.Session.UpdateContent(view);
                        eventArgs.Cancel();
                    }
                    else if ((Int32)eventArgs.Parameter == 3)
                    {
                        //打开 学习卡 绑定 
                        Messenger.Default.Send(new NavigateMessage(BindingCardVM.ViewName, null), "ShowUserpapers");
                    }
                    else if ((Int32)eventArgs.Parameter == 4 || (Int32)eventArgs.Parameter == 6)
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
                        Validity = Convert.ToDateTime(GlobalUser.STUDYCARD?.expire_time).ToString("yyyy年MM月dd日 HH时mm分 到期");

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

                        //Messenger.Default.Send(new NavigateMessage(MenuExamVM.ViewName, "R", true), "ShowUserpapers");
                        Messenger.Default.Send(new NavigateMessage(null, "NewCard", true), "ShowUserpapers");
                    }
                    else if ((Int32)eventArgs.Parameter == 5)
                    {
                        int card_auth_num = Convert.ToInt32(GlobalUser.STUDYCARD.card_auth);
                        if ((card_auth_num & (1 << 2)) > 0) //可在线续费
                        {
                            var view = new UserPayBoxDialog();
                            //var result = DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedClosing2PayEventHandler);

                            eventArgs.Session.UpdateContent(view);
                            eventArgs.Cancel();
                            return;
                        }
                        //else if ((card_auth_num & (1 << 1)) > 0) //可绑卡续费
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

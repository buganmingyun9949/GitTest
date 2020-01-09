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
    public class BindingCardVM : MainViewModel, IView
    {
        public const string ViewName = "BindingCardVM";

        public BindingCardVM()
        {
            ShowBackBtn = Visibility.Collapsed;
        }


        public BindingCardVM(BindingCard bindingCard, bool reBanding = false) : this()
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
        public BindingCard _BindingCard;

        private string pattern = @"^[a-zA-Z0-9-]{4}(\-)[a-zA-Z0-9-]{4}(\-)[a-zA-Z0-9-]{4}(\-)[a-zA-Z0-9-]{4}$";

        private bool BandingOK = false;

        private string _txtCardNo1;
        private string _txtCardNo2;
        private string _txtCardNo3;
        private string _txtCardNo4;

        private string _errMsgOut;

        public string TxtCardNo1
        {
            get
            {
                return _txtCardNo1;
            }
            set
            {
                _txtCardNo1 = value;
                if (string.IsNullOrEmpty(_txtCardNo1 + _txtCardNo2 + _txtCardNo3 + _txtCardNo4))
                {
                    ErrMsgOut = "";
                }
                RaisePropertyChanged("TxtCardNo1");
            }
        }
        public string TxtCardNo2
        {
            get
            {
                return _txtCardNo2;
            }
            set
            {
                _txtCardNo2 = value;
                if (string.IsNullOrEmpty(_txtCardNo1 + _txtCardNo2 + _txtCardNo3 + _txtCardNo4))
                {
                    ErrMsgOut = "";
                }
                RaisePropertyChanged("TxtCardNo2");
            }
        }
        public string TxtCardNo3
        {
            get
            {
                return _txtCardNo3;
            }
            set
            {
                _txtCardNo3 = value;
                if (string.IsNullOrEmpty(_txtCardNo1 + _txtCardNo2 + _txtCardNo3 + _txtCardNo4))
                {
                    ErrMsgOut = "";
                }
                RaisePropertyChanged("TxtCardNo3");
            }
        }
        public string TxtCardNo4
        {
            get
            {
                return _txtCardNo4;
            }
            set
            {
                _txtCardNo4 = value;
                if (string.IsNullOrEmpty(_txtCardNo1 + _txtCardNo2 + _txtCardNo3 + _txtCardNo4))
                {
                    ErrMsgOut = "";
                }
                RaisePropertyChanged("TxtCardNo4");
            }
        }

        private string _TxtCardNum;

        public string TxtCardNum
        {
            get { return _TxtCardNum; }
            set
            {
                if (!string.IsNullOrEmpty(value) || !string.IsNullOrWhiteSpace(value))
                {
                    value = value.Replace("-", "").Trim();

                    if (value.Length >= 4)
                    {
                        value = value.Substring(0, 4) + "-" + value.Substring(4);
                    }

                    if (value.Length >= 9)
                    {
                        value = value.Substring(0, 9) + "-" + value.Substring(9);
                        ;
                    }

                    if (value.Length >= 14)
                    {
                        value = value.Substring(0, 14) + "-" + value.Substring(14);
                        ;
                    }
                }

                if (value.Length > 19)
                    value = value.Substring(0, 19);
                _TxtCardNum = value;
                RaisePropertyChanged("TxtKeyNum");
                _BindingCard.txtKeyNum.SelectionStart = _TxtCardNum.Length;
            }
        }

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

        private RelayCommand bindingCardCmd;//绑定新卡

        public RelayCommand BindingCardCmd => bindingCardCmd ?? (bindingCardCmd = new RelayCommand(
                           (action) =>
                           {
                               if (TxtCardNum == null || TxtCardNum.Split('-').Length != 4 || TxtCardNum.Length != 19)
                               {
                                   ErrMsgOut = Properties.Settings.Default.ErrorCardNum;
                                   return;
                               }

                               Regex reg = new Regex(pattern);
                               if (!reg.IsMatch(TxtCardNum))
                               {
                                   ErrMsgOut = Properties.Settings.Default.ErrorCardNum;
                                   return;
                               }

                               // 异步请求，防止界面假死
                               //Task.Run(() =>
                               //{
                               Application.Current.Dispatcher.Invoke(new Action(() =>
                                   {
                                       UserBindCardModel model = new UserBindCardModel()
                                       {
                                           card_key = TxtCardNum// $"{TxtCardNo1}-{TxtCardNo2}-{TxtCardNo3}-{TxtCardNo4}"
                                       };

                                       //开始绑定
                                       var result = WebApiProxy.GetHtmlRespInfo(model, ApiType.UserBindCard, GlobalUser.USER.Token);

                                       if (result.retCode == 0)
                                       {
                                           ErrMsgOut = "";
                                           // 绑定成功,卡片内容
                                           //GlobalUser.USER.Card = new Card()
                                           //{
                                           //    card_id = result.retData.card_id,
                                           //    card_key = result.retData.card_key,
                                           //    is_used = result.retData.is_used,
                                           //    is_current = result.retData.is_current,
                                           //    used_time = result.retData.used_time,
                                           //    used_user_id = result.retData.used_user_id,
                                           //    expire_time = result.retData.expire_time,
                                           //    grade = result.retData.grade,
                                           //    ctime = result.retData.ctime,
                                           //    card_type = result.retData.card_type,
                                           //    expire_status = 1
                                           //};
                                           var status0 = GlobalUser.STUDYCARD?.expire_status ?? 0;
                                           GlobalUser.STUDYCARD = new Study_Card
                                           {
                                               used_time = result.retData.used_time,
                                               card_key = result.retData.card_key,
                                               expire_status =
                                                   (Convert.ToDateTime(result.retData.expire_time) > DateTime.Now)
                                                       ? 1
                                                       : status0, //result.retData.expire_status,
                                               expire_time = result.retData.expire_time,
                                               agent_id = result.retData.agent_id,
                                               grade = result.retData.grade,
                                               card_type = result.retData.card_type,
                                               card_auth = result.retData.card_auth,
                                               card_price = result.retData.card_price,
                                               card_name = result.retData.card_name,
                                               card_setting = result.retData.card_setting
                                           };

                                           GlobalUser.USER.Expire_status = GlobalUser.STUDYCARD.expire_status;

                                           User = GlobalUser.USER;
                                           Validity = Convert.ToDateTime(GlobalUser.STUDYCARD?.expire_time).ToString("yyyy年MM月dd日 HH时mm分 到期");
                                           RaisePropertyChanged("Validity");
                                           RememberUser();
                                           BandingOK = true;

                                           Messenger.Default.Send(new NavigateMessage(null, "NewCard", true), "ShowUserpapers");
                                           // 显示 卡片内容
                                           ExecuteRunExtendedDialog(action);

                                       }
                                       else
                                       {
                                           //绑定失败
                                           ErrMsgOut = result.retMsg;
                                       }
                                   }));
                               //});
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


        private async void ExecuteRunExtendedDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new BindCardOKDialog()
            {
                DataContext = new BindCardOKVM()
                {
                    CardNo = GlobalUser.STUDYCARD.card_key,
                    CardName = $"{GlobalUser.STUDYCARD.grade}年级",
                    Validity = $"{Convert.ToDateTime(GlobalUser.STUDYCARD.used_time).ToString("yyyy年MM月dd日")} - {Convert.ToDateTime(GlobalUser.STUDYCARD.expire_time).ToString("yyyy年MM月dd日")}"
                }
            };

            // 绑卡后更新用户菜单有效期
            ChangeCardEvent?.Invoke(GlobalUser.STUDYCARD.expire_time);
            //show the dialog
            await DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler);
        }
        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            //lets run a fake operation for 5 seconds then close this baby.
            //Task.Delay(TimeSpan.FromSeconds(3))
            //    .ContinueWith(
            //        (t, _) =>
            //        {
            //            eventargs.Session.Close();

            //            //跳转到 列表
            //            Messenger.Default.Send(new NavigateMessage(), "ShowUserpapers");
            //        }, null,
            //        TaskScheduler.FromCurrentSynchronizationContext());//关闭程序


            Task.Factory.StartNew(() => Thread.Sleep(1000))
                .ContinueWith(t =>
                {
                    eventargs.Session.Close();
                    //跳转到 列表
                    //Messenger.Default.Send(new NavigateMessage(), "ShowUserpapers");
                    //Messenger.Default.Send(new NavigateMessage(MenuExamVM.ViewName, "R", true), "ShowUserpapers");
                    Messenger.Default.Send(new NavigateMessage(null, "NewCard", true), "ShowUserpapers");
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

    }


    public class BindCardOKVM : ViewModelBase
    {
        private string cardNo;
        private string cardName;
        private string validity;

        public string CardNo
        {
            get
            {
                return cardNo;
            }
            set
            {
                cardNo = value;
                RaisePropertyChanged("CardNo");
            }
        }

        public string CardName
        {
            get
            {
                return cardName;
            }
            set
            {
                cardName = value;
                RaisePropertyChanged("CardName");
            }
        }

        public string Validity
        {
            get
            {
                return validity;
            }
            set
            {
                validity = value;
                RaisePropertyChanged("Validity");
            }
        }
    }
}

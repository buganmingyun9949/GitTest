using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using ST.Common;
using Card = ST.Models.Api.Card;

namespace Personal_App.ViewModel
{
    public class BindingCardVM : MainViewModel, IView
    {
        public const string ViewName = "BindingCardView";


        public delegate void ChangeCardHandler(string cardValidity);
        //定义事件
        public event ChangeCardHandler ChangeCardEvent;

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

        private RelayCommand bindingCardCmd;//绑定新卡

        public RelayCommand BindingCardCmd => bindingCardCmd ?? (bindingCardCmd = new RelayCommand(
                           (action) =>
                           {
                               if (!string.IsNullOrEmpty(TxtCardNo1 + TxtCardNo2 + TxtCardNo3 + TxtCardNo4))
                               {
                                   // 异步请求，防止界面假死
                                   //Task.Run(() =>
                                   //{
                                   Application.Current.Dispatcher.Invoke(new Action(() =>
                                   {
                                       UserBindCardModel model = new UserBindCardModel()
                                       {
                                           card_key = $"{TxtCardNo1}-{TxtCardNo2}-{TxtCardNo3}-{TxtCardNo4}"
                                       };

                                       //开始绑定
                                       var result = WebApiProxy.GetHtmlRespInfo(model, ApiType.UserBindCard, GlobalUser.USER.Token);

                                       if (result.retCode == 0)
                                       {
                                           ErrMsgOut = "";
                                           // 绑定成功,卡片内容
                                           GlobalUser.USER.Card = new Card()
                                           {
                                               card_id = result.retData.card_id,
                                               card_key = result.retData.card_key,
                                               is_used = result.retData.is_used,
                                               is_current = result.retData.is_current,
                                               used_time = result.retData.used_time,
                                               used_user_id = result.retData.used_user_id,
                                               expire_time = result.retData.expire_time,
                                               grade = result.retData.grade,
                                               ctime = result.retData.ctime,
                                               card_type = result.retData.card_type,
                                           };
                                           GlobalUser.USER.Expire_status = GlobalUser.USER.Card.expire_status;
                                           User = GlobalUser.USER;
                                           Validity = Convert.ToDateTime(GlobalUser.USER.Card?.expire_time).ToString("yyyy年MM月dd日 HH时mm分 到期");
                                           RaisePropertyChanged("Validity");
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
                               }
                           }));


        public void Activated(object state)
        {
            throw new NotImplementedException();
        }


        private async void ExecuteRunExtendedDialog(object o)
        {

            //#if (DEBUG)
            //            if (GlobalUser.USERCARD == null)
            //            {
            //                GlobalUser.USERCARD = new ST.Models.Api.Card()
            //                {
            //                    CardNo = "111",
            //                    CardName = "222",
            //                    ValidityBegin = "1520930224000",//"2017.11.14",
            //                    ValidityEnd = "1520930224000",//"2018.11.13",
            //                };
            //            }
            //#else
            //               GlobalUser.USERCARD = new ST.Models.Api.Card()
            //            {
            //                CardNo = "111",
            //                CardName = "222",
            //                ValidityBegin = "1520930224000",//"2017.11.14",
            //                ValidityEnd = "1520930224000",//"2018.11.13",
            //            };

            //#endif

            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new BindCardOKDialog()
            {
                DataContext = new BindCardOKVM()
                {
                    CardNo = GlobalUser.USER.Card.card_key,
                    CardName = $"{GlobalUser.USER.Card.grade}年级",
                    Validity = $"{Convert.ToDateTime(GlobalUser.USER.Card.used_time).ToString("yyyy年MM月dd日")} - {Convert.ToDateTime(GlobalUser.USER.Card.expire_time).ToString("yyyy年MM月dd日")}"
                }
            };

            // 绑卡后更新用户菜单有效期
            ChangeCardEvent?.Invoke(GlobalUser.USER.Card.expire_time);

            #region 刷新用户信息...

            //GlobalUser.USERCARD
            //Validity

            #endregion

            //show the dialog
            await DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler);

            //check the result...
            //Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
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
                    Messenger.Default.Send(new NavigateMessage(), "ShowUserpapers");
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }
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

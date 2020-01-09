
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
    public class ExpireCardVM : MainViewModel, IView
    {
        public void Activated(object state)
        {
            throw new NotImplementedException();
        }

        public const string ViewName = "ExpireCardView";

        private string _txtCardNo;
        private string _errMsgOut;

        public string TxtCardNo
        {
            get
            {
                return _txtCardNo;
            }
            set
            {
                _txtCardNo = value;
                if (string.IsNullOrEmpty(_txtCardNo))
                {
                    ErrMsgOut = "";
                }
                RaisePropertyChanged("TxtCardNo");
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

        public RelayCommand BindingCardCmd
        {
            get
            {
                return bindingCardCmd ?? (bindingCardCmd = new RelayCommand(
                           (action) =>
                           {
                               if (!string.IsNullOrEmpty(TxtCardNo))
                               {
                                   // 异步请求，防止界面假死
                                   //Task.Run(() =>
                                   //{
                                   Application.Current.Dispatcher.Invoke(new Action(() =>
                                   {
                                       UserBindCardModel model = new UserBindCardModel()
                                       {
                                           card_key = TxtCardNo
                                       };

                                       //开始绑定
                                       var result = WebApiProxy.GetHtmlRespInfo(model, ApiType.UserBindCard, GlobalUser.USER.Token);
                                       if (result.retCode == 1)
                                       {
                                           ErrMsgOut = "";
                                           //绑定成功,卡片内容
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
                                           //显示 卡片内容
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
            }
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
                    Validity = $"{GlobalUser.STUDYCARD.used_time} - {GlobalUser.STUDYCARD.expire_time}"
                }
            };

            //show the dialog
            await DialogHost.Show(view, o, ExtendedOpenedEventHandler);

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


            Task.Factory.StartNew(() => Thread.Sleep(3000))
                .ContinueWith(t => {
                    eventargs.Session.Close();

                    //跳转到 列表
                    Messenger.Default.Send(new NavigateMessage(), "ShowUserpapers");
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
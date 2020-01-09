using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using Personal_App.Domain;
using ST.Common;
using ST.Common.WebApi;
using ST.Models.Api;

namespace Personal_App.ViewModel.Menu
{
    public class MenuFullVerVM : MainViewModel
    {

        public const string ViewName = "MenuFullVer";

        public MenuFullVerVM() { }

        #region << 属性 字段 >>


        #endregion


        #region << Btn Command >>

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

        private RelayCommand _TyHomewordCommand;// 打开 课后作业

        public RelayCommand TyHomewordCommand
        {
            get
            {
                return _TyHomewordCommand ?? (_TyHomewordCommand = new RelayCommand(
                           (action) =>
                           {
                               Messenger.Default.Send(new NavigateMessage(TyHomeworkControlVM.ViewName, TyHomeworkControlVM.ViewName, true), "ShowUserpapers");

                           }));
            }
        }

        private RelayCommand _TyDanciCommand;// 打开 单词跟读
        public RelayCommand TyDanciCommand
        {
            get
            {
                return _TyDanciCommand ?? (_TyDanciCommand = new RelayCommand(
                           (action) =>
                           {
                               GlobalUser.MenuType = MenuType.Sync;

                               //加载 单词清单
                               var view4 = new SyncListControl();
                               view4.DataContext = new SyncListControlVM(view4.TrainDetailContentPanel,
                                   4);

                               Messenger.Default.Send(
                                   new NavigateMessage(SyncListControlVM.ViewName, "N", view4, true),
                                   "ShowUserpapers");
                           }));
            }
        }

        private RelayCommand _TyKewenCommand;// 打开 课文跟读
        public RelayCommand TyKewenCommand
        {
            get
            {
                return _TyKewenCommand ?? (_TyKewenCommand = new RelayCommand(
                           (action) =>
                           {
                               GlobalUser.MenuType = MenuType.Sync;

                               //加载 单元清单
                               var view5 = new SyncUnitListControl();
                               view5.DataContext = new SyncUnitListControlVM(view5.SyncUnitContentPanel,
                                   5);

                               Messenger.Default.Send(
                                   new NavigateMessage(SyncUnitListControlVM.ViewName, "N", view5, true),
                                   "ShowUserpapers");
                           }));
            }
        }

        private RelayCommand _TyTingshuoCommand;// 打开 听说模拟
        public RelayCommand TyTingshuoCommand
        {
            get
            {
                return _TyTingshuoCommand ?? (_TyTingshuoCommand = new RelayCommand(
                           (action) =>
                           {
                               GlobalUser.MenuType = MenuType.Exam;
                               Messenger.Default.Send(new NavigateMessage(MenuExamVM.ViewName, "N", false), "ShowUserpapers");

                           }));
            }
        }

        private RelayCommand _TyZhuanxiangCommand;// 打开 专项练习
        public RelayCommand TyZhuanxiangCommand
        {
            get
            {
                return _TyZhuanxiangCommand ?? (_TyZhuanxiangCommand = new RelayCommand(
                           (action) =>
                           {
                               GlobalUser.MenuType = MenuType.Train;

                               var view10 = new TrainListControl();
                               view10.DataContext = new TrainListControlVM(view10.TrainDetailContentPanel,
                                   "","专项练习");

                               Messenger.Default.Send(
                                   new NavigateMessage(TrainListControlVM.ViewName, "N", view10, true),
                                   "ShowUserpapers");
                           }));
            }
        }

        #endregion

        #region << 自定义 >>


        private async void ExecuteReBindingCardExtendedDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            //var view = new UserReBandingCard();
            ////show the dialog
            ////await DialogHost.Show(view, o, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
            //await DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler, ExtendedReBandingClosingEventHandler);

            //dialogNum = 4;

            int card_auth_num = Convert.ToInt32(GlobalUser.STUDYCARD.card_auth);
            if ((card_auth_num & (1 << 2)) > 0) //可在线续费
            {
                var view = new UserPayBoxDialog();
                var result = DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedClosing2PayEventHandler);
                return;
            }
            //else if ((card_auth_num & (1 << 1)) > 0) //可绑卡续费
            else if ((card_auth_num & (1 << 1)) > 0 || card_auth_num == 0) //可绑卡续费
            {
                //打开 学习卡 绑定 
                Messenger.Default.Send(new NavigateMessage(BindingCardVM.ViewName, null), "ShowUserpapers");
            }
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

        /// <summary>
        /// 支付。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ExtendedClosing2PayEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
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
                    ;
                    //打开 学习卡 绑定 
                    Messenger.Default.Send(new NavigateMessage(BindingCardVM.ViewName, null), "ShowUserpapers");
                }
                else if ((Int32)eventArgs.Parameter == 4 || (Int32)eventArgs.Parameter == 6)
                {
                    if ((Int32)eventArgs.Parameter == 6)
                    {
                        return;
                    }

                    var userResult = WebProxy(ApiType.UserInfo, GlobalUser.USER.Token);
                    if (!string.IsNullOrEmpty(userResult.retData.study_card.ToString()))
                    {
                        if (GlobalUser.STUDYCARD?.card_key.ToString() ==
                            userResult.retData.study_card?.card_key.ToString())
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

                    // 绑卡后更新用户菜单有效期
                    //DialogHostEx.ShowDialog(GlobalUser.MainWin, view);
                    //MainLoadoadMenu(true);
                    Messenger.Default.Send(new NavigateMessage(null, "NewCard", true), "ShowUserpapers");
                    //打开 对话框
                    eventArgs.Session.UpdateContent(view);
                    eventArgs.Cancel();
                }
                else
                {
                    //关闭
                }
            }
        }

        #endregion
    }
}

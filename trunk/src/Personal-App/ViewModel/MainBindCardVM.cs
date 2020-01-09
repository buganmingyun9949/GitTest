using System;
using System.Text.RegularExpressions;
using System.Windows;
using ST.Common;
using ST.Common.WebApi;
using ST.Models.Api;

namespace Personal_App.ViewModel
{
    public class MainBindCardVM : MainViewModel
    {
        public MainBindCardVM(MainBindCard bindingCard)
        {
            _BindingCard = bindingCard;
        }

        #region << 属性 字段 >>

        private string pattern = @"^[a-zA-Z0-9-]{4}(\-)[a-zA-Z0-9-]{4}(\-)[a-zA-Z0-9-]{4}(\-)[a-zA-Z0-9-]{4}$";

        //定义事件
        public MainBindCard _BindingCard;

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

                                       //GlobalUser.STUDYCARD = new Study_Card();
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
                                       //BandingOK = true;
                                       // 显示 卡片内容
                                       //ExecuteRunExtendedDialog(action);
                                       _BindingCard.Hide();
                                       GlobalUser.MainWin = new MainWindow();
                                       GlobalUser.MainWin.Show();
                                       GlobalUser.MainWin.Focus();
                                   }
                                   else
                                   {
                                       //绑定失败
                                       ErrMsgOut = result.retMsg;
                                   }
                               }));
                               //});
                           }));
        //BackUpCommand

        private RelayCommand backUpCmd;//返回登录

        public RelayCommand BackUpCmd => backUpCmd ?? (backUpCmd = new RelayCommand(
                           (action) =>
                           {
                               _BindingCard.Hide();
                               var viewWin = new MainLogin();
                               viewWin.DataContext = new  MainLoginVM(false);
                               GlobalUser.LoginWin = viewWin;
                               viewWin.Show();
                               _BindingCard = null;
                           }));
        #endregion

        #region << 自定义方法 >>

        #endregion
    }
}
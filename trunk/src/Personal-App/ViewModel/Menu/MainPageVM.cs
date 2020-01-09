using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using Personal_App.Domain;
using Personal_App.Properties;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Models.Api;

namespace Personal_App.ViewModel.Menu
{
    public class MainPageVM: MainViewModel
    {

        public const string ViewName = "MainPage";

        public MainPageVM() { }

        public MainPageVM(WrapPanel fPPanel, WrapPanel newZyPanel)
        {
            _FPPanel = fPPanel;
            _NewZyPanel = newZyPanel;

            BindFreePractics();

            BindNewZy();

            ShowPayBox = Visibility.Collapsed;
            CheckCardTime();
        }

        #region << 属性 字段 >>

        private WrapPanel _FPPanel;
        private WrapPanel _NewZyPanel;



        private Visibility _ShowPayBox;

        /// <summary>
        /// 超时 过期  过期前 提示
        /// </summary>
        public Visibility ShowPayBox
        {
            get => _ShowPayBox;
            set
            {
                _ShowPayBox = value;
                RaisePropertyChanged("ShowPayBox");
            }
        }

        private string _TxtCardTimeout;

        /// <summary>
        /// 超时 说明 文本
        /// </summary>
        public string TxtCardTimeout
        {
            get => _TxtCardTimeout;
            set
            {
                _TxtCardTimeout = value;
                RaisePropertyChanged("TxtCardTimeout");
            }
        }

        #endregion

        #region << Btn Command >>



        /// <summary>
        /// 打开支付 盒子。
        /// </summary>
        private RelayCommand _OpenPayBoxCommand;

        public RelayCommand OpenPayBoxCommand
        {
            get
            {
                return _OpenPayBoxCommand ?? (_OpenPayBoxCommand = new RelayCommand((action) =>
                {
                    //if (GlobalUser.STUDYCARD.card_type == 2)//测试卡
                    //{
                    //    //打开 学习卡 绑定 
                    //    Messenger.Default.Send(new NavigateMessage(BindingCardVM.ViewName, null), "ShowUserpapers");
                    //    return;
                    //}
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
                }));
            }
        }

        #endregion

        #region << 自定义方法 >>

        private void CheckCardTime()
        {

            if (!string.IsNullOrEmpty(GlobalUser.STUDYCARD?.expire_time))
            {
                if (Convert.ToDateTime(GlobalUser.STUDYCARD?.expire_time).AddDays(-7) < DateTime.Now)
                {
                    //过期7天提醒
                    ShowPayBox = Visibility.Visible;
                    TxtCardTimeout =
                        $"学习卡将于{Convert.ToDateTime(GlobalUser.STUDYCARD?.expire_time).ToString("yyyy-MM-dd")}过期";
                }
            }
            if (GlobalUser.STUDYCARD.card_type == 2)
            {
                //试用卡永久
                ShowPayBox = Visibility.Visible;
                TxtCardTimeout =
                    $"学习卡将于{Convert.ToDateTime(GlobalUser.STUDYCARD?.expire_time).ToString("yyyy-MM-dd")}过期";
            } //未绑卡
            if (GlobalUser.STUDYCARD == null)
            {
                ShowPayBox = Visibility.Collapsed;
                //Validity = "尚未绑定学习卡";
            }
            //卡过期
            else if (GlobalUser.STUDYCARD.expire_status == 0)
            {
                ShowPayBox = Visibility.Visible;
                TxtCardTimeout =
                    $"学习卡于{Convert.ToDateTime(GlobalUser.STUDYCARD?.expire_time).ToString("yyyy-MM-dd")}过期";
            }
            else if (GlobalUser.STUDYCARD.expire_status == -1)
            {
                ShowPayBox = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 自由练习
        /// 同步
        /// </summary>
        private void BindFreePractics()
        {
            // 异步请求，防止界面假死
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var ct = JsonHelper.FromJsonTo<Card_Setting>(GlobalUser.STUDYCARD.card_setting?.ToString());
                int card_module_num = Convert.ToInt32(ct.card_modules.ToString());

                if (ct.module_type == 1)
                {
                    _FPPanel.Children.Clear();

                    if ((card_module_num & (1 << 1)) > 0)
                    {
                        #region << 单词跟读 >>
                        //type:4

                        var pc = new FreePracticsDetailControl();
                        pc.DataContext = new FreePracticsDetailControlVM(1, "单词跟读", 4);
                        pc.Margin = new Thickness(0, 0, 20, 20);

                        _FPPanel.Children.Add(pc);

                        #endregion
                    }

                    if ((card_module_num & (1 << 2)) > 0)
                    {
                        #region << 课文跟读 >>
                        //type:5
                        var pc = new FreePracticsDetailControl();
                        pc.DataContext = new FreePracticsDetailControlVM(2, "课文跟读", 5);
                        pc.Margin = new Thickness(0, 0, 20, 20);

                        _FPPanel.Children.Add(pc);

                        #endregion
                    }

                    if ((card_module_num & (1 << 4)) > 0)
                    {
                        #region << 听说专项 >>
                        //type:3
                        var pc = new FreePracticsDetailControl();
                        pc.DataContext = new FreePracticsDetailControlVM(4, "专项练习", 3);
                        pc.Margin = new Thickness(0, 0, 20, 20);

                        _FPPanel.Children.Add(pc);

                        #endregion
                    }

                    if ((card_module_num & (1 << 3)) > 0)
                    {
                        #region << 听说模拟 >>
                        //type:2
                        var pc = new FreePracticsDetailControl();
                        pc.DataContext = new FreePracticsDetailControlVM(3, "听说模考", 2);
                        pc.Margin = new Thickness(0, 0, 20, 20);

                        _FPPanel.Children.Add(pc);

                        #endregion
                    }

                    if ((card_module_num & (1 << 5)) > 0)
                    {
                        #region << 课后作业 >>

                        var pc = new FreePracticsDetailControl();
                        pc.DataContext = new FreePracticsDetailControlVM(5, "课后作业", 0);
                        pc.Margin = new Thickness(0, 0, 20, 20);

                        _FPPanel.Children.Add(pc);

                        #endregion
                    }
                }
            }));
        }

        private void BindNewZy()
        {
            // 异步请求，防止界面假死
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var ml = new GetUserHomeworkListModel()
                {
                    attend_status = 1,
                    pagesize = 20,
                    page = 1,
                    token = GlobalUser.USER.Token
                };

                var result = WebProxy(ml, ApiType.GetUserHomeworkList, null, "get");

                if (result.retCode == 0)
                {
                    _NewZyPanel.Children.Clear();

                    List<HomeworkListItem> ls =
                        JsonHelper.FromJson<List<HomeworkListItem>>(result.retData.list.ToString());

                    //Messenger.OverrideDefault();
                    if (ls != null && ls.Count > 0)
                    {
                        ls.ForEach(x =>
                        {
                            x.publishTime = TimeHelper.GetDateTimeFrom1970Ticks(x.publish_time);
                            x.finishTime = TimeHelper.GetDateTimeFrom1970Ticks(x.finish_time);
                        });

                        var sList = new List<HomeworkListItem>();
                        ls = ls.OrderByDescending(o => o.finishTime).ThenByDescending(t => t.publishTime).ToList();

                        var newLs = ls.Where(w => w.finishTime > DateTime.Now).OrderByDescending(o => o.publishTime)
                            .ToList();

                        if (newLs == null || newLs.Count == 0)
                        {
                            BindNoZy();

                            return;
                        }

                        foreach (var v in newLs)
                        {
                            sList.Add(v);
                        }


                        for (int i = 0; i < sList.Count; i++)
                        {
                            //string keyName = $"{sList[i].exam_id}_{sList[i].paper_title}_{i}";
                            sList[i].exam_id = $"{sList[i].exam_id}#{i + 1}";

                            sList[i].publishTime = TimeHelper.GetDateTimeFrom1970Ticks(sList[i].publish_time);
                            sList[i].finishTime = TimeHelper.GetDateTimeFrom1970Ticks(sList[i].finish_time);

                            var pc = new HomeworkControl();
                            pc.DataContext = new HomeworkViewModel(sList[i]);
                            pc.Margin = new Thickness(0, 0, 20, 20);
                            pc.Width = 403;
                            pc.Height = 181;

                            _NewZyPanel.Children.Add(pc);
                        }
                    }
                    else
                    {
                        BindNoZy();
                    }
                }
                else
                {
                    //无试卷可用
                    //todo
                    //MessageBox.Show("无试卷可用!");
                    //Messenger.Default.Send(new MainDialogMessage($"{result.retMsg}"), "MainMessageDialog");
                    //Messenger.Default.Send(new NavigateMessage(BindingClassVM.ViewName, BindingClassVM.ViewName, true), "ShowUserpapers");

                    _NewZyPanel.Children.Clear();

                    var bclass = new BindingClass();
                    bclass.DataContext = new BindingClassVM(true);
                    bclass.Margin = new Thickness(0,-280,0,-240);
                    _NewZyPanel.Children.Add(bclass);
                }
            }));
        }

        private void BindNoZy()
        {
            _NewZyPanel.Children.Clear();
            //无内容
            var wpContent = new StackPanel();
            wpContent.HorizontalAlignment = HorizontalAlignment.Center;
            wpContent.VerticalAlignment = VerticalAlignment.Top;
            wpContent.Margin = new Thickness(220, 20, 0, 0);

            //无内容
            var img = new Image();
            img.Source = ConvertHelper.ChangeBitmapToImageSource(Resources.nozy);
            img.Height = 160;
            img.Stretch = Stretch.Uniform;
            img.HorizontalAlignment = HorizontalAlignment.Center;
            img.VerticalAlignment = VerticalAlignment.Top;
            img.Margin = new Thickness(0, 20, 0, 0);

            var color = (Color)ColorConverter.ConvertFromString("#537588");
            var txtMsg = new TextBlock
            {
                Text = Settings.Default.NoHomework,
                FontSize = 22,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20),
                Foreground = new SolidColorBrush(color)
            };

            wpContent.Children.Add(img);
            wpContent.Children.Add(txtMsg);

            _NewZyPanel.Children.Add(wpContent);
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
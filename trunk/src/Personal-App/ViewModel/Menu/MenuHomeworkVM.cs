
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GalaSoft.MvvmLight.Messaging;
using Personal_App.Domain;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Models.Api;
using Image = System.Windows.Controls.Image;

namespace Personal_App.ViewModel.Menu
{
    public class MenuHomeworkVM: MainViewModel
    {
        public const string ViewName = "MenuHomework";


        private WrapPanel _NewContentPanel;
        private WrapPanel _CompleteContentPanel;

        public MenuHomeworkVM(WrapPanel newContentPanel, WrapPanel completeContentPanel)
        {
            _NewContentPanel = newContentPanel;
            _CompleteContentPanel = completeContentPanel;

            _NewContentPanel.Children.Clear();
            _CompleteContentPanel.Children.Clear();

            LoadZYUC();
        }

        #region << 属性 字段 >>


        private int _HomeworkSelectedIndex;

        public int HomeworkSelectedIndex
        {
            get => _HomeworkSelectedIndex;
            set
            {
                if (_HomeworkSelectedIndex != value)
                {
                    _HomeworkSelectedIndex = value;
                    ShowZY();
                    RaisePropertyChanged("HomeworkSelectedIndex");
                }
            }
        }

        #endregion

        #region << Btn Command >>


        private RelayCommand switchClassCommand;//切换 更新班级

        public RelayCommand SwitchClassCommand
        {
            get
            {
                return switchClassCommand ?? (switchClassCommand = new RelayCommand(
                           (action) =>
                           {
                               Messenger.Default.Send(new NavigateMessage(BindingClassVM.ViewName,
                                   BindingClassVM.ViewName, true), "ShowUserpapers");
                           }));
            }
        } 


        private RelayCommand updateZYCommand;//更新作业

        public RelayCommand UpdateZYCommand
        {
            get
            {
                return updateZYCommand ?? (updateZYCommand = new RelayCommand(
                           (action) =>
                           {
                               LoadZYUC();
                           }));
            }
        } 
        #endregion

        #region << 自定义方法 >>

        private void ShowZY()
        {
            if (_HomeworkSelectedIndex == 0)
            {
                _NewContentPanel.Visibility = Visibility.Visible;
                _CompleteContentPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                _NewContentPanel.Visibility = Visibility.Collapsed;
                _CompleteContentPanel.Visibility = Visibility.Visible;
            }
        }

        private void LoadZYUC()
        {
            LoadNew();
            LoadComplete();
        }

        private void LoadNew()
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
                    _NewContentPanel.Children.Clear();

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
                        var overLs = ls.Where(w => w.finishTime <= DateTime.Now).OrderByDescending(o => o.publishTime)
                            .ToList();

                        foreach (var v in newLs)
                        {
                            sList.Add(v);
                        }
                        foreach (var v in overLs)
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

                            _NewContentPanel.Children.Add(pc);
                        }
                    }
                    else
                    {
                        _NewContentPanel.Children.Clear();
                        //无内容
                        StackPanel wpContent = new StackPanel();
                        wpContent.HorizontalAlignment = HorizontalAlignment.Center;
                        wpContent.VerticalAlignment = VerticalAlignment.Top;
                        wpContent.Margin = new Thickness(200, 40, 0, 0);

                        //无内容
                        Image img = new Image();
                        img.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.nozy);
                        img.Width = 430;
                        img.Height = 240;
                        img.HorizontalAlignment = HorizontalAlignment.Center;
                        img.VerticalAlignment = VerticalAlignment.Top;
                        img.Margin = new Thickness(0, 20, 0, 0);

                        var color = (Color)ColorConverter.ConvertFromString("#537588");
                        TextBlock txtMsg = new TextBlock();
                        txtMsg.Text = Properties.Settings.Default.NoHomework;
                        txtMsg.FontSize = 22;
                        txtMsg.VerticalAlignment = VerticalAlignment.Bottom;
                        txtMsg.HorizontalAlignment = HorizontalAlignment.Center;
                        txtMsg.Margin = new Thickness(0, 0, 0, 20);
                        txtMsg.Foreground = new SolidColorBrush(color);


                        wpContent.Children.Add(img);
                        wpContent.Children.Add(txtMsg);

                        _NewContentPanel.Children.Add(wpContent);
                    }
                }
                else
                {
                    //无试卷可用
                    //todo
                    //MessageBox.Show("无试卷可用!");
                    //Messenger.Default.Send(new MainDialogMessage($"{result.retMsg}"), "MainMessageDialog");
                    //Messenger.Default.Send(new NavigateMessage(BindingClassVM.ViewName, BindingClassVM.ViewName, true), "ShowUserpapers");
                }
            }));
        }

        private void LoadComplete()
        {

            // 异步请求，防止界面假死
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {

                var ml = new GetUserHomeworkListModel()
                {
                    attend_status = 2,
                    pagesize = 20,
                    page = 1,
                    token = GlobalUser.USER.Token
                };

                var result = WebProxy(ml, ApiType.GetUserHomeworkList, null, "get");

                if (result.retCode == 0)
                {
                    _CompleteContentPanel.Children.Clear();

                    List<HomeworkListItem> ls =
                        JsonHelper.FromJson<List<HomeworkListItem>>(result.retData.list.ToString());
                    //Messenger.OverrideDefault();
                    if (ls != null && ls.Count > 0)
                    {
                        ls.ForEach(x =>
                        {
                            x.publishTime = TimeHelper.GetDateTimeFrom1970Ticks(x.publish_time);
                            x.finishTime = TimeHelper.GetDateTimeFrom1970Ticks(x.finish_time);
                            x.startat = TimeHelper.GetDateTimeFrom1970Ticks(x.start_at);
                            x.endedat = TimeHelper.GetDateTimeFrom1970Ticks(x.ended_at);
                        });

                        var sList = ls.OrderByDescending(t => t.publishTime).ToList();

                        for (int i = 0; i < sList.Count; i++)
                        {
                            //string keyName = $"{sList[i].exam_id}_{sList[i].paper_title}_{i}";
                            sList[i].exam_id = $"{sList[i].exam_id}#{i + 1}";

                            sList[i].publishTime = TimeHelper.GetDateTimeFrom1970Ticks(sList[i].publish_time);
                            sList[i].finishTime = TimeHelper.GetDateTimeFrom1970Ticks(sList[i].finish_time);
                            sList[i].startat = TimeHelper.GetDateTimeFrom1970Ticks(sList[i].start_at);
                            sList[i].endedat = TimeHelper.GetDateTimeFrom1970Ticks(sList[i].ended_at);

                            var pc = new HomeworkControl();
                            pc.DataContext = new HomeworkViewModel(sList[i], true);
                            pc.Margin = new Thickness(0, 0, 20, 20);
                            _CompleteContentPanel.Children.Add(pc);
                        }
                    }
                    else
                    {
                        _CompleteContentPanel.Children.Clear();

                        StackPanel wpContent =new StackPanel();
                        wpContent.HorizontalAlignment = HorizontalAlignment.Center;
                        wpContent.VerticalAlignment = VerticalAlignment.Top;
                        wpContent.Margin = new Thickness(200, 40, 0, 0);

                        //无内容
                        Image img = new Image();
                        img.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.nozy);
                        img.Width = 430;
                        img.Height = 240;
                        img.HorizontalAlignment = HorizontalAlignment.Center;
                        img.VerticalAlignment = VerticalAlignment.Top;
                        img.Margin = new Thickness(0, 20, 0, 0);

                        var color = (Color) ColorConverter.ConvertFromString("#537588");
                        TextBlock txtMsg = new TextBlock();
                        txtMsg.Text = Properties.Settings.Default.NoOverHomework;
                        txtMsg.FontSize = 22;
                        txtMsg.VerticalAlignment = VerticalAlignment.Bottom;
                        txtMsg.HorizontalAlignment = HorizontalAlignment.Center;
                        txtMsg.Margin = new Thickness(0, 0, 0, 20);
                        txtMsg.Foreground = new SolidColorBrush(color);


                        wpContent.Children.Add(img);
                        wpContent.Children.Add(txtMsg);

                        _CompleteContentPanel.Children.Add(wpContent);
                    }
                }
                else
                {
                    //无试卷可用
                    //todo
                    //MessageBox.Show("无试卷可用!");
                }
            }));
        }

        #endregion
    }
}

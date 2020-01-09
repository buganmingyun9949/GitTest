using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Framework.Logging;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using Personal_App.Domain;
using Personal_App.Domain.Exam;
using Personal_App.ViewModel.Exam;
using Personal_App.ViewModel.Menu;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using ST.Models.Paper;

namespace Personal_App.ViewModel
{
    public class TrainListControlVM : MainViewModel
    {
        public const string ViewName = "TrainListControlVM";
         
        private WrapPanel _TrainDetailContentPanel;

        private string _qsType;

        public TrainListControlVM(WrapPanel trainDetailContentPanel, string qsType, string titleName = null)
        {
            ShowBackBtn = Visibility.Collapsed;
            ShowTitle = Visibility.Collapsed;
            if (string.IsNullOrEmpty(qsType))
                ShowTitle = Visibility.Visible;
            else
                ShowBackBtn = Visibility.Visible;

            if (!string.IsNullOrEmpty(titleName))
            {
                TitleName = titleName;
            }

            _TrainDetailContentPanel = trainDetailContentPanel;

            _TrainDetailContentPanel.Children.Clear();

            this._qsType = qsType;

            LoadTrainUC();
        }

        #region << 属性 字段 >>


        private Visibility _ShowBackBtn;
        /// <summary>
        /// 显示 返回按钮
        /// </summary>
        public Visibility ShowBackBtn
        {
            get => _ShowBackBtn;
            set
            {
                _ShowBackBtn = value;
                RaisePropertyChanged("ShowBackBtn");
            }
        }

        private string _TitleName;

        public string TitleName
        {
            get => _TitleName;
            set
            {
                _TitleName = value;
                RaisePropertyChanged("TitleName");
            }
        }

        private Visibility _ShowTitle;
        /// <summary>
        /// 显示标题
        /// </summary>
        public Visibility ShowTitle
        {
            get => _ShowTitle;
            set
            {
                _ShowTitle = value;
                RaisePropertyChanged("ShowTitle");
            }
        }

        #endregion

        #region << Btn Command>>


        private RelayCommand backTrainCmd;//返回按钮

        public RelayCommand BackTrainCmd
        {
            get
            {
                return backTrainCmd ?? (backTrainCmd = new RelayCommand(
                           (action) =>
                           {
                               Messenger.Default.Send(new NavigateMessage(MenuTrainVM.ViewName, MenuTrainVM.ViewName, true), "ShowUserpapers");
                           }));
            }
        }

        #endregion

        #region << 自定义方法 >>


        private void LoadTrainUC()
        {

            // 异步请求，防止界面假死
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var ml = new GetSimulationListModel()
                {
                    paper_type = 3,
                    qs_type = _qsType,
                    pagesize = 100,
                    token = GlobalUser.USER.Token
                };

                if (GlobalUser.GRADEINFO != null && GlobalUser.GRADEINFO.Grade_Id > 0)
                {
                    ml.selected_grade = GlobalUser.GRADEINFO.Grade_Id;
                }

                var result = WebProxy(ml, ApiType.GetSimulationList, null, "get");

                if (result.retCode == 0 && result.retData.total > 0)
                {
                    List<SimulationItem> lsPapers =
                        JsonHelper.FromJson<List<SimulationItem>>(result.retData.list.ToString());

                    //Messenger.OverrideDefault();
                    if (lsPapers != null && lsPapers.Count > 0)
                    {
                        var sList = lsPapers;

                        for (int i = 0; i < sList.Count; i++)
                        {
                            string keyName = $"{sList[i].exam_id}_{sList[i].paper_title}_{i}";
                            sList[i].exam_id = $"{sList[i].exam_id}#{i + 1}";

                            var pc = new TrainDetailControl();
                            pc.DataContext = new TrainDetailControlVM(sList[i], (i + 1) % 4);
                            pc.Margin = new Thickness(0, 0, 20, 20);

                            _TrainDetailContentPanel.Children.Add(pc);
                        }
                    }
                }
                else
                {
                    if (result.retCode == 40400)
                    {
                        Messenger.Default.Send(new MainDialogMessage(result.retMsg), "MainMessageDialog");
                        return;
                    }

                    if (GlobalUser.USER?.Expire_status == 1 || GlobalUser.USER?.Expire_status == 0)
                    {
                        //无试卷可用
                        //todo
                        //Messenger.Default.Send(new MainDialogMessage("暂无训练内容!"), "MainMessageDialog");
                        //MessageBox.Show("无试卷可用!");

                        _TrainDetailContentPanel.Children.Clear();
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
                        img.Margin = new Thickness(0, 80, 0, 0);

                        var color = (Color)ColorConverter.ConvertFromString("#537588");
                        TextBlock txtMsg = new TextBlock();
                        txtMsg.Text = Properties.Settings.Default.NoTrainPaper;
                        txtMsg.FontSize = 22;
                        txtMsg.VerticalAlignment = VerticalAlignment.Bottom;
                        txtMsg.HorizontalAlignment = HorizontalAlignment.Center;
                        txtMsg.Margin = new Thickness(0, 10, 0, 20);
                        txtMsg.Foreground = new SolidColorBrush(color);


                        wpContent.Children.Add(img);
                        wpContent.Children.Add(txtMsg);

                        _TrainDetailContentPanel.Children.Add(wpContent);
                    }
                }
            }));
        }

        #endregion


    }
}
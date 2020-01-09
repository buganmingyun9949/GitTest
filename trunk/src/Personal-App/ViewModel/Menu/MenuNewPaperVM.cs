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

namespace Personal_App.ViewModel.Menu
{
    public class MenuNewPaperVM : MainViewModel
    {

        public const string ViewName = "MenuNewPaper";


        private WrapPanel _NewPaperContentPanel;

        public MenuNewPaperVM(WrapPanel paperContentPanel)
        {
            _NewPaperContentPanel = paperContentPanel;

            _NewPaperContentPanel.Children.Clear();

            LoadNewsPaperUC();
        }

        #region << 属性 字段 >>


        #endregion

        /// <summary>
        /// 同步报纸 信息
        /// </summary>
        private void LoadNewsPaperUC()
        {
            // 异步请求，防止界面假死
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {

                var ml = new GetSimulationListModel()
                {
                    paper_type = 6,
                    pagesize = 100,
                    token = GlobalUser.USER.Token
                };

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

                            var pc = new NewPaperControl();
                            pc.DataContext = new NewPaperControlVM(sList[i], (i + 1) % 4);
                            pc.Margin = new Thickness(0, 0, 20, 20);
                            _NewPaperContentPanel.Children.Add(pc);
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
                        //Messenger.Default.Send(new MainDialogMessage("暂无试卷可练习!"), "MainMessageDialog");
                        //MessageBox.Show("无试卷可用!");

                        _NewPaperContentPanel.Children.Clear();
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

                        _NewPaperContentPanel.Children.Add(wpContent);
                    }
                }
            }));
        }
    }
}

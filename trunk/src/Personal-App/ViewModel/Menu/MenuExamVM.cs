
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
    public class MenuExamVM : MainViewModel
    {

        public const string ViewName = "MenuExam";


        private WrapPanel _PaperContentPanel;

        public MenuExamVM(WrapPanel paperContentPanel)
        {
            _PaperContentPanel = paperContentPanel;

            _PaperContentPanel.Children.Clear();

            LoadPaperUC();
        }

        #region << 属性 字段 >>


        private string _UserGradeName;

        public string UserGradeName
        {
            get => _UserGradeName;
            set
            {
                _UserGradeName = value;
                RaisePropertyChanged("UserGradeName");
            }
        }

        private Visibility _ShowUserGradeName;

        public Visibility ShowUserGradeName
        {
            get => _ShowUserGradeName;
            set
            {
                _ShowUserGradeName = value;
                RaisePropertyChanged("ShowUserGradeName");
            }
        }

        private List<UserGradeList> _UserGradeList;

        public List<UserGradeList> UserGradeList
        {
            get => _UserGradeList;
            set
            {
                _UserGradeList = value;
                RaisePropertyChanged("UserGradeList");
            }
        }

        private UserGradeList _SelectNewGrade;

        public UserGradeList SelectNewGrade
        {
            get => _SelectNewGrade;
            set
            {
                if (value != null)
                {
                    _SelectNewGrade = value;
                    LoadPaperUC(_SelectNewGrade.id);
                    RaisePropertyChanged("SelectNewGrade");
                }
            }
        }

        #endregion


        private void LoadPaperUC(int gradeId = 0)
        {

            // 异步请求，防止界面假死
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                _PaperContentPanel.Children.Clear();

                var ml = new GetSimulationListModel()
                {
                    paper_type = 2,
                    pagesize = 100,
                    token = GlobalUser.USER.Token
                };
                if (gradeId > 0)
                {
                    ml.selected_grade = gradeId;
                }
                else if (gradeId == 0 && GlobalUser.GRADEINFO != null && GlobalUser.GRADEINFO.Grade_Id > 0)
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

                            var pc = new PaperControl();
                            pc.DataContext = new PaperControlVM(sList[i], (i + 1) % 4);
                            pc.Margin = new Thickness(0, 0, 20, 20);
                            _PaperContentPanel.Children.Add(pc);
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

                    if (GlobalUser.USER?.Expire_status == 1|| GlobalUser.USER?.Expire_status == 0)
                    {
                        //无试卷可用
                        //todo
                        //Messenger.Default.Send(new MainDialogMessage("暂无试卷可练习!"), "MainMessageDialog");
                        //MessageBox.Show("无试卷可用!");

                        _PaperContentPanel.Children.Clear();
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
                        txtMsg.Text = Properties.Settings.Default.NoExamPaper;
                        txtMsg.FontSize = 22;
                        txtMsg.VerticalAlignment = VerticalAlignment.Bottom;
                        txtMsg.HorizontalAlignment = HorizontalAlignment.Center;
                        txtMsg.Margin = new Thickness(0, 10, 0, 20);
                        txtMsg.Foreground = new SolidColorBrush(color);


                        wpContent.Children.Add(img);
                        wpContent.Children.Add(txtMsg);

                        _PaperContentPanel.Children.Add(wpContent);
                    }
                }

                //年级信息
                var um = new GetUserGradeInfoModel()
                {
                    token = GlobalUser.USER.Token
                };
                var result1 = WebProxy(um, ApiType.GetGradeList, null, "get");


                if (result1.retCode == 0)
                {
                    UserGradeInfoModel ug = JsonHelper.FromJson<UserGradeInfoModel>(result1.retData.ToString());
                    UserGradeList = ug.gradeList;

                    if (ug.gradeList?.Count > 1)
                    {
                        ShowUserGradeName = Visibility.Visible;

                        if (GlobalUser.GRADEINFO != null && GlobalUser.GRADEINFO.Grade_Id > 0 && gradeId == 0)
                            UserGradeName = GlobalUser.GRADEINFO.Grade_Name;
                        else
                        {
                            var gi = ug.gradeList.FirstOrDefault(f => f.id == (gradeId == 0 ? GlobalUser.STUDYCARD.grade : gradeId));

                            UserGradeName = gi?.name;
                            GlobalUser.GRADEINFO = new GradeInfo()
                            {
                                Grade_Id = gi.id,
                                Grade_Name = gi.name,
                            };
                        }

                        RememberUser();
                    }
                }

            }));
        }
    }
}

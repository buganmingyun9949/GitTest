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
    public class SyncListControlVM : MainViewModel
    {
        public const string ViewName = "SyncListControlVM";

        private WrapPanel _TrainDetailContentPanel;

        private int _qsType;

        private int? _unitId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trainDetailContentPanel"></param>
        /// <param name="qsType">1:word 单词  2:pred  短文短句</param>
        public SyncListControlVM(WrapPanel trainDetailContentPanel, int qsType, int? unitId = null)
        {
            ShowUserGradeName = Visibility.Collapsed;
            ShowBtn = Visibility.Collapsed;
            ShowTitle = Visibility.Visible;

            if (qsType == 4)
            {
                TitleName = "单词跟读";
            }
            if (qsType == 5)
            {
                TitleName = "课文跟读";
                ShowTitle = Visibility.Hidden;
                ShowBtn = Visibility.Visible;
            }


            _TrainDetailContentPanel = trainDetailContentPanel;

            _TrainDetailContentPanel.Children.Clear();

            this._qsType = qsType;
            this._unitId = unitId;

            LoadSyncUC();
        }

        #region << 属性 字段 >>

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

        private Visibility _ShowBtn;

        public Visibility ShowBtn
        {
            get => _ShowBtn;
            set
            {
                _ShowBtn = value;
                RaisePropertyChanged("ShowBtn");
            }
        }

        private Visibility _ShowTitle;

        public Visibility ShowTitle
        {
            get => _ShowTitle;
            set
            {
                _ShowTitle = value;
                RaisePropertyChanged("ShowTitle");
            }
        }

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
                    LoadSyncUC(_SelectNewGrade.id);
                    RaisePropertyChanged("SelectNewGrade");
                }
            }
        }

        #endregion

        #region << Btn Command>>


        private RelayCommand backSyncCmd;//返回按钮

        public RelayCommand BackSyncCmd
        {
            get
            {
                return backSyncCmd ?? (backSyncCmd = new RelayCommand(
                           (action) =>
                           {
                               if (_qsType == 4)
                                   Messenger.Default.Send(
                                       new NavigateMessage(MainPageVM.ViewName, MainPageVM.ViewName, true),
                                       "ShowUserpapers");
                               else
                               {

                                   var view = new SyncUnitListControl();
                                   view.DataContext = new SyncUnitListControlVM(view.SyncUnitContentPanel, _qsType);

                                   Messenger.Default.Send(
                                       new NavigateMessage(SyncUnitListControlVM.ViewName, "N", view, true),
                                       "ShowUserpapers");

                               }

                           }));
            }
        }

        #endregion

        #region << 自定义方法 >>


        private void LoadSyncUC(int gradeId = 0)
        {

            // 异步请求，防止界面假死
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var ml = new GetSimulationListModel()
                {
                    paper_type = _qsType,
                    pagesize = 100,
                    token = GlobalUser.USER.Token
                };
                if (_qsType == 5)
                {
                    ml.unit_id = _unitId;
                }

                if (gradeId > 0)
                {
                    ml.selected_grade = gradeId;
                }
                else if (gradeId == 0 && GlobalUser.GRADEINFO != null && GlobalUser.GRADEINFO.Grade_Id > 0)
                {
                    ml.selected_grade = GlobalUser.GRADEINFO.Grade_Id;
                }

                var result = WebProxy(ml, ApiType.GetSimulationList, null, "get");

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

                if (result.retCode == 0 && result.retData.total > 0)
                {
                    List<SimulationItem> lsPapers =
                        JsonHelper.FromJson<List<SimulationItem>>(result.retData.list.ToString());

                    //Messenger.OverrideDefault();
                    if (lsPapers != null && lsPapers.Count > 0)
                    {
                        _TrainDetailContentPanel.Children.Clear();
                        var sList = lsPapers;

                        for (int i = 0; i < sList.Count; i++)
                        {
                            string keyName = $"{sList[i].exam_id}_{sList[i].paper_title}_{i}";
                            sList[i].exam_id = $"{sList[i].exam_id}#{i + 1}#{_qsType}";
                            if (_qsType == 5)
                            {
                                sList[i].exam_id = $"{sList[i].exam_id}#{_unitId}";
                            }

                            var pc = new SyncDetailControl();
                            pc.DataContext = new SyncDetailControlVM(sList[i], (i + 1) % 4);
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
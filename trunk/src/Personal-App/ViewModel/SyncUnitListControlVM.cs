﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GalaSoft.MvvmLight.Messaging;
using Personal_App.Domain;
using Personal_App.Domain.Menu;
using Personal_App.ViewModel.Menu;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Models.Api;

namespace Personal_App.ViewModel
{
    public class SyncUnitListControlVM : MainViewModel
    {

        public const string ViewName = "SyncUnitListControlVM";

        private WrapPanel _TrainContentPanel;
        private int _paperType;


        public SyncUnitListControlVM(WrapPanel trainContentPanel, int paperType = -1)
        {
            ShowUserGradeName = Visibility.Collapsed;
            _TrainContentPanel = trainContentPanel;

            TitleName = "课文跟读";

            this._paperType = paperType;

            LoadSyncUnitUC();
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
                    LoadSyncUnitUC(_SelectNewGrade.id);
                    RaisePropertyChanged("SelectNewGrade");
                }
            }
        }


        #endregion

        #region << Btn Command >>


        private RelayCommand backSyncUnitCmd;//返回按钮

        public RelayCommand BackSyncUnitCmd
        {
            get
            {
                return backSyncUnitCmd ?? (backSyncUnitCmd = new RelayCommand(
                           (action) =>
                           {
                               //var view = new MenuSync();
                               //view.DataContext = new MenuSyncVM();
                               Messenger.Default.Send(new NavigateMessage(MainPageVM.ViewName, MainPageVM.ViewName, true),
                                   "ShowUserpapers");
                           }));
            }
        }

        #endregion

        #region << 自定义方法 >>

        private void LoadSyncUnitUC(int gradeId = 0)
        {
            // 异步请求，防止界面假死
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                _TrainContentPanel.Children.Clear();

               var ml = new GetSimulationListModel()
                {
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

                var result = WebProxy(ml, ApiType.GetSyncUnits, null, "get");


                if (result.retCode == 0 && result.retData != null)
                {
                    List<SyncUnitInfo> lsUnits =
                        JsonHelper.FromJson<List<SyncUnitInfo>>(result.retData.ToString());

                    //Messenger.OverrideDefault();
                    if (lsUnits != null && lsUnits.Count > 0)
                    {
                        _TrainContentPanel.Children.Clear();

                        var sList = lsUnits;

                        for (int i = 0; i < sList.Count; i++)
                        {
                            var pc = new SyncUnitControl();
                            pc.DataContext = new SyncUnitControlVM(sList[i], (i + 1) % 4);
                            pc.Margin = new Thickness(0, 0, 20, 20);

                            _TrainContentPanel.Children.Add(pc);
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
                        ////无试卷可用
                        ////todo
                        //Messenger.Default.Send(new MainDialogMessage("暂无试卷可练习!"), "MainMessageDialog");
                        ////MessageBox.Show("无试卷可用!");
                        
                        //无试卷可用
                        //todo
                        //Messenger.Default.Send(new MainDialogMessage("暂无试卷可练习!"), "MainMessageDialog");
                        //MessageBox.Show("无试卷可用!");

                        _TrainContentPanel.Children.Clear();
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

                        _TrainContentPanel.Children.Add(wpContent);
                    }
                }

                //年级
                var um = new GetUserGradeInfoModel()
                {
                    token = GlobalUser.USER.Token
                };
                var result1 = WebProxy(um, ApiType.GetGradeList, null, "get");


                if (result1.retCode == 0)
                {
                    UserGradeInfoModel ug = JsonHelper.FromJson<UserGradeInfoModel>(result1.retData.ToString());
                    UserGradeList = ug.gradeList;

                    if  ( ug.gradeList?.Count > 1)
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

        #endregion

    }
}

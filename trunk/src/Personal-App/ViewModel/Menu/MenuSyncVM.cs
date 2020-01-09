
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using Personal_App.Domain;
using Personal_App.Domain.Exam;
using Personal_App.ViewModel.Exam;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;

namespace Personal_App.ViewModel.Menu
{
    public class MenuSyncVM : MainViewModel
    {

        public const string ViewName = "MenuSync";


        private WrapPanel _PaperContentPanel;

        private DialogSession dialogSession;

        public MenuSyncVM() { }

        public MenuSyncVM(WrapPanel paperContentPanel)
        {
            _PaperContentPanel = paperContentPanel;

            _PaperContentPanel.Children.Clear();
        }

        #region << 属性 字段 >>


        #endregion


        #region << Btn Command >>


        private RelayCommand showWordUcCmd;// 打开 单词训练

        public RelayCommand ShowWordUcCmd
        {
            get
            {
                return showWordUcCmd ?? (showWordUcCmd = new RelayCommand(
                           (action) =>
                           {
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


        private RelayCommand showPredUcCmd;//打开 句子训练

        public RelayCommand ShowPredUcCmd
        {
            get
            {
                return showPredUcCmd ?? (showPredUcCmd = new RelayCommand(
                           (action) =>
                           {
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
        #endregion

        #region << 自定义 >>

        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            dialogSession = eventArgs.Session;
        }
         

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
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

namespace Personal_App.ViewModel
{
    public class TrainControlVM : MainViewModelBase
    {
        public const string ViewName = "TrainControlVM";

        private TrainInfo _TrainInfo;

        public TrainControlVM()
        {

        }

        public TrainControlVM(TrainInfo trainInfo, int index) : this()
        {
            this._TrainInfo = trainInfo;

            this.TrainBg = $"/Resources/zhuanxiang{index + 1}.png";

            this.TrainTypeName = trainInfo.Qs_Type_Name;

            this.TrainTypeNumName = $"{trainInfo.paper_num ?? "0"}道题目";
        }

        #region << 属性 字段 >>

        private bool Doing = false;

        private object _TrainBg;
        /// <summary>
        /// 图片 分数提示图框。
        /// </summary>
        public object TrainBg
        {
            get
            {
                return _TrainBg;
            }
            set
            {
                _TrainBg = value;
                RaisePropertyChanged("TrainBg");
            }
        } 

        private string _TrainTypeName;
        /// <summary>
        /// 作业 名称
        /// </summary>
        public string TrainTypeName
        {
            get => _TrainTypeName;
            set
            {
                _TrainTypeName = value;
                RaisePropertyChanged("TrainTypeName");
            }
        }

        private string _TrainTypeNumName;
        /// <summary>
        /// 作业 内容 数量
        /// </summary>
        public string TrainTypeNumName
        {
            get => _TrainTypeNumName;
            set
            {
                _TrainTypeNumName = value;
                RaisePropertyChanged("TrainTypeNumName");
            }
        }

        private string _TxtHwPushTime;
        /// <summary>
        /// 作业 发布时间
        /// </summary>
        public string TxtHwPushTime
        {
            get => _TxtHwPushTime;
            set
            {
                _TxtHwPushTime = value;
                RaisePropertyChanged("TxtHwPushTime");
            }
        }
         
        #endregion

        #region << Btn Command >>


        private RelayCommand startTrainCommand;//开始 作业

        public RelayCommand StartTrainCommand
        {
            get
            {
                return startTrainCommand ?? (startTrainCommand = new RelayCommand(
                           (action) =>
                           {
                               var view10 = new TrainListControl();
                               view10.DataContext = new TrainListControlVM(view10.TrainDetailContentPanel,
                                   _TrainInfo.Qs_Type_Id);

                               Messenger.Default.Send(
                                   new NavigateMessage(TrainListControlVM.ViewName, "N", view10, true),
                                   "ShowUserpapers");
                           }));
            }
        } 

        #endregion

        #region << 自定义方法 >>



        #endregion


    }
}

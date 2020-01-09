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
    public class SyncUnitControlVM : MainViewModel
    {
        public const string ViewName = "SyncUnitControlVM";

        private SyncUnitInfo _unitInfo;

        private string _unitId;

        public SyncUnitControlVM()
        {

        }

        public SyncUnitControlVM(SyncUnitInfo unitInfo, int index) : this()
        {
            this._unitInfo = unitInfo;

            this.TrainBg = $"/Resources/zhuanxiang{index + 1}.png";

            this._unitId = unitInfo.unit_id;

            this.SyncTypeName = unitInfo.unit_name;

            this.SyncTypeNumName = $"{unitInfo.paper_num ?? "0"}篇课文";
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

        private string _SyncTypeName;
        /// <summary>
        /// 作业 名称
        /// </summary>
        public string SyncTypeName
        {
            get => _SyncTypeName;
            set
            {
                _SyncTypeName = value;
                RaisePropertyChanged("SyncTypeName");
            }
        }

        private string _SyncTypeNumName;
        /// <summary>
        /// 作业 题目数 标记
        /// </summary>
        public string SyncTypeNumName
        {
            get => _SyncTypeNumName;
            set
            {
                _SyncTypeNumName = value;
                RaisePropertyChanged("SyncTypeNumName");
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
                               //加载 单词清单
                               var view5 = new SyncListControl();
                               view5.DataContext = new SyncListControlVM(view5.TrainDetailContentPanel,
                                   5, int.Parse(_unitId));

                               Messenger.Default.Send(
                                   new NavigateMessage(TrainListControlVM.ViewName, "N", view5, true),
                                   "ShowUserpapers");
                           }));
            }
        } 

        #endregion

        #region << 自定义方法 >>



        #endregion


    }
}

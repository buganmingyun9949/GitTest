using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
using ST.Common.WebApi;
using ST.Models.Api;
using ST.Models.Paper;
using Personal_App.Common;
using Personal_App.Domain.Exam;
using ST.Common;
using VoiceRecorder.Audio;

namespace Personal_App.ViewModel.Exam
{
    public class ExamCommonVM : MainViewModel, IDisposable
    {
        private SimulationPaper SPaper { get; set; }

        #region << 属性 >>


        public DispatcherTimer _dTimer { get; set; }

        public bool IsSubmitQs { get; set; } = false;

        private int _titleIndex = 1;

        public int TitleIndex
        {
            get
            {
                return _titleIndex;
            }
            set
            {
                _titleIndex = value;
            }
        }

        public int ExamDetailId { get; set; }

        private SampleAggregator _SampleAggregator;

        /// <summary>
        /// 波形
        /// </summary>
        public SampleAggregator SampleAggregator
        {
            get { return _SampleAggregator; }
            set
            {
                _SampleAggregator = value;
                RaisePropertyChanged("SampleAggregator");
            }
        }


        private ExamListItem _examItem;
        public ExamListItem ExamItem
        {
            get
            {
                return _examItem;
            }
            set
            {
                _examItem = value;
            }
        }


        private UserPanelVM _userPanelVM = new UserPanelVM();
        /// <summary>
        /// 用户面板视图模型。 
        /// </summary>
        public UserPanelVM UserPanelVM
        {
            get
            {
                return _userPanelVM;
            }
            set
            {
                _userPanelVM = value;
                RaisePropertyChanged("UserPanelVM");
            }
        }

        private AudioPanelVM _audioPanelVM = new AudioPanelVM();
        /// <summary>
        /// 音频面板视图模型。
        /// </summary>
        public AudioPanelVM AudioPanelVM
        {
            get { return _audioPanelVM; }
            set
            {
                _audioPanelVM = value;
                RaisePropertyChanged("AudioPanelVM");
            }
        }

        #endregion

        public ExamCommonVM()
        {

            #region 初始化用户信息...

            UserPanelVM.User = GlobalUser.USER;
            UserPanelVM.ExamTitle = "中考英语听说模考";
            string pagerName = GlobalUser.SelectPaperViewName;
            //if (!string.IsNullOrWhiteSpace(pagerName))
            //{
            //    pagerName = Regex.Replace(pagerName, @"\d", "");
            //    pagerName = pagerName.Substring(pagerName.Length - 4, 4);
            //    UserPanelVM.SelectPaperViewName = pagerName;
            //}
            //UserPanelVM.SelectPaperViewName = pagerName;
            if (!string.IsNullOrEmpty(GlobalUser.SelectPaperNumber))
                UserPanelVM.SelectPaperNumber = GlobalUser.SelectPaperNumber.Split('#')[1];

            #endregion

            #region 初始化音频面板...

            //AudioPanelVM = new AudioPanelVM();

            #endregion
        }

        /// <summary>
        /// 选择 显示 窗口内容
        /// </summary>
        /// <param name="eTtype"></param>
        public void CheckWindowContent(ExamType eTtype)
        {
            if (GlobalUser.MenuType == MenuType.Sync)
            {
                BindPaperInfo();
                bool interOK = true;
                if (string.IsNullOrEmpty(GlobalUser.AttendPaperItemId))
                {
                    //获取批次号  开始新的考试
                    interOK = BeginExamApi();
                }

                if (interOK)
                    SyncAnswerWin();
            }
            else
            {
                ExamAnswerWin(eTtype);
            }
        }

        /// <summary>
        /// 单词 句子 同步
        /// </summary>
        private void SyncAnswerWin()
        {
            BindSyncQsUC();
        }

        /// <summary>
        /// 考试
        /// 作业
        /// 训练
        /// </summary>
        /// <param name="eTtype"></param>
        private void ExamAnswerWin(ExamType eTtype)
        {
            GlobalUser.DoneScore = ScoreType.NoScore;
            switch (eTtype)
            {
                case ExamType.First:
                    GlobalUser.DoneItemExam = false;
                    bool interOK = true;
                    if (string.IsNullOrEmpty(GlobalUser.AttendPaperItemId))
                    {
                        //获取批次号  开始新的考试
                        interOK = BeginExamApi();
                    }

                    if (interOK)
                        //开始模考
                        BindExamUC();
                    //BindExamQsUC();
                    //BindExamCompleteUC();
                    break;
                case ExamType.Continue:
                    GlobalUser.DoneItemExam = true;
                    //继续模考
                    BindExamQsUC();
                    break;
                case ExamType.Again:
                    GlobalUser.DoneItemExam = false;
                    //获取批次号  开始新的考试
                    if (BeginExamApi())
                        //开始模考
                        BindExamUC();
                    //BindExamQsUC();
                    break;
                case ExamType.FirstTrain:
                    GlobalUser.DoneItemExam = false;

                    bool trainOK = true;
                    if (string.IsNullOrEmpty(GlobalUser.AttendPaperItemId))
                    {
                        //获取批次号  开始新的考试
                        trainOK = BeginExamApi();
                    }

                    if (trainOK)
                        //开始模考
                        BindExamQsUC();
                    //BindExamQsUC();
                    //BindExamCompleteUC();
                    break;
                case ExamType.ContinueTrain:
                    GlobalUser.DoneItemExam = true;
                    //继续模考
                    BindExamQsUC();
                    break;
                case ExamType.AgainTrain:
                    GlobalUser.DoneItemExam = false;
                    //获取批次号  开始新的考试
                    if (BeginExamApi())
                        //开始模考
                        //BindExamUC();
                        BindExamQsUC();
                    break;
                case ExamType.FirstTask: //开始作业
                    GlobalUser.DoneItemExam = false;
                    //开始模考
                    //BindExamUC();
                    BindZySchedule();
                    BindExamQsUC();
                    //BindExamCompleteUC();
                    break;
                case ExamType.ContinueTask:
                    //继续完成作业
                    BindExamQsUC();
                    break;
                case ExamType.AgainTask:
                    //再做一次作业
                    BindZySchedule();
                    BindExamQsUC();
                    break;
                case ExamType.Result:
                    BindExamResultUC();
                    break;
                case ExamType.ReDo:
                    GlobalUser.DoneScore = ScoreType.ScoreFailure;
                    BindReDoExamQsUC();
                    break;
                case ExamType.ReUpload:
                    GlobalUser.DoneScore = ScoreType.ScoreFailure;
                    BindReloadExamCompleteUC();
                    break;
            }
        }
        
        /// <summary>
        /// 开始考试 生成考试号  服务端
        /// </summary>
        private bool BeginExamApi()
        {
            var ml = new BeginExamModel()
            {
                exam_id = GlobalUser.SelectPaperNumber.Split('#')[0]
            };

            var result1 = WebProxy(ml, ApiType.BeginExam, GlobalUser.USER.Token);

            if (result1.retCode == 4001 && result1.retMsg.ToLower().Contains("token"))
            {
                //回到登录
                GlobalUser.CleanUp();
                Messenger.Default.Send(new ExamScoreNavigateMessage(), "LoginFailure");

                return false;
            }

            if (result1.retCode == 40400)
            {
                Messenger.Default.Send(new MainDialogMessage(result1.retMsg), "MainMessageDialog");
                return false;
            }

            GlobalUser.AttendPaperItemId = result1.retData.exam_attend_id;

            GlobalUser.SelectExamAttendResult = "";
            return true;
        }

        /// <summary>
        /// 更新作业 进度
        /// </summary>
        private void BindZySchedule()
        {
            if(GlobalUser.USER.UserZy==null) return;

            string zyId = GlobalUser.SelectPaperNumber.Split('#')[0];

            foreach (var taskInfo in GlobalUser.USER.UserZy.Where(w =>
                w.UserPhone == GlobalUser.USER.Mobile && w.ZyID == zyId))
            {
                if (taskInfo.ZySubs == null || taskInfo.ZySubs.Count < 1)
                {
                    taskInfo.ZySubs = new List<TaskSubInfo>();

                    var zyDetails = GlobalUser.SelectPaperInfo.paper_detail;

                    for (int i = 0; i < zyDetails.Count; i++)
                    {
                        taskInfo.ZySubs.Add(new TaskSubInfo()
                        {
                            SubId = zyDetails[i].qs_id,
                            SubName = zyDetails[i].qs_title,
                            Done = false
                        });
                    }
                }
                else
                {
                    if (taskInfo.CurrentTaskDone)
                    {
                        taskInfo.ZySubs.ForEach(x => x.Done = false);
                        taskInfo.ConpleteAgainCount++;
                    }
                }
            }
        }

        /// <summary>
        /// 开始作业 生成作业号  服务端
        /// </summary>
        private void BeginZYApi()
        {
            //var ml = new BeginExamModel()
            //{
            //    exam_id = GlobalUser.SelectPaperNumber.Split('#')[0]
            //};

            //var result1 = WebProxy(ml, ApiType.BeginExam, GlobalUser.USER.Token);

            //GlobalUser.AttendPaperItemId = result1.retData.exam_attend_id;

            //GlobalUser.SelectExamAttendResult = "";
        }

        public void BindExamResultUC()
        {
            var view = new ExamResultUC();

            Messenger.Default.Send(
                new ExamNavigateMessage(ExamResultVM.ViewName, view,
                    new ExamResultVM(view.PaperDetailView, view.ResultSV, view.BtnReturnHome)), "MainExamMainWin");
        }

        /// <summary>
        /// 系统检查。
        /// </summary>
        public void BindExamUC()
        {
            Messenger.Default.Send(new ExamNavigateMessage(CheckHeadsetVM.ViewName, new CheckHeadsetUC(), new CheckHeadsetVM()), "MainExamMainWin");
        }

        /// <summary>
        /// 模考欢迎。
        /// </summary>
        public void BindExamWelcomeUC()
        {
            Messenger.Default.Send(new ExamNavigateMessage(ExamWelcomeVM.ViewName, new ExamWelcome(), new ExamWelcomeVM()), "MainExamMainWin");
        }

        /// <summary>
        /// 绑定题目内容
        /// </summary>
        public void BindSyncQsUC()
        {
            GlobalUser.DoAnswer = true;

            var view = new SyncQsMainWin();
            //view.dgSyncQsListItems.ItemsSource = null;

            var vm = ViewModelLocator.SyncQsMain;
            vm.SelectedSyncQsShow = 0;
            vm.LoadSyncQsMainWinVM(view.dgSyncQsListItems,view.QsContentPanel);
            Messenger.Default.Send(
                new ExamNavigateMessage(SyncQsMainWinVM.ViewName, view,
                    vm), "MainExamMainWin");
        }

        /// <summary>
        /// 绑定题目内容
        /// </summary>
        public void BindExamQsUC()
        {
            ViewModelLocator.ExamQsMain._isReload = false;
            GlobalUser.DoAnswer = true;

            var view = new ExamQsMainWin();
            var vm = ViewModelLocator.ExamQsMain;
            vm.LoadExamQsMainWinVM(view.QsContentPanel, view.BtnSkipNext); 

            Messenger.Default.Send(
                new ExamNavigateMessage(ExamQsMainWinVM.ViewName, view,
                    vm), "MainExamMainWin");
        }
        /// <summary>
        /// 绑定题目内容
        /// </summary>
        public void BindReDoExamQsUC()
        {
            ViewModelLocator.ExamQsMain._isReload = true;//需要显示作答内容;
            GlobalUser.DoAnswer = true;

            var view = new ExamQsMainWin();
            var vm = ViewModelLocator.ExamQsMain;
            vm.LoadExamQsMainWinVM(view.QsContentPanel, view.BtnSkipNext, true);

            Messenger.Default.Send(
                new ExamNavigateMessage(ExamQsMainWinVM.ViewName, view,
                    vm), "MainExamMainWin");
        }

        public void BindExamCompleteUC(ExamDetailListItem item = null, ContinueExamItem citem = null, string waveFileName = "")
        {
            ViewModelLocator.ExamQsMain._isReload = false;
            var view0 = new ExamCompleteUC();

            Messenger.Default.Send(new ExamNavigateMessage(ExamCompleteVM.ViewName, view0, new ExamCompleteVM(view0.CloseBtnD, item, waveFileName)), "MainExamMainWin");
        }

        public void BindReloadExamCompleteUC(ExamDetailListItem item = null, ContinueExamItem citem = null, string waveFileName = "")
        {
            ViewModelLocator.ExamQsMain._isReload = true;
            GlobalUser.DoAnswer = true;

            if (GlobalUser.SelectExamAttend.exam_process == 1)
            {
                GlobalUser.DoneScore = ScoreType.ScoreSuccess;
            }

            var view0 = new ExamCompleteUC();

            Messenger.Default.Send(new ExamNavigateMessage(ExamCompleteVM.ViewName, view0, new ExamCompleteVM(view0.CloseBtnD, item, waveFileName)), "MainExamMainWin");
        }


        public void CleanUp()
        {
            if (GlobalUser.AudioFileReader != null)
            {
                GlobalUser.AudioFileReader?.Dispose();
                GlobalUser.AudioFileReader = null;
            }
            if (GlobalUser.WavePlayer != null)
            {
                GlobalUser.WavePlayer?.Stop();
                GlobalUser.WavePlayer?.Dispose();
                GlobalUser.WavePlayer = null;
            }
            if (GlobalUser.Recorder != null)
            {
                GlobalUser.Recorder?.Stop();
                GlobalUser.Recorder?.Dispose();
                GlobalUser.Recorder = null;
            }

            _dTimer?.Stop();
        }

        public void Dispose()
        {
            CleanUp();

            if (_dTimer != null)
            {
                _dTimer = null;
            }

            ClearMemory();
        }
    }

    public enum ExamType
    {
        First,
        Continue,
        Again,
        FirstTrain,
        ContinueTrain,
        AgainTrain,
        FirstTask,
        ContinueTask,
        AgainTask,
        FirstSync,
        Result,
        ReDo,
        ReUpload,
    }

    public enum FlowType
    {
        RecordTime, //录音时间
        StartAudio,//开始录音音频
        DiAudio,//开始录音前提醒音频
    }
}
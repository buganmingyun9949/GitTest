using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
using ST.Common.ToolsHelper;
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
            BindPaperInfo();
            GlobalUser.DoneScore = ScoreType.NoScore;
            switch (eTtype)
            {
                case ExamType.First:
                    GlobalUser.DoneItemExam = false;
                    if (string.IsNullOrEmpty(GlobalUser.AttendPaperItemId))
                    {
                        //获取批次号  开始新的考试
                        BeginExamApi();
                    }
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
                    BeginExamApi();
                    //开始模考
                    BindExamUC();
                    //BindExamQsUC();
                    break;
                case ExamType.Result:
                    BindExamResultUC();
                    break;
            }
        }

        /// <summary>
        /// 绑定选中试卷信息
        /// </summary>
        public void BindPaperInfo()
        {
            var ml = new GetPaperInfoDetail()
            {
                exam_id = GlobalUser.SelectPaperNumber.Split('#')[0],
                token = GlobalUser.USER.Token
            };

            var result1 = WebProxy(ml, ApiType.GetPaperInfoDetail, null, "get");

            GlobalUser.SelectPaperInfo =
                JsonHelper.FromJson<Paper_Info>(result1.retData.paper_info.ToString());

            GlobalUser.SelectExamAttendResult = result1.retData.exam_attend_result.ToString().Replace("[]", "");

            GlobalUser.SelectExamAttend =
                JsonHelper.FromJson<Exam_Attend>(result1.retData.exam_attend.ToString());

        }

        /// <summary>
        /// 开始考试
        /// </summary>
        private void BeginExamApi()
        {
            var ml = new BeginExamModel()
            {
                exam_id = GlobalUser.SelectPaperNumber.Split('#')[0]
            };

            var result1 = WebProxy(ml, ApiType.BeginExam, GlobalUser.USER.Token);

            GlobalUser.AttendPaperItemId = result1.retData.exam_attend_id;

            GlobalUser.SelectExamAttendResult = "";
        }

        public void BindExamResultUC()
        {
            var view = new ExamResultUC();

            Messenger.Default.Send(
                new ExamNavigateMessage(ExamResultVM.ViewName, view,
                    new ExamResultVM(view.PaperDetailView, view.BtnReturnHome)), "MainExamMainWin");
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
        public void BindExamQsUC()
        {
            GlobalUser.DoAnswer = true;

            var view = new ExamQsMainWin();

            Messenger.Default.Send(
                new ExamNavigateMessage(ExamQsMainWinVM.ViewName, view,
                    new ExamQsMainWinVM(view.QsContentPanel, view.BtnSkipNext)), "MainExamMainWin");
        }

        public void BindExamCompleteUC(ExamDetailListItem item = null, ContinueExamItem citem = null, string waveFileName = "")
        {
            Messenger.Default.Send(new ExamNavigateMessage(ExamCompleteVM.ViewName, new ExamCompleteUC(), new ExamCompleteVM(item, waveFileName)), "MainExamMainWin");
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

            GC.Collect(0);
        }
    }

    public enum ExamType
    {
        First,
        Continue,
        Again,
        Result
    }

    public enum FlowType
    {
        RecordTime, //录音时间
        StartAudio,//开始录音音频
        DiAudio,//开始录音前提醒音频
    }
}
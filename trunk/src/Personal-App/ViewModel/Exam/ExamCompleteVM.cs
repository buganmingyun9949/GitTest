using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Threading;
using Framework.Engine;
using Framework.Logging;
using GalaSoft.MvvmLight.Messaging;
using ST.Common.ToolsHelper;
using ST.Models.Api;
using Personal_App.Common;
using Personal_App.Domain.Exam;
using MaterialDesignThemes.Wpf;
using Personal_App.Domain.DialogForm;
using ST.Common;
using ST.Common.Domain;
using ST.Common.WebApi;
using ST.Models.Paper;

namespace Personal_App.ViewModel.Exam
{
    public class ExamCompleteVM : ExamCommonVM
    {

        public const string ViewName = "ExamCompleteView";


        public ExamCompleteVM(Button closeButton,ExamDetailListItem examDetailListItem = null, string waveFileName = "")
        {
            this._CloseButton = closeButton;
            _reCheckResult = 6;

            UserPanelVM.ExamTitle = "中考英语听说模考";
            UserPanelVM.SelectPaperNumber = GlobalUser.SelectPaperNumber.Split('#')[1];
            if (examDetailListItem != null)
            {
                _examDetailListItem = examDetailListItem;
            }
            if (String.IsNullOrEmpty(waveFileName))
            {
                _waveFileName = waveFileName;
            }

            _dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)//5秒钟
            };
            _dispatcherTimer.Tick += DispatcherTimer_Tick;

            _dispatcherTimer.Start();


            ProgressValue = 0;
            TxtProgressValue = $"打分中{ProgressValue}%...";
        }

        #region << 属性 字段 >>


        private DispatcherTimer _dispatcherTimer;

        private int _timeOut = 60 * 2; // 2分钟超时

        private int _reCheckResult = 6; // 答案检测次数 默认 循环6次   

        private ExamDetailListItem _examDetailListItem;

        private Button _CloseButton;

        private string _waveFileName;
        private bool _IsErrDialogOpen;

        public bool IsErrDialogOpen
        {
            get { return _IsErrDialogOpen; }
            set
            {
                if (_IsErrDialogOpen == value) return;
                _IsErrDialogOpen = value;
                RaisePropertyChanged("IsErrDialogOpen");
            }
        }

        private object _ErrDialogContent;
        public object ErrDialogContent
        {
            get { return _ErrDialogContent; }
            set
            {
                if (_ErrDialogContent == value) return;
                _ErrDialogContent = value;
                RaisePropertyChanged("ErrDialogContent");
            }
        }

        private string _TxtProgressValue;

        /// <summary>
        /// 进度显示文本
        /// </summary>
        public string TxtProgressValue
        {
            get
            {
                return _TxtProgressValue;
            }
            set
            {
                _TxtProgressValue = value;
                RaisePropertyChanged("TxtProgressValue");
            }
        }

        private int _ProgressValue;

        /// <summary>
        /// 进度显示 值
        /// </summary>
        public int ProgressValue
        {
            get
            {
                return _ProgressValue;
            }
            set
            {
                _ProgressValue = value;
                RaisePropertyChanged("ProgressValue");
            }
        }

        #endregion


        #region << Btn Command >>

        private RelayCommand _ReuploadNowCmd;//立即上传

        public RelayCommand ReuploadNowCmd
        {
            get
            {
                return _ReuploadNowCmd ?? (_ReuploadNowCmd = new RelayCommand(
                           (action) =>
                           {
                               GlobalUser.DoneScore = ScoreType.ScoreFailure;
                               IsErrDialogOpen = false;
                               _dispatcherTimer?.Stop();
                               _dispatcherTimer =null;
                               //再次上传 错误失败的记录
                               _timeOut = 60 * 2;
                               _dispatcherTimer = new DispatcherTimer
                               {
                                   Interval = TimeSpan.FromSeconds(5)//5秒钟
                               };
                               _dispatcherTimer.Tick += DispatcherTimer_Tick;

                               _dispatcherTimer.Start();
                           }));
            }
        }

        private RelayCommand _ReuploadLaterCmd;//稍后上传

        public RelayCommand ReuploadLaterCmd
        {
            get
            {
                return _ReuploadLaterCmd ?? (_ReuploadLaterCmd = new RelayCommand(
                           (action) =>
                           {
                               IsErrDialogOpen = false;
                               _dispatcherTimer?.Stop();
                               _dispatcherTimer = null;
                               //退出到父列表窗口
                               ButtonAutomationPeer peer =
                                   new ButtonAutomationPeer(_CloseButton);

                               IInvokeProvider invokeProv =
                                   peer.GetPattern(PatternInterface.Invoke)
                                       as IInvokeProvider;

                               invokeProv.Invoke();
                               //_CloseButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                           }));
            }
        }

        private RelayCommand _AnswerNowCmd;//重新作答

        public RelayCommand AnswerNowCmd
        {
            get
            {
                return _AnswerNowCmd ?? (_AnswerNowCmd = new RelayCommand(
                           (action) =>
                           {
                               IsErrDialogOpen = false;
                               _dispatcherTimer?.Stop();
                               _dispatcherTimer = null;
                               //进入错题的重新作答窗口
                               GlobalUser.DoAnswer = true;

                               var view = new ExamQsMainWin();
                               var vm = ViewModelLocator.ExamQsMain;
                               vm._isReload = true;
                               vm.LoadExamQsMainWinVM(view.QsContentPanel, view.BtnSkipNext, true);

                               Messenger.Default.Send(
                                   new ExamNavigateMessage(ExamQsMainWinVM.ViewName, view,
                                       vm), "MainExamMainWin");
                           }));
            }
        }

        private RelayCommand _AnswerLaterCmd;//稍后作答

        public RelayCommand AnswerLaterCmd
        {
            get
            {
                return _AnswerLaterCmd ?? (_AnswerLaterCmd = new RelayCommand(
                           (action) =>
                           {
                               IsErrDialogOpen = false;
                               _dispatcherTimer?.Stop();
                               _dispatcherTimer = null;
                               //退出到父列表窗口
                               ButtonAutomationPeer peer =
                                   new ButtonAutomationPeer(_CloseButton);

                               IInvokeProvider invokeProv =
                                   peer.GetPattern(PatternInterface.Invoke)
                                       as IInvokeProvider;

                               invokeProv.Invoke();
                           }));
            }
        }

        #endregion

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (GlobalUser.MainWin == null)
            {
                _dispatcherTimer.Stop();
                return;
            }

            _timeOut = _timeOut - _dispatcherTimer.Interval.Seconds;
            
            var allCount = GlobalUser.SelectPaperInfo.paper_detail.Sum(s =>
                s.info.Sum(s1 => s1.items.Where(w => w.answers.Count > 0).Count()));
            var overCount = GlobalUser.ErrScoreInfo?.Count > 0 ? GlobalUser.ErrScoreInfo.Count : 1;

            ProgressValue =  Convert.ToInt32(100 * (allCount - overCount) / allCount);
            TxtProgressValue = $"打分中{ProgressValue}%...";

            //检查是否有评分(上传结果)异常,每个异常评分 执行超过3(SubmitNum-1)次
            if (_timeOut > 0 && GlobalUser.ErrScoreInfo?.Keys.Count > 0 &&
                GlobalUser.DoneScore != ScoreType.Scoring)
            {
                foreach (var key in GlobalUser.ErrScoreInfo.Keys)
                {
                    //var key = GlobalUser.ErrScoreInfo?.Keys.FirstOrDefault();
                    if (GlobalUser.ErrScoreInfo[key].SubmitNum < 999)
                    {
                        Messenger.Default.Send(GlobalUser.ErrScoreInfo[key],
                            "ExamScoreNavi");
                        return;
                    }
                }
            } 

            //保存 更新 ErrScore结果数据
            GlobalUser.UpdateErrScore(User.Mobile,
                GlobalUser.SelectPaperNumber,
                GlobalUser.AttendPaperItemId);


            if (GlobalUser.ErrScoreInfo?.Count > 0 && GlobalUser.DoneScore != ScoreType.Scoring && _timeOut <= 0)
            {
                _dispatcherTimer.Stop();

                //重评失败 无法操作
                //判断有无 SubmitNum==999 无法重评 重新作答
                if (GlobalUser.ErrScoreInfo.Values.Any(w => w.SubmitNum == 999))
                {
                    //显示重新 作答窗口
                    var view1 = new ScoreServiceErrDialog();
                    //view1.DataContext = new ScoreServiceErrDialogVM();
                    //打开 对话框
                    //DialogHost.Show(view1, "ExamMainDialog", ExamClosingEventHandler);
                    ErrDialogContent = view1;
                    IsErrDialogOpen = true;
                    return;
                }

                //显示 重新评分窗口
                var view2 = new ScoreNetErrDialog();
                //view2.DataContext = new ScoreNetErrDialogVM();
                //打开 对话框
                //DialogHost.Show(view2, "ExamMainDialog", ExamClosingEventHandler);
                ErrDialogContent = view2;
                IsErrDialogOpen = true;

                return;
            }

            if (GlobalUser.DoneScore == ScoreType.ScoreSuccess)
            {
                //确认线上消息 返回完成进度100% --> true
                if (GetCompleteResult())
                {
                    //记录完整 展示 评分和答题窗口
                    _dispatcherTimer.Stop();

                    GoToResultView();
                    return;
                }
                else
                {
                    if (_reCheckResult < 0)
                    {
                        //记录完整 展示 评分和答题窗口
                        _dispatcherTimer.Stop();

                        //操作失败
                        //todo
                        ResubmitAnswerDialog dialog = new ResubmitAnswerDialog();

                        DialogHost.Show(dialog, "ExamMainDialog");//ResubmitDialogClosingEventHandler

                        ErrMsgLog msgInfo = new ErrMsgLog();
                        msgInfo.user = GlobalUser.USER.Mobile;
                        msgInfo.msginfo = "等待超时,作答正常完成,未正常返回结果消息"; 
                        msgInfo.isErr = true;
                        Log4NetHelper.Error(msgInfo.ToJson());//$"[user:{GlobalUser.USER?.Mobile},msg:作答正常完成,未正常返回结果消息]");

                        {
                            Log_Data log = new Log_Data();
                            log.log_desc = Log_Type.PC_Complete_Error.ToString();
                            log.log_text = msgInfo.ToJson();//$"[user:{GlobalUser.USER?.Mobile},msg:作答正常完成,未正常返回结果消息]";// $"评分异常,未正常返回结果消息";
                            log.log_device = JsonHelper.ToJson(GlobalUser.MACHINEINFO.GetDevice());
                            WebApiProxy.GetHtmlRespInfo(log, ApiType.SysLog, null, "Post");
                        }
                        return;
                    }

                    _reCheckResult--;
                }
            }

            if (GlobalUser.DoneScore == ScoreType.ScoreFailure &&
                (GlobalUser.ErrScoreInfo == null || !GlobalUser.ErrScoreInfo.Any()))
            {
                //记录完整 展示 评分和答题窗口
                _dispatcherTimer.Stop();

                if (GetCompleteResult())
                {
                    GoToResultView();

                    return;
                }
                else
                {
                    //操作失败
                    //todo
                    ResubmitAnswerDialog dialog = new ResubmitAnswerDialog();

                    DialogHost.Show(dialog, "ExamMainDialog"); //ResubmitDialogClosingEventHandler
                    return;
                }
            }

            if (_timeOut <= 0)
            {
                _dispatcherTimer.Stop();
                // 评分失败
                if (GlobalUser.DoneScore != ScoreType.ScoreSuccess)
                {
                    //超时后处理操作
                    //todo
                    ResubmitAnswerDialog dialog = new ResubmitAnswerDialog();

                    DialogHost.Show(dialog, "ExamMainDialog");//ResubmitDialogClosingEventHandler
                    return;
                }
            }

        }

        /// <summary>
        /// 提交完成
        /// 并进入结果显示
        /// </summary>
        private void GoToResultView()
        {
            CompleteExamModel model = new CompleteExamModel()
            {
                exam_attend_id =
                    (string.IsNullOrEmpty(GlobalUser.AttendPaperItemId) || GlobalUser.SelectExamAttend.exam_attend_id < 1)
                        ? GlobalUser.AttendPaperItemId
                        : GlobalUser.SelectExamAttend.exam_attend_id.ToString(),
            };

            ApiType api = ApiType.CompleteExam;
            if (GlobalUser.MenuType == MenuType.Task)
            {
                api = ApiType.CompleteUserTask;
            }

            //完成练习 作业 训练
            var result = WebApiProxy.GetHtmlRespInfo(model, api, GlobalUser.USER.Token);
            if (result.retCode == 0)
            {
                _dispatcherTimer.Stop();

                //移除 ErrScore结果数据
                GlobalUser.CheckErrorScoreSource(User.Mobile,
                    GlobalUser.SelectPaperNumber.Split(new[] {"#"}, StringSplitOptions.RemoveEmptyEntries)[0],
                    GlobalUser.AttendPaperItemId, true);

                this.Cleanup();
                GlobalUser.DoneItemExam = true;
                BindExamResultUC();
            }
            else
            {
                _dispatcherTimer.Stop();

                //操作失败
                //todo
                ResubmitAnswerDialog dialog = new ResubmitAnswerDialog();

                DialogHost.Show(dialog, "ExamMainDialog"); //ResubmitDialogClosingEventHandler

                ErrMsgLog msgInfo = new ErrMsgLog();
                msgInfo.user = GlobalUser.USER.Mobile;
                msgInfo.msginfo = "作答正常完成,提交 Complete 失败";
                msgInfo.error = result.retHtml;
                msgInfo.isErr = true;
                Log4NetHelper.Error(msgInfo
                    .ToJson()); //$"[user:{GlobalUser.USER?.Mobile},msg:作答正常完成,未正常返回统计消息,data:{result.retHtml}]");

                {
                    Log_Data log = new Log_Data();
                    log.log_desc = Log_Type.PC_Complete_Error.ToString();
                    log.log_text =
                        msgInfo.ToJson(); //$"[user:{GlobalUser.USER?.Mobile},msg:作答正常完成,未正常返回统计消息,data:{result.retHtml}]";// $"{GlobalUser.USER?.Mobile}-作答正常完成,未正常返回统计消息---{result.retMsg}";
                    log.log_device = JsonHelper.ToJson(GlobalUser.MACHINEINFO.GetDevice());
                    WebApiProxy.GetHtmlRespInfo(log, ApiType.SysLog, null, "Post");
                }
            }
        }

        private bool GetCompleteResult()
        {
            bool resultOK = false;

            var ml = new GetPaperInfoDetail()
            {
                exam_id = GlobalUser.SelectPaperNumber.Split('#')[0],
                token = GlobalUser.USER.Token
            };

            var result1 = WebProxy(ml, ApiType.GetPaperInfoDetail, null, "get");

            if (result1?.retCode == 0)
            {
                //发送 
                var exam_attend =
                    JsonHelper.FromJson<Exam_Attend>(result1.retData.exam_attend.ToString());

                if (exam_attend.exam_process == 1)
                {
                    ProgressValue = 100;
                    TxtProgressValue = $"打分中{ProgressValue}%...";

                    resultOK = true;
                    return resultOK;
                }
            }
            else
            {
                ErrMsgLog msgInfo = new ErrMsgLog();
                msgInfo.user = GlobalUser.USER.Mobile;
                msgInfo.msginfo = "作答正常完成,未正常返回统计消息";
                msgInfo.error = result1.retHtml;
                msgInfo.isErr = true;

                Log4NetHelper.Error(msgInfo.ToJson());//$"[user:{GlobalUser.USER?.Mobile},msg:作答正常完成,未正常返回统计消息,data:{result1.retHtml}]");

                {
                    Log_Data log = new Log_Data();
                    log.log_desc = Log_Type.PC_Logout.ToString();
                    log.log_text = msgInfo.ToJson();//$"[user:{GlobalUser.USER?.Mobile},msg:作答正常完成,未正常返回统计消息,data:{result1.retHtml}]";
                    log.log_device = JsonHelper.ToJson(GlobalUser.MACHINEINFO.GetDevice());
                    WebApiProxy.GetHtmlRespInfo(log, ApiType.SysLog, null, "Post");
                }
            }

            return resultOK;
        }

        /// <summary>
        /// 重新提交对话框关闭事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ResubmitDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {

            if (eventArgs.Parameter != null)
            {
                if ((bool)eventArgs.Parameter)
                {
                    BindExamResultUC();
                }
                else
                {
                    Log4NetHelper.Info("完成考试取消提交！");
                }
            }

        }

        /// <summary>
        /// 模考关闭事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventargs"></param>
        private void ExamClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter != null)
            {
                if ((bool)eventArgs.Parameter)
                {
                }
            }
        }

        public new void Dispose()
        {
            CleanUp();

            if (_dispatcherTimer != null)
            {
                _dispatcherTimer?.Stop();
                _dispatcherTimer = null;
            }

            ClearMemory();
        }
    }
}

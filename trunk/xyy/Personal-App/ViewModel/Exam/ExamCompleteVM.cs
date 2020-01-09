using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Framework.Engine;
using Framework.Logging;
using GalaSoft.MvvmLight.Messaging;
using ST.Common.ToolsHelper;
using ST.Models.Api;
using Personal_App.Common;
using Personal_App.Domain.Exam;
using MaterialDesignThemes.Wpf;
using ST.Common;
using ST.Common.WebApi;
using ST.Models.Paper;

namespace Personal_App.ViewModel.Exam
{
    public class ExamCompleteVM : ExamCommonVM
    {

        public const string ViewName = "ExamCompleteView";

        private DispatcherTimer _dispatcherTimer;

        private int _timeOut = 60 * 3; // 3分钟超时

        private int _reCheckResult = 6; // 答案检测次数 默认 循环6次   

        private ExamDetailListItem _examDetailListItem;

        private string _waveFileName;

        public ExamCompleteVM(ExamDetailListItem examDetailListItem = null, string waveFileName = "")
        {
            UserPanelVM.ExamTitle = "中考英语听说模考";
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
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (GlobalUser.DoneScore == ScoreType.ScoreSuccess)
            {
                if (GetCompleteResult())
                {
                    //记录完整 展示 评分和答题窗口
                    _dispatcherTimer.Stop();

                    CompleteExamModel model = new CompleteExamModel()
                    {
                        exam_attend_id =
                            (GlobalUser.SelectExamAttend == null || GlobalUser.SelectExamAttend.exam_attend_id < 1)
                                ? GlobalUser.AttendPaperItemId
                                : GlobalUser.SelectExamAttend.exam_attend_id.ToString(),
                    };

                    //完成练习
                    var result = WebApiProxy.GetHtmlRespInfo(model, ApiType.CompleteExam, GlobalUser.USER.Token);
                    if (result.retCode == 0)
                    {
                        _dispatcherTimer.Stop();

                        BindExamResultUC();
                    }
                    else
                    {
                        _dispatcherTimer.Stop();

                        //操作失败
                        //todo
                        ResubmitAnswerDialog dialog = new ResubmitAnswerDialog();

                        DialogHost.Show(dialog, "ExamMainDialog");//ResubmitDialogClosingEventHandler

                        Log4NetHelper.Error("评分正常完成,未正常返回统计消息---" + result.retMsg);
                    }
                }
                else
                {
                    if (_reCheckResult < 1)
                    {
                        //记录完整 展示 评分和答题窗口
                        _dispatcherTimer.Stop();

                        //操作失败
                        //todo
                        ResubmitAnswerDialog dialog = new ResubmitAnswerDialog();

                        DialogHost.Show(dialog, "ExamMainDialog");//ResubmitDialogClosingEventHandler
                        Log4NetHelper.Error("评分异常,未正常返回结果消息");
                    }

                    _reCheckResult--;
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
                }
            }

            _timeOut = _timeOut - _dispatcherTimer.Interval.Seconds;
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
                    resultOK = true;
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
                    //_dispatcherTimer.Stop();
                    //if (GlobalUser.DoneScore)
                    //{
                    //    BindExamResultUC();
                    //}
                    //else
                    //{
                    //    GlobalUser.DoneScore = true; //评分

                    //    Messenger.Default.Send(
                    //        new ExamScoreNavigateMessage("ExamQuestionFour", ExamItem.examId,
                    //            _examDetailListItem.questionList[0].questionId, _waveFileName,
                    //            Convert.ToDecimal(_examDetailListItem.questionList[0].score),
                    //            _examDetailListItem.text.ToDBC().Zh2En(), EngineQsType.READING, EngineType.PARA),
                    //        "ExamScoreNavi");
                    //}
                }
                else
                {
                    Log4NetHelper.Info("完成考试取消提交！");
                }
            }

        }

    }
}

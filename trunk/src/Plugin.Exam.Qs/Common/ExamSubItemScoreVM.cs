using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Framework.Engine;
using Framework.Engine.Models;
using Framework.Logging;
using GalaSoft.MvvmLight.Messaging;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using ST.Models.Score;
using ST.Common;
using ST.Common.Domain;

namespace Plugin.Exam.Qs.Common
{
    public class ExamSubItemScoreVM
    {
        private bool _isAgain = true;

        private string[] _errIds = new[] {"20009", "20013", "20010", "42003" };

        public ExamSubItemScoreVM() { }

        public ExamSubItemScoreVM(ExamScoreNavigateMessage examScore)
        {
            this.ExamScore = examScore;
        }

        #region << 评分 >>

        private ExamScoreNavigateMessage ExamScore;

        private static IEngine ES;

        public event ScoreFinishedHandler OnScoreFinished;

        private static EngineDelegete.EngineCallback callback = null;

        private static readonly int SKEGN_MESSAGE_TYPE_JSON = 1;

        public void EngineScore()
        {
            //启动引擎
            EngineProvider.AppKey = GlobalUser.APPKEY;
            EngineProvider.SecretKey = GlobalUser.SECRETKEY;
            EngineProvider.ServerUri = GlobalUser.SERVERURI;
            EngineProvider.ServerTimeout = GlobalUser.SERVERTIMEOUT;
            ES = EngineProvider.GetEngine();
            Log4NetHelper.Info("----------------------------- 引擎 启动 成功-----------------------------");

        }
        /// <summary>
        /// 口语题
        /// </summary>
        /// <param name="answers"></param>
        public void AutoScore()
        {
            if (ExamScore == null || string.IsNullOrEmpty(ExamScore.WaveFileName)|| !File.Exists(ExamScore.WaveFileName))
            {
                if (GlobalUser.ErrScoreInfo == null) GlobalUser.ErrScoreInfo = new Dictionary<string, ExamScoreNavigateMessage>();
                if (!GlobalUser.ErrScoreInfo.ContainsKey(ExamScore?.WaveFileName))
                {
                    ExamScore.SubmitNum = 999;
                    GlobalUser.ErrScoreInfo.Add(ExamScore.WaveFileName, ExamScore);
                }
                else
                {
                    GlobalUser.ErrScoreInfo[ExamScore.WaveFileName].SubmitNum = 999;
                }

                GlobalUser.DoneScore = ScoreType.ScoreFailure;
                return;
            }

            if (GlobalUser.DoneScore == ScoreType.Scoring)
            {
                if (GlobalUser.ErrScoreInfo == null) GlobalUser.ErrScoreInfo = new Dictionary<string, ExamScoreNavigateMessage>();
                if (!GlobalUser.ErrScoreInfo.ContainsKey(ExamScore?.WaveFileName))
                {
                    ExamScore.SubmitNum = 1;
                    GlobalUser.ErrScoreInfo.Add(ExamScore.WaveFileName, ExamScore);
                }
                else
                {
                    GlobalUser.ErrScoreInfo[ExamScore.WaveFileName].SubmitNum = 999;
                }
                return;
            }


            GlobalUser.DoneScore = ScoreType.Scoring;//正在评分

            #region << Start 参数 >>

            BaseEngineStartUpParameter startParameter = null;

            startParameter = GetStartParam();

            startParameter.audioType = "wav";//"mp3";
            startParameter.sampleRate = 16000;
            startParameter.compress = "raw";

            #endregion

            //引擎的启动
            var rec = string.Empty;
            callback = ScoreCallBack;
            ES.Start(startParameter, ref rec, callback, IntPtr.Zero, GlobalUser.USER.Mobile ?? "YYS_PC_User");

            using (Stream str = new FileStream(ExamScore.WaveFileName, FileMode.Open, FileAccess.Read))
            {
                var headBuffer = new byte[44];

                var buffer = new byte[1024];
                //var mm = new MemoryStream();
                var len = 0;

                str.Read(headBuffer, 0, headBuffer.Length);//读取前44字节

                while ((len = str.Read(buffer, 0, buffer.Length)) > 0)
                {
                    //mm.Write(buffer, 0, len);
                    ES.Feed(buffer);
                }
            }
            ES.Stop();
        }

        /// <summary>
        /// 评测 传参
        /// </summary>
        /// <returns></returns>
        private BaseEngineStartUpParameter GetStartParam()
        {
            BaseEngineStartUpParameter startParameter;

            switch (ExamScore.EType)
            {
                case EngineType.PARA:
                case EngineType.OPEN:
                    startParameter = new EngineStartUpQsOpenParameter()
                    {
                        EType = ExamScore.EType,
                        QsClass = EngineQClass.JUNIOR,//默认初中
                        QsType = ExamScore.QsType,//EngineQsType.SCENEASK,//EngineQsType.READING,
                        Precision = ExamScore.AnswerModel.precision,//1,
                        Scale = Math.Abs(ExamScore.AnswerModel.item_score),//100,
                        RefText = ExamScore.AnswerModel.item_answer,
                        KeyWords = "",
                        paragraph_need_word_score = 1,
                        getParam = 1,
                        attachAudioUrl = 1
                    };
                    break;
                case EngineType.WORD:
                    startParameter = new EngineStartUpSingleParameter()
                    {
                        EType = ExamScore.EType,
                        QsClass = EngineQClass.JUNIOR,//默认初中
                        //QsType = ExamScore.QsType,//EngineQsType.SCENEASK,//EngineQsType.READING,
                        Precision = ExamScore.AnswerModel.precision,//1,
                        Scale = Math.Abs(ExamScore.AnswerModel.item_score),//100,
                        RefText = ExamScore.AnswerModel.item_answer,
                        paragraph_need_word_score = 1,
                        getParam = 1,
                        attachAudioUrl = 1
                    };
                    break;
                case EngineType.SENT:
                case EngineType.CHOICE:
                    startParameter = new EngineStartUpSingleParameter()
                    {
                        EType = ExamScore.EType,
                        QsClass = EngineQClass.JUNIOR,//默认初中
                        //QsType = ExamScore.QsType,//EngineQsType.SCENEASK,//EngineQsType.READING,
                        Precision = ExamScore.AnswerModel.precision,//1,
                        Scale = Math.Abs(ExamScore.AnswerModel.item_score),//100,
                        RefText = ExamScore.AnswerModel.item_answer,
                        paragraph_need_word_score = 1,
                        getParam = 1,
                        attachAudioUrl = 1
                    };
                    break;
                default:
                    startParameter = new EngineStartUpQsOpenParameter()
                    {
                        EType = ExamScore.EType,
                        QsClass = EngineQClass.JUNIOR,//默认初中
                        QsType = ExamScore.QsType,//EngineQsType.SCENEASK,//EngineQsType.READING,
                        Precision = ExamScore.AnswerModel.precision,//1,
                        Scale = Math.Abs(ExamScore.AnswerModel.item_score),//100,
                        RefText = ExamScore.AnswerModel.item_answer,
                        KeyWords = "",
                        paragraph_need_word_score = 1,
                        getParam = 1,
                        attachAudioUrl = 1
                    };
                    break;
            }
            return startParameter;
        }

        private int ScoreCallBack(IntPtr usrdata, string record_id, int type, byte[] message, int size)
        {
            string resultStr = String.Empty;
            try
            {
                if (type == SKEGN_MESSAGE_TYPE_JSON)
                {
                    resultStr = Encoding.UTF8.GetString(message);

                    if (!string.IsNullOrEmpty(resultStr))
                    {
                        Log4NetHelper.Info("-----------------------------评分解析-----------------------------");
                        Log4NetHelper.Info(resultStr);
                        Log4NetHelper.Info("-----------------------------评分结束-----------------------------");
                        //6.保存答案
                        UpdateAnswer(resultStr);
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                ErrMsgLog msgInfo = new ErrMsgLog();
                msgInfo.user = GlobalUser.USER.Mobile;
                msgInfo.msginfo = "评分Callback异常";
                msgInfo.error = ex;
                msgInfo.scoreJson = resultStr;
                msgInfo.isErr = true;

                //var errStr = $"[user:{GlobalUser.USER?.Mobile},error:{ex},scoreJson: {resultStr}]";
                Log4NetHelper.ErrorFormat(msgInfo.ToJson());
                Log4NetHelper.Error("再评一次");
                //todo
                if (_isAgain)
                {
                    AutoScore();
                    _isAgain = false;
                }
                else
                {
                    GlobalUser.DoneScore = ScoreType.ScoreFailure; //提交评分 失败了

                    if (!GlobalUser.ErrScoreInfo.ContainsKey(ExamScore.WaveFileName))
                    {
                        ExamScore.SubmitNum++;
                        GlobalUser.ErrScoreInfo.Add(ExamScore.WaveFileName, ExamScore);
                    }

                    Log_Data log = new Log_Data();
                    log.log_desc = Log_Type.PC_Score.ToString();
                    log.log_text = msgInfo.ToJson();
                    log.log_device = JsonHelper.ToJson(GlobalUser.MACHINEINFO.GetDevice());
                    WebApiProxy.GetHtmlRespInfo(log, ApiType.SysLog, null, "Post");
                }
            }
            finally
            {

                if (OnScoreFinished != null)
                {
                    OnScoreFinished.Invoke();
                }

                //DelEngine();
            }
            return 0;
        }

        public void DelEngine()
        {
            try
            {
                EngineProvider.DeleteEngine();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion


        private void UpdateAnswer(string scoreJson)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                string errStr = "";
                ErrMsgLog msgInfo = new ErrMsgLog();
                msgInfo.user = GlobalUser.USER.Mobile;

                if (GlobalUser.ErrScoreInfo==null) GlobalUser.ErrScoreInfo = new Dictionary<string, ExamScoreNavigateMessage>();

                try
                {
                    if (!GlobalUser.DoAnswer)
                    {
                        //errStr = $"评分失败 -- result: 当前不是做题过程!";
                        msgInfo.msginfo = "评分失败";
                        msgInfo.error = "当前不是做题过程";
                        msgInfo.isErr = true;
                        GlobalUser.DoneScore = ScoreType.NoScore;
                        Log4NetHelper.Error(msgInfo.ToJson());
                        return;
                    }

                    ScoreRoot score = JsonHelper.FromJson<ScoreRoot>(scoreJson.Replace("\"params\"", "\"param\""));

                    if (!string.IsNullOrEmpty(score.errId))
                    {
                        //20009   网络无效
                        //20013   评分超时
                        //20010   调用顺序出错
                        //42003   请求顺序出错
                        //可以重评
                        if (_errIds.Contains(score.errId))
                            ExamScore.SubmitNum++;
                        else
                            ExamScore.SubmitNum = 999;

                        if (!GlobalUser.ErrScoreInfo.ContainsKey(ExamScore.WaveFileName))
                            GlobalUser.ErrScoreInfo.Add(ExamScore.WaveFileName, ExamScore);
                        throw new Exception($"评分失败 errID:{score.errId}");
                    }

                    string scoreStr = "0";

                    if (ExamScore.EType == EngineType.CHOICE)
                    {
                        var answer =
                            ExamScore.AnswerModel.item_answer.Split(new string[] {"|"}, StringSplitOptions.None);

                        ExamScore.AnswerModel.item_answer = answer[answer.Length - 1];

                        if (score.result.confidence >= (ExamScore.AnswerModel.item_score * 0.45) &&
                            ExamScore.AnswerModel.item_answer.ToLower().Trim()
                                .Equals(score.result.recognition.ToLower().Trim()))
                        {
                            scoreStr = ExamScore.AnswerModel.item_score.ToString("f2");
                        }
                    }
                    else
                    {
                        scoreStr = score.result.overall
                            .ToString(); //((score.result.overall / 100) * ExamScore.AnswerModel.item_score).ToString("f2");
                    }

                    ExamScore.AnswerModel.user_answer = scoreJson.TrimEnd('\0');

                    ExamScore.AnswerModel.exam_score = Convert.ToSingle(scoreStr); //Convert.ToSingle(scoreStr);

                    if (ExamScore.AnswerModel.item_score < 0)
                        ExamScore.AnswerModel.exam_score = Math.Abs(ExamScore.AnswerModel.item_score);

                    var result = WebApiProxy.GetHtmlRespInfo(ExamScore.AnswerModel, ApiType.SubmitRecord,
                        GlobalUser.USER.Token); 

                    Log4NetHelper.Info($"完成评分 -- result:{result.retCode == 1} {result.retCode} {result.retMsg}");

                    //Log4NetHelper.Info($"完成评分 -- 音频文件: {ExamScore.WaveFileName}");

                    GlobalUser.DoneItemExam = true;
                    GlobalUser.DoneScore = ScoreType.ScoreSuccess; //评分
                    if (result?.retCode != 0)
                    {
                        //errStr = $"errmsg:上传评分失败,result:{result.retCode} {result.retCode} {result.retMsg}";
                        msgInfo.msginfo = "上传评分失败";
                        msgInfo.error = $"{result.retCode} {result.retCode} {result.retMsg}";
                        msgInfo.isErr = true;

                        GlobalUser.DoneScore = ScoreType.ScoreFailure;
                        Log4NetHelper.Error(msgInfo.ToJson());

                        if (result.retCode == 4001 && result.retMsg.Contains("密码"))
                        {
                            //回到登录
                            GlobalUser.CleanUp();
                            Messenger.Default.Send(new ExamScoreNavigateMessage(), "LoginFailure");
                            //errStr += ",LoginFailure:已在其他设备登录!";
                            msgInfo.error += ",已在其他设备登录";
                            msgInfo.isErr = true;
                        }
                    }
                    else
                    {
                        msgInfo.isErr = false;
                        if (GlobalUser.ErrScoreInfo != null &&GlobalUser.ErrScoreInfo.Count>0)
                        {
                            //移除异常记录
                            if (GlobalUser.ErrScoreInfo.ContainsKey(ExamScore.WaveFileName))
                                GlobalUser.ErrScoreInfo.Remove(ExamScore.WaveFileName);
                            else
                            {
                                foreach (var key in GlobalUser.ErrScoreInfo.Keys)
                                {
                                    if (GlobalUser.ErrScoreInfo.Values.ToList().Where(w =>
                                        w.AnswerModel.exam_attend_id == GlobalUser.AttendPaperItemId &&
                                        w.AnswerModel.item_id == ExamScore.AnswerModel.item_id).Any())
                                    {
                                        GlobalUser.ErrScoreInfo.Remove(key);
                                        break;
                                    }
                                }
                            }

                            GlobalUser.UpdateErrScore(GlobalUser.USER.Mobile,
                                GlobalUser.SelectPaperNumber,
                                GlobalUser.AttendPaperItemId);

                        }

                        if(File.Exists(Path.Combine(ExamScore.WaveFileName)))
                            File.Delete(ExamScore.WaveFileName);

                        if (GlobalUser.MenuType == MenuType.Sync)
                        {
                            //更新 当前 同步 评分
                            Messenger.Default.Send(
                                new SyncScoreCallbckMessage(ExamScore.ModelTarget, score, ExamScore.EType),
                                ExamScore.ModelTarget);
                            //更新 左侧列表 得分
                            Messenger.Default.Send(
                                new SyncUploadScoreCallbckMessage(ExamScore.ModelTarget, Convert.ToInt32(score.result.overall), ExamScore.AnswerModel.item_id),
                                "SyncUploadScoreNavi");

                        }
                    }
                }
                catch (Exception ex)
                {
                    //errStr = $"[user:{GlobalUser.USER?.Mobile},error:{ex},scoreJson: {scoreJson}]";
                    msgInfo.msginfo = "上传评分失败";
                    msgInfo.error = ex;
                    msgInfo.scoreJson = scoreJson;
                    msgInfo.isErr = true;

                    Log4NetHelper.Error(msgInfo.ToJson());
                    //Log4NetHelper.Error($"scoreJson: {scoreJson}");
                    Log4NetHelper.Error($"record url: {ExamScore.WaveFileName}");
                }
                finally
                {
                    if (msgInfo.isErr)
                    {
                        GlobalUser.DoneScore = ScoreType.ScoreFailure;

                        if (!GlobalUser.ErrScoreInfo.ContainsKey(ExamScore.WaveFileName))
                        {
                            ExamScore.SubmitNum++;
                            GlobalUser.ErrScoreInfo.Add(ExamScore.WaveFileName, ExamScore);
                        }

                        GlobalUser.UpdateErrScore(GlobalUser.USER.Mobile,
                            GlobalUser.SelectPaperNumber,
                            GlobalUser.AttendPaperItemId);

                        Log_Data log = new Log_Data();
                        log.log_desc = Log_Type.PC_Score.ToString();
                        log.log_text = msgInfo.ToJson();
                        log.log_device = JsonHelper.ToJson(GlobalUser.MACHINEINFO.GetDevice());
                        WebApiProxy.GetHtmlRespInfo(log, ApiType.SysLog, null, "Post");
                    }
                }
            }));
        }
    }
}

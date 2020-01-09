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
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using ST.Models.Score;
using ST.Common;

namespace Plugin.Exam.Qs.Common
{
    public class ExamSubItemScoreVM
    {
        private bool _isAgain = true;

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
            GlobalUser.DoneScore = ScoreType.NoScore;//正在评分

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
            ES.Start(startParameter, ref rec, callback, IntPtr.Zero, "user");

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
                        Precision = 0.1,//1,
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
                        Precision = 0.1,//1,
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
                        Precision = 0.1,//1,
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
                        Precision = 0.1,//1,
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
                Log4NetHelper.ErrorFormat($"评分失败,ScoreCallBack 异常,Json:{resultStr}", ex);
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
                try
                {

                    if (GlobalUser.DoAnswer)
                    {
                        ScoreRoot score = JsonHelper.FromJson<ScoreRoot>(scoreJson.Replace("\"params\"", "\"param\""));

                        string scoreStr = "0";

                        if (ExamScore.EType == EngineType.CHOICE)
                        {
                            var answer =
                                ExamScore.AnswerModel.item_answer.Split(new string[] {"|"}, StringSplitOptions.None);

                            ExamScore.AnswerModel.item_answer = answer[answer.Length - 1];

                            if (score.result.confidence >= 45 ||
                                ExamScore.AnswerModel.item_answer == score.result.recognition)
                            {
                                scoreStr = ExamScore.AnswerModel.item_score.ToString("000.00");
                            }
                        }
                        else
                        {
                            scoreStr =
                                ((score.result.overall / 100) * ExamScore.AnswerModel.item_score).ToString("000.00");
                        }

                        ExamScore.AnswerModel.user_answer = scoreJson.TrimEnd('\0');

                        ExamScore.AnswerModel.exam_score = score.result.overall; //Convert.ToSingle(scoreStr);

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
                            GlobalUser.DoneScore = ScoreType.ScoreFailure;
                            Log4NetHelper.Error(
                                $"上传评分失败 -- result:{result.retCode == 1} {result.retCode} {result.retMsg}");
                        }
                    }
                    else
                    {
                        GlobalUser.DoneScore = ScoreType.NoScore;
                        Log4NetHelper.Error($"评分失败 -- result: 当前不是做题过程!");
                    }
                }
                catch (Exception ex)
                {
                    GlobalUser.DoneScore = ScoreType.NoScore;
                    Log4NetHelper.Error($"评分失败 -- result: 当前不是做题过程!", ex);
                    Log4NetHelper.Error($"scoreJson: {scoreJson}");
                    Log4NetHelper.Error($"record url: {ExamScore.WaveFileName}");
                }
            }));
        }
    }
}

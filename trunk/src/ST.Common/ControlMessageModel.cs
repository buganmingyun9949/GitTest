using System;
using System.IO;
using System.Windows;
using Framework.Engine;
using ST.Models.Api;
using ST.Models.Score;
using VoiceRecorder.Audio;

namespace ST.Common
{
    public class ControlMessageModel
    {
    }

    public class ExamQsCmdMessage
    {
        public int PlayingTime { get; private set; }
        public int TotalTime { get; private set; }
        public string PromptCommandText { get; private set; }
        public ExamQsCmdMessage() { }

        public ExamQsCmdMessage(int playingTime = 1, int totalTime = 1, string promptCommandText = "命令")
        {
            this.PlayingTime = playingTime;
            this.TotalTime = totalTime;
            this.PromptCommandText = promptCommandText;
        }
    }
    public class ExamQsSampleAggregatorMessage
    {
        public SampleAggregator SampleAggregator { get; private set; }
        public ExamQsSampleAggregatorMessage() { }

        public ExamQsSampleAggregatorMessage(SampleAggregator sampleAggregator)
        {
            this.SampleAggregator = sampleAggregator;
        }
    }

    public class ExamNextQsNavigateMessage
    {
        public int NextQsIndex { get; private set; }

        public int InfoIndex { get; private set; }

        public int ItemIndex { get; private set; }

        public ExamNextQsNavigateMessage(int qsIndex, int infoIndex, int itemIndex)
        {
            this.NextQsIndex = qsIndex;
            this.InfoIndex = infoIndex;
            this.ItemIndex = itemIndex;
        }
    }

    public class ExamScoreNavigateMessage
    {
        public string ModelTarget { get; private set; }
        public SubmitRecordModel AnswerModel { get; private set; }

        public string WaveFileName { get; private set; }

        public string EType { get; private set; }

        public EngineQsType QsType { get; private set; }
        public string QType { get; private set; }
        public int SubmitNum { get; set; }

        public ExamScoreNavigateMessage() { }
        public ExamScoreNavigateMessage(SubmitRecordModel answerModel, string waveFileName,
           EngineQsType qsType, string eType = EngineType.OPEN, string qType = null)
        {
            this.AnswerModel = answerModel;
            this.WaveFileName = waveFileName;
            this.EType = eType;
            this.QsType = qsType;
            this.QType = qType;
            this.SubmitNum = 1;
        }
        public ExamScoreNavigateMessage(string modelTarget, SubmitRecordModel answerModel, string waveFileName,
            EngineQsType qsType, string eType = EngineType.OPEN, string qType = null)
        {
            this.ModelTarget = modelTarget;
            this.AnswerModel = answerModel;
            this.WaveFileName = waveFileName;
            this.EType = eType;
            this.QsType = qsType;
            this.QType = qType;
            this.SubmitNum = 1;
        }
    }

    public class SyncUploadScoreCallbckMessage
    {
        public string ModelTarget { get; private set; }

        public int Overall { get; private set; }

        public string Item_Id { get; private set; }

        public SyncUploadScoreCallbckMessage() { }

        /// <summary>
        /// 返回 评分结果
        /// 刷新 评测页面信息
        /// </summary>
        /// <param name="modelTarget">对象 类型  ViewModel 名称</param>
        /// <param name="scoreResult">评分对象</param>
        /// <param name="eType">评测 题型 EngineType.WORD</param>
        public SyncUploadScoreCallbckMessage(string modelTarget, int overall, string item_id)
        {
            this.ModelTarget = modelTarget;
            this.Overall = overall;
            this.Item_Id = item_id;
        }
    }

    public class SyncScoreCallbckMessage
    {
        public string ModelTarget { get; private set; }

        public ScoreRoot ScoreResult { get; private set; } 

        public string EType { get; private set; } 

        public SyncScoreCallbckMessage() { }

        /// <summary>
        /// 返回 评分结果
        /// 刷新 评测页面信息
        /// </summary>
        /// <param name="modelTarget">对象 类型  ViewModel 名称</param>
        /// <param name="scoreResult">评分对象</param>
        /// <param name="eType">评测 题型 EngineType.WORD</param>
        public SyncScoreCallbckMessage(string modelTarget, ScoreRoot scoreResult,  string eType)
        {
            this.ModelTarget = modelTarget;
            this.ScoreResult = scoreResult;
            this.EType = eType; 
        }
    }
}
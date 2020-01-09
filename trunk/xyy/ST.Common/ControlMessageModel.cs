using System;
using System.IO;
using System.Windows;
using Framework.Engine;
using ST.Models.Api;
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
        public SubmitRecordModel AnswerModel { get; private set; }

        public string WaveFileName { get; private set; }

        public string EType { get; private set; }

        public EngineQsType QsType { get; private set; }
        public string QType { get; private set; }

        public ExamScoreNavigateMessage(SubmitRecordModel answerModel, string waveFileName,
           EngineQsType qsType, string eType = EngineType.OPEN, string qType = null)
        {
            this.AnswerModel = answerModel;
            this.WaveFileName = waveFileName;
            this.EType = eType;
            this.QsType = qsType;
            this.QType = qType;
        }
    }
}
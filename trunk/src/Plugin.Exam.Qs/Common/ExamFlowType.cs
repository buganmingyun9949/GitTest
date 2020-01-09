namespace Plugin.Exam.Qs.Common
{
    public enum ExamFlowType
    {
        /// <summary>
        /// 题目旁白 音频
        /// </summary>
        TitleAudio = 0, //题目旁白 音频
        /// <summary>
        /// 提示 音频0
        /// </summary>
        PromptAudio, //提示 音频0
        /// <summary>
        /// 提示 音频1
        /// </summary>
        PromptAudio1, //提示 音频1
        /// <summary>
        /// 提示 音频2
        /// </summary>
        PromptAudio2, //提示 音频2
        /// <summary>
        /// 提示 音频3
        /// </summary>
        PromptAudio3, //提示 音频3
        /// <summary>
        /// 提示 音频4
        /// </summary>
        PromptAudio4, //提示 音频4
        /// <summary>
        /// 提示 音频5
        /// </summary>
        PromptAudio5, //提示 音频5
        /// <summary>
        /// 间隔 提示 音
        /// </summary>
        TipAudio, //间隔 提示 音
        /// <summary>
        /// 间隔 提示 音
        /// </summary>
        ItemTipAudio, //间隔 提示 音
        /// <summary>
        /// 间隔 时间
        /// </summary>
        TipTime, //间隔 时间
        /// <summary>
        /// 题目 音频
        /// </summary>
        QuestionInfoAudio, //题目 音频
        /// <summary>
        /// 题目 音频
        /// </summary>
        QuestionAudio, //题目 音频
        /// <summary>
        /// 题目 音频 第二遍
        /// </summary>
        QuestionAudioAgain, //题目 音频 第二遍
        /// <summary>
        /// 题目2 音频
        /// </summary>
        Question1Audio, //题目2 音频
        /// <summary>
        /// 题目2 音频 第二遍
        /// </summary>
        Question1AudioAgain, //题目2 音频 第二遍
        /// <summary>
        /// 录音后 互动 音频
        /// </summary>
        UnderQuestionAudio, //录音后 互动 音频
        /// <summary>
        /// 准备时间
        /// </summary>
        PrepareTime, //准备时间
        /// <summary>
        /// 准备时间1
        /// </summary>
        PrepareTime1, //准备时间1
        /// <summary>
        /// 准备时间2
        /// </summary>
        PrepareTime2, //准备时间
        /// <summary>
        /// 答题时间
        /// </summary>
        AnswerTime, //答题时间
        /// <summary>
        /// 录音时间
        /// </summary>
        RecordTime, //录音时间
        /// <summary>
        /// 开始录音音频
        /// </summary>
        StartRecordAudio,//开始录音音频
        /// <summary>
        /// 停止录音音频
        /// </summary>
        StopRecordAudio,//停止录音音频
        /// <summary>
        /// 开始录音前提醒音频
        /// </summary>
        DiAudio,//开始录音前提醒音频
        /// <summary>
        /// 下一题
        /// </summary>
        NextQs, //下一题
    }

    public enum RecordState
    {
        /// <summary>
        /// 未开始
        /// </summary>
        UnRecord = 0,

        /// <summary>
        /// 录音中
        /// </summary>
        Recording,

        /// <summary>
        /// 停止录音  完成录音
        /// </summary>
        StopRecord,
    }
}
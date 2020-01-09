using System.ComponentModel;

namespace Framework.Engine
{
    public class EngineType
    {
        ///// <summary>
        ///// 短文朗读
        ///// </summary>
        //[Description("短文朗读")]
        //pred = 1,

        ///// <summary>
        ///// 情景对话
        ///// </summary>
        //[Description("情景对话")]
        //scne = 2,

        ///// <summary>
        ///// 话题简述
        ///// </summary>
        //[Description("看图说话")]
        //pict = 4,

        ///// <summary>
        ///// 看图说话
        ///// </summary>
        //[Description("口头作文")]
        //oesy = 8,

        ///// <summary>
        ///// 句子评测
        ///// </summary>
        //[Description("句子评测")]
        //sent = 16,

        ///// <summary>
        ///// 英文有限分支识别
        ///// </summary>
        //[Description("有限分支识别")]
        //rec

        ///// <summary>
        ///// en.sent.score
        ///// </summary>
        //[Description("en.sent.score")]
        //SENT = 0,

        ///// <summary>
        ///// en.word.score
        ///// </summary>
        //[Description("en.word.score")]
        //WORD,

        ///// <summary>
        ///// en.sent.rec
        ///// </summary>
        //[Description("en.sent.rec")]
        //REC,

        ///// <summary>
        ///// en.open
        ///// </summary>
        //[Description("en.open")]
        //OPEN,

        ///// <summary>
        ///// en.grammar
        ///// </summary>
        //[Description("en.grammar")]
        //GRAMMAR,


        /// <summary>
        /// sent.eval
        /// 英文句子评测( 传音频和单词文本，返回单词得分)
        /// </summary>
        [Description("英文句子评测( 传音频和单词文本，返回单词得分)")]
        public const string SENT = "sent.eval";

        /// <summary>
        /// word.eval
        /// 英文单词评测(传音频和句子文本，返回句子得分)
        /// </summary>
        [Description("英文单词评测(传音频和句子文本，返回句子得分)")]
        public const string WORD = "word.eval";

        /// <summary>
        /// choice.rec
        /// 英文句子识别(传音频和多个句子文本(用竖线｜隔开)，返回音频中读到的句子和得分)
        /// </summary>
        [Description("英文句子识别(传音频和多个句子文本(用竖线｜隔开)，返回音频中读到的句子和得分)")]
        public const string CHOICE = "choice.rec";

        /// <summary>
        /// open.eval
        /// 开放题（考试技术：段落朗读、情景问答、口头作文、看图说话、故事复述、句子翻译等）
        /// </summary>
        [Description("开放题（考试技术：段落朗读、情景问答、口头作文、看图说话、故事复述、句子翻译等）")]
        public const string OPEN = "open.eval";

        /// <summary>
        /// grammar.rec
        /// 语法题
        /// </summary>
        [Description("语法题")]
        public const string GRAMMAR = "grammar.rec";

        /// <summary>
        /// para.eval
        /// 朗读短文
        /// </summary>
        [Description("朗读短文")]
        public const string PARA = "para.eval";

        /// <summary>
        /// asr.rec
        /// 英文自由识别(传音频，返回识别出的文本)
        /// </summary>
        [Description("英文自由识别(传音频，返回识别出的文本)")]
        public const string ASR = "asr.rec";


    }

    public enum EngineQClass
    {
        /// <summary>
        /// 小学
        /// </summary>
        [Description("小学")]
        PRIMARY = 0,

        /// <summary>
        /// 初中
        /// </summary>
        [Description("初中")]
        JUNIOR = 1,

        /// <summary>
        /// 高中
        /// </summary>
        [Description("高中")]
        HIGH = 2,

        /// <summary>
        /// 四级
        /// </summary>
        [Description("四级")]
        CET4 = 3,

        /// <summary>
        /// 六级
        /// </summary>
        [Description("六级")]
        CET6 = 4
    }

    public enum EngineQsType
    {
        /// <summary>
        /// 短文朗读
        /// </summary>
        [Description("短文朗读")]
        READING = 0,

        /// <summary>
        /// 短文跟读
        /// </summary>
        [Description("短文跟读")]
        REPEAT = 1,

        /// <summary>
        /// 句子翻译
        /// </summary>
        [Description("句子翻译")]
        SENTTRANS = 2,

        /// <summary>
        /// 段落翻译
        /// </summary>
        [Description("段落翻译")]
        PARAGRAPHTRANS = 3,

        /// <summary>
        /// 故事复述
        /// </summary>
        [Description("故事复述")]
        STORY = 4,

        /// <summary>
        /// 看图说话
        /// </summary>
        [Description("看图说话")]
        PICTALK = 5,

        /// <summary>
        /// 情景问答
        /// </summary>
        [Description("情景问答")]
        SCENEASK = 6,

        /// <summary>
        /// 口头作文
        /// </summary>
        [Description("口头作文")]
        COMPOSITION = 7,
        /// <summary>
        /// 朗读答案
        /// </summary>
        [Description("朗读答案")]
        LDDA = 15,

        /// <summary>
        /// 非OPEN题型
        /// </summary>
        [Description("非OPEN题型")]
        NOTOPEN = 100,



        /// <summary>
        /// 无效
        /// </summary>
        [Description("无效")]
        ERR = 999
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Plugin.Exam.Qs.Common
{
    public enum QuestionType
    {
        /// <summary>
        /// 听力选答(单题)
        /// </summary>
        [Description("听力选答")]
        HearingSingleChoice = 1,

        /// <summary>
        /// 听力选图(单题)
        /// </summary>
        [Description("听力选图")]
        HearingSinglePicChoice = 2,

        /// <summary>
        /// 听长对话选答
        /// </summary>
        [Description("听长对话选答")]
        HearingDiaChoice = 3,

        /// <summary>
        /// 听力短文选答
        /// </summary>
        [Description("听力短文选答")]
        HearingEssayChoice = 4,

        /// <summary>
        /// 朗读短文
        /// </summary>
        [Description("朗读短文")]
        SpokenPred = 5,

        /// <summary>
        /// 短文跟读
        /// </summary>
        [Description("短文跟读")]
        SpokenPflw = 6,

        /// <summary>
        /// 句子跟读|朗读句子(短句)
        /// </summary>
        [Description("句子跟读")]
        SpokenSentence = 7,

        /// <summary>
        /// 句子复述
        /// </summary>
        [Description("句子复述")]
        SpokenSrtl = 8,

        /// <summary>
        /// 情景对话
        /// </summary>
        [Description("情景对话")]
        SpokenScne = 9,

        /// <summary>
        /// 故事复述
        /// </summary>
        [Description("故事复述")]
        SpokenPrtl = 10,

        /// <summary>
        /// 看图说话
        /// </summary>
        [Description("看图说话")]
        SpokenPict = 11,

        /// <summary>
        /// 口头作文
        /// </summary>
        [Description("口头作文")]
        SpokenOesy = 12,

        /// <summary>
        /// 读单词|看图读单词
        /// </summary>
        [Description("看图读单词")]
        ReadWords = 13,

        /// <summary>
        /// 问题回答(情景问答) 单个题目呈现
        /// </summary>
        [Description("问题回答(情景问答)")]
        SpokenScneSingle = 14,

        /// <summary>
        /// 朗读答案(根据提示信息,说出正确答案)
        /// </summary>
        [Description("朗读答案(根据提示信息,说出正确答案)")]
        SpokenRightAnswer = 15,

        /// <summary>
        /// 跟读模仿(模仿跟读 短文下的每句)
        /// </summary>
        [Description("跟读模仿(模仿跟读 短文下的每句)")]
        RepeatImitate = 16,

        /// <summary>
        /// 朗读(复述)填空(信息转述)
        /// </summary>
        [Description("朗读填空(信息转述)")]
        SpokenAndText = 17,

        /// <summary>
        /// 信息获取
        /// </summary>
        [Description("信息获取(信息转述)")]
        ObtainInfo = 18,

        /// <summary>
        /// 信息获取
        /// </summary>
        [Description("看图简述(高中)")]
        FigureSketch = 20,

        /// <summary>
        /// 情景问答/快速应答(高中)
        /// </summary>
        [Description("情景问答/快速应答(高中)")]
        ScneQA_G = 21,

        /// <summary>
        /// 回答和简述(高中-上海)
        /// </summary>
        [Description("回答和简述(高中-上海)")]
        AnswerSketch = 22,

        /// <summary>
        /// 三问五答(高中-广东)
        /// </summary>
        [Description("三问五答(高中-广东)")]
        Q3A5 = 23,

        /// <summary>
        /// 看图回答问题(鄂尔多斯 小学)
        /// </summary>
        [Description("看图回答问题(鄂尔多斯 小学)")]
        SpokenScnePic = 24,

        /// <summary>
        /// 信息转述及询问
        /// </summary>
        [Description("信息转述及询问")]
        ObtainInfoAsk = 37,

        /// <summary>
        /// 朗读答案(根据提示信息,听句子，选择正确答语)
        /// </summary>
        [Description("朗读答案(根据提示信息,听句子，选择正确答语)")]
        SpokenRightAnswer2 = 38,

        /// <summary>
        /// 听短文判断句子填空
        /// </summary>
        [Description("莲林 听短文判断句子填空")]
        LLType1101 = 1101,

        /// <summary>
        /// 听短文判断图片填空
        /// </summary>
        [Description("莲林 听短文判断图片填空")]
        LLType1102 = 1102,

        /// <summary>
        /// 单词补全
        /// </summary>
        [Description("莲林 单词补全")]
        LLType1201 = 1201,

        /// <summary>
        /// 字母补全
        /// </summary>
        [Description("莲林 字母补全")]
        LLType1205 = 1205,

        /// <summary>
        /// 听录音选词填空
        /// </summary>
        [Description("莲林 听录音选词填空")]
        LLType1203 = 1203,

        /// <summary>
        /// 听录音看图填空
        /// </summary>
        [Description("莲林 听录音看图填空")]
        LLType1204 = 1204,

        /// <summary>
        /// 看单词读单词
        /// </summary>
        [Description("莲林 看单词读单词")]
        LLType1301 = 1301,

        /// <summary>
        /// 看句子读句子
        /// </summary>
        [Description("莲林 看句子读句子")]
        LLType1302 = 1302,

        /// <summary>
        /// 看句子回答
        /// </summary>
        [Description("莲林 看句子回答")]
        LLType1303 = 1303,

        /// <summary>
        /// ERR 10001
        /// </summary>
        [Description("10001")]
        PaperInfo = 10001,

        /// <summary>
        /// ERR 10002
        /// </summary>
        [Description("10002")]
        AreaInfo = 10002,
    }
}

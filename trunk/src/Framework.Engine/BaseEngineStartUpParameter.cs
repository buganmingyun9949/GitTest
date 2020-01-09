using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Engine.Models.MetaParameters;

namespace Framework.Engine
{
    public abstract class BaseEngineStartUpParameter
    {
        /// <summary>
        /// 引擎类型
        /// </summary>
        public string EType { get; set; }

        /// <summary>
        /// 可选，分制，默认100，取值范围(0-100],影响范围包括各评分维度分数
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// 可选，得分精度，默认1，建议取值的范围(0,1]
        /// </summary>
        public double Precision { get; set; }

        /// <summary>
        /// 适合级别
        /// </summary>
        public EngineQClass QsClass { get; set; }

        /// <summary>
        /// 题目类型
        /// </summary>
        public EngineQsType QsType { get; set; }

        /// <summary>
        /// // 必须, 云端支持:wav, mp3, flv, ogg, amr格式, 本地支持: wav格式
        /// </summary>
        public string audioType { get; set; }

        /// <summary>
        /// // 必须, 采样率, 必须与音频本身采样率一致，不同语音服务有不同的要求,要求16k
        /// </summary>
        public int sampleRate { get; set; }

        /// <summary>
        /// 可选, 音频压缩配置,speex:speex压缩(默认配置), raw:不压缩
        /// </summary>
        public string compress { get; set; }

        /// <summary>
        /// 可选，返回结果是否带请求参数，默认1返回，0不返回
        /// </summary>
        public int getParam { get; set; }

        /// <summary>
        /// 可选，para.eval精确到单词得分
        /// </summary>
        public int paragraph_need_word_score { get; set; }

        /// <summary>
        /// 使用云服务时可选，指定服务器使返回结果附带音频下载地址
        /// </summary>
        public int attachAudioUrl { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class Log_Device
    {
        /// <summary>
        /// 平台名称   Windows  Android IOS
        /// </summary>
        [DefaultValue("Windows")]
        public string platform { get; set; }
        /// <summary>
        /// 软件 版本
        /// </summary>
        public string currentVersion { get; set; }

        /// <summary>
        /// 分辨率  宽
        /// </summary>
        public string width { get; set; }

        /// <summary>
        /// 分辨率 高
        /// </summary>
        public string height { get; set; }

        public string apiLevel { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public string brand { get; set; }

        /// <summary>
        /// 品牌编号
        /// </summary>
        public string buildNumber { get; set; }

        public string bundleId { get; set; }

        public string deviceId { get; set; }

        /// <summary>
        /// 驱动名称
        /// </summary>
        public string deviceName { get; set; }

        /// <summary>
        /// 物理内存容量
        /// </summary>
        public string maxMemory { get; set; }

        /// <summary>
        /// 系统版本
        /// </summary>
        public string systemVersion { get; set; }

        /// <summary>
        /// 唯一ID
        /// </summary>
        public string uniqueId { get; set; }

        /// <summary>
        /// 软件 版本
        /// </summary>
        public string version { get; set; }
    }

    public class Log_Data
    {
        /// <summary>
        /// 日志描述
        /// </summary>
        public string log_desc { get; set; }

        /// <summary>
        /// 日志详情
        /// </summary>
        public string log_text { get; set; }

        /// <summary>
        /// 设备详情[Log_Device]
        /// </summary>
        public string log_device { get; set; }
    }

    public enum Log_Type
    {
        /// <summary>
        /// 登录
        /// </summary>
        PC_Login =0,
        /// <summary>
        /// 运行
        /// </summary>
        APP_RUN,

        /// <summary>
        /// 注销-退出
        /// </summary>
        PC_Logout,

        /// <summary>
        /// 评分 异常
        /// </summary>
        PC_Score,

        /// <summary>
        /// 完成异常
        /// </summary>
        PC_Complete_Error,

        /// <summary>
        /// 其他 异常
        /// </summary>
        PC_Error,
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Engine.CustomExceptions
{
    public enum EngineErrorCode
    {
        /// <summary>
        /// 创建失败
        /// </summary>
        Create,

        /// <summary>
        /// 启动失败
        /// </summary>
        Start,

        /// <summary>
        /// 填充数据失败
        /// </summary>
        Feed,

        /// <summary>
        /// 回调失败
        /// </summary>
        CallBack,
    }
}

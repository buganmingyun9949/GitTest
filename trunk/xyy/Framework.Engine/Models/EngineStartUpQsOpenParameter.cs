using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Engine.Models
{
    public class EngineStartUpQsOpenParameter : BaseEngineStartUpParameter
    {
        /// <summary>
        /// 参考文本
        /// </summary>
        public string RefText { get; set; }

        /// 关键字
        /// </summary>
        public string KeyWords { get; set; }
    }
}

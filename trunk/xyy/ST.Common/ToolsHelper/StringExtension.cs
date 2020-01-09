using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ST.Common.ToolsHelper
{
    public static class StringExtension
    {
        /// <summary>
        /// 将指定的字符串转换为词首字母小写。
        /// </summary>
        /// <param name="str">需要转换的字符串。</param>
        /// <returns>首字母小写的字符串。</returns>
        public static string ToTitleLower(this string str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                return str.Substring(0, 1).ToLower() + str.Substring(1, str.Length - 1);
            }
            return str;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ST.Common.ToolsHelper
{
    /// <summary>
    /// 操作正则表达式的公共类
    /// </summary>    
    public class RegexHelper
    {
        #region 验证输入字符串是否与模式字符串匹配
        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>        
        public static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="options">筛选条件</param>
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }
        #endregion


        /// <summary>
        /// 验证电话号码的主要代码如下
        /// </summary>
        /// <param name="str_telephone"></param>
        /// <returns></returns>
        public bool IsTelephone(string str_telephone)
        {
            return IsMatch(str_telephone, @"^(\d{3,4}-)?\d{6,8}$");
        }

        /// <summary>
        /// 验证手机号码的主要代码如下
        /// </summary>
        /// <param name="str_handset"></param>
        /// <returns></returns>
        public static bool IsHandset(string str_handset)
        {
            return IsMatch(str_handset, @"^[1]+[3,4,5,6,7,8,9]+\d{9}");
        }

        /// <summary>
        /// 验证身份证号的主要代码如下
        /// </summary>
        /// <param name="str_idcard"></param>
        /// <returns></returns>
        public static bool IsIDcard(string str_idcard)
        {
            return IsMatch(str_idcard, @"(^\d{17}(?:\d|x)$)|(^\d{15}$)");
        }

        /// <summary>
        /// 验证输入为数字的主要代码如下
        /// </summary>
        /// <param name="str_number"></param>
        /// <returns></returns>
        public static bool IsNumber(string str_number)
        {
            return IsMatch(str_number, @"^[0-9]*$");
        }

        /// <summary>
        /// 验证邮编的主要代码如下
        /// </summary>
        /// <param name="str_postalcode"></param>
        /// <returns></returns>
        public static bool IsPostalcode(string str_postalcode)
        {
            return IsMatch(str_postalcode, @"^\d{6}$");
        }
    }
}

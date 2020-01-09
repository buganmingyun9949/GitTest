using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ST.Common.ToolsHelper
{
    /// <summary>
    /// 强制转化辅助类(无异常抛出)
    /// </summary>
    public static class ConvertHelper
    {
        #region 强制转化
        /// <summary>
        /// object转化为Bool类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ObjToBool(this object obj)
        {
            bool flag;
            if (obj == null)
            {
                return false;
            }
            if (obj.Equals(DBNull.Value))
            {
                return false;
            }
            return (bool.TryParse(obj.ToString(), out flag) && flag);
        }
        /// <summary>
        /// object强制转化为DateTime类型(吃掉异常)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? ObjToDateNull(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            try
            {
                return new DateTime?(Convert.ToDateTime(obj));
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }
        /// <summary>
        /// int强制转化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ObjToInt(this object obj)
        {
            if (obj != null)
            {
                int num;
                if (obj.Equals(DBNull.Value))
                {
                    return 0;
                }
                if (int.TryParse(obj.ToString(), out num))
                {
                    return num;
                }
            }
            return 0;
        }
        /// <summary>
        /// 强制转化为long
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long ObjToLong(this object obj)
        {
            if (obj != null)
            {
                long num;
                if (obj.Equals(DBNull.Value))
                {
                    return 0;
                }
                if (long.TryParse(obj.ToString(), out num))
                {
                    return num;
                }
            }
            return 0;
        }
        /// <summary>
        /// 强制转化可空int类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int? ObjToIntNull(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj.Equals(DBNull.Value))
            {
                return null;
            }
            return new int?(ObjToInt(obj));
        }
        /// <summary>
        /// 强制转化为string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjToStr(this object obj)
        {
            if (obj == null)
            {
                return "";
            }
            if (obj.Equals(DBNull.Value))
            {
                return "";
            }
            return Convert.ToString(obj);
        }
        /// <summary>
        /// Decimal转化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ObjToDecimal(this object obj)
        {
            if (obj == null)
            {
                return 0M;
            }
            if (obj.Equals(DBNull.Value))
            {
                return 0M;
            }
            try
            {
                return Convert.ToDecimal(obj);
            }
            catch
            {
                return 0M;
            }
        }
        /// <summary>
        /// Decimal可空类型转化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal? ObjToDecimalNull(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj.Equals(DBNull.Value))
            {
                return null;
            }
            return new decimal?(ObjToDecimal(obj));
        }
        #endregion

        #region Image

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]

        public static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// 从bitmap转换成ImageSource
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static ImageSource ChangeBitmapToImageSource(Bitmap bitmap)
        {
            //Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                //throw new System.ComponentModel.Win32Exception();
            }

            return wpfBitmap;
        }

        #endregion


        #region Emun

        public static string GetEnumDescription(Enum enumValue)
        {
            string str = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            DescriptionAttribute da = (DescriptionAttribute)objs[0];
            return da.Description;
        }

        #endregion

        #region 中英文标点转换



        // '定义一个中文标点的数组对象  
        static string[] ChineseInterpunction =
             {"。", "，", "；", "：", "？", "！", "‘", "’", "“", "”", "……", "—", "～", "（", "）", "《", "》"};
        //'定义一个英文标点的数组对象 
        static string[] EnglishInterpunction =
            {".", ",", ";", ":", "?", "!", "'", "'", "\"", "\"", "…", "-", "~", "(", ")", "<", ">"};

        public static string Zh2En(this string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            for (int i = 0; i < ChineseInterpunction.Length; i++)
            {
                text = text.Replace(ChineseInterpunction[i], EnglishInterpunction[i]);
            }

            return text;
        }


        /// <summary>
        /// 转半角的函数(DBC case)
        ///任意字符串
        ///半角字符串
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String ToDBC(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new String(c);
        }


        /// <summary>
        /// 转全角的函数(SBC case) 
        /// 任意字符串 
        /// 全角字符串
        /// 全角空格为12288,半角空格为32 
        /// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248    
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(this string input)
        { //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288; continue;
                }
                if (c[i] < 127) c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }


        #endregion
    }
}

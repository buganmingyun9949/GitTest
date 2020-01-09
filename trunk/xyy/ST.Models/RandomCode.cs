using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Models
{
    /// <summary>
    /// 生成随机验证码
    /// </summary>
    public static class RandomCode
    {
        /// <summary>
        /// 返回一个N位验证码
        /// </summary>
        /// <param name="N">位数</param>
        /// <returns></returns>
        public static string RandomCodeCommand(int N)
        {
            string code = "";
            Random random = new Random();
            for (int i = 0; i < N; i++)
            {
                code += random.Next(9);
            }
            return code;
        }
    }
}

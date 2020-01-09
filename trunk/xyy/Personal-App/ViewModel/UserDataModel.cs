using ST.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personal_App.ViewModel
{
    public class UserDataModel
    {
        /// <summary>
        /// 获取或设置用户名称。
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置手机号码。
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 头像。
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 获取或设置访问令牌。
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 获取或设置用户信息。
        /// </summary>
        public UserInfo Data { get; set; }


    }
}

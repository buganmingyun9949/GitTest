using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class UserInfo
    {
        #region << 用户信息 >>

        /// <summary>
        /// 用户编号。
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 电话号码。
        /// </summary>
        public string Moblie { get; set; }

        /// <summary>
        /// 用户名。
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 头像。
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 用户状态。
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 更新时间。
        /// </summary>
        public string UpdateTime { get; set; }

        /// <summary>
        /// 登录成功标识。
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 卡名。
        /// </summary>
        public string MyStudyCard { get; set; }

        #endregion

        public string Token { get; set; }

        public int Expire_status { get; set; }

        public Card Card;

        public List<Card> CardsList;
    }
}

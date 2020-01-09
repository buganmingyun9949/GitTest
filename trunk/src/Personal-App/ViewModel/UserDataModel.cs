using ST.Models.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// 登录密码。
        /// </summary>
        public string Password { get; set; }

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
        /// 首次打开
        /// </summary>
        [DefaultValue(false)]
        public bool UnFirstOpen { get; set; }

        /// <summary>
        /// 获取或设置用户信息。
        /// </summary>
        public UserInfo Data { get; set; }

        /// <summary>
        /// 获取或设置用户班级信息。
        /// </summary>
        public ClassInfo ClassData { get; set; }

        /// <summary>
        /// 卡片信息。 包含菜单详情
        /// </summary>
        public Study_Card StudyCard { get; set; }

        /// <summary>
        /// 获取或设置用户年级信息
        /// </summary>
        public GradeInfo GradeData { get; set; }

        /// <summary>
        /// 作业记录
        /// </summary>
        public List<TaskInfo> UserZy { get; set; }

    }
}

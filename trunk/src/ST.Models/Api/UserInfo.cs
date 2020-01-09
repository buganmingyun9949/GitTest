using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public string Mobile { get; set; }

        private string _ShowPartMobile;
        /// <summary>
        /// 电话号码。
        /// </summary>
        public string ShowPartMobile
        {
            get
            {
                return string.IsNullOrEmpty(Mobile)
              ? ""
              : Mobile.Substring(0, 3) + "***" + Mobile.Substring(7);
            }
        }

        /// <summary>
        /// 用户名。
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码。
        /// </summary>
        public string Password { get; set; }

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
        //public string AccessToken { get; set; }

        /// <summary>
        /// 卡名。
        /// </summary>
        public string MyStudyCard { get; set; }

        #endregion

        public string Token { get; set; }

        public int Expire_status { get; set; }

        public int RetCode { get; set; }

        /// <summary>
        /// 学习卡
        /// </summary>
        //public Card Card { get; set; }

        /// <summary>
        /// 学习卡 们
        /// </summary>
        public List<Card> CardsList { get; set; }

        /// <summary>
        /// 作业记录
        /// </summary>
        public List<TaskInfo> UserZy { get; set; }

        /// <summary>
        /// 导航菜单信息
        /// </summary>
        public dynamic study_card;

        /// <summary>
        /// 班级信息
        /// </summary>
        public dynamic class_info;

        /// <summary>
        /// 首次打开
        /// </summary>
        [DefaultValue(false)]
        public bool UnFirstOpen { get; set; }
    }

    public class Study_Card
    {
        public string used_time { get; set; }
        public string card_key { get; set; }
        public int expire_status { get; set; }
        public string expire_time { get; set; }
        public string agent_id { get; set; }
        public int grade { get; set; }
        public int card_type { get; set; }
        public int card_auth { get; set; }
        public string card_price { get; set; }
        public string card_name { get; set; }
        public dynamic card_setting { get; set; }
    }

    public class Card_Setting
    {

        /// <summary>
        /// 菜单模版类型
        /// 1:按照数值计算得到菜单详情,默认用户本地样式,,,card_modules & (1<<1) > 0 ：单词跟读
        /// 2:转为 实际菜单对象
        /// 其他 不显示菜单
        /// </summary>
        public int module_type { get; set; }
        public dynamic card_modules { get; set; }
    }

    public class Card_Module
    {
        public int unit_id { get; set; }
        public string unit_name { get; set; }
        public string unit_pic { get; set; }
        public string unit_short_name { get; set; }
        public string unit_desc { get; set; }
        public string unit_icon { get; set; }
    }

    public class UserReport
    {
        /// <summary>
        /// 用户输入的反馈内容
        /// </summary>
        public string report_content { get; set; }

        /// <summary>
        /// 用户上传截图地址，通过上传图片接口返回，多个用英文逗号隔开
        /// 先上传图片  再提交反馈
        /// </summary>
        public string report_imgs { get; set; }

        /// <summary>
        /// 反馈分类，文本型，具体查看产品设计文档
        /// 默认填写  意见反馈
        /// </summary>
        public string report_category { get; set; } = "意见反馈";

        /// <summary>
        /// 用户设备信息，设备信息的结构参数名称见日志反馈接口
        /// 添加 设备json
        /// </summary>
        public string report_device_info { get; set; }

        /// <summary>
        /// 1001英语说学生IOS
        /// 1002英语说学生安卓
        /// 1003英语说学生PC
        /// 1004英语说老师PCWEB
        /// 1005英语说老师WXAPP
        /// </summary>
        public int report_product { get; set; }

    }


    /// <summary>
    /// 用户的年级信息
    /// </summary>
    public class UserGradeInfoModel
    {
        public bool? showGradeList { get; set; }

        public string schoolSystem { get; set; }

        public string selected_grade { get; set; }

        public List<UserGradeList> gradeList { get; set; }
    }

    /// <summary>
    /// 用户年级信息
    /// </summary>
    public class UserGradeList
    {
        public int id { get; set; }

        public string name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{
    public class RespObject
    {
        #region old  ---> 不再使用

        #region << 验证码 >>
        public string IdentifyingCode { get; set; }

        #endregion

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

        #region << 学习卡 >>

        /// <summary>
        /// 学习卡列表。
        /// </summary>
        public List<CardsListItem> CardsList { get; set; }
        /// <summary>
        /// 默认学习卡。
        /// </summary>
        public Card Card { get; set; }

        #endregion

        #region << 绑卡 >>

        /// <summary>
        /// 是否绑卡，0：未绑定，1：绑定。
        /// </summary>
        public int IsBindCard { get; set; }

        /// <summary>
        /// 默认卡号对应的题库包 Id。
        /// </summary>
        public string CardPackageId { get; set; }

        #endregion

        #region 试卷列表...

        public List<SimulationItem> SimulationList { get; set; }


        #endregion

        #region << 继续模考 >>
        /// <summary>
        /// 继续模考 独享
        /// </summary>
        public ContinueExamItem ExamProgressInfo { get; set; }

        #endregion

        #region << 在做一次 >>

        /// <summary>
        /// 
        /// </summary>
        public string SimulationId { get; set; }

        #endregion

        #region << 评分结果 >>

        public List<ExamScoreItem> ExamScoreList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float DefeatPercent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float TotalScore5Points { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float TotalScore { get; set; }

        #endregion

        #region 验证试卷...



        #endregion

        #region 返回结果...

        /// <summary>
        /// 
        /// </summary>
        public Result Result { get; set; }

        #endregion

        #endregion


        #region << 验证码 >>

        /// <summary>
        /// 用户身份，每次请求需要身份认证的接口时需要加入到url参数
        /// </summary>
        public string token { get; set; }

        #endregion

        #region << 登录 >>

        /// <summary>
        /// 学习卡状态 -1未绑定 0 已绑定已失效 1已绑定未失效
        /// </summary>
        public int expire_status{ get; set; }
        public object retData{ get; set; }

        #endregion

        #region << 卡片 >>

        public string card_id { get; set; }
        public string card_key { get; set; }
        public string is_used { get; set; }
        public string is_current { get; set; }
        public string used_time { get; set; }
        public string used_user_id { get; set; }
        public string expire_time { get; set; }
        public string agent_id { get; set; }
        public string grade { get; set; }
        public string ctime { get; set; }
        public string card_type { get; set; }


        #endregion
    }
}

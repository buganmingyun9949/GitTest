using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ST.Models.Api
{

    public enum ApiType
    {
        /// <summary>
        /// 发送验证码。
        /// </summary>
        SendCode = 0,
        /// <summary>
        /// 获取验证码。
        /// </summary>
        GetIdentifyingCode,
        /// <summary>
        /// 用户注册。
        /// </summary>
        UserReg,
        /// <summary>
        /// 用户找回密码。
        /// </summary>
        UserPwdFind,
        /// <summary>
        /// 用户登录。
        /// </summary>
        UserLogin,        
        /// <summary>
        /// 用户登录。
        /// </summary>
        UserLogout,
        /// <summary>
        /// 用户信息。
        /// </summary>
        UserInfo,
        /// <summary>
        /// 更新用户信息。
        /// </summary>
        ModifyUserInfo,
        /// <summary>
        /// 更新用户名称。
        /// </summary>
        ModifyUserName,
        /// <summary>
        /// 是否绑卡。
        /// </summary>
        IsBindCard,
        /// <summary>
        /// 用户绑卡。
        /// </summary>
        UserBindCard,
        /// <summary>
        /// 我的学习卡列表。
        /// </summary>
        CardList,
        /// <summary>
        /// 首页试卷列表。
        /// </summary>
        GetSimulationList,
        /// <summary>
        /// 试卷 json 清单。
        /// 参数  :试卷ID：exam_id   题目ID：item_id
        /// </summary>
        GetPaperInfoDetail,
        /// <summary>
        /// 开始考试。
        /// 参数  :试卷ID：exam_id   题目ID：item_id
        /// </summary>
        BeginExam,
        /// <summary>
        /// 完成考试。
        /// 参数  :试卷ID：exam_id   题目ID：item_id
        /// </summary>
        CompleteExam,
        /// <summary>
        /// 提交做题记录接口。
        /// </summary>
        SubmitRecord,
        /// <summary>
        /// 继续模考接口。
        /// </summary>
        ContinueExam,
        /// <summary>
        /// 重新模考接口。
        /// </summary>
        RedoExam,
        /// <summary>
        /// 验证模拟题状态接口。
        /// </summary>
        VerifySimulation,
        /// <summary>
        /// 获取模考总成绩接口。
        /// </summary>
        GetSimulationTotalScore,
        /// <summary>
        /// 获取用户答案。
        /// </summary>
        GetUserPracticeResult,

    }

    /// <summary>
    /// 获取指定数据。
    /// 只传 token
    /// </summary>
    public class TokenModel
    {
        public string token { get; set; }
    }

    /// <summary>
    /// 获取验证码。
    /// </summary>
    public class SendCodeModel
    {
        public string phone { get; set; }
    }


    /// <summary>
    /// 用户 注册。
    /// 短信验证码
    /// </summary>
    public class UserRegModel
    {
        public int role { get; set; } =1;

        public string phone { get; set; }
        public string phone_code { get; set; }

        public string password { get; set; }

        /// <summary>
        /// 用户姓名，如不填则同手机号码
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 推荐码，老师用户必填
        /// </summary>
        //public string agent_code { get; set; }
    }

    /// <summary>
    /// 通过手机找回密码
    /// </summary>
    public class UserPwdFindModel
    {
        public string phone { get; set; }

        /// <summary>
        /// 手机验证码

        /// </summary>
        public string phone_code { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 用户角色，学生1，老师2
        /// </summary>
        public int role { get; set; } = 1;

        /// <summary>
        /// 设备类型，固定为IOS、ANDRIOD、PC、WEB、WXAPP
        /// </summary>
        public string device { get; set; } = "PC";
    }

    /// <summary>
    /// 手机号码密码登录
    /// </summary>
    public class UserLoginModel
    {
        public string phone { get; set; }

        public string password { get; set; }

        /// <summary>
        /// 用户角色，学生1，老师2
        /// </summary>
        public int role { get; set; } = 1;

        /// <summary>
        /// 设备类型，固定为IOS、ANDRIOD、PC、WEB、WXAPP
        /// </summary>
        public string device { get; set; } = "PC";
    }

    /// <summary>
    /// 更新用户信息。
    /// </summary>
    public class ModifyUserInfoModel
    {
        /// <summary>
        /// 初始化 <see cref="ModifyUserInfoModel"/> 类的新实例。
        /// </summary>
        public ModifyUserInfoModel()
        {
        }

        /// <summary>
        /// 新用户名。
        /// </summary>
        public string user_name { get; set; }

    }

    /// <summary>
    /// 更新用户名称。
    /// </summary>
    public class ModifyUserNameModel
    {
        /// <summary>
        /// 初始化 <see cref="ModifyUserNameModel"/> 类的新实例。
        /// </summary>
        public ModifyUserNameModel()
        {
        }

        /// <summary>
        /// 新用户名。
        /// </summary>
        public string newUserName { get; set; }

    }

    /// <summary>
    /// 绑定学习卡。
    /// </summary>
    public class UserBindCardModel
    {
        public string card_key { get; set; }
        //public string token { get; set; }
    }

    /// <summary>
    /// 首页 试卷列表
    /// </summary>
    public class GetSimulationListModel
    {

        /// <summary>
        /// 试卷类型，2：听说模拟 3：听说专项
        /// 是否可选: 否
        /// </summary>
        public int paper_type { get; set; }

        /// <summary>
        /// 试卷类型为3的时候必填
        /// 是否可选: 是
        /// </summary>
        public int? qs_type { get; set; }

        /// <summary>
        /// 默认10条结果
        /// 是否可选: 是
        /// </summary>
        public int? pagesize { get; set; }

        /// <summary>
        /// 默认第一页
        /// 是否可选: 是
        /// </summary>
        public int? page { get; set; }

        public string token { get; set; }
    }

    /// <summary>
    /// 首页 试卷 题目清单
    /// </summary>
    public class GetPaperInfoDetail
    {

        /// <summary>
        /// 试卷ID
        /// </summary>
        public string exam_id { get; set; }

        /// <summary>
        /// 题目ID
        /// </summary>
        public string item_id { get; set; }

        public string token { get; set; }
    }

    /// <summary>
    /// 首页 试卷列表
    /// </summary>
    public class GetSimulationDetailModel
    {
        /// <summary>
        /// 试卷ID
        /// </summary>
        public string exam_id { get; set; }

        public string token { get; set; }
    }

    /// <summary>
    /// 开始考试 接口。
    /// </summary>
    public class BeginExamModel
    {
        /// <summary>
        /// 试卷ID。
        /// </summary>
        public string exam_id { get; set; }

        public string token { get; set; }
    }

    /// <summary>
    /// 完成考试 接口。
    /// </summary>
    public class CompleteExamModel
    {
        /// <summary>
        /// 批次 ID。
        /// </summary>
        public string exam_attend_id { get; set; }

        public string token { get; set; }
    }

    /// <summary>
    /// 验证模拟题状态。
    /// </summary>
    public class VerifySimulationModel
    {
        /// <summary>
        /// 模拟题编号。
        /// </summary>
        public string SimulationId { get; set; }
    }

    /// <summary>
    /// 提交做题记录接口
    /// </summary>
    public class SubmitRecordModel
    {
        /// <summary>
        /// 练习记录ID，在获取练习详情中可获得,如练习详情为null，则需要调用开始练习接口开始一次新的练习
        /// </summary>
        public string exam_attend_id { get; set; }
        /// <summary>
        /// 小题题目ID
        /// </summary>
        public string item_id { get; set; }
        /// <summary>
        /// 	题目总分满分
        /// </summary>
        public float item_score { get; set; }
        /// <summary>
        /// 题目得分，目前由客户端评分
        /// </summary>
        public float exam_score { get; set; }
        /// <summary>
        /// 答题内容，口语题请提交评分结果json字符串，填空选择题提交输入或选择的内容
        /// </summary>
        public string user_answer { get; set; }
        /// <summary>
        /// 题目答案，题目的正确参考答案
        /// </summary>
        public string item_answer { get; set; }
        /// <summary>
        /// 答题类型，1、选择题，3、填空题，2、口语题
        /// </summary>
        public int item_answer_type { get; set; }
        /// <summary>
        /// 题目ID
        /// </summary>
        public string qs_id { get; set; }
        /// <summary>
        /// 题目类型
        /// </summary>
        public string qs_type { get; set; }
        /// <summary>
        /// 题号
        /// </summary>
        public int item_no { get; set; }
    }

    /// <summary>
    /// 继续模考接口
    /// </summary>
    public class ContinueExam
    {
        public string simulationId { get; set; }
    }

    /// <summary>
    /// 重新模考接口
    /// </summary>
    public class RedoModel
    {
        public string simulationId { get; set; }
    }

    /// <summary>
    /// 重新模考接口
    /// </summary>
    public class UserPracticeResultModel
    {
        public string examId { get; set; }
    }
}

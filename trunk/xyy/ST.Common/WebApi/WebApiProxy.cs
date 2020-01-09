using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using Framework.Logging;
using Framework.Network.HttpHelper;
using Framework.Network.HttpHelper.Enum;
using ST.Common.ToolsHelper;
using ST.Models.Api;

namespace ST.Common.WebApi
{
    public class WebApiProxy
    {
        public const string TOKEN = "saltaNzM6pvv1W";

        // 远程接口地址
        public const string APIURL = "https://api.365speak.cn";//"https://api.cp.17kouyu.com";//
        // 媒体文件 地址
        public const string MEDIAURL = "http://exam.17kouyu.com";

        //发送验证码
        public const string SENDCODE = "/v2/account/login/send-code";
        //获取验证码
        public const string GETIDENTIFYINGCODE = "/pcUser/v1/getIdentifyingCode.shtml";//废弃
        /// <summary>
        /// 用户注册。
        /// </summary>
        public const string USERREG = "/v2/account/reg";
        /// <summary>
        /// 用户找回密码。
        /// </summary>
        public const string USERPWDFIND = "/v2/account/find";
        /// <summary>
        /// 用户登录。
        /// </summary>
        public const string USERLOGIN = "/v2/account/login";
        /// <summary>
        /// 用户注销。
        /// </summary>
        public const string USERLOGOUT = "/user/v2/deleteAccessToken.shtml";
        /// <summary>
        /// 更新用户信息。
        /// </summary>
        public const string USERINFO = "/v2/student/info";
        /// <summary>
        /// 更新用户信息。
        /// </summary>
        public const string MODIFYUSERINFO = "/v2/user/info/save";
        /// <summary>
        /// 更新用户名称。
        /// </summary>
        public const string MODIFYUSERNAME = "/user/v1/modifyUserName.shtml";
        //用户是否绑定学习卡
        public const string ISBINDCARD = "/card/v1/isBindCard.shtml";
        //用户绑定学习卡
        public const string USERBINDCARD = "/v2/student/study-card/bind";
        //我的学习卡列表
        public const string CARDLIST = "/v2/student/study-card/list";
        /// <summary>
        /// 提交做题记录接口。
        /// </summary>
        public const string SUBMITRECORD = "/v2/student/exercise/save";
        //首页接口
        public const string GETSIMULATIONLIST = "/v2/student/exercise/list";
        //首页接口
        public const string GETPAPERINFODETAIL = "/v2/student/exercise/detail";
        //开始考试
        public const string BEGINEXAM = "/v2/student/exercise/attend/start";
        //完成考试
        public const string COMPLETEEXAM = "/v2/student/exercise/attend/end";
        //继续模考接口
        public const string CONTINUEEXAM = "/simulation/v1/continueExam.shtml";
        //重新模考接口
        public const string REDOEXAM = "/simulation/v1/redo.shtml";
        /// <summary>
        /// 验证模拟题状态接口。
        /// </summary>
        public const string VERIFYSIMULATION = "/simulation/v1/verifySimulationStatus.shtml";
        /// <summary>
        /// 获取模考总成绩。
        /// </summary>
        public const string GETSIMULATIONTOTALSCORE = "/simulation/v1/getSimulationTotalScore.shtml";
        /// <summary>
        /// 获取用户答案。
        /// </summary>
        public const string GETUSERPRACTICERESULT = "/specialTraining/v1/getUserPracticeResult.shtml";


        public static string MD5 = "";

        private static string GetReqParam<T>(T model)
        {
            if (model == null) return "";

            StringBuilder reqParam = new StringBuilder();

            PropertyInfo[] propertys = model.GetType().GetProperties();
            foreach (PropertyInfo pinfo in propertys)
            {
                if (pinfo.GetValue(model, null) != null)
                {
                    if (reqParam.Length > 2)
                    {
                        reqParam.Append("&");
                    }
                    //CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                    //TextInfo textInfo = cultureInfo.TextInfo;

                    //reqParam.Append($"{pinfo.Name}={pinfo.GetValue(model, null)}");

                    if (char.IsLower(pinfo.Name[0]))
                    {
                        reqParam.Append($"{pinfo.Name}={pinfo.GetValue(model, null)}");
                    }
                    else
                    {
                        reqParam.Append($"{pinfo.Name.ToTitleLower()}={pinfo.GetValue(model, null)}");
                    }
                    //reqParam.Append($"{pinfo.Name.ToTitleLower()}={pinfo.GetValue(model, null)}");
                }
            }

            return reqParam.ToString();
        }

        /// <summary>
        /// 生成 协议码
        /// </summary>
        /// <param name="phonenum"></param>
        public static void SetMd5(string phonenum)
        {
            MD5 = Md5Helper.GetMd5Hash(phonenum + TOKEN).ToUpper();
        }

        /// <summary>
        /// 连接 接口 返回 result:trus?false?
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool GetHtmlInfo<T>(T model, ApiType _apiType, string _accessToken = null, string _Method = "Post")
        {

            string url = GetLinkUrl(_apiType);

            RespCode result = LinkServer(url, GetReqParam(model), _accessToken, _Method);

            return result.retCode == 1;
        }

        /// <summary>
        /// 连接 接口 返回 完整信息-RespCode
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static RespCode GetHtmlRespInfo<T>(T _model, ApiType _apiType, string _accessToken = null, string _Method = "Post")
        {
            string url = GetLinkUrl(_apiType);

            RespCode result = LinkServer(url, GetReqParam(_model),  _accessToken, _Method);

            return result;
        }

        /// <summary>
        /// 连接 接口 返回 完整信息-RespCode
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static RespCode GetHtmlRespInfo(ApiType _apiType,  string _accessToken = null, string _Method = "Post")
        {
            string url = GetLinkUrl(_apiType);

            RespCode result = LinkServer(url,  _accessToken, _Method);

            return result;
        }

        private static string GetLinkUrl(ApiType _apiType)
        {
            string url = "";

            switch (_apiType)
            {
                case ApiType.SendCode:
                    url = $"{APIURL}{SENDCODE}";
                    break;
                case ApiType.GetIdentifyingCode:
                    url = $"{APIURL}{GETIDENTIFYINGCODE}";
                    break;
                case ApiType.UserReg:
                    url = $"{APIURL}{USERREG}";
                    break;
                case ApiType.UserPwdFind:
                    url = $"{APIURL}{USERPWDFIND}";
                    break;
                case ApiType.UserLogin:
                    url = $"{APIURL}{USERLOGIN}";
                    break;
                case ApiType.UserLogout:
                    url = $"{APIURL}{USERLOGOUT}";
                    break;
                case ApiType.ModifyUserInfo:
                    url = $"{APIURL}{MODIFYUSERINFO}";
                    break;
                case ApiType.UserInfo:
                    url = $"{APIURL}{USERINFO}";
                    break;
                case ApiType.ModifyUserName:
                    url = $"{APIURL}{MODIFYUSERNAME}";
                    break;
                case ApiType.UserBindCard:
                    url = $"{APIURL}{USERBINDCARD}";
                    break;
                case ApiType.CardList:
                    url = $"{APIURL}{CARDLIST}";
                    break;
                case ApiType.GetSimulationList:
                    url = $"{APIURL}{GETSIMULATIONLIST}";
                    break;
                case ApiType.GetPaperInfoDetail:
                    url = $"{APIURL}{GETPAPERINFODETAIL}";
                    break;
                case ApiType.BeginExam:
                    url = $"{APIURL}{BEGINEXAM}";
                    break;
                case ApiType.CompleteExam:
                    url = $"{APIURL}{COMPLETEEXAM}";
                    break;
                case ApiType.VerifySimulation:
                    url = $"{APIURL}{VERIFYSIMULATION}";
                    break;
                case ApiType.SubmitRecord:
                    url = $"{APIURL}{SUBMITRECORD}";
                    Log4NetHelper.Info(String.Format("HTTP 请求提交做题记录接口：{0}。", url));
                    break;
                case ApiType.ContinueExam:
                    url = $"{APIURL}{CONTINUEEXAM}";
                    Log4NetHelper.Info(String.Format("HTTP 请求继续模考接口：{0}。", url));
                    break;
                case ApiType.RedoExam:
                    url = $"{APIURL}{REDOEXAM}";
                    //Log4NetHelper.Info(String.Format("HTTP 请求重新模考接口：{0}。", url));
                    break;
                case ApiType.GetSimulationTotalScore:
                    url = $"{APIURL}{GETSIMULATIONTOTALSCORE}";
                    //Log4NetHelper.Info(String.Format("HTTP 请求获取模考成绩接口：{0}。", url));
                    break;
                case ApiType.GetUserPracticeResult:
                    url = $"{APIURL}{GETUSERPRACTICERESULT}";
                    //Log4NetHelper.Info(String.Format("HTTP 请求获取用户答案接口：{0}。", url));
                    break;
                default:
                    url = $"{APIURL}{SENDCODE}";
                    break;
            }

            return url;
        }


        private static RespCode LinkServer(string _url, string _accessToken = null, string _Method = "Post")
        {
            return LinkServer(_url, String.Empty, _accessToken, _Method);
        }

        private static RespCode LinkServer(string _url, string _objModel, string _accessToken = null, string _Method = "Post")
        {
            Log4NetHelper.Info(String.Format("HTTP 请求地址：{0}。", _url));

            Log4NetHelper.Info(String.Format("HTTP 请求参数：{0}。", _objModel));

            //创建Httphelper对象
            HttpHelper http = new HttpHelper();

            WebHeaderCollection header = new WebHeaderCollection();

            if (!string.IsNullOrEmpty(_accessToken))
                //header.Add("accessToken", _accessToken);
                _url = $"{_url}?token={_accessToken}";

            if(_Method.Trim().ToLower()== "get")
                _url = $"{_url}?{_objModel}";

            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = _url,
                Method = _Method,
                Timeout = 1000 * 60 * 6,
                ReadWriteTimeout = 1000 * 60 * 6,
                //Header = header,
                //Encoding = Encoding.Default,
                PostEncoding = Encoding.UTF8,
                //Header = wcHeader,
                ContentType = "application/x-www-form-urlencoded;charset=utf-8", //"application/json",
                PostDataType = PostDataType.Byte,
                //PostdataByte = data,
                Postdata = _objModel, //"nowTick=" + DateTime.Now.Ticks.ToString("x"),
            };

            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            //string cookie = result.Cookie;

            var resp = JsonHelper.FromJson<RespCode>(html);
            Log4NetHelper.Info(String.Format("HTTP 响应内容：{0}。", resp.ToJsonItem()));
             
            return resp;
        }
        public static Stream GetFile(string _url)
        {
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();

            WebHeaderCollection header = new WebHeaderCollection();

            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = _url,
                Timeout = 1000 * 60 * 6,
                ReadWriteTimeout = 1000 * 60 * 6,
                //Header = header,
                //Encoding = Encoding.Default,
                PostEncoding = Encoding.UTF8,
                //Header = wcHeader,
                ContentType = "application/x-www-form-urlencoded;charset=utf-8", //"application/json",
                ResultType = ResultType.Byte,
            };

            //请求的返回值对象
            var result = http.GetHtml(item).ResultByte;
            //string html = result.Html;
            //string cookie = result.Cookie;

            MemoryStream ms = new MemoryStream(result);

            return ms;
        }
    }

}

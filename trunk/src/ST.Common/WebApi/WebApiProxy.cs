using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
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

        public static Stopwatch sw = new Stopwatch();

        // 远程接口地址
        public static string APIURL = "http://api.365speak.cn";//"http://api.cp.17kouyu.com"; //"http://test.api.stkouyu.com:8082";// 
        // 媒体文件 地址
        public const string MEDIAURL = "http://exam.17kouyu.com";
        // 远程接口地址
        public const string STATICURL = "http://static.365speak.cn";//http://static.365speak.cn/word/json/ABC.json

        /// <summary>
        /// 系统日志
        /// </summary>
        public const string SYSLOG = "/v2/sys-log";

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
        /// 更新用户头像。
        /// </summary>
        public const string MODIFYUSERHEAD = "/v2/user/info/upload-avatar";
        /// <summary>
        /// 修改密码。
        /// </summary>
        public const string MODIFYUSERPWD = "/v2/user/password";
        /// <summary>
        /// 修改手机号。
        /// </summary>
        public const string MODIFYUSERPHONE = "/v2/user/info/save-phone";
        /// <summary>
        /// 检查新手机号。
        /// </summary>
        public const string CHECKUSERPHONE = "/v2/account/reg/check";
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
        /// <summary>
        /// 获取作业列表。
        /// </summary>
        public const string GETUSERHOMEWORKLIST = "/v2/student/homework/list";
        /// <summary>
        /// 获取作业详情。
        /// </summary>
        public const string GETUSERHOMEWORKDETAIL = "/v2/student/homework/detail";
        /// <summary>
        /// 完成作业。
        /// </summary>
        public const string COMPLETEUSERTASK = "/v2/student/homework/attend/end";
        /// <summary>
        /// 获取班级列表。
        /// </summary>
        public const string GETTEACHERCLASSLIST = "/v2/student/sclass/query";
        /// <summary>
        /// 绑定班级。
        /// </summary>
        public const string USERBINDCLASS = "/v2/student/sclass/join";
        /// <summary>
        /// 获取专项 题型。
        /// </summary>
        public const string GETTRAINTYPES = "/v2/student/exercise/list/types";

        /// <summary>
        /// 获取 句子跟读单元列表。
        /// </summary>
        public const string GETSYNCUNITS = "/v2/student/exercise/list/units";

        /// <summary>
        /// 获取 句子跟读单元列表。
        /// </summary>
        public const string GETSYNCHISTORY = "/v2/student/exercise/item-history";


        /// <summary>
        /// 创建 学习卡 支付 订单
        /// </summary>
        public const string GETCARDORDER = "/v2/student/order/create";

        /// <summary>
        /// 获取扫码支付二维码
        /// </summary>
        public const string GETQRCODE = "/v2/student/order/pay-qrcode";

        /// <summary>
        /// 代理商信息
        /// </summary>
        public const string GETAGENTINFO = "/v2/user/info/agent";

        /// <summary>
        /// 用户反馈
        /// </summary>
        public const string USERREPORT = "/v2/user/report";

        /// <summary>
        /// 可选年级信息
        /// </summary>
        public const string GETGRADELIST = "/v2/grade/list";
        /// <summary>
        /// 更新我的显示年级信息
        /// </summary>
        public const string SETGRADESELECT = "/v2/grade/select";

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
            MD5 = SecurityHelper.Md5Encrypt(phonenum + TOKEN, Encoding.UTF8).ToUpper();
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

            RespCode result = LinkServer(url, GetReqParam(_model), _accessToken, _Method);

            return result;
        }

        /// <summary>
        /// 连接 接口 返回 完整信息-RespCode
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static Image GetImage<T>(T _model, ApiType _apiType, string _accessToken = null, string _Method = "Post")
        {
            string url = GetLinkUrl(_apiType);

            var img = GetImage(url, GetReqParam(_model), _accessToken, _Method);

            return img;
        }

        /// <summary>
        /// 连接 接口 返回 完整信息-RespCode
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static RespCode GetHtmlRespInfo(ApiType _apiType, string _accessToken = null, string _Method = "Post")
        {
            string url = GetLinkUrl(_apiType);

            RespCode result = LinkServer(url, _accessToken, _Method);

            return result;
        }

        /// <summary>
        /// 连接 接口 返回 完整信息-RespCode
        /// </summary>
        /// <param name="model"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static RespCode GetHtmlFileRespInfo(string filePath, ApiType _apiType, string _accessToken = null,
            string _Method = "Post")
        {
            string url = GetLinkUrl(_apiType);

            RespCode result = UploadFile(url, filePath, _accessToken, _Method);//, PostDataType.FilePath);

            return result;
        }

        private static string GetLinkUrl(ApiType _apiType)
        {
#if DEBUG

            var appSet = ConfigurationManager.AppSettings["TestUrl"];

            if (!string.IsNullOrEmpty(appSet))
            {
                APIURL = appSet;
            }
#endif

            string url = "";

            switch (_apiType)
            {
                case ApiType.SysLog:
                    url = $"{APIURL}{SYSLOG}";
                    break;
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
                case ApiType.ModifyUserHead:
                    url = $"{APIURL}{MODIFYUSERHEAD}";
                    break;
                case ApiType.ModifyUserPwd:
                    url = $"{APIURL}{MODIFYUSERPWD}";
                    break;
                case ApiType.ModifyUserPhone:
                    url = $"{APIURL}{MODIFYUSERPHONE}";
                    break;
                case ApiType.CheckUserPhone:
                    url = $"{APIURL}{CHECKUSERPHONE}";
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
                    url = $"{APIURL}{GETPAPERINFODETAIL}"; //$"{APIURL}{GETPAPERINFODETAIL}";
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
                    url = $"{APIURL}{SUBMITRECORD}"; //$"{APIURL}{SUBMITRECORD}";
                    break;
                case ApiType.ContinueExam:
                    url = $"{APIURL}{CONTINUEEXAM}";
                    break;
                case ApiType.RedoExam:
                    url = $"{APIURL}{REDOEXAM}";
                    break;
                case ApiType.GetSimulationTotalScore:
                    url = $"{APIURL}{GETSIMULATIONTOTALSCORE}";
                    break;
                case ApiType.GetUserPracticeResult:
                    url = $"{APIURL}{GETUSERPRACTICERESULT}";
                    break;
                case ApiType.GetUserHomeworkList:
                    url = $"{APIURL}{GETUSERHOMEWORKLIST}";
                    break;
                case ApiType.GetUserHomeworkDetail:
                    url = $"{APIURL}{GETUSERHOMEWORKDETAIL}";
                    break;
                case ApiType.CompleteUserTask:
                    url = $"{APIURL}{COMPLETEUSERTASK}";
                    break;
                case ApiType.GetTeacherClassList:
                    url = $"{APIURL}{GETTEACHERCLASSLIST}";
                    break;
                case ApiType.UserBindClass:
                    url = $"{APIURL}{USERBINDCLASS}";
                    break;
                case ApiType.GetTrainTypes:
                    url = $"{APIURL}{GETTRAINTYPES}";
                    break;
                case ApiType.GetSyncUnits:
                    url = $"{APIURL}{GETSYNCUNITS}";
                    break;
                case ApiType.GetSyncHistory:
                    url = $"{APIURL}{GETSYNCHISTORY}";
                    break;
                case ApiType.GetCardOrder:
                    url = $"{APIURL}{GETCARDORDER}";
                    break;
                case ApiType.GetQrCode:
                    url = $"{APIURL}{GETQRCODE}";
                    break;
                case ApiType.GetAgentInfo:
                    url = $"{APIURL}{GETAGENTINFO}";
                    break;
                case ApiType.UserReport:
                    url = $"{APIURL}{USERREPORT}";
                    break;
                case ApiType.GetGradeList:
                    url = $"{APIURL}{GETGRADELIST}";
                    break;
                case ApiType.SetGradeSelect:
                    url = $"{APIURL}{SETGRADESELECT}";
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

        private static RespCode LinkServer(string _url, string _objModel, string _accessToken = null,
            string _Method = "Post", PostDataType pdType = PostDataType.Byte)
        {
            sw.Start();
            sw.Restart();

            Log4NetHelper.Debug(String.Format("HTTP 请求地址：{0}。", _url));

            Log4NetHelper.Debug(String.Format("HTTP 请求参数：{0}。", _objModel));

            //创建Httphelper对象
            HttpHelper http = new HttpHelper();

            WebHeaderCollection header = new WebHeaderCollection();

            if (!string.IsNullOrEmpty(_accessToken))
                //header.Add("accessToken", _accessToken);
                _url = $"{_url}?token={_accessToken}";

            if (_Method.Trim().ToLower() == "get" && !string.IsNullOrEmpty(_objModel))
                _url = $"{_url}?{_objModel}";


            //header.Add("User-Agent", $"yys-PlatformID.Windows-{MachineInfoHelper.LD?.currentVersion}");
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = _url,
                Method = _Method,
                Timeout = 1000 * 30,
                ReadWriteTimeout = 1000 * 30,
                //Header = header,
                UserAgent = $"yys-PlatformID.Windows-{MachineInfoHelper.LD?.currentVersion}",
                //Encoding = Encoding.Default,
                PostEncoding = Encoding.UTF8,
                //Header = wcHeader,
                ContentType = "application/x-www-form-urlencoded;charset=utf-8", //"application/json",
                PostDataType = pdType,
                //PostdataByte = fileData,
                Postdata = _objModel, //"nowTick=" + DateTime.Now.Ticks.ToString("x"),
            };

            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            //string cookie = result.Cookie;

            var resp = JsonHelper.FromJson<RespCode>(html);
            Log4NetHelper.Debug(String.Format("HTTP 响应内容：{0}。", html));

            if (result.StatusCode != HttpStatusCode.OK)
            {
                Log4NetHelper.Error(String.Format("HTTP 请求参数：{0}，响应内容 请求失败 ---> {1}。", _objModel, JsonHelper.ToJson(result)));

                if (resp == null)
                {
                    resp = new RespCode()
                    {
                        retCode = 40400,
                        retMsg = "网络连接失败，请检查后重试！",
                        retData = "",
                        retHtml = JsonHelper.ToJson(result)
                    };
                }
                else
                {
                    resp.retCode = 40400;
                    resp.retMsg = "网络连接失败，请检查后重试！";
                    resp.retHtml = JsonHelper.ToJson(result);
                }
            }

            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            Log4NetHelper.Debug(String.Format("LinkServer 执行时间：{0}s。", ts2.TotalSeconds));
            sw.Reset();

            return resp;
        }


        private static Image GetImage(string _url, string _objModel, string _accessToken = null, string _Method = "Post")
        {
            Log4NetHelper.Debug(String.Format("HTTP 请求地址：{0}。", _url));

            Log4NetHelper.Debug(String.Format("HTTP 请求参数：{0}。", _objModel));

            //创建Httphelper对象
            HttpHelper http = new HttpHelper();

            WebHeaderCollection header = new WebHeaderCollection();

            if (!string.IsNullOrEmpty(_accessToken))
                //header.Add("accessToken", _accessToken);
                _url = $"{_url}?token={_accessToken}";

            if (_Method.Trim().ToLower() == "get" && !string.IsNullOrEmpty(_objModel))
                _url = $"{_url}?{_objModel}";
            //?x-oss-process=image/resize,h_100
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = _url,
                Method = _Method,
                Timeout = 1000 * 30,
                ReadWriteTimeout = 1000 * 30,
                //Header = header,
                //Encoding = Encoding.Default,
                PostEncoding = Encoding.UTF8,
                //Header = wcHeader,
                ContentType = "application/x-www-form-urlencoded;charset=utf-8", //"application/json",
                //PostdataByte = fileData,
                Postdata = _objModel, //"nowTick=" + DateTime.Now.Ticks.ToString("x"),
            };

            //请求的返回值对象
            Image result = http.GetImage(item);

            return result;
        }
        public static string GetRedirectUrl(string _url)
        {
            if (_url.Contains("records."))
                return _url;

            _url = _url.Replace("https://", "http://");
            //创建Httphelper对象
            HttpHelper http = new HttpHelper();
            //创建Httphelper参数对象
            HttpItem item = new HttpItem()
            {
                URL = _url,//URL     必需项    
                //Method = "get",//URL     可选项 默认为Get   
                ContentType = "text/html",//返回类型    可选项有默认值   
                Allowautoredirect = false//默认为False就是不根据重定向自动跳转
            };
            //请求的返回值对象
            HttpResult result = http.GetHtml(item);
            //获取请求的Cookie
            string cookie = result.Cookie;

            //获取302跳转URl
            string redirectUrl = result.RedirectUrl;
            return redirectUrl;
        }

        public static byte[] GetAudioFile(string _url)
        {
            try
            {
                _url = _url.Replace("https://", "http://");
                //创建Httphelper对象
                HttpHelper http = new HttpHelper();
                //创建Httphelper参数对象
                HttpItem item = new HttpItem()
                {
                    URL = GetRedirectUrl(_url),//URL     必需项    
                                               //Method = "get",//URL     可选项 默认为Get   
                    ContentType = "text/html",//返回类型    可选项有默认值   
                    Allowautoredirect = false,//默认为False就是不根据重定向自动跳转
                    ResultType = ResultType.Byte
                };
                //请求的返回值对象
                HttpResult result = http.GetHtml(item);

                return result.ResultByte;
            }
            catch (WebException ex)
            {
                Log4NetHelper.Error(ex);
                //GlobalUse.LogError("Upload", "Upload", $"上传失败,json:{strJson}", ex);
                //return false;
                return null;
            }
        }

        private static RespCode UploadFile(string _url, string filePath, string _accessToken = null, string _Method = "Post")
        {
            string strJson = String.Empty;
            var webClient = new WebClient();
            try
            {
                if (!string.IsNullOrEmpty(_accessToken))
                    //header.Add("accessToken", _accessToken);
                    _url = $"{_url}?token={_accessToken}";

                var bytes = webClient.UploadFile(_url, _Method, filePath);
                strJson = Encoding.UTF8.GetString(bytes).Replace("static", "static1");
                var resp = JsonHelper.FromJson<RespCode>(strJson);
                Log4NetHelper.Debug(String.Format("HTTP 响应内容：{0}。", resp.ToJsonItem()));
                webClient.Dispose();
                return resp;
            }
            catch (WebException ex)
            {
                Log4NetHelper.Error(ex);
                //webClient.Dispose();
                //GlobalUse.LogError("Upload", "Upload", $"上传失败,json:{strJson}", ex);
                //return false;
                return null;
            }
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
                Timeout = 1000 * 30,
                ReadWriteTimeout = 1000 * 30,
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


#region 数组组合
        public static byte[] ComposeArrays(byte[] Array1, byte[] Array2)
        {
            byte[] Temp = new byte[Array1.Length + Array2.Length];
            Array1.CopyTo(Temp, 0);
            Array2.CopyTo(Temp, Array1.Length);
            return Temp;
        }

#endregion

#region 图片转Byte数组

        /// <summary>
        /// 图片转Byte数组
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        private static byte[] ImageToBytesFromFilePath(string FilePath)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (Image Img = Image.FromFile(FilePath))
                {
                    using (Bitmap Bmp = new Bitmap(Img))
                    {
                        Bmp.Save(ms, Img.RawFormat);
                    }
                }
                return ms.ToArray();
            }
        }

#endregion

    }

}

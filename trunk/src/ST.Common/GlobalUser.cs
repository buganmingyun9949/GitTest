using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ST.Common.WebApi;
using ST.Models.Api;
using ST.Models.Paper;
using NAudio.Wave;
using ST.Common.ToolsHelper;
using VoiceRecorder.Audio;

namespace ST.Common
{
    public class GlobalUser
    {

        static GlobalUser()
        {
            //WavePlayer = CreateWavePlayer();
        }

        public static Window LoginWin;
        public static Window MainWin;


        public const string APPKEY = "1511140684000046";//Appkey

        public const string SECRETKEY = "bd8f96907789ca51ab9f6edf5c9185df";//Secretkey

        public const string SERVERURI = "ws://api.17kouyu.com:8080";//请求地址

        public const string SERVERTIMEOUT = "300";//请求超时  s:秒

        public const string FILEPWD = "yatour-st-@-2048";//pwd


        /// <summary>
        /// 当前主机硬件信息
        /// </summary>
        public static MachineInfoHelper MACHINEINFO = MachineInfoHelper.I();

        public static UserInfo USER; //登录后用户信息

        public static string VER; //软件版本

        public static ClassInfo CLASSINFO; //登录后用户班级信息

        public static GradeInfo GRADEINFO; //登录后用户同步内容的年级信息

        public static Study_Card STUDYCARD; //菜单信息 导航

        public static AgentInfo AGENTINFO; //代理商信息

        //public static Card USERCARD; //登录后用户卡片信息
        /// <summary>
        /// 是否自动登录。
        /// </summary>
        public static bool AutoLoggedIn = false;
        /// <summary>
        /// 是否登录过期。
        /// </summary>
        public static bool LoginExpired = false;
        //public static Card CARD; //绑定的卡片信息

        public const string DATAFOLDER = "Data";//试卷文件夹

        public static readonly string AUDIODATAFOLDER = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp");//Path.GetTempPath();

        public static string SyncTag;

        /// <summary>
        /// 完成评分 -- (口语评分)返回后 记录评分成功/失败
        /// </summary>
        public static MenuType MenuType = MenuType.Exam;

        public static string SelectPaperJson;//选中试卷 json 文件名

        public static string SelectPaperName;//选中试卷 目录名 试卷名

        public static Paper_Info SelectPaperInfo;//选中试卷 内容详情

        public static float? SelectPaperTotalScore;//选中试卷 总分

        public static float? SelectPaperAvgScore5Points;//选中试卷 5星评分

        /// <summary>
        /// 一参与的试卷试题数据
        /// 结构:json
        /// </summary>
        public static string SelectExamAttendResult;

        /// <summary>
        /// 一参与的试卷试题数据
        /// 评分 汇总
        /// </summary>
        public static Exam_Attend SelectExamAttend;

        /// <summary>
        /// 选中试卷试卷名称（中文显示）。
        /// </summary>
        public static string SelectPaperViewName;

        /// <summary>
        /// 选中试卷试卷编号。
        /// </summary>
        public static string SelectPaperNumber;
        /// <summary>
        /// 批次号
        /// null 表示 还未参与答题。 需要接口获取
        /// </summary>
        public static string AttendPaperItemId;

        /// <summary>
        /// 试卷题目加载正常   true:正常
        /// </summary>
        public static bool PaperQuestionLoaded = true; 

        /// <summary>
        /// 正在做题 -- 是否在 答题操作
        /// </summary>
        public static bool DoAnswer = true;

        /// <summary>
        /// 完成评分 -- (口语评分)返回后 记录评分成功/失败
        /// </summary>
        public static ScoreType DoneScore = ScoreType.NoScore;

        /// <summary>
        /// 题目已做--返回后 试题控件(PaperControlVM) 刷新控制
        /// </summary>
        public static bool DoneItemExam = false;

        public static Dictionary<string, FrameworkElement> ExamViews;

        /// <summary>
        /// 默认音量。
        /// </summary>
        public static float AudioVolume = 0.50f;
        /// <summary>
        /// 默认音量。
        /// </summary>
        public static float RecordingVolume = 0.80f;

        public static IWavePlayer WavePlayer;
        public static AudioFileReader AudioFileReader;
        public static IAudioRecorder Recorder;

        private static IWavePlayer CreateWavePlayer()
        {
            return new WaveOut();
        }

        /// <summary>
        /// 关闭音频播放，释放资源。
        /// </summary>
        public static void AudioCleanUp()
        {
            if (AudioFileReader != null)
            {
                AudioFileReader?.Dispose();
                AudioFileReader = null;
            }
            if (WavePlayer != null)
            {
                WavePlayer?.Stop();
                WavePlayer?.Dispose();
                WavePlayer = null;
            }
        }

        /// <summary>
        /// 清空释放音频资源。
        /// </summary>
        public static void CleanUp()
        {
            if (AudioFileReader != null)
            {
                AudioFileReader?.Dispose();
                AudioFileReader = null;
            }
            if (WavePlayer != null)
            {
                WavePlayer?.Stop();
                WavePlayer?.Dispose();
                WavePlayer = null;
            }

            if (Recorder != null)
            {
                Recorder?.Stop();
                Recorder?.Dispose();
                Recorder = null;
            }
        }

        public static Queue BackgroundTasks { get; set; }

        public static void ExecuteTasks()
        {
            Queue<String> queue = new Queue<string>();
            //queue.Enqueue()
            BackgroundTasks.Enqueue("");
            //var task = BackgroundTasks.Dequeue() as BackgroundTask;
            //WebApiProxy.GetHtmlRespInfo
            //BackgroundTasks.
        }

        public static void UpdateAudioDashboard()
        {

        }

        /// <summary>
        /// 获取用户数据目录。
        /// </summary>
        /// <returns>用户数据目录。</returns>
        public static string GetUserDataFolder()
        {
            string userDataFolder = String.Format("{0}/USER_{1}", DATAFOLDER, USER.UserId);
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, userDataFolder);
        }


        /// <summary>
        /// 异常 评分记录
        /// </summary>
        public static Dictionary<string, ExamScoreNavigateMessage> ErrScoreInfo;

        #region << 评分错误消息记录 >>

        /// <summary>
        /// 更新 异常评分数据
        /// </summary>
        public static void UpdateErrScore(string user,string paperid,string attendid)
        {
            var filePath = Path.Combine(GlobalUser.AUDIODATAFOLDER, $"{user}_{paperid.Split(new[] { "#" }, StringSplitOptions.RemoveEmptyEntries)[0]}_{attendid}");

            if (GlobalUser.ErrScoreInfo == null || GlobalUser.ErrScoreInfo.Count == 0)
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                return;
            }

            for (int i = 0; i < GlobalUser.ErrScoreInfo?.Count; i++)
            {
                if(GlobalUser.ErrScoreInfo[GlobalUser.ErrScoreInfo.Keys.ToList()[i]].AnswerModel?.exam_attend_id == attendid) continue;

                {
                    GlobalUser.ErrScoreInfo.Remove(GlobalUser.ErrScoreInfo.Keys.ToList()[i]);
                    i--;
                }
            }

            //读写文件
            //File
            var errjson = GlobalUser.ErrScoreInfo.ToJson();
            var errjson_e = Base64Provider.AESEncrypt(errjson);
            //byte[] data = System.Text.Encoding.Default.GetBytes(errjson_e);
            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                sw.Write(errjson_e);
            }
        }

        /// <summary>
        /// 读取 错误评分你记录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="paperid"></param>
        /// <param name="attendid"></param>
        public static void GetErrorScoreSource(string user, string paperid, string attendid)
        {
            var filePath = Path.Combine(GlobalUser.AUDIODATAFOLDER, $"{user}_{paperid.Split(new[] { "#" }, StringSplitOptions.RemoveEmptyEntries)[0]}_{attendid}");
            string errjson_e = "";

            if(!File.Exists(filePath)) return; 

            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    errjson_e = sr.ReadToEnd();
                }
            }

            var errjson = Base64Provider.AESDecrypt(errjson_e);

            Dictionary<string, dynamic> dic = JsonHelper.FromJson<Dictionary<string, dynamic>>(errjson);

            if (dic.Count > 0)
            {
                ErrScoreInfo = new Dictionary<string, ExamScoreNavigateMessage>();

                foreach (var key in dic.Keys)
                {
                    var val = dic[key];
                    
                    ExamScoreNavigateMessage enm = new ExamScoreNavigateMessage(JsonHelper.FromJson<SubmitRecordModel>(val.AnswerModel.ToString()),
                        val.WaveFileName.ToString(),
                        Enum.Parse(typeof(Framework.Engine.EngineQsType), val.QsType.ToString()),
                        val.EType.ToString(),
                        val.QType.ToString()
                    );
                    enm.SubmitNum = Int32.Parse(val.SubmitNum.ToString());

                    ErrScoreInfo.Add(key, enm);
                }
            } 
        }

        /// <summary>
        /// 检查 文件
        /// </summary>
        /// <param name="user"></param>
        /// <param name="paperid"></param>
        /// <param name="attendid"></param>
        /// <param name="isDel">是否删除  true 存在则删除 并返回 false</param>
        public static bool CheckErrorScoreSource(string user, string paperid, string attendid, bool isDel = false)
        {
            var filePath = Path.Combine(GlobalUser.AUDIODATAFOLDER, $"{user}_{paperid.Split(new[] { "#" }, StringSplitOptions.RemoveEmptyEntries)[0]}_{attendid}");
            if (File.Exists(filePath))
            {
                if (isDel)
                {
                    File.Delete(filePath);
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}

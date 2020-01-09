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
using VoiceRecorder.Audio;

namespace Personal_App.Common
{
    public class GlobalUser
    {

        static GlobalUser()
        {
            //WavePlayer = CreateWavePlayer();
        }

        public static Window LoginWin;
        public static Window MainWin;


        public const string APPKEY = "151012757600003e";//Appkey

        public const string SECRETKEY = "6c3a3c5be4bdcd5e43093e545bdd1528";//Secretkey

        public const string SERVERURI = "ws://api.17kouyu.com:8080";//请求地址

        public const string SERVERTIMEOUT = "300";//请求超时  s:秒

        public static UserInfo USER; //登录后用户信息

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

        public static string SelectPaperJson;//选中试卷 json 文件名

        public static string SelectPaperName;//选中试卷 目录名 试卷名

        public static Paper_Info SelectPaperInfo;//选中试卷 内容详情

        /// <summary>
        /// 一参与的试卷试题数据
        /// 结构:json
        /// </summary>
        public static string SelectExamAttendResult;

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
        public static bool DoneScore = true;

        /// <summary>
        /// 题目已做--返回后 试题控件(PaperControlVM) 刷新控制
        /// </summary>
        public static bool DoneItemExam = false;

        public static Dictionary<string, FrameworkElement> ExamViews;

        /// <summary>
        /// 默认音量。
        /// </summary>
        public static float AudioVolume = 0.50f;

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

    }
}

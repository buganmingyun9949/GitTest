using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Framework.Logging;
using GalaSoft.MvvmLight;
using MaterialDesignThemes.Wpf;
using Personal_App.Domain;
using Personal_App.ViewModel.Exam;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Paper;

namespace Personal_App.ViewModel
{
    /// <summary>
    /// 下载进度对话框视图模型。
    /// </summary>
    public class DownloadProgressDialogVM : ViewModelBase
    {
        public const string ViewName = "DownloadProgressDialogVM";

        public event DownloadFinishedHandler PaperDownloadComplete;

        public event DownloadFinishedHandler PaperDownloadError;

        public DownloadProgressDialogVM(string savePath)
        {
            FileNameDic =
                JsonHelper.FromJson<Dictionary<string, string>>(GlobalUser.SelectPaperInfo.paper_assets.ToString());

            var myAnswerResults = new List<Exam_Attend_Result_Item>();
            if (!string.IsNullOrEmpty(GlobalUser.SelectExamAttendResult?.Trim()))
                myAnswerResults = JsonHelper.FromJson<List<Exam_Attend_Result_Item>>(GlobalUser.SelectExamAttendResult)
                    .Where(w => w.user_answer != null && w.user_answer.Contains("records")).ToList();

            for (int i = 0; i < myAnswerResults.Count; i++)
            {
                var userAnswer = myAnswerResults[i].user_answer;
                var userAnswerSplit = userAnswer.Split('/');
                if (userAnswerSplit.Length == 2)
                    FileNameDic.Add(userAnswerSplit[1], $"http://{userAnswer}.mp3");
            }

            SavePath = savePath;
        }

        #region << 属性 字段 >>

        /// <summary>
        /// 用户下载的通信对象
        /// </summary>
        public ExamType EType { get; set; }

        private DialogSession dialogSession;

        private Dictionary<string, string> FileNameDic;

        /// <summary>
        /// 用户下载的通信对象
        /// </summary>
        WebClientEx client;

        private int TimeOut = 6; //秒

        private string SavePath;

        /// <summary>
        /// 用户主动终止下载。
        /// </summary>
        private bool StopDownload = false;

        string d_url = ""; //下载 路径

        string s_url = ""; //保存路径

        int d_count = 0;

        /// <summary>
        /// 每次读取的字节数
        /// </summary>
        int readBytes = 4 * 1024;

        private long _downloadBytes;

        /// <summary>
        /// 已下载的字节数
        /// </summary>
        public long DownloadBytes
        {
            get { return _downloadBytes; }
            set
            {
                _downloadBytes = value;
                this.RaisePropertyChanged("DownloadBytes", "Progress");
            }
        }

        private long _totalBytes;

        /// <summary>
        /// 要下载的字节总数(文件大小)
        /// </summary>
        public long TotalBytes
        {
            get { return _totalBytes; }
            set
            {
                _totalBytes = value;
                this.RaisePropertyChanged("TotalBytes", "Progress");
            }
        }

        private bool _isDownloading;

        /// <summary>
        /// 是否正在下载
        /// </summary>
        public bool IsDownloading
        {
            get { return _isDownloading; }
            set
            {
                _isDownloading = value;
                this.RaisePropertyChanged("IsDownloading");
            }
        }

        private bool _isCompleted;

        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                _isCompleted = value;
                this.RaisePropertyChanged("IsCompleted");
            }
        }

        private Visibility _downloadingSpeedVisibility;

        /// <summary>
        /// 下载进度显示。
        /// </summary>
        public Visibility DownloadingSpeedVisibility
        {
            get => _downloadingSpeedVisibility;
            set
            {
                _downloadingSpeedVisibility = value;
                RaisePropertyChanged("DownloadingSpeedVisibility");
            }
        }

        private double _DownloadingCount;

        /// <summary>
        /// 下载进度。 文件量进度
        /// </summary>
        public double DownloadingCount
        {
            get => _DownloadingCount;
            set
            {
                _DownloadingCount = value;
                RaisePropertyChanged("DownloadingCount");
            }
        }

        private double _downloadingSpeed;

        /// <summary>
        /// 下载进度。
        /// </summary>
        public double DownloadingSpeed
        {
            get => _downloadingSpeed;
            set
            {
                _downloadingSpeed = value;
                RaisePropertyChanged("DownloadingSpeed");
            }
        }

        private string _DownloadingCountText;

        /// <summary>
        /// 下载文件量进度文本（正在下载试题，已完成 2/5。）。
        /// </summary>
        public string DownloadingCountText
        {
            get => _DownloadingCountText;
            set
            {
                _DownloadingCountText = value;
                RaisePropertyChanged("DownloadingCountText");
            }
        }

        private string _downloadingSpeedText;

        /// <summary>
        /// 下载进度文本（正在下载试题，已完成 100%。）。
        /// </summary>
        public string DownloadingSpeedText
        {
            get => _downloadingSpeedText;
            set
            {
                _downloadingSpeedText = value;
                RaisePropertyChanged("DownloadingSpeedText");
            }
        }


        private string _errMsgOut;

        public string ErrMsgOut
        {
            get
            {
                return _errMsgOut;
            }
            set
            {
                _errMsgOut = value;
                RaisePropertyChanged("ErrMsgOut");
            }
        }

        #endregion

        #region<< 自定义 >>

        public void Start()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                StartDownload(); //要异步调用的方法或异步执行的语句
            }), System.Windows.Threading.DispatcherPriority.Background);

        }

        /// <summary>
        /// 下载进度变化时触发的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            TimerReset();
            ErrMsgOut = "";

            this.TotalBytes = e.TotalBytesToReceive;
            this.DownloadBytes = e.BytesReceived;


            int downloadingSpeed = Convert.ToInt32((DownloadBytes * 100 / this.TotalBytes));
            DownloadingSpeedText = $"当前 {downloadingSpeed}%";
            DownloadingSpeed =
                downloadingSpeed == 100.00d ? 99.99d : downloadingSpeed;
            DownloadingSpeedVisibility =
                downloadingSpeed > 1.00f ? Visibility.Visible : Visibility.Hidden;
        }

        private void Client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            DownloadingSpeed = 100;
            DownloadingSpeedVisibility =
                DownloadingSpeed > 1.00f ? Visibility.Visible : Visibility.Hidden;

            this.IsDownloading = false;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    FileSecretHelper.EncryptFileBybt(new MemoryStream(e.Result), s_url);

                    if (!e.Cancelled)
                    {
                        this.IsCompleted = true;

                        client.Dispose();
                        d_count++;
                        DownloadNext(d_count);
                    }
                }
                catch (Exception request)
                {
                    Console.WriteLine(request);

                    Log4NetHelper.Error(String.Format("下载数据异常。{0}", request));

                    client?.Dispose();

                    PaperDownloadError?.Invoke(); 
                }
            }));
        }

        /// <summary>
        /// 开始下载
        /// </summary>
        public void StartDownload()
        {
            d_count = 0;

            IsDownloading = true;

            if (FileNameDic.Keys.Count > 0)
            {

                if (FileNameDic[FileNameDic.Keys.ToList()[d_count]].ToLower().Contains("http://") || FileNameDic[
                        FileNameDic.Keys.ToList()
                            [d_count]].ToLower().Contains("https://"))
                    d_url = FileNameDic[FileNameDic.Keys.ToList()[d_count]];
                else
                    d_url = $"{WebApiProxy.MEDIAURL}{FileNameDic[FileNameDic.Keys.ToList()[d_count]]}";
                s_url = Path.Combine(SavePath,
                    SecurityHelper.HmacMd5Encrypt(Path.GetFileNameWithoutExtension(d_url), GlobalUser.FILEPWD,
                            Encoding.UTF8)
                        .ToLower() + ".qf");

                DownloadingCount =
                    Convert.ToInt32((d_count + 1) * 100 / FileNameDic.Keys.Count);
                DownloadingCountText = $"总进度 {Convert.ToInt32((d_count + 1) * 100 / FileNameDic.Keys.Count)}%";

                if (File.Exists(s_url))
                {
                    d_count++;
                    DownloadNext(d_count);
                    return;
                }

                OpenDownClient();
            }
        }

        public void DownloadNext(int index)
        {
            if (FileNameDic.Keys.Count <= index)
            {
                client?.Dispose();
                Complete();
                return;
            }

            if (FileNameDic[FileNameDic.Keys.ToList()[index]].ToLower().Contains("http://") || FileNameDic[
                    FileNameDic.Keys.ToList()
                        [index]].ToLower().Contains("https://"))
                d_url = FileNameDic[FileNameDic.Keys.ToList()[index]];
            else
                d_url = $"{WebApiProxy.MEDIAURL}{FileNameDic[FileNameDic.Keys.ToList()[index]]}";
            s_url = Path.Combine(SavePath,
                SecurityHelper.HmacMd5Encrypt(Path.GetFileNameWithoutExtension(d_url), GlobalUser.FILEPWD,
                        Encoding.UTF8)
                    .ToLower() + ".qf");

            DownloadingCount =
                Convert.ToInt32((index + 1) * 100 / FileNameDic.Keys.Count);
            DownloadingCountText = $"总进度{Convert.ToInt32((index + 1) * 100 / FileNameDic.Keys.Count)}%";

            if (File.Exists(s_url))
            {
                d_count++;
                DownloadNext(d_count);
                return;
            }

            OpenDownClient();
        }

        private void OpenDownClient()
        {
            try
            {
                TimeOut = 30;

                client = new WebClientEx(TimeOut);

                //this.Timeout = Timeout;
                this.TimeOver += _timer_TimeOver;
                //client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(WebClientEx_DownloadProgressChanged);

                client.DownloadDataAsync(new Uri(d_url));
                this.TimerStart();

                //client.DownloadFileAsyncWithTimeout(new Uri(d_url), "", null);

                client.DownloadProgressChanged += Client_DownloadProgressChanged;
                client.DownloadDataCompleted += Client_DownloadDataCompleted;
            }
            catch (Exception e)
            {
                Log4NetHelper.Error(String.Format("下载数据异常。", e));
                Console.WriteLine(e);
                PaperDownloadError?.Invoke();
            }
        }

        public void Complete()
        {
            this.IsCompleted = true;
            this.IsDownloading = false;
            //this.Speed = 0;
            //client?.Dispose();
            client?.Dispose();

            if (!StopDownload)
            {
                PaperDownloadComplete?.Invoke();
                ///关闭 等待窗
                //Application.Current.Dispatcher.Invoke(new Action(() => { dialogSession?.Close(); }));
            }
        }

        public void Dispose()
        {
            TimerStop();
            client?.Dispose();
            client = null;
            PaperDownloadComplete = null;
            PaperDownloadError = null;

        }

        /// <summary>
        /// 计时器过期
        /// </summary>
        /// <param name="userdata"></param>
        void _timer_TimeOver(object userdata)
        {
            client?.CancelAsync();//取消下载
            ErrMsgOut = Properties.Settings.Default.PaperDownloadTimeOut;
        }

        #endregion

        #region << 定时 >>

        /// <summary>
        /// 时间到事件
        /// </summary>
        public event TimeoutCaller TimeOver;

        /// <summary>
        /// 开始时间
        /// </summary>
        private DateTime _startTime;
        //private TimeSpan _timeout = new TimeSpan(0, 0, 6);
        private bool _hasStarted = false;
        object _userdata;

        /// <summary>
        /// 超时退出
        /// </summary>
        /// <param name="userdata"></param>
        public virtual void OnTimeOver(object userdata)
        {
            TimerStop();
        }

        ///// <summary>
        ///// 过期时间(秒)
        ///// </summary>
        //public int Timeout
        //{
        //    get
        //    {
        //        return _timeout.Seconds;
        //    }
        //    set
        //    {
        //        if (value <= 0)
        //            return;
        //        _timeout = new TimeSpan(0, 0, value);
        //    }
        //}

        /// <summary>
        /// 是否已经开始计时
        /// </summary>
        public bool HasStarted
        {
            get
            {
                return _hasStarted;
            }
        }

        /// <summary>
        /// 开始计时
        /// </summary>
        public void TimerStart()
        {
            TimerReset();
            _hasStarted = true;
            Thread th = new Thread(TimerWaitCall);
            th.IsBackground = true;
            th.Start();
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void TimerReset()
        {
            _startTime = DateTime.Now;
        }

        /// <summary>
        /// 停止计时
        /// </summary>
        public void TimerStop()
        {
            _hasStarted = false;
        }

        /// <summary>
        /// 检查是否过期
        /// </summary>
        /// <returns></returns>
        private bool checkTimeout()
        {
            return (DateTime.Now - _startTime).Seconds >= TimeOut;
        }

        private void TimerWaitCall()
        {
            try
            {
                //循环检测是否过期
                while (_hasStarted && !checkTimeout())
                {
                    Thread.Sleep(1000);
                }
                TimeOver?.Invoke(_userdata);
            }
            catch (Exception)
            {
                TimerStop();
            }
        }

        #endregion

    }

    public class WebClientEx : WebClient
    {
        //private Calculagraph _timer;
        private int _timeOut = 6;

        /// <summary>
        /// 过期时间
        /// </summary>
        public int Timeout
        {
            get
            {
                return _timeOut;
            }
            set
            {
                if (value <= 0)
                    _timeOut = 6;
                _timeOut = value;
            }
        }

        public WebClientEx(int timeOut)
        {
            Timeout = timeOut;
        }

        /// <summary>
        /// 重写GetWebRequest,添加WebRequest对象超时时间
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = 1000 * Timeout;
            request.ReadWriteTimeout = 1000 * Timeout;
            return request;
        }

        ///// <summary>
        ///// 带过期计时的下载
        ///// </summary>
        //public void DownloadFileAsyncWithTimeout(Uri address, string fileName, object userToken)
        //{
        //    if (_timer == null)
        //    {
        //        _timer = new Calculagraph(this);
        //        _timer.Timeout = Timeout;
        //        _timer.TimeOver += new TimeoutCaller(_timer_TimeOver);
        //        this.DownloadProgressChanged += new DownloadProgressChangedEventHandler(WebClientEx_DownloadProgressChanged);
        //    }

        //    DownloadFileAsync(address, fileName, userToken);
        //    _timer.Start();
        //}

        ///// <summary>
        ///// WebClient下载过程事件，接收到数据时引发
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void WebClientEx_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        //{
        //    _timer.Reset();//重置计时器
        //}


    } 

    /// <summary>
    /// 过期时回调委托
    /// </summary>
    /// <param name="userdata"></param>
    public delegate void TimeoutCaller(object userdata);
}

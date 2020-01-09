using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using ST.Common;
using Personal_App.Common;
using Personal_App.Domain;
using MaterialDesignThemes.Wpf;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;

namespace Personal_App.ViewModel
{
    public class DownloadPaperModel : ViewModelBase
    {

        public event DownloadFinishedHandler PaperDownloadComplete;


        #region 字段
        /// <summary>
        /// 用户多线程同步的对象
        /// </summary>
        static object syncObject = new object();
        /// <summary>
        /// 用于计算速度的临时变量
        /// </summary>
        private long downloadedBytes = 0;
        /// <summary>
        /// 用户下载的通信对象
        /// </summary>
        readonly WebClient client;
        /// <summary>
        /// 每次读取的字节数
        /// </summary>
        int readBytes = 4 * 1024;

        private DialogSession dialogSession;
        /// <summary>
        /// 用户主动终止下载。
        /// </summary>
        private bool StopDownload = false;

        private Dictionary<string, string> FileNameDic;

        private string SavePath;

        #endregion

        #region 属性
        private string _downloadUrl;
        /// <summary>
        /// 下载路径
        /// </summary>
        public string DownloadUrl
        {
            get { return _downloadUrl; }
            set
            {
                _downloadUrl = value;
                this.RaisePropertyChanged("DownloadUrl");
            }
        }
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

        /// <summary>
        /// 当前进度
        /// </summary>
        public double Progress
        {
            get { return DownloadBytes * 100.0 / TotalBytes; }
        }

        private double _speed;
        /// <summary>
        /// 即时速度
        /// </summary>
        public double Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                this.RaisePropertyChanged("Speed");
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

        private string _fileName;
        /// <summary>
        /// 保存在本地的文件名称
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                this.RaisePropertyChanged("FileName");
            }
        }

        #endregion

        #region 构造器

        public DownloadPaperModel(string downloadUrl, string mainWin)
        {
            client = new WebClient();
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.DownloadFileCompleted += Client_DownloadFileCompleted;
            this.FileName = string.Format($"{Path.GetTempFileName()}");
            StartDownload(mainWin);
        }

        public DownloadPaperModel(string savePath)
        {
            FileNameDic = JsonHelper.FromJson<Dictionary<string, string>>(GlobalUser.SelectPaperInfo.paper_assets.ToString());

            SavePath = savePath;
        }

        public void Start()
        {
            StartDownload(FileNameDic, SavePath);
        }

        void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            this.IsDownloading = false;
            this.Speed = 0;
            if (!e.Cancelled)
            {
                this.IsCompleted = true;
            }
        }
        #endregion

        /// <summary>
        /// 下载进度变化时触发的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.TotalBytes = e.TotalBytesToReceive;
            this.DownloadBytes = e.BytesReceived;
        }

        /// <summary>
        /// 计算速度
        /// </summary>
        /// <param name="milliseconds"></param>
        public void InitSpeed(int milliseconds)
        {
            if (IsCompleted) return;
            if (!IsDownloading) return;
            if (milliseconds <= 0) return;
            lock (syncObject)
            {
                var haveDownloaded = this.DownloadBytes - downloadedBytes;
                this.Speed = (haveDownloaded * 1000.0) / milliseconds;
                downloadedBytes = this.DownloadBytes;
            }
        }

        /// <summary>
        /// 开始下载
        /// </summary>
        public void StartDownload(Dictionary<string, string> _aduioFileDic, string savePath,
            string mainWin = "MainDialog")
        {
            int d_count = 0;

            string d_url = ""; //下载 路径
            string s_url = ""; //保存路径

            IsDownloading = true;
            DownloadProgressDialogVM downloadProgressDialogVM = new DownloadProgressDialogVM("");
            //显示 等待窗
            var view = new DownloadProgressDialog
            {
                DataContext = downloadProgressDialogVM
            };

            //1.等待窗 (2.执行下载, 3.关闭 等待)
            var result = DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler,
                ExtendedClosingEventHandler);
            //view.DownloadingSpeedText.Text = "正在下载试题，已完成 100%";

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                for (int i = 0; i < _aduioFileDic.Keys.Count; i++)
                {

                    d_url = $"{WebApiProxy.MEDIAURL}{_aduioFileDic[_aduioFileDic.Keys.ToList()[i]]}";
                    s_url = Path.Combine(savePath,
                        SecurityHelper.HmacMd5Encrypt(Path.GetFileNameWithoutExtension(d_url), GlobalUser.FILEPWD,
                                Encoding.UTF8)
                            .ToLower() + ".qf");

                    downloadProgressDialogVM.DownloadingCount =
                        Convert.ToInt32(((i + 1) / _aduioFileDic.Keys.Count) * 100);
                    downloadProgressDialogVM.DownloadingCountText = $"总进度 {(i + 1)}/{_aduioFileDic.Keys.Count}";

                    HttpWebRequest request = (HttpWebRequest) WebRequest.Create(d_url);
                    //if (DownloadBytes > 0)
                    //{
                    //    request.AddRange(DownloadBytes);
                    //}

                    //发送请求并获取相应回应数据
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    //var response = request.EndGetResponse(ar);
                    DownloadBytes = TotalBytes = 0;
                    if (this.TotalBytes == 0) this.TotalBytes = response.ContentLength;
                    using (var writer = new MemoryStream())
                    {
                        using (var stream = response.GetResponseStream())
                        {
                            while (DownloadBytes != TotalBytes)
                            {
                                byte[] data = new byte[readBytes];
                                int readNumber = stream.Read(data, 0, data.Length);
                                if (readNumber > 0)
                                {
                                    writer.Write(data, 0, readNumber);
                                    DownloadBytes += readNumber;
                                }

                                int downloadingSpeed = Convert.ToInt32((DownloadBytes * 100 / TotalBytes));
                                downloadProgressDialogVM.DownloadingSpeedText = $"当前 {downloadingSpeed}%";
                                downloadProgressDialogVM.DownloadingSpeed =
                                    downloadingSpeed == 100.00d ? 99.99d : downloadingSpeed;
                                downloadProgressDialogVM.DownloadingSpeedVisibility =
                                    downloadingSpeed > 1.00f ? Visibility.Visible : Visibility.Hidden;

                            }
                        }

                        if (DownloadBytes == TotalBytes)
                        {
                            // Set the position to the beginning of the stream.
                            writer.Seek(0, SeekOrigin.Begin);

                            FileSecretHelper.EncryptFileBybt(writer, s_url);
                            d_count++;

                            if (d_count == _aduioFileDic.Keys.Count)
                            {
                                Complete();

                            }
                        }
                    }

                }
            }));
        }

        /// <summary>
        /// 开始下载
        /// </summary>
        public void StartDownload(string mainWin = "MainDialog")
        {
            IsDownloading = true;
            DownloadProgressDialogVM downloadProgressDialogVM = new DownloadProgressDialogVM("");
            //显示 等待窗
            var view = new DownloadProgressDialog
            {
                DataContext = downloadProgressDialogVM
            };
            //1.等待窗 (2.执行下载, 3.关闭 等待)
            var result = DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
            //view.DownloadingSpeedText.Text = "正在下载试题，已完成 100%";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.DownloadUrl);
            if (DownloadBytes > 0)
            {
                request.AddRange(DownloadBytes);
            }
            request.BeginGetResponse(ar =>
            {
                var response = request.EndGetResponse(ar);
                if (this.TotalBytes == 0) this.TotalBytes = response.ContentLength;
                using (var writer = new FileStream(this.FileName, FileMode.OpenOrCreate))
                {
                    using (var stream = response.GetResponseStream())
                    {
                        while (IsDownloading)
                        {
                            byte[] data = new byte[readBytes];
                            int readNumber = stream.Read(data, 0, data.Length);
                            if (readNumber > 0)
                            {
                                writer.Write(data, 0, readNumber);
                                DownloadBytes += readNumber;

                            }
                            int downloadingSpeed = Convert.ToInt32((DownloadBytes * 100 / TotalBytes));
                            downloadProgressDialogVM.DownloadingSpeedText = $"正在下载试题，已完成 {downloadingSpeed}%";
                            downloadProgressDialogVM.DownloadingSpeed = downloadingSpeed == 100.00d ? 99.99d : downloadingSpeed;
                            downloadProgressDialogVM.DownloadingSpeedVisibility = downloadingSpeed > 1.00f ? Visibility.Visible : Visibility.Hidden;
                            if (DownloadBytes == TotalBytes)
                            {
                                break;
                            }
                        }
                    }
                }
                Complete();
            }, null);
        }

        public void Complete()
        {
            this.IsCompleted = true;
            this.IsDownloading = false;
            this.Speed = 0;
            client?.Dispose();


            if (!StopDownload)
            {
                PaperDownloadComplete?.Invoke();
                ///关闭 等待窗
                Application.Current.Dispatcher.Invoke(new Action(() => { dialogSession?.Close(); }));
            }
        }

        public void PauseDownload()
        {
            IsDownloading = false;
            this.Speed = 0;
        }

        /// <summary>
        /// 删除下载的文件。
        /// </summary>
        public void DeleteFile()
        {
            File.Delete(FileName);
        }

        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            // 关闭对话框传值
            dialogSession = eventArgs.Session;
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter != null)
            {
                if ((bool)eventArgs.Parameter)
                {
                    //PauseDownload();
                    StopDownload = true;
                    this.IsCompleted = true;
                    this.IsDownloading = false;
                    this.Speed = 0;
                    client?.Dispose();
                    //DeleteFile();
                    //PaperDownloadComplete?.Invoke();
                    //dialogSession.
                    //// 关闭对话框
                    //Application.Current.Dispatcher.Invoke(() => { dialogSession?.Close(); });
                }
            }
        }
    }
}

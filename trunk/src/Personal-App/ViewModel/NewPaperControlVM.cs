using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Framework.Logging;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using Personal_App.Domain;
using Personal_App.Domain.Exam;
using Personal_App.ViewModel.Exam;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using ST.Models.Paper;

namespace Personal_App.ViewModel
{
    public class NewPaperControlVM : MainViewModel
    {
        public const string ViewName = "NewPaperControlVM";

        private SimulationItem paperItem;

        private SimulationPaper paperPaper;

        private string tempFile;

        private bool IsDownloaded = false; //是否 下载完成  true:已下载;false:未下载

        private bool IsErrorBeginBack = false; //续考 异常,重新模考

        #region << Show 属性 >>

        private Visibility showMyScore;

        private Visibility showExamProcess;

        private Visibility showDownloadBtn;

        private Visibility showStarScore;

        private Visibility showCompleteExamBtn;

        private Visibility showContinueExamBtn;

        private Visibility showStartExamBtn;

        private bool startExamEnable;

        private Visibility showReDoBtn;

        private Visibility showReUploadBtn;

        private bool downloadBtnEnable;

        private string downloadContent;

        private float examCompleteProgress;

        private string downloadContentColor;

        private ImageSource totalStarFiveScore;

        private string txtMyScore;
        private string txtMyScoreProcess;
        private string txtMyScoreLabel;
        private string txtTotalScore;

        private string _PaperName;

        private DialogSession dialogSession;

        private object _PaperBg;
        /// <summary>
        /// 图片 分数提示图框。
        /// </summary>
        public object PaperBg
        {
            get
            {
                return _PaperBg;
            }
            set
            {
                _PaperBg = value;
                RaisePropertyChanged("PaperBg");
            }
        }

        private Cursor downloadPaperCursor;

        /// <summary>
        /// 鼠标样式。
        /// </summary>
        public Cursor DownloadPaperCursor
        {
            get { return downloadPaperCursor; }
            set
            {
                downloadPaperCursor = value;
                RaisePropertyChanged("DownloadPaperCursor");
            }
        }

        private Visibility showNewLable;

        /// <summary>
        /// 新试卷标签。
        /// </summary>
        public Visibility ShowNewLable
        {
            get { return showNewLable; }
            set
            {
                showNewLable = value;
                RaisePropertyChanged("ShowNewLable");
            }
        }

        public string PaperName
        {
            get => _PaperName;
            set
            {
                _PaperName = value;
                RaisePropertyChanged("PaperName");
            }
        }

        /// <summary>
        /// 我的评分 显示
        /// </summary>
        public Visibility ShowMyScore
        {
            get { return showMyScore; }
            set
            {
                showMyScore = value;
                RaisePropertyChanged("ShowMyScore");
            }
        }

        /// <summary>
        /// 考试进度
        /// </summary>
        public Visibility ShowExamProcess
        {
            get { return showExamProcess; }
            set
            {
                showExamProcess = value;
                RaisePropertyChanged("ShowExamProcess");
            }
        }

        /// <summary>
        /// 试卷下载 按钮 显示
        /// </summary>
        public Visibility ShowDownloadBtn
        {
            get { return showDownloadBtn; }
            set
            {
                showDownloadBtn = value;
                RaisePropertyChanged("ShowDownloadBtn");
            }
        }

        /// <summary>
        /// 评分星星 显示
        /// </summary>
        public Visibility ShowStarScore
        {
            get { return showStarScore; }
            set
            {
                showStarScore = value;
                RaisePropertyChanged("ShowStarScore");
            }
        }

        /// <summary>
        /// 完成考试 按钮 显示
        /// </summary>
        //public Visibility ShowCompleteExamBtn
        //{
        //    get { return showCompleteExamBtn; }
        //    set
        //    {
        //        showCompleteExamBtn = value;
        //        RaisePropertyChanged("ShowCompleteExamBtn");
        //    }
        //}

        /// <summary>
        /// 继续模考 按钮 显示
        /// </summary>
        //public Visibility ShowContinueExamBtn
        //{
        //    get { return showContinueExamBtn; }
        //    set
        //    {
        //        showContinueExamBtn = value;
        //        RaisePropertyChanged("ShowContinueExamBtn");
        //    }
        //}

        /// <summary>
        /// 开始模考 按钮 显示
        /// </summary>
        public Visibility ShowStartExamBtn
        {
            get { return showStartExamBtn; }
            set
            {
                showStartExamBtn = value;
                RaisePropertyChanged("ShowStartExamBtn");
            }
        }

        /// <summary>
        /// 开始模考 按钮 Enable
        /// </summary>
        public bool StartExamEnable
        {
            get { return startExamEnable; }
            set
            {
                startExamEnable = value;
                RaisePropertyChanged("StartExamEnable");
            }
        }

        /// <summary>
        /// 完成考试 按钮 显示
        /// </summary>
        public Visibility ShowCompleteExamBtn
        {
            get { return showCompleteExamBtn; }
            set
            {
                showCompleteExamBtn = value;
                RaisePropertyChanged("ShowCompleteExamBtn");
            }
        }

        /// <summary>
        /// 继续模考 按钮 显示
        /// </summary>
        public Visibility ShowContinueExamBtn
        {
            get { return showContinueExamBtn; }
            set
            {
                showContinueExamBtn = value;
                RaisePropertyChanged("ShowContinueExamBtn");
            }
        }

        /// <summary>
        /// 重新提交答案 按钮 显示
        /// </summary>
        public Visibility ShowReUploadBtn
        {
            get { return showReUploadBtn; }
            set
            {
                showReUploadBtn = value;
                RaisePropertyChanged("ShowReUploadBtn");
            }
        }

        /// <summary>
        /// 重新做提交失败的题 按钮 显示
        /// </summary>
        public Visibility ShowReDoBtn
        {
            get { return showReDoBtn; }
            set
            {
                showReDoBtn = value;
                RaisePropertyChanged("ShowReDoBtn");
            }
        }

        /// <summary>
        /// 下载 按钮 Enable
        /// </summary>
        public bool DownloadBtnEnable
        {
            get { return downloadBtnEnable; }
            set
            {
                downloadBtnEnable = value;
                RaisePropertyChanged("DownloadBtnEnable");
            }
        }

        /// <summary>
        /// 按钮显示内容
        /// </summary>
        public string DownloadContent
        {
            get { return downloadContent; }
            set
            {
                downloadContent = value;
                RaisePropertyChanged("DownloadContent");
            }
        }

        /// <summary>
        /// 试卷完成 进度
        /// </summary>
        public float ExamCompleteProgress
        {
            get { return examCompleteProgress; }
            set
            {
                examCompleteProgress = value;
                RaisePropertyChanged("ExamCompleteProgress");
            }
        }

        /// <summary>
        /// 按钮内容颜色
        /// </summary>
        public string DownloadContentColor
        {
            get { return downloadContentColor; }
            set
            {
                downloadContentColor = value;
                RaisePropertyChanged("DownloadContentColor");
            }
        }

        /// <summary>
        /// 五星
        /// </summary>
        public ImageSource TotalStarFiveScore
        {
            get { return totalStarFiveScore; }
            set
            {
                totalStarFiveScore = value;
                RaisePropertyChanged("TotalStarFiveScore");
            }
        }

        /// <summary>
        /// 我的得分
        /// </summary>
        public string TxtMyScoreLabel
        {
            get { return txtMyScoreLabel; }
            set
            {
                txtMyScoreLabel = value;
                RaisePropertyChanged("TxtMyScoreLabel");
            }
        }

        /// <summary>
        /// 我的得分
        /// </summary>
        public string TxtMyScore
        {
            get { return txtMyScore; }
            set
            {
                txtMyScore = value;
                RaisePropertyChanged("TxtMyScore");
            }
        }

        /// <summary>
        /// 考试进度
        /// </summary>
        public string TxtMyScoreProcess
        {
            get { return txtMyScoreProcess; }
            set
            {
                txtMyScoreProcess = value;
                RaisePropertyChanged("TxtMyScoreProcess");
            }
        }

        /// <summary>
        /// 试卷总分
        /// </summary>
        public string TxtTotalScore
        {
            get { return txtTotalScore; }
            set
            {
                txtTotalScore = value;
                RaisePropertyChanged("TxtTotalScore");
            }
        }

        #endregion

        #region <<  Btn Command >>

        private RelayCommand downloadBtnCommand; //下载 试卷

        public RelayCommand DownloadBtnCommand
        {
            get
            {
                return downloadBtnCommand ?? (downloadBtnCommand = new RelayCommand(
                           (action) =>
                           {
                               //if (!IsDownloaded)
                               //StartDownloadPaper(action.ToString());
                           }));
            }
        }

        private RelayCommand startExamCommand; //开始 模考

        public RelayCommand StartExamCommand
        {
            get
            {
                return startExamCommand ?? (startExamCommand = new RelayCommand(
                           (action) =>
                           {
                               //GlobalUser.SelectPaperJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.GetUserDataFolder(), paperItem.ZipFileName,
                               //    $"{paperItem.ZipFileName}.json");
                               //GlobalUser.SelectPaperName = paperItem.ZipFileName;

                               GlobalUser.SelectPaperName =
                                   GlobalUser.SelectPaperViewName = paperItem.paper_title;
                               GlobalUser.SelectPaperNumber = paperItem.exam_id;
                               GlobalUser.AttendPaperItemId = paperItem.exam_attend_id;
                               GlobalUser.SelectPaperTotalScore = paperItem.paper_score;

                               StartDownloadPaper(ExamType.First, action.ToString());
                           }));
            }
        }

        private RelayCommand continueExamCommand; //继续 模考

        public RelayCommand ContinueExamCommand
        {
            get
            {
                return continueExamCommand ?? (continueExamCommand = new RelayCommand(
                           (action) =>
                           {
                               //GlobalUser.SelectPaperJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.GetUserDataFolder(), paperItem.ZipFileName, $"{paperItem.ZipFileName}.json");
                               //GlobalUser.SelectPaperName = paperItem.ZipFileName;
                               GlobalUser.SelectPaperName = GlobalUser.SelectPaperViewName = paperItem.Name;
                               GlobalUser.SelectPaperNumber = paperItem.exam_id;
                               GlobalUser.AttendPaperItemId = paperItem.exam_attend_id;
                               GlobalUser.SelectExamAttendResult = "";
                               GlobalUser.SelectPaperTotalScore = paperItem.paper_score;

                               StartDownloadPaper(ExamType.Continue, action.ToString());
                           }));
            }
        }

        private void CheckAudioOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            //lets run a fake operation for 5 seconds then close this baby.
            //Task.Delay(TimeSpan.FromSeconds(3))
            //    .ContinueWith(
            //        (t, _) =>
            //        {
            //            eventargs.Session.Close();
            //            //跳转到 列表
            //            Messenger.Default.Send(new NavigateMessage(), "ShowUserpapers");
            //        }, null,
            //        TaskScheduler.FromCurrentSynchronizationContext());//关闭程序

            Task.Factory.StartNew(() => Thread.Sleep(3000))
                .ContinueWith(t =>
                {
                    eventargs.Session.Close();
                    //跳转到 列表
                    Messenger.Default.Send(new NavigateMessage(), "ShowUserpapers");
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }


        /// <summary>
        /// 重新模考对话框关闭事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void RedoExamDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {

            if (eventArgs.Parameter != null)
            {
                GlobalUser.CleanUp(); // 退出 录播设备

                if ((bool)eventArgs.Parameter)
                {
                    if (IsErrorBeginBack)
                    {
                        //true: 题目操作阶段
                        if (GlobalUser.DoneItemExam == false)
                        {
                            return;
                        }

                        //更新 试卷信息控件
                        UploadPaperUC();

                        return;
                    }

                    IsErrorBeginBack = true;
                    //new 显示
                    var view = new ExamMainWin(null, ExamType.AgainTrain);
                    //打开 对话框
                    eventArgs.Session.UpdateContent(view);
                    //eventArgs.Session.Close();
                    //var vv = DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler,
                    //    ExtendedClosingEventHandler);
                    eventArgs.Cancel();
                    //更新 试卷信息控件
                    UploadPaperUC();

                }
                //else
                //{

                //}
            }

        }


        private RelayCommand viewReportCommand; //查看 报告

        public RelayCommand ViewReportCommand
        {
            get
            {
                return viewReportCommand ?? (viewReportCommand = new RelayCommand(
                           (action) =>
                           {
                               //GlobalUser.SelectPaperJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.GetUserDataFolder(), paperItem.ZipFileName,
                               //    $"{paperItem.ZipFileName}.json");
                               //GlobalUser.SelectPaperName = paperItem.ZipFileName;

                               GlobalUser.SelectPaperName = GlobalUser.SelectPaperViewName = paperItem.Name;
                               GlobalUser.SelectPaperNumber = paperItem.exam_id;
                               GlobalUser.AttendPaperItemId = paperItem.exam_attend_id;
                               GlobalUser.SelectExamAttendResult = "";
                               GlobalUser.SelectPaperAvgScore5Points = paperItem.AvgScore5Points;
                               GlobalUser.SelectPaperTotalScore = paperItem.paper_score;

                               StartDownloadPaper(ExamType.Result, action.ToString());
                               ////new 显示
                               //var view = new ExamMainWin(null, ExamType.Result);
                               ////打开 对话框
                               //DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler,
                               //    ExtendedClosingEventHandler);
                           }));
            }
        }

        private RelayCommand againExamCommand; //再做一遍

        public RelayCommand AgainExamCommand
        {
            get
            {
                return againExamCommand ?? (againExamCommand = new RelayCommand(
                           (action) =>
                           {
                               //GlobalUser.SelectPaperJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.GetUserDataFolder(), paperItem.ZipFileName,
                               //    $"{paperItem.ZipFileName}.json");
                               //GlobalUser.SelectPaperName = paperItem.ZipFileName;

                               GlobalUser.SelectPaperName = GlobalUser.SelectPaperViewName = paperItem.Name;
                               GlobalUser.SelectPaperNumber = paperItem.exam_id;
                               GlobalUser.AttendPaperItemId = paperItem.exam_attend_id;
                               GlobalUser.SelectExamAttendResult = "";
                               GlobalUser.SelectPaperTotalScore = paperItem.paper_score;

                               StartDownloadPaper(ExamType.Again, action.ToString());
                           }));
            }
        }

        private RelayCommand reDoExamCommand; //再做一遍 失败题目

        public RelayCommand ReDoExamCommand
        {
            get
            {
                return reDoExamCommand ?? (reDoExamCommand = new RelayCommand(
                           (action) =>
                           {
                               //GlobalUser.SelectPaperJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.GetUserDataFolder(), paperItem.ZipFileName,
                               //    $"{paperItem.ZipFileName}.json");
                               //GlobalUser.SelectPaperName = paperItem.ZipFileName;

                               GlobalUser.SelectPaperName = GlobalUser.SelectPaperViewName = paperItem.Name;
                               GlobalUser.SelectPaperNumber = paperItem.exam_id;
                               GlobalUser.AttendPaperItemId = paperItem.exam_attend_id;
                               GlobalUser.SelectExamAttendResult = "";
                               GlobalUser.SelectPaperTotalScore = paperItem.paper_score;

                               StartDownloadPaper(ExamType.ReDo, action.ToString());
                           }));
            }
        }

        private RelayCommand _ReUploadCommand; //重新上传答案

        public RelayCommand ReUploadCommand
        {
            get
            {
                return _ReUploadCommand ?? (_ReUploadCommand = new RelayCommand(
                           (action) =>
                           {

                               GlobalUser.SelectPaperName = GlobalUser.SelectPaperViewName = paperItem.Name;
                               GlobalUser.SelectPaperNumber = paperItem.exam_id;
                               GlobalUser.AttendPaperItemId = paperItem.exam_attend_id;
                               GlobalUser.SelectExamAttendResult = "";
                               GlobalUser.SelectPaperTotalScore = paperItem.paper_score;

                               StartDownloadPaper(ExamType.ReUpload, action.ToString());
                           }));
            }
        }


        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            dialogSession = eventArgs.Session;
        }

        private void ExtendedOpenedDPEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            dialogSession = eventArgs.Session;

            var view = dialogSession.Content as DownloadProgressDialog;
            var vm = view.DataContext as DownloadProgressDialogVM;
            vm.Start();
        }

        /// <summary>
        /// 作业关闭事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ExtendedClosingDpEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter != null)
            {
                GlobalUser.CleanUp(); // 退出 录播设备

                if ((bool)eventArgs.Parameter)
                {
                    var view = dialogSession.Content as DownloadProgressDialog;
                    var vm = view.DataContext as DownloadProgressDialogVM;
                    vm.Dispose();
                }
            }
        }

        /// <summary>
        /// 欢迎页、模考页，关闭按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!string.IsNullOrEmpty(GlobalUser.AttendPaperItemId))
            {
                paperItem.exam_attend_id = GlobalUser.AttendPaperItemId;
            }


            if (eventArgs.Parameter != null)
            {
                GlobalUser.DoAnswer = false;

                GlobalUser.CleanUp(); // 退出 录播设备

                if ((bool)eventArgs.Parameter)
                {
                    //true: 题目操作阶段
                    if (GlobalUser.DoneItemExam == false)
                    {
                        DialogHost.CloseAllShow();
                        //return;
                    }

                    //更新 试卷信息控件
                    UploadPaperUC();
                }
                else
                {
                    eventArgs.Cancel();
                    //new 显示
                    var view = new ExamClosingDialog();
                    //打开 对话框
                    DialogHost.Show(view, "ExamMainDialog", ExamClosingEventHandler);
                }
            }

            //var result = DialogHostEx.ShowDialog(new CloseExamDialog(), "CloseExamReadDialog", ExtendedCloseExamOpenedEventHandler,
            //    ExtendedCloseExamClosingEventHandler);
        }

        /// <summary>
        /// 模考关闭事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventargs"></param>
        private void ExamClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter != null)
            {
                if ((bool)eventArgs.Parameter)
                {
                    GlobalUser.DoAnswer = false;

                    GlobalUser.CleanUp(); //退出 录播设备

                    //关闭 等待窗
                    Application.Current.Dispatcher.Invoke(new Action(() => { dialogSession?.Close(); }));
                    //true: 题目操作阶段
                    if (GlobalUser.DoneItemExam == false)
                    {
                        return;
                    }

                    //更新 试卷信息控件
                    UploadPaperUC();
                }
            }


        }

        private void CleanUp()
        {
            if (GlobalUser.AudioFileReader != null)
            {
                GlobalUser.AudioFileReader?.Dispose();
                GlobalUser.AudioFileReader = null;
            }

            if (GlobalUser.WavePlayer != null)
            {
                GlobalUser.WavePlayer?.Stop();
                GlobalUser.WavePlayer?.Dispose();
                GlobalUser.WavePlayer = null;
            }

            if (GlobalUser.Recorder != null)
            {
                GlobalUser.Recorder?.Stop();
                GlobalUser.Recorder?.Dispose();
                GlobalUser.Recorder = null;
            }

            //刷新 次控件
            //todo
        }

        #endregion

        public NewPaperControlVM()
        {
            //1.分数 显示
            ShowMyScore = Visibility.Collapsed;

            //2.中间 进度内容 显示
            ShowDownloadBtn = Visibility.Visible;
            ShowStarScore = Visibility.Collapsed;

            //3.底部按钮 显示
            //ShowCompleteExamBtn = Visibility.Collapsed; //再做一遍
            //ShowContinueExamBtn = Visibility.Collapsed; //继续模考
            ShowStartExamBtn = Visibility.Visible; //开始模考
            ShowReUploadBtn = Visibility.Collapsed;//上传答案 
            ShowReDoBtn = Visibility.Collapsed;//继续作答
            StartExamEnable = false;

            IsDownloaded = false;
            //DownloadBtnEnable = true; //下载按钮 enable
            //ExamCompleteProgress = 0;//考试完成进度
            TxtMyScore = "0%";
            TxtMyScoreProcess = "0.1";
            TxtMyScoreLabel = "完成进度";
            //DownloadContentColor = "#64e9ff";//按钮 内容颜色
            //DownloadContent = "下载";//按钮 内容
            // 新考题标签
            ShowNewLable = Visibility.Collapsed;
        }

        public NewPaperControlVM(SimulationItem _paperItem, int index) : this()
        {
            this.paperItem = _paperItem;

            this.PaperBg = $"/Resources/liebiao{index + 1}.png";

            PaperName = paperItem.paper_title;

            //MatchControlShow();


            ShowPaperUcInfo();
        }

        #region << 自定义 >>

        /// <summary>
        /// 匹配 显示的控件效果
        /// </summary>
        private void MatchControlShow()
        {

            // 1、验证是否已下载
            string paperJsonFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.GetUserDataFolder(),
                paperItem.ZipFileName,
                $"{paperItem.ZipFileName}.json");
            if (File.Exists(paperJsonFile))
            {
                IsDownloaded = true;

                CheckFile(paperJsonFile);

                DownloadPaperCursor = Cursors.Arrow;


                // 2、未下载、未考试
                ShowPaperUcInfo();
            }
            else
            {
                DownloadPaperCursor = Cursors.Hand;
                // 试卷美容不完成，重新打开下载按钮
                IsDownloaded = false;

                if (paperItem.exam_process == null || paperItem.exam_process == 0.00f)
                {
                    ShowNewLable = Visibility.Visible;
                }
                else
                {
                    //CheckNewSimulation();
                    var paperFileFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                        GlobalUser.GetUserDataFolder());
                    try
                    {
                        if (Directory.Exists(paperFileFolder))
                        {
                            var paperDirectoryName = String.Format("{0}_{1}", paperItem.ZipFileName.Split('_')[0],
                                paperItem.ZipFileName.Split('_')[1]);
                            //if (Directory.Exists(paperDirectoryName))
                            //{
                            DirectoryInfo directory = new DirectoryInfo(paperFileFolder);
                            var directories = directory.GetDirectories();
                            var oldPaperFileDirectories =
                                directories.Where(d => d.Name.Contains(paperDirectoryName)).ToList();
                            foreach (var item in oldPaperFileDirectories)
                            {
                                if (item.Name != paperItem.ZipFileName)
                                {
                                    ShowNewLable = Visibility.Visible;
                                }
                            }

                            //}
                        }
                    }
                    catch (Exception e)
                    {
                        Log4NetHelper.Error(String.Format("获取用户目录失败，失败原因：{0}", e));
                    }
                }
            }
        }

        /// <summary>
        /// 检查新试卷。
        /// </summary>
        private void CheckNewSimulation()
        {
            var paperFileFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.GetUserDataFolder());
            try
            {
                if (Directory.Exists(paperFileFolder))
                {
                    var paperDirectoryName = String.Format("{0}_{1}", paperItem.ZipFileName.Split('_')[0],
                        paperItem.ZipFileName.Split('_')[1]);
                    //if (Directory.Exists(paperDirectoryName))
                    //{
                    DirectoryInfo directory = new DirectoryInfo(paperFileFolder);
                    var directories = directory.GetDirectories();
                    var oldPaperFileDirectories = directories.Where(d => d.Name.Contains(paperDirectoryName)).ToList();
                    foreach (var item in oldPaperFileDirectories)
                    {
                        if (item.Name != paperItem.ZipFileName)
                        {
                            ShowNewLable = Visibility.Visible;
                        }
                    }

                    //}
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error(String.Format("获取用户目录失败，失败原因：{0}", e));
            }
        }

        private void ShowPaperUcInfo()
        {
            ShowCompleteExamBtn = Visibility.Collapsed; //再做一遍/查看报告
            ShowContinueExamBtn = Visibility.Collapsed; //继续模考
            ShowReUploadBtn = Visibility.Collapsed;
            ShowStartExamBtn = Visibility.Visible; //开始模考
            ShowReDoBtn = Visibility.Collapsed;

            TxtTotalScore = $"{paperItem.paper_score}";
            if (paperItem.exam_process < 1 || paperItem.exam_process == null)
            {
                IsDownloaded = true;
                ExamCompleteProgress = Convert.ToInt32(paperItem.exam_process * 100); //考试完成进度
                //DownloadContentColor = "#999999"; //按钮 内容颜色
                TxtMyScoreLabel = "完成进度";
                TxtMyScore = $"{ExamCompleteProgress}%"; //按钮 内容
                TxtMyScoreProcess = $"{ExamCompleteProgress}";

                //3.下载完成 未完成全部答题
                ShowCompleteExamBtn = Visibility.Collapsed; //再做一遍

                if (paperItem.exam_process == null || paperItem.exam_process == 0)
                {
                    //ShowContinueExamBtn = Visibility.Collapsed; //继续模考
                    //ShowStartExamBtn = Visibility.Visible; //开始模考
                    StartExamEnable = true;
                    ShowMyScore = Visibility.Collapsed;
                    ShowStarScore = Visibility.Collapsed;
                    ShowDownloadBtn = Visibility.Visible;
                    ShowExamProcess = Visibility.Visible;
                    TxtMyScoreProcess = "0.1";
                }
                else
                {
                    //ShowContinueExamBtn = Visibility.Visible; //继续模考
                    //ShowStartExamBtn = Visibility.Collapsed; //开始模考
                    ShowDownloadBtn = Visibility.Visible;
                    ShowStarScore = Visibility.Collapsed;
                    ShowMyScore = Visibility.Collapsed;
                    ShowNewLable = Visibility.Collapsed;
                    ShowExamProcess = Visibility.Visible;
                    StartExamEnable = true;
                }
            }
            else
            {
                //4.完成全部答题
                ShowCompleteExamBtn = Visibility.Visible; //再做一遍
                ShowContinueExamBtn = Visibility.Collapsed; //继续模考
                ShowStartExamBtn = Visibility.Collapsed; //开始模考
                ShowDownloadBtn = Visibility.Collapsed; //隐藏 下载 模考进度
                ShowStarScore = Visibility.Visible; //显示 星星
                ShowExamProcess = Visibility.Collapsed;
                StartExamEnable = true;
                //5. 星星处理

                Application.Current.Dispatcher.Invoke(new Action(() => { TotalStarFiveScore = SetStarScore(); }));

                //其他 分数 显示
                ShowMyScore = Visibility.Visible;
                TxtMyScoreLabel = "得分";
                TxtMyScore = paperItem.score.ToString();
            }

            GlobalUser.GetErrorScoreSource(User.Mobile, paperItem.exam_id, paperItem.exam_attend_id);

            if (GlobalUser.ErrScoreInfo?.Count > 0 &&
                GlobalUser.ErrScoreInfo.Values.FirstOrDefault()?.AnswerModel.exam_attend_id ==
                paperItem.exam_attend_id)
            {
                ShowStartExamBtn = Visibility.Collapsed; //开始模考
                ShowCompleteExamBtn = Visibility.Collapsed; //再做一遍/查看报告
                ShowContinueExamBtn = Visibility.Collapsed; //继续模考
                ShowReUploadBtn = Visibility.Visible;

                if ((GlobalUser.ErrScoreInfo?.Values.ToList().Where(w => w.SubmitNum == 999)).Any())
                {
                    ShowStartExamBtn = Visibility.Collapsed; //开始模考
                    ShowCompleteExamBtn = Visibility.Collapsed; //再做一遍/查看报告
                    ShowContinueExamBtn = Visibility.Collapsed; //继续模考
                    ShowReUploadBtn = Visibility.Collapsed;
                    ShowReDoBtn = Visibility.Visible;
                }
            }
        }

        private ImageSource SetStarScore()
            {
                paperItem.score = paperItem.score == null ? 0 : paperItem.score;

                float? AvgScore5Points = 5 * (paperItem.avg_score / 100);

                string strScore = Convert.ToDouble(AvgScore5Points).ToString("0.00");

                int leftPoint = int.Parse(strScore.Substring(0, 1));
                int rightPoint = int.Parse(strScore.Substring(2, 2));


                if (AvgScore5Points > 0)
                {

                    if (rightPoint > 0 && rightPoint <= 30)
                    {
                        rightPoint = 25;
                    }
                    else if (rightPoint > 30 && rightPoint <= 60)
                    {
                        rightPoint = 50;
                    }
                    else if (rightPoint > 60 && rightPoint <= 99)
                    {
                        rightPoint = 75;
                    }
                    else if (rightPoint > 99)
                    {
                        if (leftPoint < 5)
                            leftPoint = leftPoint + 1;

                        rightPoint = 00;
                    }

                    AvgScore5Points = float.Parse($"{leftPoint}.{rightPoint}");
                }

                paperItem.AvgScore5Points = AvgScore5Points;

                switch (AvgScore5Points)
                {
                    case 0.0f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_0_0);
                    case 0.25f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_0_1);
                    case 0.5f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_0_2);
                    case 0.75f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_0_3);
                    case 1.0f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_1);
                    case 1.25f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_1_1);
                    case 1.5f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_1_2);
                    case 1.75f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_1_3);
                    case 2.0f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_2);
                    case 2.25f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_2_1);
                    case 2.5f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_2_2);
                    case 2.75f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_2_3);
                    case 3.0f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_3);
                    case 3.25f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_3_1);
                    case 3.5f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_3_2);
                    case 3.75f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_3_3);
                    case 4.0f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_4);
                    case 4.25f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_4_1);
                    case 4.5f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_4_2);
                    case 4.75f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_4_3);
                    case 5.0f:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_5);
                    default:
                        return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_1_0);
                }
            }


            private void StartDownloadPaper(ExamType eType, string mainWin)
            {
                GlobalUser.MenuType = MenuType.PaperSync;

                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    if (!BindPaperInfo()) return;

                    string pfile = SecurityHelper.HmacMd5Encrypt(GlobalUser.SelectPaperName + GlobalUser.SelectPaperNumber,
                        GlobalUser.FILEPWD, Encoding.UTF8).ToUpper();

                    string pdir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER, pfile);

                    if (!Directory.Exists(pdir))
                    {
                        Directory.CreateDirectory(pdir);
                    }

                //var paperFile = new DownloadPaperModel(pdir);
                //paperFile.PaperDownloadComplete += PaperFileOnPaperDownloadComplete;
                //paperFile.Start();

                if (CheckFile(pdir))
                    {
                    //todo
                    //打开考试窗口

                    //new 显示
                    var view0 = new ExamMainWin(null, eType);
                    //打开 对话框
                    DialogHostEx.ShowDialog(GlobalUser.MainWin, view0, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);

                        return;
                    }

                //new 显示 打开下载
                var view = new DownloadProgressDialog();
                    var vm = new DownloadProgressDialogVM(pdir);
                    vm.EType = eType;
                    vm.PaperDownloadComplete += PaperFileOnPaperDownloadComplete;
                    vm.PaperDownloadError += PaperFileOnPaperDownloadError;
                    view.DataContext = vm;
                //打开 对话框
                DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedDPEventHandler,
                        ExtendedClosingDpEventHandler);

                }));
            }

            private void OfflineTipsOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
            {
                //lets run a fake operation for 5 seconds then close this baby.
                //Task.Delay(TimeSpan.FromSeconds(5))
                //    .ContinueWith(
                //        (t, _) =>
                //        {
                //            eventargs.Session.Close();
                //            //跳转到 列表
                //            Messenger.Default.Send(new NavigateMessage(), "ShowUserpapers");
                //        }, null,
                //        TaskScheduler.FromCurrentSynchronizationContext());//关闭程序

                Task.Factory.StartNew(() => Thread.Sleep(1000))
                    .ContinueWith(t =>
                    {
                        eventargs.Session.Close();
                    //跳转到 列表
                    Messenger.Default.Send(new NavigateMessage(), "ShowUserpapers");
                    }, TaskScheduler.FromCurrentSynchronizationContext());
            }


            private void PaperFileOnPaperDownloadComplete()
            {
                var view = dialogSession.Content as DownloadProgressDialog;
                var vm = view.DataContext as DownloadProgressDialogVM;
                var et = vm.EType;
                dialogSession?.Close();

                if (et != null)
                {

                    //new 显示
                    var view0 = new ExamMainWin(null, et);
                    //打开 对话框
                    DialogHostEx.ShowDialog(GlobalUser.MainWin, view0, ExtendedOpenedEventHandler,
                        ExtendedClosingEventHandler);
                }
            }

            private void PaperFileOnPaperDownloadError()
            {
                dialogSession?.Close();
                //Messenger.Default.Send(new MainDialogMessage("你的网络开小差了，请检查网络设置后重新下载！"), "MainMessageDialog");

                var view = new MessageDialog();
                view.DataContext = new MessageDialogVM()
                {
                    MsgTitle = "提示消息",
                    MsgContent = Properties.Settings.Default.PaperDownloadTimeOut1,
                };
                DialogHostEx.ShowDialog(GlobalUser.MainWin, view, MessageDialogClose);
            }

            private void UploadPaperUC()
            {
                // 异步请求，防止界面假死
                //Task.Run(() =>
                //{
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {

                    var ml = new GetPaperInfoDetail()
                    {
                        exam_id = GlobalUser.SelectPaperNumber.Split('#')[0],
                        token = GlobalUser.USER.Token
                    };

                    var result1 = WebProxy(ml, ApiType.GetPaperInfoDetail, null, "get");

                    if (result1?.retCode == 0)
                    {
                    //发送 
                    var exam_attend =
                            JsonHelper.FromJson<Exam_Attend>(result1.retData.exam_attend.ToString());

                        paperItem.exam_process = exam_attend.exam_process;
                        paperItem.score = exam_attend.score;

                        ShowPaperUcInfo();
                    }
                }));
            }

            #endregion
        }
    }
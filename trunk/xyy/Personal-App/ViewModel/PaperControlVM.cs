using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Framework.Logging;
using GalaSoft.MvvmLight.Messaging;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using ST.Models.Paper;
using Personal_App.Common;
using Personal_App.Domain;
using Personal_App.Domain.Exam;
using Personal_App.ViewModel.Exam;
using MaterialDesignThemes.Wpf;
using NAudio.CoreAudioApi;
using ST.Common;

namespace Personal_App.ViewModel
{
    public class PaperControlVM : MainViewModel
    {
        public const string ViewName = "PaperControlView";

        private SimulationItem paperItem;

        private SimulationPaper paperPaper;

        private string tempFile;

        private bool IsDownloaded = false;//是否 下载完成  true:已下载;false:未下载

        private bool IsErrorBeginBack = false;//续考 异常,重新模考

        #region << Show 属性 >>

        private Visibility showMyScore;

        private Visibility showDownloadBtn;

        private Visibility showStarScore;

        private Visibility showCompleteExamBtn;

        private Visibility showContinueExamBtn;

        private Visibility showStartExamBtn;

        private bool startExamEnable;

        private bool downloadBtnEnable;

        private string downloadContent;

        private float examCompleteProgress;

        private string downloadContentColor;

        private ImageSource totalStarFiveScore;

        private string txtMyScore;
        private string txtTotalScore;

        private string _PaperName;

        private DialogSession dialogSession;


        private Cursor downloadPaperCursor;

        /// <summary>
        /// 鼠标样式。
        /// </summary>
        public Cursor DownloadPaperCursor
        {
            get
            {
                return downloadPaperCursor;
            }
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
            get
            {
                return showNewLable;
            }
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
            get
            {
                return showMyScore;
            }
            set
            {
                showMyScore = value;
                RaisePropertyChanged("ShowMyScore");
            }
        }

        /// <summary>
        /// 试卷下载 按钮 显示
        /// </summary>
        public Visibility ShowDownloadBtn
        {
            get
            {
                return showDownloadBtn;
            }
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
            get
            {
                return showStarScore;
            }
            set
            {
                showStarScore = value;
                RaisePropertyChanged("ShowStarScore");
            }
        }

        /// <summary>
        /// 完成考试 按钮 显示
        /// </summary>
        public Visibility ShowCompleteExamBtn
        {
            get
            {
                return showCompleteExamBtn;
            }
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
            get
            {
                return showContinueExamBtn;
            }
            set
            {
                showContinueExamBtn = value;
                RaisePropertyChanged("ShowContinueExamBtn");
            }
        }

        /// <summary>
        /// 开始模考 按钮 显示
        /// </summary>
        public Visibility ShowStartExamBtn
        {
            get
            {
                return showStartExamBtn;
            }
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
            get
            {
                return startExamEnable;
            }
            set
            {
                startExamEnable = value;
                RaisePropertyChanged("StartExamEnable");
            }
        }

        /// <summary>
        /// 下载 按钮 Enable
        /// </summary>
        public bool DownloadBtnEnable
        {
            get
            {
                return downloadBtnEnable;
            }
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
            get
            {
                return downloadContent;
            }
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
            get
            {
                return examCompleteProgress;
            }
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
            get
            {
                return downloadContentColor;
            }
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
            get
            {
                return totalStarFiveScore;
            }
            set
            {
                totalStarFiveScore = value;
                RaisePropertyChanged("TotalStarFiveScore");
            }
        }

        /// <summary>
        /// 我的得分
        /// </summary>
        public string TxtMyScore
        {
            get
            {
                return txtMyScore;
            }
            set
            {
                txtMyScore = value;
                RaisePropertyChanged("TxtMyScore");
            }
        }

        /// <summary>
        /// 试卷总分
        /// </summary>
        public string TxtTotalScore
        {
            get
            {
                return txtTotalScore;
            }
            set
            {
                txtTotalScore = value;
                RaisePropertyChanged("TxtTotalScore");
            }
        }

        #endregion

        #region <<  Btn Command>>

        private RelayCommand downloadBtnCommand;//下载 试卷

        public RelayCommand DownloadBtnCommand
        {
            get
            {
                return downloadBtnCommand ?? (downloadBtnCommand = new RelayCommand(
                           (action) =>
                           {
                               //if (!IsDownloaded)
                                   StartDownloadPaper(action.ToString());
                           }));
            }
        }

        private RelayCommand startExamCommand;//开始 模考

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

                               GlobalUser.SelectPaperName = GlobalUser.SelectPaperViewName = paperItem.paper_title;
                               GlobalUser.SelectPaperNumber = paperItem.exam_id;
                               GlobalUser.AttendPaperItemId = paperItem.exam_attend_id;
                               GlobalUser.SelectPaperTotalScore = paperItem.paper_score;

                               //new 显示
                               var view = new ExamMainWin(null, ExamType.First);
                               //打开 对话框
                               DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
                           }));
            }
        }

        private RelayCommand continueExamCommand;//继续 模考

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

                               // 显示视图
                               var view = new ExamMainWin(null, ExamType.Continue);
                               // 打开 对话框
                               DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler,
                                   ExtendedClosingEventHandler);

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
                .ContinueWith(t => {
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
                    var view = new ExamMainWin(null, ExamType.Again);
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


        private RelayCommand viewReportCommand;//查看 报告

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

                               //new 显示
                               var view = new ExamMainWin(null, ExamType.Result);
                               //打开 对话框
                               DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
                           }));
            }
        }

        private RelayCommand againExamCommand;//再做一遍

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

                               //new 显示
                               var view = new ExamMainWin(null, ExamType.Again);
                               //打开 对话框
                               DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
                               //更新 试卷信息控件
                               UploadPaperUC();
                           }));
            }
        }


        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            dialogSession = eventArgs.Session;
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

                GlobalUser.CleanUp(); // 退出 录播设备

                if ((bool)eventArgs.Parameter)
                {
                    //true: 题目操作阶段
                    if (GlobalUser.DoneItemExam == false)
                    {
                        return;
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
                    GlobalUser.CleanUp();//退出 录播设备

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

        public PaperControlVM()
        {
            //1.分数 显示
            ShowMyScore = Visibility.Collapsed;

            //2.中间 进度内容 显示
            ShowDownloadBtn = Visibility.Visible;
            ShowStarScore = Visibility.Collapsed;

            //3.底部按钮 显示
            ShowCompleteExamBtn = Visibility.Collapsed;//再做一遍
            ShowContinueExamBtn = Visibility.Collapsed;//继续模考
            ShowStartExamBtn = Visibility.Visible;//开始模考
            StartExamEnable = false;

            IsDownloaded = false;
            //DownloadBtnEnable = true; //下载按钮 enable
            ExamCompleteProgress = 0;//考试完成进度
            DownloadContentColor = "#64e9ff";//按钮 内容颜色
            DownloadContent = "下载";//按钮 内容
            // 新考题标签
            ShowNewLable = Visibility.Collapsed;
        }

        public PaperControlVM(SimulationItem _paperItem) : this()
        {
            this.paperItem = _paperItem;

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
                    var paperDirectoryName = String.Format("{0}_{1}", paperItem.ZipFileName.Split('_')[0], paperItem.ZipFileName.Split('_')[1]);
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
            TxtTotalScore = $"总分 {paperItem.paper_score}";
            if (paperItem.exam_process < 1|| paperItem.exam_process == null)
            {

                IsDownloaded = true;
                ExamCompleteProgress = Convert.ToInt32(paperItem.exam_process * 100); //考试完成进度
                DownloadContentColor = "#999999"; //按钮 内容颜色
                DownloadContent = $"完成 {ExamCompleteProgress}%"; //按钮 内容

                //3.下载完成 未完成全部答题
                ShowCompleteExamBtn = Visibility.Collapsed; //再做一遍

                if (paperItem.exam_process == null || paperItem.exam_process == 0)
                {
                    ShowContinueExamBtn = Visibility.Collapsed; //继续模考
                    ShowStartExamBtn = Visibility.Visible; //开始模考
                    StartExamEnable = true;
                    ShowMyScore = Visibility.Collapsed;
                    ShowStarScore = Visibility.Collapsed;
                    ShowDownloadBtn = Visibility.Visible;
                }
                else
                {
                    ShowContinueExamBtn = Visibility.Visible; //继续模考
                    ShowStartExamBtn = Visibility.Collapsed; //开始模考
                    ShowDownloadBtn = Visibility.Visible;
                    ShowStarScore = Visibility.Collapsed;
                    ShowMyScore = Visibility.Collapsed;
                    ShowNewLable = Visibility.Collapsed;
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
                StartExamEnable = true;
                //5. 星星处理

                Application.Current.Dispatcher.Invoke(new Action(() => { TotalStarFiveScore = SetStarScore(); }));

                //其他 分数 显示
                ShowMyScore = Visibility.Visible;
                TxtMyScore = paperItem.score.ToString();
            }
        }

        private ImageSource SetStarScore()
        {
            paperItem.score = paperItem.score == null ? 0 : paperItem.score;

            float? AvgScore5Points = 5 * (paperItem.score / paperItem.paper_score);

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
                else if(rightPoint > 99)
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

        /// <summary>
        /// 检查 音频 图片 资料文件
        /// </summary>
        /// <param name="paperJsonFile"></param>
        private void CheckFile(string paperJsonFile)
        {
            string paperJson;
            using (var jsonContent = new FileStream(paperJsonFile, FileMode.Open))
            {
                int fsLen = (int)jsonContent.Length;
                byte[] heByte = new byte[fsLen];
                int r = jsonContent.Read(heByte, 0, heByte.Length);
                //string myStr = Encoding.UTF8.GetString(heByte);

                paperJson = Base64Provider.Decrypt(Encoding.UTF8.GetString(heByte), Base64Provider.KEY, Base64Provider.IV);
            }

            paperPaper = JsonHelper.FromJson<SimulationPaper>(paperJson);

            if (paperPaper != null)
            {
                foreach (var qs in paperPaper.examList)
                {
                    foreach (var qsItem in qs.examDetailList)
                    {
                        if (!string.IsNullOrEmpty(qsItem.audio))
                        {
                            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                GlobalUser.GetUserDataFolder(),
                                paperItem.ZipFileName, qsItem.audio)))
                            {
                                //文件缺失,重新下载 打开"下载"按钮
                                IsDownloaded = false;
                                break;
                            }
                        }

                        if (!string.IsNullOrEmpty(qsItem.pic))
                        {
                            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                GlobalUser.GetUserDataFolder(),
                                paperItem.ZipFileName, qsItem.pic)))
                            {
                                //文件缺失,重新下载 打开"下载"按钮
                                IsDownloaded = false;
                                break;
                            }
                        }
                    }
                }
            }

        }

        private void StartDownloadPaper(string mainWin)
        {

            if (!string.IsNullOrEmpty(paperItem.ZipFileUrl))
            {
                #region 验证模拟题状态...
                // 异步请求，防止界面假死
                //Task.Run(() =>
                //{
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    VerifySimulationModel verifySimulationModel = new VerifySimulationModel
                    {
                        SimulationId = paperItem.SimulationId.ToString()
                    };

                    var result = WebProxy(verifySimulationModel, ApiType.VerifySimulation, GlobalUser.USER.AccessToken);

                    #endregion

                    if (result.retCode == 1)
                    {
                        var paperFile = new DownloadPaperModel(paperItem.ZipFileUrl, mainWin);
                        paperFile.PaperDownloadComplete += PaperFileOnPaperDownloadComplete;
                        tempFile = paperFile.FileName;
                    }
                    else
                    {
                        var view = new OfflineTipsDialog();
                        // 显示对话框
                        var dialog = DialogHostEx.ShowDialog(GlobalUser.MainWin, view, OfflineTipsOpenedEventHandler);
                    }
                }));
                //});
            }
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
                .ContinueWith(t => {
                    eventargs.Session.Close();
                    //跳转到 列表
                    Messenger.Default.Send(new NavigateMessage(), "ShowUserpapers");
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }


        private void PaperFileOnPaperDownloadComplete()
        {
            if (File.Exists(tempFile))
            {
                //解压试卷到指定 Data 目录
                ZipHelper.UnZip(tempFile, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.GetUserDataFolder()));

                UploadPaperUC();

                DownloadPaperCursor = Cursors.Arrow;
                ShowNewLable = Visibility.Collapsed;
            }
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
                    paperItem.score =exam_attend.score;

                    ShowPaperUcInfo();
                }
            }));
        } 

        #endregion
    }
}

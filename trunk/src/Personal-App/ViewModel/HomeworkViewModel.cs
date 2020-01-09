using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using Personal_App.Domain;
using Personal_App.Domain.Exam;
using Personal_App.ViewModel.Exam;
using Personal_App.ViewModel.Menu;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;

namespace Personal_App.ViewModel
{
    public class HomeworkViewModel : MainViewModel
    {

        private HomeworkListItem homeworkItem;

        private DialogSession dialogSession;

        public HomeworkViewModel()
        {

        }

        public HomeworkViewModel(HomeworkListItem _homeworkItem, bool isFinish = false) : this()
        {
            this.homeworkItem = _homeworkItem;

            TxtHwTitle = homeworkItem.exam_title;

            BtnAgainZYView = Visibility.Collapsed;
            ShowReUploadBtnView = Visibility.Collapsed;
            ShowReDoBtnView = Visibility.Collapsed;
            //ShowOverTimeImg = Visibility.Collapsed;

            HwScore = $"{homeworkItem.score}分";
            HwTotalScore = $"/{homeworkItem.paper_score}分";
            TxtCompleteProgress = Convert.ToInt32((homeworkItem.exam_process ?? 0) * 100);

            if (isFinish)
            {
                BtnBeginZYView = Visibility.Collapsed;
                BtnShowZYView = Visibility.Visible;
                BtnAgainZYView = Visibility.Visible;
                BtnBeginZYEnable = true;
                DoAgainTxt = "再做一次";
                //ScoreImageBg = "/Resources/hwmyscore.png";
                //HwScoreEvaluate = "优";

                if (homeworkItem.score >= homeworkItem.paper_score * 0.90)
                {
                    HwScoreEvaluate = "优";
                    var color = (Color) ColorConverter.ConvertFromString("#41B612");
                    HwScoreColor = new SolidColorBrush(color);
                    ScoreImageBg = "/Resources/zys1.png";
                }
                else if (homeworkItem.score >= homeworkItem.paper_score * 0.80 &&
                         homeworkItem.score < homeworkItem.paper_score * 0.90)
                {
                    HwScoreEvaluate = "良";
                    var color = (Color) ColorConverter.ConvertFromString("#1394FA");
                    HwScoreColor = new SolidColorBrush(color);
                    ScoreImageBg = "/Resources/zys2.png";
                }
                else if (homeworkItem.score >= homeworkItem.paper_score * 0.60 &&
                         homeworkItem.score < homeworkItem.paper_score * 0.80)
                {
                    HwScoreEvaluate = "中";
                    var color = (Color) ColorConverter.ConvertFromString("#FF8414");
                    HwScoreColor = new SolidColorBrush(color);
                    ScoreImageBg = "/Resources/zys3.png";
                }
                else
                {
                    HwScoreEvaluate = "差";
                    var color = (Color) ColorConverter.ConvertFromString("#F44116");
                    HwScoreColor = new SolidColorBrush(color);
                    ScoreImageBg = "/Resources/zys4.png";
                }

                HwScore = $"{homeworkItem.score}分";
                TxtHwPushTime = $"{homeworkItem.endedat.ToString("MM月dd日 HH:mm")} 完成";

                if (homeworkItem.finishTime < DateTime.Now)
                {//过期的作业
                    BtnBeginZYEnable = false;
                    DoAgainTxt = "已结束";
                }
            }
            else
            {


                BtnBeginZYView = Visibility.Visible;
                BtnShowZYView = Visibility.Collapsed;
                BtnBeginZYEnable = true;
                //ScoreImageBg = "/Resources/hwdefaultscore.png";

                //var color = (Color) ColorConverter.ConvertFromString("#41B612");
                //HwScoreColor = new SolidColorBrush(color);

                if (homeworkItem.finishTime < DateTime.Now)
                {
                    //过期的作业  不再显示 "做题"按钮
                    BtnBeginZYEnable = false;
                    //ShowOverTimeImg = Visibility.Visible;
                    ScoreImageBg = "/Resources/overtimezy.png";
                }
                TxtHwPushTime = $"{homeworkItem.publishTime.ToString("MM月dd日 HH:mm")} 发布";

                if (homeworkItem.exam_process == 1)
                {
                    BtnBeginZYView = Visibility.Collapsed;
                    BtnShowZYView = Visibility.Collapsed;
                    BtnAgainZYView = Visibility.Collapsed;
                    BtnBeginZYEnable = false;
                    ShowReUploadBtnView = Visibility.Visible;
                    GlobalUser.DoneScore = ScoreType.ScoreSuccess;
                }
            }


            TxtHwOverTime = $"{homeworkItem.finishTime.ToString("MM月dd日 HH:mm")} 结束";


            GlobalUser.GetErrorScoreSource(User.Mobile, homeworkItem.exam_id, homeworkItem.exam_attend_id);

            if (GlobalUser.ErrScoreInfo?.Count > 0&& homeworkItem.finishTime > DateTime.Now && GlobalUser.ErrScoreInfo.Values.FirstOrDefault()?.AnswerModel.exam_attend_id == homeworkItem.exam_attend_id)
            {
                BtnBeginZYView = Visibility.Collapsed;
                BtnShowZYView = Visibility.Collapsed;
                BtnAgainZYView = Visibility.Collapsed;
                BtnBeginZYEnable = false;
                ShowReUploadBtnView = Visibility.Visible;

                if ((GlobalUser.ErrScoreInfo?.Values.ToList().Where(w => w.SubmitNum == 999)).Any())
                {
                    ShowReUploadBtnView = Visibility.Collapsed;
                    ShowReDoBtnView = Visibility.Visible;
                }
            }
        }

        #region << 属性 字段 >>

        private bool Doing = false;

        private object _ScoreImageBg;

        /// <summary>
        /// 图片 分数提示图框。
        /// </summary>
        public object ScoreImageBg
        {
            get { return _ScoreImageBg; }
            set
            {
                _ScoreImageBg = value;
                RaisePropertyChanged("ScoreImageBg");
            }
        }

        private bool _BtnBeginZYEnable;

        /// <summary>
        /// 按钮 开始作业。
        /// </summary>
        public bool BtnBeginZYEnable
        {
            get { return _BtnBeginZYEnable; }
            set
            {
                _BtnBeginZYEnable = value;
                RaisePropertyChanged("BtnBeginZYEnable");
            }
        }

        private Visibility _BtnBeginZYView;

        /// <summary>
        /// 按钮 开始作业。
        /// </summary>
        public Visibility BtnBeginZYView
        {
            get { return _BtnBeginZYView; }
            set
            {
                _BtnBeginZYView = value;
                RaisePropertyChanged("BtnBeginZYView");
            }
        }

        private Visibility _BtnAgainZYView;

        /// <summary>
        /// 按钮 再做一次。
        /// </summary>
        public Visibility BtnAgainZYView
        {
            get { return _BtnAgainZYView; }
            set
            {
                _BtnAgainZYView = value;
                RaisePropertyChanged("BtnAgainZYView");
            }
        }

        private Visibility _BtnShowZYView;

        /// <summary>
        /// 按钮 查看详情。
        /// </summary>
        public Visibility BtnShowZYView
        {
            get { return _BtnShowZYView; }
            set
            {
                _BtnShowZYView = value;
                RaisePropertyChanged("BtnShowZYView");
            }
        }

        private Visibility showReUploadBtnView;
        /// <summary>
        /// 重新提交答案 按钮 显示
        /// </summary>
        public Visibility ShowReUploadBtnView
        {
            get { return showReUploadBtnView; }
            set
            {
                showReUploadBtnView = value;
                RaisePropertyChanged("ShowReUploadBtnView");
            }
        }

        private Visibility showReDoBtnView;
        /// <summary>
        /// 重新做提交失败的题 按钮 显示
        /// </summary>
        public Visibility ShowReDoBtnView
        {
            get { return showReDoBtnView; }
            set
            {
                showReDoBtnView = value;
                RaisePropertyChanged("ShowReDoBtnView");
            }
        }

        private string _TxtHwTitle;

        /// <summary>
        /// 作业 名称
        /// </summary>
        public string TxtHwTitle
        {
            get => _TxtHwTitle;
            set
            {
                _TxtHwTitle = value;
                RaisePropertyChanged("TxtHwTitle");
            }
        }

        private string _TxtHwPushTime;

        /// <summary>
        /// 作业 发布时间
        /// </summary>
        public string TxtHwPushTime
        {
            get => _TxtHwPushTime;
            set
            {
                _TxtHwPushTime = value;
                RaisePropertyChanged("TxtHwPushTime");
            }
        }

        private string _TxtHwOverTime;

        /// <summary>
        /// 作业 最晚提交时间
        /// </summary>
        public string TxtHwOverTime
        {
            get => _TxtHwOverTime;
            set
            {
                _TxtHwOverTime = value;
                RaisePropertyChanged("TxtHwOverTime");
            }
        }

        private string _HwScore;

        /// <summary>
        /// 作业 得分
        /// </summary>
        public string HwScore
        {
            get => _HwScore;
            set
            {
                _HwScore = value;
                RaisePropertyChanged("HwScore");
            }
        }

        private string _HwTotalScore;

        /// <summary>
        /// 作业 总分
        /// </summary>
        public string HwTotalScore
        {
            get => _HwTotalScore;
            set
            {
                _HwTotalScore = value;
                RaisePropertyChanged("HwTotalScore");
            }
        }

        private string _HwScoreEvaluate;

        /// <summary>
        /// 作业 评价
        /// </summary>
        public string HwScoreEvaluate
        {
            get => _HwScoreEvaluate;
            set
            {
                _HwScoreEvaluate = value;
                RaisePropertyChanged("HwScoreEvaluate");
            }
        }

        private object _HwScoreColor;

        /// <summary>
        /// 我的得分  分数颜色
        /// </summary>
        public object HwScoreColor
        {
            get => _HwScoreColor;
            set
            {
                _HwScoreColor = value;
                RaisePropertyChanged("HwScoreColor");
            }
        }

        //private Visibility _ShowOverTimeImg;

        ///// <summary>
        ///// 显示 作业过期图标
        ///// </summary>
        //public Visibility ShowOverTimeImg
        //{
        //    get => _ShowOverTimeImg;
        //    set
        //    {
        //        _ShowOverTimeImg = value;
        //        RaisePropertyChanged("ShowOverTimeImg");
        //    }
        //}


        private double _TxtCompleteProgress;

        /// <summary>
        /// 显示 完成进度
        /// </summary>
        public double TxtCompleteProgress
        {
            get => _TxtCompleteProgress;
            set
            {
                _TxtCompleteProgress = value;
                RaisePropertyChanged("TxtCompleteProgress");
            }
        }

        private string _DoAgainTxt;

        /// <summary>
        /// 显示 完成  再做一次   逾期 过期后 显示为  已结束
        /// </summary>
        public string DoAgainTxt
        {
            get => _DoAgainTxt;
            set
            {
                _DoAgainTxt = value;
                RaisePropertyChanged("DoAgainTxt");
            }
        }
        #endregion

        #region << Btn Command >>


        private RelayCommand startZYCommand; //开始 作业

        public RelayCommand StartZYCommand
        {
            get
            {
                return startZYCommand ?? (startZYCommand = new RelayCommand(
                           (action) =>
                           {
                               Doing = false;
                               //GlobalUser.SelectPaperJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.GetUserDataFolder(), paperItem.ZipFileName,
                               //    $"{paperItem.ZipFileName}.json");
                               //GlobalUser.SelectPaperName = paperItem.ZipFileName;

                               GlobalUser.SelectPaperName = GlobalUser.SelectPaperViewName = homeworkItem.exam_title;
                               GlobalUser.SelectPaperNumber = homeworkItem.exam_id;
                               GlobalUser.AttendPaperItemId = homeworkItem.exam_attend_id;
                               GlobalUser.SelectPaperTotalScore = homeworkItem.paper_score;
                               
                               StartDownloadPaper(ExamType.FirstTask, "");

                               //string zyId = homeworkItem.exam_id.Split('#')[0];

                               //if (GlobalUser.USER.UserZy == null || GlobalUser.USER.UserZy.Count < 1)
                               //{
                               //    if (GlobalUser.USER.UserZy == null)
                               //        GlobalUser.USER.UserZy = new List<TaskInfo>();
                               //    GlobalUser.USER.UserZy.Add(new TaskInfo()
                               //    {
                               //        UserPhone = GlobalUser.USER.Mobile,
                               //        ZyID = zyId,
                               //        ZyName = homeworkItem.exam_title,
                               //        ZySubs = new List<TaskSubInfo>()
                               //    });
                               //}
                               //else
                               //{
                               //    var userTask = GlobalUser.USER.UserZy.FirstOrDefault(w =>
                               //        w.UserPhone == GlobalUser.USER.Mobile && w.ZyID == zyId);
                               //    if (userTask != null && !userTask.CurrentTaskDone)
                               //    {
                               //        //new 显示
                               //        var redoView = new RedoTaskDialog();
                               //        //打开 对话框
                               //        DialogHostEx.ShowDialog(GlobalUser.MainWin, redoView, ExtendedOpenedEventHandler,
                               //            ExtendedReDoViewClosingEventHandler);

                               //        return;
                               //    }
                               //}

                               ////new 显示
                               //var view = new ExamMainWin(null, ExamType.FirstTask);
                               ////打开 对话框
                               //DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler,
                               //    ExtendedClosingEventHandler);
                           }));
            }
        }

        private RelayCommand againZYCommand; //再做一次 作业

        public RelayCommand AgainZYCommand
        {
            get
            {
                return againZYCommand ?? (againZYCommand = new RelayCommand(
                           (action) =>
                           {
                               Doing = false;

                               GlobalUser.SelectPaperName = GlobalUser.SelectPaperViewName = homeworkItem.exam_title;
                               GlobalUser.SelectPaperNumber = homeworkItem.exam_id;
                               GlobalUser.AttendPaperItemId = homeworkItem.exam_attend_id;
                               GlobalUser.SelectPaperTotalScore = homeworkItem.paper_score;

                               StartDownloadPaper(ExamType.AgainTask, "");

                               //string zyId = homeworkItem.exam_id.Split('#')[0];

                               //if (GlobalUser.USER.UserZy == null)
                               //{
                               //    GlobalUser.USER.UserZy = new List<TaskInfo>();
                               //    GlobalUser.USER.UserZy.Add(new TaskInfo()
                               //    {
                               //        UserPhone = GlobalUser.USER.Mobile,
                               //        ZyID = zyId,
                               //        ZyName = homeworkItem.exam_title,
                               //        ZySubs = new List<TaskSubInfo>()
                               //    });
                               //}
                               //else
                               //{
                               //    var userTask = GlobalUser.USER.UserZy.FirstOrDefault(w =>
                               //        w.UserPhone == GlobalUser.USER.Mobile && w.ZyID == zyId);
                               //    if (userTask != null &&
                               //        (!(userTask.ZySubs.Sum(m => m.DoneInt) == 0 ||
                               //           userTask.ZySubs.Sum(m => m.DoneInt) == userTask.ZySubs.Count)))
                               //    {
                               //        //new 显示
                               //        var redoView = new RedoTaskDialog();
                               //        //打开 对话框
                               //        DialogHostEx.ShowDialog(GlobalUser.MainWin, redoView, ExtendedOpenedEventHandler,
                               //            ExtendedReDoViewClosingEventHandler);

                               //        return;
                               //    }
                               //}

                               ////new 显示
                               //var view = new ExamMainWin(null, ExamType.AgainTask);
                               ////打开 对话框
                               //DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler,
                               //    ExtendedClosingEventHandler);
                           }));
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

                               GlobalUser.SelectPaperName = GlobalUser.SelectPaperViewName = homeworkItem.exam_title;
                               GlobalUser.SelectPaperNumber = homeworkItem.exam_id;
                               GlobalUser.AttendPaperItemId = homeworkItem.exam_attend_id;
                               GlobalUser.SelectExamAttendResult = "";
                               //GlobalUser.SelectPaperAvgScore5Points = paperItem.AvgScore5Points;
                               GlobalUser.SelectPaperTotalScore = homeworkItem.paper_score;


                               StartDownloadPaper(ExamType.Result, "");
                               ////new 显示
                               //var view = new ExamMainWin(null, ExamType.Result);
                               ////打开 对话框
                               //DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler,
                               //    ExtendedClosingEventHandler);
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
                               GlobalUser.SelectPaperName = GlobalUser.SelectPaperViewName = homeworkItem.exam_title;
                               GlobalUser.SelectPaperNumber = homeworkItem.exam_id;
                               GlobalUser.AttendPaperItemId = homeworkItem.exam_attend_id;
                               GlobalUser.SelectExamAttendResult = "";
                               //GlobalUser.SelectPaperAvgScore5Points = paperItem.AvgScore5Points;
                               GlobalUser.SelectPaperTotalScore = homeworkItem.paper_score; 

                               StartDownloadPaper(ExamType.ReDo, "");
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

                               GlobalUser.SelectPaperName = GlobalUser.SelectPaperViewName = homeworkItem.exam_title;
                               GlobalUser.SelectPaperNumber = homeworkItem.exam_id;
                               GlobalUser.AttendPaperItemId = homeworkItem.exam_attend_id;
                               GlobalUser.SelectExamAttendResult = "";
                               //GlobalUser.SelectPaperAvgScore5Points = paperItem.AvgScore5Points;
                               GlobalUser.SelectPaperTotalScore = homeworkItem.paper_score;

                               StartDownloadPaper(ExamType.ReUpload, "");
                           }));
            }
        }

        #endregion

        #region << 自定义方法 >>


        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {
            dialogSession = eventArgs.Session;
        }


        /// <summary>
        /// 作业关闭事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter != null)
            {
                GlobalUser.DoAnswer = false;

                GlobalUser.CleanUp(); // 退出 录播设备

                if ((bool) eventArgs.Parameter)
                {//关闭 等待窗
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        try
                        {
                            ((dialogSession.Content as ExamMainWin).GridContent.Children[0] as ExamCompleteUC).Dispose();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }));

                    //true: 题目操作阶段
                    if (GlobalUser.DoneItemExam == false)
                    {
                        return;
                    }
                    else
                    {
                        if (GlobalUser.MenuType == MenuType.Task)
                            RememberUser();

                        Messenger.Default.Send(new NavigateMessage(MenuHomeworkVM.ViewName, "R", true),
                            "ShowUserpapers");
                    }
                }
                else
                {
                    //try
                    //{
                    //    ((dialogSession.Content as ExamMainWin).GridContent.Children[0] as ExamCompleteUC).Dispose();
                    //}
                    //catch (Exception e)
                    //{
                    //    Console.WriteLine(e);
                    //}
                }
            }

            //var result = DialogHostEx.ShowDialog(new CloseExamDialog(), "CloseExamReadDialog", ExtendedCloseExamOpenedEventHandler,
            //    ExtendedCloseExamClosingEventHandler);
        }

        /// <summary>
        /// 作业关闭事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ExtendedReDoViewClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter != null)
            {
                GlobalUser.DoAnswer = false;

                if (Doing)
                {
                    Doing = false;

                    GlobalUser.CleanUp(); // 退出 录播设备

                    if ((bool) eventArgs.Parameter)
                    {
                        //true: 题目操作阶段
                        if (GlobalUser.DoneItemExam == false)
                        {
                            return;
                        }
                        else
                        {
                            if (GlobalUser.MenuType == MenuType.Task)
                                RememberUser();

                            Messenger.Default.Send(new NavigateMessage(MenuHomeworkVM.ViewName, "R", true),
                                "ShowUserpapers");
                        }
                    }
                }
                else
                {
                    //DialogHost.CloseAllShow();

                    if ((int) eventArgs.Parameter == 1)
                    {

                        string zyId = homeworkItem.exam_id.Split('#')[0];

                        var userTask = GlobalUser.USER.UserZy.FirstOrDefault(w =>
                            w.UserPhone == GlobalUser.USER.Mobile && w.ZyID == zyId);
                        foreach (var taskInfo in GlobalUser.USER.UserZy.Where(w =>
                            w.UserPhone == GlobalUser.USER.Mobile && w.ZyID == zyId))
                        {
                            taskInfo.ZySubs.ForEach(x => x.Done = false);
                        }

                        //重新做
                        var view = new ExamMainWin(null, ExamType.AgainTask);
                        //打开 对话框
                        //DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);

                        //dialogSession.UpdateContent(view);
                        eventArgs.Session.UpdateContent(view);

                        Doing = true;

                        eventArgs.Cancel();
                    }
                    else if ((int) eventArgs.Parameter == 2)
                    {
                        //继续做
                        var view = new ExamMainWin(null, ExamType.ContinueTask);
                        //打开 对话框
                        //DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
                        //dialogSession.UpdateContent(view);
                        eventArgs.Session.UpdateContent(view);
                        Doing = true;

                        eventArgs.Cancel();
                    }
                    else
                    {
                        Doing = false;
                    }
                }
            }
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

        private void StartDownloadPaper(ExamType eType, string mainWin)
        {
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
                    OpenExamWin(eType);
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

        private void PaperFileOnPaperDownloadComplete()
        {
            var view = dialogSession.Content as DownloadProgressDialog;
            var vm = view.DataContext as DownloadProgressDialogVM;
            var et = vm.EType;
            dialogSession?.Close();

            OpenExamWin(et);
        }

        private void PaperFileOnPaperDownloadError()
        {
            if (!dialogSession.IsEnded)
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

        /// <summary>
        /// 打开 作业窗口
        /// </summary>
        /// <param name="et"></param>
        private void OpenExamWin(ExamType et)
        {
            if (et != null)
            {
                if (et == ExamType.FirstTask || et == ExamType.ContinueTask )
                {
                    CheckZySchedule();
                }

                if (et == ExamType.AgainTask)
                {
                    if (CheckZySchedule())
                        return;
                }

                //new 显示
                var view0 = new ExamMainWin(null, et);
                //打开 对话框
                DialogHostEx.ShowDialog(GlobalUser.MainWin, view0, ExtendedOpenedEventHandler,
                    ExtendedClosingEventHandler);
            }
        }

        /// <summary>
        /// 检查作业进度
        /// </summary>
        private bool CheckZySchedule()
        {
            string zyId = homeworkItem.exam_id.Split('#')[0];

            if (GlobalUser.USER.UserZy == null || GlobalUser.USER.UserZy.Count < 1)
            {
                if (GlobalUser.USER.UserZy == null)
                    GlobalUser.USER.UserZy = new List<TaskInfo>();
                GlobalUser.USER.UserZy.Add(new TaskInfo()
                {
                    UserPhone = GlobalUser.USER.Mobile,
                    ZyID = zyId,
                    ZyName = homeworkItem.exam_title,
                    ZySubs = new List<TaskSubInfo>()
                });
            }
            else
            {
                var userTask = GlobalUser.USER.UserZy.FirstOrDefault(w =>
                    w.UserPhone == GlobalUser.USER.Mobile && w.ZyID == zyId);

                if (userTask == null)
                {
                    GlobalUser.USER.UserZy.Add(new TaskInfo()
                    {
                        UserPhone = GlobalUser.USER.Mobile,
                        ZyID = zyId,
                        ZyName = homeworkItem.exam_title,
                        ZySubs = new List<TaskSubInfo>()
                    });
                    return false;
                }

                if (userTask != null && !userTask.CurrentTaskDone)
                {
                    //new 显示
                    var redoView = new RedoTaskDialog();
                    //打开 对话框
                    DialogHostEx.ShowDialog(GlobalUser.MainWin, redoView, ExtendedOpenedEventHandler,
                        ExtendedReDoViewClosingEventHandler);

                    return true; 
                }
            }

            return false;
        }

        #endregion
    }
}

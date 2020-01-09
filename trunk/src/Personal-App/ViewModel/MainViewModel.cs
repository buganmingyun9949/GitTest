using Framework.Logging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using ST.Models.Paper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Personal_App.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : MainViewModelBase
    {
        internal Dictionary<string, FrameworkElement> views;
        internal FrameworkElement currentView;
        internal string currentViewName;


        public virtual void MainLoadoadMenu(bool isReload)
        {
        }

        public void SetupView(string viewName, FrameworkElement view, ViewModelBase viewModel)
        {
            if (!views.Keys.Contains(viewName))
            {
                view.DataContext = viewModel;
                views?.Add(viewName, view);
            }
        }

        public void SetupView(string viewName, FrameworkElement view, ViewModelBase viewModel,
            Dictionary<string, FrameworkElement> _views)
        {
            view.DataContext = viewModel;
            _views.Add(viewName, view);
        }

        public void ClearDefaultBindWin()
        {
            views = new Dictionary<string, FrameworkElement>();
        }

        public void ClearOneBindWin(string viewName)
        {
            views[viewName].DataContext = null;
            views.Remove(viewName);
        }


        public FrameworkElement CurrentView
        {
            get { return currentView; }
            set
            {
                if (this.currentView != value)
                {
                    currentView = value;
                    RaisePropertyChanged("CurrentView");
                }
            }
        }


        private UserInfo _user;

        /// <summary>
        /// 用户名。
        /// </summary>
        public UserInfo User
        {
            get { return GlobalUser.USER; }
            set
            {
                _user = value;
                RaisePropertyChanged("User");
            }
        }

        private ClassInfo _classInfo;

        /// <summary>
        /// 用户名。
        /// </summary>
        public ClassInfo Classinfo
        {
            get { return GlobalUser.CLASSINFO; }
            set
            {
                _classInfo = value;
                RaisePropertyChanged("ClassInfo");
            }
        }

        ///// <summary>
        ///// 有效期。
        ///// </summary>
        //public string Validity { get; set; }

        private string _validity;

        /// <summary>
        /// 有效期。
        /// </summary>
        public string Validity
        {
            get { return _validity; }
            set
            {
                _validity = value;
                RaisePropertyChanged("Validity");
            }
        }

        /// <summary>
        /// 绑定选中试卷信息
        /// </summary>
        /// <param name="OutMsg">是否显示异常消息  true 显示</param>
        /// <returns></returns>
        public bool BindPaperInfo(bool OutMsg = true)
        {
            var ml = new GetPaperInfoDetail()
            {
                exam_id = GlobalUser.SelectPaperNumber.Split('#')[0],
                token = GlobalUser.USER.Token
            };

            ApiType api = ApiType.GetPaperInfoDetail;

            if (GlobalUser.MenuType == MenuType.Task)
            {
                api = ApiType.GetUserHomeworkDetail;
            }

            var result1 = WebProxy(ml, api, null, "get");

            if (result1.retCode == 4001 && result1.retMsg.ToLower().Contains("token"))
            {
                //回到登录
                GlobalUser.CleanUp();
                if (OutMsg)
                    Messenger.Default.Send(new ExamScoreNavigateMessage(), "LoginFailure");

                return false;
            }

            if (result1.retCode == 40400)
            {
                if (OutMsg)
                    Messenger.Default.Send(new MainDialogMessage(result1.retMsg), "MainMessageDialog");
                return false;
            }

            GlobalUser.SelectPaperInfo =
                JsonHelper.FromJson<Paper_Info>(result1.retData.paper_info.ToString());

            GlobalUser.SelectExamAttendResult = result1.retData.exam_attend_result.ToString().Replace("[]", "");

            GlobalUser.SelectExamAttend =
                JsonHelper.FromJson<Exam_Attend>(result1.retData.exam_attend.ToString());

            return true;
        }


        /// <summary>
        /// result bool: true:下载文件都有   false:下载的试卷内容文件 有缺失 不完整
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool CheckFile(string path, bool isUserFiles = false)
        {
            string s_url = ""; //保存路径
            Dictionary<string, string> fileNameDic =
                JsonHelper.FromJson<Dictionary<string, string>>(GlobalUser.SelectPaperInfo.paper_assets.ToString());

            var myAnswerResults = new List<Exam_Attend_Result_Item>();

            if (isUserFiles)
            {
                if (!string.IsNullOrEmpty(GlobalUser.SelectExamAttendResult.Trim()))
                    myAnswerResults = JsonHelper
                        .FromJson<List<Exam_Attend_Result_Item>>(GlobalUser.SelectExamAttendResult)
                        .Where(w => w.user_answer.Contains("records.")).ToList();
            }


            for (int i = 0; i < myAnswerResults.Count; i++)
            {
                var userAnswer = myAnswerResults[i].user_answer;
                var userAnswerSplit = userAnswer.Split('/');
                if (userAnswerSplit.Length == 2)
                    fileNameDic.Add(userAnswerSplit[1], $"http://{userAnswer}.mp3");
            }

            if (fileNameDic == null) return true;

            foreach (string key in fileNameDic.Keys)
            {
                s_url = Path.Combine(path,
                    SecurityHelper.HmacMd5Encrypt(Path.GetFileNameWithoutExtension(fileNameDic[key]),
                            GlobalUser.FILEPWD,
                            System.Text.Encoding.UTF8)
                        .ToLower() + ".qf");

                if (!File.Exists(s_url))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 注销
        /// </summary>
       protected void Loginout()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                //var logoutResult = WebProxy(ApiType.UserLogout, GlobalUser.USER.AccessToken);//暂时 不用调用接口  移除token记录

                string userFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER);
                string userDataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER,
                    $"user.dat");
                GlobalUser.USER.Token = "";
                UserDataModel userDataModel = new UserDataModel
                {
                    AccessToken = "",
                    UserName = GlobalUser.USER.UserName,
                    Phone = GlobalUser.USER.Mobile,
                    Avatar = GlobalUser.USER.Avatar,
                    Password = GlobalUser.USER.Password,
                    Data = GlobalUser.USER,
                    ClassData = GlobalUser.CLASSINFO,
                    UserZy = GlobalUser.USER.UserZy,
                    StudyCard = GlobalUser.STUDYCARD,
                    UnFirstOpen = GlobalUser.USER.UnFirstOpen,
                };
                GlobalUser.USER = null;
                string userEncoded =
                    Base64Provider.AESEncrypt(userDataModel.ToJsonItem(), Base64Provider.KEY, Base64Provider.IV);
                try
                {
                    if (!Directory.Exists(userFolder))
                    {
                        Directory.CreateDirectory(userFolder);
                    }

                    if (!File.Exists(userDataFile))
                    {
                        var file = File.Create(userDataFile);
                        file.Close();
                        file.Dispose();
                    }

                    using (StreamWriter sw = new StreamWriter(userDataFile, false))
                    {
                        sw.Write(userEncoded);
                    }
                }
                catch (Exception e)
                {
                    Log4NetHelper.Error("写入用户信息异常：", e);
                }

                DialogHost.CloseAllShow();

                {
                    Log_Data log = new Log_Data();
                    log.log_desc = Log_Type.PC_Logout.ToString();
                    log.log_text = $"注销:{GlobalUser.USER?.Mobile}";
                    log.log_device = JsonHelper.ToJson(GlobalUser.MACHINEINFO.GetDevice());
                    WebApiProxy.GetHtmlRespInfo(log, ApiType.SysLog, null, "Post");
                }

                GlobalUser.MainWin?.Hide();
                GlobalUser.MainWin = null;
                //GlobalUser.MainWin?.Close();

                ClearMemory();

                GlobalUser.AutoLoggedIn = false;
                GlobalUser.MainWin = new MainLogin();
                GlobalUser.MainWin.DataContext = new MainLoginVM(false);
                GlobalUser.MainWin.Show();
                GlobalUser.MainWin.Focus();

                //_metroWindow.Hide();
                //_metroWindow.Close();
                //_metroWindow = null;
            }));
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Framework.Logging;
using MaterialDesignThemes.Wpf;
using Personal_App.Domain;
using Personal_App.Properties;
using ST.Common;
using ST.Common.Domain;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using Card = ST.Models.Api.Card;

namespace Personal_App.ViewModel
{
    public class MainLoginVM : MainViewModel
    {
        public MainLoginVM()
        {
            validCode = new ValidCode(4, ValidCode.CodeType.Characters);

            ValidCodeImg = BitmapFrame.Create(validCode.CreateCheckCodeImage());

            LoginUcView = Visibility.Visible;
            RegUcView = Visibility.Collapsed;
            ResetUcView = Visibility.Collapsed;
            ErrIsShow = Visibility.Collapsed;

            CheckRememberUser();
        }

        public MainLoginVM(bool autoLogin)
        {
            validCode = new ValidCode(4, ValidCode.CodeType.Characters);

            ValidCodeImg = BitmapFrame.Create(validCode.CreateCheckCodeImage());

            LoginUcView = Visibility.Visible;
            RegUcView = Visibility.Collapsed;
            ResetUcView = Visibility.Collapsed;
            ErrIsShow = Visibility.Collapsed;

            CheckRememberUser(autoLogin);
        }

        /// <summary>
        ///     关闭程序。
        /// </summary>
        public ICommand CloseLoginWindowCommand => new RelayCommand(LoginWindowClose);

        /// <summary>
        ///     检查记住的用户。
        /// </summary>
        private void CheckRememberUser(bool autoLogin = true)
        {
            try
            {
                var userDataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER,
                    "user.dat");

                if (File.Exists(userDataFile))
                    using (var streamReader = new StreamReader(userDataFile))
                    {
                        var userEncoded = streamReader.ReadToEnd();
                        if (!string.IsNullOrEmpty(userEncoded))
                        {
                            var userData =
                                Base64Provider.AESDecrypt(userEncoded, Base64Provider.KEY, Base64Provider.IV);
                            var userDataModel = userData.FromJsonTo<UserDataModel>();
                            if (!string.IsNullOrEmpty(userDataModel?.Phone))
                            {
                                GlobalUser.USER = userDataModel?.Data;
                                GlobalUser.USER.Mobile = PhoneNum = userDataModel?.Phone;
                                GlobalUser.USER.Password = Pwd = userDataModel?.Password;
                                GlobalUser.USER.UserZy = userDataModel?.UserZy == null
                                    ? new List<TaskInfo>()
                                    : userDataModel?.UserZy;
                                GlobalUser.USER.UnFirstOpen = userDataModel.UnFirstOpen;

                                if (!string.IsNullOrEmpty(userDataModel?.AccessToken))
                                {
                                    GlobalUser.CLASSINFO = userDataModel?.ClassData;
                                    GlobalUser.STUDYCARD = userDataModel?.StudyCard;
                                    GlobalUser.GRADEINFO = userDataModel?.GradeData;

                                    GlobalUser.USER.Expire_status = GlobalUser.STUDYCARD.expire_status;


                                    var windows = Application.Current.Windows;
                                    //GlobalUser.LoginWin = new Window();
                                    //foreach (Window window in windows)
                                    //    if (window.Name == "ml")
                                    //        GlobalUser.LoginWin = window;
                                    GlobalUser.AutoLoggedIn = true;
                                    checkOK = true;
                                    if (autoLogin)
                                        LoggedIn(GlobalUser.LoginWin);
                                }
                            }
                        }
                    }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("读取用户信息异常：", e);
            }
        }

        /// <summary>
        ///     记住用户。
        /// </summary>
        /// <param name="userName">用户名称。</param>
        /// <param name="phone">手机号码。</param>
        /// <param name="accessToken">访问令牌。</param>
        //private new void RememberUser()
        //{
        //    var userFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER);
        //    var userDataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER, "user.dat");
        //    var userDataModel = new UserDataModel
        //    {
        //        UserName = GlobalUser.USER.UserName,
        //        Phone = GlobalUser.USER.Mobile,
        //        Avatar = GlobalUser.USER.Avatar,
        //        AccessToken = GlobalUser.USER.Token,
        //        Password = GlobalUser.USER.Password,
        //        Data = GlobalUser.USER,
        //        ClassData = GlobalUser.CLASSINFO,
        //        StudyCard = GlobalUser.STUDYCARD,
        //        UnFirstOpen = GlobalUser.USER.UnFirstOpen
        //    };

        //    var userEncoded =
        //        Base64Provider.AESEncrypt(userDataModel.ToJsonItem(), Base64Provider.KEY, Base64Provider.IV);
        //    try
        //    {
        //        if (!Directory.Exists(userFolder)) Directory.CreateDirectory(userFolder);
        //        if (!File.Exists(userDataFile))
        //        {
        //            var file = File.Create(userDataFile);
        //            file.Close();
        //            file.Dispose();
        //        }

        //        using (var sw = new StreamWriter(userDataFile, false))
        //        {
        //            sw.Write(userEncoded);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Log4NetHelper.Error("写入用户信息异常：", e);
        //    }
        //}


        /// <summary>
        ///     登录成功。
        /// </summary>
        /// <param name="loginWin">登录窗口。</param>
        private void LoggedIn(Window loginWin)
        {
            if (!GlobalUser.AutoLoggedIn) RememberUser();
            ErrIsShow = Visibility.Hidden;

            //ErrMsg = "请输入有效的手机号！";
            //MessageBox.Show("验证成功");

            //GetUserCard();

            if (GlobalUser.USER.RetCode == 40400)
            {
                loginWin.Show();
                //loginWin.Focus();
                return;
            }

            loginWin.Hide();
            {
                var log = new Log_Data();
                log.log_desc = Log_Type.APP_RUN.ToString();
                log.log_text = GlobalUser.USER.ToJsonItem(); //$"登录:{GlobalUser.USER?.Mobile}";
                log.log_device = JsonHelper.ToJson(GlobalUser.MACHINEINFO.GetDevice());
                WebApiProxy.GetHtmlRespInfo(log, ApiType.SysLog, null, "Post");
            }

            //未绑卡
            if (GlobalUser.STUDYCARD == null || GlobalUser.STUDYCARD.expire_status == -1 ||
                GlobalUser.STUDYCARD.expire_time == null)
            {
                var card = new MainBindCard();
                card.Show();
                card.Focus();
                return;
            }

            //if (GlobalUser.MainWin == null)
            if (!GlobalUser.LoginExpired) GlobalUser.MainWin = new MainWindow();

            GlobalUser.MainWin.Show();
            GlobalUser.MainWin.Focus();
        }


        /// <summary>
        ///     获取用户学习卡。
        /// </summary>
        private void GetUserCard()
        {
            #region 获取用户学习卡...

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                //TokenModel lm = new TokenModel()
                //{
                //    token = GlobalUser.USER.token
                //};

                var cardResult = WebProxy(ApiType.CardList, GlobalUser.USER.Token);

                if (cardResult.retData == null) return;
                if (cardResult.retCode == 40400)
                {
                    GlobalUser.USER.RetCode = cardResult.retCode;
                    ErrIsShow = Visibility.Visible;
                    ErrMsg = cardResult.retMsg;
                    return;
                }

                GlobalUser.USER.CardsList = JsonHelper.FromJson<List<Card>>(cardResult.retData.ToString()); //抓取我卡片

                // 从学习卡列表获取默认卡，默认取第一个。
                var defaultCard =
                    GlobalUser.USER.CardsList.FirstOrDefault(c => c.is_current == 1);

                if (defaultCard != null)
                {
                    //GlobalUser.USER.Card = defaultCard;
                    //GlobalUser.USER.Expire_status = defaultCard.expire_status;
                }
            }));

            #endregion
        }


        private void LoginWindowClose(object o)
        {
            ////let's set up a little MVVM, cos that's what the cool kids are doing:
            //var view = new SignOutDialog()
            //{
            //    DataContext = new SignOutDialogVM()
            //};

            ////show the dialog
            //var result = await DialogHost.Show(view, o, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);

            ////check the result...
            //Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
            //Task.Delay(TimeSpan.FromSeconds(1)).ContinueWith((t, _) => Environment.Exit(0), null, TaskScheduler.FromCurrentSynchronizationContext());//关闭程序


            Task.Factory.StartNew(() => Thread.Sleep(1000))
                .ContinueWith(t => Environment.Exit(0), TaskScheduler.FromCurrentSynchronizationContext());
        }

        #region 界面字段...

        private string phoneNum; //手机号
        private string identifyCode; //验证码
        private string imageCode; //图片验证码
        private Visibility errIsShow = Visibility.Hidden; //默认不显示 错误提示
        private string errMsg = "错误提示"; //要显示错误提示
        private Visibility imageShow = Visibility.Collapsed; //图片验证窗口默认隐藏

        private int checkCount; //每次运行登录,手机验证码验证次数,超过3次后 本地开启图片验证 

        private bool checkOK; //登录内容验证

        private object validCodeImg; //图片验证码

        private readonly ValidCode validCode;

        private Visibility _LoginUcView;
        private Visibility _RegUcView;
        private Visibility _ResetUcView;

        /// <summary>
        ///     显示 登录
        /// </summary>
        public Visibility LoginUcView
        {
            get => _LoginUcView;

            set
            {
                _LoginUcView = value;
                RaisePropertyChanged("LoginUcView");
            }
        }

        /// <summary>
        ///     显示 注册
        /// </summary>
        public Visibility RegUcView
        {
            get => _RegUcView;

            set
            {
                _RegUcView = value;
                RaisePropertyChanged("RegUcView");
            }
        }

        /// <summary>
        ///     显示 重置密码
        /// </summary>
        public Visibility ResetUcView
        {
            get => _ResetUcView;

            set
            {
                _ResetUcView = value;
                RaisePropertyChanged("ResetUcView");
            }
        }

        public string PhoneNum
        {
            get => phoneNum;
            set
            {
                phoneNum = value;
                RaisePropertyChanged("PhoneNum");
            }
        }

        public Visibility ErrIsShow
        {
            get => errIsShow;

            set
            {
                errIsShow = value;
                RaisePropertyChanged("ErrIsShow");
            }
        }

        public string ErrMsg
        {
            get => errMsg;

            set
            {
                errMsg = value;
                RaisePropertyChanged("ErrMsg");
            }
        }

        public string IdentifyCode
        {
            get => identifyCode;

            set
            {
                identifyCode = value;
                RaisePropertyChanged("IdentifyCode");
            }
        }

        public string ImageCode
        {
            get => imageCode;

            set
            {
                imageCode = value;
                RaisePropertyChanged("ImageCode");
            }
        }

        public Visibility ImageShow
        {
            get => imageShow;

            set
            {
                imageShow = value;
                RaisePropertyChanged("ImageShow");
            }
        }

        public object ValidCodeImg
        {
            get => validCodeImg;

            set
            {
                validCodeImg = value;
                RaisePropertyChanged("ValidCodeImg");
            }
        }

        #region << 登录 >>

        //private string _PhoneNum;

        //public string PhoneNum
        //{
        //    get
        //    {
        //        return _PhoneNum;
        //    }
        //    set
        //    {
        //        _PhoneNum = value;
        //        RaisePropertyChanged("PhoneNum");
        //    }
        //}

        private string _Pwd;

        public string Pwd
        {
            get => _Pwd;
            set
            {
                _Pwd = value;
                RaisePropertyChanged("Pwd");
            }
        }

        #endregion

        #region << 注册 >>

        private string _NewPhoneNum;

        public string NewPhoneNum
        {
            get => _NewPhoneNum;
            set
            {
                _NewPhoneNum = value;
                PhoneNum = value;
                RaisePropertyChanged("NewPhoneNum");
            }
        }

        private string _NewPhoneCode;

        public string NewPhoneCode
        {
            get => _NewPhoneCode;
            set
            {
                _NewPhoneCode = value;
                RaisePropertyChanged("NewPhoneCode");
            }
        }

        private string _NewPwd;

        public string NewPwd
        {
            get => _NewPwd;
            set
            {
                _NewPwd = value;
                RaisePropertyChanged("NewPwd");
            }
        }

        private string _NewTrueName;

        /// <summary>
        ///     真实姓名
        /// </summary>
        public string NewTrueName
        {
            get => _NewTrueName;
            set
            {
                _NewTrueName = value;
                RaisePropertyChanged("NewTrueName");
            }
        }

        #endregion

        #region << 修改 密码 >>

        private string _ReUsername;

        public string ReUsername
        {
            get => _ReUsername;
            set
            {
                _ReUsername = value;
                PhoneNum = value;
                RaisePropertyChanged("ReUsername");
            }
        }

        private string _RePhoneCode;

        public string RePhoneCode
        {
            get => _RePhoneCode;
            set
            {
                _RePhoneCode = value;
                RaisePropertyChanged("RePhoneCode");
            }
        }

        private string _RePwd;

        public string RePwd
        {
            get => _RePwd;
            set
            {
                _RePwd = value;
                RaisePropertyChanged("RePwd");
            }
        }

        #endregion

        #endregion

        #region 为获取验证码按钮设置内容...

        private PhoneVerificationCode src = new PhoneVerificationCode();

        public PhoneVerificationCode Src
        {
            get => src;

            set
            {
                src = value;
                RaisePropertyChanged("Src");
            }
        }

        #endregion

        #region  << Btn Command >>

        private RelayCommand sendCode; //获取短信验证码

        public RelayCommand SendCode
        {
            get
            {
                return sendCode ?? (sendCode = new RelayCommand(
                           () =>
                           {
                               if (!string.IsNullOrEmpty(PhoneNum))
                               {
                                   PhoneNum = phoneNum.ToDBC();
                                   if (RegexHelper.IsHandset(PhoneNum))
                                   {
                                       src.GetCode(PhoneNum);

                                       var lm = new SendCodeModel
                                       {
                                           phone = phoneNum
                                       };
                                       // 异步获取验证码
                                       Application.Current.Dispatcher.Invoke(new Action(() =>
                                       {
                                           var result = WebProxy(lm, ApiType.SendCode);
                                           if (result != null)
                                           {
                                               if (result.retCode == 0)
                                               {
                                                   ErrIsShow = Visibility.Hidden;
                                               }
                                               else
                                               {
                                                   ErrIsShow = Visibility.Visible;
                                                   //ErrMsg = "与服务器通讯失败，请稍候重试！";
                                                   ErrMsg = result.retMsg;
                                                   Src.ClearCode();
                                               }
                                           }
                                           else
                                           {
                                               ErrIsShow = Visibility.Visible;
                                               ErrMsg = "与服务器通讯失败，请稍候重试！";
                                           }
                                       }));
                                   }
                                   else
                                   {
                                       ErrIsShow = Visibility.Visible;
                                       ErrMsg = "请输入有效的手机号！";
                                   }
                               }
                               else
                               {
                                   //MessageBox.Show("手机号不能为空！");
                                   ErrIsShow = Visibility.Visible;
                                   ErrMsg = "手机号不能为空！";
                               }
                           }));
            }
        }

        private RelayCommand validCodeImgbtn; //获取本地验证码

        public RelayCommand ValidCodeImgbtn
        {
            get
            {
                return validCodeImgbtn ?? (validCodeImgbtn = new RelayCommand(
                           () =>
                           {
                               //validCode = new ValidCode(4, ValidCode.CodeType.Alphas);
                               ValidCodeImg = BitmapFrame.Create(validCode.CreateCheckCodeImage());
                           }));
            }
        }

        private RelayCommand loginIn; //登录

        public RelayCommand LoginIn
        {
            get
            {
                return loginIn ?? (loginIn = new RelayCommand(action =>
                {
                    var param = action as List<object>;

                    var loginWin = param[2] as Window;

                    //验证手机号
                    if (string.IsNullOrEmpty(PhoneNum))
                    {
                        ErrIsShow = Visibility.Visible;
                        checkOK = false;
                        ErrMsg = "手机号不能为空！";
                        return;
                    }

                    PhoneNum = PhoneNum.ToDBC();
                    if (!RegexHelper.IsHandset(PhoneNum))
                    {
                        ErrIsShow = Visibility.Visible;
                        checkOK = false;
                        ErrMsg = "请输入有效手机号！";
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(Pwd))
                    {
                        ErrIsShow = Visibility.Visible;
                        checkOK = false;
                        ErrMsg = "请输入密码！";
                        return;
                    }

                    #region <<验证 手机验证码>>

                    Pwd = Pwd.ToDBC();
                    // 联网验证账号密码
                    // 异步登录，防止界面假死
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        var lm = new UserLoginModel
                        {
                            phone = PhoneNum,
                            password = Pwd
                        };

                        var respResult = WebProxy(lm, ApiType.UserLogin);
                        if (respResult != null)
                        {
                            if (respResult.retCode == 0)
                            {
                                LoginApi(lm.password, respResult.retData.token.ToString());

                                checkOK = true;
                            }
                            else
                            {
                                ErrIsShow = Visibility.Visible;
                                checkOK = false;
                                ErrMsg = respResult.retMsg;
                            }
                        }
                        else
                        {
                            ErrIsShow = Visibility.Visible;
                            checkOK = false;
                            ErrMsg = "登录失败！";

                            //MessageBox.Show("登录失败");
                        }
                    }));

                    #endregion

                    //登录 成功
                    if (checkOK)
                    {
                        GlobalUser.LoginExpired = false;
                        LoggedIn(loginWin);
                    }
                }));
            }
        }

        public void LoginApi(string pwd, string token)
        {
            try
            {
                var userResult = WebProxy(ApiType.UserInfo, token);


                var unFirstOpen = GlobalUser.USER != null ? GlobalUser.USER.UnFirstOpen : false;


                GlobalUser.USER = new UserInfo
                {
                    Token = token
                    //Expire_status = respResult.retData.expire_status
                };
                GlobalUser.USER.UnFirstOpen = unFirstOpen;
                GlobalUser.USER.UserName = PhoneNum;
                GlobalUser.USER.Mobile = PhoneNum;

                if (userResult.retCode == 0)
                {
                    GlobalUser.USER.UserName = userResult.retData.user_name;
                    GlobalUser.USER.Mobile = userResult.retData.user_phone;
                    GlobalUser.USER.Avatar = string.IsNullOrEmpty(userResult.retData.user_avatar.ToString())
                        ? ""
                        : WebApiProxy.GetRedirectUrl(userResult.retData.user_avatar.ToString())
                            .Replace("https://", "http://");
                    GlobalUser.USER.Password = pwd;
                    GlobalUser.USER.study_card = userResult.retData.study_card;
                    GlobalUser.USER.class_info = userResult.retData.class_info;

                    if (string.IsNullOrEmpty(GlobalUser.USER.Avatar))
                        GlobalUser.USER.Avatar = "/Resources/head.png";
                }

                GlobalUser.CLASSINFO = null;
                if (!string.IsNullOrEmpty(userResult.retData.class_info.ToString()))
                    GlobalUser.CLASSINFO = new ClassInfo
                    {
                        Token = token,
                        Class_id = userResult.retData.class_info?.class_id,
                        Class_name = userResult.retData.class_info?.class_name,
                        Class_status = userResult.retData.class_info?.class_status
                    };

                GlobalUser.STUDYCARD = null;
                if (!string.IsNullOrEmpty(userResult.retData.study_card.ToString()))
                    GlobalUser.STUDYCARD = new Study_Card
                    {
                        used_time = userResult.retData.study_card.used_time,
                        card_key = userResult.retData.study_card.card_key,
                        expire_status = userResult.retData.study_card.expire_status,
                        expire_time = userResult.retData.study_card.expire_time,
                        agent_id = userResult.retData.study_card.agent_id,
                        grade = userResult.retData.study_card.grade,
                        card_type = userResult.retData.study_card.card_type,
                        card_auth = userResult.retData.study_card.card_auth,
                        card_price = userResult.retData.study_card.card_price,
                        card_name = userResult.retData.study_card.card_name,
                        card_setting = userResult.retData.study_card.card_setting
                    };
                GlobalUser.GRADEINFO = null;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("LoginApi Error", ex);
            }
        }

        private RelayCommand gridShowModel; //显示 grid 模式

        public RelayCommand GridShowModel
        {
            get
            {
                return gridShowModel ?? (gridShowModel = new RelayCommand(
                           action =>
                           {
                               ErrIsShow = Visibility.Collapsed;
                               switch (action.ToString())
                               {
                                   case "reset":
                                       LoginUcView = Visibility.Collapsed;
                                       RegUcView = Visibility.Collapsed;
                                       ResetUcView = Visibility.Visible;
                                       RePhoneCode = "";
                                       RePwd = "";
                                       break;
                                   case "reg":
                                       LoginUcView = Visibility.Collapsed;
                                       RegUcView = Visibility.Visible;
                                       ResetUcView = Visibility.Collapsed;
                                       NewPhoneNum = "";
                                       NewPhoneCode = "";
                                       NewPwd = "";
                                       break;
                                   default:
                                       LoginUcView = Visibility.Visible;
                                       RegUcView = Visibility.Collapsed;
                                       ResetUcView = Visibility.Collapsed;
                                       PhoneNum = GlobalUser.USER?.Mobile;
                                       Pwd = GlobalUser.USER?.Password;
                                       break;
                               }

                               //if (action == "reset")
                               //{
                               //    LoginUcView = Visibility.Collapsed;
                               //    RegUcView = Visibility.Collapsed;
                               //    ResetUcView = Visibility.Visible;
                               //}
                               //else if (action == "reg")
                               //{
                               //    LoginUcView = Visibility.Collapsed;
                               //    RegUcView = Visibility.Visible;
                               //    ResetUcView = Visibility.Collapsed;
                               //}
                               //else
                               //{
                               //    LoginUcView = Visibility.Visible;
                               //    RegUcView = Visibility.Collapsed;
                               //    ResetUcView = Visibility.Collapsed;
                               //}
                           }));
            }
        }


        private RelayCommand _RegIn; //注册

        public RelayCommand RegIn
        {
            get
            {
                return _RegIn ?? (_RegIn = new RelayCommand(action =>
                {
                    var param = action as List<object>;

                    var loginWin = param[4] as Window;

                    //验证手机号
                    NewPhoneNum = NewPhoneNum.ToDBC();
                    if (!RegexHelper.IsHandset(NewPhoneNum))
                    {
                        ErrIsShow = Visibility.Visible;
                        checkOK = false;
                        ErrMsg = "请输入有效手机号！";
                        return;
                    }

                    #region <<验证 输入 并联网注册>>

                    //验证 手机验证码
                    if (string.IsNullOrWhiteSpace(NewPhoneCode))
                    {
                        ErrIsShow = Visibility.Visible;
                        checkOK = false;
                        ErrMsg = "请输入短信验证码！";
                        return;
                    }

                    if (string.IsNullOrEmpty(NewPwd))
                    {
                        ErrIsShow = Visibility.Visible;
                        checkOK = false;
                        ErrMsg = "请输入登录密码！";
                        return;
                    }

                    NewPwd = NewPwd.ToDBC();
                    if (NewPwd.Length < 6 || NewPwd.Length > 20)
                    {
                        ErrIsShow = Visibility.Visible;
                        checkOK = false;
                        ErrMsg = "请确认密码在6~20位之间！";
                        return;
                    }

                    NewPhoneCode = NewPhoneCode.ToDBC();

                    //真实姓名 验证 长度验证
                    if (string.IsNullOrEmpty(NewTrueName) || NewTrueName.Length < 2 || NewTrueName.Length > 6)
                    {
                        ErrIsShow = Visibility.Visible;
                        checkOK = false;
                        ErrMsg = "请输入您的真实姓名！";
                        return;
                    }

                    // 联网注册账号 -- 密码
                    // 异步登录，防止界面假死
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        var lm1 = new UserPwdFindModel
                        {
                            phone = NewPhoneNum,
                            role = 1
                        };

                        var reqResult1 = WebProxy(lm1, ApiType.CheckUserPhone, null);

                        string retData1 = reqResult1?.retData.ToString();

                        if (reqResult1?.retCode == 0 && retData1.ToLower().Contains("check") && !retData1.Contains("1"))
                        {
                        }
                        else
                        {
                            checkOK = false;
                            ErrIsShow = Visibility.Visible;
                            ErrMsg = Settings.Default.UsedPhoneNum;
                            return;
                        }

                        var lm = new UserRegModel
                        {
                            phone = NewPhoneNum,
                            phone_code = NewPhoneCode,
                            password = NewPwd,
                            user_name = NewTrueName
                        };

                        var respResult = WebProxy(lm, ApiType.UserReg);

                        if (respResult != null)
                        {
                            if (respResult.retCode == 0)
                            {
                                GlobalUser.USER = new UserInfo();
                                checkOK = true;
                                ErrIsShow = Visibility.Hidden;
                                //ErrMsg = "注册完成,返回登录！";

                                var view = new MessageDialog();
                                view.DataContext = new MessageDialogVM
                                {
                                    MsgTitle = "提示消息",
                                    MsgContent = "注册成功，请绑定学习卡" //"注册成功，请登录吧!"
                                };

                                try
                                {
                                    GlobalUser.LoginWin.ShowDialog(view, MessageDialogClose);
                                }
                                catch (Exception e)
                                {
                                    DialogHost.Show(view, "LoginDialog", MessageDialogClose);

                                    Log4NetHelper.Error("注册 打开ShowDialog异常,使用Show", e); 
                                }

                                LoginUcView = Visibility.Visible;
                                RegUcView = Visibility.Collapsed;
                                ResetUcView = Visibility.Collapsed;

                                LoginApi(lm.password, respResult.retData.token.ToString());
                            }
                            else
                            {
                                ErrIsShow = Visibility.Visible;
                                checkOK = false;
                                ErrMsg = respResult.retMsg;
                            }
                        }
                        else
                        {
                            ErrIsShow = Visibility.Visible;
                            checkOK = false;
                            ErrMsg = "注册失败！";
                        }
                    }));

                    #endregion

                    //登录 成功
                    if (checkOK)
                    {
                        GlobalUser.LoginExpired = false;
                        LoggedIn(loginWin);
                    }
                }));
            }
        }

        private RelayCommand _RePwdIn; //修改 密码

        public RelayCommand RePwdIn
        {
            get
            {
                return _RePwdIn ?? (_RePwdIn = new RelayCommand(action =>
                {
                    var param = action as List<object>;

                    var loginWin = param[3] as Window;

                    //验证手机号
                    if (string.IsNullOrEmpty(ReUsername))
                    {
                        ErrIsShow = Visibility.Visible;
                        checkOK = false;
                        ErrMsg = "手机号不能为空！";
                        return;
                    }

                    ReUsername = ReUsername.ToDBC();
                    if (!RegexHelper.IsHandset(ReUsername))
                    {
                        ErrIsShow = Visibility.Visible;
                        checkOK = false;
                        ErrMsg = "请输入有效手机号！";
                        return;
                    }

                    var phoneCode = (param[1] as TextBox).Text;
                    if (string.IsNullOrWhiteSpace(phoneCode))
                    {
                        ErrIsShow = Visibility.Visible;
                        checkOK = false;
                        ErrMsg = "无效的验证码！";
                        return;
                    }

                    #region <<验证 手机验证码>>

                    //验证 手机验证码
                    if (param[2] is PasswordBox)
                    {
                        var pwd = (param[2] as PasswordBox).Password;
                        if (string.IsNullOrWhiteSpace(pwd))
                        {
                            ErrIsShow = Visibility.Visible;
                            checkOK = false;
                            ErrMsg = "请输入新密码！";
                            return;
                        }

                        pwd = pwd.ToDBC();
                        if (pwd.Length < 6 || pwd.Length > 20)
                        {
                            ErrIsShow = Visibility.Visible;
                            checkOK = false;
                            ErrMsg = "请确认密码在6~21位之间！";
                            return;
                        }

                        // 联网验证账号密码
                        if (!string.IsNullOrEmpty(pwd) && ReUsername == src.PhoneNum)
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                var lm = new UserPwdFindModel
                                {
                                    phone = ReUsername,
                                    password = pwd,
                                    phone_code = phoneCode
                                };

                                var respResult = WebProxy(lm, ApiType.UserPwdFind);
                                if (respResult != null && respResult.retCode == 0)
                                {
                                    if (respResult.retCode == 0)
                                    {
                                        GlobalUser.USER = new UserInfo();
                                        checkOK = true;
                                    }
                                }
                                else
                                {
                                    ErrIsShow = Visibility.Visible;
                                    checkOK = false;
                                    ErrMsg = respResult.retMsg;
                                }
                            }));
                        }
                        else
                        {
                            ErrIsShow = Visibility.Visible;
                            checkOK = false;
                            ErrMsg = "短信验证码错误！";
                            return;
                        }
                    }
                    else
                    {
                        ErrIsShow = Visibility.Visible;
                        checkOK = false;
                        ErrMsg = "请输入正确的短信验证码！";

                        return;
                    }

                    #endregion

                    //登录 成功
                    if (checkOK)
                    {
                        ErrIsShow = Visibility.Visible;
                        ErrMsg = "修改完成,请重新登录！";

                        LoginUcView = Visibility.Visible;
                        RegUcView = Visibility.Collapsed;
                        ResetUcView = Visibility.Collapsed;
                    }
                }));
            }
        }

        #endregion
    }
}
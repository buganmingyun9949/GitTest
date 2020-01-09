using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Framework.Logging;
using Framework.Network.HttpHelper;
using GalaSoft.MvvmLight.Command;
using ST.Common.Domain;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using Personal_App.Common;
using Personal_App.Domain;
using MaterialDesignThemes.Wpf;
using ST.Common;
using Card = ST.Models.Api.Card;

namespace Personal_App.ViewModel
{
    public class MainLoginVM : MainViewModel
    {
        #region 界面字段...

        private string phoneNum;//手机号
        private string identifyCode;//验证码
        private string imageCode;//图片验证码
        private Visibility errIsShow = Visibility.Hidden;//默认不显示 错误提示
        private string errMsg = "错误提示";//要显示错误提示
        private Visibility imageShow = Visibility.Collapsed;//图片验证窗口默认隐藏

        private int checkCount = 0;//每次运行登录,手机验证码验证次数,超过3次后 本地开启图片验证 

        private bool checkOK = false; //登录内容验证

        private object validCodeImg;//图片验证码

        private ValidCode validCode;

        private Visibility _LoginUcView;
        private Visibility _RegUcView;
        private Visibility _ResetUcView;

        /// <summary>
        /// 显示 登录
        /// </summary>
        public Visibility LoginUcView
        {
            get
            {
                return _LoginUcView;
            }

            set
            {
                _LoginUcView = value;
                RaisePropertyChanged("LoginUcView");
            }
        }

        /// <summary>
        /// 显示 注册
        /// </summary>
        public Visibility RegUcView
        {
            get
            {
                return _RegUcView;
            }

            set
            {
                _RegUcView = value;
                RaisePropertyChanged("RegUcView");
            }
        }

        /// <summary>
        /// 显示 重置密码
        /// </summary>
        public Visibility ResetUcView
        {
            get
            {
                return _ResetUcView;
            }

            set
            {
                _ResetUcView = value;
                RaisePropertyChanged("ResetUcView");
            }
        }

        public string PhoneNum
        {
            get
            {
                //if (string.IsNullOrWhiteSpace(phoneNum))
                //    return "";
                //else if (RegexHelper.IsNumber(phoneNum))
                //    return phoneNum;
                //else return null;

                return phoneNum;
            }
            set
            {
                phoneNum = value;
                RaisePropertyChanged("PhoneNum");
            }
        }

        public Visibility ErrIsShow
        {
            get
            {
                return errIsShow;
            }

            set
            {
                errIsShow = value;
                RaisePropertyChanged("ErrIsShow");
            }
        }

        public string ErrMsg
        {
            get
            {
                return errMsg;
            }

            set
            {
                errMsg = value;
                RaisePropertyChanged("ErrMsg");
            }
        }

        public string IdentifyCode
        {
            get
            {
                return identifyCode;
            }

            set
            {
                identifyCode = value;
                RaisePropertyChanged("IdentifyCode");
            }
        }

        public string ImageCode
        {
            get
            {
                return imageCode;
            }

            set
            {
                imageCode = value;
                RaisePropertyChanged("ImageCode");
            }
        }

        public Visibility ImageShow
        {
            get
            {
                return imageShow;
            }

            set
            {
                imageShow = value;
                RaisePropertyChanged("ImageShow");
            }
        }
        public object ValidCodeImg
        {
            get
            {
                return validCodeImg;
            }

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
            get
            {
                return _Pwd;
            }
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
            get
            {
                return _NewPhoneNum;
            }
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
            get
            {
                return _NewPhoneCode;
            }
            set
            {
                _NewPhoneCode = value;
                RaisePropertyChanged("NewPhoneCode");
            }
        }

        private string _NewPwd;

        public string NewPwd
        {
            get
            {
                return _NewPwd;
            }
            set
            {
                _NewPwd = value;
                RaisePropertyChanged("NewPwd");
            }
        }
        #endregion

        #region << 修改 密码 >>

        private string _ReUsername;

        public string ReUsername
        {
            get
            {
                return _ReUsername;
            }
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
            get
            {
                return _RePhoneCode;
            }
            set
            {
                _RePhoneCode = value;
                RaisePropertyChanged("RePhoneCode");
            }
        }

        private string _RePwd;

        public string RePwd
        {
            get
            {
                return _RePwd;
            }
            set
            {
                _RePwd = value;
                RaisePropertyChanged("RePwd");
            }
        }
        #endregion

        #endregion

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

        /// <summary>
        /// 检查记住的用户。
        /// </summary>
        private void CheckRememberUser()
        {
            try
            {
                string userDataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER, $"user.dat");

                if (File.Exists(userDataFile))
                {
                    using (StreamReader streamReader = new StreamReader(userDataFile))
                    {
                        string userEncoded = streamReader.ReadToEnd();
                        if (!String.IsNullOrEmpty(userEncoded))
                        {
                            string userData = Base64Provider.AESDecrypt(userEncoded, Base64Provider.KEY, Base64Provider.IV);
                            UserDataModel userDataModel = userData.FromJsonTo<UserDataModel>();
                            if (!String.IsNullOrEmpty(userDataModel?.Phone))
                            {
                                PhoneNum = userDataModel?.Phone;
                                if (!String.IsNullOrEmpty(userDataModel?.AccessToken))
                                {
                                    GlobalUser.USER = userDataModel?.Data;

                                    var windows = Application.Current.Windows;
                                    var loginWin = new Window();
                                    foreach (Window window in windows)
                                    {
                                        if (window.Name == "ml")
                                        {
                                            loginWin = window as Window;
                                        }
                                    }
                                    GlobalUser.AutoLoggedIn = true;
                                    checkOK = true;
                                    LoggedIn(loginWin);
                                }
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
        /// 记住用户。
        /// </summary>
        /// <param name="userName">用户名称。</param>
        /// <param name="phone">手机号码。</param>
        /// <param name="accessToken">访问令牌。</param>
        private new void RememberUser(string userName, string phone, string avatar, string accessToken)
        {
            string userFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER);
            string userDataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER, $"user.dat");
            UserDataModel userDataModel = new UserDataModel
            {
                UserName = userName,
                Phone = phone,
                Avatar = avatar,
                AccessToken = accessToken,
                Data = GlobalUser.USER
            };

            string userEncoded = Base64Provider.AESEncrypt(userDataModel.ToJsonItem(), Base64Provider.KEY, Base64Provider.IV);
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
        }

        #region 为获取验证码按钮设置内容...

        PhoneVerificationCode src = new PhoneVerificationCode();
        public PhoneVerificationCode Src
        {
            get { return src; }

            set
            {
                src = value;
                RaisePropertyChanged("Src");
            }
        }

        #endregion

        #region  << btn


        private RelayCommand<string> sendCode;//获取短信验证码

        public RelayCommand<string> SendCode
        {
            get
            {
                return sendCode ?? (sendCode = new RelayCommand<string>(
                           (cType) =>
                           {
                               if (cType == "new")
                               {
                                   PhoneNum = NewPhoneNum;

                                   //if (!string.IsNullOrEmpty(NewPhoneNum))
                                   //{
                                   //    ErrIsShow = Visibility.Visible;
                                   //    ErrMsg = "手机号不能为空！";
                                   //}

                                   //if (RegexHelper.IsHandset(ReUsername))
                                   //{
                                   //}
                               }
                               else if (cType == "old")
                               {
                                   PhoneNum = ReUsername;

                                   //if (!string.IsNullOrEmpty(ReUsername))
                                   //{
                                   //    ErrIsShow = Visibility.Visible;
                                   //    ErrMsg = "手机号不能为空！";
                                   //}

                                   //if (RegexHelper.IsHandset(ReUsername))
                                   //{
                                   //}
                               }


                               if (!string.IsNullOrEmpty(PhoneNum))
                               {
                                   PhoneNum = phoneNum.ToDBC();
                                   if (RegexHelper.IsHandset(PhoneNum))
                                   {
                                       src.GetCode(PhoneNum);

                                       WebApiProxy.SetMd5(phoneNum);
                                       SendCodeModel lm = new SendCodeModel()
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
                                                   ErrMsg = "与服务器通讯失败，请稍候重试！";
                                                   ErrMsg = result.retMsg;
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

        private RelayCommand validCodeImgbtn;//获取本地验证码

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

        private RelayCommand loginIn;//登录

        public RelayCommand LoginIn
        {
            get
            {
                return loginIn ?? (loginIn = new RelayCommand((action) =>
                {
                    var param = action as List<object>;

                    var loginWin = (param[2] as Window);


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
                        UserLoginModel lm = new UserLoginModel()
                        {
                            phone = PhoneNum,
                            password = Pwd
                        };

                        var respResult = WebProxy(lm, ApiType.UserLogin);
                        if (respResult != null)
                        {
                            if (respResult.retCode == 0)
                            {

                                var userResult = WebProxy(ApiType.UserInfo, respResult.retData.token.ToString());

                                GlobalUser.USER = new UserInfo()
                                {
                                    Token = respResult.retData.token,
                                    Expire_status = respResult.retData.expire_status
                                };

                                GlobalUser.USER.UserName = PhoneNum;
                                GlobalUser.USER.Moblie = PhoneNum;

                                if (userResult.retCode == 0)
                                {
                                    GlobalUser.USER.UserName = userResult.retData.user_name;
                                    GlobalUser.USER.Moblie = userResult.retData.user_phone;
                                    GlobalUser.USER.Avatar = userResult.retData.user_avatar;
                                }

                                checkOK = true;
                            }
                            else
                            {
                                ErrIsShow = Visibility.Visible;
                                checkOK = false;
                                ErrMsg = respResult.retMsg;
                                return;
                            }
                        }
                        else
                        {
                            ErrIsShow = Visibility.Visible;
                            checkOK = false;
                            ErrMsg = "登录失败！";

                            //MessageBox.Show("登录失败");
                            return;
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


        private RelayCommand gridShowModel;//显示 grid 模式

        public RelayCommand GridShowModel
        {
            get
            {
                return gridShowModel ?? (gridShowModel = new RelayCommand(
                           (action) =>
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



        private RelayCommand _RegIn;//注册

        public RelayCommand RegIn
        {
            get
            {
                return _RegIn ?? (_RegIn = new RelayCommand((action) =>
                {
                    var param = action as List<object>;

                    var loginWin = (param[2] as Window);


                    //验证手机号
                    if (string.IsNullOrEmpty(NewPhoneNum))
                    {
                        ErrIsShow = Visibility.Visible;
                        checkOK = false;
                        ErrMsg = "手机号不能为空！";
                        return;
                    }

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
                    if (!(NewPwd.Length > 5 && NewPwd.Length < 21))
                    {
                        ErrIsShow = Visibility.Visible;
                        checkOK = false;
                        ErrMsg = "请确认密码在6~20位之间！";
                        return;
                    }

                    NewPhoneCode = NewPhoneCode.ToDBC();

                    // 联网注册账号 -- 密码
                    // 异步登录，防止界面假死
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        UserRegModel lm = new UserRegModel()
                        {
                            phone = NewPhoneNum,
                            phone_code = NewPhoneCode,
                            password = NewPwd
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
                                view.DataContext = new MessageDialogVM()
                                {
                                    MsgTitle = "提示消息",
                                    MsgContent = "注册成功，请登录吧!",
                                };
                                DialogHostEx.ShowDialog(App.Current.MainWindow, view, MessageDialogClose);

                                LoginUcView = Visibility.Visible;
                                RegUcView = Visibility.Collapsed;
                                ResetUcView = Visibility.Collapsed;
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

                            //MessageBox.Show("验证失败");
                            return;
                        }
                    }));

                    #endregion

                    //登录 成功
                    //if (checkOK)
                    //{
                    //    GlobalUser.LoginExpired = false;
                    //    LoggedIn(loginWin);
                    //}
                }));
            }
        }

        private RelayCommand _RePwdIn;//修改 密码

        public RelayCommand RePwdIn
        {
            get
            {
                return _RePwdIn ?? (_RePwdIn = new RelayCommand((action) =>
                {
                    var param = action as List<object>;

                    var loginWin = (param[3] as Window);


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
                        if (!(pwd.Length > 5 && pwd.Length < 21))
                        {
                            ErrIsShow = Visibility.Visible;
                            checkOK = false;
                            ErrMsg = "请确认密码在6~21位之间！";
                            return;
                        }

                        // 联网验证账号密码
                        if (!string.IsNullOrEmpty(pwd) && ReUsername == src.PhoneNum)
                        {
                            // 异步登录，防止界面假死
                            //Task.Run(() =>
                            //{
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                UserPwdFindModel lm = new UserPwdFindModel()
                                {
                                    phone = ReUsername,
                                    password = pwd,
                                    phone_code = phoneCode,
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

                                    //checkCount++;
                                    //if (checkCount > 2)
                                    //{
                                    //    ImageShow = Visibility.Visible;
                                    //    loginWin.Height = 700;
                                    //}

                                    //MessageBox.Show("验证失败");
                                    return;
                                }
                            }));
                            //});
                        }
                        else
                        {
                            ErrIsShow = Visibility.Visible;
                            checkOK = false;
                            ErrMsg = "短信验证码错误！";

                            checkCount++;
                            if (checkCount > 2)
                            {
                                ImageShow = Visibility.Visible;
                                loginWin.Height = 700;
                            }
                            //MessageBox.Show("验证失败");
                            return;
                        }
                    }
                    else
                    {
                        ErrIsShow = Visibility.Visible;
                        checkOK = false;
                        ErrMsg = "请输入正确的短信验证码！";

                        //checkCount++;
                        //if (checkCount > 2)
                        //{
                        //    ImageShow = Visibility.Visible;
                        //    loginWin.Height = 700;
                        //}

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

                        //GlobalUser.LoginExpired = false;
                        //LoggedIn(loginWin);
                    }
                }));
            }
        }
        #endregion


        /// <summary>
        /// 登录成功。
        /// </summary>
        /// <param name="loginWin">登录窗口。</param>
        private void LoggedIn(Window loginWin)
        {
            if (!GlobalUser.AutoLoggedIn)
            {
                RememberUser(GlobalUser.USER.UserName, GlobalUser.USER.Moblie, GlobalUser.USER.Avatar,
                    GlobalUser.USER.Token);
            }
            ErrIsShow = Visibility.Hidden;

            //ErrMsg = "请输入有效的手机号！";
            //MessageBox.Show("验证成功");
            loginWin.Hide();

            GetUserCard();

            if (GlobalUser.MainWin == null)
            {
                GlobalUser.MainWin = new MainWindow();
            }
            if (!GlobalUser.LoginExpired)
            {
                GlobalUser.MainWin.Show();
                GlobalUser.MainWin.Focus();
            }
            else
            {
                GlobalUser.MainWin.Hide();
                GlobalUser.MainWin.Close();
                //loginWin.Show();
                //loginWin.Focus();
            }
        }



        /// <summary>
        /// 获取用户学习卡。
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

                if (cardResult.retData == null)
                {
                    return;
                }

                GlobalUser.USER.CardsList = JsonHelper.FromJson<List<Card>>(cardResult.retData.ToString());//抓取我卡片

                // 从学习卡列表获取默认卡，默认取第一个。
                var defaultCard =
                    GlobalUser.USER.CardsList.FirstOrDefault(c => c.is_current == 1);

                if (defaultCard != null)
                {
                    //ST.Models.Api.Card card = new ST.Models.Api.Card
                    //{
                    //    CardName = defaultCard.CardName,
                    //    card_id = defaultCard.card_id,
                    //    used_time = defaultCard.used_time,
                    //    expire_time = defaultCard.expire_time
                    //};
                    ////判断学习卡是否过期
                    //var validityEnd = Convert.ToDateTime(defaultCard.expire_time);
                    //var currentDate = DateTime.Now.Date;
                    //if (DateTime.Compare(validityEnd, currentDate) > 0)
                    //    card.Status = 1;
                    //else
                    //    card.Status = 2;
                    GlobalUser.USER.Card = defaultCard;
                    GlobalUser.USER.Expire_status = defaultCard.expire_status;
                }
            }));

            #endregion
        }

        /// <summary>
        /// 关闭程序。
        /// </summary>
        public ICommand CloseLoginWindowCommand => new RelayCommand(LoginWindowClose);


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


    }
}

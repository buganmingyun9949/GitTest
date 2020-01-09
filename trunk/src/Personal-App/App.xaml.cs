using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Framework.Logging;
using Lierda.WPFHelper;
using Personal_App.Common;
using MaterialDesignThemes.Wpf;
using Personal_App.ViewModel;
using ST.Common;
using ST.Common.Domain;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;

namespace Personal_App
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private Mutex mutex;
        LierdaCracker cracker = new LierdaCracker();

        protected override void OnStartup(StartupEventArgs e)
        {
            //cracker.Cracker(120);

            base.OnStartup(e);

            //Common.SysHook.CloseScreenSave();
            Log4NetHelper.InitLog();
            //AppConfig.GlobalConfig.Ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //AppConfig.Save(AppConfig.GlobalConfig);

            //AppConfig.ReLoadTea(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

            Current.DispatcherUnhandledException += App_OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            GlobalUser.VER = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //if (!IsAdministrator())
            //{
            //    MessageBox.Show("请使用管理员权限打开本程序", "启动提示", MessageBoxButton.OK, MessageBoxImage.Error);
            //    Shutdown();
            //}

            if (!IaCreateNew())
            {
                MessageBox.Show("程序已经启动", "启动提示", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }

            CheckAppDefaultPath();
            
            Task.Factory.StartNew(() => {
                GlobalUser.MACHINEINFO.GetDevice();
            });
            
            var viewWin = new MainLogin();
            GlobalUser.LoginWin = viewWin;
            viewWin.DataContext = new MainLoginVM();
            if (!GlobalUser.AutoLoggedIn)
            {
                viewWin.Show();
            }
            else
            {
                viewWin.Hide();
            }

            DisableStandby();

            //var win = new TestWin();
            //win.Show();

            //MessageBox.Show(GlobalUser.MACHINEINFO.GetCPUName());
        }

        private const uint ES_SYSTEM_REQUIRED = 0x00000001;
        private const uint ES_DISPLAY_REQUIRED = 0x00000002;
        private const uint ES_CONTINUOUS = 0x80000000;

        /// <summary>
        /// 控制系统休眠。
        /// </summary>
        /// <param name="esFlags"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern uint SetThreadExecutionState(uint esFlags);

        /// <summary>
        /// 禁止休眠和睡眠。
        /// </summary>
        public static void DisableStandby()
        {
            SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);
        }

        /// <summary>
        /// 允许睡醒和休眠。
        /// </summary>
        public static void EnableStandby()
        {
            SetThreadExecutionState(ES_CONTINUOUS);
        }

        /// <summary>
        /// 检查默认默认目录
        /// </summary>
        private void CheckAppDefaultPath()
        {
            if (!Directory.Exists(GlobalUser.AUDIODATAFOLDER))
                Directory.CreateDirectory(GlobalUser.AUDIODATAFOLDER);

            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER)))
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER));

        }


        protected override void OnExit(ExitEventArgs e)
        {
            EnableStandby();
        }

        public bool IaCreateNew()
        {

            string targetExeName = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string productName = System.IO.Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().GetName().Name);

            mutex = new Mutex(true, productName, out bool createNew);

            return createNew;

        }

        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// UI 线程抛出全局异常事件处理。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                Log4NetHelper.Error("UI 线程全局异常", e.Exception);
                UpErr(e.Exception);
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("不可恢复的 UI 线程全局异常", ex);
                UpErr(ex);
                MessageBox.Show("应用程序发生不可恢复的异常，将要退出！");
            }
        }

        /// <summary>
        /// 非 UI 线程抛出全局异常事件处理。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                if (e.ExceptionObject is Exception exception)
                {
                    Log4NetHelper.Error("非 UI 线程全局异常", exception);
                    UpErr(exception);
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("不可恢复的非 UI 线程全局异常", ex);
                UpErr(ex);
                MessageBox.Show("应用程序发生不可恢复的异常，将要退出！");
            }
        }

        private void UpErr(Exception e)
        {
            ErrMsgLog msgInfo = new ErrMsgLog();
            msgInfo.user = GlobalUser.USER.Mobile;
            msgInfo.msginfo = "系统错误";
            msgInfo.error = e;
            msgInfo.isErr = true;
            Log_Data log = new Log_Data();
            log.log_desc = Log_Type.PC_Error .ToString();
            log.log_text = msgInfo.ToJson();
            log.log_device = JsonHelper.ToJson(GlobalUser.MACHINEINFO.GetDevice());
            WebApiProxy.GetHtmlRespInfo(log, ApiType.SysLog, null, "Post");
        }
    }
}

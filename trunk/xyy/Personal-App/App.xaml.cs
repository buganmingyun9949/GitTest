using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Framework.Logging;
using Personal_App.Common;
using MaterialDesignThemes.Wpf;
using ST.Common;

namespace Personal_App
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private Mutex mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Common.SysHook.CloseScreenSave();
            Log4NetHelper.InitLog();
            //AppConfig.GlobalConfig.Ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //AppConfig.Save(AppConfig.GlobalConfig);

            //AppConfig.ReLoadTea(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

            Current.DispatcherUnhandledException += App_OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;


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

            var viewWin = new MainLogin();
            if (!GlobalUser.AutoLoggedIn)
            {
                viewWin.Show();
            }
            else
            {
                viewWin.Hide();
            }
            DisableStandby(); 
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
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("不可恢复的 UI 线程全局异常", ex);
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
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("不可恢复的非 UI 线程全局异常", ex);
                MessageBox.Show("应用程序发生不可恢复的异常，将要退出！");
            }
        }
    }
}

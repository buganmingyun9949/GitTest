using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Personal_App.Common
{
    public class ChangeWindowSize
    {
        /// <summary>
        /// 边框宽度
        /// </summary>
        private readonly int Thickness = 4;

        /// <summary>
        /// 改变大小的通知消息
        /// </summary>
        private const int WMNCHITTEST = 0x0084;

        /// <summary>
        /// 窗口的大小和位置将要被改变时的消息
        /// </summary>
        private const int WMWINDOWPOSCHANGING = 0x0046;

        /// <summary>
        /// 拐角宽度
        /// </summary>
        private readonly int angelWidth = 12;

        /// <summary>
        /// 要改变窗体大小的对象
        /// </summary>
        private Window window = null;

        /// <summary>
        /// 鼠标坐标
        /// </summary>
        private Point mousePoint = new Point();

        /// <summary>
        /// 构造函数，初始化目标窗体对象
        /// </summary>
        /// <param name="window">目标窗体</param>
        public ChangeWindowSize(Window window)
        {
            this.window = window;
        }

        /// <summary>
        /// 进行注册钩子
        /// </summary>
        public void RegisterHook()
        {
            HwndSource hwndSource = PresentationSource.FromVisual(this.window) as HwndSource;
            if (hwndSource != null)
            {
                hwndSource.AddHook(new HwndSourceHook(this.WndProc));
            }
        }

        /// <summary>
        /// 窗体回调程序
        /// </summary>
        /// <param name="hwnd">窗体句柄</param>
        /// <param name="msg">消息</param>
        /// <param name="wideParam">附加参数1</param>
        /// <param name="longParam">附加参数2</param>
        /// <param name="handled">是否处理</param>
        /// <returns>返回句柄</returns>
        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wideParam, IntPtr longParam, ref bool handled)
        {
            // 获得窗体的 样式
            int oldstyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_STYLE);
            switch (msg)
            {
                case WMNCHITTEST:
                    this.mousePoint.X = longParam.ToInt32() & 0xFFFF;
                    this.mousePoint.Y = longParam.ToInt32() >> 16;
                    // 更改窗体的样式为无边框窗体
                    NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_STYLE, oldstyle & ~NativeMethods.WS_CAPTION);
                    return new IntPtr((int)HitTest.HTCAPTION);
                //}

                case WMWINDOWPOSCHANGING:

                    // 在将要改变的时候，是样式添加系统菜单
                    NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_STYLE, oldstyle & ~NativeMethods.WS_CAPTION | NativeMethods.WS_SYSMENU);
                    break;
            }

            return IntPtr.Zero;
        }
    }

    /// <summary>
    /// 主窗体内部类
    /// </summary>
    public class NativeMethods
    {
        /// <summary>
        /// 带有外边框和标题的windows的样式
        /// </summary>
        public const int WS_CAPTION = 0X00C0000;

        /// <summary>
        /// 系统菜单
        /// </summary>
        public const int WS_SYSMENU = 0x00080000;

        /// <summary>
        /// window 扩展样式 分层显示
        /// </summary>
        public const int WS_EX_LAYERED = 0x00080000;

        /// <summary>
        /// 带有alpha的样式
        /// </summary>
        public const int LWA_ALPHA = 0x00000002;

        /// <summary>
        /// 颜色设置
        /// </summary>
        public const int LWA_COLORKEY = 0x00000001;

        /// <summary>
        /// window的基本样式
        /// </summary>
        public const int GWL_STYLE = -16;

        /// <summary>
        /// window的扩展样式
        /// </summary>
        public const int GWL_EXSTYLE = -20;

        /// <summary>
        /// 设置窗体的样式
        /// </summary>
        /// <param name="handle">操作窗体的句柄</param>
        /// <param name="oldStyle">进行设置窗体的样式类型.</param>
        /// <param name="newStyle">新样式</param>
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern void SetWindowLong(IntPtr handle, int oldStyle, int newStyle);

        /// <summary>
        /// 获取窗体指定的样式.
        /// </summary>
        /// <param name="handle">操作窗体的句柄</param>
        /// <param name="style">要进行返回的样式</param>
        /// <returns>当前window的样式</returns>
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern int GetWindowLong(IntPtr handle, int style);

        /// <summary>
        /// 设置窗体的工作区域.
        /// </summary>
        /// <param name="handle">操作窗体的句柄.</param>
        /// <param name="handleRegion">操作窗体区域的句柄.</param>
        /// <param name="regraw">if set to <c>true</c> [regraw].</param>
        /// <returns>返回值</returns>
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern int SetWindowRgn(IntPtr handle, IntPtr handleRegion, bool regraw);

        /// <summary>
        /// 创建带有圆角的区域.
        /// </summary>
        /// <param name="x1">左上角坐标的X值.</param>
        /// <param name="y1">左上角坐标的Y值.</param>
        /// <param name="x2">右下角坐标的X值.</param>
        /// <param name="y2">右下角坐标的Y值.</param>
        /// <param name="width">圆角椭圆的 width.</param>
        /// <param name="height">圆角椭圆的 height.</param>
        /// <returns>hRgn的句柄</returns>
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int width, int height);

        /// <summary>
        /// Sets the layered window attributes.
        /// </summary>
        /// <param name="handle">要进行操作的窗口句柄</param>
        /// <param name="colorKey">RGB的值</param>
        /// <param name="alpha">Alpha的值，透明度</param>
        /// <param name="flags">附带参数</param>
        /// <returns>true or false</returns>
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr handle, uint colorKey, byte alpha, int flags);
    }

    /// <summary>
    /// 枚举测试命中
    /// </summary>
    public enum HitTest : int
    {
        /// <summary>
        /// 错误
        /// </summary>
        HTERROR = -2,

        /// <summary>
        /// 透明
        /// </summary>
        HTTRANSPARENT = -1,

        /// <summary>
        /// 任意位置
        /// </summary>
        HTNOWHERE = 0,

        /// <summary>
        /// 客户端
        /// </summary>
        HTCLIENT = 1,

        /// <summary>
        /// 标题
        /// </summary>
        HTCAPTION = 2,

        /// <summary>
        /// 系统菜单
        /// </summary>
        HTSYSMENU = 3,

        /// <summary>
        /// GroupBOx
        /// </summary>
        HTGROWBOX = 4,

        /// <summary>
        /// GroupBox的大小
        /// </summary>
        HTSIZE = HTGROWBOX,

        /// <summary>
        /// 菜单
        /// </summary>
        HTMENU = 5,

        /// <summary>
        /// 水平滚动条
        /// </summary>
        HTHSCROLL = 6,

        /// <summary>
        /// 垂直滚动条
        /// </summary>
        HTVSCROLL = 7,

        /// <summary>
        /// 最小化按钮
        /// </summary>
        HTMINBUTTON = 8,

        /// <summary>
        /// 最大化按钮
        /// </summary>
        HTMAXBUTTON = 9,

        /// <summary>
        /// 窗体左边
        /// </summary>
        HTLEFT = 10,

        /// <summary>
        /// 窗体右边
        /// </summary>
        HTRIGHT = 11,

        /// <summary>
        /// 窗体顶部
        /// </summary>
        HTTOP = 12,

        /// <summary>
        /// 窗体左上角
        /// </summary>
        HTTOPLEFT = 13,

        /// <summary>
        /// 窗体右上角
        /// </summary>
        HTTOPRIGHT = 14,

        /// <summary>
        /// 窗体底部
        /// </summary>
        HTBOTTOM = 15,

        /// <summary>
        /// 窗体左下角
        /// </summary>
        HTBOTTOMLEFT = 16,

        /// <summary>
        /// 窗体右下角
        /// </summary>
        HTBOTTOMRIGHT = 17,

        /// <summary>
        /// 窗体边框
        /// </summary>
        HTBORDER = 18,

        /// <summary>
        /// 窗体缩小
        /// </summary>
        HTREDUCE = HTMINBUTTON,

        /// <summary>
        /// 窗体填出
        /// </summary>
        HTZOOM = HTMAXBUTTON,

        /// <summary>
        /// 开始改变大小
        /// </summary>
        HTSIZEFIRST = HTLEFT,

        /// <summary>
        /// 结束改变大小
        /// </summary>
        HTSIZELAST = HTBOTTOMRIGHT,

        /// <summary>
        /// 对象
        /// </summary>
        HTOBJECT = 19,

        /// <summary>
        /// 关闭
        /// </summary>
        HTCLOSE = 20,

        /// <summary>
        /// 帮助
        /// </summary>
        HTHELP = 21,
    }
}

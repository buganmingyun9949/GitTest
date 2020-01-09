using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Framework.Logging;
using Personal_App.ViewModel;
using ST.Common;
using ST.Common.ToolsHelper;
using Path = System.IO.Path;

namespace Personal_App
{
    /// <summary>
    /// LoadingOperationPromptPage.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingOperationPromptPage : UserControl
    {
        public LoadingOperationPromptPage()
        {
            InitializeComponent();
        }

        private void LoadingOperationPromptPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            op1.Visibility = Visibility.Visible;
            op2.Visibility = Visibility.Hidden;
            op3.Visibility = Visibility.Hidden;
        }


        #region 加载圆圈的margin
        [Description("加载圆圈的margin"), CategoryAttribute("扩展"), DefaultValueAttribute(0)]
        public string LoadCirclesMargin
        {
            get { return (string)GetValue(LoadCirclesMarginProperty); }
            set { SetValue(LoadCirclesMarginProperty, value); }
        }


        public static readonly DependencyProperty LoadCirclesMarginProperty =
            DependencyProperty.Register("LoadCirclesMargin", typeof(string), typeof(LoadingOperationPromptPage),
            new FrameworkPropertyMetadata("50"));
        #endregion

        #region 加载中的提示
        [DescriptionAttribute("加载中的提示"), CategoryAttribute("扩展"), DefaultValueAttribute(0)]
        public string LoadingText
        {
            get { return (string)GetValue(LoadingTextProperty); }
            set { SetValue(LoadingTextProperty, value); }
        }


        public static readonly DependencyProperty LoadingTextProperty =
            DependencyProperty.Register("LoadingText", typeof(string), typeof(LoadingOperationPromptPage),
            new FrameworkPropertyMetadata("加载中"));
        #endregion

        #region 加载中的提示的字体大小
        [DescriptionAttribute("加载中的提示的字体大小"), CategoryAttribute("扩展"), DefaultValueAttribute(0)]
        public int LoadingTextFontSize
        {
            get { return (int)GetValue(LoadingTextFontSizeProperty); }
            set { SetValue(LoadingTextFontSizeProperty, value); }
        }


        public static readonly DependencyProperty LoadingTextFontSizeProperty =
            DependencyProperty.Register("LoadingTextFontSize", typeof(int), typeof(LoadingOperationPromptPage),
            new FrameworkPropertyMetadata(12));
        #endregion

        #region 圆圈的颜色
        [DescriptionAttribute("圆圈的颜色"), CategoryAttribute("扩展"), DefaultValueAttribute(0)]
        public Brush CirclesBrush
        {
            get { return (Brush)GetValue(CirclesBrushProperty); }
            set { SetValue(CirclesBrushProperty, value); }
        }


        public static readonly DependencyProperty CirclesBrushProperty =
            DependencyProperty.Register("CirclesBrush", typeof(Brush), typeof(LoadingOperationPromptPage),
            new FrameworkPropertyMetadata(Brushes.Black));
        #endregion

        #region 加载中的提示的字体颜色
        [DescriptionAttribute("加载中的提示的字体颜色"), CategoryAttribute("扩展"), DefaultValueAttribute(0)]
        public Brush LoadingTextForeground
        {
            get { return (Brush)GetValue(LoadingTextForegroundProperty); }
            set { SetValue(LoadingTextForegroundProperty, value); }
        }


        public static readonly DependencyProperty LoadingTextForegroundProperty =
            DependencyProperty.Register("LoadingTextForeground", typeof(Brush), typeof(LoadingOperationPromptPage),
            new FrameworkPropertyMetadata(Brushes.DarkSlateGray));
        #endregion

        #region << 下一步  按钮 单击  >>

        private void Button1_OnClick(object sender, RoutedEventArgs e)
        {
            op1.Visibility = Visibility.Hidden;
            op2.Visibility = Visibility.Visible;
            op3.Visibility = Visibility.Hidden;
        }

        private void Button2_OnClick(object sender, RoutedEventArgs e)
        {
            op1.Visibility = Visibility.Hidden;
            op2.Visibility = Visibility.Hidden;
            op3.Visibility = Visibility.Visible;
        }

        private void Button3_OnClick(object sender, RoutedEventArgs e)
        {
            op1.Visibility = Visibility.Hidden;
            op2.Visibility = Visibility.Hidden;
            op3.Visibility = Visibility.Hidden;

            this.Visibility = Visibility.Collapsed;

            if (GlobalUser.USER != null)
            {
                GlobalUser.USER.UnFirstOpen = true;
                RememberUser();
            }
        }

        #endregion


        public void RememberUser()
        {
            string userFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER);
            string userDataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalUser.DATAFOLDER, $"user.dat");
            UserDataModel userDataModel = new UserDataModel
            {
                UserName = GlobalUser.USER.UserName,
                Phone = GlobalUser.USER.Mobile,
                Avatar = GlobalUser.USER.Avatar,
                AccessToken = GlobalUser.USER.Token,
                Password = GlobalUser.USER.Password,
                Data = GlobalUser.USER,
                ClassData = GlobalUser.CLASSINFO,
                UserZy = GlobalUser.USER.UserZy,
                StudyCard = GlobalUser.STUDYCARD,
                UnFirstOpen =GlobalUser.USER.UnFirstOpen,
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
                    file.Dispose();
                }
                using (StreamWriter sw = new StreamWriter(userDataFile, false))
                {
                    sw.Write(userEncoded);
                    sw.Flush();
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("写入用户信息异常：", e);
            }
        }

    }
}

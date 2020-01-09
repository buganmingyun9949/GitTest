
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Framework.Logging;
using Microsoft.Win32;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Models.Api;
using ST.Style.ControlEx;
using TextBox = System.Windows.Controls.TextBox;

namespace Personal_App.ViewModel
{
    public class CustomerServiceDialogVM : MainViewModel
    {
        public const string ViewName = "CustomerServiceDialogVM";

        public CustomerServiceDialogVM()
        {
            try
            {
                var kefuInfo = JsonHelper.FromJsonTo<KeFu>(GlobalUser.AGENTINFO.kefu.ToString());

                KeFuPhone = kefuInfo.phone;
                KeFuQQ = kefuInfo.qq;
                KeFuWorktime = kefuInfo.worktime;
            }
            catch (Exception e)
            {
                Log4NetHelper.Error("未知的客服信息", e);
            }
        }

        #region << 属性 字段 >>

        private List<string> imgStrs = new List<string>();
        
        /// <summary>
        /// 最小上传文本长度 5
        /// </summary>
        private int MiniReportTextLength = 5;
        /// <summary>
        /// 最大上传文本长度 10000
        /// </summary>
        private int MaxReportTextLength = 10000;

        /// <summary>
        /// 图片上传最大数量
        /// </summary>
        private static int MaxCount = 3;

        /// <summary>
        /// 图片上传最大数量
        /// 2M
        /// </summary>
        private static int MaxSize = 2 * 1024 * 1024;

        private WrapPanel _ImgPanel;


        private string _KeFuPhone;

        public string KeFuPhone
        {
            get { return _KeFuPhone; }
            set
            {
                _KeFuPhone = value;
                RaisePropertyChanged("KeFuPhone");
            }
        }

        private string _KeFuQQ;

        public string KeFuQQ
        {
            get { return _KeFuQQ; }
            set
            {
                _KeFuQQ = value;
                RaisePropertyChanged("KeFuQQ");
            }
        }

        private string _KeFuWorktime;

        public string KeFuWorktime
        {
            get { return _KeFuWorktime; }
            set
            {
                _KeFuWorktime = value;
                RaisePropertyChanged("KeFuWorktime");
            }
        }

        private string _FeedBackMsg;

        public string FeedBackMsg
        {
            get { return _FeedBackMsg; }
            set
            {
                _FeedBackMsg = value;
                SubmitBtn = false;
                if (_FeedBackMsg.Trim().Length >= MiniReportTextLength && _FeedBackMsg.Trim().Length <= MaxReportTextLength)
                {
                    SubmitBtn = true;
                }

                RaisePropertyChanged("FeedBackMsg");
            }
        }

        private string _ErrMsgOut;

        public string ErrMsgOut
        {
            get { return _ErrMsgOut; }
            set
            {
                _ErrMsgOut = value;
                RaisePropertyChanged("ErrMsgOut");
            }
        }


        private bool _SubmitBtn;

        public bool SubmitBtn
        {
            get { return _SubmitBtn; }
            set
            {
                _SubmitBtn = value;
                RaisePropertyChanged("SubmitBtn");
            }
        }

        #endregion

        #region << Btn Command >>

        private RelayCommand _BtnAddImg;//添加 图片

        public RelayCommand BtnAddImg
        {
            get
            {
                return _BtnAddImg ?? (_BtnAddImg = new RelayCommand(
                           (action) =>
                           {
                               _ImgPanel = action as WrapPanel;
                               UpLoadImg(action as WrapPanel);
                           }));
            }
        }

        private RelayCommand _CommitReportCMD;//上传 意见反馈

        public RelayCommand CommitReportCMD
        {
            get
            {
                return _CommitReportCMD ?? (_CommitReportCMD = new RelayCommand(
                           (action) =>
                           {
                               var param = action as List<object>;
                               if (param.Count != 2) return;

                               if ((param[0] as TextBox).Text.Trim().Length < MiniReportTextLength ||
                                   (param[0] as TextBox).Text.Trim().Length > MaxReportTextLength) return;

                               if (Report((param[0] as TextBox).Text))
                               {
                                   //todo
                                   //执行退出按钮
                                   ButtonAutomationPeer peer =
                                       new ButtonAutomationPeer(param[1] as Button);

                                   IInvokeProvider invokeProv =
                                       peer.GetPattern(PatternInterface.Invoke)
                                           as IInvokeProvider;

                                   invokeProv.Invoke();
                               }


                           }));
            }
        }

        #endregion

        #region << 自定义方法 >>

        private void UpLoadImg(WrapPanel panel)
        {

            if (imgStrs?.Count >= 3)
            {
                ErrMsgOut = "上传截图不多于3张";
                return;
            }

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "选择一个图片";
            openFile.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.gif";
            openFile.FileName = string.Empty;
            openFile.FilterIndex = 1;
            openFile.RestoreDirectory = true;
            openFile.DefaultExt = "jpg";

            if (openFile.ShowDialog() == true)
            {
                if (File.ReadAllBytes(openFile.FileName).Length > MaxSize)
                {
                    ErrMsgOut = "上传的图片，大小不能超过2M";

                    return;
                }

                //Feedback
                string FileDirPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Feedback");

                ///2019-01-07  修改  上传的图片 保存到 系统temp目录
                if (!Directory.Exists(FileDirPath))
                    Directory.CreateDirectory(FileDirPath);
                string newPath = System.IO.Path.Combine(GlobalUser.AUDIODATAFOLDER,
                    $"temp_{Guid.NewGuid().ToString()}{System.IO.Path.GetExtension(openFile.FileName)}");

                File.Copy(openFile.FileName, newPath);

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.UriSource = new Uri(newPath);
                bi.EndInit();

                var reqResult = WebProxy(newPath, ApiType.ModifyUserHead,
                    GlobalUser.USER.Token);

                if (reqResult?.retCode == 0)
                {
                    if (imgStrs == null) imgStrs = new List<string>();
                    imgStrs.Add(reqResult.retData.static1.ToString());
                    //reqResult.retData.static1;
                    ImageButton img = new ImageButton();
                    img.IconHeight = img.IconWidth = 48;
                    img.Margin = new Thickness(0, 0, 10, 0);
                    img.Icon = bi;
                    img.IconStretch = Stretch.Fill;
                    img.Click += Img_Click;
                    img.CommandParameter = reqResult.retData.static1.ToString();
                    _ImgPanel.Children.Add(img);
                    //_ImgPanel.Background = new SolidColorBrush(Colors.BlueViolet);
                }
            }
        }

        private void Img_Click(object sender, RoutedEventArgs e)
        {
            var btn = (ImageButton) sender;

            if (imgStrs?.Count > 0 || btn.CommandParameter != null)
            {
                imgStrs.Remove(btn.CommandParameter.ToString());
            }

            ErrMsgOut = "";

            _ImgPanel.Children.Remove(btn);
        }

        private bool Report(string value0)
        {
            UserReport model = new UserReport()
            {
                report_device_info = JsonHelper.ToJson(GlobalUser.MACHINEINFO.GetDevice()),
                report_category = "意见反馈",
                report_content = value0,
                report_imgs = string.Join(",", imgStrs.ToArray()),
                report_product = 1003
            };

            var reqResult = WebProxy(model, ApiType.UserReport,
                GlobalUser.USER.Token);

            if (reqResult.retCode == 0)
            {
                ErrMsgOut = reqResult.retMsg;
                return true;
            }

            ErrMsgOut = reqResult.retMsg;

            return false;
        }

        #endregion
    }
}

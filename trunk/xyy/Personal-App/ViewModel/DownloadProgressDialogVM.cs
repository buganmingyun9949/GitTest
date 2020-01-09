using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
namespace Personal_App.ViewModel
{
    /// <summary>
    /// 下载进度对话框视图模型。
    /// </summary>
    public class DownloadProgressDialogVM : ViewModelBase
    {

        private Visibility _downloadingSpeedVisibility;

        /// <summary>
        /// 下载进度显示。
        /// </summary>
        public Visibility DownloadingSpeedVisibility
        {
            get => _downloadingSpeedVisibility;
            set
            {
                _downloadingSpeedVisibility = value;
                RaisePropertyChanged("DownloadingSpeedVisibility");
            }
        }

        private double _downloadingSpeed;

        /// <summary>
        /// 下载进度。
        /// </summary>
        public double DownloadingSpeed
        {
            get => _downloadingSpeed;
            set
            {
                _downloadingSpeed = value;
                RaisePropertyChanged("DownloadingSpeed");
            }
        }

        private string _downloadingSpeedText;

        /// <summary>
        /// 下载进度文本（正在下载试题，已完成 100%。）。
        /// </summary>
        public string DownloadingSpeedText
        {
            get => _downloadingSpeedText;
            set
            {
                _downloadingSpeedText = value;
                RaisePropertyChanged("DownloadingSpeedText");
            }
        }

    }
}

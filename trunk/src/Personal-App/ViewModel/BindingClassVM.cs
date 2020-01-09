
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using Personal_App.Common;
using Personal_App.Domain;
using MaterialDesignThemes.Wpf;
using Personal_App.ViewModel.Menu;
using ST.Common;
using Card = ST.Models.Api.Card;

namespace Personal_App.ViewModel
{
    public class BindingClassVM : MainViewModel, IView
    {
        public void Activated(object state)
        {
            throw new NotImplementedException();
        }

        public const string ViewName = "BindingClass";

        private bool BandingOK = false;

        public BindingClassVM()
        {
            ShowImg = Visibility.Visible;
            ShowText = Visibility.Collapsed;

            TxtTeacherPhone = "";

            if (string.IsNullOrEmpty(GlobalUser.CLASSINFO?.Class_name))
            {
                ShowBackZYBtn = Visibility.Collapsed;
            }
            else
            {
                ShowBackZYBtn = Visibility.Visible;
            }
        }

        public BindingClassVM(bool isMain = false)
        {
            if (isMain)
            {
                ShowImg = Visibility.Collapsed;
                ShowText = Visibility.Visible;
            }
            else
            {
                ShowImg = Visibility.Visible;
                ShowText = Visibility.Collapsed;
            }

            TxtTeacherPhone = "";

            if (string.IsNullOrEmpty(GlobalUser.CLASSINFO?.Class_name))
            {
                ShowBackZYBtn = Visibility.Collapsed;
            }
            else
            {
                ShowBackZYBtn = Visibility.Visible;
            }
        }

        #region << 属性 字段 >>


        private string _txtCardNo;
        private string _errMsgOut;
        private string _txtTeacherPhone;

        public string TxtCardNo
        {
            get
            {
                return _txtCardNo;
            }
            set
            {
                _txtCardNo = value;
                if (string.IsNullOrEmpty(_txtCardNo))
                {
                    ErrMsgOut = "";
                }
                RaisePropertyChanged("TxtCardNo");
            }
        }

        public string ErrMsgOut
        {
            get
            {
                return _errMsgOut;
            }
            set
            {
                _errMsgOut = value;
                RaisePropertyChanged("ErrMsgOut");
            }
        }

        /// <summary>
        /// 老师号码
        /// </summary>
        public string TxtTeacherPhone
        {
            get
            {
                return _txtTeacherPhone;
            }
            set
            {
                _txtTeacherPhone = value;
                RaisePropertyChanged("TxtTeacherPhone");
            }
        }

        private Visibility _ShowBackZYBtn;

        /// <summary>
        /// 显示返回按钮
        /// </summary>
        public Visibility ShowBackZYBtn
        {
            get
            {
                return _ShowBackZYBtn;
            }
            set
            {
                _ShowBackZYBtn = value;
                RaisePropertyChanged("ShowBackZYBtn");
            }
        }

        private Visibility _ShowImg;

        /// <summary>
        /// 显示 
        /// </summary>
        public Visibility ShowImg
        {
            get
            {
                return _ShowImg;
            }
            set
            {
                _ShowImg = value;
                RaisePropertyChanged("ShowImg");
            }
        }

        private Visibility _ShowText;

        /// <summary>
        /// 显示 
        /// </summary>
        public Visibility ShowText
        {
            get
            {
                return _ShowText;
            }
            set
            {
                _ShowText = value;
                RaisePropertyChanged("ShowText");
            }
        }

        #endregion

        #region << Btn Command >>


        private RelayCommand bindingClassCmd;//绑定新卡

        public RelayCommand BindingClassCmd
        {
            get
            {
                return bindingClassCmd ?? (bindingClassCmd = new RelayCommand(
                           (action) =>
                           {

                               if (RegexHelper.IsHandset(TxtTeacherPhone))
                               {
                                   //打开班级列表
                                   //show the dialog
                                   ExecuteRunExtendedDialog(action);
                               }
                               else
                               {
                                   ErrMsgOut = "格式错误，请输入正确的手机号";
                               }

                           }));
            }
        }

        private RelayCommand backZYCmd;//返回按钮

        public RelayCommand BackZYCmd
        {
            get
            {
                return backZYCmd ?? (backZYCmd = new RelayCommand(
                           (action) =>
                           {
                               Messenger.Default.Send(
                                   new NavigateMessage(MenuHomeworkVM.ViewName, BandingOK ? "R" : "", true),
                                   "ShowUserpapers");
                           }));
            }
        }

        #endregion

        #region << 自定义方法 >>

        private async void ExecuteRunExtendedDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new ClassListBinding();
            var viewVM = new ClassListBindingVM(view, view.classPanel, TxtTeacherPhone);
            view.DataContext = viewVM;
            //show the dialog
            //await DialogHost.Show(view, o, ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
            await DialogHostEx.ShowDialog(GlobalUser.MainWin, view, ExtendedOpenedEventHandler,
                ExtendedClosingEventHandler);
        }

        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            //Task.Factory.StartNew(() => Thread.Sleep(3000))
            //    .ContinueWith(t =>
            //    {
            //        eventargs.Session.Close();

            //        //跳转到 列表
            //        Messenger.Default.Send(new NavigateMessage(), "ShowUserpapers");
            //    }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// 关闭按钮事件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter != null)
            {
                if ((bool)eventArgs.Parameter)
                {
                    BandingOK = true;
                    if (!string.IsNullOrEmpty(GlobalUser.CLASSINFO?.Class_name))
                    {
                        Messenger.Default.Send(new NavigateMessage(MenuHomeworkVM.ViewName, BandingOK ? "R" : "", true), "ShowUserpapers");
                    }

                }
            }
        }

        #endregion
    }
}
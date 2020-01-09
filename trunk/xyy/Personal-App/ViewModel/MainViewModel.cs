using GalaSoft.MvvmLight;
using ST.Common;
using ST.Models.Api;
using System.Collections.Generic;
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


        public void SetupView(string viewName, FrameworkElement view, ViewModelBase viewModel)
        {
            if (!views.Keys.Contains(viewName))
            {
                view.DataContext = viewModel;
                views?.Add(viewName, view);
            }
        }

        public void SetupView(string viewName, FrameworkElement view, ViewModelBase viewModel, Dictionary<string, FrameworkElement> _views)
        {
            view.DataContext = viewModel;
            _views.Add(viewName, view);
        }

        public FrameworkElement CurrentView
        {
            get
            {
                return currentView;
            }
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
            get
            {
                return GlobalUser.USER;
            }
            set
            {
                _user = value;
                RaisePropertyChanged("User");
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
            get
            {
                return _validity;
            }
            set
            {
                _validity = value;
                RaisePropertyChanged("Validity");
            }
        }
    }
}
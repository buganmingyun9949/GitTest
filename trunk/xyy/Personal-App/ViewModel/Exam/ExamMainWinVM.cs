
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using ST.Common.WebApi;
using ST.Models.Api;
using ST.Models.Paper;
using Personal_App.Common;
using Personal_App.Domain;
using Personal_App.Domain.Exam;

namespace Personal_App.ViewModel.Exam
{
    public class ExamMainWinVM : ExamCommonVM
    {
        private Grid _mainContent;

        public ExamMainWinVM() { }

        public ExamMainWinVM(Grid mainContent, ExamType eTtype)
        {
            _mainContent = mainContent;
            Messenger.Reset();
            Messenger.Default.Unregister<ExamNavigateMessage>(this);
            Messenger.Default.Unregister<ExamNavigateMessage>(this, "MainExamMainWin");

            Messenger.Default.Register<ExamNavigateMessage>(this, "MainExamMainWin",
                (message) => OnExamNavigate(message), true);

            views = new Dictionary<string, FrameworkElement>();

            CheckWindowContent(eTtype);
        }

        private void OnExamNavigate(ExamNavigateMessage message)
        {
            if (message.viewElement.DataContext == null)
                message.viewElement.DataContext = message.ViewModel;
            //this.CurrentView = views[message.TargetView];
            //this.currentViewName = message.TargetView;
            //if (_mainContent.Children.Count > 0)
            //{
            //    _mainContent.Children.RemoveAt(0);
            //}
            //else
            _mainContent.Children.Clear();
            _mainContent.Children.Add(message.viewElement);
        }
    }
}

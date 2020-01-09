
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

        public ExamMainWinVM()
        {
            //Messenger.Reset();
            Messenger.Default.Unregister<ExamNavigateMessage>(this);
            Messenger.Default.Unregister<ExamNavigateMessage>(this, "MainExamMainWin");

            Messenger.Default.Register<ExamNavigateMessage>(this, "MainExamMainWin", (message) => OnExamNavigate(message), true);

        }

        public ExamMainWinVM(Grid mainContent, ExamType eTtype):this()
        {
            views = new Dictionary<string, FrameworkElement>();

            _mainContent = mainContent;

            CheckWindowContent(eTtype);
        }

        public void LoadExamMainWinVM(Grid mainContent, ExamType eTtype)
        {
            _mainContent = mainContent;

            CheckWindowContent(eTtype);
        }

        private void OnExamNavigate(ExamNavigateMessage message)
        {
            if(_mainContent==null)
                return;

            if (message.viewElement.DataContext == null)
                message.viewElement.DataContext = message.ViewModel;
            _mainContent?.Children.Clear();
            _mainContent?.Children.Add(message.viewElement);
        }
    }
}

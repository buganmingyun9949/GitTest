using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using Personal_App.Common;
using Personal_App.ViewModel.Exam;
using Plugin.Exam.Qs.ViewModel;
using ST.Common;

namespace Personal_App.Domain.Exam
{
    /// <summary>
    /// ExamQsMainWin.xaml 的交互逻辑
    /// </summary>
    public partial class ExamQsMainWin : UserControl
    {
        public ExamType ExamType { get; set; }

        public ExamQsMainWin()
        {
            InitializeComponent();

            //this.DataContext = new ExamQsMainWinVM(QsContentPanel);
        }

        private void DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            //Console.WriteLine("SAMPLE 1: Closing dialog with parameter: " + (eventArgs.Parameter ?? ""));

            //you can cancel the dialog close:
            //eventArgs.Cancel();


            if (!Equals(eventArgs.Parameter, true)) return;

            GlobalUser.DoAnswer = false;
            //GlobalUser.AudioCleanUp(); //关闭音频播放
            //(this.DataContext as ExamQsMainWinVM)?.Dispose();

            if (QsContentPanel.Children.Count > 0)
            {
                //清理 题目 对象
                ((QsContentPanel.Children[0] as UserControl).DataContext as QsBaseViewModel).Dispose();

                //清理题目控件内容
                QsContentPanel.Children.Clear();
            }
            //清理当前窗口内容
            this.DataContext = null;

            ButtonAutomationPeer peer =
                new ButtonAutomationPeer(CloseBtn);

            IInvokeProvider invokeProv =
                peer.GetPattern(PatternInterface.Invoke)
                    as IInvokeProvider;

            invokeProv.Invoke();
            //CloseBtn.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
        }
    }
}

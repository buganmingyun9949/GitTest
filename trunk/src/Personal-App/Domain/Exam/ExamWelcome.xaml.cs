﻿using Personal_App.Common;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Personal_App.ViewModel.Exam;
using ST.Common;

namespace Personal_App.Domain.Exam
{
    /// <summary>
    /// ExamWelcome.xaml 的交互逻辑
    /// </summary>
    public partial class ExamWelcome : UserControl
    {
        public ExamWelcome()
        {
            InitializeComponent();

        }

        private void DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            //Console.WriteLine("SAMPLE 1: Closing dialog with parameter: " + (eventArgs.Parameter ?? ""));

            //you can cancel the dialog close:
            //eventArgs.Cancel();


            if (!Equals(eventArgs.Parameter, true)) return;

            GlobalUser.DoAnswer = false;
            //GlobalUser.AudioCleanUp(); //关闭音频播放
            (this.DataContext as ExamCommonVM)?.Dispose();
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

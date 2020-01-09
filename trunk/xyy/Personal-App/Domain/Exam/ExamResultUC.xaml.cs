using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using MaterialDesignThemes.Wpf;
using ST.Common;

namespace Personal_App.Domain.Exam
{
    /// <summary>
    /// ExamResultUC.xaml 的交互逻辑
    /// </summary>
    public partial class ExamResultUC : UserControl
    {
        public ExamResultUC()
        {
            InitializeComponent();
        }
         

        private void ExamResultUC_OnLoaded(object sender, RoutedEventArgs e)
        {

            try
            {
                ButtonAutomationPeer peer =
                    new ButtonAutomationPeer(BtnReturnHome);

                IInvokeProvider invokeProv =
                    peer.GetPattern(PatternInterface.Invoke)
                        as IInvokeProvider;

                invokeProv.Invoke();

                //CloseBtn.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
            }
            catch
            {

            }
        }

        private void BtnOkCommand_OnClick(object sender, RoutedEventArgs e)
        {
            //for (int i = 0; i < 10; i++)
            //{
            //    QsItemlb.SelectedIndex = 1;

            //    Thread.Sleep(60);

            //    QsItemlb.SelectedIndex = 0;
            //}

            QsItemlb.SelectedIndex = 0;
        }

        private void BtnReturnHome_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                ButtonAutomationPeer peer =
                    new ButtonAutomationPeer(CloseCurrentBtn);

                IInvokeProvider invokeProv =
                    peer.GetPattern(PatternInterface.Invoke)
                        as IInvokeProvider;
                invokeProv.Invoke();
            }
            catch
            {

            }
        }
    }
}

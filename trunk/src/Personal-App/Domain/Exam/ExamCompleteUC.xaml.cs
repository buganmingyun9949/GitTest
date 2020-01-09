using Personal_App.Common;
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
    /// ExamCompleteUC.xaml 的交互逻辑
    /// </summary>
    public partial class ExamCompleteUC : UserControl, IDisposable
    {
        public ExamCompleteUC()
        {
            InitializeComponent(); 
        } 

        private void DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            //Console.WriteLine("SAMPLE 1: Closing dialog with parameter: " + (eventArgs.Parameter ?? ""));

            //you can cancel the dialog close:
            //eventArgs.Cancel();


            if (!Equals(eventArgs.Parameter, true)) return;

            GlobalUser.AudioCleanUp(); //关闭音频播放

            ButtonAutomationPeer peer =
                new ButtonAutomationPeer(CloseBtn);

            IInvokeProvider invokeProv =
                peer.GetPattern(PatternInterface.Invoke)
                    as IInvokeProvider;

            invokeProv.Invoke();

            //CloseBtn.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~ExamCompleteUC() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            //GC.SuppressFinalize(this);

            var vm = this.DataContext as ExamCompleteVM;
            vm.Dispose();
        }
        #endregion
    }
}

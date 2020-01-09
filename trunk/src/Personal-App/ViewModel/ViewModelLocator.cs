/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:WpfApplication1"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Personal_App.ViewModel.Exam;

namespace Personal_App.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            //ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //SimpleIoc.Default.Register<MainWindowVM>();
            //SimpleIoc.Default.Register<ExamQsMainWinVM>();
            //SimpleIoc.Default.Register<ExamMainWinVM>();
        }

        private static MainWindowVM _MainWindow;
        public static MainWindowVM MainWindow
        {
            set { _MainWindow = value; }
            get
            {
                if (_MainWindow == null)
                    _MainWindow = new MainWindowVM();
                return _MainWindow;
            }
        }

        private static ExamQsMainWinVM _ExamQsMain;
        public static ExamQsMainWinVM ExamQsMain
        {
            set { _ExamQsMain = value; }
            get
            {
                if (_ExamQsMain == null)
                    _ExamQsMain = new ExamQsMainWinVM();
                return _ExamQsMain;
            }
        }

        private static SyncQsMainWinVM _SyncQsMain;
        public static SyncQsMainWinVM SyncQsMain
        {
            set { _SyncQsMain = value; }
            get
            {
                if (_SyncQsMain == null)
                    _SyncQsMain = new SyncQsMainWinVM();
                return _SyncQsMain;
            }
        }

        private static ExamMainWinVM _ExamMain;
        public static ExamMainWinVM ExamMain
        {
            set { _ExamMain = value; }
            get
            {
                if (_ExamMain == null)
                    _ExamMain = new ExamMainWinVM();
                return _ExamMain;
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}
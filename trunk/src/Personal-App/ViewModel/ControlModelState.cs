using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Framework.Engine;
using GalaSoft.MvvmLight;
using Personal_App.ViewModel.Exam;
using VoiceRecorder.Audio;

namespace Personal_App.ViewModel
{
    public class NavigateMessage
    {
        public string TargetView { get; private set; }
        public object State { get; private set; }
        public FrameworkElement Fe { get; private set; }
        public ViewModelBase VmBase { get; private set; }
        public bool IsClear { get; private set; }
        public NavigateMessage() { }
        public NavigateMessage(string targetView, object state, bool isClear = true)
        {
            this.TargetView = targetView;
            this.State = state;
            this.IsClear = isClear;
        }

        public NavigateMessage(string targetView, object state, FrameworkElement fe, bool isClear = true)
        {
            this.TargetView = targetView;
            this.State = state;
            this.Fe = fe;
            this.IsClear = isClear;
        }
    }
    public class MainDialogMessage
    {
        public string MessageText { get; private set; } 

        public FrameworkElement ControlElement { get; private set; }

        public MainDialogMessage(string msgText)
        {
            this.MessageText = msgText;
        }

        public MainDialogMessage(FrameworkElement controlElement, string msgText)
        {
            this.ControlElement = controlElement;

            this.MessageText = msgText;
        }
    }

    public class ExamNavigateMessage
    {
        public string TargetView { get; private set; }

        public FrameworkElement viewElement { get; private set; }
        public object State { get; private set; }
        public ExamCommonVM ViewModel { get; private set; }

        public ExamNavigateMessage(string targetView, FrameworkElement view, ExamCommonVM viewModel)
        {
            this.TargetView = targetView;
            this.viewElement = view;
            this.ViewModel = viewModel;
        }
    }

    public class ExamQsNavigateMessage
    {
        public string TargetView { get; private set; }

        public FrameworkElement viewElement { get; private set; }
        public object State { get; private set; }
        public ViewModelBase ViewModel { get; private set; }

        public ExamQsNavigateMessage(string targetView, FrameworkElement view, ViewModelBase viewModel)
        {
            this.TargetView = targetView;
            this.viewElement = view;
            this.ViewModel = viewModel;
        }
    }
     
    public class CardExpireDialogMessage
    {
        public string TargetView { get; private set; }

        public FrameworkElement ViewElement { get; private set; }

        public object State { get; private set; }

        public CardExpireDialogVM ViewModel { get; private set; }

        public CardExpireDialogMessage(string targetView, FrameworkElement view, CardExpireDialogVM viewModel)
        {
            this.TargetView = targetView;
            this.ViewElement = view;
            this.ViewModel = viewModel;
        }
    }

    class ControlModelState
    {
        private string recordingFileName;
        private string effectedFileName;
        private AutoTuneSettings autoTuneSettings;

        public ControlModelState(string recordingFileName, string effectedFileName)
        {
            this.RecordingFileName = recordingFileName;
            this.EffectedFileName = effectedFileName;
            this.autoTuneSettings = new AutoTuneSettings();
        }

        public string RecordingFileName
        {
            get
            {
                return recordingFileName;
            }
            set
            {
                if ((recordingFileName != null) && (recordingFileName != value))
                {
                    DeleteFile(recordingFileName);
                }
                this.recordingFileName = value;
            }
        }

        public string EffectedFileName
        {
            get
            {
                return effectedFileName;
            }
            set
            {
                if ((effectedFileName != null) && (effectedFileName != value))
                {
                    DeleteFile(effectedFileName);
                }
                this.effectedFileName = value;
            }
        }

        public string ActiveFile
        {
            get
            {
                if (autoTuneSettings.Enabled && !String.IsNullOrEmpty(EffectedFileName))
                {
                    return EffectedFileName;
                }
                return RecordingFileName;
            }
        }

        public AutoTuneSettings AutoTuneSettings
        {
            get
            {
                return autoTuneSettings;
            }
        }

        public void DeleteFiles()
        {
            this.RecordingFileName = null;
            this.EffectedFileName = null;
        }

        private void DeleteFile(string fileName)
        {
            if (!String.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }

}

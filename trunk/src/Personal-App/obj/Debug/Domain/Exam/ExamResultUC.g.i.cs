﻿#pragma checksum "..\..\..\..\Domain\Exam\ExamResultUC.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "60982D8C9E0D3FDB6C895E5728FBD08EFCDD65E2"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
using ST.Common.Domain;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Personal_App.Domain.Exam {
    
    
    /// <summary>
    /// ExamResultUC
    /// </summary>
    public partial class ExamResultUC : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\..\..\Domain\Exam\ExamResultUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image TopLogoImage;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\..\Domain\Exam\ExamResultUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MinimizeBtn;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\..\Domain\Exam\ExamResultUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid TotalResultView;
        
        #line default
        #line hidden
        
        
        #line 128 "..\..\..\..\Domain\Exam\ExamResultUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer ResultSV;
        
        #line default
        #line hidden
        
        
        #line 129 "..\..\..\..\Domain\Exam\ExamResultUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid PaperDetailView;
        
        #line default
        #line hidden
        
        
        #line 136 "..\..\..\..\Domain\Exam\ExamResultUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOkCommand;
        
        #line default
        #line hidden
        
        
        #line 138 "..\..\..\..\Domain\Exam\ExamResultUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnReturnHome;
        
        #line default
        #line hidden
        
        
        #line 141 "..\..\..\..\Domain\Exam\ExamResultUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox QsItemlb;
        
        #line default
        #line hidden
        
        
        #line 180 "..\..\..\..\Domain\Exam\ExamResultUC.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CloseCurrentBtn;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Personal-App;component/domain/exam/examresultuc.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Domain\Exam\ExamResultUC.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 10 "..\..\..\..\Domain\Exam\ExamResultUC.xaml"
            ((Personal_App.Domain.Exam.ExamResultUC)(target)).Loaded += new System.Windows.RoutedEventHandler(this.ExamResultUC_OnLoaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.TopLogoImage = ((System.Windows.Controls.Image)(target));
            return;
            case 3:
            this.MinimizeBtn = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.TotalResultView = ((System.Windows.Controls.Grid)(target));
            return;
            case 5:
            this.ResultSV = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 6:
            this.PaperDetailView = ((System.Windows.Controls.Grid)(target));
            return;
            case 7:
            this.btnOkCommand = ((System.Windows.Controls.Button)(target));
            
            #line 136 "..\..\..\..\Domain\Exam\ExamResultUC.xaml"
            this.btnOkCommand.Click += new System.Windows.RoutedEventHandler(this.BtnOkCommand_OnClick);
            
            #line default
            #line hidden
            return;
            case 8:
            this.BtnReturnHome = ((System.Windows.Controls.Button)(target));
            
            #line 139 "..\..\..\..\Domain\Exam\ExamResultUC.xaml"
            this.BtnReturnHome.Click += new System.Windows.RoutedEventHandler(this.BtnReturnHome_OnClick);
            
            #line default
            #line hidden
            return;
            case 9:
            this.QsItemlb = ((System.Windows.Controls.ListBox)(target));
            return;
            case 10:
            this.CloseCurrentBtn = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}


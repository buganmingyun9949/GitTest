using Personal_App.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Personal_App.CustomDependencyProperties
{
    public class WindowProperties
    {
        //public static readonly DependencyProperty WindowClosingProperty = DependencyProperty.RegisterAttached("WindowClosing", typeof(RelayCommand), typeof(WindowProperties), new UIPropertyMetadata(null, WindowClosing));

        //public static object GetWindowClosing(DependencyObject depObj)
        //{
        //    return (RelayCommand)depObj.GetValue(WindowClosingProperty);
        //}

        //public static void SetWindowClosing(DependencyObject depObj, RelayCommand value)
        //{
        //    depObj.SetValue(WindowClosingProperty, value);
        //}

        //private static void WindowClosing(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        //{
        //    var element = (Window)depObj;

        //    if (element != null)
        //        element.Closing += OnWindowClosing;

        //}

        //private static void OnWindowClosing(object sender, CancelEventArgs e)
        //{
        //    RelayCommand command = (RelayCommand)GetWindowClosing((DependencyObject)sender);
        //    command.Execute((Window)sender);
        //}


        ///// <summary>
        ///// The DependencyProperty for the CommandParameter
        ///// </summary>
        //public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter", typeof(RelayCommand), typeof(WindowProperties), new UIPropertyMetadata(null, BindingCommandParameter));

        //public static object GetCommandParameter(DependencyObject depObj)
        //{
        //    return (RelayCommand)depObj.GetValue(CommandParameterProperty);
        //}

        //public static void SetCommandParameter(DependencyObject depObj, RelayCommand value)
        //{
        //    depObj.SetValue(CommandParameterProperty, value);
        //}

        //private static void BindingCommandParameter(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        //{
        //    var element = (Window)depObj;
        //    if (element != null)
        //        element.Closing += OnBindingCommandParameter;
        //    //Binding binding = new Binding("Text") { Source = textBox };
        //    //BindingOperations.SetBinding(test, Test.ContentProperty, binding);
        //}

        //private static void OnBindingCommandParameter(object sender, CancelEventArgs e)
        //{
        //    RelayCommand command = (RelayCommand)GetCommandParameter((DependencyObject)sender);
        //    command.Execute((Window)sender);
        //}

        ///// <summary>
        ///// Reflects the parameter to pass to the CommandProperty upon execution.
        ///// </summary>
        //[Bindable(true), Category("Action")]
        //[Localizability(LocalizationCategory.NeverLocalize)]
        //public object CommandParameter
        //{
        //    get
        //    {
        //        return GetValue(CommandParameterProperty);
        //    }
        //    set
        //    {
        //        SetValue(CommandParameterProperty, value);
        //    }
        //}


        //      public static readonly DependencyProperty WindowCommandParameterProperty =
        //DependencyProperty.RegisterAttached("CommandParameter", typeof(RelayCommand), typeof(WindowProperties), new UIPropertyMetadata(null, CommandParameter));

        //      public static object GetCommandParameter(DependencyObject depObj)
        //      {
        //          return (RelayCommand)depObj.GetValue(WindowCommandParameterProperty);
        //      }

        //      public static void SetCommandParameter(DependencyObject depObj, RelayCommand value)
        //      {
        //          depObj.SetValue(WindowClosingProperty, value);
        //      }

        //      private static void CommandParameter(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        //      {
        //          var element = (Window)depObj;

        //          if (element != null)
        //              element.Closing += OnCommandParameter;

        //      }

        //      private static void OnCommandParameter(object sender, CancelEventArgs e)
        //      {
        //          RelayCommand command = (RelayCommand)GetCommandParameter((DependencyObject)sender);
        //          command.Execute((Window)sender);
        //      }
    }
}

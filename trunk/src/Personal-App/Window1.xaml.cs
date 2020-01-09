using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Personal_App
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        private void OnClick(object sender, RoutedEventArgs e)
        {
            _popup.IsOpen = !_popup.IsOpen;

            DependencyObject parent = _popup.Child;


            do
            {
                parent = VisualTreeHelper.GetParent(parent);

                if (parent != null && parent.ToString() == "System.Windows.Controls.Primitives.PopupRoot")
                {
                    var element = parent as FrameworkElement;

                    var mainWin = Application.Current.MainWindow;

                    element.Height = this._popup.Height;
                    element.Width = this._popup.Width;

                    break;
                }
            }
            while (parent != null);
        }
    }
}

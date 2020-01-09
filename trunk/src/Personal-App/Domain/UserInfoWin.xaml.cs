using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ST.Common.ToolsHelper;

namespace Personal_App.Domain
{
    /// <summary>
    /// BindingClass.xaml 的交互逻辑
    /// </summary>
    public partial class UserInfoWin : UserControl
    {
        Point _point;

        public UserInfoWin()
        {
            InitializeComponent();

            //BindCardImage.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.bindCard);
        }

        //private void LblName_OnSizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    _point = lblName.TransformToAncestor(UserNamePanel).Transform(new Point(0, 0));

        //    var width = lblName.ActualWidth;

        //    BtnEditUserName.Margin = new Thickness(width + 50, 2, 0, 0);

        //}

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Image img = sender as Image;

            img.Visibility = Visibility.Visible;
        }
    }
}

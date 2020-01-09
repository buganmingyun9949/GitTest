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
    /// BindingCard.xaml 的交互逻辑
    /// </summary>
    public partial class BindingCardRemind : UserControl
    {
        public BindingCardRemind()
        {
            InitializeComponent();

            //BindCardImage.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.bindCard);
        }

        //private void TxtCardNo1_OnTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (txtCardNo1.Text.Length== 4)
        //    {
        //        txtCardNo1.Text = txtCardNo1.Text.Substring(0, 4);
        //        txtCardNo2.Focus();
        //    }
        //}

        //private void TxtCardNo2_OnTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (txtCardNo2.Text.Length >= 4)
        //    {
        //        txtCardNo2.Text = txtCardNo2.Text.Substring(0, 4);
        //        txtCardNo3.Focus();
        //    }
        //}

        //private void TxtCardNo3_OnTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (txtCardNo3.Text.Length == 4)
        //    {
        //        txtCardNo3.Text = txtCardNo3.Text.Substring(0, 4);
        //        txtCardNo4.Focus();
        //    }
        //}

        //private void TxtCardNo4_OnTextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (txtCardNo4.Text.Length >= 4)
        //        txtCardNo4.Text = txtCardNo4.Text.Substring(0, 4);

        //}
    }
}

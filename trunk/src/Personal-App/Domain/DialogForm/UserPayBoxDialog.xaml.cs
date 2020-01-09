using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
using ST.Common;
using ST.Common.ToolsHelper;
using Image = System.Windows.Controls.Image;

namespace Personal_App.Domain
{
    /// <summary>
    /// UserPayBoxDialog.xaml 的交互逻辑
    /// </summary>
    public partial class UserPayBoxDialog : UserControl
    {
        public UserPayBoxDialog()
        {
            InitializeComponent();

            //BindCardImage.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.bindCard);
            int card_auth_num = Convert.ToInt32(GlobalUser.STUDYCARD.card_auth);
            //if (!((card_auth_num & (1 << 1)) > 0))

            gdEntityCard.Visibility = Visibility.Collapsed;
            if ((card_auth_num & (1 << 1)) > 0) //可绑卡续费
            {
                gdEntityCard.Visibility = Visibility.Visible;
                //this.Height = 320;
            }
        }

        private void UserPayBoxDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            TxtPayMoney1.Text = TxtPayMoney2.Text = GlobalUser.STUDYCARD.card_price + "元";
            TxtCardName1.Text = TxtCardName2.Text = GlobalUser.STUDYCARD.card_name;

            //int card_auth_num = Convert.ToInt32(GlobalUser.STUDYCARD.card_auth);
            //if ((card_auth_num & (1 << 1)) > 0)
            //{
            //    btnBindCard.Visibility = Visibility.Collapsed;
            //    this.Height = this.Height - 40;
            //}

            //GeneratorQR("12321321321321321321321", ImgAliPay);
        }

        //// 生成二维码
        //private Image GeneratorQR(string msg, Image img)
        //{
        //    //BarcodeWriter writer = new BarcodeWriter
        //    //{
        //    //    Format = BarcodeFormat.QR_CODE
        //    //};
        //    //writer.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");     // 编码问题
        //    //writer.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ZXing.QrCode.Internal.ErrorCorrectionLevel.H);
        //    //int codeSizeInPixels = 256;      // 设置图片长宽
        //    //writer.Options.Height = codeSizeInPixels;
        //    //writer.Options.Width = codeSizeInPixels;
        //    //writer.Options.Margin = 0;       // 设置边框
        //    //BitMatrix bm = writer.Encode(msg);
        //    //Bitmap img0 = writer.Write(bm);
        //    //img.Source = BitmapToBitmapImage(img0);
        //    //return img;
        //} 

        // Bitmap --> BitmapImage
        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }

        // BitmapImage --> Bitmap
        public static Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);
                return new Bitmap(bitmap);
            }
        }
    }
}

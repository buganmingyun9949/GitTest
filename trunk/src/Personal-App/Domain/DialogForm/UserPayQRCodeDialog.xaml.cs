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
using ST.Common.WebApi;
using ST.Models.Api;
using Image = System.Windows.Controls.Image;

namespace Personal_App.Domain
{
    /// <summary>
    /// UserPayBoxDialog.xaml 的交互逻辑
    /// </summary>
    public partial class UserPayQRCodeDialog : UserControl
    {
        public UserPayQRCodeDialog()
        {
            InitializeComponent();
        }

        public UserPayQRCodeDialog(int payType) : this()
        {
            if (payType == 1)
            {
                PayTypeName.Text = "使用支付宝支付";
                ImgPayIcon.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.alipay);
            }

            if (payType == 2)
            {
                PayTypeName.Text = "使用微信支付";
                ImgPayIcon.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.wechatpay);
            }

            PayMoneyShow.Text = GlobalUser.STUDYCARD.card_price + "元";

            var result0 = WebApiProxy.GetHtmlRespInfo(ApiType.GetCardOrder, GlobalUser.USER.Token);

            UserOrderModel model = new UserOrderModel()
            {
                token = GlobalUser.USER.Token,
                order_id = result0.retData.order_id.ToString()
            };
            var result1 = WebApiProxy.GetImage(model, ApiType.GetQrCode, null, "get");
            
            ImgQrCode.Source = ConvertHelper.ChangeBitmapToImageSource(result1 as Bitmap);
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

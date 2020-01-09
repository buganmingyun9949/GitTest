using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Framework.Recorder;
//using ControlzEx.Standard;
using ST.Common.ToolsHelper;
using ST.Models.Score;
using Personal_App.Common;
using Personal_App.ViewModel;
using MaterialDesignThemes.Wpf;
using NAudio.Wave;
using ST.Common.WebApi;

namespace Personal_App
{
    /// <summary>
    /// TestWin.xaml 的交互逻辑
    /// </summary>
    public partial class TestWin : Window
    {
        public TestWin()
        {
            InitializeComponent();

            this.DataContext = new TestWinVM();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //var mySettings = new MetroDialogSettings()
            //{
            //    AffirmativeButtonText = "确认",
            //    NegativeButtonText = "取消",
            //    ColorScheme = MetroDialogColorScheme.Theme
            //};

            //MessageDialogResult result = this.ShowModalMessageConfirmExternal("", "关闭程序!",
            //    MessageDialogStyle.AffirmativeAndNegative, mySettings, 280);

            //if (result == MessageDialogResult.Affirmative)
            //    Environment.Exit(0); //关闭程序
        }

        //private MessageDialogResult ShowModalMessageConfirmExternal(string v1, string v2, MessageDialogStyle affirmativeAndNegative, MetroDialogSettings mySettings, int v3)
        //{
        //    throw new NotImplementedException();
        //}

        //private async void ShowDialogOutside(object sender, RoutedEventArgs e)
        //{
        //    var dialog = (BaseMetroDialog)this.Resources["CustomDialogTest"];
        //    dialog.DialogSettings.ColorScheme = MetroDialogColorScheme.Theme;
        //    dialog = dialog.ShowDialogExternally();

        //    await Task.Delay(5000);

        //    await dialog.RequestCloseAsync();
        //}
        //private async void ShowMyInfo(object sender, RoutedEventArgs e)
        //{
        //    var dialog = (BaseMetroDialog)this.Resources["CustomDialogTest"];
        //    dialog.DialogSettings.ColorScheme = MetroDialogColorScheme.Theme;
        //    dialog = dialog.ShowDialogExternally();

        //    await Task.Delay(5000);

        //    await dialog.RequestCloseAsync();
        //}

        private void Sample1_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("SAMPLE 1: Closing dialog with parameter: " + (eventArgs.Parameter ?? ""));

            //you can cancel the dialog close:
            //eventArgs.Cancel();

            if (!Equals(eventArgs.Parameter, true)) return;

            if (!string.IsNullOrWhiteSpace(FruitTextBox.Text))
                FruitListBox.Items.Add(FruitTextBox.Text.Trim());
        }

        private void Sample2_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("SAMPLE 2: Closing dialog with parameter: " + (eventArgs.Parameter ?? ""));
        }

        private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
        {
            string paperJson;
            using (var jsonContent = new FileStream(@"E:\C#\EnglishExam\LTS\trunk\LTS-PC\bin\LTS-App\Debug\Data/USER_39\simulation_12_2018031611070156160369\simulation_12_2018031611070156160369.json", FileMode.Open))
            {
                int fsLen = (int)jsonContent.Length;
                byte[] heByte = new byte[fsLen];
                int r = jsonContent.Read(heByte, 0, heByte.Length);
                //string myStr = Encoding.UTF8.GetString(heByte);

                paperJson = Base64Provider.Decrypt(Encoding.UTF8.GetString(heByte), Base64Provider.KEY, Base64Provider.IV);
            }

            // '定义一个中文标点的数组对象  
            string[] ChineseInterpunction =
                {"。", "，", "；", "：", "？", "！", "“", "”", "……", "—", "～", "（", "）", "《", "》"};
            //'定义一个英文标点的数组对象 
            string[] EnglishInterpunction =
                {".", ",", ";", ":", "?", "!", "\"", "\"", "…", "-", "~", "(", ")", "<", ">"};


            //var vvv = Microsoft.VisualBasic.Strings.StrConv("你好！朋友。", Microsoft.VisualBasic.VbStrConv.Narrow, 0);
            string strSBC = "中华１２５９ｔｅｓｔ ’。！";
            string result = ToDBC(strSBC);
            Console.WriteLine(result);
            //Assert.AreEqual(result, "中华1259test'.!");


            for (int i = 0; i < ChineseInterpunction.Length; i++)
            {
                strSBC = strSBC.Replace(ChineseInterpunction[i], EnglishInterpunction[i]);
            }

            Console.WriteLine(strSBC);
        }

        private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
        {
            var msStream = WebApiProxy.GetFile(@"http://win.web.nf01.sycdn.kuwo.cn/resource/n1/10/47/2323037830.mp3");
            var readFullyStream = new Framework.Recorder.ReadFullyStream(msStream);

            try
            {

                //var inputStream = new AudioFileReader(msStream);
                //var aggregator = new SampleAggregator(inputStream);

                //var waveOut = CreateWaveOut();
                ////waveOut.PlaybackStopped += OnPlaybackStopped;
                ////var volumeProvider = new VolumeWaveProvider16(bufferedWaveProvider);
                ////volumeProvider.Volume = volumeSlider1.Volume;
                //waveOut.Init(aggregator);
                //waveOut.Play();

            }
            catch (EndOfStreamException)
            {
                // reached the end of the MP3 file / stream
            }
            catch (WebException)
            {
                // probably we have aborted download from the GUI thread
            }
            catch (Exception)
            {
                // probably we have aborted download from the GUI thread
            }

            if (IsBufferNearlyFull)
            {

            }
        }

        private IWavePlayer CreateWaveOut()
        {
            return new WaveOut();
        }

        private static IMp3FrameDecompressor CreateFrameDecompressor(Mp3Frame frame)
        {
            WaveFormat waveFormat = new Mp3WaveFormat(frame.SampleRate, frame.ChannelMode == ChannelMode.Mono ? 1 : 2,
                frame.FrameLength, frame.BitRate);
            return new AcmMp3FrameDecompressor(waveFormat);
        }

        BufferedWaveProvider bufferedWaveProvider = null;
        private bool IsBufferNearlyFull
        {
            get
            {
                return bufferedWaveProvider != null &&
                       bufferedWaveProvider.BufferLength - bufferedWaveProvider.BufferedBytes
                       < bufferedWaveProvider.WaveFormat.AverageBytesPerSecond / 4;
            }
        }

        ///
        /// 转半角的函数(DBC case)
        ///
        ///任意字符串
        ///半角字符串
        ///
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///
        public static String ToDBC(String input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new String(c);
        }

        private void Button111_Click(object sender, RoutedEventArgs e)
        {
            string resultStr =
                @"{""tokenId"":""5ad459bb05479116ac000007"",""refText"":""Hi everyone. Welcome to the island and nice to meet you here! My name is Ted. I will be your guide for the next seven days. Now, let me introduce our traveling plan. In May, the weather in the north is cool and wet, but in the south it's hot and wet. So please keep an umbrella or a raincoat with you. This island has beautiful views everywhere, so don't forget to take your camera. While we are visiting places of interest, please stay with the group. If you get lost, don't worry. Just call me! Then stay where you are and I will find you. My phone number is 258-5465."",""audioUrl"":""records.17kouyu.com\/5ad45997319bbae7f400134f"",""dtLastResponse"":""2018-04-16 16:07:07:738"",""result"":{""fluency"":73,""duration"":""85.000"",""kernel_version"":""2.7.3"",""warning"":[{""code"":1004,""message"":""Audio noisy!""}],""integrity"":74,""rhythm"":59,""resource_version"":""1.4.8"",""overall"":47,""pronunciation"":47,""sentences"":[{""overall"":42,""sentence"":""Hi everyone."",""details"":[{""prominence"":0,""overall"":21,""word"":""Hi""},{""prominence"":0,""overall"":62,""word"":""everyone.""}]},{""overall"":46,""sentence"":""Welcome to the island and nice to meet you here!"",""details"":[{""prominence"":0,""overall"":38,""word"":""Welcome""},{""prominence"":0,""overall"":99,""word"":""to""},{""prominence"":0,""overall"":22,""word"":""the""},{""prominence"":0,""overall"":44,""word"":""island""},{""prominence"":0,""overall"":63,""word"":""and""},{""prominence"":0,""overall"":41,""word"":""nice""},{""prominence"":0,""overall"":61,""word"":""to""},{""prominence"":1,""overall"":15,""word"":""meet""},{""prominence"":0,""overall"":1,""word"":""you""},{""prominence"":0,""overall"":73,""word"":""here!""}]},{""overall"":75,""sentence"":""My name is Ted."",""details"":[{""prominence"":0,""overall"":77,""word"":""My""},{""prominence"":0,""overall"":66,""word"":""name""},{""prominence"":0,""overall"":67,""word"":""is""},{""prominence"":0,""overall"":88,""word"":""Ted.""}]},{""overall"":40,""sentence"":""I will be your guide for the next seven days."",""details"":[{""prominence"":0,""overall"":16,""word"":""I""},{""prominence"":0,""overall"":2,""word"":""will""},{""prominence"":0,""overall"":77,""word"":""be""},{""prominence"":0,""overall"":72,""word"":""your""},{""prominence"":0,""overall"":48,""word"":""guide""},{""prominence"":0,""overall"":64,""word"":""for""},{""prominence"":0,""overall"":5,""word"":""the""},{""prominence"":0,""overall"":27,""word"":""next""},{""prominence"":0,""overall"":15,""word"":""seven""},{""prominence"":0,""overall"":75,""word"":""days.""}]},{""overall"":67,""sentence"":""Now,"",""details"":[{""prominence"":0,""overall"":67,""word"":""Now,""}]},{""overall"":43,""sentence"":""let me introduce our traveling plan."",""details"":[{""prominence"":0,""overall"":72,""word"":""let""},{""prominence"":0,""overall"":67,""word"":""me""},{""prominence"":0,""overall"":9,""word"":""introduce""},{""prominence"":0,""overall"":22,""word"":""our""},{""prominence"":0,""overall"":76,""word"":""traveling""},{""prominence"":0,""overall"":14,""word"":""plan.""}]},{""overall"":39,""sentence"":""In May,"",""details"":[{""prominence"":0,""overall"":75,""word"":""In""},{""prominence"":0,""overall"":3,""word"":""May,""}]},{""overall"":43,""sentence"":""the weather in the north is cool and wet,"",""details"":[{""prominence"":0,""overall"":33,""word"":""the""},{""prominence"":0,""overall"":63,""word"":""weather""},{""prominence"":0,""overall"":68,""word"":""in""},{""prominence"":0,""overall"":5,""word"":""the""},{""prominence"":0,""overall"":42,""word"":""north""},{""prominence"":0,""overall"":70,""word"":""is""},{""prominence"":0,""overall"":7,""word"":""cool""},{""prominence"":0,""overall"":63,""word"":""and""},{""prominence"":0,""overall"":35,""word"":""wet,""}]},{""overall"":60,""sentence"":""but in the south it's hot and wet."",""details"":[{""prominence"":0,""overall"":100,""word"":""but""},{""prominence"":0,""overall"":93,""word"":""in""},{""prominence"":0,""overall"":9,""word"":""the""},{""prominence"":0,""overall"":44,""word"":""south""},{""prominence"":0,""overall"":78,""word"":""it's""},{""prominence"":0,""overall"":33,""word"":""hot""},{""prominence"":0,""overall"":78,""word"":""and""},{""prominence"":0,""overall"":47,""word"":""wet.""}]},{""overall"":59,""sentence"":""So please keep an umbrella or a raincoat with you."",""details"":[{""prominence"":0,""overall"":18,""word"":""So""},{""prominence"":0,""overall"":95,""word"":""please""},{""prominence"":0,""overall"":97,""word"":""keep""},{""prominence"":0,""overall"":71,""word"":""an""},{""prominence"":0,""overall"":65,""word"":""umbrella""},{""prominence"":0,""overall"":71,""word"":""or""},{""prominence"":0,""overall"":36,""word"":""a""},{""prominence"":0,""overall"":39,""word"":""raincoat""},{""prominence"":0,""overall"":60,""word"":""with""},{""prominence"":0,""overall"":33,""word"":""you.""}]},{""overall"":38,""sentence"":""This island has beautiful views everywhere,"",""details"":[{""prominence"":0,""overall"":61,""word"":""This""},{""prominence"":0,""overall"":22,""word"":""island""},{""prominence"":0,""overall"":62,""word"":""has""},{""prominence"":0,""overall"":66,""word"":""beautiful""},{""prominence"":0,""overall"":1,""word"":""views""},{""prominence"":0,""overall"":18,""word"":""everywhere,""}]},{""overall"":75,""sentence"":""so don't forget to take your camera."",""details"":[{""prominence"":0,""overall"":89,""word"":""so""},{""prominence"":0,""overall"":64,""word"":""don't""},{""prominence"":0,""overall"":64,""word"":""forget""},{""prominence"":0,""overall"":94,""word"":""to""},{""prominence"":0,""overall"":75,""word"":""take""},{""prominence"":0,""overall"":72,""word"":""your""},{""prominence"":0,""overall"":68,""word"":""camera.""}]},{""overall"":23,""sentence"":""While we are visiting places of interest,"",""details"":[{""prominence"":0,""overall"":6,""word"":""While""},{""prominence"":0,""overall"":6,""word"":""we""},{""prominence"":0,""overall"":1,""word"":""are""},{""prominence"":0,""overall"":1,""word"":""visiting""},{""prominence"":0,""overall"":66,""word"":""places""},{""prominence"":0,""overall"":69,""word"":""of""},{""prominence"":0,""overall"":13,""word"":""interest,""}]},{""overall"":43,""sentence"":""please stay with the group."",""details"":[{""prominence"":0,""overall"":21,""word"":""please""},{""prominence"":0,""overall"":78,""word"":""stay""},{""prominence"":0,""overall"":61,""word"":""with""},{""prominence"":0,""overall"":26,""word"":""the""},{""prominence"":0,""overall"":29,""word"":""group.""}]},{""overall"":55,""sentence"":""If you get lost,"",""details"":[{""prominence"":0,""overall"":93,""word"":""If""},{""prominence"":0,""overall"":58,""word"":""you""},{""prominence"":0,""overall"":4,""word"":""get""},{""prominence"":0,""overall"":63,""word"":""lost,""}]},{""overall"":54,""sentence"":""don't worry."",""details"":[{""prominence"":0,""overall"":69,""word"":""don't""},{""prominence"":0,""overall"":38,""word"":""worry.""}]},{""overall"":62,""sentence"":""Just call me!"",""details"":[{""prominence"":0,""overall"":62,""word"":""Just""},{""prominence"":0,""overall"":99,""word"":""call""},{""prominence"":0,""overall"":24,""word"":""me!""}]},{""overall"":46,""sentence"":""Then stay where you are and I will find you."",""details"":[{""prominence"":0,""overall"":61,""word"":""Then""},{""prominence"":0,""overall"":90,""word"":""stay""},{""prominence"":0,""overall"":68,""word"":""where""},{""prominence"":0,""overall"":21,""word"":""you""},{""prominence"":0,""overall"":67,""word"":""are""},{""prominence"":0,""overall"":61,""word"":""and""},{""prominence"":0,""overall"":7,""word"":""I""},{""prominence"":0,""overall"":10,""word"":""will""},{""prominence"":0,""overall"":37,""word"":""find""},{""prominence"":0,""overall"":37,""word"":""you.""}]},{""overall"":31,""sentence"":""My phone number is 258-5465."",""details"":[{""prominence"":0,""overall"":24,""word"":""My""},{""prominence"":1,""overall"":26,""word"":""phone""},{""prominence"":0,""overall"":73,""word"":""number""},{""prominence"":0,""overall"":33,""word"":""is""},{""prominence"":1,""overall"":1,""word"":""258-5465.""}]}]},""eof"":1,""applicationId"":""151012757600003e"",""recordId"":""5ad45997319bbae7f400134f""}";


            ScoreRoot score = JsonHelper.FromJson<ScoreRoot>(resultStr.Replace("\"params\"", "\"param\""));
        }
    }
    public class SliderValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double val = (double)values[0];
            double min = (double)values[1];
            double max = (double)values[2];
            double sliderWidth = (double)values[3];
            return sliderWidth * (val - min) / (max - min);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Framework.Engine;
using Framework.Logging;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using NAudio.Wave;
using Plugin.Exam.Qs.Common;
using Plugin.Exam.Qs.View;
using Plugin.Exam.Qs.View.SubItem;
using Plugin.Exam.Qs.ViewModel.SubItem;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using ST.Models.Paper;
using ST.Models.Score;
using VoiceRecorder.Audio;

namespace Plugin.Exam.Qs.ViewModel
{
    /// <summary>
    /// 单词
    /// </summary>
    public class SyncWordViewModel : QsBaseViewModel
    {
        public Paper_InfoItem PaperInfoItem;
        private SyncScoreHistoryItem HistoryItem;
        private SyncWordView WordView;
        public const string ViewName = "SyncWordViewModel";
        private const string MediaPic = ".JPG .GIF .PNG .BMP .JPEG";
        private int _itemIndex = 0;
        private int _audioPlayTimes=0;
        private int _prepareTime = 0;
        private int _answerTime = 0;
        private int _item_repet_times = 1;
        private string _syncQsContentAudio = "";
        private bool _IsSpell = false;

        private string tagUrl = "";

        public SyncWordViewModel()
        {
            tagUrl = $"UploadCurrentScoreResult_{Guid.NewGuid()}";

            Messenger.Default.Unregister<SyncScoreCallbckMessage>(this);
            Messenger.Default.Unregister<SyncScoreCallbckMessage>(this, tagUrl);
            Messenger.Default.Register<SyncScoreCallbckMessage>(this, tagUrl, (message) => OnScoreCallback(message));
        }

        /// <summary>
        /// 单词
        /// </summary>
        public SyncWordViewModel(int qsIndex, int qsCount):this()
        {
            QsIndex = qsIndex;
            QsCount = qsCount;
            _dTimer = new DispatcherTimer();
            _dTimer.Interval = TimeSpan.FromMilliseconds(100);
            _dTimer.Tick += DTimerOnTick;
        }

        /// <summary>
        /// 单词
        /// </summary>
        /// <param name="paperDetail">试题完成内容</param>
        /// <param name="item_id">小题编号  默认值:null</param>
        public SyncWordViewModel(Paper_InfoItem syncQsItem, SyncScoreHistoryItem historyItem, SyncWordView wordView, int qsIndex, int qsCount) : this(qsIndex, qsCount)
        {
            PaperInfoItem = syncQsItem;

            HistoryItem = historyItem;

            SyncTitleIndexStyle = $"{qsIndex}/{qsCount}";

            WordView = wordView;

            WordView.ScorePanel.Visibility = Visibility.Hidden;

            BindQsItemInfo();
        }

        #region << 属性 字段 >>

        private int _TxtPlayingTime;

        /// <summary>
        /// 录音 进度 时长
        /// </summary>
        public int TxtPlayingTime
        {
            get
            {
                return _TxtPlayingTime;
            }
            set
            {
                _TxtPlayingTime = value;
                RaisePropertyChanged("TxtPlayingTime");
            }
        }

        private int _TxtTotalTime;

        /// <summary>
        /// 录音总 时长
        /// </summary>
        public int TxtTotalTime
        {
            get
            {
                return _TxtTotalTime;
            }
            set
            {
                _TxtTotalTime = value;
                RaisePropertyChanged("TxtTotalTime");
            }
        }

        private string _SyncTitleIndexStyle;

        /// <summary>
        /// 单词 索引标记
        /// </summary>
        public string SyncTitleIndexStyle
        {
            get
            {
                return _SyncTitleIndexStyle;
            }
            set
            {
                _SyncTitleIndexStyle = value;
                RaisePropertyChanged("SyncTitleIndexStyle");
            }
        }

        private object _SyncQsInfoImage;
        /// <summary>
        /// 单词 图片
        /// </summary>
        public object SyncQsInfoImage
        {
            get
            {
                return _SyncQsInfoImage;
            }
            set
            {
                _SyncQsInfoImage = value;
                RaisePropertyChanged("SyncQsInfoImage");
            }
        }

        private string _SyncQsContent;
        /// <summary>
        /// 单词 
        /// </summary>
        public string SyncQsContent
        {
            get
            {
                return _SyncQsContent;
            }
            set
            {
                _SyncQsContent = value;
                RaisePropertyChanged("SyncQsContent");
            }
        }

        private string _SyncQsYbContent;
        /// <summary>
        /// 单词 音标
        /// </summary>
        public string SyncQsYbContent
        {
            get
            {
                return _SyncQsYbContent;
            }
            set
            {
                _SyncQsYbContent = value;
                RaisePropertyChanged("SyncQsYbContent");
            }
        }

        private string _SyncQsZhContent;
        /// <summary>
        ///  单词 中文
        /// </summary>
        public string SyncQsZhContent
        {
            get
            {
                return _SyncQsZhContent;
            }
            set
            {
                _SyncQsZhContent = value;
                RaisePropertyChanged("SyncQsZhContent");
            }
        }

        private string _SyncQsExEContent;
        /// <summary>
        ///  单词 英文 例句
        /// </summary>
        public string SyncQsExEContent
        {
            get
            {
                return _SyncQsExEContent;
            }
            set
            {
                _SyncQsExEContent = value;
                RaisePropertyChanged("SyncQsExEContent");
            }
        }

        private string _SyncQsExZContent;
        /// <summary>
        ///  单词 中文 例句
        /// </summary>
        public string SyncQsExZContent
        {
            get
            {
                return _SyncQsExZContent;
            }
            set
            {
                _SyncQsExZContent = value;
                RaisePropertyChanged("SyncQsExZContent");
            }
        }

        #endregion

        #region << 按钮方法 >>

        private RelayCommand<string> _CloseExamCommand;//录音 正常,打开 考试

        public RelayCommand<string> CloseExamCommand
        {
            get
            {
                return _CloseExamCommand ?? (_CloseExamCommand = new RelayCommand<string>(s =>
                {
                }));
            }
        }


        private RelayCommand<string> _PlayAudioCommand;//音频 正常,打开 考试

        public RelayCommand<string> PlayAudioCommand
        {
            get
            {
                return _PlayAudioCommand ?? (_PlayAudioCommand = new RelayCommand<string>(s =>
                {
                    if (GlobalUser.Recorder?.RecordingState == RecordingState.Recording)
                    {
                        GlobalUser.Recorder?.Stop();
                        GlobalUser.Recorder = null;

                        PlayTime = TotalTime;
                        _dTimer.Stop();
                        RecordingEnable = false;
                    }

                    if (GlobalUser.WavePlayer?.PlaybackState == PlaybackState.Playing)
                    {
                        GlobalUser.WavePlayer?.Stop();
                        GlobalUser.WavePlayer = null;
                        PlayingIconEnable = false;
                        return;
                    }

                    if (!string.IsNullOrEmpty(_syncQsContentAudio))
                    {
                        PlayAudio1(_syncQsContentAudio);
                        PlayingIconEnable = true;
                    }
                }));
            }
        }

        private RelayCommand<string> _RecordingCommand;//录音 正常,打开 考试

        public RelayCommand<string> RecordingCommand
        {
            get
            {
                return _RecordingCommand ?? (_RecordingCommand = new RelayCommand<string>(s =>
                {
                    if (GlobalUser.Recorder?.RecordingState ==  RecordingState.Recording)
                    {
                        GlobalUser.Recorder?.Stop();
                        GlobalUser.Recorder = null;
                        RecordingEnable = false;
                        TxtPlayingTime = PlayTime = TotalTime;
                        //CleanUp(); 
                        //CleanUp();
                        //UpdateAnswer();
                        return;
                    }

                    if (!string.IsNullOrEmpty(_syncQsContentAudio))
                    {
                        TotalTime = TxtTotalTime;
                        TxtPlayingTime = PlayTime = 0;
                        PlayRecorder1();
                        RecordingEnable = true;
                    }
                }));
            }
        }

        private RelayCommand<string> _NextOneCommand;//下一个

        public RelayCommand<string> NextOneCommand
        {
            get
            {
                return _NextOneCommand ?? (_NextOneCommand = new RelayCommand<string>(s =>
                {
                    Dispose();
                    Messenger.Default.Send(new ExamNextQsNavigateMessage(0, 0, 0),
                        "SyncNextQsView");
                }));
            }
        }

        #endregion

        #region << 自定义方法 >>

        /// <summary>
        /// 加载题目
        /// </summary>
        /// <param name="item_id"></param>
        private void BindQsItemInfo()
        {
            //初始化
            TxtPlayingTime = PlayTime = 0;
            TxtTotalTime = TotalTime = PaperInfoItem.items[0].item_answer_second * 10;
            _audioPlayTimes = 0;
            _item_repet_times = 1;
            _Recording = RecordState.UnRecord;

            //默认取第一题
            //PaperItems
            SyncQsContent = PaperInfoItem.items[0].item_content;
            _syncQsContentAudio = PaperInfoItem.items[0].source_content;

            WordView.QsItemContent.Children.Clear();
            var txt = new TextBlock();
            txt.Text = SyncQsContent;
            txt.TextWrapping = TextWrapping.Wrap;
            txt.FontSize = 28;
            txt.Margin = new Thickness(0, 0, 6, 0);
            //BrushConverter brushConverter = new BrushConverter();
            //Brush brush = (Brush)brushConverter.ConvertFromString("#333333");
            txt.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#353535"));
            WordView.QsItemContent.Children.Add(txt);

            try
            {
                KeyWordEx keyWord = JsonHelper.FromJson<KeyWordEx>(PaperInfoItem.items[0].item_keyword);
                SyncQsYbContent = $"[ {keyWord.yb} ]";
                SyncQsZhContent = keyWord.desc;
                SyncQsExEContent = keyWord.ex_en;
                SyncQsExZContent = keyWord.ex_zh;

                if (string.IsNullOrEmpty(keyWord.sp))
                {
                    _IsSpell = true;
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error($"单词 KeyWord 绑定异常:{PaperInfoItem.items[0].item_keyword}", ex);
            }

            if (!string.IsNullOrEmpty(PaperInfoItem.items[0].img_source_content) &&
                    MediaPic.Contains(PaperInfoItem.items[0].img_source_content.ToUpper().Split('.').LastOrDefault()))
            {
                string url = WebApiProxy.GetRedirectUrl($"{WebApiProxy.MEDIAURL}{PaperInfoItem.items[0].img_source_content}");
                SyncQsInfoImage = new BitmapImage(new Uri(url));
            }


            BindUserScoreBox();

            //BeginExam(_NextFlowType); 
        }

        private void BindUserScoreBox(SyncScoreCallbckMessage msg = null)
        {
            try
            {
                if (HistoryItem != null)
                {
                    if (HistoryItem.best != null || HistoryItem.last != null)
                    {
                        HistoryScoreItemView view0 =null;
                        if (WordView.ScoreBoxView.Children.Count == 2)
                        {
                            view0 = WordView.ScoreBoxView.Children[1] as HistoryScoreItemView;
                        }
                        if (WordView.ScoreBoxView.Children.Count == 3)
                        {
                            view0 = WordView.ScoreBoxView.Children[2] as HistoryScoreItemView;
                        }

                        WordView.ScoreBoxView.Children.Clear();
                        ScoreRoot score = JsonHelper.FromJson<ScoreRoot>(HistoryItem.best.score_result.Replace("\"params\"", "\"param\""));

                        var view = new HistoryScoreItemView();
                        //var viewModel = new HistoryScoreItemViewModel(this, "历史最高:",
                        //    $"{Convert.ToInt32(HistoryItem.best.exam_score)} 分", HistoryItem.best.user_answer, HistoryItem.best.score_result);
                        var viewModel = new HistoryScoreItemViewModel(this, "历史最高:", HistoryItem.best);
                        view.DataContext = viewModel;
                        view.SetValue(Grid.ColumnProperty, 0); //设置按钮所在Grid控件的列
                        view.SetValue(Grid.RowProperty, 0);
                        //grid.Children.Add(view);
                        WordView.ScoreBoxView.Children.Add(view);

                        if (view0 != null)
                        {
                            view0.TxtTitleName.Text = "上次得分";
                            view0.SetValue(Grid.ColumnProperty, 1); //设置按钮所在Grid控件的列
                            view0.SetValue(Grid.RowProperty, 0);
                            view0.Margin = new Thickness(0);
                            WordView.ScoreBoxView.Children.Add(view0);
                            WordView.ScoreBoxView.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            score = JsonHelper.FromJson<ScoreRoot>(HistoryItem.last.score_result.Replace("\"params\"", "\"param\""));
                            var view1 = new HistoryScoreItemView();
                            //var viewModel1 = new HistoryScoreItemViewModel(this, "上次得分:",
                            //    $"{Convert.ToInt32(HistoryItem.last.exam_score)} 分", HistoryItem.last.user_answer, HistoryItem.last.score_result);
                            var viewModel1 = new HistoryScoreItemViewModel(this, "上次得分:", HistoryItem.last);
                            view1.DataContext = viewModel1;
                            view1.SetValue(Grid.ColumnProperty, 1); //设置按钮所在Grid控件的列
                            view1.SetValue(Grid.RowProperty, 0);
                            view1.Margin = new Thickness(0);
                            //grid.Children.Add(view1); 
                            WordView.ScoreBoxView.Children.Add(view1);
                            WordView.ScoreBoxView.Visibility = Visibility.Visible;
                        }
                    }
                }

                //本次答题记录
                if (msg != null)
                {
                    if (msg.ModelTarget != tagUrl) return;
                    
                    if (msg.EType != EngineType.WORD) return;

                    if (msg.ScoreResult?.result == null) return;

                    var view2 = new HistoryScoreItemView();
                    //var viewModel2 = new HistoryScoreItemViewModel(this, "本次得分:",
                    //    $"{Convert.ToInt32(msg.ScoreResult.result.overall)} 分", msg.ScoreResult.audioUrl, msg.ScoreResult.result.words.ToString());
                    var viewModel2 = new HistoryScoreItemViewModel(this, "本次得分:", msg.ScoreResult);
                    view2.DataContext = viewModel2;
                    view2.SetValue(Grid.ColumnProperty, 2); //设置按钮所在Grid控件的列
                    view2.SetValue(Grid.RowProperty, 0);
                    view2.Margin = new Thickness(20, 0, 10, 0);
                    //grid.Children.Add(view1); 
                    WordView.ScoreBoxView.Children.Add(view2);

                    BindUserScore(Convert.ToInt32(msg.ScoreResult.result.overall));

                    if(_IsSpell)
                        BindUserAnswerContent(msg.ScoreResult.result.words.ToString());
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Word 绑定历史记录失败", ex);
            }
        }

        public override void ShowPlayingResult(string result)
        {
            ScoreRoot score = JsonHelper.FromJson<ScoreRoot>(result.Replace("\"params\"", "\"param\""));
            BindUserAnswerContent(score.result.words.ToString());


            BindUserScore(Convert.ToInt32(score.result.overall));

            if (_IsSpell)
                BindUserAnswerContent(score.result.words.ToString());
        }

        private void DTimerOnTick(object sender, EventArgs e)
        {

            if (!GlobalUser.DoAnswer)
            {
                _dTimer?.Stop();
                _dTimer = null;
                return;
            }

            if (PlayTime >= TotalTime)
            {
                CleanUp();
                UpdateAnswer();
            }
            else
            {
                PlayTime++;
                TxtPlayingTime = PlayTime; 
            }
        }


        #endregion

        #region << 考试 >>

        public override void BeginExam(ExamFlowType nextFlowType)
        {
        }

        public override void OnScoreCallback(SyncScoreCallbckMessage msg)
        {
            var ml = new HistoryItemModel()
            {
                exam_attend_id = GlobalUser.AttendPaperItemId,
                item_id = PaperInfoItem.items[0].item_id,
                token = GlobalUser.USER.Token
            };
            var result1 = WebApiProxy.GetHtmlRespInfo(ml, ApiType.GetSyncHistory, null, "get");

            SyncScoreHistoryItem historyItem = new SyncScoreHistoryItem();
            try
            {
                if (result1?.retCode == 0)
                {
                    HistoryItem = JsonHelper.FromJson<SyncScoreHistoryItem>(result1.retData.ToString());
                }
            }
            catch (Exception e)
            {
                Log4NetHelper.Error($"抓取历史 单题答题记录 异常", e);
            }

            BindUserScoreBox(msg);
        }

        private void BindUserScore(int score)
        {
            //score=123
            int a, b, c = 0;
            a = score / 100; //a=1,score=123
            score = score % 100;
            b = score / 10; //b=2,score=23
            score = score % 10;
            c = score / 1; // c=1,score=3

            WordView.ScorePanel.Visibility = Visibility.Hidden;
            WordView.ScoreInfopanel.Children.Clear();

            Image img;
            if (a != 0)
            {
                img = GetScoreImage(a);
                img.Width = 36;
                img.Height = 56;
                WordView.ScoreInfopanel.Children.Add(img);
            }

            if (b != 0 || (a > 0 && b == 0))
            {
                img = GetScoreImage(b);
                img.Width = 36;
                img.Height = 56;
                WordView.ScoreInfopanel.Children.Add(img);
            }

            if (c >= 0)
            {
                img = GetScoreImage(c);
                img.Width = 36;
                img.Height = 56;
                WordView.ScoreInfopanel.Children.Add(img);
            }

            if (a + b + c >= 0)
            {
                WordView.ScorePanel.Visibility = Visibility.Visible;
            }

        }

        /// <summary>
        /// 绑定 说话内容 彩色评价
        /// </summary>
        /// <param name="answerText"></param>
        private void BindUserAnswerContent(string answerText)
        {
            try
            {
                List<WordsItem> wordsList = JsonHelper.FromJson<List<WordsItem>>(answerText);
                WordView.QsItemContent.Children.Clear();

                WrapPanel wp = new WrapPanel();
                wp.Margin = new Thickness(0, 0, 6, 0);

                TextBlock txtItem = new TextBlock();

                //char[] charKey = { '|', '#', '$', '^', '*', '(',')', '+', '{', '}', '?', '[', ']', '.', '\\' };
                //splitKey = Regex.Escape(splitKey);
                for (int i = 0; i < wordsList.Count; i++)
                {
                    for (int j = 0; j < wordsList[i].phonics.Count; j++)
                    {
                        txtItem = new TextBlock();
                        txtItem.Text = Regex.Replace(wordsList[i].phonics[j].spell, "[\n]|[\t]|[\r]", "",
                            RegexOptions.IgnoreCase);

                        //    差：f44116
                        //    中：ff8414
                        //    良：1394fa
                        //    优：41b612
                        if (wordsList[i].phonics[j].overall > 75)
                        {
                            txtItem.Foreground =
                                new SolidColorBrush((Color) ColorConverter.ConvertFromString("#41B612"));
                        }
                        else if (wordsList[i].phonics[j].overall > 60 && wordsList[i].phonics[j].overall <= 75)
                        {
                            txtItem.Foreground =
                                new SolidColorBrush((Color) ColorConverter.ConvertFromString("#1394FA"));
                        }
                        else if (wordsList[i].phonics[j].overall > 30 && wordsList[i].phonics[j].overall <= 60)
                        {
                            txtItem.Foreground =
                                new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FF8414"));
                        }
                        else
                        {
                            txtItem.Foreground =
                                new SolidColorBrush((Color) ColorConverter.ConvertFromString("#F44116"));
                        }

                        txtItem.FontSize = 28;
                        txtItem.Margin = new Thickness(0, 0, 1, 0);
                        wp.Children.Add(txtItem);
                    }
                }

                WordView.QsItemContent.Children.Add(wp);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(ex);
            }
        }

        private Image GetScoreImage(int score)
        {
            Image img = new Image();
            img.Height = 58;
            switch (score)
            {
                case 0:
                    img.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources._0);
                    break;
                case 1:
                    img.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources._1);
                    break;
                case 2:
                    img.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources._2);
                    break;
                case 3:
                    img.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources._3);
                    break;
                case 4:
                    img.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources._4);
                    break;
                case 5:
                    img.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources._5);
                    break;
                case 6:
                    img.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources._6);
                    break;
                case 7:
                    img.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources._7);
                    break;
                case 8:
                    img.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources._8);
                    break;
                case 9:
                    img.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources._9);
                    break;
                default:
                    img.Source = ConvertHelper.ChangeBitmapToImageSource(Properties.Resources._0);
                    break;
            }
            return img;
        }

        private void UpdateAnswer()
        {
            var paperItemsItem = PaperInfoItem.items[0];
            float myScore = 0f;
            string myAnswer = "";

            SubmitRecordModel model = new SubmitRecordModel
            {
                exam_attend_id = GlobalUser.AttendPaperItemId,
                item_id = paperItemsItem.item_id,
                item_score = 100,
                exam_score = myScore,
                user_answer = myAnswer,
                item_answer = paperItemsItem.answers[0].answer_content.ToDBC().Zh2En(),
                item_answer_type = 2,
                precision = 1, //评分精度
                qs_id = PaperInfoItem.qs_id,
                qs_type = PaperInfoItem.qs_type.ToString(),
                item_no = paperItemsItem.item_no,
                score_type = 2
            };

            Messenger.Default.Send(
                new ExamScoreNavigateMessage(tagUrl, model, _waveFileName, EngineQsType.NOTOPEN, EngineType.WORD),
                "SyncScoreNavi");
        }

        public void Dispose()
        {
            Messenger.Default.Unregister<SyncScoreCallbckMessage>(this);
            Messenger.Default.Unregister<SyncScoreCallbckMessage>(this, tagUrl);
            
            CleanUp();

            GC.Collect(0);
        }

        #endregion
    }
}

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
using Image = System.Windows.Controls.Image;

namespace Plugin.Exam.Qs.ViewModel
{
    /// <summary>
    /// 句子
    /// </summary>
    public class SyncPredViewModel : QsBaseViewModel
    {
        public Paper_InfoItem PaperInfoItem;
        public StackPanel _QsItemContent;
        private SyncScoreHistoryItem HistoryItem;
        private SyncPredView PredView;
        public const string ViewName = "SyncPredViewModel";
        private const string MediaPic = ".JPG .GIF .PNG .BMP .JPEG";
        private int _itemIndex = 0;
        private int _audioPlayTimes=0;
        private int _prepareTime = 0;
        private int _answerTime = 0;
        private int _item_repet_times = 1;
        private string _syncQsContentAudio = "";


        private string tagUrl = "";

        public SyncPredViewModel()
        {
            tagUrl = $"UploadCurrentScoreResult_{Guid.NewGuid()}";

            Messenger.Default.Unregister<SyncScoreCallbckMessage>(this);
            Messenger.Default.Unregister<SyncScoreCallbckMessage>(this, tagUrl);
            Messenger.Default.Register<SyncScoreCallbckMessage>(this, tagUrl, (message) => OnScoreCallback(message));
        }

        /// <summary>
        /// 单词
        /// </summary>
        public SyncPredViewModel(int qsIndex, int qsCount):this()
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
        public SyncPredViewModel(Paper_InfoItem syncQsItem, SyncScoreHistoryItem historyItem, SyncPredView predView, int qsIndex, int qsCount) : this(qsIndex, qsCount)
        {
            PaperInfoItem = syncQsItem;

            HistoryItem = historyItem;

            SyncTitleIndexStyle = $"{qsIndex}/{qsCount}";

            PredView = predView;
            PredView.FiveStarBoxBorder.Visibility = Visibility.Hidden;
            PredView.ScoreBoxView.Visibility = Visibility.Hidden;
            PredView.ScorePanel.Visibility = Visibility.Hidden;

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
        /// 句子 
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

        private string _SyncQsYsContent;
        /// <summary>
        /// 语速
        /// </summary>
        public string SyncQsYsContent
        {
            get
            {
                return _SyncQsYsContent;
            }
            set
            {
                _SyncQsYsContent = value;
                RaisePropertyChanged("SyncQsYsContent");
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


        private RelayCommand<string> _PlayAudioCommand;//录音 正常,打开 考试

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
                    if (GlobalUser.Recorder?.RecordingState == RecordingState.Recording)
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

            PredView.QsItemContent.Children.Clear();

            var txt = new TextBlock();
            txt.Text = SyncQsContent;
            txt.HorizontalAlignment = HorizontalAlignment.Center;
            txt.TextWrapping = TextWrapping.Wrap;
            txt.FontSize = 28;
            txt.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#353535"));
            PredView.QsItemContent.Children.Add(txt);

            if (!string.IsNullOrEmpty(PaperInfoItem.items[0].img_source_content) &&
                MediaPic.Contains(PaperInfoItem.items[0].img_source_content.ToUpper().Split('.').LastOrDefault()))
            {
                string url = WebApiProxy.GetRedirectUrl($"{WebApiProxy.MEDIAURL}{PaperInfoItem.items[0].img_source_content}");
                SyncQsInfoImage = new BitmapImage(new Uri(url));
            }

            BindUserScoreBox();
        }

        private void BindUserScoreBox(SyncScoreCallbckMessage msg = null)
        {
            try
            {
                if (HistoryItem != null)
                {
                    if (HistoryItem.best != null || HistoryItem.last != null)
                    {
                        HistoryScoreItemView view0 = null;
                        if (PredView.ScoreBoxView.Children.Count == 2)
                        {
                            view0 = PredView.ScoreBoxView.Children[1] as HistoryScoreItemView;
                        }
                        if (PredView.ScoreBoxView.Children.Count == 3)
                        {
                            view0 = PredView.ScoreBoxView.Children[2] as HistoryScoreItemView;
                        }

                        PredView.ScoreBoxView.Children.Clear();
                        ScoreRoot score = JsonHelper.FromJson<ScoreRoot>(HistoryItem.best.score_result.Replace("\"params\"", "\"param\""));

                        var view = new HistoryScoreItemView();
                        //var viewModel = new HistoryScoreItemViewModel(this, "历史最高:",
                        //    $"{Convert.ToInt32(HistoryItem.best.exam_score)} 分", HistoryItem.best.user_answer, score.result.words.ToString());
                        var viewModel = new HistoryScoreItemViewModel(this, "历史最高:", HistoryItem.best);
                        view.DataContext = viewModel;
                        view.SetValue(Grid.ColumnProperty, 0); //设置按钮所在Grid控件的列
                        view.SetValue(Grid.RowProperty, 0);
                        //grid.Children.Add(view);
                        PredView.ScoreBoxView.Children.Add(view);


                        if (view0 != null)
                        {
                            view0.SetValue(Grid.ColumnProperty, 1); //设置按钮所在Grid控件的列
                            view0.SetValue(Grid.RowProperty, 0);
                            view0.Margin = new Thickness(0);
                            view0.TxtTitleName.Text = "上次得分:";
                            PredView.ScoreBoxView.Children.Add(view0);
                            PredView.ScoreBoxView.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            score = JsonHelper.FromJson<ScoreRoot>(HistoryItem.last.score_result.Replace("\"params\"", "\"param\""));
                            var view1 = new HistoryScoreItemView();
                            //var viewModel1 = new HistoryScoreItemViewModel(this, "上次得分:",
                            //    $"{Convert.ToInt32(HistoryItem.last.exam_score)} 分", HistoryItem.last.user_answer, score.result.words.ToString());
                            var viewModel1 = new HistoryScoreItemViewModel(this, "上次得分:", HistoryItem.last);
                            view1.DataContext = viewModel1;
                            view1.SetValue(Grid.ColumnProperty, 1); //设置按钮所在Grid控件的列
                            view1.SetValue(Grid.RowProperty, 0);
                            view1.Margin = new Thickness(0);
                            //grid.Children.Add(view1); 
                            PredView.ScoreBoxView.Children.Add(view1);
                            PredView.ScoreBoxView.Visibility = Visibility.Visible;
                        }
                    }
                }

                //本次答题记录
                if (msg != null)
                {
                    if (msg.ModelTarget != tagUrl) return;

                    if (msg.EType != EngineType.SENT) return;

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
                    PredView.ScoreBoxView.Children.Add(view2);
                    PredView.ScoreBoxView.Visibility = Visibility.Visible;

                    BindUserAnswerContent(msg.ScoreResult.result.words.ToString());

                    BindFiveStarUc(msg.ScoreResult);

                    BindUserScore(Convert.ToInt32(msg.ScoreResult.result.overall));
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("Word 绑定历史记录失败", ex);
            }
        }

        public override void ShowPlayingResult(string result)
        {
            //BindUserAnswerContent(result);

            ScoreRoot score = JsonHelper.FromJson<ScoreRoot>(result.Replace("\"params\"", "\"param\""));
            //BindUserAnswerContent(score.result.words.ToString());


            BindFiveStarUc(score);
            BindUserScore(Convert.ToInt32(score.result.overall));
            BindUserAnswerContent(score.result.words.ToString());
        }

        /// <summary>
        /// 绑定 评测评价
        /// </summary>
        /// <param name="msg"></param>
        private void BindFiveStarUc(ScoreRoot msg)
        {
            try
            {
                PredView.FiveStarBoxBorder.Visibility = Visibility.Hidden;
                SyncQsYsContent = "";
                SyncQsYsContent = $"语速：{msg.result.speed}词/分钟";

                // 流畅度  准确度 完整度
                var view10 = new EvaluationScoreItemView();
                var viewModel10 =
                    new EvaluationScoreItemViewModel("流利度", Convert.ToSingle(msg.result.fluency));
                view10.DataContext = viewModel10;
                view10.SetValue(Grid.ColumnProperty, 0); //设置按钮所在Grid控件的列
                view10.SetValue(Grid.RowProperty, 0);
                PredView.FiveStarBox.Children.Add(view10);
                var view11 = new EvaluationScoreItemView();
                var viewModel11 = new EvaluationScoreItemViewModel("准确度",
                    Convert.ToSingle(msg.result.pronunciation));
                view11.DataContext = viewModel11;
                view11.SetValue(Grid.ColumnProperty, 1); //设置按钮所在Grid控件的列
                view11.SetValue(Grid.RowProperty, 0);
                PredView.FiveStarBox.Children.Add(view11);
                var view12 = new EvaluationScoreItemView();
                var viewModel12 = new EvaluationScoreItemViewModel("完整度",
                    Convert.ToSingle(msg.result.integrity));
                view12.DataContext = viewModel12;
                view12.SetValue(Grid.ColumnProperty, 2); //设置按钮所在Grid控件的列
                view12.SetValue(Grid.RowProperty, 0);
                PredView.FiveStarBox.Children.Add(view12);
                PredView.FiveStarBox.Visibility = Visibility.Visible;
                PredView.FiveStarBoxBorder.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("绑定评分评价异常 Pred", ex);
            }
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

        /// <summary>
        /// 绑定 说话内容 彩色评价
        /// </summary>
        /// <param name="answerText"></param>
        private void BindUserAnswerContent(string answerText)
        {
            try
            {
                List<WordsItem> sentList = JsonHelper.FromJson<List<WordsItem>>(answerText);
                //List<SentencesItem> sentList = JsonHelper.FromJson<List<SentencesItem>>(answerText);
                PredView.QsItemContent.Children.Clear();

                //WrapPanel wp = new WrapPanel();
                //wp.Margin = new Thickness(6);
                //wp.HorizontalAlignment = HorizontalAlignment.Center;
                //var txt = new TextBlock();
                //txt.Text = SyncQsContent;
                //txt.FontSize = 24; 
                //WrapPanel wp = new WrapPanel();

                TextBlock txtItem = new TextBlock();

                //char[] charKey = { '|', '#', '$', '^', '*', '(',')', '+', '{', '}', '?', '[', ']', '.', '\\' };
                //splitKey = Regex.Escape(splitKey);
                for (int i = 0; i < sentList.Count; i++)
                {
                    txtItem = new TextBlock();
                    txtItem.Text = Regex.Replace(sentList[i].word, "[\n]|[\t]|[\r]", "",
                        RegexOptions.IgnoreCase);

                    //    差：f44116
                    //    中：ff8414
                    //    良：1394fa
                    //    优：41b612
                    if (sentList[i].scores.overall > 75)
                    {
                        txtItem.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#41B612"));
                    }
                    else if (sentList[i].scores.overall > 60 && sentList[i].scores.overall <= 75)
                    {
                        txtItem.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#1394FA"));
                    }
                    else if (sentList[i].scores.overall > 30 && sentList[i].scores.overall <= 60)
                    {
                        txtItem.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#FF8414"));
                    }
                    else
                    {
                        txtItem.Foreground = new SolidColorBrush((Color) ColorConverter.ConvertFromString("#F44116"));
                    }

                    txtItem.FontSize = 28;
                    txtItem.Margin = new Thickness(0, 0, 6, 0);
                    PredView.QsItemContent.Children.Add(txtItem);
                }

                //PredView.QsItemContent.Children.Add(wp);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(ex);
            }
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

            PredView.ScorePanel.Visibility = Visibility.Hidden;
            PredView.ScoreInfopanel.Children.Clear();

            Image img;
            if (a != 0)
            {
                img = GetScoreImage(a);
                img.Width = 36;
                img.Height = 56;
                PredView.ScoreInfopanel.Children.Add(img);
            }

            if (b != 0 || (a > 0 && b == 0))
            {
                img = GetScoreImage(b);
                img.Width = 36;
                img.Height = 56;
                PredView.ScoreInfopanel.Children.Add(img);
            }

            if (c >= 0)
            {
                img = GetScoreImage(c);
                img.Width = 36;
                img.Height = 56;
                PredView.ScoreInfopanel.Children.Add(img);
            }

            if (a + b + c >= 0)
            {
                PredView.ScorePanel.Visibility = Visibility.Visible;
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
                new ExamScoreNavigateMessage(tagUrl, model, _waveFileName, EngineQsType.NOTOPEN, EngineType.SENT),
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

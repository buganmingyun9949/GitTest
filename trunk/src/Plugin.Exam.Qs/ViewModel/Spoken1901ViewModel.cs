using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Framework.Engine;
using Framework.Logging;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using NAudio.Wave;
using Plugin.Exam.Qs.Common;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Api;
using ST.Models.Paper;

namespace Plugin.Exam.Qs.ViewModel
{
    /// <summary>
    /// 填空并转述
    /// </summary>
    public class Spoken1901ViewModel : QsBaseViewModel
    {
        public new QuestionType QsType = QuestionType.HearingSingleChoice;
        public const string ViewName = "Spoken1901ViewModel";
        private const string MediaPic = ".JPG .GIF .PNG .BMP .JPEG";
        private int _itemIndex = 0;
        private int _audioPlayTimes=0;
        private int _prepareTime = 0;
        private int _answerTime = 0;
        private int _item_repet_times = 1;

        private List<info_content> infoList;

        /// <summary>
        /// 填空并转述
        /// </summary>
        public Spoken1901ViewModel(int qsIndex, int itemIndex)
        {
            QsIndex = qsIndex;
            _itemIndex = itemIndex;
            _dTimer = new DispatcherTimer();
            _dTimer.Interval = TimeSpan.FromSeconds(1);
            _dTimer.Tick += DTimerOnTick;
        }

        /// <summary>
        /// 填空并转述
        /// </summary>
        /// <param name="paperDetail">试题完成内容</param>
        /// <param name="item_id">小题编号  默认值:null</param>
        public Spoken1901ViewModel(Paper_DetailItem paperDetail, Button btn, int qsIndex, int itemIndex, string item_id = null) : this(qsIndex, itemIndex)
          {
            infoList = new List<info_content>();

            PaperDetailItem = paperDetail;

            _BtnSkipNext = btn;
            _BtnSkipNext.IsEnabled = false; 
            _BtnSkipNext.Click += BtnSkipNext_Click;
            
            QsID = paperDetail.qs_id;
            QsTitle = paperDetail.qs_title;
            QsTitleContent = paperDetail.qs_content;
            QsTitleAudio = paperDetail.source_content;
            PaperItems = paperDetail.info.OrderBy(o => o.info_sort).ToList();

            ShowTextBox = Visibility.Collapsed;
            EnableTextBox = true;

            BindQsItemInfo(item_id);
        }

        #region << 属性 字段 >>

        private string qsItemContent;

        /// <summary>
        /// 选择题内容
        /// </summary>
        public string QsItemContent
        {
            get
            {
                return qsItemContent;
            }
            set
            {
                qsItemContent = value;
                RaisePropertyChanged("QsItemContent");
            }
        }

        private object _QsInfoImage;
        /// <summary>
        /// 音频 播放按钮
        /// </summary>
        public object QsInfoImage
        {
            get
            {
                return _QsInfoImage;
            }
            set
            {
                _QsInfoImage = value;
                RaisePropertyChanged("QsInfoImage");
            }
        }

        private Visibility _ShowTextBox;

        /// <summary>
        /// 填空 是否显示
        /// </summary>
        public Visibility ShowTextBox
        {
            get
            {
                return _ShowTextBox;
            }
            set
            {
                _ShowTextBox = value;
                RaisePropertyChanged("ShowTextBox");
            }
        }

        private bool _EnableTextBox;

        /// <summary>
        /// 填空是否可用
        /// </summary>
        public bool EnableTextBox
        {
            get
            {
                return _EnableTextBox;
            }
            set
            {
                _EnableTextBox = value;
                RaisePropertyChanged("EnableTextBox");
            }
        }

        private string _TxtAnswer1;

        /// <summary>
        /// 填空1
        /// </summary>
        public string TxtAnswer1
        {
            get
            {
                return _TxtAnswer1;
            }
            set
            {
                _TxtAnswer1 = value;
                RaisePropertyChanged("TxtAnswer1");
            }
        }

        private string _TxtAnswer2;

        /// <summary>
        /// 填空2
        /// </summary>
        public string TxtAnswer2
        {
            get
            {
                return _TxtAnswer2;
            }
            set
            {
                _TxtAnswer2 = value;
                RaisePropertyChanged("TxtAnswer2");
            }
        }

        private string _TxtAnswer3;

        /// <summary>
        /// 填空3
        /// </summary>
        public string TxtAnswer3
        {
            get
            {
                return _TxtAnswer3;
            }
            set
            {
                _TxtAnswer3 = value;
                RaisePropertyChanged("TxtAnswer3");
            }
        }

        private string _TxtAnswer4;

        /// <summary>
        /// 填空4
        /// </summary>
        public string TxtAnswer4
        {
            get
            {
                return _TxtAnswer4;
            }
            set
            {
                _TxtAnswer4 = value;
                RaisePropertyChanged("TxtAnswer4");
            }
        }

        private string _TxtAnswer5;

        /// <summary>
        /// 填空5
        /// </summary>
        public string TxtAnswer5
        {
            get
            {
                return _TxtAnswer5;
            }
            set
            {
                _TxtAnswer5 = value;
                RaisePropertyChanged("TxtAnswer5");
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


        #endregion

        #region << 自定义方法 >>

        /// <summary>
        /// 加载题目
        /// </summary>
        /// <param name="item_id"></param>
        private void BindQsItemInfo(string item_id = null)
        {

            if (_itemIndex >= PaperItems.Count)
            {
                _dTimer.Stop();
                _dTimer = null;
                //进入 下一道大题
                NextQsView();
            }
            else
            {
                //默认取第一题
                //PaperItems
                //QsItemContent = PaperItems[0].items[_itemIndex].item_content;

                infoList.AddRange(PaperItems[0].info_content.FromJsonTo<List<info_content>>());

                infoList.AddRange(PaperItems[1].info_content.FromJsonTo<List<info_content>>());

                QsTitleContent = infoList[0].text;


                if (!string.IsNullOrEmpty(PaperItems[0].info_content_img) &&
                    MediaPic.Contains(PaperItems[0].info_content_img.ToUpper().Split('.').LastOrDefault()))
                {
                    string url = WebApiProxy.GetRedirectUrl($"{WebApiProxy.MEDIAURL}{PaperItems[0].info_content_img}");
                    QsInfoImage = new BitmapImage(new Uri(url));
                }

                //初始化
                PlayTime = 0;
                TotalTime = 0;
                _audioPlayTimes = 0;
                _item_repet_times = 1;
                _Recording = RecordState.UnRecord;

                BeginExam(_NextFlowType);
            }
        }

        private void DTimerOnTick(object sender, EventArgs e)
        {
            if (_BtnSkipNext != null&& !_BtnSkipNext.IsEnabled)
                _BtnSkipNext.IsEnabled = (PlayTime >= 1);

            if (!GlobalUser.DoAnswer)
            {
                _dTimer?.Stop();
                _dTimer= null;
                return;
            }

            if (PlayTime >= TotalTime)
            {
                if (_Recording == RecordState.UnRecord)
                {
                    PlayTime = TotalTime;
                    CleanUp();
                    BeginExam(_NextFlowType);
                }
                else if (_Recording == RecordState.StopRecord)
                {
                    PlayTime = TotalTime;
                    BeginExam(_NextFlowType);
                }
            }
            else
            {
                PlayTime++;

                SendProgress();
            }
        }


        #endregion

        #region << 考试 >>

        public override void BeginExam(ExamFlowType nextFlowType)
        {
            if (!GlobalUser.DoAnswer)
            {

                CleanUp();

                return;
            }
            //Paper_ItemsItem itemInfo = PaperItems[0].items[_itemIndex];
            
            switch (nextFlowType)
            {
                case ExamFlowType.TitleAudio:
                    //1.播放大题题干
                    PromptCommandText = "听指令";
                    if (string.IsNullOrEmpty(QsTitleAudio))
                    {
                        _NextFlowType = ExamFlowType.PromptAudio;
                        BeginExam(_NextFlowType);
                        break;
                    }
                    _NextFlowType = ExamFlowType.PromptAudio;
                    PlayAudio(QsTitleAudio);
                    break;

                case ExamFlowType.PromptAudio:
                    //2.播放大题题干 提示音频,
                    PromptCommandText = "听指令";
                    if (string.IsNullOrEmpty(infoList[0].audio))
                    {
                        _NextFlowType = ExamFlowType.PromptAudio1;
                        BeginExam(_NextFlowType);
                        break;
                    }

                    _NextFlowType = ExamFlowType.PromptAudio1;
                    QsTitleContent = infoList[0].text;
                    PlayAudio(infoList[0].audio);
                    break;
                case ExamFlowType.PromptAudio1:
                    //2.播放大题题干 提示音频,
                    PromptCommandText = "听指令";
                    if (string.IsNullOrEmpty(infoList[1].audio))
                    {
                        _NextFlowType = ExamFlowType.PrepareTime;
                        BeginExam(_NextFlowType);
                        break;
                    }

                    _NextFlowType = ExamFlowType.PrepareTime;
                    QsTitleContent = infoList[1].text;
                    PlayAudio(infoList[1].audio);
                    break;
                case ExamFlowType.PrepareTime:
                    //3.阅读题目
                    PromptCommandText = "请看题";
                    TotalTime = PaperItems[0].items[0].item_prepare_second;
                    PlayTime = 0;
                    SendProgress();
                    _NextFlowType = ExamFlowType.QuestionAudio;
                    _dTimer.Start();
                    break;

                case ExamFlowType.QuestionAudio:
                    //3.播放 题目音频
                    PromptCommandText = "请听录音";
                    if (string.IsNullOrEmpty(PaperItems[0].info_content_source_content))
                    {
                        _NextFlowType = ExamFlowType.PromptAudio2;
                        BeginExam(_NextFlowType);
                        break;
                    }

                    PlayAudio(PaperItems[0].info_content_source_content);
                    if (PaperItems[0].info_repet_times > 1 &&
                        PaperItems[0].info_repet_times > _item_repet_times)
                    {
                        Thread.SpinWait(2 * 1000);
                        _item_repet_times++;
                        _NextFlowType = ExamFlowType.QuestionAudio;
                    }
                    else
                        _NextFlowType = ExamFlowType.PromptAudio2;

                    break;

                case ExamFlowType.PromptAudio2:
                    ShowTextBox = Visibility.Visible;
                    //2.播放大题题干 提示音频,
                    PromptCommandText = "听指令";
                    if (string.IsNullOrEmpty(infoList[2].audio))
                    {
                        _NextFlowType = ExamFlowType.AnswerTime;
                        BeginExam(_NextFlowType);
                        break;
                    }

                    _NextFlowType = ExamFlowType.AnswerTime;
                    QsTitleContent = infoList[2].text;
                    PlayAudio(infoList[2].audio);
                    break;
                case ExamFlowType.AnswerTime:
                    //3.填空
                    PromptCommandText = "开始答题";
                    _answerTime = TotalTime = PaperItems[0].items.Sum(s => s.item_answer_second);
                    PlayTime = 0;
                    _NextFlowType = ExamFlowType.PromptAudio3;
                    _dTimer.Start();
                    break;

                case ExamFlowType.PromptAudio3:
                    EnableTextBox = false;
                    UpdateTextAnswer();//填空提交
                    //2.播放大题题干 提示音频,
                    PromptCommandText = "听指令";
                    if (string.IsNullOrEmpty(infoList[3].audio))
                    {
                        _NextFlowType = ExamFlowType.Question1Audio;
                        BeginExam(_NextFlowType);
                        break;
                    }

                    _NextFlowType = ExamFlowType.Question1Audio;
                    QsTitleContent = infoList[3].text;
                    PlayAudio(infoList[3].audio);
                    break;

                case ExamFlowType.Question1Audio:
                    //3.播放 题目音频
                    PromptCommandText = "请听录音";
                    if (string.IsNullOrEmpty(PaperItems[1].info_content_source_content))
                    {
                        _NextFlowType = ExamFlowType.PromptAudio4;
                        BeginExam(_NextFlowType);
                        break;
                    }

                    _NextFlowType = ExamFlowType.PromptAudio4;
                    PlayAudio(PaperItems[0].info_content_source_content);
                    break;

                case ExamFlowType.PromptAudio4:
                    //2.播放大题题干 提示音频,
                    PromptCommandText = "听指令";
                    if (string.IsNullOrEmpty(infoList[4].audio))
                    {
                        _NextFlowType = ExamFlowType.PrepareTime2;
                        BeginExam(_NextFlowType);
                        break;
                    }

                    _NextFlowType = ExamFlowType.PrepareTime2;
                    QsTitleContent = infoList[4].text;
                    PlayAudio(infoList[4].audio);
                    break;

                case ExamFlowType.PrepareTime2:
                    //3.阅读题目
                    PromptCommandText = "请看题";
                    TotalTime = PaperItems[1].items[0].item_prepare_second;
                    PlayTime = 0;
                    SendProgress();
                    _NextFlowType = ExamFlowType.PromptAudio5;
                    _dTimer.Start();
                    break;

                case ExamFlowType.PromptAudio5:
                    //2.播放大题题干 提示音频,
                    PromptCommandText = "听指令";
                    if (string.IsNullOrEmpty(infoList[5].audio))
                    {
                        _NextFlowType = ExamFlowType.RecordTime;
                        BeginExam(_NextFlowType);
                        break;
                    }

                    _NextFlowType = ExamFlowType.RecordTime;
                    QsTitleContent = infoList[5].text;
                    PlayAudio(infoList[5].audio);
                    break;

                case ExamFlowType.StartRecordAudio:
                    //3.阅读题目
                    PromptCommandText = "准备录音";
                    _NextFlowType = ExamFlowType.RecordTime;
                    //PlayAudio(PaperItems[_itemIndex].source_content);
                    PlaySysAudio(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "startrecord.mp3"));
                    break;
                case ExamFlowType.RecordTime:
                    //3.阅读题目
                    //PromptCommandText = "开始录音";
                    //TotalTime = itemInfo.item_answer_second;
                    //PlayTime = 0;
                    //_NextFlowType = ExamFlowType.StopRecordAudio;
                    //_Recording = RecordState.Recording;
                    //PlayRecorder();
                    PromptCommandText = "开始答题";
                    TotalTime = PaperItems[1].items[0].item_answer_second;
                    PlayTime = 0;
                    _NextFlowType = ExamFlowType.StopRecordAudio;
                    _Recording = RecordState.Recording;
                    PlayRecorderWithSysAduio("startrecord");
                    break;
                case ExamFlowType.StopRecordAudio:
                    //3.阅读题目
                    PromptCommandText = "停止录音";
                    _NextFlowType = ExamFlowType.NextQs;
                    //PlayAudio(PaperItems[_itemIndex].source_content);
                    PlaySysAudio(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "stoprecord.mp3"));
                    break;
                //case ExamFlowType.AnswerTime:
                //    //3.阅读题目
                //    PromptCommandText = "开始答题";
                //    TotalTime = itemInfo.item_answer_second;
                //    PlayTime = 0;
                //    _NextFlowType = ExamFlowType.NextQs;
                //    _Recording = RecordState.Recording;
                //    PlayRecorderWithSysAduio("startrecord");
                //    break;
                case ExamFlowType.NextQs:
                    //记录答案
                    UpdateAnswer();
                    //3.下一题
                    _itemIndex = 2;
                    _NextFlowType = ExamFlowType.PrepareTime;
                    BindQsItemInfo();
                    break;
            }

        }

        private void UpdateTextAnswer()
        { 
            // 异步请求，防止界面假死
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                List<float> myScores = new List<float>();
                List<string> userAnswers = new List<string>();
                userAnswers.Add(TxtAnswer1);
                userAnswers.Add(TxtAnswer2);
                userAnswers.Add(TxtAnswer3);
                userAnswers.Add(TxtAnswer4);
                userAnswers.Add(TxtAnswer5);

                if (PaperItems[0].items[0].answers[0].answer_content.Trim().Split('|').Contains(TxtAnswer1?.Trim()))
                {
                    myScores.Add(PaperItems[0].items[0].item_score);
                }
                else
                    myScores.Add(0);
                if (PaperItems[0].items[1].answers[0].answer_content.Trim().Split('|').Contains(TxtAnswer2?.Trim()))
                {
                    myScores.Add(PaperItems[0].items[1].item_score);
                }
                else
                    myScores.Add(0);
                if (PaperItems[0].items[2].answers[0].answer_content.Trim().Split('|').Contains(TxtAnswer3?.Trim()))
                {
                    myScores.Add(PaperItems[0].items[2].item_score);
                }
                else
                    myScores.Add(0);
                if (PaperItems[0].items[3].answers[0].answer_content.Trim().Split('|').Contains(TxtAnswer4?.Trim()))
                {
                    myScores.Add(PaperItems[0].items[3].item_score);
                }
                else
                    myScores.Add(0);
                if (PaperItems[0].items[4].answers[0].answer_content.Trim().Split('|').Contains(TxtAnswer5?.Trim()))
                {
                    myScores.Add(PaperItems[0].items[4].item_score);
                }
                else
                    myScores.Add(0);


                //评分
                for (int i = 0; i < PaperItems[0].items.Count; i++)
                { 
                    SubmitRecordModel model = new SubmitRecordModel
                    {
                        exam_attend_id = GlobalUser.AttendPaperItemId,
                        item_id = PaperItems[0].items[i].item_id,
                        item_score = PaperItems[0].items[i].item_score,
                        exam_score = myScores[i],
                        user_answer = userAnswers[i],
                        item_answer = PaperItems[0].items[i].answers[0].answer_content,
                        item_answer_type = 1,
                        qs_id = PaperDetailItem.qs_id,
                        qs_type = PaperDetailItem.qs_type,
                        item_no = PaperItems[0].items[i].item_no
                    };

                    var result = WebApiProxy.GetHtmlRespInfo(model, ApiType.SubmitRecord, GlobalUser.USER.Token);

                    GlobalUser.DoneItemExam = true;
                    GlobalUser.DoneScore = ScoreType.ScoreSuccess; //评分
                    if (result?.retCode != 0)
                    {
                        GlobalUser.DoneScore = ScoreType.ScoreFailure; //提交评分 失败了
                        Log4NetHelper.Error("UpdateSubChoiceAnswer: Error");
                        Log4NetHelper.Error("上传评分失败 -- result: 提交评分失败");
                    }
                }
            }));
        }

        private void UpdateAnswer()
        {
            var paperItemsItem = PaperItems[1].items[0];
            float myScore = 0f;
            string myAnswer = "";

            SubmitRecordModel model = new SubmitRecordModel
            {
                exam_attend_id = GlobalUser.AttendPaperItemId,
                item_id = paperItemsItem.item_id,
                item_score = paperItemsItem.item_score,
                exam_score = myScore,
                user_answer = myAnswer,
                item_answer = paperItemsItem.answers[0].answer_content.ToDBC().Zh2En(),
                item_answer_type = 2,
                qs_id = PaperDetailItem.qs_id,
                qs_type = PaperDetailItem.qs_type,
                item_no = PaperItems[1].items[0].item_no
            };

            Messenger.Default.Send(
                new ExamScoreNavigateMessage(model, _waveFileName, EngineQsType.COMPOSITION, EngineType.OPEN),
                "ExamScoreNavi");
        }

        #endregion
    }
}

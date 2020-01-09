using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    /// 信息获取及转述。
    /// </summary>
    public class ObtainInfoAskViewModel : QsBaseViewModel
    {
        public new QuestionType QsType = QuestionType.HearingSingleChoice;
        public const string ViewName = "ObtainInfoAskViewModel";
        private const string MediaPic = ".JPG .GIF .PNG .BMP .JPEG";
        private int _infoIndex = 0;
        private int _itemIndex = 0;
        private int _audioPlayTimes=0;
        private int _prepareTime = 0;
        private int _answerTime = 0;
        private int _item_repet_times = 1;
        private int _info_repet_times = 1;

        /// <summary>
        /// 信息获取。
        /// </summary>
        public ObtainInfoAskViewModel(int qsIndex,int infoIndex, int itemIndex)
        {
            QsIndex = qsIndex;
            _infoIndex = infoIndex;
            _itemIndex = itemIndex;
            _dTimer = new DispatcherTimer();
            _dTimer.Interval = TimeSpan.FromSeconds(1);
            _dTimer.Tick += DTimerOnTick;
        }

        /// <summary>
        /// 信息获取。
        /// </summary>
        /// <param name="paperDetail">试题完成内容</param>
        /// <param name="item_id">小题编号  默认值:null</param>
        public ObtainInfoAskViewModel(Paper_DetailItem paperDetail, Button btn, int qsIndex, int infoIndex, int itemIndex = 0) : this(qsIndex, infoIndex, itemIndex)
        {
            PaperDetailItem = paperDetail;

            _BtnSkipNext = btn;
            _BtnSkipNext.IsEnabled = false;
            _BtnSkipNext.Click += BtnSkipNext_Click;

            QsID = paperDetail.qs_id;
            QsTitle = paperDetail.qs_title;
            QsTitleContent = paperDetail.qs_content;
            QsTitleAudio = paperDetail.source_content;
            PaperItems = paperDetail.info.OrderBy(o => o.info_sort).ToList();

            BindQsItemInfo(itemIndex);
        }

        #region << 属性 字段 >>

        private string qsItemContent;

        /// <summary>
        /// 内容
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

        private string qsItemContent1;

        /// <summary>
        /// 内容
        /// </summary>
        public string QsItemContent1
        {
            get
            {
                return qsItemContent1;
            }
            set
            {
                qsItemContent1 = value;
                RaisePropertyChanged("QsItemContent1");
            }
        }

        private object _QsInfoImage;
        /// <summary>
        /// 图片
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
        private void BindQsItemInfo(int item_id = 0)
        {
            if (_infoIndex == 0|| _infoIndex == 1)
            {
                if (_itemIndex >= PaperItems[1].items.Count())
                {
                    _itemIndex = 0;
                    _infoIndex = 2;
                }
            }
            else
            {

                if (_itemIndex >= PaperItems[_infoIndex].items.Count())
                {
                    _itemIndex = 0;
                    _infoIndex++;
                }
            }


            if (_infoIndex >= PaperItems.Count)
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
                if (_infoIndex == 2)
                {
                    QsTitleContent = PaperItems[_infoIndex].info_content;
                    QsItemContent = "";
                    QsItemContent1 = PaperItems[_infoIndex].items[_itemIndex].item_content;
                    QsInfoImage = null;
                }
                else
                {
                    QsItemContent = PaperItems[_infoIndex].info_content;

                    if (!string.IsNullOrEmpty(PaperItems[0].info_content_img) &&
                        MediaPic.Contains(PaperItems[0].info_content_img.ToUpper().Split('.').LastOrDefault()))
                    {
                        QsInfoImage = new BitmapImage(new Uri($"{WebApiProxy.MEDIAURL}{PaperItems[0].info_content_img}"));
                    }
                }



                //初始化
                PlayTime = 0;
                TotalTime = 0;
                _audioPlayTimes = 0;
                _item_repet_times = 1;
                _info_repet_times = 1;
                _Recording = RecordState.UnRecord;

                BeginExam(_NextFlowType);
            }
        }

        private void DTimerOnTick(object sender, EventArgs e)
        {
            if (_BtnSkipNext != null&& !_BtnSkipNext.IsEnabled)
                _BtnSkipNext.IsEnabled = (PlayTime >= 1);

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
                    CleanUp();
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
            if (_infoIndex == 2)
            {
                //询问
                switch (nextFlowType)
                {
                    case ExamFlowType.TitleAudio:
                        //1.播放大题题干
                        PromptCommandText = "听指令";
                        _NextFlowType = ExamFlowType.PromptAudio;
                        PlayAudio(QsTitleAudio);
                        break;
                    case ExamFlowType.PromptAudio:
                        //2.播放大题题干 提示音频,
                        PromptCommandText = "听指令";
                        if (string.IsNullOrEmpty(PaperItems[_infoIndex].source_content))
                        {
                            _NextFlowType = ExamFlowType.QuestionAudio;
                            BeginExam(_NextFlowType);
                            break;
                        }
                        _NextFlowType = ExamFlowType.QuestionAudio;
                        PlayAudio(PaperItems[_infoIndex].source_content);
                        break;
                    case ExamFlowType.QuestionAudio:
                        //3.播放 题目音频
                        PromptCommandText = "请听录音";
                        if (string.IsNullOrEmpty(PaperItems[_infoIndex].items[_itemIndex].source_content))
                        {
                            _NextFlowType = ExamFlowType.PrepareTime;
                            BeginExam(_NextFlowType);
                            break;
                        }
                        PlayAudio(PaperItems[_infoIndex].items[_itemIndex].source_content);
                        if (PaperItems[_infoIndex].items[_itemIndex].item_repet_times > 1 &&
                            PaperItems[_infoIndex].items[_itemIndex].item_repet_times > _item_repet_times)
                        {
                            _item_repet_times++;
                            _NextFlowType = ExamFlowType.QuestionAudio;
                        }
                        else
                            _NextFlowType = ExamFlowType.PrepareTime;

                        break;
                    case ExamFlowType.PrepareTime:
                        //3.阅读题目
                        PromptCommandText = "请看题";
                        TotalTime = PaperItems[_infoIndex].items[_itemIndex].item_prepare_second;
                        PlayTime = 0;
                        SendProgress();
                        _NextFlowType = ExamFlowType.AnswerTime;
                        _dTimer.Start();
                        break;
                    case ExamFlowType.StartRecordAudio:
                        //3.阅读题目
                        PromptCommandText = "准备录音";
                        _NextFlowType = ExamFlowType.AnswerTime;
                        //PlayAudio(PaperItems[_infoIndex].source_content);
                        PlaySysAudio(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "startrecord.mp3"));
                        break;
                    case ExamFlowType.AnswerTime:
                        ////3.阅读题目
                        //PromptCommandText = "开始录音";
                        //TotalTime = PaperItems[_infoIndex].items[_itemIndex].item_answer_second;
                        //PlayTime = 0;
                        //_NextFlowType = ExamFlowType.StopRecordAudio;
                        //_Recording = RecordState.Recording;
                        //PlayRecorder();
                        //3.阅读题目
                        PromptCommandText = "开始答题";
                        TotalTime = PaperItems[_infoIndex].items[_itemIndex].item_answer_second;
                        PlayTime = 0;
                        _NextFlowType = ExamFlowType.StopRecordAudio;
                        _Recording = RecordState.Recording;
                        PlayRecorderWithSysAduio("startrecord");
                        break;
                    case ExamFlowType.StopRecordAudio:
                        //3.阅读题目
                        PromptCommandText = "停止录音";
                        _NextFlowType = ExamFlowType.NextQs;
                        //PlayAudio(PaperItems[_infoIndex].source_content);
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
                        UpdateAnswer(EngineQsType.SCENEASK);
                        //3.下一题
                        _itemIndex++;
                        _NextFlowType = ExamFlowType.QuestionAudio;
                        BindQsItemInfo();
                        break;
                }

            }
            else
            {
                //信息获取
                switch (nextFlowType)
                {
                    case ExamFlowType.TitleAudio:
                        //1.播放大题题干
                        PromptCommandText = "听指令";
                        _NextFlowType = ExamFlowType.PromptAudio;
                        PlayAudio(QsTitleAudio);
                        break;
                    case ExamFlowType.PromptAudio:
                        //2.播放大题题干 提示音频,
                        PromptCommandText = "听指令";
                        if (string.IsNullOrEmpty(PaperItems[_infoIndex].source_content))
                        {
                            _NextFlowType = ExamFlowType.PrepareTime;
                            BeginExam(_NextFlowType);
                            break;
                        }

                        _NextFlowType = ExamFlowType.PrepareTime;
                        PlayAudio(PaperItems[_infoIndex].source_content);
                        break;
                    case ExamFlowType.PrepareTime:
                        //3.阅读题目
                        PromptCommandText = "请看题";
                        TotalTime = PaperItems[0].info_prepare_second;
                        PlayTime = 0;
                        SendProgress();
                        _NextFlowType = ExamFlowType.QuestionAudio;
                        _dTimer.Start();
                        break;
                    case ExamFlowType.QuestionAudio:
                        //3.播放 题目音频
                        PromptCommandText = "请听录音";
                        if (string.IsNullOrEmpty(PaperItems[_infoIndex].info_content_source_content))
                        {
                            _NextFlowType = ExamFlowType.PrepareTime2;
                            BeginExam(_NextFlowType);
                            break;
                        }

                        PlayAudio(PaperItems[_infoIndex].info_content_source_content);
                        if (PaperItems[_infoIndex].info_repet_times > 1 &&
                            PaperItems[_infoIndex].info_repet_times > _info_repet_times)
                        {
                            _info_repet_times++;
                            _NextFlowType = ExamFlowType.QuestionAudio;
                        }
                        else
                            _NextFlowType = ExamFlowType.PrepareTime2;

                        break;
                    case ExamFlowType.PrepareTime2:
                        QsItemContent = "";
                        QsItemContent1 = PaperItems[1].info_content;
                        //3.阅读题目
                        PromptCommandText = "请看题";
                        TotalTime = PaperItems[1].items[0].item_prepare_second;
                        PlayTime = 0;
                        SendProgress();
                        _NextFlowType = ExamFlowType.AnswerTime;
                        _dTimer.Start();
                        break;
                    case ExamFlowType.StartRecordAudio:
                        //3.阅读题目
                        PromptCommandText = "准备录音";
                        _NextFlowType = ExamFlowType.AnswerTime;
                        //PlayAudio(PaperItems[_infoIndex].source_content);
                        PlaySysAudio(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "startrecord.mp3"));
                        break;
                    case ExamFlowType.AnswerTime:
                        ////3.阅读题目
                        //PromptCommandText = "开始录音";
                        //TotalTime = PaperItems[1].items[0].item_answer_second;
                        //PlayTime = 0;
                        //_NextFlowType = ExamFlowType.StopRecordAudio;
                        //_Recording = RecordState.Recording;
                        //PlayRecorder();
                        //3.阅读题目
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
                        //PlayAudio(PaperItems[_infoIndex].source_content);
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
                        UpdateAnswer(EngineQsType.COMPOSITION, 1, 0);
                        //3.下一题
                        _itemIndex = 2;
                        _NextFlowType = ExamFlowType.PromptAudio;
                        BindQsItemInfo();
                        break;
                }
            }



        }

        private void UpdateAnswer(EngineQsType eqType , int infoIndex = -999, int itemIndex =-999)
        {
            if (infoIndex == -999)
            {
                infoIndex = _infoIndex;
            }
            if (itemIndex == -999)
            {
                itemIndex = _itemIndex;
            }

            var paperItemsItem = PaperItems[infoIndex].items[itemIndex];
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
                item_no = paperItemsItem.item_no
            };

            Messenger.Default.Send(
                new ExamScoreNavigateMessage(model, _waveFileName, eqType, EngineType.OPEN),
                "ExamScoreNavi");
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
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

namespace Plugin.Exam.Qs.ViewModel
{
    /// <summary>
    /// 听长对话 回答问题 淄博 39
    /// </summary>
    public class DiaChoice39ViewModel : QsBaseViewModel
    {
        public new QuestionType QsType = QuestionType.HearingSingleChoice;
        public const string ViewName = "DiaChoiceViewModel";
        private const string MediaPic = ".JPG .GIF .PNG .BMP .JPEG";
        private int _itemIndex = 0;
        private int _audioPlayTimes=0;
        private int _prepareTime = 0;
        private int _answerTime = 0;
        private int _info_repet_times = 1;
        private int _item_repet_times = 1;
        private int _showIndexNum = 5;
        private StackPanel _subChoiceQsContent;

        /// <summary>
        /// 听长对话 回答问题
        /// </summary>
        /// <param name="qsIndex"></param>
        /// <param name="itemIndex"></param>
        public DiaChoice39ViewModel(int qsIndex, int itemIndex)
        {
            QsIndex = qsIndex;
            _itemIndex = itemIndex;
            _dTimer = new DispatcherTimer();
            _dTimer.Interval = TimeSpan.FromSeconds(1);
            _dTimer.Tick += DTimerOnTick;
        }

        /// <summary>
        /// 听长对话 回答问题
        /// </summary>
        /// <param name="paperDetail"></param>
        /// <param name="qsIndex"></param>
        /// <param name="itemIndex"></param>
        /// <param name="item_id"></param>
        public DiaChoice39ViewModel(StackPanel subChoiceQsContent, Paper_DetailItem paperDetail, Button btn, int qsIndex, int itemIndex, string item_id = null) : this(qsIndex, itemIndex)
        {
            if (!GlobalUser.DoAnswer)
            {

                CleanUp();

                return;
            }

            _subChoiceQsContent = subChoiceQsContent;

            _BtnSkipNext = btn;
            _BtnSkipNext.IsEnabled = false;
            _BtnSkipNext.Click += BtnSkipNext_Click;

            PaperDetailItem = paperDetail;
            QsID = paperDetail.qs_id;
            QsTitle = paperDetail.qs_title;
            QsTitleContent = paperDetail.qs_content;
            QsTitleAudio = paperDetail.source_content;
            PaperItems = paperDetail.info.OrderBy(o => o.info_sort).ToList();

            BindQsItemInfo(item_id);
        }

        #region << 属性 字段 >>

        private string titleTwo;

        /// <summary>
        /// 二级 题目 标题
        /// </summary>
        public string TitleTwo
        {
            get
            {
                return titleTwo;
            }
            set
            {
                titleTwo = value;
                RaisePropertyChanged("TitleTwo");
            }
        }



        #endregion

        #region << 按钮方法 >>

        private RelayCommand<string> _CloseExamCommand;//录音 正常,打开 考试

        public RelayCommand<string> CloseExamCommand
        {
            get { return _CloseExamCommand ?? (_CloseExamCommand = new RelayCommand<string>(s =>
            {
                
            })); }
        }


        #endregion

        #region << 自定义方法 >>

        /// <summary>
        /// 加载题目
        /// </summary>
        /// <param name="item_id"></param>
        private void BindQsItemInfo(string item_id = null)
        {
            if (_itemIndex >= PaperItems[0].items.Count)
            {
                //进入 下一道大题
                NextQsView();
            }
            else
            {

                if (_itemIndex == 0)
                {
                    TitleTwo = PaperItems[0].info_content;
                    _subChoiceQsContent.Children.Clear();

                    for (int i = 0; i < PaperItems[0].items.Count; i++)
                    {
                        var subChoiceView = new SubTextChoiceView();
                        subChoiceView.DataContext =
                            new SubTextChoiceViewModel(PaperItems[0].items[i], i, (i + 1));

                        _subChoiceQsContent.Children.Add(subChoiceView);
                    }

                    //初始化
                    PlayTime = 0;
                    TotalTime = 0;
                    _audioPlayTimes = 0;
                    _prepareTime = 0;
                    _answerTime = 0;
                    _item_repet_times = 1;
                }

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
                PlayTime = TotalTime;
                CleanUp();
                BeginExam(_NextFlowType);
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
            var itemInfo = PaperItems[0];


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
                    _NextFlowType = ExamFlowType.PrepareTime;
                    if (string.IsNullOrEmpty(itemInfo.source_content))
                    {
                        BeginExam(_NextFlowType);
                        break;
                    }

                    //_NextFlowType = ExamFlowType.PrepareTime;
                    PlayAudio(itemInfo.source_content);
                    break;
                case ExamFlowType.PrepareTime:
                    //3.阅读题目
                    PromptCommandText = "请看题";
                    _prepareTime = TotalTime = itemInfo.items.Sum(s=>s.item_prepare_second);
                    PlayTime = 0;
                    SendProgress();
                    _NextFlowType = ExamFlowType.QuestionInfoAudio;
                    _dTimer.Start();
                    break;
                case ExamFlowType.QuestionInfoAudio:
                    //3.播放 题目音频
                    PromptCommandText = "请听录音";
                    if (string.IsNullOrEmpty(itemInfo.info_content_source_content))
                    {
                        _NextFlowType = ExamFlowType.QuestionAudio;
                        BeginExam(_NextFlowType);
                        break;
                    }

                    PlayAudio(itemInfo.info_content_source_content);
                    if (itemInfo.info_repet_times > 1 &&
                        itemInfo.info_repet_times > _item_repet_times)
                    {
                        _item_repet_times++;
                        _NextFlowType = ExamFlowType.TipTime;
                    }
                    else
                        _NextFlowType = ExamFlowType.QuestionAudio;

                    break;
                case ExamFlowType.TipTime:
                    //间隔 等待时间
                    PromptCommandText = "";
                    TotalTime = PaperItems[0].info_interval;
                    PlayTime = 0;
                    SendProgress();
                    _NextFlowType = ExamFlowType.QuestionInfoAudio;
                    _dTimer.Start();
                    break;
                //case ExamFlowType.TipAudio:
                //    //间隔 等待提示音
                //    PromptCommandText = "";
                //    _NextFlowType = ExamFlowType.QuestionInfoAudio;
                //    if (string.IsNullOrEmpty(PaperItems[0].item_tip_source_content))
                //    {
                //        BeginExam(_NextFlowType);
                //        break;
                //    }
                //    //_NextFlowType = ExamFlowType.PrepareTime;
                //    PlayAudio(PaperItems[0].item_tip_source_content);
                //    break;
                case ExamFlowType.ItemTipAudio:
                    //间隔 等待提示音
                    PromptCommandText = "";
                    _NextFlowType = ExamFlowType.QuestionAudio;
                    if (string.IsNullOrEmpty(PaperItems[0].item_tip_source_content))
                    {
                        BeginExam(_NextFlowType);
                        break;
                    }
                    //_NextFlowType = ExamFlowType.PrepareTime;
                    PlayAudio(PaperItems[0].item_tip_source_content);
                    break;
                case ExamFlowType.QuestionAudio:
                    //间隔 等待提示音
                    PromptCommandText = "请听录音";
                    _NextFlowType = ExamFlowType.PrepareTime;
                    if (string.IsNullOrEmpty(itemInfo.items[_itemIndex].source_content))
                    {
                        BeginExam(_NextFlowType);
                        break;
                    }
                    PlayAudio(itemInfo.items[_itemIndex].source_content);
                    if (itemInfo.items[_itemIndex].item_repet_times > 1 &&
                        itemInfo.items[_itemIndex].item_repet_times > _item_repet_times)
                    {
                        _item_repet_times++;
                        _NextFlowType = ExamFlowType.ItemTipAudio;
                    }
                    else
                        _NextFlowType = ExamFlowType.AnswerTime;

                    break;
                case ExamFlowType.AnswerTime:
                    //3.阅读题目
                    PromptCommandText = "开始答题";
                    _answerTime = TotalTime = itemInfo.items[_itemIndex].item_answer_second;
                    PlayTime = 0;
                    _NextFlowType = ExamFlowType.NextQs;
                    _dTimer.Start();
                    break;
                case ExamFlowType.NextQs:
                    //记录答案
                    UpdateAnswer();
                    //3.下一题
                    _itemIndex++;
                    _NextFlowType = ExamFlowType.ItemTipAudio;
                    BindQsItemInfo();
                    break;
            }

        }

        private void UpdateAnswer()
        {
            if (_subChoiceQsContent.Children.Count > 0 && _subChoiceQsContent.Children.Count == (_itemIndex + 1))
            {
                for (int i = 0; i < _subChoiceQsContent.Children.Count; i++)
                {
                    var subView = _subChoiceQsContent.Children[i] as SubTextChoiceView;

                    var subVM = subView.DataContext as SubTextChoiceViewModel;

                    subVM.UpdateSubChoiceAnswer(PaperDetailItem.qs_id, PaperDetailItem.qs_type);
                }
            }
        }

        #endregion


    }
}

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
    /// 听长对话 回答问题
    /// </summary>
    public class DiaChoiceViewModel : QsBaseViewModel
    {
        public new QuestionType QsType = QuestionType.HearingSingleChoice;
        public const string ViewName = "DiaChoiceViewModel";
        private const string MediaPic = ".JPG .GIF .PNG .BMP .JPEG";
        private int _itemIndex = 0;
        private int _audioPlayTimes=0;
        private int _prepareTime = 0;
        private int _answerTime = 0;
        private int _item_repet_times = 1;
        private int _showIndexNum = 5;
        private StackPanel _subChoiceQsContent;

        /// <summary>
        /// 听长对话 回答问题
        /// </summary>
        /// <param name="qsIndex"></param>
        /// <param name="itemIndex"></param>
        public DiaChoiceViewModel(int qsIndex, int itemIndex)
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
        public DiaChoiceViewModel(StackPanel subChoiceQsContent, Paper_DetailItem paperDetail, Button btn, int qsIndex, int itemIndex, string item_id = null) : this(qsIndex, itemIndex)
        {
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
            if (_itemIndex >= PaperItems.Count)
            {
                //进入 下一道大题
                NextQsView();
            }
            else
            {
                TitleTwo = PaperItems[_itemIndex].info_content;

                if (_itemIndex > 0)
                {
                    _showIndexNum = _showIndexNum + PaperItems[0].items.Count;
                }

                _subChoiceQsContent.Children.Clear();

                if (PaperItems[_itemIndex].items.Count > 0)
                {
                    var subChoiceView = new SubTextChoiceView();
                    subChoiceView.DataContext =
                        new SubTextChoiceViewModel(PaperItems[_itemIndex].items[0], 0, (_showIndexNum + 1));

                    _subChoiceQsContent.Children.Add(subChoiceView);
                }
                if (PaperItems[_itemIndex].items.Count > 1)
                {
                    var subChoiceView = new SubTextChoiceView();
                    subChoiceView.DataContext = new SubTextChoiceViewModel(PaperItems[_itemIndex].items[1], 1, (_showIndexNum + 2));

                    _subChoiceQsContent.Children.Add(subChoiceView);
                }
                if (PaperItems[_itemIndex].items.Count > 2)
                {
                    var subChoiceView = new SubTextChoiceView();
                    subChoiceView.DataContext = new SubTextChoiceViewModel(PaperItems[_itemIndex].items[2], 2, (_showIndexNum + 3));

                    _subChoiceQsContent.Children.Add(subChoiceView);
                }


                //初始化
                PlayTime = 0;
                TotalTime = 0;
                _audioPlayTimes = 0;
                _prepareTime = 0;
                _answerTime = 0;
                _item_repet_times = 1;

                BeginExam(_NextFlowType);
            }
        }

        private void DTimerOnTick(object sender, EventArgs e)
        {
            if (_BtnSkipNext != null&& !_BtnSkipNext.IsEnabled)
                _BtnSkipNext.IsEnabled = (PlayTime >= 1);

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
            var itemInfo = PaperItems[_itemIndex];


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
                    if (string.IsNullOrEmpty(itemInfo.source_content))
                    {
                        _NextFlowType = ExamFlowType.PrepareTime;
                        BeginExam(_NextFlowType);
                        break;
                    }

                    _NextFlowType = ExamFlowType.PrepareTime;
                    PlayAudio(itemInfo.source_content);
                    break;
                case ExamFlowType.PrepareTime:
                    //3.阅读题目
                    PromptCommandText = "请看题";
                    _prepareTime = TotalTime = itemInfo.items.Sum(s=>s.item_prepare_second);
                    PlayTime = 0;
                    SendProgress();
                    _NextFlowType = ExamFlowType.QuestionAudio;
                    _dTimer.Start();
                    break;
                case ExamFlowType.QuestionAudio:
                    //3.播放 题目音频
                    PromptCommandText = "请听录音";
                    if (string.IsNullOrEmpty(itemInfo.info_content_source_content))
                    {
                        _NextFlowType = ExamFlowType.AnswerTime;
                        BeginExam(_NextFlowType);
                        break;
                    }

                    PlayAudio(itemInfo.info_content_source_content);
                    if (itemInfo.info_repet_times > 1 &&
                        itemInfo.info_repet_times > _item_repet_times)
                    {
                        _item_repet_times++;
                        _NextFlowType = ExamFlowType.QuestionAudio;
                    }
                    else
                        _NextFlowType = ExamFlowType.AnswerTime;

                    break;
                case ExamFlowType.AnswerTime:
                    //3.阅读题目
                    PromptCommandText = "开始答题";
                    _answerTime = TotalTime = itemInfo.items.Sum(s => s.item_answer_second);
                    PlayTime = 0;
                    _NextFlowType = ExamFlowType.NextQs;
                    _dTimer.Start();
                    break;
                case ExamFlowType.NextQs:
                    //记录答案
                    UpdateAnswer();
                    //3.下一题
                    _itemIndex++;
                    _NextFlowType = ExamFlowType.PrepareTime;
                    BindQsItemInfo();
                    break;
            }

        }

        private void UpdateAnswer()
        {
            if (_subChoiceQsContent.Children.Count > 0)
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

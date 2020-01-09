﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using NAudio.Wave;
using Plugin.Exam.Report.View.SubItem;
using Plugin.Exam.Report.ViewModel.SubItem;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Models.Paper;
using ST.Models.Score;

namespace Plugin.Exam.Report.ViewModel
{
    public class SpokenScneRVM : QsBaseRVM
    {
        #region << 属性 >>

        private StackPanel _qsItemContent;

        private string userAudio = "";

        private string paperContentAudio = "";
        private string qsItemContentAudio1 = "";
        private string qsItemContentAudio2 = "";

        private int _SelectedChoiceExamShow;

        public int SelectedChoiceExamShow
        {
            get => _SelectedChoiceExamShow;
            set
            {
                if (_SelectedChoiceExamShow != value)
                {
                    _SelectedChoiceExamShow = value;
                    RaisePropertyChanged("SelectedChoiceExamShow");
                    BindSubItemChoice();
                }
            }
        }

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

        private string _PlayUserAudioUrl1;
        /// <summary>
        /// 我的录音音频 1
        /// </summary>
        public string PlayUserAudioUrl1
        {
            get
            {
                return _PlayUserAudioUrl1;
            }
            set
            {
                _PlayUserAudioUrl1 = value;
                RaisePropertyChanged("PlayUserAudioUrl1");
            }
        }

        private string _PlayUserAudioUrl2;
        /// <summary>
        /// 我的录音音频 2
        /// </summary>
        public string PlayUserAudioUrl2
        {
            get
            {
                return _PlayUserAudioUrl2;
            }
            set
            {
                _PlayUserAudioUrl2 = value;
                RaisePropertyChanged("PlayUserAudioUrl2");
            }
        }

        private string _QsItemContent1;
        /// <summary>
        /// 问题内容 1
        /// </summary>
        public string QsItemContent1
        {
            get
            {
                return _QsItemContent1;
            }
            set
            {
                _QsItemContent1 = value;
                RaisePropertyChanged("QsItemContent1");
            }
        }

        private string _QsItemContent2;
        /// <summary>
        /// 问题内容 2
        /// </summary>
        public string QsItemContent2
        {
            get
            {
                return _QsItemContent2;
            }
            set
            {
                _QsItemContent2 = value;
                RaisePropertyChanged("QsItemContent2");
            }
        }

        private string _SpeakRefText1;
        /// <summary>
        /// 参考文本内容 1
        /// </summary>
        public string SpeakRefText1
        {
            get
            {
                return _SpeakRefText1;
            }
            set
            {
                _SpeakRefText1 = value;
                RaisePropertyChanged("SpeakRefText1");
            }
        }

        private string _SpeakRefText2;
        /// <summary>
        /// 参考文本内容 2
        /// </summary>
        public string SpeakRefText2
        {
            get
            {
                return _SpeakRefText2;
            }
            set
            {
                _SpeakRefText2 = value;
                RaisePropertyChanged("SpeakRefText2");
            }
        }

        private Visibility _ShowPlayBtn;
        /// <summary>
        /// 音频 播放按钮
        /// </summary>
        public Visibility ShowPlayBtn
        {
            get
            {
                return _ShowPlayBtn;
            }
            set
            {
                _ShowPlayBtn = value;
                RaisePropertyChanged("ShowPlayBtn");
            }
        }

        private Visibility _ShowPlayBtn1;
        /// <summary>
        /// 问题音频 播放按钮 1
        /// </summary>
        public Visibility ShowPlayBtn1
        {
            get
            {
                return _ShowPlayBtn1;
            }
            set
            {
                _ShowPlayBtn1 = value;
                RaisePropertyChanged("ShowPlayBtn1");
            }
        }

        private Visibility _ShowPlayBtn2;
        /// <summary>
        /// 问题音频 播放按钮 2
        /// </summary>
        public Visibility ShowPlayBtn2
        {
            get
            {
                return _ShowPlayBtn2;
            }
            set
            {
                _ShowPlayBtn2 = value;
                RaisePropertyChanged("ShowPlayBtn2");
            }
        }

        #endregion


        #region << 按钮 command >>


        private RelayCommand _PlayQsAudio;//播放 音频

        public RelayCommand PlayQsAudio
        {
            get
            {
                return _PlayQsAudio ?? (_PlayQsAudio = new RelayCommand(
                           () =>
                           {
                               if (GlobalUser.WavePlayer?.PlaybackState == PlaybackState.Playing)
                               {
                                   GlobalUser.WavePlayer?.Stop();
                               }

                               if (!string.IsNullOrEmpty(paperContentAudio))
                               {
                                   PlayAudio(paperContentAudio);
                                   PlayIcon = "9-1";
                               }

                           }));
            }
        }
        private RelayCommand _PlayQsItemAudio1;//播放 音频

        public RelayCommand PlayQsItemAudio1
        {
            get
            {
                return _PlayQsItemAudio1 ?? (_PlayQsItemAudio1 = new RelayCommand(
                           () =>
                           {
                               if (GlobalUser.WavePlayer?.PlaybackState == PlaybackState.Playing)
                               {
                                   GlobalUser.WavePlayer?.Stop();
                               }

                               if (!string.IsNullOrEmpty(qsItemContentAudio1))
                               {
                                   PlayAudio(qsItemContentAudio1);
                                   PlayIcon = "9-2";
                               }

                           }));
            }
        }
        private RelayCommand _PlayQsItemAudio2;//播放 音频

        public RelayCommand PlayQsItemAudio2
        {
            get
            {
                return _PlayQsItemAudio2 ?? (_PlayQsItemAudio2 = new RelayCommand(
                           () =>
                           {
                               if (GlobalUser.WavePlayer?.PlaybackState == PlaybackState.Playing)
                               {
                                   GlobalUser.WavePlayer?.Stop();
                               }

                               if (!string.IsNullOrEmpty(qsItemContentAudio2))
                               {
                                   PlayAudio(qsItemContentAudio2);
                                   PlayIcon = "9-3";
                               }

                           }));
            }
        }

        #endregion

        public SpokenScneRVM(StackPanel qsItemContent, Paper_DetailItem paperDetail, Exam_Attend_Result examResult)
        {
            _qsItemContent = qsItemContent;
            PaperDetail = paperDetail;
            ExamResult = examResult;

            QsTitle = PaperDetail.qs_title;
            QsTitleContent = PaperDetail.qs_content;
            QsTitleAudio = PaperDetail.source_content;

            SelectedChoiceExamShow = 0;

            //绑定分数
            BindUserTotalScore();
            //绑定选项内容
            BindSubItemChoice();
        }

        #region << 自定义方法 >>

        /// <summary>
        /// 绑定 选择题 内容
        /// </summary>
        private void BindSubItemChoice()
        {
            var detailInfo = PaperDetail.info[0];

            TitleTwo = detailInfo.info_content;

            paperContentAudio = detailInfo.source_content;
            if (string.IsNullOrEmpty(paperContentAudio))
            {
                ShowPlayBtn = Visibility.Collapsed;
            }

            _qsItemContent.Children.Clear();//清空内容

            //var itemAnswerResult = ExamResult.result_items.Where(w => w.item_id == detailInfo.items[0].item_id).ToList()[0];

            for (int i = 0; i < detailInfo.items.Count; i++)
            {
                var detailItem = detailInfo.items[i];
                var itemAnswerResult = ExamResult.result_items.Where(w => w.item_id == detailItem.item_id).ToList()[0];

                var itemView = new SubSpokenScneRV();
                itemView.DataContext =
                    new SubSpokenScneRVM(detailItem, i, itemAnswerResult, false);
                _qsItemContent.Children.Add(itemView);
            }


            //PlayUserAudioUrl1 = itemAnswerResult.user_answer;
            //SpeakRefText1 = detailInfo.items[0].answers[0].answer_content;

            //QsItemContent1 = string.IsNullOrEmpty(detailInfo.items[0].item_content)
            //    ? "问题1"
            //    : detailInfo.items[0].item_content;

            //qsItemContentAudio1 = detailInfo.items[0].source_content;
            //if (string.IsNullOrEmpty(qsItemContentAudio1))
            //{
            //    ShowPlayBtn1 = Visibility.Collapsed;
            //}

            //itemAnswerResult = ExamResult.result_items.Where(w => w.item_id == detailInfo.items[1].item_id).ToList()[0];
            //PlayUserAudioUrl2 = itemAnswerResult.user_answer;
            //SpeakRefText2 = detailInfo.items[1].answers[0].answer_content;

            //QsItemContent2 = string.IsNullOrEmpty(detailInfo.items[1].item_content)
            //    ? "问题2"
            //    : detailInfo.items[1].item_content;

            //qsItemContentAudio2 = detailInfo.items[1].source_content;
            //if (string.IsNullOrEmpty(qsItemContentAudio2))
            //{
            //    ShowPlayBtn2 = Visibility.Collapsed;
            //}

        }

        /// <summary>
        /// 绑定分数
        /// </summary>
        private void BindUserTotalScore()
        {
            float totaolScore = 0;
            float? userScore = 0;

            var myAllItemId = string.Join(" ", (ExamResult.result_items.Select(s => s.item_id).ToArray()));

            for (int i = 0; i < PaperDetail.info.Count; i++)
            {
                for (int j = 0; j < PaperDetail.info[i].items.Count; j++)
                {
                    totaolScore = totaolScore + Math.Abs(PaperDetail.info[i].items[j].item_score);

                    userScore = userScore + ExamResult.result_items
                                    .Where(w => w.item_id == PaperDetail.info[i].items[j].item_id).ToList()[0].exam_score;

                }
            }

            PaperDetailUserScore = $"{Convert.ToSingle(userScore).ToString("f1")}";
            PaperDetailTotalScore = $"{totaolScore}";
        }

        #endregion

    }
}
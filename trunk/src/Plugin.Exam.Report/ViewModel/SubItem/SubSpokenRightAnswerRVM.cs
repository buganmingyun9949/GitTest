using System;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using NAudio.Wave;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Models.Paper;
using ST.Models.Score;

namespace Plugin.Exam.Report.ViewModel.SubItem
{
    /// <summary>
    /// 说出正确的句子
    /// </summary>
    public class SubSpokenRightAnswerRVM : QsBaseRVM
    {

        #region << 属性 >>

        private Exam_Attend_Result_Item ExamItemResult;

        private string sourceAudio = "";

        private string _QsItemNum;
        public string QsItemNum
        {
            get => _QsItemNum;
            set
            {
                if (_QsItemNum != value)
                {
                    _QsItemNum = value;
                    RaisePropertyChanged("QsItemNum");
                }
            }
        }

        private string _QsItemContent;
        public string QsItemContent
        {
            get => _QsItemContent;
            set
            {
                if (_QsItemContent != value)
                {
                    _QsItemContent = value;
                    RaisePropertyChanged("QsItemContent");
                }
            }
        }

        private string _QsTitle2;
        public string QsTitle2
        {
            get => _QsTitle2;
            set
            {
                if (_QsTitle2 != value)
                {
                    _QsTitle2 = value;
                    RaisePropertyChanged("QsTitle2");
                }
            }
        } 

        private string _SpeakRefText;
        public string SpeakRefText
        {
            get => _SpeakRefText;
            set
            {
                if (_SpeakRefText != value)
                {
                    _SpeakRefText = value;
                    RaisePropertyChanged("SpeakRefText");
                }
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

        private string _PlayUserAudioUrl;
        /// <summary>
        /// 我的录音音频
        /// </summary>
        public string PlayUserAudioUrl
        {
            get
            {
                return _PlayUserAudioUrl;
            }
            set
            {
                _PlayUserAudioUrl = value;
                RaisePropertyChanged("PlayUserAudioUrl");
            }
        }

        private string _UserItemScore;
        public string UserItemScore
        {
            get => _UserItemScore;
            set
            {
                if (_UserItemScore != value)
                {
                    _UserItemScore = value;
                    RaisePropertyChanged("UserItemScore");
                }
            }
        }

        private string _ItemTotalScore;
        public string ItemTotalScore
        {
            get => _ItemTotalScore;
            set
            {
                if (_ItemTotalScore != value)
                {
                    _ItemTotalScore = value;
                    RaisePropertyChanged("ItemTotalScore");
                }
            }
        }

        #endregion

        #region << 按钮 command >>


        private RelayCommand _PlayQsItemAudio;//播放 音频

        public RelayCommand PlayQsItemAudio
        {
            get
            {
                return _PlayQsItemAudio ?? (_PlayQsItemAudio = new RelayCommand(
                           () =>
                           {
                               if (GlobalUser.WavePlayer?.PlaybackState == PlaybackState.Playing)
                               {
                                   GlobalUser.WavePlayer?.Stop();
                               }

                               if (!string.IsNullOrEmpty(sourceAudio))
                               {
                                   PlayAudio(sourceAudio);
                                   PlayIcon = "15-1-1";
                               }

                           }));
            }
        }

        #endregion

        public SubSpokenRightAnswerRVM(Paper_InfoItem paperInfoItem, int itemIndex, Exam_Attend_Result_Item examItemResult, bool showPlayBtn = true)
        {
            CleanUp();

            ExamItemResult = examItemResult;
            QsItemNum = $"{itemIndex + 1}.";
            QsItemContent =paperInfoItem.info_content;

            sourceAudio = paperInfoItem.items[0].source_content;
            ItemTotalScore = $"{paperInfoItem.items[0].item_score}分";

            if (string.IsNullOrEmpty(sourceAudio))
            {
                ShowPlayBtn = Visibility.Collapsed;
            }

            PlayUserAudioUrl = examItemResult.user_answer;

            //ScoreRoot score = JsonHelper.FromJson<ScoreRoot>(examItemResult.score_result.Replace("\"params\"", "\"param\""));
            var answer =
                paperInfoItem.items[0].answers[0].answer_content.Split(new string[] {"|"}, StringSplitOptions.None);
            SpeakRefText = answer[answer.Length - 1];
            UserItemScore = $"{examItemResult.exam_score}分";
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using NAudio.Wave;
using Plugin.Exam.Report.View.SubItem;
using Plugin.Exam.Report.ViewModel.SubItem;
using ST.Common;
using ST.Common.ToolsHelper;
using ST.Common.WebApi;
using ST.Models.Paper;
using ST.Models.Score;

namespace Plugin.Exam.Report.ViewModel
{
    public class Spoken1901RVM : QsBaseRVM
    {
        #region << 属性 >>

        private StackPanel _qsItemContent;

        private string userAudio = "";

        private string paperContentAudio = "";
        private string qsItemContentAudio1 = "";
        private string qsItemContentAudio2 = "";

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

        private Visibility _TitleTextVisibility;
        /// <summary>
        /// 问题音频 播放按钮 1
        /// </summary>
        public Visibility TitleTextVisibility
        {
            get
            {
                return _TitleTextVisibility;
            }
            set
            {
                _TitleTextVisibility = value;
                RaisePropertyChanged("TitleTextVisibility");
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


        private string qsTitleContent1;
        /// <summary>
        /// 题目 一级标题 独白内容
        /// </summary>
        public string QsTitleContent1
        {
            get
            {
                return qsTitleContent1;
            }
            set
            {
                qsTitleContent1 = value;
                RaisePropertyChanged("QsTitleContent1");
            }
        }


        private string qsTitleContent2;
        /// <summary>
        /// 题目 一级标题 独白内容
        /// </summary>
        public string QsTitleContent2
        {
            get
            {
                return qsTitleContent2;
            }
            set
            {
                qsTitleContent2 = value;
                RaisePropertyChanged("QsTitleContent2");
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



        private string _SpeakRefText;
        /// <summary>
        /// 我的录音音频
        /// </summary>
        public string SpeakRefText
        {
            get
            {
                return _SpeakRefText;
            }
            set
            {
                _SpeakRefText = value;
                RaisePropertyChanged("SpeakRefText");
            }
        }

        private string _SpeakRefText1;
        /// <summary>
        /// 我的录音音频
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
        /// 我的录音音频
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

        private string _QsItemContent1;
        /// <summary>
        /// 小题内容
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
        /// 小题内容
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

        private string _UserItemScore1;
        public string UserItemScore1
        {
            get => _UserItemScore1;
            set
            {
                if (_UserItemScore1 != value)
                {
                    _UserItemScore1 = value;
                    RaisePropertyChanged("UserItemScore1");
                }
            }
        }

        private string _ItemTotalScore1;
        public string ItemTotalScore1
        {
            get => _ItemTotalScore1;
            set
            {
                if (_ItemTotalScore1 != value)
                {
                    _ItemTotalScore1 = value;
                    RaisePropertyChanged("ItemTotalScore1");
                }
            }
        }


        private string _UserItemScore2;
        public string UserItemScore2
        {
            get => _UserItemScore2;
            set
            {
                if (_UserItemScore2 != value)
                {
                    _UserItemScore2 = value;
                    RaisePropertyChanged("UserItemScore2");
                }
            }
        }

        private string _ItemTotalScore2;
        public string ItemTotalScore2
        {
            get => _ItemTotalScore2;
            set
            {
                if (_ItemTotalScore2 != value)
                {
                    _ItemTotalScore2 = value;
                    RaisePropertyChanged("ItemTotalScore2");
                }
            }
        }

        private string _TextAnswer1;
        public string TextAnswer1
        {
            get => _TextAnswer1;
            set
            {
                if (_TextAnswer1 != value)
                {
                    _TextAnswer1 = value;
                    RaisePropertyChanged("TextAnswer1");
                }
            }
        }
        private string _TextAnswer2;
        public string TextAnswer2
        {
            get => _TextAnswer2;
            set
            {
                if (_TextAnswer2 != value)
                {
                    _TextAnswer2 = value;
                    RaisePropertyChanged("TextAnswer2");
                }
            }
        }
        private string _TextAnswer3;
        public string TextAnswer3
        {
            get => _TextAnswer3;
            set
            {
                if (_TextAnswer3 != value)
                {
                    _TextAnswer3 = value;
                    RaisePropertyChanged("TextAnswer3");
                }
            }
        }
        private string _TextAnswer4;
        public string TextAnswer4
        {
            get => _TextAnswer4;
            set
            {
                if (_TextAnswer4 != value)
                {
                    _TextAnswer4 = value;
                    RaisePropertyChanged("TextAnswer4");
                }
            }
        }
        private string _TextAnswer5;
        public string TextAnswer5
        {
            get => _TextAnswer5;
            set
            {
                if (_TextAnswer5 != value)
                {
                    _TextAnswer5 = value;
                    RaisePropertyChanged("TextAnswer5");
                }
            }
        }


        private string _OptionStyle1;
        public string OptionStyle1
        {
            get => _OptionStyle1;
            set
            {
                if (_OptionStyle1 != value)
                {
                    _OptionStyle1 = value;
                    RaisePropertyChanged("OptionStyle1");
                }
            }
        }
        private string _OptionStyle2;
        public string OptionStyle2
        {
            get => _OptionStyle2;
            set
            {
                if (_OptionStyle2 != value)
                {
                    _OptionStyle2 = value;
                    RaisePropertyChanged("OptionStyle2");
                }
            }
        }
        private string _OptionStyle3;
        public string OptionStyle3
        {
            get => _OptionStyle3;
            set
            {
                if (_OptionStyle3 != value)
                {
                    _OptionStyle3 = value;
                    RaisePropertyChanged("OptionStyle3");
                }
            }
        }
        private string _OptionStyle4;
        public string OptionStyle4
        {
            get => _OptionStyle4;
            set
            {
                if (_OptionStyle4 != value)
                {
                    _OptionStyle4 = value;
                    RaisePropertyChanged("OptionStyle4");
                }
            }
        }
        private string _OptionStyle5;
        public string OptionStyle5
        {
            get => _OptionStyle5;
            set
            {
                if (_OptionStyle5 != value)
                {
                    _OptionStyle5 = value;
                    RaisePropertyChanged("OptionStyle5");
                }
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

        public Spoken1901RVM(StackPanel qsItemContent, Paper_DetailItem paperDetail, Exam_Attend_Result examResult)
        {
            _qsItemContent = qsItemContent;
            PaperDetail = paperDetail;
            ExamResult = examResult;

            QsTitle = PaperDetail.qs_title;
            //QsTitleContent = PaperDetail.qs_content;
            QsTitleAudio = PaperDetail.source_content;

            //绑定分数
            BindUserTotalScore();
            //绑定选项内容
            BindSubItem();
        }

        #region << 自定义方法 >>

        /// <summary>
        /// 绑定 选择题 内容
        /// </summary>
        private void BindSubItem()
        {
            var detailInfo = PaperDetail.info[0];
            var detailInfo1 = PaperDetail.info[1];
            //var detailInfo2 = PaperDetail.info[2];
            //TitleTwo = detailInfo.info_content;
            TitleTextVisibility = Visibility.Collapsed;

            paperContentAudio = detailInfo.info_content_source_content;
            if (string.IsNullOrEmpty(paperContentAudio))
            {
                ShowPlayBtn = Visibility.Collapsed;
            }

            _qsItemContent.Children.Clear();//清空内容

            QsTitleContent = detailInfo.info_content.FromJsonTo<List<info_content>>()[0].text;
            QsTitleContent1 = detailInfo1.info_content.FromJsonTo<List<info_content>>()[0].text;
            //QsTitleContent2 = detailInfo2.info_content;

            string url = WebApiProxy.GetRedirectUrl($"{WebApiProxy.MEDIAURL}{detailInfo.info_content_img}");
            QsInfoImage = new BitmapImage(new Uri(url));

            var itemAnswerResult = ExamResult.result_items.Where(w => detailInfo.items.Select(s=>s.item_id).ToList().Contains(w.item_id)).ToList();
            var itemAnswerResult1 = ExamResult.result_items.Where(w => w.item_id == detailInfo1.items[0].item_id).ToList()[0];
            //var itemAnswerResult2 = ExamResult.result_items.Where(w => w.item_id == detailInfo2.items[1].item_id).ToList()[0];
            //PlayUserAudioUrl = itemAnswerResult.user_answer;
            PlayUserAudioUrl1 = itemAnswerResult1.user_answer;
            //PlayUserAudioUrl2 = itemAnswerResult2.user_answer;

            //ScoreRoot score = JsonHelper.FromJson<ScoreRoot>(itemAnswerResult.score_result.Replace("\"params\"", "\"param\""));
            ScoreRoot score1 = JsonHelper.FromJson<ScoreRoot>(itemAnswerResult1.score_result.Replace("\"params\"", "\"param\""));
            //ScoreRoot score2 = JsonHelper.FromJson<ScoreRoot>(itemAnswerResult2.score_result.Replace("\"params\"", "\"param\""));
            //var answer = score.param.request.refText.Split(new string[] { "|" }, StringSplitOptions.None);
            //var answer1 = score1.param.request.refText.Split(new string[] { "|" }, StringSplitOptions.None);
            //var answer2 = score2.param.request.refText.Split(new string[] { "|" }, StringSplitOptions.None);

            SpeakRefText = string.Join(", ", detailInfo.items.Select(s => s.item_no + "." + s.answers[0].answer_content).ToArray());
            SpeakRefText1 = GetRefText(score1.param.request.refText);
            //SpeakRefText2 = GetRefText(score2.param.request.refText);

            UserItemScore = $"{itemAnswerResult.Sum(s=>s.exam_score)}分";
            UserItemScore1 = $"{score1.result.overall}分";
            //UserItemScore2 = $"{score2.result.overall}分";

            ItemTotalScore = $"{detailInfo.items.Sum(s=>s.item_score)}分";
            ItemTotalScore1 = $"{detailInfo1.items[0].item_score}分";

            UserPronunciation = (100 * score1.result.pronunciation / Convert.ToSingle(PaperDetailTotalScore)).ToString("0");
            UserFluency = (100 * score1.result.fluency / Convert.ToSingle(PaperDetailTotalScore)).ToString("0");
            UserCoherence = (100 * score1.result.coherence / Convert.ToSingle(PaperDetailTotalScore)).ToString("0");
            UserSpeed = $"{score1.result.speed.ToString("0")} 词/分钟";
            //ItemTotalScore2 = $"{detailInfo2.items[1].item_score}分";

            TextAnswer1 = "1." + itemAnswerResult[0].user_answer;
            TextAnswer2 = "2." + itemAnswerResult[1].user_answer;
            TextAnswer3 = "3." + itemAnswerResult[2].user_answer;
            TextAnswer4 = "4." + itemAnswerResult[3].user_answer;
            TextAnswer5 = "5." + itemAnswerResult[4].user_answer;

            if (itemAnswerResult[0].exam_score == null || itemAnswerResult[0].exam_score == 0f)
                OptionStyle1 = "#ff6161";
            else
                OptionStyle1 = "#41b790";

            if (itemAnswerResult[1].exam_score == null || itemAnswerResult[1].exam_score == 0f)
                OptionStyle2 = "#ff6161";
            else
                OptionStyle2 = "#41b790";

            if (itemAnswerResult[2].exam_score == null || itemAnswerResult[2].exam_score == 0f)
                OptionStyle3 = "#ff6161";
            else
                OptionStyle3 = "#41b790";

            if (itemAnswerResult[3].exam_score == null || itemAnswerResult[3].exam_score == 0f)
                OptionStyle4 = "#ff6161";
            else
                OptionStyle4 = "#41b790";

            if (itemAnswerResult[4].exam_score == null || itemAnswerResult[4].exam_score == 0f)
                OptionStyle5 = "#ff6161";
            else
                OptionStyle5 = "#41b790";

            //#41b790  #ff6161


            //QsItemContent1 = $"{detailInfo2.items[0].item_content}";
            //QsItemContent2 = $"{detailInfo2.items[1].item_content}";
        }

        /// <summary>
        /// 绑定分数
        /// </summary>
        private void BindUserTotalScore()
        {
            float totaolScore = 0f;
            float? userScore = 0f;

            var myAllItemId = string.Join(" ", (ExamResult.result_items.Select(s => s.item_id).ToArray()));

            for (int i = 0; i < PaperDetail.info.Count; i++)
            {
                for (int j = 0; j < PaperDetail.info[i].items.Count; j++)
                {
                    totaolScore = totaolScore + PaperDetail.info[i].items[j].item_score;

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
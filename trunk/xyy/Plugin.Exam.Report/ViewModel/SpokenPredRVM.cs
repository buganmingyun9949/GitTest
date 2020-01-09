using System;
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
    public class SpokenPredRVM : QsBaseRVM
    {
        #region << 属性 >>

        private StackPanel _qsItemContent;

        private string userAudio = "";

        private string paperContentAudio = "";

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
                                   PlayIcon = "5-1";
                               }

                           }));
            }
        }

        #endregion

        public SpokenPredRVM(StackPanel qsItemContent, Paper_DetailItem paperDetail, Exam_Attend_Result examResult)
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
            var detailItem = PaperDetail.info[0].items[0];
            var itemAnswerResult = ExamResult.result_items.Where(w => w.item_id == detailItem.item_id).ToList()[0];

            _qsItemContent.Children.Clear();

            paperContentAudio = detailItem.source_content;
            PlayUserAudioUrl = itemAnswerResult.user_answer;

            if (string.IsNullOrEmpty(paperContentAudio))
            {
                ShowPlayBtn = Visibility.Collapsed;
            }

            WrapPanel wp = GetMainContent(itemAnswerResult.score_result);

            _qsItemContent.Children.Add(wp);
        }

        /// <summary>
        /// 绑定分数
        /// </summary>
        private void BindUserTotalScore()
        {
            float totaolScore = 0;
            float? userScore = 0;

            var myAllItemId = string.Join(" ", (ExamResult.result_items.Select(s => s.item_id).ToArray()));

            for (int i = 0; i < PaperDetail.info[0].items.Count; i++)
            {
                totaolScore = totaolScore + PaperDetail.info[0].items[i].item_score;

                userScore = userScore + ExamResult.result_items
                                .Where(w => w.item_id == PaperDetail.info[0].items[i].item_id).ToList()[0].exam_score;
            }

            PaperDetailUserScore = $"{userScore}";
            PaperDetailTotalScore = $"{totaolScore}";
        }

        private WrapPanel GetMainContent(string input)
        {
            ScoreRoot score = JsonHelper.FromJson<ScoreRoot>(input.Replace("\"params\"", "\"param\""));
            if (score == null)
            {
                MessageBox.Show("无效的评分结果,请重新测试");

                return null;
            }

            UserPronunciation = (100 * score.result.pronunciation / Convert.ToSingle(PaperDetailTotalScore)).ToString("0");
            UserFluency = (100 * score.result.fluency / Convert.ToSingle(PaperDetailTotalScore)).ToString("0");
            UserIntegrity = (100 * score.result.integrity / Convert.ToSingle(PaperDetailTotalScore)).ToString("0");
            UserSpeed = $"{score.result.speed.ToString("0")} 词/分钟";

            List<SentencesItem> sentList = JsonHelper.FromJson<List<SentencesItem>>(score.result.sentences.ToString());

            //StringBuilder strPred = new StringBuilder();

            WrapPanel wp = new WrapPanel();

            TextBlock txtItem = new TextBlock();

            for (int i = 0; i < sentList.Count; i++)
            {
                for (int j = 0; j < sentList[i].details.Count; j++)
                {
                    //strPred.AppendFormat("{0} ", sentList[i].details[j].word);

                    txtItem = new TextBlock();
                    txtItem.Text = sentList[i].details[j].word;

                    //    差：f44116
                    //    中：ff8414
                    //    良：1394fa
                    //    优：41b612
                    if (sentList[i].details[j].overall > Convert.ToSingle(PaperDetailTotalScore) *0.75)
                    {
                        txtItem.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#41B612"));
                    }
                    else if (sentList[i].details[j].overall > Convert.ToSingle(PaperDetailTotalScore) * 0.6 && sentList[i].details[j].overall <= Convert.ToSingle(PaperDetailTotalScore) * 0.75)
                    {
                        txtItem.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1394FA"));
                    }
                    else if (sentList[i].details[j].overall > Convert.ToSingle(PaperDetailTotalScore) * 0.3 && sentList[i].details[j].overall <= Convert.ToSingle(PaperDetailTotalScore) * 0.6)
                    {
                        txtItem.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF8414"));
                    }
                    else
                    {
                        txtItem.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44116"));
                    }
                    txtItem.FontSize = 24;
                    txtItem.Margin = new Thickness(0, 0, 6, 10);
                    wp.Children.Add(txtItem);
                }
            }

            return wp;
        }

        ///<summary>  
        /// 获取一个网页源码中的标签列表，支持嵌套，一般或去div，td等容器  
        ///</summary>  
        ///<param name="input"></param>  
        ///<param name="tag"></param>  
        ///<returns></returns>  
        public static List<string> GetTags(string input, string tag)
        {
            StringReader strReader = new StringReader(input);
            int lowerThanCharCounter = 0;
            int lowerThanCharPos = 0;
            Stack<int> tagPos = new Stack<int>();
            List<string> taglist = new List<string>();
            int i = 0;
            while (true)
            {
                try
                {
                    int intCharacter = strReader.Read();
                    if (intCharacter == -1) break;

                    char convertedCharacter = Convert.ToChar(intCharacter);

                    if (lowerThanCharCounter > 0)
                    {
                        if (convertedCharacter == '>')
                        {
                            lowerThanCharCounter--;

                            string biaoqian = input.Substring(lowerThanCharPos, i - lowerThanCharPos + 1);
                            if (biaoqian.StartsWith(string.Format("<{0}", tag)))
                            {
                                tagPos.Push(lowerThanCharPos);
                            }
                            if (biaoqian.StartsWith(string.Format("</{0}", tag)))
                            {
                                if (tagPos.Count < 1)
                                    continue;
                                int tempTagPos = tagPos.Pop();
                                string strdiv = input.Substring(tempTagPos, i - tempTagPos + 1);
                                taglist.Add(strdiv);
                            }
                        }
                    }

                    if (convertedCharacter == '<')
                    {
                        lowerThanCharCounter++;
                        lowerThanCharPos = i;
                    }
                }
                finally
                {
                    i++;
                }
            }
            return taglist;
        }

        #endregion

    }
}
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using NAudio.Wave;
using ST.Common;
using ST.Common.WebApi;
using ST.Models.Paper;

namespace Plugin.Exam.Report.ViewModel.SubItem
{
    /// <summary>
    /// 选择
    /// </summary>
    public class SubImageChoiceRVM : QsBaseRVM
    {

        #region << 属性 >>

        private Exam_Attend_Result_Item ExamItemResult;

        private string sourceAudio = "";

        private string _QsText;
        public string QsText
        {
            get => _QsText;
            set
            {
                if (_QsText != value)
                {
                    _QsText = value;
                    RaisePropertyChanged("QsText");
                }
            }
        }

        private string _QsTitle1;
        public string QsTitle1
        {
            get => _QsTitle1;
            set
            {
                if (_QsTitle1 != value)
                {
                    _QsTitle1 = value;
                    RaisePropertyChanged("QsTitle1");
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

        private object _ChoiceTextA;
        public object ChoiceTextA
        {
            get => _ChoiceTextA;
            set
            {
                if (_ChoiceTextA != value)
                {
                    _ChoiceTextA = value;
                    RaisePropertyChanged("ChoiceTextA");
                }
            }
        }

        private object _ChoiceTextB;
        public object ChoiceTextB
        {
            get => _ChoiceTextB;
            set
            {
                if (_ChoiceTextB != value)
                {
                    _ChoiceTextB = value;
                    RaisePropertyChanged("ChoiceTextB");
                }
            }
        }

        private object _ChoiceTextC;
        public object ChoiceTextC
        {
            get => _ChoiceTextC;
            set
            {
                if (_ChoiceTextC != value)
                {
                    _ChoiceTextC = value;
                    RaisePropertyChanged("ChoiceTextC");
                }
            }
        }


        private string _ChoiceUserScore;
        public string ChoiceUserScore
        {
            get => _ChoiceUserScore;
            set
            {
                if (_ChoiceUserScore != value)
                {
                    _ChoiceUserScore = value;
                    RaisePropertyChanged("ChoiceUserScore");
                }
            }
        }

        private string _ChoiceUserScoreColor;
        public string ChoiceUserScoreColor
        {
            get => _ChoiceUserScoreColor;
            set
            {
                if (_ChoiceUserScoreColor != value)
                {
                    _ChoiceUserScoreColor = value;
                    RaisePropertyChanged("ChoiceUserScoreColor");
                }
            }
        }


        private string _ChoiceTotalScore;
        public string ChoiceTotalScore
        {
            get => _ChoiceTotalScore;
            set
            {
                if (_ChoiceTotalScore != value)
                {
                    _ChoiceTotalScore = value;
                    RaisePropertyChanged("ChoiceTotalScore");
                }
            }
        }

        private string _OptionStyleA;
        public string OptionStyleA
        {
            get => _OptionStyleA;
            set
            {
                if (_OptionStyleA != value)
                {
                    _OptionStyleA = value;
                    RaisePropertyChanged("OptionStyleA");
                }
            }
        }

        private string _OptionStyleB;
        public string OptionStyleB
        {
            get => _OptionStyleB;
            set
            {
                if (_OptionStyleB != value)
                {
                    _OptionStyleB = value;
                    RaisePropertyChanged("OptionStyleB");
                }
            }
        }

        private string _OptionStyleC;
        public string OptionStyleC
        {
            get => _OptionStyleC;
            set
            {
                if (_OptionStyleC != value)
                {
                    _OptionStyleC = value;
                    RaisePropertyChanged("OptionStyleC");
                }
            }
        }

        private string _TitleTwo;
        public string TitleTwo
        {
            get => _TitleTwo;
            set
            {
                if (_TitleTwo != value)
                {
                    _TitleTwo = value;
                    RaisePropertyChanged("TitleTwo");
                }
            }
        }

        private Visibility _QsPromptTextVisibility;
        /// <summary>
        /// 是否显示提示信息
        /// </summary>
        public Visibility QsPromptTextVisibility
        {
            get
            {
                return _QsPromptTextVisibility;
            }
            set
            {
                _QsPromptTextVisibility = value;
                RaisePropertyChanged("QsPromptTextVisibility");
            }
        }

        private Visibility _QsThreeImageVisibility;
        /// <summary>
        /// 是否显示提示信息
        /// </summary>
        public Visibility QsThreeImageVisibility
        {
            get
            {
                return _QsThreeImageVisibility;
            }
            set
            {
                _QsThreeImageVisibility = value;
                RaisePropertyChanged("QsThreeImageVisibility");
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

                               if (!string.IsNullOrEmpty(sourceAudio))
                               {
                                   PlayAudio(sourceAudio);
                                   PlayIcon = "1-2-1";
                               }

                           }));
            }
        }

        #endregion

        public SubImageChoiceRVM(Paper_ItemsItem paperItemsItem, int itemIndex, Exam_Attend_Result_Item examItemResult, bool showPlayBtn = true)
        {
            CleanUp();

            if (!showPlayBtn)
            {
                ShowPlayBtn = Visibility.Collapsed;
            }

            ExamItemResult = examItemResult;

            QsTitle1 = $"{paperItemsItem.item_content}";//{itemIndex + 1}、
            sourceAudio = paperItemsItem.source_content;

            //ChoiceTextA = $"A. {paperItemsItem.answers[0].answer_content}";
            //ChoiceTextB = $"B. {paperItemsItem.answers[1].answer_content}";
            //ChoiceTextC = $"C. {paperItemsItem.answers[2].answer_content}";
            string urla = WebApiProxy.GetRedirectUrl($"{WebApiProxy.MEDIAURL}{paperItemsItem.answers[0].source_content}");
            ChoiceTextA = new BitmapImage(new Uri(urla));

            string urlb = WebApiProxy.GetRedirectUrl($"{WebApiProxy.MEDIAURL}{paperItemsItem.answers[1].source_content}");
            ChoiceTextB = new BitmapImage(new Uri(urlb));

            string urlc = WebApiProxy.GetRedirectUrl($"{WebApiProxy.MEDIAURL}{paperItemsItem.answers[2].source_content}");
            ChoiceTextC = new BitmapImage(new Uri(urlc));

            ChoiceUserScore = "0 分";
            ChoiceTotalScore = $"{paperItemsItem.item_score} 分";
            OptionStyleA = "#333333";
            OptionStyleB = "#333333";
            OptionStyleC = "#333333";

            if (examItemResult.exam_score == null || examItemResult.exam_score == 0)
            {
                if (string.IsNullOrEmpty(examItemResult.user_answer))
                {
                    ChoiceUserScore = "未作答";
                }

                ChoiceUserScoreColor = "#ff6161";

                switch (examItemResult.user_answer)
                {
                    case "A":
                        OptionStyleA = "#ff6161";
                        break;
                    case "B":
                        OptionStyleB = "#ff6161";
                        break;
                    case "C":
                        OptionStyleC = "#ff6161";
                        break;
                }

                //if (examItemResult.user_answer == GetChoiceName(paperItemsItem.answers[0].answer_sort))
                //{
                //    OptionStyleA = "#ff6161";
                //}

                //if (examItemResult.user_answer == GetChoiceName(paperItemsItem.answers[1].answer_sort))
                //{
                //    OptionStyleB = "#ff6161";
                //}

                //if (examItemResult.user_answer == GetChoiceName(paperItemsItem.answers[2].answer_sort))
                //{
                //    OptionStyleC = "#ff6161";
                //}

                if (paperItemsItem.answers[0].answer_is_right == 1)
                {
                    OptionStyleA = "#41b790";
                }
                else if (paperItemsItem.answers[1].answer_is_right == 1)
                {
                    OptionStyleB = "#41b790";
                }
                else if (paperItemsItem.answers[2].answer_is_right == 1)
                {
                    OptionStyleC = "#41b790";
                }
            }
            else
            {
                ChoiceUserScoreColor = "#41b790";
                ChoiceUserScore = $"{examItemResult.exam_score} 分";
                //if (examItemResult.user_answer == GetChoiceName(paperItemsItem.answers[0].answer_sort))
                //{
                //    OptionStyleA = "#41b790";
                //}

                //if (examItemResult.user_answer == GetChoiceName(paperItemsItem.answers[1].answer_sort))
                //{
                //    OptionStyleB = "#41b790";
                //}

                //if (examItemResult.user_answer == GetChoiceName(paperItemsItem.answers[2].answer_sort))
                //{
                //    OptionStyleC = "#41b790";
                //}
                switch (examItemResult.user_answer)
                {
                    case "A":
                        OptionStyleA = "#41b790";
                        break;
                    case "B":
                        OptionStyleB = "#41b790";
                        break;
                    case "C":
                        OptionStyleC = "#41b790";
                        break;
                }
            }
        }



        private string GetChoiceName(int answer)
        {
            string name = "";
            if (answer == 1)
            {
                name = "A";
            }
            else if (answer == 2)
            {
                name = "B";
            }
            else if (answer == 3)
            {
                name = "C";
            }
            else if (answer == 4)
            {
                name = "D";
            }

            return name;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
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
using ST.Models.Score;

namespace Plugin.Exam.Qs.ViewModel.SubItem
{
    /// <summary>
    /// 听小对话 回答问题
    /// </summary>
    public class HistoryScoreItemViewModel : QsBaseViewModel
    {
        public const string ViewName = "HistoryScoreItemViewModel";
        private QsBaseViewModel ParentView;
        private string Result;

        public HistoryScoreItemViewModel()
        {
        }

        public HistoryScoreItemViewModel(QsBaseViewModel parentView, string titleName, SyncScoreDetailItem userScore) : this()
        {
            ParentView = parentView;

            TitleName = titleName;
            ScoreValue = $"{Convert.ToInt32(userScore.exam_score)} 分";
            AudioUrl = userScore.user_answer;
            Result = userScore.score_result;

            BindQsSubItemInfo();
        }

        public HistoryScoreItemViewModel(QsBaseViewModel parentView, string titleName, ScoreRoot userScore) : this()
        {
            ParentView = parentView;

            TitleName = titleName;
            ScoreValue = $"{Convert.ToInt32(userScore.result.overall)} 分";
            AudioUrl = userScore.audioUrl;
            Result = JsonHelper.ToJson(userScore);

            BindQsSubItemInfo();
        }

        public HistoryScoreItemViewModel(string titleName,string scoreValue,string audioUrl) : this()
        {
            TitleName = titleName;
            ScoreValue = scoreValue;
            AudioUrl = audioUrl;

            BindQsSubItemInfo();
        }

        public HistoryScoreItemViewModel(QsBaseViewModel parentView, string titleName,string scoreValue,string audioUrl ,string result) : this()
        {
            ParentView = parentView;

            TitleName = titleName;
            ScoreValue = scoreValue;
            AudioUrl = audioUrl;
            Result = result;

            BindQsSubItemInfo();
        }

        #region << 属性 字段 >>
        
        private string _TitleName;

        /// <summary>
        /// 名称
        /// </summary>
        public string TitleName
        {
            get
            {
                return _TitleName;
            }
            set
            {
                _TitleName = value;
                RaisePropertyChanged("TitleName");
            }
        }

        private string _ScoreValue;

        /// <summary>
        /// 得分
        /// </summary>
        public string ScoreValue
        {
            get
            {
                return _ScoreValue;
            }
            set
            {
                _ScoreValue = value;
                RaisePropertyChanged("ScoreValue");
            }
        }

        private string _AudioUrl;

        /// <summary>
        /// 播放 地址
        /// </summary>
        public string AudioUrl
        {
            get
            {
                return _AudioUrl;
            }
            set
            {
                _AudioUrl = value;
                RaisePropertyChanged("AudioUrl");
            }
        }


        #endregion

        #region << 按钮方法 >>

        private RelayCommand<string> _PlayUserAudioCommand;//录音 正常,打开 播放

        public RelayCommand<string> PlayUserAudioCommand
        {
            get
            {
                return _PlayUserAudioCommand ?? (_PlayUserAudioCommand = new RelayCommand<string>(s =>
                {
                    if (GlobalUser.WavePlayer?.PlaybackState == PlaybackState.Playing)
                    {
                        GlobalUser.WavePlayer?.Stop();
                        GlobalUser.WavePlayer = null;
                        PlayingIconEnable = false;
                        return;
                    }

                    PlayingIconEnable = true;
                    ParentView.ShowPlayingResult(Result);

                    PlayAudio1(AudioUrl);
                }));
            }
        }


        #endregion

        #region << 自定义方法 >>

        /// <summary>
        /// 加载题目
        /// </summary>
        /// <param name="item_id"></param>
        private void BindQsSubItemInfo()
        {  
        }
         

        #endregion 

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
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

namespace Plugin.Exam.Qs.ViewModel.SubItem
{
    /// <summary>
    /// 五星评价
    /// </summary>
    public class EvaluationScoreItemViewModel : QsBaseViewModel
    {
        public const string ViewName = "EvaluationScoreItemViewModel";

        public EvaluationScoreItemViewModel()
        {
        }

        public EvaluationScoreItemViewModel(string titleName,float scoreValue) : this()
        {
            TitleName = titleName;
            TotalStarFiveScore = SetStarScore(scoreValue);

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

        private ImageSource totalStarFiveScore;
        /// <summary>
        /// 五星
        /// </summary>
        public ImageSource TotalStarFiveScore
        {
            get { return totalStarFiveScore; }
            set
            {
                totalStarFiveScore = value;
                RaisePropertyChanged("TotalStarFiveScore");
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
            get { return _PlayUserAudioCommand ?? (_PlayUserAudioCommand = new RelayCommand<string>(s =>
                             { 
                             })); }
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

        private ImageSource SetStarScore(float avgScore)
        {
            float? AvgScore5Points = 5 * (avgScore / 100);
            string strScore = Convert.ToDouble(AvgScore5Points).ToString("0.00");

            int leftPoint = int.Parse(strScore.Substring(0, 1));
            int rightPoint = int.Parse(strScore.Substring(2, 2));

            if (AvgScore5Points > 0)
            {

                if (rightPoint > 0 && rightPoint <= 30)
                {
                    rightPoint = 25;
                }
                else if (rightPoint > 30 && rightPoint <= 60)
                {
                    rightPoint = 50;
                }
                else if (rightPoint > 60 && rightPoint <= 99)
                {
                    rightPoint = 75;
                }
                else if (rightPoint > 99)
                {
                    if (leftPoint < 5)
                        leftPoint = leftPoint + 1;

                    rightPoint = 00;
                }

                AvgScore5Points = float.Parse($"{leftPoint}.{rightPoint}");
            } 

            switch (AvgScore5Points)
            {
                case 0.0f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_0_0);
                case 0.25f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_0_1);
                case 0.5f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_0_2);
                case 0.75f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_0_3);
                case 1.0f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_1);
                case 1.25f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_1_1);
                case 1.5f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_1_2);
                case 1.75f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_1_3);
                case 2.0f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_2);
                case 2.25f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_2_1);
                case 2.5f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_2_2);
                case 2.75f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_2_3);
                case 3.0f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_3);
                case 3.25f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_3_1);
                case 3.5f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_3_2);
                case 3.75f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_3_3);
                case 4.0f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_4);
                case 4.25f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_4_1);
                case 4.5f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_4_2);
                case 4.75f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_4_3);
                case 5.0f:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_5);
                default:
                    return ConvertHelper.ChangeBitmapToImageSource(Properties.Resources.star_0_0);
            }

        }

        #endregion 

    }
}

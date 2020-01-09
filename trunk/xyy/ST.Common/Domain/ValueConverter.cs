﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace ST.Common.Domain
{

    public class ValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                if (value.ToString() == parameter.ToString())
                {
                    return true;
                }
            }
            return false;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                if ((bool)value)
                {
                    return parameter.ToString();
                }
            }
            return null;
        }

        #endregion
    }
    public class ResultChoiceValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {

                if (value.ToString() == "#41b790")
                {
                    return "#41b790";
                }
                if (value.ToString() == "#ff6161")
                {
                    return "#ff6161";
                }
            }
            return "#FFFFFF";
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                if ((bool)value)
                {
                    return parameter.ToString();
                }
            }
            return null;
        }

        #endregion
    }
    public class KindConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {

                if (value.ToString() == "#41b790")
                {
                    return "Check";
                }
                if (value.ToString() == "#ff6161")
                {
                    return "Close";
                }
            }
            return "Check";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 时间转换器。
    /// </summary>
    public class DateTimeToIntConverter : IValueConverter
    {
        /// <summary>
        /// 时间戳转换成时间。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long longTime = long.Parse(value + "0000");
            TimeSpan timeSpan = new TimeSpan(longTime);
            if (parameter != null)
            {
                return dtStart.Add(timeSpan).ToString(parameter.ToString());
            }
            else
            {
                return dtStart.Add(timeSpan).ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateTime = System.Convert.ToDateTime(value);
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            long longTime = (dateTime.Ticks - startTime.Ticks) / 10000;   // 除10000调整为13位      
            return longTime;
        }
    }


    public class PlayControlIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {

                if (value.ToString() == parameter.ToString())
                {
                    return "Stop";
                }
                if (value.ToString() == "Play")
                {
                    return "Play";
                }
            }
            return "Play";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ScoreEvaluationValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    int score = int.Parse(value.ToString());
                    //    差：f44116
                    //    中：ff8414
                    //    良：1394fa
                    //    优：41b612
                    if (score > 75)
                    {
                        return "#41b612";
                    }
                    else if (score > 60 && score <= 75)
                    {
                        return "#1394fa";
                    }
                    else if (score > 30 && score <= 60)
                    {
                        return "#ff8414";
                    }
                    else
                    {
                        return "#f44116";
                    }
                }
            }
            catch (Exception)
            {
                return "#41b790";
            }
            return "#41b790";
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                if ((bool)value)
                {
                    return parameter.ToString();
                }
            }
            return null;
        }

        #endregion
    }
}
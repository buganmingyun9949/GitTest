using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ST.Style.ControlEx
{
    public class ContentViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                try
                {
                    if (value.GetType().Name == "TextBlock")
                    {
                        TextBlock tb = value as TextBlock;
                        return tb.Text;
                    }
                }
                catch (Exception ex)
                {
                    return value;
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

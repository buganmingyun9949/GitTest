using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace ST.Common.Domain
{
    public class DataConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public static object ConverterObject;

        public object Convert(object[] values, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            ConverterObject = values;

            return values.ToList();
        }

        public object[] ConvertBack(object value, Type[] targetTypes,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


    public class ImageRadiusXYDataConverter : IValueConverter
    { 
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int value1 = System.Convert.ToInt32(value, CultureInfo.InvariantCulture);

                int value2 = System.Convert.ToInt32(value1 / 2) - 1;
                
                return value2;
            }
            catch (FormatException)
            {
                return Binding.DoNothing;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        } 
    }

    public class ImageRadiusPointValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 2 && values[0] != null && values[1] != null)
            {
                double x, y;
                if (double.TryParse(values[0].ToString(), out x) &&
                    double.TryParse(values[1].ToString(), out y))

                    return new Point(x, y);
            }

            return new Point();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value is Point)
            {
                var point = (Point)value;
                return new object[] { point.X, point.Y };
            }

            return new object[0];
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
}

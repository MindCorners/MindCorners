using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MindCorners.Helpers.Converters
{   public class BooleanToColorConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public Color TrueValue { get; set; }
        public Color FalseValue { get; set; }


        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            var castedValue = bool.Parse(value.ToString());
            return (castedValue ? TrueValue : FalseValue);
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            return false;
        }

        #endregion
    }
}

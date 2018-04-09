using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MindCorners.Helpers.Converters
{   public class IntToVisibilityConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            var position = int.Parse(parameter.ToString());
            var length = int.Parse(value.ToString());
            return position == 0 ? false : true;
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

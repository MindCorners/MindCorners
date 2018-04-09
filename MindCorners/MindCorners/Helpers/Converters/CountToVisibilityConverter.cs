using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MindCorners.Helpers.Converters
{   public class CountToVisibilityConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
           
            int count=0;
            int.TryParse(value.ToString(), out count);
            return count > 0;
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

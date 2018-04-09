using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MindCorners.Helpers.Converters
{   public class IndexToSequentialNumberConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            
            var index = int.Parse(value.ToString());
            return index+1;
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            return 1;
        }

        #endregion
    }
}

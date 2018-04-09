using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MindCorners.Helpers.Converters
{   public class ListViewRowCountToHeightConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            var rowHeigth = int.Parse(parameter.ToString());
            var rowsCount = int.Parse(value.ToString());
            return rowHeigth*rowsCount;
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            return 0;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MindCorners.Helpers.Converters
{
    public class NullImageToDefaultConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
           System.Globalization.CultureInfo culture)
        {
            if (value ==null)
            {
                return "images.png";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return false;
        }

        #endregion
    }
}

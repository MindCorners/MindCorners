
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
    public class BooleanToImageConverter : IValueConverter
    {
        #region IValueConverter Members
        public string DisabledPrefix { get; set; }
        public object Convert(object value, Type targetType, object parameter,
           System.Globalization.CultureInfo culture)
        {
            var castedValue = bool.Parse(value.ToString());
            var imageName = parameter.ToString();
            return (castedValue ? imageName : DisabledPrefix+ imageName);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return false;
        }

        #endregion
    }
}

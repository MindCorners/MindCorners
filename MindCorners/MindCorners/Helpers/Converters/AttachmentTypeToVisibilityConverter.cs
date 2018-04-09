using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Models.Enums;
using Xamarin.Forms;

namespace MindCorners.Helpers.Converters
{
    public class AttachmentTypeToVisibilityConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            var type = int.Parse(value.ToString());
            var controlType = int.Parse(parameter.ToString());
            return type == controlType;
            //switch (type)
            //{
            //    case (int)ChatType.Text:
            //        return true;
            //    case (int)ChatType.Text:
            //        return true;
            //    case (int)ChatType.Text:
            //        return true;
            //    case (int)ChatType.Text:
            //        return true;
            //}
           // return position == 0 ? false : true;
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

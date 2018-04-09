using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Models.Enums;
using Xamarin.Forms;

namespace MindCorners.Helpers.Converters
{   public class ChatTypeToDataTemplateNameConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            var chatType = int.Parse(value.ToString());
            switch (chatType)
            {
                case (int)ChatType.Text:
                    return "TextTemplate";
                case (int)ChatType.Image:
                    return "ImageTemplate";
                case (int)ChatType.Audio:
                    return "AudioTemplate";
                case (int)ChatType.Video:
                    return "VideoTemplate";
                default: return "TextTemplate";
            }
           
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

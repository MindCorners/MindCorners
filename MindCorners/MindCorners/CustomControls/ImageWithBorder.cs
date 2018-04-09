using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MindCorners.CustomControls
{
    public class ImageWithBorder : Image
    {
        public static readonly BindableProperty BorderColorProperty =
          BindableProperty.Create("BorderColor", typeof(string), typeof(ImageWithBorder), null, BindingMode.OneWay);

        public string BorderColor
        {
            get { return (string)GetValue(BorderColorProperty); }
            set
            {
                if (value != BorderColor)
                {
                    SetValue(BorderColorProperty, value);
                }
            }
        }

        public static readonly BindableProperty BorderWidthProperty =
          BindableProperty.Create("BorderWidth", typeof(float), typeof(ImageWithBorder),0, BindingMode.OneWay);

        public float BorderWidth
        {
            get { return (float)GetValue(BorderWidthProperty); }
            set
            {
                if (value != BorderWidth)
                {
                    SetValue(BorderWidthProperty, value);
                }
            }
        }
    }
}

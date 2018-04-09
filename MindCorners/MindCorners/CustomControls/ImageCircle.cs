using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MindCorners.CustomControls
{
    public class ImageCircle : Image
    {
        public static readonly BindableProperty BorderColorProperty =
          BindableProperty.Create("BorderColor", typeof(Color), typeof(ImageCircle), Color.Transparent, BindingMode.OneWay);

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set
            {
                if (value != BorderColor)
                {
                    SetValue(BorderColorProperty, value);
                }
            }
        }

        public static readonly BindableProperty BorderWidthProperty =
        BindableProperty.Create("BorderWidth", typeof(int), typeof(ImageCircle), 5, BindingMode.OneWay);

        public int BorderWidth
        {
            get { return (int)GetValue(BorderWidthProperty); }
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

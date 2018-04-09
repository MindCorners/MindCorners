using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MindCorners.CustomControls
{
    public class CircleButton : Button
    {
        public static readonly BindableProperty CircleBorderColorProperty =
          BindableProperty.Create("CircleBorderColor", typeof(Color), typeof(ImageCircle), Color.Transparent, BindingMode.OneWay);

        public Color CircleBorderColor
        {
            get { return (Color)GetValue(CircleBorderColorProperty); }
            set
            {
                if (value != CircleBorderColor)
                {
                    SetValue(CircleBorderColorProperty, value);
                }
            }
        }

        public static readonly BindableProperty CircleBorderWidthProperty =
        BindableProperty.Create("CircleBorderWidth", typeof(int), typeof(ImageCircle), 5, BindingMode.OneWay);

        public int CircleBorderWidth
        {
            get { return (int)GetValue(CircleBorderWidthProperty); }
            set
            {
                if (value != CircleBorderWidth)
                {
                    SetValue(CircleBorderWidthProperty, value);
                }
            }
        }
    }
}

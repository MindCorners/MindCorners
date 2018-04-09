using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MindCorners.CustomControls
{
    public class CustomFrame : Frame
    {
        public static readonly BindableProperty ShadowOffsetXProperty = BindableProperty.Create("ShadowOffsetX", typeof(float), typeof(CustomFrame), 0f);
        public float ShadowOffsetX
        {
            get { return (float)GetValue(ShadowOffsetXProperty); }
            set { SetValue(ShadowOffsetXProperty, value); }
        }

        public static readonly BindableProperty ShadowOffsetYProperty = BindableProperty.Create("ShadowOffsetY", typeof(float), typeof(CustomFrame), 0f);
        public float ShadowOffsetY
        {
            get { return (float)GetValue(ShadowOffsetYProperty); }
            set { SetValue(ShadowOffsetYProperty, value); }
        }

      
        public static readonly BindableProperty ShadowOpacityProperty = BindableProperty.Create("ShadowOpacity", typeof(float), typeof(CustomFrame), 1f);
        public float ShadowOpacity
        {
            get { return (float)GetValue(ShadowOpacityProperty); }
            set { SetValue(ShadowOpacityProperty, value); }
        }

        public static readonly BindableProperty ShadowRadiusProperty = BindableProperty.Create("ShadowRadius", typeof(float), typeof(CustomFrame), 0f);
        public float ShadowRadius
        {
            get { return (float)GetValue(ShadowRadiusProperty); }
            set { SetValue(ShadowRadiusProperty, value); }
        }
        
        public static readonly BindableProperty ShadowColorProperty = BindableProperty.Create("ShadowColor", typeof(Color), typeof(CustomFrame), Color.Transparent);
        public Color ShadowColor
        {
            get { return (Color)GetValue(ShadowColorProperty); }
            set { SetValue(ShadowColorProperty, value); }
        }
    }
}

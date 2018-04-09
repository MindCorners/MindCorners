using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;

namespace MindCorners.Droid.CustomControl.CustomRenderer
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {   
            base.OnElementChanged(e);

            if (Control != null)
            {
                DrawBorder();
            }
        }

        void DrawBorder()
        {
            //FrameLayout borderLayer = new FrameLayout();
            //borderLayer.Background.BackgroundColor = Color.FromRGB(23, 162, 227).CGColor;
            //borderLayer.Frame = new CGRect(0, 45, 335, 5);
            //View.AddSublayer(borderLayer);
        }
    }
}
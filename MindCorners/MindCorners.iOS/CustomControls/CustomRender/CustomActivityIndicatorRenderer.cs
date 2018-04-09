using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace MindCorners.iOS.CustomControls.CustomRender
{
    public class CustomActivityIndicatorRenderer : ViewRenderer<ActivityIndicator, UIActivityIndicatorView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ActivityIndicator> e)
        {
            base.OnElementChanged(e);

            Control.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.WhiteLarge;
        }
    }
}
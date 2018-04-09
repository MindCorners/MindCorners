using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Foundation;
using MindCorners.CustomControls;
using MindCorners.iOS.CustomControls.CustomRender;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(RoundedBox), typeof(RoundedBoxRenderer))]
namespace MindCorners.iOS.CustomControls.CustomRender
{
    public class RoundedBoxRenderer : BoxRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);

            if (Element != null)
            {
                Layer.MasksToBounds = true;
                UpdateCornerRadius(Element as RoundedBox);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == RoundedBox.CornerRadiusProperty.PropertyName)
            {
                UpdateCornerRadius(Element as RoundedBox);
            }
        }

        void UpdateCornerRadius(RoundedBox box)
        {
            Layer.CornerRadius = (float)box.CornerRadius;
        }
    }
}
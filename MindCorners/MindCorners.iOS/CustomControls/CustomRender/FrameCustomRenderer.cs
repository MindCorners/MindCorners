using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using MindCorners.CustomControls;
using MindCorners.iOS.CustomControls.CustomRender;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomFrame), typeof(FrameCustomRenderer))]

namespace MindCorners.iOS.CustomControls.CustomRender
{
    public class FrameCustomRenderer :FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            //Layer.ShadowOffset = new CGSize(0, 3);
            //Layer.ShadowOpacity = 1;
            //Layer.ShadowRadius = 5;
            //Layer.ShadowColor = new CGColor(red: (float)0.88, green: (float)0.88, blue: (float)0.89, alpha: (float)0.86);
            //Layer.CornerRadius = 9;

            var newElement = (CustomFrame) Element;
            if (newElement != null)
            {
                Layer.ShadowOffset = new CGSize(newElement.ShadowOffsetX, newElement.ShadowOffsetY);
                Layer.ShadowOpacity = newElement.ShadowOpacity;
                Layer.ShadowRadius = newElement.ShadowRadius;
                Layer.ShadowColor = newElement.ShadowColor.ToCGColor();
                Layer.CornerRadius = newElement.CornerRadius;
            }
        }

        //public override void Draw(CGRect rect)
        //{
        //    base.Draw(rect);
          
        //}
    }
}
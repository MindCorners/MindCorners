using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using LabelRenderer = MindCorners.iOS.CustomControls.CustomRender.LabelRenderer;

//[assembly: ExportRenderer(typeof(Label), typeof(LabelRenderer))]
namespace MindCorners.iOS.CustomControls.CustomRender
{
    public class LabelRenderer : Xamarin.Forms.Platform.iOS.LabelRenderer
    {
       // protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
       // {
       //     base.OnElementChanged(e);
            //Element.FontSize = e.OldElement.FontSize;
           // Control.SizeToFit();
			//Control.Bounds = new CGRect (0, 0, Control.Bounds.Width, Control.Bounds.Height + 22);
       // }
    }
}
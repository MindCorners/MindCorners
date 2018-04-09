using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreAnimation;
using Foundation;
using MindCorners.CustomControls;
using MindCorners.iOS.CustomControl.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace MindCorners.iOS.CustomControl.CustomRenderer
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control == null || Element == null || e.OldElement != null) return;

            var element = (CustomEntry)Element;
            // var ourCustomColorHere = element.BorderColor.ToAndroid();

            //var borderLayer = new CALayer();
            //borderLayer.MasksToBounds = true;
            //borderLayer.Frame = new CoreGraphics.CGRect(0f, Frame.Height / 2, Frame.Width, 1f);
            //borderLayer.BorderColor = element.BorderColor.ToCGColor();
            //borderLayer.BorderWidth = 1.0f;
            //borderLayer.CornerRadius = 1;
            //Control.Layer.AddSublayer(borderLayer);
            //Control.BorderStyle = UITextBorderStyle.None;
            //Control.TextColor = element.BorderColor.ToUIColor();


            //var shape = new ShapeDrawable(new RectShape());
            //shape.Paint.Alpha = 0;
            //shape.Paint.Color=Color.Azure;
            //shape.Paint.SetStyle(Paint.Style.Stroke);
            //Control.ba.SetBackgroundDrawable(shape);

            //var customColor = Xamarin.Forms.Color.Chocolate;//.FromHex("#0F9D58");
            // Control.bo.Background.SetColorFilter(customColor.ToAndroid(), PorterDuff.Mode.SrcAtop);

            var borderLayer = new CALayer();
            borderLayer.MasksToBounds = true;
            borderLayer.Frame = new CoreGraphics.CGRect(0f, Frame.Height/2, Frame.Width, 1f);
            borderLayer.BorderColor = element.BorderColor.ToCGColor();
            
            borderLayer.BorderWidth = 1.0f;

            Control.Layer.AddSublayer(borderLayer);
            Control.BorderStyle = UITextBorderStyle.None;
            //var placeholderString = new NSAttributedString(element.Placeholder, new UIStringAttributes { ForegroundColor = element.BorderColor.ToUIColor() });
            //Control.AttributedPlaceholder = placeholderString;
            //var textString = new NSAttributedString(element.Text, new UIStringAttributes { ForegroundColor = element.TextColor.ToUIColor() });
            //Control.AttributedText = textString;
        }
    }
}
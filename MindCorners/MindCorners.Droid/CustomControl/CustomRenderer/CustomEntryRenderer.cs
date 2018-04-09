using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MindCorners.CustomControls;
using MindCorners.Droid.CustomControl.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace MindCorners.Droid.CustomControl.CustomRenderer
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Control == null || Element == null || e.OldElement != null) return;

            var element = (CustomEntry)Element;
            var ourCustomColorHere = element.BorderColor.ToAndroid();
            Control.Background.SetColorFilter(ourCustomColorHere, PorterDuff.Mode.SrcAtop);




            //var shape = new ShapeDrawable(new RectShape());
            //shape.Paint.Alpha = 0;
            //shape.Paint.Color=Color.Azure;
            //shape.Paint.SetStyle(Paint.Style.Stroke);
            //Control.ba.SetBackgroundDrawable(shape);

            //var customColor = Xamarin.Forms.Color.Chocolate;//.FromHex("#0F9D58");
           // Control.bo.Background.SetColorFilter(customColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
        }
    }
}
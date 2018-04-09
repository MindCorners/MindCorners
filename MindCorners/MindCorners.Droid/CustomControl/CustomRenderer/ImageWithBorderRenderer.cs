using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MindCorners.CustomControls;
using MindCorners.Droid.CustomControl;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(ImageWithBorder), typeof(ImageWithBorderRenderer))]
namespace MindCorners.Droid.CustomControl
{
    public class ImageWithBorderRenderer : ImageRenderer
    {
        //...Implementation
        protected override bool DrawChild(Canvas canvas, View child, long drawingTime)
        {
            try
            {
                //Create path to clip
                var path = new Path();
                path.AddRect(0, 0, Width, Height, Path.Direction.Ccw);
                canvas.Save();
                canvas.ClipPath(path);

                var result = base.DrawChild(canvas, child, drawingTime);

                canvas.Restore();

                // Create path for circle border
                path = new Path();
                path.AddRect(0, 0, Width, Height, Path.Direction.Ccw);

                var paint = new Paint();
                paint.AntiAlias = true;
               
                //paint.StrokeWidth = ((ImageWithBorder)Element).BorderWidth;
                paint.StrokeWidth = 15;
                paint.SetStyle(Paint.Style.Stroke);
                paint.Color = Android.Graphics.Color.ParseColor(((ImageWithBorder)Element).BorderColor);
                canvas.DrawPath(path, paint);

                //Properly dispose
                paint.Dispose();
                path.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine("Unable to create circle image: " + ex);
            }

            return base.DrawChild(canvas, child, drawingTime);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {

                if ((int)Android.OS.Build.VERSION.SdkInt < 18)
                    SetLayerType(LayerType.Software, null);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using MindCorners.CustomControls;
using MindCorners.iOS.CustomControl;
using MindCorners.iOS.CustomControl.CustomRender;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.PlatformConfiguration;

[assembly: ExportRenderer(typeof(ImageWithBorder), typeof(ImageWithBorderRenderer))]
namespace MindCorners.iOS.CustomControl.CustomRender
{
    public class ImageWithBorderRenderer : ImageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
        }
        private void CreateCircle()
        {
            try
            {
                Control.Layer.MasksToBounds = false;
                Control.Layer.BorderColor = Color.FromHex(((ImageWithBorder)Element).BorderColor).ToCGColor();
                Control.Layer.BorderWidth = ((ImageWithBorder)Element).BorderWidth;
                Control.ClipsToBounds = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to create circle image: " + ex);
            }
        }

        ////...Implementation
        //protected override bool DrawChild(Canvas canvas, View child, long drawingTime)
        //{
        //    try
        //    {
        //        //Create path to clip
        //        var path = new Path();
        //        path.AddRect(0, 0, Width, Height, Path.Direction.Ccw);
        //        canvas.Save();
        //        canvas.ClipPath(path);

        //        var result = base.DrawChild(canvas, child, drawingTime);

        //        canvas.Restore();

        //        // Create path for circle border
        //        path = new Path();
        //        path.AddRect(0, 0, Width, Height, Path.Direction.Ccw);

        //        var paint = new Paint();
        //        paint.AntiAlias = true;
               
        //        //paint.StrokeWidth = ((ImageWithBorder)Element).BorderWidth;
        //        paint.StrokeWidth = 15;
        //        paint.SetStyle(Paint.Style.Stroke);
        //        paint.Color = Android.Graphics.Color.ParseColor(((ImageWithBorder)Element).BorderColor);
        //        canvas.DrawPath(path, paint);

        //        //Properly dispose
        //        paint.Dispose();
        //        path.Dispose();
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Debug.WriteLine("Unable to create circle image: " + ex);
        //    }

        //    return base.DrawChild(canvas, child, drawingTime);
        //}
    }
}
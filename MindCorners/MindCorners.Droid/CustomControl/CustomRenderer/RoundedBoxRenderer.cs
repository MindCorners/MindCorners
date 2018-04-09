using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using MindCorners.Droid.CustomControl.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RoundedBox), typeof(RoundedBoxRenderer))]
namespace MindCorners.Droid.CustomControl.CustomRenderer
{
    public class RoundedBoxRenderer : BoxRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
        {
            base.OnElementChanged(e);

            SetWillNotDraw(false);

            Invalidate();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == RoundedBox.CornerRadiusProperty.PropertyName)
            {
                Invalidate();
            }
        }

        public override void Draw(Canvas canvas)
        {
            var box = Element as RoundedBox;
            var rect = new Rect();
            var paint = new Paint()
            {
                Color = box.BackgroundColor.ToAndroid(),
                AntiAlias = true,
            };

            GetDrawingRect(rect);

            var radius = (float)(rect.Width() / box.Width * box.CornerRadius);

              canvas.DrawRoundRect(new RectF(rect), radius, radius, paint);

            //var path = new Path();
            //path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);
            //canvas.Save();
            //canvas.ClipPath(path);

            //canvas.DrawPath(path, paint);

            //  base.Draw(canvas);
        }
    }
}
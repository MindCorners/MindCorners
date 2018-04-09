using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MindCorners.CustomControls;
using MindCorners.iOS.CustomControl.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CircleButton), typeof(CircleButtonRenderer))]
namespace MindCorners.iOS.CustomControl.CustomRenderer
{

    public class CircleButtonRenderer
    {
    }
   /* :

ButtonRenderer
    {
        private GradientDrawable _normal, _pressed;
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            //Control.TitleEdgeInsets = new UIEdgeInsets(4, 4, 4, 4);
            //Control.Text..LineBreakMode = UILineBreakMode.WordWrap;
            //Control.TitleLabel.TextAlignment = UITextAlignment.Center;


            //    if (Control != null)
            //    {
            //        var button = (CircleButton)e.NewElement;

            //        button.SizeChanged += (s, args) =>
            //        {
            //            var radius = (float)Math.Min(button.Width, button.Height);

            //            // Create a drawable for the button's normal state
            //            _normal = new Android.Graphics.Drawables.GradientDrawable();

            //            if (button.BackgroundColor.R == -1.0 && button.BackgroundColor.G == -1.0 && button.BackgroundColor.B == -1.0)
            //                _normal.SetColor(Android.Graphics.Color.ParseColor("#ff2c2e2f"));
            //            else
            //                _normal.SetColor(button.BackgroundColor.ToAndroid());

            //            _normal.SetCornerRadius(radius);

            //            // Create a drawable for the button's pressed state
            //            _pressed = new Android.Graphics.Drawables.GradientDrawable();
            //            var highlight = Context.ObtainStyledAttributes(new int[] { Android.Resource.Attribute.ColorActivatedHighlight }).GetColor(0, Android.Graphics.Color.Gray);
            //            _pressed.SetColor(highlight);
            //            _pressed.SetCornerRadius(radius);

            //            // Add the drawables to a state list and assign the state list to the button
            //            var sld = new StateListDrawable();
            //            sld.AddState(new int[] { Android.Resource.Attribute.StatePressed }, _pressed);
            //            sld.AddState(new int[] { }, _normal);
            //            Control.SetBackgroundDrawable(sld);
            //        };
            //    }
        }



        protected override bool DrawChild(Canvas canvas, View child, long drawingTime)
        {
            try
            {
                var radius = Math.Min(Width, Height) / 2;
                var strokeWidth = 10;
                radius -= strokeWidth / 2;

                //Create path to clip
                var path = new Path();
                path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);
                canvas.Save();
                canvas.ClipPath(path);

                var result = base.DrawChild(canvas, child, drawingTime);

                //canvas.Restore();

                ////// Create path for circle border
                //path = new Path();
                //path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);

                //var paint = new Paint();
                //paint.AntiAlias = true;
                //paint.StrokeWidth = ((CircleButton)Element).CircleBorderWidth;
                //paint.SetStyle(Paint.Style.Stroke);
                //paint.Color = ((CircleButton)Element).CircleBorderColor.ToAndroid();

                //canvas.DrawPath(path, paint);

                //////Properly dispose
                //paint.Dispose();
                //path.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine("Unable to create circle image: " + ex);
            }

            return base.DrawChild(canvas, child, drawingTime);

            //try
            //{
            //    var radius = Math.Min(Width, Height) / 2;

            //    var strokeWidth = ((CircleButton)Element).CircleBorderWidth;
            //    radius -= strokeWidth / 2;
            //    //Create path to clip
            //    var path = new Path();
            //    path.AddCircle(Width / 2, Height / 2, (float)radius, Path.Direction.Ccw);
            //    canvas.Save();
            //    canvas.ClipPath(path);

            //    var result = base.DrawChild(canvas, child, drawingTime);


            //    //canvas.Restore();

            //    //// Create path for circle border
            //    //path = new Path();
            //    //path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);

            //    //var paint = new Paint();
            //    //paint.AntiAlias = true;
            //    //paint.StrokeWidth = strokeWidth;
            //    //paint.SetStyle(Paint.Style.Stroke);
            //    //paint.Color = ((CircleButton)Element).CircleBorderColor.ToAndroid();

            //    //canvas.DrawPath(path, paint);

            //    ////Properly dispose
            //    //paint.Dispose();
            //    //path.Dispose();

            //    //canvas.Restore();

            //    //// Create path for circle border
            //    //path = new Path();
            //    //path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);

            //    //var paint = new Paint();
            //    //paint.AntiAlias = true;
            //    //var button = ((CircleButton)Element);
            //    //Android.Graphics.Color color;
            //    //if (button.BackgroundColor.R == -1.0 && button.BackgroundColor.G == -1.0 && button.BackgroundColor.B == -1.0)
            //    //    color = (Android.Graphics.Color.ParseColor("#ff2c2e2f"));
            //    //else
            //    //    color = (button.BackgroundColor.ToAndroid());

            //    //paint.Color = color;

            //    ////canvas.DrawPath(path, paint);
            //    //canvas.DrawCircle(0, 0, radius, paint);

            //    ////Properly dispose
            //    //paint.Dispose();
            //    //path.Dispose();
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    //Debug.WriteLine("Unable to create circle image: " + ex);
            //}

            //return base.DrawChild(canvas, child, drawingTime);
        }


    }*/
}
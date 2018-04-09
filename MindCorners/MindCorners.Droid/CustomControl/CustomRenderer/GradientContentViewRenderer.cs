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
using Xamarin.Forms.PlatformConfiguration;
using Color = Xamarin.Forms.Color;
[assembly: ExportRenderer(typeof(GradientContentView), typeof(GradientContentViewRenderer))]
namespace MindCorners.Droid.CustomControl.CustomRenderer
{
    class GradientContentViewRenderer : ViewRenderer<GradientContentView, Android.Views.View>
    {
        public GradientDrawable GradientDrawable { get; set; }
        public ShapeDrawable ShapeDrawable { get; set; }
        /// <summary>
        /// Gets the underlying element typed as an <see cref="GradientContentView"/>.
        /// </summary>
        private GradientContentView GradientContentView
        {
            get { return (GradientContentView)Element; }
        }

        /// <summary>
        /// Setup the gradient layer
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<GradientContentView> e)
        {
            base.OnElementChanged(e);

            if (GradientContentView != null)
            {
                ShapeDrawable = new ShapeDrawable(new RoundRectShape(new float[] { 8, 8, 8, 8, 8, 8, 8, 8 }, null, null));

                GradientDrawable = new GradientDrawable();
                GradientDrawable.SetColors(new int[] { GradientContentView.StartColor.ToAndroid(), GradientContentView.EndColor.ToAndroid() });
                
                //this.SetBackgroundResource(Resource.Drawable.);

                //    var gradient1 = new GradientDrawable(GradientDrawable.Orientation.TopBottom, new[] {
                //    Color.FromRgba(255, 255, 255, 255).ToAndroid().ToArgb(),
                //    Color.FromRgba(70, 70, 70, 50).ToAndroid().ToArgb()
                //});
                // GradientDrawable.SetCornerRadius(270);

                // var element = (GradientContentView)e.NewElement;
                // var linearGradientBrush = new LinearGradientBrush();
                // var linearGradientBrush = new LinearGradientBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString("#FFDFD991"), (System.Windows.Media.Color)ColorConverter.ConvertFromString("#FFDFD991"),(double)220);
                //var linearGradientBrush = new  LinearGradientBrush(new GradientStopCollection()
                //{ new GradientStop() { Color =new System.Windows.Media.Color.()  },
                //new GradientStop() { Color = new Windows.UI.Color() { A = Convert.ToByte((element.EndColor.A)*255), B = Convert.ToByte((element.EndColor.B)*255), G = Convert.ToByte((element.EndColor.G)*255), R= Convert.ToByte((element.EndColor.R)*255) }, Offset = 1 } }, 90);
                //linearGradientBrush.StartPoint = new Windows.Foundation.Point(0, 0);
                //linearGradientBrush.EndPoint = new Windows.Foundation.Point(0, 1);

                // GradientDrawable.SetOrientation(GradientDrawable.Orientation.BottomTop);
                // Background = GradientDrawable;
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (GradientDrawable != null && GradientContentView != null)
            {

                if (e.PropertyName == GradientContentView.StartColorProperty.PropertyName ||
                    e.PropertyName == GradientContentView.EndColorProperty.PropertyName)
                {
                    GradientDrawable.SetColors(new int[] { GradientContentView.StartColor.ToAndroid(), GradientContentView.EndColor.ToAndroid() });
                    Invalidate();
                }

                if (e.PropertyName == VisualElement.WidthProperty.PropertyName ||
                    e.PropertyName == VisualElement.HeightProperty.PropertyName ||
                    e.PropertyName == GradientContentView.OrientationProperty.PropertyName)
                {
                    Invalidate();
                }
            }
        }

        protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime)
        {

            var box = (GradientContentView)Element;
            //var radius = Math.Min(box.Width, Height) / 2;
            //var strokeWidth = 10;
            //radius -= strokeWidth / 2;

            ////Create path to clip
            //var path = new Path();
            //path.AddRoundRect(0,0,0,0, (float)box.Width, (float)box.Height, Path.Direction.Ccw);
            //canvas.Save();
            //canvas.ClipPath(path);

            //var result = base.DrawChild(canvas, child, drawingTime);

            //canvas.Restore();

            //canvas.S
            //// Create path for circle border
            //path = new Path();
            //path.AddCircle(Width / 2, Height / 2, radius, Path.Direction.Ccw);

            ////var paint = new Paint();
            ////paint.AntiAlias = true;
            ////paint.StrokeWidth = 5;
            ////paint.SetStyle(Paint.Style.Stroke);
            ////paint.Color = Android.Graphics.Color.ParseColor(((ImageCircle)Element).BorderColor);

            //canvas.DrawPath(path, paint);

            ////Properly dispose
            //paint.Dispose();
            //path.Dispose();
            //return result;

            //var path = new RoundRectShape(new float[] { 8, 8, 8, 8, 8, 8, 8, 8 }, null, null);
            var path = new Path();
            path.AddRoundRect(new RectF(0, 0, Width, Height), box.CornerRadius, box.CornerRadius, Path.Direction.Ccw);
            double width = 0;
            double height = 0;
            if (box.Orientation == GradientOrientation.Horizontal)
            {
                width = Width;
            }
            else
            {
                height = Height;
            }
            // var gradientDrawable =  new GradientDrawable(GradientDrawable.Orientation.BlTr, );

            //var gradient = new Android.Graphics.LinearGradient((float)box.StartPointX, (float)box.StartPointY, (float)box.EndPointX, (float)box.EndPointY,
            float StartPointX, StartPointY, EndPointX, EndPointY = 0;

            StartPointX = ((float)Width * (float) box.StartPointX);
            StartPointY = ((float)Height * (float)box.StartPointY);
            EndPointX = ((float)Width * (float)box.EndPointX);
            EndPointY = ((float)Height * (float)box.EndPointY);

            //var gradient = new Android.Graphics.LinearGradient((float)box.StartPointX, (float)box.StartPointY, (float)width, (float)height,
            var gradient = new Android.Graphics.LinearGradient(StartPointX, StartPointY, EndPointX, EndPointY,

              box.StartColor.ToAndroid(),
              box.EndColor.ToAndroid(),
              Android.Graphics.Shader.TileMode.Clamp);

            var paint = new Android.Graphics.Paint()
            {
                Dither = true,
            };



            paint.SetShader(gradient);
            canvas.DrawPath(path, paint);

            // canvas.DrawPath();

            //GradientDrawable.Bounds = canvas.ClipBounds;
            //GradientDrawable.SetOrientation(GradientContentView.Orientation == GradientOrientation.Vertical
            //    ? GradientDrawable.Orientation.TopBottom
            //    : GradientDrawable.Orientation.LeftRight);
            //GradientDrawable.Draw(canvas);





            return base.DrawChild(canvas, child, drawingTime);
        }
    }
}
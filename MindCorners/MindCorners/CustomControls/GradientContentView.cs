using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MindCorners.CustomControls
{
    /// <summary>
    /// Enum GradientOrientation
    /// </summary>
    public enum GradientOrientation
    {
        /// <summary>
        /// The vertical
        /// </summary>
        Vertical,
        /// <summary>
        /// The horizontal
        /// </summary>
        Horizontal
    }

    /// <summary>
    /// ContentView that allows you to have a Gradient for
    /// the background. Let there be Gradients!
    /// </summary>
    public class GradientContentView : ContentView
    {
        /// <summary>
        /// Start color of the gradient
        /// Defaults to White
        /// </summary>
        public GradientOrientation Orientation
        {
            get { return (GradientOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// The orientation property
        /// </summary>
        public static readonly BindableProperty OrientationProperty =
            BindableProperty.Create<GradientContentView, GradientOrientation>(x => x.Orientation, GradientOrientation.Vertical, BindingMode.OneWay);



        //public int CornerRadius
        //{
        //    get { return (int)GetValue(CornerRadiusProperty); }
        //    set { SetValue(CornerRadiusProperty, value); }
        //}

        ///// <summary>
        ///// The orientation property
        ///// </summary>
        //public static readonly BindableProperty CornerRadiusProperty =
        //    BindableProperty.Create<GradientContentView, int>(x => x.CornerRadius, 0, BindingMode.OneWay);


        /// <summary>
        /// Start color of the gradient
        /// Defaults to White
        /// </summary>
        public Color StartColor
        {
            get { return (Color)GetValue(StartColorProperty); }
            set { SetValue(StartColorProperty, value); }
        }

        /// <summary>
        /// Using a BindableProperty as the backing store for StartColor.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly BindableProperty StartColorProperty =
            BindableProperty.Create<GradientContentView, Color>(x => x.StartColor, Color.White, BindingMode.OneWay);

        /// <summary>
        /// End color of the gradient
        /// Defaults to Black
        /// </summary>
        public Color EndColor
        {
            get { return (Color)GetValue(EndColorProperty); }
            set { SetValue(EndColorProperty, value); }
        }

        /// <summary>
        /// Using a BindableProperty as the backing store for EndColor.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly BindableProperty EndColorProperty = BindableProperty.Create<GradientContentView, Color>(x => x.EndColor, Color.Black, BindingMode.OneWay);


        public static readonly BindableProperty StartPointXProperty = BindableProperty.Create("StartPointX", typeof(double), typeof(GradientContentView), (double)0);
        public double StartPointX
        {
            get { return (double)GetValue(StartPointXProperty); }
            set { SetValue(StartPointXProperty, value); }
        }

        public static readonly BindableProperty StartPointYProperty = BindableProperty.Create("StartPointY", typeof(double), typeof(GradientContentView), (double)0);
        public double StartPointY
        {
            get { return (double)GetValue(StartPointYProperty); }
            set { SetValue(StartPointYProperty, value); }
        }

        public static readonly BindableProperty EndPointXProperty = BindableProperty.Create("EndPointX", typeof(double), typeof(GradientContentView), (double)1);
        public double EndPointX
        {
            get { return (double)GetValue(EndPointXProperty); }
            set { SetValue(EndPointXProperty, value); }
        }

        public static readonly BindableProperty EndPointYProperty = BindableProperty.Create("EndPointY", typeof(double), typeof(GradientContentView), (double)1);
        public double EndPointY
        {
            get { return (double)GetValue(EndPointYProperty); }
            set { SetValue(EndPointYProperty, value); }
        }

        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create("CornerRadius", typeof(int), typeof(GradientContentView), 0);
        public int CornerRadius
        {
            get { return (int)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty RoundCornersProperty = BindableProperty.Create("RoundCorners", typeof(RoundType), typeof(GradientContentView), RoundType.RoundAll);
        public RoundType RoundCorners
        {
            get { return (RoundType)GetValue(RoundCornersProperty); }
            set { SetValue(RoundCornersProperty, value); }
        }
    }

    public enum RoundType
    {
        RoundAll,
        RoundTop,
		RoundBottom,
		RoundLeft,
		RoundRight,
		RoundLeftTop,
		RoundLeftBottom,
		RoundRightTop,
		RoundRightBottom,
    }
}

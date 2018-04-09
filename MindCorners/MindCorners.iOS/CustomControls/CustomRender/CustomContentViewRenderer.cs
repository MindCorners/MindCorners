using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CoreAnimation;
using CoreGraphics;
using MindCorners.CustomControls;
using MindCorners.iOS.CustomControls.CustomRender;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Windows.Media;
using Foundation;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(CustomContentView), typeof(CustomContentViewRenderer))]
namespace MindCorners.iOS.CustomControls.CustomRender
{
    class CustomContentViewRenderer : VisualElementRenderer<ContentView>
    {
        /// <summary>
        /// Gets the underlying element typed as an <see cref="GradientContentView"/>.
        /// </summary>
        private CustomContentView CustomContentView
        {
            get { return (CustomContentView)Element; }
        }

        protected CAGradientLayer GradientLayer { get; set; }

        public override void LayoutSubviews()
        {
            //foreach (var layer in NativeView?.Layer.Sublayers.Where(layer => layer is CAGradientLayer))
            //{
            //    layer.Frame = NativeView.Bounds;

            //    if (layer.CornerRadius == 0)
            //    {
            //        if (GradientContentView.RoundCorners != RoundType.RoundAll)
            //        {
            //            var roundCornersTop = GradientContentView.RoundCorners == RoundType.RoundTop;
            //            layer.Frame = new CGRect(0, roundCornersTop ? GradientContentView.CornerRadius : 0,
            //                this.Bounds.Size.Width, Bounds.Size.Height - GradientContentView.CornerRadius);
            //        }
            //    }
            //}
            
            base.LayoutSubviews();
            
        }


        /// <summary>
        /// Setup the gradient layer
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<ContentView> e)
        {
            base.OnElementChanged(e);

            if (CustomContentView != null &&
                NativeView != null)
            {
                NativeView.Layer.BorderColor= CustomContentView.BorderColor.ToCGColor();
                NativeView.Layer.BorderWidth = CustomContentView.BorderWidth;
                NativeView.Layer.CornerRadius = CustomContentView.CornerRadius;

                //NativeView.BackgroundColor = UIColor.Blue;
            }
        }
    }
}

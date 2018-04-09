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

[assembly: ExportRenderer(typeof(GradientContentView), typeof(GradientContentViewRenderer))]
namespace MindCorners.iOS.CustomControls.CustomRender
{
    class GradientContentViewRenderer : VisualElementRenderer<ContentView>
    {
        /// <summary>
        /// Gets the underlying element typed as an <see cref="GradientContentView"/>.
        /// </summary>
        private GradientContentView GradientContentView
        {
            get { return (GradientContentView)Element; }
        }

        protected CAGradientLayer GradientLayer { get; set; }

        public override void LayoutSubviews()
        {
            foreach (var layer in NativeView?.Layer.Sublayers.Where(layer => layer is CAGradientLayer))
            {
                layer.Frame = NativeView.Bounds;

                if (layer.CornerRadius == 0)
                {
                    if (GradientContentView.RoundCorners != RoundType.RoundAll)
                    {
                        var roundCornersTop = GradientContentView.RoundCorners == RoundType.RoundTop;
                        layer.Frame = new CGRect(0, roundCornersTop ? GradientContentView.CornerRadius : 0,
                            this.Bounds.Size.Width, Bounds.Size.Height - GradientContentView.CornerRadius);
                    }
                }
            }
            
            base.LayoutSubviews();
            
        }


        /// <summary>
        /// Setup the gradient layer
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<ContentView> e)
        {
            base.OnElementChanged(e);

            if (GradientContentView != null &&
                NativeView != null)
            {
                // Create a gradient layer and add it to the 
                // underlying UIView
                GradientLayer = new CAGradientLayer();
               
                GradientLayer.Frame = NativeView.Bounds;
                GradientLayer.Colors = new CGColor[]
                {
                    GradientContentView.StartColor.ToCGColor(),
                    GradientContentView.EndColor.ToCGColor()
                };
                // GradientLayer.Locations  = new NSNumber[] {0,1};
                GradientLayer.StartPoint = new CGPoint(GradientContentView.StartPointX, GradientContentView.StartPointY);
                GradientLayer.EndPoint = new CGPoint(GradientContentView.EndPointX, GradientContentView.EndPointY);
                
                // GradientLayer.CornerRadius = 8;
                //GradientLayer.CornerRadius =new nfloat();
                // GradientLayer.StartPoint = new CGPoint(0.97, 0.07);
                //  GradientLayer.EndPoint = new CGPoint(0.23, 0.75);
                SetOrientation();
               GradientLayer.CornerRadius = GradientContentView.CornerRadius;
                //GradientLayer.ShadowOffset = new CGSize(3, 3);

                //UIBezierPath maskPath = UIBezierPath.FromRoundedRect(this.Bounds, UIRectCorner.TopLeft | UIRectCorner.TopRight, new SizeF(20, 20));

                //CAShapeLayer maskLayer = new CAShapeLayer();
                //maskLayer.Frame = this.Bounds;
                //maskLayer.Path = maskPath.CGPath;
                //maskLayer.InsertSublayer(GradientLayer,0);
                //maskLayer.FillColor = Color.Aqua.ToCGColor();

                //GradientLayer.Mask = maskLayer;
                //maskLayer.
                // maskLayer.FillColor= GradientContentView.StartColor.ToCGColor();

                // Set the newly created shape layer as the mask for the image view's layer
                // this.Layer.Mask = maskLayer;
               


                NativeView.Layer.InsertSublayer(GradientLayer, 0);

                if (GradientContentView.RoundCorners != RoundType.RoundAll)
                {

					if (GradientContentView.RoundCorners == RoundType.RoundTop ||
						GradientContentView.RoundCorners == RoundType.RoundBottom)
					{

						var roundCornersTop = GradientContentView.RoundCorners == RoundType.RoundTop;
						CAGradientLayer gradient2 = new CAGradientLayer();
						gradient2.Frame = new CGRect(0, roundCornersTop ? GradientContentView.CornerRadius : 0,
							this.Bounds.Size.Width, Bounds.Size.Height - GradientContentView.CornerRadius);
						gradient2.Colors = new CGColor[]
						{
							GradientContentView.StartColor.ToCGColor(),
							GradientContentView.EndColor.ToCGColor()
						};
						// GradientLayer.Locations  = new NSNumber[] {0,1};
						gradient2.StartPoint = new CGPoint(GradientContentView.StartPointX,
							GradientContentView.StartPointY);
						gradient2.EndPoint = new CGPoint(GradientContentView.EndPointX, GradientContentView.EndPointY);
						gradient2.CornerRadius = 0.0f;

						NativeView.Layer.InsertSublayer(gradient2, 1);
					}
					else
					{
						CAGradientLayer gradient2 = new CAGradientLayer();
						gradient2.Colors = new CGColor[]
						{
							GradientContentView.StartColor.ToCGColor(),
							GradientContentView.EndColor.ToCGColor()
						};
						// GradientLayer.Locations  = new NSNumber[] {0,1};
						gradient2.StartPoint = new CGPoint(GradientContentView.StartPointX,
							GradientContentView.StartPointY);
						gradient2.EndPoint = new CGPoint(GradientContentView.EndPointX, GradientContentView.EndPointY);
						gradient2.CornerRadius = 0.0f;



						CAGradientLayer gradient3 = new CAGradientLayer();
						gradient3.Colors = new CGColor[]
						{
							GradientContentView.StartColor.ToCGColor(),
							GradientContentView.EndColor.ToCGColor()
						};
						// GradientLayer.Locations  = new NSNumber[] {0,1};
						gradient3.StartPoint = new CGPoint(GradientContentView.StartPointX,
							GradientContentView.StartPointY);
						gradient3.EndPoint = new CGPoint(GradientContentView.EndPointX, GradientContentView.EndPointY);
						gradient3.CornerRadius = 0.0f;



						switch (GradientContentView.RoundCorners)
						{
						case RoundType.RoundLeft:
							gradient2.Frame = new CGRect(GradientContentView.CornerRadius, 0, this.Bounds.Size.Width - GradientContentView.CornerRadius, Bounds.Size.Height);
							gradient3 = null;
							break;
						case RoundType.RoundRight:
							gradient2.Frame = new CGRect(0,0,this.Bounds.Size.Width - GradientContentView.CornerRadius, Bounds.Size.Height);
							gradient3 = null;
							break;
						case RoundType.RoundLeftTop:/*done*/
							gradient2.Frame = new CGRect(0, GradientContentView.CornerRadius, this.Bounds.Size.Width, Bounds.Size.Height -GradientContentView.CornerRadius);
							gradient3.Frame = new CGRect(GradientContentView.CornerRadius, 0,this.Bounds.Size.Width - GradientContentView.CornerRadius, Bounds.Size.Height);
							break;
						case RoundType.RoundLeftBottom:/*done*/
							gradient2.Frame = new CGRect(0, 0, this.Bounds.Size.Width, Bounds.Size.Height - GradientContentView.CornerRadius);
							gradient3.Frame = new CGRect(GradientContentView.CornerRadius, 0,  this.Bounds.Size.Width - GradientContentView.CornerRadius, Bounds.Size.Height);
							break;
						case RoundType.RoundRightTop:
							gradient2.Frame = new CGRect(0, GradientContentView.CornerRadius, this.Bounds.Size.Width, Bounds.Size.Height - GradientContentView.CornerRadius);
							gradient3.Frame = new CGRect(0, 0, this.Bounds.Size.Width - GradientContentView.CornerRadius, Bounds.Size.Height);
							break;
						case RoundType.RoundRightBottom:/*done*/
							gradient2.Frame = new CGRect(0, 0, this.Bounds.Size.Width, Bounds.Size.Height - GradientContentView.CornerRadius);
							gradient3.Frame = new CGRect(0, 0, this.Bounds.Size.Width - GradientContentView.CornerRadius, Bounds.Size.Height);
							break;
						}




						NativeView.Layer.InsertSublayer(gradient2, 1);
						if (gradient3 != null)
						{
							NativeView.Layer.InsertSublayer(gradient3, 2);
						}
					}

                }


                //   NativeView.Layer.InsertSublayerBelow(GradientLayer, gradient2);
                //   NativeView.Layer.InsertSublayerBelow(gradient2, layer);
                //  GradientLayer.Mask = maskLayer;



                /*

                                UIView* layer = [[UIView alloc] initWithFrame: CGRectMake(24, 154, 321, 272)];
                                CAGradientLayer* gradient = [CAGradientLayer layer];
                                gradient.frame = CGRectMake(0, 0, 321, 272); 
                                gradient.colors = @[(id)[[UIColor colorWithRed:0.55 green:0.69 blue:0.92 alpha:1]CGColor],(id)[[UIColor colorWithRed:0.36 green:0.58 blue:0.94 alpha:1]CGColor]];
                                gradient.locations = @[@(0), @(1)];
                                gradient.startPoint = CGPointMake(0.97, 0.07); 
                                gradient.endPoint = CGPointMake(0.23, 0.75);
                                [[layer layer]addSublayer:gradient];
                                [[self view]addSubview:layer];

                    */

               
               
               

                //NativeView.BackgroundColor = UIColor.Blue;
            }
        }

        /// <summary>
        /// Update the underlying controls as needed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (GradientLayer != null && GradientContentView != null)
            {
                // Turn off Animations
                CATransaction.Begin();
                CATransaction.DisableActions = true;

                if (e.PropertyName == GradientContentView.StartColorProperty.PropertyName)
                    GradientLayer.Colors[0] = GradientContentView.StartColor.ToCGColor();

                if (e.PropertyName == GradientContentView.EndColorProperty.PropertyName)
                    GradientLayer.Colors[1] = GradientContentView.EndColor.ToCGColor();

                if (e.PropertyName == VisualElement.WidthProperty.PropertyName ||
                    e.PropertyName == VisualElement.HeightProperty.PropertyName)
                    GradientLayer.Frame = NativeView.Bounds;

                if (e.PropertyName == GradientContentView.OrientationProperty.PropertyName)
                    SetOrientation();

                CATransaction.Commit();
            }
        }
        void SetOrientation()
        {
            if (GradientContentView.Orientation == GradientOrientation.Horizontal)
            {
                GradientLayer.StartPoint = new CGPoint(GradientContentView.StartPointX, GradientContentView.StartPointY);
                GradientLayer.EndPoint = new CGPoint(GradientContentView.EndPointX, GradientContentView.EndPointY);
            }
            else
            {
                GradientLayer.StartPoint = new CGPoint(GradientContentView.StartPointX, GradientContentView.StartPointY);
                GradientLayer.EndPoint = new CGPoint(GradientContentView.EndPointX, GradientContentView.EndPointY);
            }

            //if (GradientContentView.Orientation == GradientOrientation.Horizontal)
            //{
            //    GradientLayer.StartPoint = new CGPoint(0, 0.5);
            //    GradientLayer.EndPoint = new CGPoint(1, 0.5);
            //}
            //else
            //{
            //    GradientLayer.StartPoint = new CGPoint(0.5, 0);
            //    GradientLayer.EndPoint = new CGPoint(0.5, 1);
            //}
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindCorners.iOS.CustomControl.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Button), typeof(CustomButtonRenderer))]
namespace MindCorners.iOS.CustomControl.CustomRenderer
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {   
            base.OnElementChanged(e);

            if (Control != null)
            {
				this.Control.ContentEdgeInsets = new UIEdgeInsets(0,10,0,10);
				//Control.WidthAnchor.ConstraintEqualTo(Element.WidthA Width * 0.4;
    			
                //DrawBorder();
            }
        }

        void DrawBorder()
        {
            //FrameLayout borderLayer = new FrameLayout();
            //borderLayer.Background.BackgroundColor = Color.FromRGB(23, 162, 227).CGColor;
            //borderLayer.Frame = new CGRect(0, 45, 335, 5);
            //View.AddSublayer(borderLayer);
        }
    }
}
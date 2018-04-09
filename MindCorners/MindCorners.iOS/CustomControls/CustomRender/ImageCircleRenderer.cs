using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using MindCorners.CustomControls;
using MindCorners.iOS.CustomControls.CustomRender;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MindCorners.CustomControls.ImageCircle), typeof(ImageCircleRenderer))]
namespace MindCorners.iOS.CustomControls.CustomRender
{

    public class ImageCircleRenderer : ImageRenderer
    { 
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
                return;

            CreateCircle();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == VisualElement.HeightProperty.PropertyName ||
                e.PropertyName == VisualElement.WidthProperty.PropertyName)
            {
                
                CreateCircle();

            }
        }
        private void CreateCircle()
        {
            try
            {
                var width = Element.WidthRequest;
                var height = Element.HeightRequest;

                double min = Math.Min(width, height);
                Control.Layer.CornerRadius = (float)min/2;
                Control.Layer.MasksToBounds = true;
                Control.Layer.BorderColor = ((MindCorners.CustomControls.ImageCircle)Element).BorderColor.ToCGColor();
                Control.Layer.BorderWidth = ((MindCorners.CustomControls.ImageCircle)Element).BorderWidth;
                Control.ClipsToBounds = true;

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to create circle image: " + ex);
            }
        }
    }
}

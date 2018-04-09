using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using MindCorners.iOS.CustomControls.CustomRender;
using CoreGraphics;

//[assembly: ExportRenderer(typeof(ActivityIndicator), typeof(CustomActivityIndicatorRenderer))]
namespace MindCorners.iOS.CustomControls.CustomRender
{
	public class CustomActivityIndicatorRenderer : ViewRenderer<ActivityIndicator, UIActivityIndicatorView>
	{
		/*
		CGSize _fitSize;
		protected override void OnElementChanged(ElementChangedEventArgs<ActivityIndicator> e)
		{
			base.OnElementChanged(e);
			if (Control == null)
			{				
				SetNativeControl(new UIActivityIndicatorView());

			}

			if (Control != null) {
				Control.SizeToFit();
				_fitSize = Control.Bounds.Size;
				Control.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.WhiteLarge;	
			}
		}
		public override CGSize SizeThatFits(CGSize size)
		{
			return _fitSize;
		}*/

	}
}
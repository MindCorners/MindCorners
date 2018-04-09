using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Foundation;
using MindCorners.CustomControls;
using MindCorners.iOS.CustomControl.CustomRenderer;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(InfiniteListView), typeof(InfiniteListViewRenderer))]
namespace MindCorners.iOS.CustomControl.CustomRenderer
{
	public class InfiniteListViewRenderer : ListViewRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
		{
			base.OnElementChanged(e);
			if (Control != null && e.NewElement != null)				
			{
				//Control.ScrollEnabled = ((InfiniteListView)e.NewElement).CanLoadMore;
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == InfiniteListView.CanLoadMoreProperty.PropertyName)
			{
				Control.ScrollEnabled = ((InfiniteListView)Element).CanLoadMore;
			}
			base.OnElementPropertyChanged(sender, e);

		}


		public override void TouchesBegan(NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);
		}

		public override void TouchesEnded(NSSet touches, UIEvent evt)
		{
			base.TouchesEnded(touches, evt);
		}

		public override void TouchesMoved(NSSet touches, UIEvent evt)
		{
			base.TouchesMoved(touches, evt);
		}
	}
}
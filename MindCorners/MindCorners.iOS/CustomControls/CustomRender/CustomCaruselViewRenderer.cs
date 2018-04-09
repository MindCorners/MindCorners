using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MindCorners.iOS.CustomControls.CustomRender;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform;
using Xamarin.Forms.Platform.iOS;
using CarouselView = Xamarin.Forms.CarouselView;
using CoreGraphics;


[assembly: ExportRenderer(typeof(Xamarin.Forms.CarouselView), typeof(CustomCarouselViewRenderer))]
namespace MindCorners.iOS.CustomControls.CustomRender
{
	public class CustomCarouselViewRenderer : CarouselViewRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.CarouselView> e)
		{
			base.OnElementChanged(e);
			var items = Control.VisibleCells;


			//Control.ContentSize = new CGSize(this.Frame.Width-100, 200);

			/*var inset = Control.ContentInset;
			inset.Left = 50;
			inset.Right = 50;
			inset.InsetRect(new CGRect(0, 0, 100, 100));
			Control.ContentInset = inset;
			//Control.PagingEnabled = true;
			Control.SetCollectionViewLayout(new UICollectionViewFlowLayout() {
				//EstimatedItemSize = new CGSize(200,120),
				ScrollDirection = UICollectionViewScrollDirection.Horizontal,
				MinimumInteritemSpacing = 10,
				MinimumLineSpacing = 10,
				//SectionInset = new UIEdgeInsets(50,50,50,50),
				ItemSize = new CGSize(100,100)
			},true);//Control.InputView.Bounds.Width = 300;*/
			//Control.Delegate = new CustomViewDelegate ();
			Control.ShowsVerticalScrollIndicator = false;
			Control.ShowsHorizontalScrollIndicator = false;
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			var propertyName = e.PropertyName;
			base.OnElementPropertyChanged (sender, e);
		}
	}

	public class CustomViewDelegate: UICollectionViewDelegateFlowLayout{
		public override CGSize GetSizeForItem (UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
		{
			return new CGSize (collectionView.Frame.Width-40, collectionView.Frame.Height);
		}
	}
}
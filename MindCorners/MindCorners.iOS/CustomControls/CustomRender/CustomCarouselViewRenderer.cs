using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using MindCorners.iOS.CustomControls.CustomRender;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform;
using Xamarin.Forms.Platform.iOS;
using XLabs.Forms.Controls;
using CarouselView = Xamarin.Forms.CarouselView;


[assembly: ExportRenderer(typeof(Xamarin.Forms.CarouselView), typeof(CustomCarouselViewRenderer))]
namespace MindCorners.iOS.CustomControls.CustomRender
{
    public class CustomCarouselViewRenderer : CarouselViewRenderer
    {
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {

            nfloat mainWidth = collectionView.Frame.Width;
            nfloat cellWidth = mainWidth / 7;

            nfloat mainHeight = (collectionView.Frame.Height * (nfloat)0.75);
            nfloat cellHeight = mainHeight / 5;

            return new CGSize(cellWidth, cellHeight);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.CarouselView> e)
        {
            base.OnElementChanged(e);

            //Control.DecelerationRate = UIScrollViewDe
            var inset = Control.ContentInset;
            
         
            inset.Left = 50;
            inset.Right = 50;
            inset.InsetRect(new CGRect(0, 0, 100, 100));
            Control.ContentInset = inset;
            var layout = new UICollectionViewFlowLayout()
            {

                //EstimatedItemSize = new CGSize(100,100),
                ScrollDirection = UICollectionViewScrollDirection.Horizontal,
                // MinimumInteritemSpacing = 0,
                // MinimumLineSpacing = 0,
                // SectionInset = new UIEdgeInsets(50,50,50,50),
                ItemSize = new CGSize(100, 100),
            };
            
            Control.SetCollectionViewLayout(layout, true);
            //.ShowsVerticalScrollIndicator = false;
            Control.ShowsHorizontalScrollIndicator = false;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var propertyName = e.PropertyName;
         
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return base.GetDesiredSize(widthConstraint, heightConstraint);
        }
    }
}
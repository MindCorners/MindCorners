using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MindCorners.iOS.CustomControls.CustomRender.WhateverYourNamespace.iOS;
using MindCorners.Pages.UserControls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//[assembly: ExportRenderer(typeof(CustomAlertDialog), typeof(TransparentPopupRenderer))]
namespace MindCorners.iOS.CustomControls.CustomRender
{

    namespace WhateverYourNamespace.iOS
    {
        public class TransparentPopupRenderer : PageRenderer
        {
            protected override void OnElementChanged(VisualElementChangedEventArgs e)
            {
                base.OnElementChanged(e);
            }

            public override void DidMoveToParentViewController(UIViewController parent)
            {
                base.DidMoveToParentViewController(parent);

                if (ParentViewController != null)
                {
                    // Preparing the view to get the presentation of the parent view for the background
                    ParentViewController.ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
                }
            }

            public override void ViewDidLoad()
            {
                base.ViewDidLoad();

                // Setting the background color to transparent when the view is appearing
                View.BackgroundColor = UIColor.Clear;
            }
        }
    }
}

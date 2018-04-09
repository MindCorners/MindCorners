using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using MindCorners.CustomControls;
using MindCorners.iOS.CustomControl.CustomRenderer;

[assembly: ExportRenderer(typeof(CardView), typeof(CardViewRenderer))]
namespace MindCorners.iOS.CustomControl.CustomRenderer
{

    public class CardViewRenderer
    {
    }
    /*
    :

ViewRenderer<CardView, Android.Support.V7.Widget.CardView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CardView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                var cardView = new Android.Support.V7.Widget.CardView(Context);
                cardView.Radius = 10;

                SetNativeControl(cardView);
            }
           
            ;
        }
    }*/
}
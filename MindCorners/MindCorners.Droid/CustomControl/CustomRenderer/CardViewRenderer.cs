using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MindCorners.Droid.CustomControl.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using MindCorners.CustomControls;

[assembly: ExportRenderer(typeof(CardView), typeof(CardViewRenderer))]
namespace MindCorners.Droid.CustomControl.CustomRenderer
{
   
    public class CardViewRenderer : ViewRenderer<CardView, Android.Support.V7.Widget.CardView>
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MindCorners.CustomControls;
using MindCorners.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomSearchBar),
                          typeof(CustomSearchBarRenderer))]
namespace MindCorners.iOS
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(
        ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            var element = Element as CustomSearchBar;

            if (element != null)
            {  
                Control.CancelButtonClicked +=
                        (sender, args) => element.CancelButtonCommand.Execute(null);
                Control.ShowsCancelButton = true;
                Control.BarTintColor = Color.FromHex("#FFF").ToUIColor();
                //Control.TintColor = Color.Red.ToUIColor();
                element.CancelButtonColor = Color.FromHex("#FFF");
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "Text")
            {
                Control.ShowsCancelButton = true;
            }
        }
    }
}
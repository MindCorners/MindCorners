using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using MindCorners.CustomControls;
using MindCorners.Droid.CustomControl.CustomRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HtmlFormattedLabel), typeof(HtmlFormattedLabelRenderer))]
namespace MindCorners.Droid.CustomControl.CustomRenderer
{
    public class HtmlFormattedLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var view = (HtmlFormattedLabel)Element;
            if (view == null) return;
            
            Control.TextChanged += Control_TextChanged;            
        }

        private void Control_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {  
            var view = (HtmlFormattedLabel)Element;
            if (view == null) return;
            Control.TextChanged -= Control_TextChanged;
            // var htmlText = !string.IsNullOrEmpty(view.Text) ? string.Format("{0}{1}", Html.FromHtml(view.Text), Html.FromHtml(view.Text).ToString().Length > 30 ? "...":"") : null;
            // var htmlText = !string.IsNullOrEmpty(view.Text) ? Html.FromHtml(view.Text) : null;
            // Control.SetText(htmlText, TextView.BufferType.Spannable);
            Element.FormattedText = new FormattedString() {Spans = { new Span() {} }};


        }

        //public FormattedString FormattedDescription
        //{
        //    get
        //    {
        //        return new FormattedString
        //        {
        //            Spans = {
        //                    new Span { Text = RoleName, FontSize = 16, FontAttributes = FontAttributes.Bold },
        //                    new Span { Text = Environment.NewLine, FontSize = 16 },
        //                    new Span { Text = "/ " + ProjectRoleID + "/ "+Part + "/ "+Gender + "/ " + AgeRange},
        //                }
        //        };
        //    }
        //    set { }
        //}
    }
}
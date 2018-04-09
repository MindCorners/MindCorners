using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MindCorners.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace MindCorners.Droid.CustomControl.CustomRenderer
{
    public class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(
        ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var element = e.NewElement as MindCorners.CustomControls.CustomEditor;
                this.Control.Hint = element.Placeholder;
            }
        }

        protected override void OnElementPropertyChanged(
            object sender,
            PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == CustomEditor.PlaceholderProperty.PropertyName)
            {
                var element = this.Element as CustomEditor;
                this.Control.Hint = element.Placeholder;
            }
        }
    }
}
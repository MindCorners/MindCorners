
using System;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using MindCorners.CustomControls;

using Cirrious.FluentLayouts.Touch;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(MindCorners.iOS.CustomControls.CustomRender.PlaceholderEditorRenderer))]
namespace MindCorners.iOS.CustomControls.CustomRender
{
    
	public class PlaceholderEditorRenderer : EditorRenderer
	{
		private UILabel _placeholderLabel;

		protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged(e);

			if (Element == null)
				return;

			CreatePlaceholderLabel((CustomEditor) Element, Control);

			Control.Ended += OnEnded;
			Control.Changed += OnChanged;
			((CustomEditor) Element).TextChangeAction = delegate {
				if (_placeholderLabel != null)
				_placeholderLabel.Hidden = ((UITextView) Control).HasText;
			};
		}

		private void CreatePlaceholderLabel(CustomEditor element, UITextView parent)
		{
			_placeholderLabel = new UILabel
			{
				Text = element.Placeholder,
				TextColor = element.PlaceholderColor.ToUIColor(),
				BackgroundColor = UIColor.Clear,
				Font = UIFont.FromName("Lato-Regular", (nfloat)element.FontSize)
			};
			_placeholderLabel.SizeToFit();

			parent.AddSubview(_placeholderLabel);

			parent.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
			parent.AddConstraints(
				_placeholderLabel.AtLeftOf(parent, 7),

				_placeholderLabel.AtTopOf(parent, 7)//,
				//_placeholderLabel.WithSameTop(parent)
			);
			parent.LayoutIfNeeded();

			_placeholderLabel.Hidden = parent.HasText;
		}

		private void OnEnded(object sender, EventArgs args)
		{
			if (!((UITextView) sender).HasText && _placeholderLabel != null)
				_placeholderLabel.Hidden = false;
		}

		private void OnChanged(object sender, EventArgs args)
		{
			if (_placeholderLabel != null)
				_placeholderLabel.Hidden = ((UITextView) sender).HasText;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Control.Ended -= OnEnded;
				Control.Changed -= OnChanged;

				_placeholderLabel?.Dispose();
				_placeholderLabel = null;
			}

			base.Dispose(disposing);
		}
	}
	/*public class CustomEditorRenderer : EditorRenderer
    {
        UILabel labelPlaceHolder;
        UITextView replacingControl;
        private string Placeholder { get; set; }
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            //replacingControl = new UITextView(Control.Bounds);
            //var adelegate = new myTextViewDelegate();
            //var element = this.Element as CustomEditor;

            //adelegate.Placeholder = element.Placeholder;

            //replacingControl.Delegate = adelegate;
            //replacingControl.TextColor = UIColor.LightGray;
            //replacingControl.Text = adelegate.Placeholder;

            //this.SetNativeControl(replacingControl);

            base.OnElementChanged(e);
            var element = this.Element as CustomEditor;

            if (Control != null && element != null)
            {
                Placeholder = element.Placeholder;
                Control.TextColor = element.PlaceholderColor.ToUIColor();
                Control.Text = Placeholder;
                Control.Changed += Control_Changed;

                Control.ShouldBeginEditing += (UITextView textView) => {
                    if (textView.Text == Placeholder)
                    {
                        textView.Text = "";
                        textView.TextColor = element.TextColor.ToUIColor(); //UIColor.Black; // Text Color
                    }

                    return true;
                };

                Control.ShouldEndEditing += (UITextView textView) => {
                    if (textView.Text == "")
                    {
                        textView.Text = Placeholder;
                        textView.TextColor = element.PlaceholderColor.ToUIColor(); // Placeholder Color
                    }
                    else { }

                    return true;
                };
            }

        }

        private void Control_Changed(object sender, EventArgs e)
        {
            
        }
    }*/
}
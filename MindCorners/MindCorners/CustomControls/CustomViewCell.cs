using System;
using Xamarin.Forms;

namespace MindCorners.CustomControls
{
	public class CustomViewCell : ViewCell
	{
		public static readonly BindableProperty DestructiveColorProperty =
			BindableProperty.Create("DestructiveColor", typeof(Color), typeof(ImageCircle), Color.Transparent, BindingMode.OneWay);

		public Color DestructiveColor
		{
			get { return (Color)GetValue(DestructiveColorProperty); }
			set
			{
				if (value != DestructiveColor)
				{
					SetValue(DestructiveColorProperty, value);
				}
			}
		}
		
	}
}


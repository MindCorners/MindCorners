using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MindCorners.CustomControls
{
    public class NotificationIcon : AbsoluteLayout
    {
        public static readonly BindableProperty IconNameProperty = BindableProperty.Create("IconName", typeof(string), typeof(NotificationIcon), "magnifyingGlass.png", BindingMode.TwoWay);
        public string IconName
        {

            get { return (string)GetValue(IconNameProperty); }

            set { SetValue(IconNameProperty, value); }

        }
        public static readonly BindableProperty LabelValueProperty = BindableProperty.Create("LabelValue", typeof(int), typeof(NotificationIcon), 0);
        public int LabelValue
        {

            get { return (int)GetValue(LabelValueProperty); }

            set { SetValue(LabelValueProperty, value); }

        }

        public static readonly BindableProperty BoxColorProperty = BindableProperty.Create("BoxColor", typeof(Color), typeof(NotificationIcon), Color.Default);

        public Color BoxColor
        {
            get { return (Color)GetValue(BoxColorProperty); }
            set { SetValue(BoxColorProperty, value); }
        }

        private Image image;
        private CustomBadge customBadge;
        public NotificationIcon()
        {
            image = new Image()
            {
                Source = IconName,
                
            };

            image.SetBinding(Image.SourceProperty, (NotificationIcon ec) => ec.IconName, BindingMode.OneWay);
            AbsoluteLayout.SetLayoutBounds(image, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(image, AbsoluteLayoutFlags.All);
            customBadge = new CustomBadge(20, 14);

            customBadge.SetBinding(CustomBadge.BoxColorProperty, (NotificationIcon ec) => ec.BoxColor, BindingMode.OneWay);
            customBadge.SetBinding(CustomBadge.TextProperty, (NotificationIcon ec) => ec.LabelValue, BindingMode.OneWay);
            //   customBadge.BoxColor = BoxColor;
            //   customBadge.Text = LabelValue.ToString();


            AbsoluteLayout.SetLayoutBounds(customBadge, new Rectangle(2, 1, -1, -1));
            AbsoluteLayout.SetLayoutFlags(customBadge, AbsoluteLayoutFlags.PositionProportional);


            this.Children.Add(image);
            this.Children.Add(customBadge);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "IconName")
            {
                image.Source = IconName;
            }
            if (propertyName == "BoxColor")
            {
                customBadge.BoxColor = BoxColor;
            }
            if (propertyName == "LabelValue")
            {
                customBadge.Text = LabelValue.ToString();
            }
        }
    }
}

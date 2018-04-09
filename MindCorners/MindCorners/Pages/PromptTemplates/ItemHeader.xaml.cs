using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MindCorners.Pages.PromptTemplates
{
    public partial class ItemHeader : Grid
    {
        public static readonly BindableProperty ProfileImageSizeProperty =
           BindableProperty.Create("ProfileImageSize", typeof(int), typeof(ItemHeader), 40);

        /// <summary>
        /// The text.
        /// </summary>
        public int ProfileImageSize
        {
            get { return (int)GetValue(ProfileImageSizeProperty); }
            set { SetValue(ProfileImageSizeProperty, value); }
        }


        public ItemHeader()
        {
            InitializeComponent();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (ProfileImageSizeProperty.PropertyName == propertyName)
            {
                ProfileImage.HeightRequest = ProfileImage.WidthRequest = ProfileImageSize;
            }
        }
    }
}

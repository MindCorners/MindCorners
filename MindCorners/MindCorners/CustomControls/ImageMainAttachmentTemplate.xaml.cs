using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Models;
using MindCorners.Pages;
using Xamarin.Forms;

namespace MindCorners.CustomControls.ChatMainAttachment
{
    public partial class ImageMainAttachmentTemplate : ViewCell
    {
        public ImageMainAttachmentTemplate()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new ImagePreview(((Post)BindingContext).MainAttachment));
        }
    }
}

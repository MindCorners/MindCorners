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
    public partial class VideoMainAttachmentTemplateGrid : Grid
    {
        public VideoMainAttachmentTemplateGrid()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new VideoPreview(((Post)BindingContext).MainAttachment));
        }
    }
}

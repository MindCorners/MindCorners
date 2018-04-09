using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Models;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.Pages.PromptTemplates
{
    public partial class VideoChatItem : ContentPage
    {
        public VideoChatItem(VideoChatItemViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var viewModel = (VideoChatItemViewModel) BindingContext;
            await App.Current.MainPage.Navigation.PushModalAsync(new VideoPreview(new PostAttachment() {FileUrl = viewModel.VideoFileLocation}));
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            player.StopPlayingAction();
            await App.NavigationPage.PopAsync();
        }
    }
}

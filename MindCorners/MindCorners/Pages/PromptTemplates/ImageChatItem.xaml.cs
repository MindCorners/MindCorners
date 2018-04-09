using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.Pages.PromptTemplates
{
    public partial class ImageChatItem : ContentPage
    {
        public ImageChatItem(ImageChatItemViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await App.NavigationPage.PopAsync();
        }

    }
}

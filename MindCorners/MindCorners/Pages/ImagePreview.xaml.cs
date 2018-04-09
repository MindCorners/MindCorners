using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.CustomControls;
using MindCorners.Models;
using MindCorners.Models.Enums;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.Pages
{
    public partial class ImagePreview : ContentPage
    {
        public ImagePreview(PostAttachment vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            // await App.Current.MainPage.Navigation.PopModalAsync();
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}

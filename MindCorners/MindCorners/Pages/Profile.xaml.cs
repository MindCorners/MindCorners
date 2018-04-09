using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Code;
using MindCorners.CustomControls;
using MindCorners.Helpers;
using MindCorners.Models;
using MindCorners.Models.Enums;
using MindCorners.ViewModels;
using Xamarin.Forms;
using Rg.Plugins.Popup.Extensions;
using Plugin.Media;
using MindCorners.Pages.UserControls;

namespace MindCorners.Pages
{
    public partial class Profile : ContentPage
    {
		public Profile(ProfileViewModel vm)
        {			
            InitializeComponent();
			BindingContext = vm;
        }
        private void OnLogOutClick(object sender, EventArgs e)
        {
            Settings.CurrnetUser = null;
			Application.Current.MainPage=App.NavigationPage = new CustomNavigationPage(new Login(new LoginPageViewModel())) { BarTextColor = Color.White };
        }

        private async void ImgProfileImage_OnClicked(object sender, EventArgs e)
        {

			if ((!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported))
			{
				await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "No Camera available.", "Ok"));
				return;
			}

			var page = new CustomCamera((ProfileViewModel)this.BindingContext)
            {
                CameraRotateImage = "rotateCamera.png",
                PictureGallaryImage = "cameraGallary.png",
                CancelImage = "backArrow.png",
                TakePhotoImage = "takePicture.png",
                FlashLightOnImage = "flashOn.png",
                FlashLightOffImage = "flashOn.png"
            };

            await App.NavigationPage.PushAsync(page);

            /* var user = Settings.CurrnetUser;
             if (user != null)
             {
                 App.NavigationPage.PushAsync(new ProfilePhoto() { BindingContext = user });

                 //DisplayAlert("Warning", string.Format("Welcome {0} {1}", user.FirstName, user.LastName), "OK");
             }
             */


            //App.NavigationPage.PushAsync(new ProfilePhoto() { BindingContext = user });
        }
    }
}

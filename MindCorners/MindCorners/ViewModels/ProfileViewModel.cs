using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.Code;
using MindCorners.CustomControls;
using MindCorners.Models;
using MindCorners.Models.Enums;
using MindCorners.Pages;
using MindCorners.Pages.PromptTemplates;
using MindCorners.Pages.UserControls;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using MindCorners.Helpers;

namespace MindCorners.ViewModels
{
    public class ProfileViewModel : BaseViewModel, ICamera
    {
        public ProfileViewModel()
        {
        }

		private User currentUserLocal;
		public User CurrentUserLocal
		{
			get { return Settings.CurrnetUser; }
			set
			{
				Settings.CurrnetUser = value;
				OnPropertyChanged();
			}
		}


        private bool canSavePhoto;
        public bool CanSavePhoto
        {
            get { return canSavePhoto; }
            set
            {
                canSavePhoto = value;
                OnPropertyChanged();
            }
        }

        private ImageSource imageItemSource;
        public ImageSource ImageItemSource
        {
            get { return imageItemSource; }
            set
            {
                imageItemSource = value;
                OnPropertyChanged();
            }
        }
        

        public async void PhotoSelected(byte[] photo)
        {
			IsBusy = true;
            //var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            //{
            //    Directory = "MindCornersFiles",
            //    SaveToAlbum = true,
            //    Name = string.Format("Image_{0}.jpg", DateTime.Now.Ticks.ToString())
            //});

           // if (file == null)
           //     return;
            var stream = new MemoryStream(photo);
            ImageItemSource = ImageSource.FromStream(() => stream);
            CanSavePhoto = true;

            var page = new ProfilePhoto(this);
            page.UpdateImageSource(photo);
            await App.NavigationPage.PushAsync(page);
			IsBusy = false;

        }
    }
}

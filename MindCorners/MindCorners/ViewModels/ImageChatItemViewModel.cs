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
using MindCorners.Pages.PromptTemplates;
using MindCorners.Pages.UserControls;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace MindCorners.ViewModels
{
    public class ImageChatItemViewModel : ChatItemAttachmentViewModel, ICamera
    {
        public ICommand TakePhotoCommand { get; set; }
        public ICommand SelectPhotoCommand { get; set; }
        public ImageChatItemViewModel()
        {
            TakePhotoCommand = new Command(TakePhoto);
            SelectPhotoCommand = new Command(SelectPhoto);
            IsFile = true;
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

        private async void TakePhoto()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "No Camera available.", "Ok"));
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            {
                Directory = "MindCornersFiles",
                SaveToAlbum = true,
                Name = string.Format("Image_{0}.jpg", DateTime.Now.Ticks.ToString())
            });

            if (file == null)
                return;

            ImageItemSource = ImageSource.FromStream(() => file.GetStream());
            FileItemSourceArray = ReadStream(file.GetStream());
            FileName = GetFileNameFromFilePath(file.AlbumPath);
            // LocalSettings.ProfilePictureStream = ReadStream(file.GetStream());
            CanSavePhoto = true;
        }


        private async void SelectPhoto()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "Picking a photo is not supported.", "Ok"));
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();
            if (file == null)
                return;

            ImageItemSource = ImageSource.FromStream(() => file.GetStream());
            FileItemSourceArray = ReadStream(file.GetStream());
            FileName = GetFileNameFromFilePath(file.Path);
            CanSavePhoto = true;
        }


        public async void PhotoSelected(byte[] photo)
        {
            
            //var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            //{
            //    Directory = "MindCornersFiles",
            //    SaveToAlbum = true,
            //    Name = string.Format("Image_{0}.jpg", DateTime.Now.Ticks.ToString())
            //});

           // if (file == null)
           //     return;
           
            ImageItemSource = ImageSource.FromStream(() => new MemoryStream(photo));
            FileItemSourceArray = photo;
            FileName = string.Format("Image_{0}.jpg", DateTime.Now.Ticks.ToString());
            // LocalSettings.ProfilePictureStream = ReadStream(file.GetStream());
            CanSavePhoto = true;

            await App.NavigationPage.PushAsync( new ImageChatItem(this));

        }
        public byte[] ReadStream(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}

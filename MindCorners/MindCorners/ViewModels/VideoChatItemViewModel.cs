using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.CustomControls;
using MindCorners.Pages.PromptTemplates;
using MindCorners.Pages.UserControls;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace MindCorners.ViewModels
{
    public class 
        VideoChatItemViewModel : ChatItemAttachmentViewModel
    {

        public ICommand TakeVideoCommand { get; set; }
        public ICommand SelectVideoCommand { get; set; }
        public VideoChatItemViewModel()
        {
            TakeVideoCommand = new Command(TakeVideo);
            SelectVideoCommand = new Command(SelectVideo);
            IsFile = true;
        }

        private string videoFileLocation;
        public string VideoFileLocation
        {
            get { return videoFileLocation; }
            set
            {
                videoFileLocation = value;
                OnPropertyChanged();
            }
        }

        private bool canSaveVideo;
        public bool CanSaveVideo
        {
            get { return canSaveVideo; }
            set
            {
                canSaveVideo = value;
                OnPropertyChanged();
            }
        }

        //private ImageSource imageItemSource;
        //public ImageSource ImageItemSource
        //{
        //    get { return imageItemSource; }
        //    set
        //    {
        //        imageItemSource = value;
        //        OnPropertyChanged();
        //    }
        //}

        private async void TakeVideo()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
            {
                await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "No Camera available.", "Ok"));
                return;
            }

            var file = await CrossMedia.Current.TakeVideoAsync(new StoreVideoOptions()
            {   
                Directory = "MindCornersFiles",
                SaveToAlbum = true,
                Name = string.Format("Video_{0}.mp4", DateTime.Now.Ticks.ToString())
            });

            if (file == null)
                return;
           
            //ImageItemSource = ImageSource.FromStream(() => file.GetStream());
            FileItemSourceArray = ReadStream(file.GetStream());
            FileName = GetFileNameFromFilePath(file.AlbumPath);
            // LocalSettings.ProfilePictureStream = ReadStream(file.GetStream());
            CanSaveVideo = true;
            var thumbnailService = DependencyService.Get<IGetVideoThumbnail>();
            FileThumbnailItemSourceArray= thumbnailService.GetVideoThumbnail(file.Path);

            VideoFileLocation = file.Path;
        }


        public async void VideoSelected(string filePath, byte[] data)
        {

           
			FileItemSourceArray = data;//ReadStream(file.GetStream());
            FileName = GetFileNameFromFilePath(filePath);
            // LocalSettings.ProfilePictureStream = ReadStream(file.GetStream());
            CanSaveVideo = true;
            var thumbnailService = DependencyService.Get<IGetVideoThumbnail>();
            FileThumbnailItemSourceArray = thumbnailService.GetVideoThumbnail(filePath);

            VideoFileLocation = filePath;

            //var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            //{
            //    Directory = "MindCornersFiles",
            //    SaveToAlbum = true,
            //    Name = string.Format("Image_{0}.jpg", DateTime.Now.Ticks.ToString())
            //});

            // if (file == null)
            //     return;

           // ImageItemSource = ImageSource.FromStream(() => new MemoryStream(photo));
           // FileItemSourceArray = photo;
           // FileName = string.Format("Image_{0}.jpg", DateTime.Now.Ticks.ToString());
            // LocalSettings.ProfilePictureStream = ReadStream(file.GetStream());
            CanSaveVideo = true;
            await App.NavigationPage.PushAsync(new VideoChatItem(this));

        }

        private async void SelectVideo()
        {
            if (!CrossMedia.Current.IsPickVideoSupported)
            {
                await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "Picking a video is not supported.", "Ok"));
                return;
            }

            var file = await CrossMedia.Current.PickVideoAsync();
            if (file == null)
                return;
            //ImageItemSource = ImageSource.FromStream(() => file.GetStream());
            
           FileItemSourceArray = ReadStream(file.GetStream());
            FileName = GetFileNameFromFilePath(file.Path);
            CanSaveVideo = true;

            var thumbnailService = DependencyService.Get<IGetVideoThumbnail>();
            FileThumbnailItemSourceArray = thumbnailService.GetVideoThumbnail(file.Path);
            VideoFileLocation = file.Path;
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

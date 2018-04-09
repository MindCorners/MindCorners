using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Code;
using MindCorners.DAL;
using MindCorners.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using System.IO;
using MindCorners.Helpers;
using MindCorners.Pages.UserControls;
using Rg.Plugins.Popup.Extensions;
using MindCorners.ViewModels;

namespace MindCorners.Pages
{
    public partial class ProfilePhoto : ContentPage
    {
        public ProfilePhoto(ProfileViewModel vm)
        {
            InitializeComponent();
			BindingContext = vm;
           
        }

        private async void BtnTakePhoto_OnClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "No Camera available.", "Ok"));
                return;
            }
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
                    {
                        //Directory = "MindCornersFiles",
                        SaveToAlbum = true,
                        Name = "ProfilePhotoForMindCorners.jpg"
                    });
            if (file == null)
                return;

            ImgProfileImage.Source = ImageSource.FromStream(() => file.GetStream());
            Settings.ProfilePictureStream = ReadStream(file.GetStream());
            BtnSavePhoto.IsVisible = true;

            //LocalSettings.ProfilePictureFilePath = file.Path;
        }

        private async void BtnSelectPhoto_OnClicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "Picking a photo is not supported.", "Ok"));
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();
            if (file == null)
                return;

            ImgProfileImage.Source = ImageSource.FromStream(() => file.GetStream());
            Settings.ProfilePictureStream = ReadStream(file.GetStream());
            BtnSavePhoto.IsVisible = true;
            Settings.ProfilePictureFilePath = file.Path;
        }

        public void UpdateImageSource(byte[] file)
        {
            ImgProfileImage.Source = ImageSource.FromStream(() => new MemoryStream(file));
            Settings.ProfilePictureStream = file;
        }

        private async void BtnSavePhoto_OnClicked(object sender, EventArgs e)
        {
			var viewModel = (ProfileViewModel)BindingContext;
			viewModel.IsBusy = true;
            // throw new NotImplementedException();
            //BtnSavePhoto.IsVisible = false;

            var user = Settings.CurrnetUser;
            if (user != null)
            {
                AccountRepository service = new AccountRepository();
                if (Settings.ProfilePictureStream != null)
                {
                    var file = Settings.ProfilePictureStream;
                    var fileString = Convert.ToBase64String(file);
                    var result = await service.SaveProfilePhoto(new UserProfileImage() {Id = Settings.CurrentUserId, Image = file });
                  
                    if (result != null)
                    {
                        if (result.IsOk)
                        {
                            BtnSavePhoto.IsVisible = false;
                            Settings.CurrentUserProfileImageString = result.FileUrl;
                            Settings.CurrnetUser.ProfileImageString = result.FileUrl;
							user.ProfileImageString = result.FileUrl;
                            var pages = Navigation.NavigationStack;
                            var cameraPage = pages.FirstOrDefault(p => p.ClassId == "MindCorners.CustomControls.CustomCamera");
                            if (cameraPage != null)
                            {
								App.NavigationPage.Navigation.RemovePage(cameraPage);
                            }

                            //var profilePage = pages.FirstOrDefault(p => p.ClassId == "MindCorners.Pages.Profile");
                            //if (profilePage != null)
                            {
                                //var vm = (ProfileViewModel)profilePage.BindingContext;
								viewModel.CurrentUserLocal = user;
                            }

							var mainPage = pages.FirstOrDefault(p => p.ClassId == "MindCorners.Pages.MainPage");
							if (mainPage != null)
							{
								var vm = (MainViewModel)mainPage.BindingContext;
								var posts = vm.LatestPosts;
								foreach (var post in posts) {
									if (post.CreatorId == Settings.CurrentUserId) {
										post.UserProfileImageName = result.FileUrl;
									}
								}
								foreach (var post in Global.LatestPosts) {
									if (post.CreatorId == Settings.CurrentUserId) {
										post.UserProfileImageName = result.FileUrl;
									}
								}
							}

                            await App.NavigationPage.PopAsync();
                            //Page currentPage = Navigation.NavigationStack.LastOrDefault();
                            // currentPage.BindingContext = LocalSettings.CurrnetUser;
                        }
                        else
                        {
                            await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", result.ErrorMessage, "OK"));
                        }
                    }
                    else
                    {
                        await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "Error while uploading photo", "OK"));
                    }
                }

                //string filename = Path.Combine(Settings.ProfilePictureFilePath);

                //using (var streamWriter = new StreamWriter(filename, true))
                //{
                //    streamWriter.WriteLine(DateTime.UtcNow);
                //}

                //using (var streamReader = new StreamReader(filename))
                //{
                //    string content = streamReader.ReadToEnd();
                //    System.Diagnostics.Debug.WriteLine(content);
                //}



                //var imageSource = ImgProfileImage.Source.;
                //var result = await service.SaveProfilePhoto(new UserProfileImage() {Id = user.Id, Image = imageSource.});

                //if (result != null)
                //{
                //    if (result.IsOk)
                //    {
                //        await App.NavigationPage.PopAsync();
                //    }
                //    else
                //    {
                //        await DisplayAlert("Warning", result.ErrorMessage, "OK");
                //    }

                //    //var mainPage = new MainPage();
                //    //mainPage.BindingContext = user;
                //    //LocalSettings.CurrnetUser = user;
                //    //Application.Current.MainPage = new NavigationPage(mainPage);
                //    // await App.NavigationPage.PushAsync(new MainPage());
                //}
                //else
                //{
                //    await DisplayAlert("Warning", "Error while uploading photo", "OK");
                //}
            }
            else
            {
                await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "User is Empty", "OK"));
            }

			viewModel.IsBusy = false;
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


        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await App.NavigationPage.PopAsync();
        }
    }
}

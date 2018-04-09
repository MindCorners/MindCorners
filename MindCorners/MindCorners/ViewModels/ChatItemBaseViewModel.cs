using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.CustomControls;
using MindCorners.DAL;
using MindCorners.Models;
using MindCorners.Models.Enums;
using MindCorners.Pages;
using MindCorners.Pages.PromptTemplates;
using MindCorners.Pages.UserControls;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Plugin.Media;

namespace MindCorners.ViewModels
{
    public class ChatItemBaseViewModel : BaseViewModel
    {
        private bool canCreatePrompt;
        public bool CanCreatePrompt
        {
            get { return canCreatePrompt; }
            set
            {
                canCreatePrompt = value;
                OnPropertyChanged();
            }
        }


        private Post parentPost;
        public Post ParentPost
        {
            get { return parentPost; }
            set
            {
                parentPost = value;
                OnPropertyChanged();
                //LoadChatInfo
            }
        }

        private ObservableCollection<Post> replies;
        public ObservableCollection<Post> Replies
        {
            get { return replies; }
            set
            {
                replies = value;
                OnPropertyChanged();
            }
        }

        private Post editigItem;
        public Post EditingItem
        {
            get { return editigItem; }
            set
            {
                editigItem = value;
                OnPropertyChanged();
                //LoadChatInfo
            }
        }
        private bool canReply;
        public bool CanReply
        {
            get { return canReply; }
            set
            {
                canReply = value;
                OnPropertyChanged();
            }
        }

        private bool showTextTempate;
        public bool ShowTextTempate
        {
            get { return showTextTempate; }
            set
            {
                showTextTempate = value;
                OnPropertyChanged();
            }
        }

        public ICommand CreatePostCommand { set; get; }
        public ChatItemBaseViewModel()
        {
            CreatePostCommand = new Command(CreateChatItem);
        }

        protected virtual async Task<Post> CreateNewPost()
        {
            return null;
        }

        private async void CreateChatItem(object chatType)
        {
			await CrossMedia.Current.Initialize();

            //if (!CanCreatePrompt)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Error", "Not all required info is entered", "OK");
            //}
            //else
            {
                var post = await CreateNewPost();

                if (post == null)
                {
                    post = EditingItem;
                }

                if (!CanCreatePrompt && post.Type == (int)PostTypes.Prompt)
                {
                    await Navigation.PushPopupAsync(new CustomAlertDialog("Error", "Not all required info is entered", "OK"));
                }


                var postTypeInt = int.Parse((string)chatType);


				if ( (postTypeInt == (int)ChatType.Image || postTypeInt == (int)ChatType.Video) /*&& (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)*/)
				{
					var camera = DependencyService.Get<ICameraPermissionChecker>();
					if (!camera.HaveCameraPermissions() || !CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
					{
						await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "No Camera available.", "Ok"));
						return;
					}
				}

				if ( (postTypeInt == (int)ChatType.Audio || postTypeInt == (int)ChatType.Video))
				{
					var AudioRecorder =  DependencyService.Get<IAudioRecorder>();
					if (!AudioRecorder.HaveMicrophonePermissions ()) {
						await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "No microphone available.", "Ok"));
						return;
					}
				}

                PostRepository postRepository = new PostRepository();
                Page pageToOpen = null;
                var editingItem = post;
                //CanReply = false;
                ShowTextTempate = post.Type == (int) PostTypes.Prompt;
                //if (CanReply)
                //{
                //    var result = await postRepository.Submit(new Post() {Type = (int)PostTypes.Reply, ParentId = EditingItem.Id});
                //}
                bool pushModal = false;
                switch (postTypeInt)
                {
                    case (int)ChatType.Text:
                       // editingItem.Type = (int)ChatType.Text;
                        pageToOpen = new TextChatItem(new TextChatItemViewModel() { CanReply = CanReply, Replies = Replies, ParentPost = post, EditingItem = new PostAttachment() { Type = (int)ChatType.Text }, ShowTextTempate = ShowTextTempate, CanSend = false});
                        break;
                    case (int)ChatType.Image:
                        //editingItem.Type = (int)ChatType.Image;
                        // pageToOpen = new ImageChatItem(new ImageChatItemViewModel() { Replies = Replies, ParentPost = post, EditingItem = new PostAttachment() { Type = (int)ChatType.Image } });
                        pushModal = true;
                        var page = new CustomCamera(new ImageChatItemViewModel() { Replies = Replies, ParentPost = post, EditingItem = new PostAttachment() { Type = (int)ChatType.Image } })
                        {
                            CameraRotateImage = "rotateCamera.png",
                            PictureGallaryImage = "cameraGallary.png",
                            CancelImage = "backArrow.png",
                            TakePhotoImage = "takePicture.png",
                            FlashLightOnImage = "flashOn.png",
                            FlashLightOffImage = "flashOn.png"
                        };
                        pageToOpen = page;
                        //pageToOpen = new PictureTake(new CameraViewModel());
                        break;
                    case (int)ChatType.Audio:
                        // editingItem.Type = (int)ChatType.Audio;
                        
                        pageToOpen = new AudioChatItem(new AudioChatItemViewModel() { Replies = Replies, ParentPost = post, EditingItem = new PostAttachment() { Type = (int)ChatType.Audio } });
                        break;
                    case (int)ChatType.Video:
                        // editingItem.Type = (int)ChatType.Video;
                        pushModal = true;

                        //pageToOpen = new VideoChatItem(new VideoChatItemViewModel() { Replies = Replies, ParentPost = post, EditingItem = new PostAttachment() { Type = (int)ChatType.Video } });
                        var videopage = new CustomVideoCamera(new VideoChatItemViewModel() { Replies = Replies, ParentPost = post, EditingItem = new PostAttachment() { Type = (int)ChatType.Video } })
                        {
                            CameraRotateImage = "rotateCamera.png",
                            PictureGallaryImage = "cameraGallary.png",
                            CancelImage = "backArrow.png",
                            StartVideoImage = "videoStartRecording.png",
                            PauseVideoImage = "takePicture.png",
                            StopVideoImage = "videoStopRecording.png",
                            FlashLightOnImage = "flashOn.png",
                            FlashLightOffImage = "flashOn.png"
                        };
                        pageToOpen = videopage;
                        break;
                }

                // var result = await postRepository.Submit(editingItem);

                //if (result != null)
                // {
                //   if (result.IsOk && result.Id.HasValue)
                //    {

                //await Application.Current.MainPage.DisplayAlert("Success", "Prompt type was saved", "OK");
                // EditingItem.Id = result.Id.Value;
                if (pushModal)
                {
                    await App.NavigationPage.PushAsync(pageToOpen);
                }
                else
                { await App.NavigationPage.PushAsync(pageToOpen);
                }
               
                //  }
                //    else
                //   {
                //      await Application.Current.MainPage.DisplayAlert("Error", result.ErrorMessage, "OK");
                //   }
                // }
                //  else
                //  {
                //      await Application.Current.MainPage.DisplayAlert("Warning", "Error", "OK");
                //  }

            }
        }
    }
}

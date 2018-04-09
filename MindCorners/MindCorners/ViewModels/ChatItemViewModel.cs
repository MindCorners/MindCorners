using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.DAL;
using MindCorners.Helpers;
using MindCorners.Models;
using MindCorners.Models.Enums;
using MindCorners.Pages;
using MindCorners.Pages.PromptTemplates;
using MindCorners.Pages.UserControls;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using XLabs;

namespace MindCorners.ViewModels
{
    public class ChatItemViewModel : ChatItemBaseViewModel
    {
        public ICommand BackCommand { protected set; get; }
        public ICommand ReplyCommand { protected set; get; }
        public ICommand OpenImageInNewWindowCommand { get; set; }
        public ICommand DeletePromptCommand { get; set; }
        public ChatItemViewModel()
        {

            ReplyCommand = new Command(CreateReplyChatItem);
            OpenImageInNewWindowCommand = new Command(OpenImagePreview);
            BackCommand = new Command(Back);
            DeletePromptCommand = new Command(DeletePrompt);
        }
        private ObservableCollection<Post> latestPosts;
        public ObservableCollection<Post> LatestPosts
        {
            get { return latestPosts; }
            set
            {
                latestPosts = value;
                OnPropertyChanged();
            }
        }

        //private ObservableCollection<Post> chatItems;
        //public ObservableCollection<Post> ChatItems
        //{
        //    get { return chatItems; }
        //    set
        //    {
        //        chatItems = value;
        //        OnPropertyChanged();
        //        IsEmptyChat = chatItems.Count > 0;
        //    }
        //}
        private Post selectedItem;
        public Post SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                selectedItem = null;
                OnPropertyChanged();
            }
        }

        private bool canDeletePost;
        public bool CanDeletePost
        {
            get { return canDeletePost; }
            set
            {
                canDeletePost = value;
                OnPropertyChanged();
            }
        }

        private List<PostAttachment> mainPostAttachments;
        public List<PostAttachment> MainPostAttachments
        {
            get { return mainPostAttachments; }
            set
            {
                mainPostAttachments = value;
                OnPropertyChanged();
            }
        }

        private bool isNew;
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                OnPropertyChanged();
            }
        }

        private bool isEmptyChat;

        public bool IsEmptyChat
        {
            get { return isEmptyChat; }
            set
            {
                isEmptyChat = value;
                OnPropertyChanged();
            }
        }

        private bool hasNoReplies;

		public bool HasNoReplies
		{
			get { return hasNoReplies; }
			set
			{
				hasNoReplies = value;
				OnPropertyChanged();
			}
		}

        private Guid? editingPostId;

        public Guid? EditingPostId
        {
            get { return editingPostId; }
            set
            {
                editingPostId = value;
                OnPropertyChanged();
                Task.Run(LoadChatPosts);
            }
        }

        public async Task LoadChatPosts()
        {
            IsBusy = true;
            if (IsNew)
            {
                IsEmptyChat = true;
            }
            else
            {
                PostRepository postRepository = new PostRepository();
                PostAttachmentRepository postAttachmentRepository = new PostAttachmentRepository();
                var observableReplies = new ObservableCollection<Post>();
                observableReplies.Add(EditingItem);
                var replies = await postRepository.GetRepleis(EditingItem.Id);
                if (replies != null)
                {
                    foreach (var reply in replies)
                    {
                        reply.CanSendTellMeMore = EditingItem.CreatorId == Settings.CurrentUserId && reply.CreatorId != Settings.CurrentUserId;
                        observableReplies.Add(reply);
                        //get TellMeMores
                        var tellMemMores = await postRepository.GetReplyTellMeMores(reply.Id);
                        if (tellMemMores != null)
                        {
                            foreach (var tellMemMore in tellMemMores)
                            {
                                observableReplies.Add(tellMemMore);
                            }
                        }
                    }
                }

                Replies = new ObservableCollection<Post>(observableReplies);
                MainPostAttachments = await postAttachmentRepository.GetAttachments(EditingItem.Id);
                IsEmptyChat = Replies.Count() == 0;
				HasNoReplies = Replies.Count(p => p.Type != (int)PostTypes.Prompt) == 0;
                CanReply = MainPostAttachments?.Count > 0;
                CanDeletePost = EditingItem.CreatorId == Settings.CurrentUserId;
            }
            IsBusy = false;
            // Circles = await circleRepository.GetAllObservableCollection(LocalSettings.CurrentUserId);
        }

        protected async override Task<Post> CreateNewPost()
        {
            PostRepository postRepository = new PostRepository();
            if (!IsNew)
            {
                var editingItem = new Post()
                {
                    Type = (int)PostTypes.Reply,
                    Title = string.Format("Reply:{0} - {1}", EditingItem.Title, DateTime.Now),
                    ParentId = EditingItem.Id,
                    CircleId = EditingItem.CircleId,
                    CircleName = EditingItem.CircleName,
                    UserId = EditingItem.UserId
                };

                return editingItem;

            }
            // editingItem.Type = (int)PostTypes.Prompt;
            // var result = await postRepository.Submit(editingItem);

            //if (result != null)
            //{
            //    if (result.IsOk && result.Id.HasValue)
            //    {
            //        // await Application.Current.MainPage.DisplayAlert("Success", "Prompt was saved", "OK");
            //        editingItem.Id = result.Id.Value;
            //       // EditingItem = editingItem;
            //    }
            //    else
            //    {
            //        await Application.Current.MainPage.DisplayAlert("Error", result.ErrorMessage, "OK");
            //    }
            //}
            //else
            //{
            //    await Application.Current.MainPage.DisplayAlert("Warning", "Error", "OK");
            //}
            return null;
        }


        private async void CreateReplyChatItem()
        {
            await App.NavigationPage.PushAsync(new ReplyItem(new ReplyItemViewModel()
            {
                Replies = Replies,
                ParentPost = EditingItem,
                EditingItem = new Post()
                {
                    Type = (int)PostTypes.Reply,
                    Title = string.Format("Reply:{0} - {1}", EditingItem.Title, DateTime.Now),
                    ParentId = EditingItem.Id,
                    CircleId = EditingItem.CircleId,
                    CircleName = EditingItem.CircleName,
                    UserId = EditingItem.UserId
                }
            }));
        }

        private async void OpenImagePreview(object item)
        {
            var dataItem = (PostAttachment)item;
            if (dataItem != null)
            {
                await Navigation.PushModalAsync(new ImagePreview(dataItem));
            }
        }

        private void Back()
        {
            RefreshLatestPosts();
            App.NavigationPage.PopAsync();
        }
		private async void OkAction()
		{
			IsBusy = true;
			PostRepository postRepository = new PostRepository();
			var result = await postRepository.Delete(new Post() { Id = EditingItem.Id });
			if (result.IsOk)
			{
				Back();
			}
			else
			{
				await Navigation.PushPopupAsync(new CustomAlertDialog("Error", result.ErrorMessage, "Ok"));
			}
		}
        private async void DeletePrompt()
        {
			var alertPage = new CustomAlertDialog("Warning", "Do you realy want to delete prompt and all it's content?", "Yes", "No", OkAction, null);

			await Navigation.PushPopupAsync(alertPage);
			/*
            var answer = await Application.Current.MainPage.DisplayAlert("Warning", "Do you realy want to delete prompt and all it's content?", "Yes", "No");
            if (answer)
            {
                PostRepository postRepository = new PostRepository();
                var result = await postRepository.Delete(new Post() { Id = EditingItem.Id });
                if (result.IsOk)
                {
                    Back();
                }
                else
                {
                    await Navigation.PushPopupAsync(new CustomAlertDialog("Error", result.ErrorMessage, "Ok"));
                }
            }*/
		}
        private async void RefreshLatestPosts()
        {
            var pages = Navigation.NavigationStack;
            var mainPage = pages.FirstOrDefault(p => p.ClassId == "MindCorners.Pages.MainPage");
            if (mainPage != null)
            {
                var context = (MainViewModel)mainPage.BindingContext;
                await context.LoadPosts();
            }
        }
    }
}
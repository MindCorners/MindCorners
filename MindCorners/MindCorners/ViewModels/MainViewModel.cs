using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.Code;
using MindCorners.DAL;
using MindCorners.Helpers;
using MindCorners.Models;
using MindCorners.Pages;
using MindCorners.ViewModels;
using Xamarin.Forms;
using XLabs;

namespace MindCorners.ViewModels
{
	public class MainViewModel : BaseViewModel
	{
		public ICommand EditPostCommand { protected set; get; }
		public ICommand OpenArchiveCommand { protected set; get; }
		public MainViewModel()
		{
			Task.Run(LoadNewNotifications);
			EditPostCommand = new Command(LoadPostItem);
			OpenArchiveCommand = new Command(OpenArchive);
			IsBusy = true;
			Task.Run(LoadAllBigData).Wait();
			IsBusy = false;
			//IsBusy = false;
		}

		private async  Task LoadAllBigData()
		{
			LatestPosts = await LoadPostsMain();
			await Global.LoadCircles();
			await Global.LoadContacts();
			Global.LatestPosts = LatestPosts.ToList();

			ContactRepository service = new ContactRepository();
			var user = await service.GetById(Settings.CurrentUserId);

			if (user != null)
			{
				Settings.CurrentUserProfileImageString = user.ProfileImageString;
			}
		}

		private User _CurrentUser = null;
		public User CurrentUser
		{
			get
			{
				return _CurrentUser;
			}
			set
			{
				_CurrentUser = value;
				OnPropertyChanged();
			}
		}


		private int listCount;

		public int ListCount
		{
			get { return listCount; }
			set { listCount = value; OnPropertyChanged(); }
		}

		private ObservableCollection<Post> latestPosts = new ObservableCollection<Post>();
		public ObservableCollection<Post> LatestPosts
		{
			get { return latestPosts; }
			set
			{
				latestPosts = value;
				OnPropertyChanged();
				ListCount = latestPosts.Count;
			}
		}

		public async Task LoadPosts()
		{
			IsBusy = true;
			LatestPosts = await LoadPostsMain();
			IsBusy = false;
		}

		private async Task<ObservableCollection<Post>> LoadPostsMain()
		{
			PostRepository postRepository = new PostRepository();
			if (postRepository != null)
			{
				var posts = await postRepository.GetLatest();
				if (posts != null)
				{
					foreach (var post in posts)
					{
						post.CanShowFormattedString = true;
					}
					return new ObservableCollection<Post>(posts);
				}
			}

			return new ObservableCollection<Post>();
		}



		private async void LoadPostItem(object id)
		{
			PostRepository postRepository = new PostRepository();
			var posts = await postRepository.GetItem((Guid)id);
			posts.CanShowFormattedString = true;
			//EditingItem.Id = result.Id.Value;
			// await App.NavigationPage.PushAsync(new Post(new PostItemViewModel()));
			await App.NavigationPage.PushAsync(new ChatItem(new ChatItemViewModel() { EditingItem = posts, EditingPostId = posts.Id }));

		}

		private async void OpenArchive()
		{
			await App.NavigationPage.PushAsync(new PromptsList(new PromptsListViewModel() { ArchiveResultText = "History" }));
		}

	}
}

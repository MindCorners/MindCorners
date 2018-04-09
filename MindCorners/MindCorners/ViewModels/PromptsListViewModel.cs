using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.Code;
using MindCorners.DAL;
using MindCorners.Models;
using MindCorners.Pages;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.ViewModels
{
	public class PromptsListViewModel : BaseViewModel
	{
		public ICommand EditPostCommand { protected set; get; }
		public ICommand OpenLatestCommand { protected set; get; }

		public ICommand LoadMoreCommand { protected set; get; }

		public ICommand SearchButtonClickedCommand { protected set; get; }

		public ICommand HideSearchBarCommand { protected set; get; }
		public ICommand ClearSearchButtonClickedCommand { protected set; get; }
		public Entry SearchEntry;

		public PromptsListViewModel()
		{
			EditPostCommand = new Command(LoadPostItem);
			OpenLatestCommand = new Command(OpenLatest);
			LoadMoreCommand = new Command(LoadMore);
			SearchButtonClickedCommand = new Command(SearchButtonClicked);
			ClearSearchButtonClickedCommand = new Command(ClearSearchButtonClicked);
			HideSearchBarCommand = new Command(HideSearchBarClicked);
			var list = Global.LatestPosts.Take(6);
			Posts = new ObservableCollection<Post>(list);
			/*Task.Run(async () => {
				IsBusy = true;

				IsBusy = false;
			});
*/
			//Task.Run(() => {
			//	LoadPosts();
			//});
		}

		private bool showSearchBar;
		public bool ShowSearchBar
		{
			get { return showSearchBar; }
			set
			{
				showSearchBar = value;
				OnPropertyChanged();
				if (SearchEntry != null)
				{
					SearchEntry.Focus();
				}
			}
		}

		private bool seachTextExits;
		public bool SeachTextExits
		{
			get { return seachTextExits; }
			set
			{
				seachTextExits = value;
				OnPropertyChanged();
			}
		}

		private string searchText;
		public string SearchText
		{
			get { return searchText; }
			set
			{
				searchText = value;
				SeachTextExits = false;
				if (!string.IsNullOrEmpty(searchText))
				{
					SeachTextExits = true;

					if (searchText.Length >= 3)
					{
						
					}
				}
				LoadPosts(true);
				OnPropertyChanged();
			}
		}


		private string archiveResultText;

		public string ArchiveResultText
		{
			get { return archiveResultText; }
			set
			{
				archiveResultText = value;
				OnPropertyChanged();
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


		private ObservableCollection<Post> posts;

		public ObservableCollection<Post> Posts
		{
			get { return posts; }
			set
			{
				posts = value;
				OnPropertyChanged();
			}
		}

		private async Task LoadPosts(bool showLoading = false)
		{
			IsBusy = showLoading;
			PostRepository postRepository = new PostRepository();
			var postsList = await postRepository.GetForArchive(0, 10, SearchText);
			if (postsList != null)
			{
				foreach (var post in postsList)
				{
					post.CanShowFormattedString = true;
				}
				Posts = new ObservableCollection<Post>(postsList);
				Global.LatestPosts = Posts.ToList();
			}
			IsBusy = false;
		}

		private async void LoadPostItem(object id)
		{
			PostRepository postRepository = new PostRepository();
			var post = await postRepository.GetItem((Guid)id);
			post.CanShowFormattedString = true;
			//EditingItem.Id = result.Id.Value;
			// await App.NavigationPage.PushAsync(new Post(new PostItemViewModel()));
			await App.NavigationPage.PushAsync(new ChatItem(new ChatItemViewModel() { EditingItem = post, EditingPostId = post.Id }));
		}

		private async void OpenLatest()
		{
			var pages = Navigation.NavigationStack;
			var mainPage = pages.FirstOrDefault(p => p.ClassId == "MindCorners.Pages.MainPage");
			if (mainPage != null)
			{
				//var context = (MainViewModel)mainPage.BindingContext;
				//await context.LoadPosts();
			}

			await App.NavigationPage.PopAsync();
		}

		private async void LoadMore()
		{
			PostRepository postRepository = new PostRepository();
			var postsList = await postRepository.GetForArchive(Posts.Count, 5, SearchText);
			if (postsList != null)
			{
				foreach (var post in postsList)
				{
					post.CanShowFormattedString = true;
					Posts.Add(post);
				}
				Global.LatestPosts = Posts.ToList();
			}
		}

		private async void SearchButtonClicked()
		{
			await LoadPosts (true);
			/*if (SearchText.Length >= 3) {
				
			} else {
				Posts = new ObservableCollection<Post>(Global.LatestPosts);
			}*/
			//ShowSearchBar = true;
			//await App.NavigationPage.PushAsync(new PromptsList(new PromptsListViewModel() { ArchiveResultText = "History", ShowSearchBar = true }));
		}
		private async void ClearSearchButtonClicked()
		{
			SearchText = null;
			SearchEntry.Unfocus ();
			await LoadPosts(true);
		}
		private async void HideSearchBarClicked()
		{
			SearchText = null;
			ShowSearchBar = false;
			SearchEntry.Unfocus ();
			await LoadPosts(true);
		}

		public override void ShowSearchBarButtonClicked()
		{   
			ShowSearchBar = true;
		}
	}
}

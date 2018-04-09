using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.Code;
using MindCorners.DAL;
using MindCorners.Helpers;
using MindCorners.Models;
using MindCorners.Models.Enums;
using MindCorners.Pages;
using Xamarin.Forms;
using XLabs;

namespace MindCorners.ViewModels
{
	public class PromptItemViewModel : ChatItemBaseViewModel
	{
		public Entry TitleEntry;
		private ObservableCollection<Contact> contacts;
		public ObservableCollection<Contact> Contacts
		{
			get { return contacts; }
			set
			{
				contacts = value;
				OnPropertyChanged();
			}
		}

		private ObservableCollection<Circle> circles;
		public ObservableCollection<Circle> Circles
		{
			get { return circles; }
			set
			{
				circles = value;
				OnPropertyChanged();
			}
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

		public ICommand CreatePromptCommand { protected set; get; }

		public PromptItemViewModel()
		{
			CreatePromptCommand = new Command(CreatePrompt);
		}

		private Post editigItem;
		public Post EditingItem
		{
			get { return editigItem; }
			set
			{
				editigItem = value;
				OnPropertyChanged();
				if (value != null)
				{
					Task.Run(async () =>
						{   
							await LoadContacts();
							await LoadCircles();
						});
				}
				else
				{
					Contacts = null;
				}
			}
		}

		private Contact selectedContact;
		public Contact SelectedContact
		{
			get { return selectedContact; }
			set
			{
				if (value != null)
				{
					value.IsSelected = !value.IsSelected;
					selectedContact = null;
					UpdateListSelection(Circles);
					UpdateListSelection(Contacts, value.Id);
					UpdateCanCreatePrompt();
				}
				OnPropertyChanged();
				if (TitleEntry != null)
				{
					TitleEntry.Unfocus();
				}
			}
		}
		private Circle selectedCircle;
		public Circle SelectedCircle
		{
			get { return selectedCircle; }
			set
			{
				if (value != null)
				{   
					//Circle tempFriend = value;
					value.IsSelected = !value.IsSelected;
					selectedCircle = null;
					UpdateListSelection(Circles, value.Id);
					UpdateListSelection(Contacts);
					UpdateCanCreatePrompt();
				}
				OnPropertyChanged();

				if (TitleEntry != null)
				{
					TitleEntry.Unfocus();
				}
			}
		}

		private string promptTitle;
		public string PromptTitle
		{
			get { return promptTitle; }
			set
			{
				promptTitle = value;
				OnPropertyChanged();
				EditingItem.Title = value;
				UpdateCanCreatePrompt();
			}
		}

		private void UpdateCanCreatePrompt()
		{
			CanCreatePrompt = (Circles.Any(p => p.IsSelected) || Contacts.Any(p => p.IsSelected)) &&
				!string.IsNullOrEmpty(PromptTitle);


		}

		private async void CreatePrompt()
		{
			PostRepository postRepository = new PostRepository();
			var editingItem = EditingItem;
			var circleName = string.Empty;
			if (Contacts.Any(p => p.IsSelected))
			{
				var selectedContact = Contacts.First(p => p.IsSelected);
				editingItem.SelectedContact = new Contact() { Id = selectedContact.Id};
				circleName = selectedContact.FullName;
			}
			else if (Circles.Any(p => p.IsSelected))
			{
				var selectedCircle = Circles.First(p => p.IsSelected);
				editingItem.SelectedCircle = new Circle() { Id = selectedCircle.Id };
				circleName = selectedCircle.Name;
			}

			editingItem.Type = (int) PostTypes.Prompt;
			//var result = await postRepository.Submit(editingItem);

			//if (result != null)
			{
				//if (result.IsOk && result.Id.HasValue)
				{
					//await Application.Current.MainPage.DisplayAlert("Success", "Prompt was saved", "OK");
					//EditingItem.Id = result.Id.Value;
					EditingItem.CanShowFormattedString = true;
					EditingItem.CreatorFullName = Settings.CurrentUserFullName;
					EditingItem.UserProfileImageName = Settings.CurrentUserProfileImageString;
					EditingItem.CircleName = circleName;
					EditingItem.MainAttachment = new PostAttachment() {Type = -1};
					//LatestPosts.Insert(0, EditingItem);
					await App.NavigationPage.PushAsync(new ChatItem(new ChatItemViewModel(){CanCreatePrompt = CanCreatePrompt, EditingItem = EditingItem , CanReply=false, CanDeletePost = false, IsNew = true, ShowTextTempate = true, EditingPostId = Guid.Empty}));
				}
				//else
				// {
				//     await Application.Current.MainPage.DisplayAlert("Error", result.ErrorMessage, "OK");
				// }
			}
			// else
			// {
			//  await Application.Current.MainPage.DisplayAlert("Warning", "Error", "OK");
			// }


			//   App.NavigationPage.PushAsync(new CircleItem(new CircleViewModel() { Circles = Circles, IsNew = true, EditingItem = new Circle() }));
		}
		private async Task LoadContacts()
		{
			var list = Global.Contacts;

			Contacts = new ObservableCollection<Contact>(list);
			UpdateListSelection(Contacts);
		}

		private async Task LoadCircles()
		{
			//CircleRepository circleRepository = new CircleRepository();
			//Circles = await circleRepository.GetAllObservableCollection(Settings.CurrentUserId);

			var list = Global.Circles;
			Circles = new ObservableCollection<Circle>(list);
			UpdateListSelection(Circles);
			if (Circles == null || !Circles.Any() && Contacts != null && Contacts.Count==1)
			{
				var contact = Contacts.First();
				contact.IsSelected = true;
			}
		}

		protected async override Task<Post> CreateNewPost()
		{

			var editingItem = EditingItem;
			var circleName = string.Empty;
			if (Contacts.Any(p => p.IsSelected))
			{
				var selectedContact = Contacts.First(p => p.IsSelected);
				editingItem.SelectedContact = new Contact() { Id = selectedContact.Id };
				circleName = selectedContact.FullName;
			}
			else if (Circles.Any(p => p.IsSelected))
			{
				var selectedCircle = Circles.First(p => p.IsSelected);
				editingItem.SelectedCircle = new Circle() { Id = selectedCircle.Id };
				circleName = selectedCircle.Name;
			}

			editingItem.Type = (int)PostTypes.Prompt;
			editingItem.CircleName = circleName;
			return editingItem;
		}
	}
}

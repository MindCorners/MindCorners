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
using MindCorners.Models.Enums;
using MindCorners.Pages;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.ViewModels
{
	public class NotificationsListViewModel : BaseViewModel
	{
		public ICommand GoToResourceCommand { protected set; get; }
		public ICommand LoadMoreCommand { protected set; get; }
		public NotificationsListViewModel()
		{
			Task.Run(LoadNotifications);
			LoadMoreCommand = new Command(LoadMore);
			GoToResourceCommand = new Command(GoToResource);
		}


		private ObservableCollection<Notification> notifications;
		public ObservableCollection<Notification> Notifications
		{
			get { return notifications; }
			set
			{
				notifications = value;
				OnPropertyChanged();
			}
		}

		private Notification selectedItem;
		public Notification SelectedItem
		{
			get { return selectedItem; }
			set
			{
				selectedItem = value;
				OnPropertyChanged();
			}
		}

		private async Task LoadNotifications()
		{
			IsBusy = true;
			NotificationRepository notificationRepository = new NotificationRepository();
			var list = await notificationRepository.Get(0, 10);
			if (list != null)
			{
				Notifications = new ObservableCollection<Notification>(list);

				foreach (var notification in list.Where(p => !p.ReadDate.HasValue).ToList())
				{
					await notificationRepository.UpdateRead(notification);
				}
			}
			IsBusy = false;

			//update notificationIcon Label
			var pages = Navigation.NavigationStack;
			var mainPage = pages.FirstOrDefault(p => p.ClassId == "MindCorners.Pages.MainPage");
			if (mainPage != null)
			{
				var context = (MainViewModel)mainPage.BindingContext;
				context.NumberOfNewNotifications = 0;
			}
			var contactsPage = pages.FirstOrDefault(p => p.ClassId == "MindCorners.Pages.Contacts");
			if (contactsPage != null)
			{
				var context = (ContactsListViewModel)contactsPage.BindingContext;
				context.NumberOfNewNotifications = 0;
			}

			//update notificationsRead State


		}
		private async void LoadMore()
		{
			NotificationRepository notificationRepository = new NotificationRepository();
			var list = await notificationRepository.Get(Notifications.Count, 5);
			if (list != null)
			{
				foreach (var item in list)
				{
					Notifications.Add(item);
					foreach (var notification in list.Where(p => !p.ReadDate.HasValue).ToList())
					{
						await notificationRepository.UpdateRead(notification);
					}
				}
			}
		}

		private async void GoToResource(object id)
		{
			CircleRepository circleRepository = new CircleRepository();
			NotificationRepository notificationRepository  = new NotificationRepository();
			ContactRepository contactRepository = new ContactRepository();
			var selectedNotification = await  notificationRepository.GetItem((Guid) id);
			if (selectedNotification != null)
			{
				var notificationType = selectedNotification.Type;
				switch (notificationType)
				{
				case (int)NotificationTypes.InvitationToContactsRecieved:

					if (selectedNotification.SourceId.HasValue)
					{
						var contact = await contactRepository.GetById(selectedNotification.SourceId.Value);
						if (contact != null)
						{
							await
							App.NavigationPage.PushAsync(new ContactItem(new ContactViewModel() { ViewItem = contact }));
						}
					}
					break;
				case (int)NotificationTypes.InvitationToContactsConfirmed:
					if (selectedNotification.SourceId.HasValue)
					{
						var contact = await contactRepository.GetById(selectedNotification.SourceId.Value);
						if (contact != null)
						{
							await App.NavigationPage.PushAsync(new ContactItem(new ContactViewModel() { ViewItem = contact }));
						}
					}
					break;

				case (int)NotificationTypes.InvitationToCircle:
					{
						if (selectedNotification.SourceId.HasValue)
						{
							var circleItem = await circleRepository.GetItem(selectedNotification.SourceId.Value);
							if (circleItem != null)
							{
								await
								App.NavigationPage.PushAsync(new CircleItem(new CircleViewModel() { EditingItem = circleItem }));
							}
						}
					}
					break;
				case (int)NotificationTypes.NewPromptInCircle:
				case (int)NotificationTypes.Reply:
				case (int)NotificationTypes.ReplyWasRead:
				case (int)NotificationTypes.TellMeMore:
					{
						if (selectedNotification.SourceId.HasValue)
						{
							PostRepository postRepository = new PostRepository();
							var post = await postRepository.GetItem(selectedNotification.SourceId.Value);
							if (post != null)
							{
								post.CanShowFormattedString = true;
								//EditingItem.Id = result.Id.Value;
								// await App.NavigationPage.PushAsync(new Post(new PostItemViewModel()));
								await App.NavigationPage.PushAsync(new ChatItem(new ChatItemViewModel() { EditingItem = post, EditingPostId = post.Id }));
							}
						}
					}
					break;
				}
			}

		}
	}
}

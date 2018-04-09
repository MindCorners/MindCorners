using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.Code;
using MindCorners.DAL;
using MindCorners.Helpers;
using MindCorners.Models;
using MindCorners.Pages;
using MindCorners.Pages.UserControls;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace MindCorners.ViewModels
{
	public class ContactsListViewModel : BaseViewModel
	{
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

		public ICommand EditCircleCommand { protected set; get; }
		public ICommand DeleteCircleCommand { protected set; get; }
		public ICommand LeaveCircleCommand { protected set; get; }
		public ICommand ViewCircleCommand { protected set; get; }

		public ICommand CreateCircleCommand { protected set; get; }

		public ICommand CreateInvitationCommand { protected set; get; }

		public ICommand BackCommand { protected set; get; }
		Contact selectedContact;
		Circle selectedCircle;

		public ContactsListViewModel()
		{
			CreateCircleCommand = new Command(CreateCircle);
			CreateInvitationCommand = new Command(CreateInvitation);

			EditCircleCommand = new Command(EditCircle);
			DeleteCircleCommand = new Command(DeleteCircle);
			LeaveCircleCommand = new Command(LeaveCircle);
			ViewCircleCommand = new Command(ViewCircle);
			BackCommand = new Command(Back);
			NumberOfNewNotifications = Global.NewNotificationsCount;
			Task.Run(LoadNewNotifications);

			Task.Run(async () =>
				{
					IsBusy = true;
					await LoadContacts();
					await LoadCircles();
					IsBusy = false;
				});

		}

		public Contact SelectedContact
		{
			get { return selectedContact; }
			set
			{
				if (selectedContact != value)
				{
					Contact tempFriend = value;
					selectedContact = null;
					OnPropertyChanged();
					App.NavigationPage.PushAsync(new ContactItem(new ContactViewModel() {ViewItem = tempFriend}));
				}
			}
		}
		public Circle SelectedCircle
		{
			get { return selectedCircle; }
			set
			{
				if (selectedCircle != value)
				{
					Circle tempFriend = value;
					selectedCircle = null;
					OnPropertyChanged();
					EditCircle(tempFriend.Id);
					//App.NavigationPage.PushAsync(new ContactItem(new ContactViewModel() {ViewItem = tempFriend}));
				}
			}
		}
		private void CreateCircle()
		{
			App.NavigationPage.PushAsync(new CircleItem(new CircleViewModel() {Circles = Circles, IsNew = true, EditingItem = new Circle()}));
		}

		private void CreateInvitation()
		{
			App.NavigationPage.PushAsync(new InviteContact(new InviteContactViewModel() { Contacts = Contacts }));
		}
		private void Back()
		{
			App.NavigationPage.PopAsync();
		}

		private async Task LoadContacts()
		{
			//if (Global.Contacts != null)
			//{
			//	Contacts = new ObservableCollection<Contact>(Global.Contacts);
			//}
			//else
			{   
				var list = await Global.LoadContacts();
				Contacts = new ObservableCollection<Contact>(list);
			}
		}

		private async Task LoadCircles()
		{
			//if (Global.Circles != null)
			//{
			//	Circles = new ObservableCollection<Circle>(Global.Circles);
			//}
			//else
			{
				var list = await Global.LoadCircles();
				Circles = new ObservableCollection<Circle>(list);
			}
		}

		private Circle GetCircleById(Guid id)
		{
			return Circles.FirstOrDefault(p => p.Id == id);
		}

		private void EditCircle(object circleId)
		{
			var circleItem = GetCircleById((Guid) circleId);
			App.NavigationPage.PushAsync(new CircleItem(new CircleViewModel() {IsView= !circleItem.IsCreatedByUser, Circles = Circles, EditingItem = circleItem}));
		}
		private void ViewCircle(object circleId)
		{
			var circleItem = GetCircleById((Guid) circleId);
			App.NavigationPage.PushAsync(new CircleItem(new CircleViewModel() { IsView = true, EditingItem = circleItem}));
		}

		private async void DeleteCircle(object circleId)
		{

			var alertPage = new CustomAlertDialog("Warning", "Do you realy want to delete circle?", "Yes", "No", async ()=>{
				CircleRepository circleRepository = new CircleRepository();
				var result = await circleRepository.Delete(new Circle {Id = (Guid) circleId});
				if (result != null)
				{
					if (result.IsOk)
					{
						await Navigation.PushPopupAsync(new CustomAlertDialog("Success", "Circle was deleted", "OK"));
						var circle = GetCircleById((Guid) circleId);
						Circles.Remove(circle);
						Global.Circles.Remove(circle);
						//await App.NavigationPage.PopAsync();
					}
					else
					{
						await Navigation.PushPopupAsync(new CustomAlertDialog("Error", result.ErrorMessage, "OK"));
					}
				}
				else
				{
					await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "Error", "OK"));
				}
			}, null);

			await Navigation.PushPopupAsync(alertPage);

			/*var answer = await Application.Current.MainPage.DisplayAlert("Warning", "Do you realy want to delete circle?", "Yes", "No");
			if (answer)
			{
				

			}*/
			// App.NavigationPage.PushAsync(new CircleItem(new CircleViewModel(Navigation) { SelectedCircle = new Circle() { Id = (Guid)selectedCircleId } }));
		}
		private async void LeaveCircle(object circleId)
		{
			var alertPage = new CustomAlertDialog("Warning", "Do you realy want to leave circle?", "Yes", "No", async ()=>{
				CircleRepository circleRepository = new CircleRepository();
				var result = await circleRepository.Leave(new Circle {Id = (Guid) circleId});
				if (result != null)
				{
					if (result.IsOk)
					{
						await Navigation.PushPopupAsync(new CustomAlertDialog("Success", "Circle was left", "OK"));
						var circle = GetCircleById((Guid) circleId);
						Circles.Remove(circle);

						Global.Circles.Remove(circle);
						//await App.NavigationPage.PopAsync();
					}
					else
					{
						await Navigation.PushPopupAsync(new CustomAlertDialog("Error", result.ErrorMessage, "OK"));
					}
				}
				else
				{
					await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "Error", "OK"));
				}
			}, null);

			await Navigation.PushPopupAsync(alertPage);

			/*var answer = await Application.Current.MainPage.DisplayAlert("Warning", "Do you realy want to leave circle?", "Yes", "No");
			if (answer)
			{
				
			}*/
		}
	}
}

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
using MindCorners.Pages;
using MindCorners.Pages.UserControls;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace MindCorners.ViewModels
{
	public class CircleViewModel : BaseViewModel
	{
		public Entry TitleEntry;
		private bool canSave;
		public bool CanSave
		{
			get { return canSave; }
			set
			{
				canSave = value;
				OnPropertyChanged();
			}
		}

		private string circleTitle;
		public string CircleTitle
		{
			get { return circleTitle; }
			set
			{
				circleTitle = value;
				OnPropertyChanged();
				if (EditingItem != null) {

					EditingItem.Name = value;	
				}
				UpdateCanSave();
			}
		}

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


		public CircleViewModel()
		{  
			SubmitCircleCommand = new Command(SubmitCircle);
		}

		private bool isNew;
		public bool IsNew
		{
			get { return isNew; }
			set { isNew = value; OnPropertyChanged(); }
		}

		private Circle editigItem;
		public Circle EditingItem
		{
			get { return editigItem; }
			set
			{
				editigItem = value;
				OnPropertyChanged();
				if (value != null)
				{
					Task.Run(LoadContacts);
				}
				else
				{
					Contacts = null;
				}
				if (editigItem != null)
				{
					CircleTitle = editigItem.Name;
				}
			}
		}

		private bool isView;
		public bool IsView
		{
			get { return isView; }
			set
			{
				isView = value;
				OnPropertyChanged();
			}
		}

		private Contact selectedContact;
		public Contact SelectedContact
		{
			get { return selectedContact; }
			set
			{
				Contact tempFriend = value;
				selectedContact = null;
				OnPropertyChanged();
				if (value != null)
				{
					tempFriend.IsSelected = !value.IsSelected;
				}
				if (TitleEntry != null)
				{
					TitleEntry.Unfocus();
				}
				UpdateCanSave();
			}
		}

		private void UpdateCanSave()
		{
			CanSave = Contacts != null && (Contacts.Any(p => p.IsSelected)) &&
				!string.IsNullOrEmpty(EditingItem.Name) && !IsView;
		}

		//private Guid circleId;
		//public Guid CircleId
		//{
		//    get { return circleId; }
		//    set
		//    {
		//        circleId = value;
		//        OnPropertyChanged();
		//    }
		//}

		//private string circleName;
		//public string CircleName
		//{
		//    get { return circleName; }
		//    set
		//    {
		//        circleName = value;
		//        OnPropertyChanged();
		//    }
		//}

		//private bool isCreatedByUser;
		//public bool IsCreatedByUser
		//{
		//    get { return isCreatedByUser; }
		//    set
		//    {
		//        isCreatedByUser = value;
		//        OnPropertyChanged();
		//    }
		//}

		public ICommand SubmitCircleCommand { protected set; get; }
		private async void SubmitCircle()
		{
			IsBusy = true;
			CircleRepository circleRepository = new CircleRepository();
			var circle = EditingItem;
			circle.SelectedContacts = Contacts.Where(p => p.IsSelected).Select(p => new Contact() {Id = p.Id}).ToList();
			var result = await circleRepository.Submit(circle);

			if (result != null)
			{
				if (result.IsOk && result.Id.HasValue)
				{
					await Navigation.PushPopupAsync(new CustomAlertDialog("Success", "Circle was saved", "OK"));
					if (IsNew)
					{
						EditingItem.Id = result.Id.Value;
						EditingItem.IsCreatedByUser = true;
						Circles.Add(EditingItem);
						Global.Circles.Add(EditingItem);
					}
					//Contacts = null;
					EditingItem = null;
					await App.NavigationPage.PopAsync();
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
			IsBusy = false;
		}

		private async Task LoadContacts()
		{
			IsBusy = true;
			ContactRepository contactRepository = new ContactRepository();
			List<Contact> contacts = null;
			if (IsView)
			{
				contacts = await contactRepository.GetOnlySelectedForCircle(EditingItem.Id);
			}
			else
			{
				contacts = await contactRepository.GetAllWithSelectedForCircle(Settings.CurrentUserId, EditingItem.Id);
			}

			if (contacts != null)
			{
				Contacts = new ObservableCollection<Contact>(contacts);
			}
			IsBusy = false;
		}
	}
}

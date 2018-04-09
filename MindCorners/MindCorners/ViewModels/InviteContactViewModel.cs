using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.DAL;
using MindCorners.Models;
using MindCorners.Pages;
using MindCorners.Pages.UserControls;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using System.Text.RegularExpressions;

namespace MindCorners.ViewModels
{
    public class InviteContactViewModel : BaseViewModel
    {
		const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +

			@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
		
        public ObservableCollection<Contact> Contacts { get; set; }

        public InviteContactViewModel() 
        {
            CreateInvitationCommand = new Command(CreateInvitation);
        }

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
				var isValid = (Regex.IsMatch(value, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));

				CanSend = !string.IsNullOrEmpty(value) && isValid;
            }
        }

         private bool canSend;
        public bool CanSend
        {
            get { return canSend; }
            set
            {
                canSend = value;
                OnPropertyChanged();
            }
        }
        public ICommand CreateInvitationCommand { protected set; get; }
        private async void CreateInvitation()
        {
			IsBusy = true;
           // await Navigation.PushModalAsync(new CustomAlertDialog("Invitation was sent"));
            
            InvitationRepository service = new InvitationRepository();
            var invitation = new Invitation() {Email = Email};
            var result = await service.CreateInvitation(invitation);

            if (result != null)
            {
                if (result.IsOk)
                {
                    await Navigation.PushPopupAsync(new CustomAlertDialog("Success", "Invitation was sent", "OK"));
                    // await Application.Current.MainPage.DisplayAlert("Success", "Invitation was sent", "OK");
                    await App.NavigationPage.PopAsync();
                }
                else
                {
                    await Navigation.PushPopupAsync(new CustomAlertDialog("Error", result.ErrorMessage, "OK"));
                }
                //var mainPage = new MainPage();
                //mainPage.BindingContext = user;
                //LocalSettings.CurrnetUser = user;
                //Application.Current.MainPage = new NavigationPage(mainPage);
                //// await App.NavigationPage.PushAsync(new MainPage());
            }
            else
            {
                await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "Error", "OK"));

                //MessagingCenter.Send(this, "Warning", "My actual alert content, or an object if you want");
                // await DisplayAlert("Warning", "Username or password is not correct!", "OK");
            }

			IsBusy = false;
            // App.NavigationPage.PushAsync(new CircleItem(new CircleViewModel() { ListViewModel = this }));

    
        }
    }
}

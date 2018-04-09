using System;
using MindCorners.ViewModels;
using MindCorners.DAL;
using MindCorners.Models;
using MindCorners.Helpers;
using MindCorners.Pages;
using Xamarin.Forms;
using MindCorners.CustomControls;
using Rg.Plugins.Popup.Extensions;
using MindCorners.Pages.UserControls;

namespace MindCorners
{
	public class RegisterViewModel : BaseViewModel
	{
		private UserRegister userInfo;
		public UserRegister UserInfo
		{
			get
			{
				return userInfo;
			}
			set
			{
				userInfo = value;
				OnPropertyChanged();
			}
		}

		public async void SignUp(Guid? ActivationCode){
			IsBusy = true;
			var invitation = ActivationCode;
			if (invitation.HasValue && invitation != Guid.Empty)
			{
				AccountRepository service = new AccountRepository();

				var userToRegister = new UserRegister()
				{
					Email = UserInfo.Email,
					FirstName = UserInfo.FirstName,
					LastName = UserInfo.LastName,
					Password = UserInfo.Password,
					InvitationId = invitation.Value
				};
				var result = await service.RegisterUser(userToRegister);

				if (result != null)
				{
					if (result.IsOk && result.ReturnedObject != null)
					{
						var user = new User
						{
							Id = result.ReturnedObject.Id,
							Email = UserInfo.Email,
							FirstName = UserInfo.FirstName,
							LastName = UserInfo.LastName
							//Password = TxtPassword.Text
						};

						Settings.CurrnetUser = user;

						var mainPage = new MainPage(new MainViewModel());
						Application.Current.MainPage = App.NavigationPage = new CustomNavigationPage(mainPage) { BarTextColor = Color.White };
						Application.Current.MainPage.Title = "Main Page";
						//Application.Current.MainPage = new CustomNavigationPage(mainPage) { BarTextColor = Color.White };
						// await App.NavigationPage.PushAsync(mainPage);
					}
					else
					{
						await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", result.ErrorMessage, "OK"));
					}

					//var mainPage = new MainPage();
					//mainPage.BindingContext = user;
					//LocalSettings.CurrnetUser = user;
					//Application.Current.MainPage = new NavigationPage(mainPage);
					// await App.NavigationPage.PushAsync(new MainPage());
				}
				else
				{
					await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "Error while registering", "OK"));
				}
			}
			else
			{
				await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "No Invitation info available", "OK"));
				await App.NavigationPage.PopAsync();
			}

			IsBusy = false;
		}
	}
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Code;
using MindCorners.CustomControls;
using MindCorners.DAL;
using MindCorners.Helpers;
using MindCorners.Pages.UserControls;
using MindCorners.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace MindCorners.Pages
{
    public partial class Login : ContentPage
    {
		public Login(LoginPageViewModel vm)
        {
			InitializeComponent();
			BindingContext = vm;
        }

        async void OnLoginClick(object sender, EventArgs e)
        {
			var bindingContext = (LoginPageViewModel)BindingContext;
			bindingContext.IsBusy = true;
            AccountRepository service = new AccountRepository();
            var user = await service.LoginUser(TxtEmail.Text, TxtPassword.Text);

            if (user != null)
            {
                Settings.CurrnetUser = user;
                var mainPage = new MainPage(new MainViewModel());
           		//Application.Current.MainPage = new CustomNavigationPage(mainPage) { BarTextColor = Color.White };
				Application.Current.MainPage = App.NavigationPage = new CustomNavigationPage(mainPage) { BarTextColor = Color.White };
				Application.Current.MainPage.Title = "Main Page";
                // await App.NavigationPage.PushAsync(new MainPage());
            }
            else
            {
                await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "Username or password is not correct!", "OK"));
            }
			bindingContext.IsBusy = false ;
            //translatedNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);
            //if (!string.IsNullOrWhiteSpace(translatedNumber))
            //{
            //    callButton.IsEnabled = true;
            //    callButton.Text = "Call " + translatedNumber;
            //}
            //else
            //{
            //    callButton.IsEnabled = false;
            //    callButton.Text = "Call";
            //}
        }

        private void OnFacebookLoginClick(object sender, EventArgs e)
        {
            string facebookLoginUrl = string.Format(Constants.FacebookAuthUrl, Constants.FacebookClientId,
                Constants.FacebookRedirectUrl);
            App.NavigationPage.PushAsync(new ExternalLogin(new ExternalLoginViewModel() {ExternalLoginUrl = facebookLoginUrl}));
            //throw new NotImplementedException();
        }

        private void OnGmailLoginClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnRegisterClick(object sender, EventArgs e)
        {
            App.NavigationPage.PushAsync(new AuthenticationCode());
        }
    }
}

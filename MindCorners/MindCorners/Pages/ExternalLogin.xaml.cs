using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MindCorners.CustomControls;
using MindCorners.DAL;
using MindCorners.Helpers;
using MindCorners.Models;
using MindCorners.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindCorners.Pages
{
    public partial class ExternalLogin : ContentPage
    {
        public ExternalLogin(ExternalLoginViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        private async void ExternalLogin_OnNavigated(object sender, WebNavigatedEventArgs e)
        {

            var accessToken = ExtractAccessTokenFromUrl(e.Url);
            if (!string.IsNullOrEmpty(accessToken))
            {
                await GetFacebookProfileAsync(accessToken);
            }
        }

        private string ExtractAccessTokenFromUrl(string url)
        {
            if (url.Contains("access_token=") && url.Contains("&expires_in="))
            {
                var startIndex = url.IndexOf("access_token=") + "access_token=".Length;
                var endIndex = url.IndexOf("&expires_in=");
                return url.Substring(startIndex, endIndex - startIndex);

            }

            return null;
        }

        private async Task GetFacebookProfileAsync(string accessToken)
        {
            //var requestUrl = "https://graph.facebook.com/v2.9/me/?fields=first_name,middle_name,last_name,id,email,picture.width(600)&access_token=" + accessToken;
            //var httpClient = new HttpClient();
            try
            {
                //var userJson = await httpClient.GetStringAsync(requestUrl);
                //var userData = JsonConvert.DeserializeObject<FbData>(userJson);
                //check user exists
                AccountRepository accountRepository = new AccountRepository();
                var user =await  accountRepository.LoginExternalUser("Facebook", accessToken, true);
				if (user != null && user.Id != Guid.Empty)
                {
                    Settings.CurrnetUser = user;
                    var mainPage = new MainPage(new MainViewModel());
                    Application.Current.MainPage = new CustomNavigationPage(mainPage) { BarTextColor = Color.White };

                    // await App.NavigationPage.PushAsync(new MainPage());
                }
                else
                {
                    await App.NavigationPage.PushAsync(new AuthenticationCode(user?.Email) {IsExternalLogin = true, AccessToken = accessToken});
                    // await DisplayAlert("Warning", "Username or password is not correct!", "OK");
                }
                //register user if not exist

            }
            catch (Exception e)
            {
                string s = e.ToString();
            }
        }
    }
}
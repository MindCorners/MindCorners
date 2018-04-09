using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Code;
using MindCorners.CustomControls;
using MindCorners.DAL;
using MindCorners.Helpers;
using MindCorners.Models;
using MindCorners.Pages.UserControls;
using MindCorners.ViewModels;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;

namespace MindCorners.Pages
{
    public partial class AuthenticationCode : ContentPage
    {
        private bool isExternalLogin;
        public bool IsExternalLogin
        {
            get { return isExternalLogin; }
            set { isExternalLogin = value; }
        }
        private string accessToken;
        public string AccessToken
        {
            get { return accessToken; }
            set { accessToken = value; }
        }
        public AuthenticationCode(string email = null)
        {
            InitializeComponent();
			TxtEmail.Text = email;
        }

        private async void OnValidationCodeClick(object sender, EventArgs e)
        {
            //if activation code is ok go to register page
            AccountRepository service = new AccountRepository();
            var checkInvitation = await service.CheckAuthenticationCode(TxtEmail.Text, TxtAuthenticationCode.Text);
            bool isActivationOk = false;
            Guid? activationCode = null;

            if (checkInvitation != null)
            {
                if (checkInvitation.IsOk && checkInvitation.Id.HasValue)
                {
                    // Settings.IsActivationCodeOk = true;
                    //Settings.InvitationId = checkInvitation.Id.Value;
                    isActivationOk = true;
                    activationCode = checkInvitation.Id.Value;

                }
                else
                {
                    await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", checkInvitation.ErrorMessage, "OK"));

                }

                //var mainPage = new MainPage();
                //mainPage.BindingContext = user;
                //LocalSettings.CurrnetUser = user;
                //Application.Current.MainPage = new NavigationPage(mainPage);
                // await App.NavigationPage.PushAsync(new MainPage());
            }
            else
            {
                await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "Error checking authentication code", "OK"));
            }

            if (isActivationOk)
            {
                if (!IsExternalLogin)
                {
					await App.NavigationPage.PushAsync(new Register(new RegisterViewModel(){UserInfo = new UserRegister{Email=TxtEmail.Text}})
                    {
                        ActivationCodeIsOk = isActivationOk,
                        ActivationCode = activationCode,
                        /*BindingContext = new UserRegister()
                        {
                            Email = TxtEmail.Text,
                          //  FirstName = "NNNino",
                          //  LastName = "Ccchogovadze",
                          //  Password = "Qwerty1$",
                          //  ConfirmPassword = "Qwerty1$"
                        }*/
                    });
                }
                else
                {
                    //register

                    var invitation = Settings.InvitationId;
                    if (invitation != Guid.Empty)
                    {
                        AccountRepository accountRepository = new AccountRepository();
                        var result = await accountRepository.RegisterExternalUser(new RegisterExternalBindingModel()
                        {
                            UserName = TxtEmail.Text,
                            InvitationId = invitation,
                            ExternalAccessToken = AccessToken,
                            Provider = "Facebook"
                        });

                        if (result != null)
                        {
                            if (result.IsOk && result.ReturnedObject != null)
                            {
                                var user = result.ReturnedObject;
                                Settings.CurrnetUser = user;

                                var mainPage = new MainPage(new MainViewModel());

								Application.Current.MainPage = App.NavigationPage = new CustomNavigationPage(mainPage) { BarTextColor = Color.White };
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
                    }
                }
            }
            //else
            //{
            //    DisplayAlert("Warning", "Authentication code or Email is not valid!", "OK");
            //}
        }

        private async void OnBackToLoginClick(object sender, EventArgs e)
        {
			await App.NavigationPage.PopToRootAsync();
        }
    }
}

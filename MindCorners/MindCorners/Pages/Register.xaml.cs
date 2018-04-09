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
    public partial class Register : ContentPage
    {
        private bool activationCodeIsOk;
        public bool ActivationCodeIsOk
        {
            get { return activationCodeIsOk; }
            set
            {
                activationCodeIsOk = value;
                OnPropertyChanged();
            }
        }
        private Guid? activationCode;
        public Guid? ActivationCode
        {
            get { return activationCode; }
            set
            {
                activationCode = value;
                OnPropertyChanged();
            }
        }
		private RegisterViewModel registerViewModel;
		public Register(RegisterViewModel vm)
        {
            InitializeComponent();
			BindingContext = vm;
			registerViewModel = vm;
        }

        private async void OnBackToLoginClick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            // await App.NavigationPage.PopAsync();
            // await App.NavigationPage.PopAsync();
			var mainPage = new Login(new LoginPageViewModel());
			Application.Current.MainPage = App.NavigationPage = new CustomNavigationPage(mainPage) { BarTextColor = Color.White };

        }

        private async void OnSignUpClick(object sender, EventArgs e)
        {
			registerViewModel.SignUp(ActivationCode);            
        }
    }
}

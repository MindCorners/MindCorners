using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.Annotations;
using MindCorners.Code;
using MindCorners.CustomControls;
using MindCorners.DAL;
using MindCorners.Helpers;
using MindCorners.Pages;
using Xamarin.Forms;

namespace MindCorners.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand;

        public LoginViewModel()
        { 
            LoginCommand = new Command(Login);
        }

        private string _Email = null;
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
                OnPropertyChanged();
            }
        }

        private string _Password = null;
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
                OnPropertyChanged();
            }
        }
        private ICommand _editItemCommand;
        public ICommand EditItemCommand
        {
            get
            {
                if (_editItemCommand == null)
                {
                    _editItemCommand = new Command(EditItem);
                }
                return _editItemCommand;
            }
        }

        public void EditItem()
        {

        }

        private async void Login()
        {
			IsBusy = true;
            AccountRepository service = new AccountRepository();
            var user = await service.LoginUser(Email, Password);

            if (user != null)
            {
                Settings.CurrnetUser = user;

                var mainPage = new MainPage(new MainViewModel());             
				Application.Current.MainPage = App.NavigationPage =new CustomNavigationPage(mainPage) { BarTextColor = Color.White };


                // await App.NavigationPage.PushAsync(new MainPage());
            }
            else
            {
                MessagingCenter.Send(this, "Warning", "Username or password is not correct!");
            }
			IsBusy = false;
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


    }
}

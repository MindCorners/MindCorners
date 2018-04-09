using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Code;
using MindCorners.CustomControls;
using MindCorners.Models;
using MindCorners.Models.Enums;
using MindCorners.Pages;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.UserControls
{
    public partial class MainPageCardContent : Grid
    {
        public MainPageCardContent()
        {
            InitializeComponent();
            //BindingContext = LocalSettings.CurrnetUser;
        }
        
        //private void OnViewProfileClick(object sender, EventArgs e)
        //{
        //    var user = LocalSettings.CurrnetUser;
        //    if (user != null)
        //    {
        //        App.NavigationPage.PushAsync(new Profile() { BindingContext = user });

        //        //DisplayAlert("Warning", string.Format("Welcome {0} {1}", user.FirstName, user.LastName), "OK");
        //    }
        //    else
        //    {
        //        Application.Current.MainPage = new NavigationPage(new Login());
        //    }
        //}

        //private void OnNotificationsClick(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        private void BtnHome_OnClicked(object sender, EventArgs e)
        {
            var mainPage = new MainPage(new MainViewModel());
            Application.Current.MainPage = new CustomNavigationPage(mainPage) { BarTextColor = Color.White };
        }

        private void BtnAddNewPropmt_OnClicked(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //var posts = ((MainViewModel)Application.Current.MainPage.BindingContext).LatestPosts;
          
        }

        private void BtnContacts_OnClicked(object sender, EventArgs e)
        {
            App.NavigationPage.PushAsync(new Contacts() { BindingContext = new ContactsListViewModel() });
        }
    }
}

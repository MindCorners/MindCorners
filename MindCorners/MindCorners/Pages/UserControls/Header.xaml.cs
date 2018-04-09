using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Code;
using MindCorners.CustomControls;
using MindCorners.Helpers;
using MindCorners.Pages;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.UserControls
{
    public partial class Header : Grid
    {
        public Header()
        {
            InitializeComponent();
            BindingContext = Settings.CurrnetUser;
            MainLogoImage.IsVisible = ShowLogo;
            PageTitleLabel.IsVisible = !ShowLogo;
            NotificationButton.IsVisible = ShowNotifications;
        }

        public static readonly BindableProperty PageTitleProperty = BindableProperty.Create("PageTitle", typeof(string), typeof(HeaderInnerPages), "", BindingMode.TwoWay);
        public string PageTitle
        {
            get { return (string)GetValue(PageTitleProperty); }
            set { SetValue(PageTitleProperty, value); }
        }

        public static readonly BindableProperty ShowLogoProperty = BindableProperty.Create("ShowLogo", typeof(bool), typeof(HeaderInnerPages), false, BindingMode.TwoWay);
        public bool ShowLogo
        {

            get { return (bool)GetValue(ShowLogoProperty); }

            set { SetValue(ShowLogoProperty, value); }

        }

        public static readonly BindableProperty ShowNotificationsProperty = BindableProperty.Create("ShowNotifications", typeof(bool), typeof(HeaderInnerPages), true, BindingMode.TwoWay);
        public bool ShowNotifications
        {

            get { return (bool)GetValue(ShowNotificationsProperty); }

            set { SetValue(ShowNotificationsProperty, value); }

        }


        private void OnViewProfileClick(object sender, EventArgs e)
        {
            var user = Settings.CurrnetUser;
            if (user != null)
            {
				App.NavigationPage.PushCustomAsync(new Profile(new ProfileViewModel()) , MindCorners.Models.Enums.TransitionTypes.LeftToRight);

                //DisplayAlert("Warning", string.Format("Welcome {0} {1}", user.FirstName, user.LastName), "OK");
            }
            else
            {
				Application.Current.MainPage = new CustomNavigationPage(new Login(new LoginPageViewModel())) { BarTextColor = Color.White };
            }
        }

        private void OnNotificationsClick(object sender, EventArgs e)
        {
            App.NavigationPage.PushAsync(new NotificationsList(new NotificationsListViewModel()));
            //throw new NotImplementedException();
        }

        private async void OnSearchClick(object sender, EventArgs e)
        {
            await App.NavigationPage.PushAsync(new PromptsList(new PromptsListViewModel() { ArchiveResultText = "Search Results", ShowSearchBar = true }));
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == PageTitleProperty.PropertyName)
            {
                PageTitleLabel.Text = PageTitle;
            }
            if (propertyName == ShowLogoProperty.PropertyName)
            {
                MainLogoImage.IsVisible = ShowLogo;
                PageTitleLabel.IsVisible = !ShowLogo;
            }
            if (propertyName == ShowNotificationsProperty.PropertyName)
            {
                NotificationButton.IsVisible = ShowNotifications;
            }
        }
    }
}

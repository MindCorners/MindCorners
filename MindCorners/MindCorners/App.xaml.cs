using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MindCorners.Code;
using MindCorners.Helpers;
using MindCorners.ViewModels;
using Xamarin.Forms;
using System.Threading.Tasks;
using MindCorners.CustomControls;

namespace MindCorners
{
	public partial class App : Application
	{
		public static CustomNavigationPage NavigationPage;
		public App()
		{
			//MainPage = new NavigationPage(new Pages.Login());
			InitializeComponent();

			if (Settings.CurrnetUser != null && Settings.CurrnetUser.Id != Guid.Empty)
			{
				var mainPage = new MindCorners.Pages.MainPage(new MainViewModel());
				//NavigationPage = new CustomNavigationPage(mainPage) { BarTextColor = Color.White };
				MainPage = NavigationPage = new CustomNavigationPage(mainPage) { BarTextColor = Color.White };
				MainPage.Title = "Main Page";

				//run Notifications checker timer
				Device.StartTimer(TimeSpan.FromSeconds(10), () =>
					{
						Task.Run(UpdateNotifications);
						return true;
					});
			}
			else
			{
				MainPage = NavigationPage  = new CustomNavigationPage(new MindCorners.Pages.Login(new LoginPageViewModel())) { BarTextColor = Color.White };
				MainPage.Title = "Login";
			}

			// MainPage = new MindCorners.Pages.Login();
		}

		private async Task UpdateNotifications()
		{
			var notificationsCount = await Global.LoadNewNotifications();
			var pages = Current.MainPage.Navigation.NavigationStack;
			var mainPage = pages.FirstOrDefault(p => p.ClassId == "MindCorners.Pages.MainPage");
			if (mainPage != null)
			{
				var context = (MainViewModel)mainPage.BindingContext;
				context.NumberOfNewNotifications = notificationsCount;
			}
			var contactsPage = pages.FirstOrDefault(p => p.ClassId == "MindCorners.Pages.Contacts");
			if (contactsPage != null)
			{
				var context = (ContactsListViewModel)contactsPage.BindingContext;
				context.NumberOfNewNotifications = notificationsCount;
			}
			// var time = await RequestTimeAsync();
			// do something with time...
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

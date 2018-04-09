using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.CustomControls;
using MindCorners.Models;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.Pages
{
    public partial class Contacts : ContentPage
    {
       
        public Contacts()
        {
            InitializeComponent();
           // BindingContext = new ContactsListViewModel(Navigation);
        }
		private void OnBindingContextChanged(object sender, EventArgs e)
		{
			base.OnBindingContextChanged();

			if (BindingContext == null)
				return;

			var vm = (ContactsListViewModel)BindingContext;

			ViewCell theViewCell = ((ViewCell)sender);
			var item = theViewCell.BindingContext as Circle;
			theViewCell.ContextActions.Clear();

			if (item != null)
			{
				if (item.IsCreatedByUser) {
					theViewCell.ContextActions.Add (new MenuItem () {
							
						Command =vm.DeleteCircleCommand,
						CommandParameter = item.Id,
						Text = "Delete", 
						IsDestructive =true,
							
					});
				} else {
					theViewCell.ContextActions.Add (new MenuItem () {

						Command = vm.LeaveCircleCommand,
							CommandParameter = item.Id,
						Text = "Leave", 
						IsDestructive =true,

					});
				}
			}
		}
        //async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        //{
        //    var item = args.SelectedItem as Contact;
        //    if (item == null)
        //    {
        //        // the item was deselected
        //        return;
        //    }
            
        //    await App.NavigationPage.PushAsync(new ContactItem(new ContactViewModel(item)));
        //    // Manually deselect item
        //    ListViewContacts.SelectedItem = null;
        //}

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();

        //    if (viewModel.Items.Count == 0)
        //        viewModel.LoadItemsCommand.Execute(null);
        //}
        private void BtnCreateCircle_OnClicked(object sender, EventArgs e)
        {
            
        }

        private void BtnInviteContact_OnClicked(object sender, EventArgs e)
        {
            
        }
        

        private void BtnCircleEdit_OnClicked(object sender, EventArgs e)
        {
            
        }

        private void BtnCircleDelete_OnClicked(object sender, EventArgs e)
        {
            
        }

        private void BtnCircleLeave_OnClicked(object sender, EventArgs e)
        {
            
        }

        private void BtnHome_OnClicked(object sender, EventArgs e)
        {
			//RefreshLatestPosts();
			App.NavigationPage.PopAsync();
            //var mainPage = new MainPage(new MainViewModel());
			//Application.Current.MainPage = App.NavigationPage = new CustomNavigationPage(mainPage) { BarTextColor = Color.White };
        }

        private void BtnContacts_OnClicked(object sender, EventArgs e)
        {
            
        }

        private void BtnAddNewPropmt_OnClicked(object sender, EventArgs e)
        {
           
        }

        private void OnNotificationsClick(object sender, EventArgs e)
        {
           
        }

		private async void RefreshLatestPosts()
		{
			var pages = Navigation.NavigationStack;
			var mainPage = pages.FirstOrDefault(p => p.ClassId == "MindCorners.Pages.MainPage");
			if (mainPage != null)
			{
				var context = (MainViewModel)mainPage.BindingContext;
				context.LoadPosts();
			}
		}
    }
}

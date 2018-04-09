using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.Annotations;
using MindCorners.Code;
using MindCorners.Helpers;
using MindCorners.Models;
using MindCorners.Pages;
using Xamarin.Forms;

namespace MindCorners.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        private int numberOfNewNotifications;
        public int NumberOfNewNotifications
        {
            get { return numberOfNewNotifications; }
            set { numberOfNewNotifications = value; OnPropertyChanged(); }
        }

        public async Task LoadNewNotifications()
        {
            NumberOfNewNotifications = await Global.LoadNewNotifications();
        }

        public ICommand ShowSearchBarButtonClickedCommand { protected set; get; }
        public BaseViewModel()
        {
            //Navigation = navigation;
            CurrentUser = Settings.CurrnetUser;
            ShowSearchBarButtonClickedCommand = new Command(ShowSearchBarButtonClicked);
        }

        public INavigation Navigation
        {
            get { return App.Current.MainPage.Navigation; }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        


        User currentUser;
        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                currentUser = value;
                OnPropertyChanged();
            }
        }


        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// Private backing field to hold the title
        /// </summary>
        string title = string.Empty;
        /// <summary>
        /// Public property to set and get the title of the item
        /// </summary>
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }
        protected void UpdateListSelection<T>(ObservableCollection<T> list, Guid? id = null, bool isSelected = false) where T : BaseSelectableModel
        {
            foreach (var listItem in list)
            {
                var isNotIdPassed = !id.HasValue;
                var isNotSameItem = id.HasValue && listItem.Id.ToString() != id.ToString();
                if (isNotIdPassed || isNotSameItem)
                {
                    listItem.IsSelected = isSelected;
                }
            }
        }

        public virtual async void ShowSearchBarButtonClicked()
        {
            var pages = Navigation.NavigationStack;
            var activePage = pages.Last(p => p.ClassId != "MindCorners.Pages.PromptsList");
            if (activePage != null)
            {
                await App.NavigationPage.PushAsync(new PromptsList(new PromptsListViewModel() { ArchiveResultText = "History", ShowSearchBar = true}));
            }

        }
        
    }
}

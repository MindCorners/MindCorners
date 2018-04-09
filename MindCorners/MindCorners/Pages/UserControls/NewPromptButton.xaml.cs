using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Models;
using MindCorners.Models.Enums;
using MindCorners.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindCorners.Pages.UserControls
{
    public partial class NewPromptButton : Grid
    {
        public NewPromptButton()
        {
            InitializeComponent();
        }

        private void BtnAddNewPropmt_OnClicked(object sender, EventArgs e)
        {
            //ObservableCollection<Post> posts = ((MainViewModel)BindingContext).LatestPosts;
            App.NavigationPage.PushAsync(new PromptItem(new PromptItemViewModel() { EditingItem = new Post() { Type = (int)PostTypes.Prompt } }));
        }
    }
}
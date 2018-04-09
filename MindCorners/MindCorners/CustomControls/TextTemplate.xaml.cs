using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Models;
using MindCorners.Models.Enums;
using MindCorners.Pages.PromptTemplates;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.CustomControls.ChatItemTemplates
{
    public partial class TextTemplate : ViewCell
    {
        public TextTemplate()
        {
            InitializeComponent();
        }
        private async void TellMeMoreButtonClick(object sender, EventArgs e)
        {
            var vm = ((Post)BindingContext);
            if (vm != null)
            {
                var post = new Post() { Type = (int)PostTypes.TellMeMore, ParentId = vm.Id, CircleId = vm.CircleId };
                var pageToOpen = new TextChatItem(new TextChatItemViewModel() { ParentPost = post, EditingItem = new PostAttachment() { Type = (int)ChatType.Text }, CanSend = false });
                await App.NavigationPage.PushCustomAsync(pageToOpen);
            }
        }
    }
}

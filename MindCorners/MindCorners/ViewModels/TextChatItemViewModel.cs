using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.CustomControls;
using MindCorners.Pages;
using Xamarin.Forms;

namespace MindCorners.ViewModels
{
    public class TextChatItemViewModel : ChatItemAttachmentViewModel
    {   
        public ICommand ChooseFromTemplatesCommand { get; set; }

		public CustomEditor EditorChatItemText { get; set; }
     
        public TextChatItemViewModel()
        {
            ChooseFromTemplatesCommand = new Command(OpenTextTemplatePage);
        }

        private async void OpenTextTemplatePage()
        {
            await App.NavigationPage.PushAsync(new TextTemplatesList(new TextTemplateItemViewModel() {EditingItem = EditingItem, TextChatItemViewModel = this}));
        }

        private string chatItemText;
        public string ChatItemText
        {
            get { return chatItemText; }
            set
            {
                chatItemText = value; 
                OnPropertyChanged();
                CanSend = !string.IsNullOrEmpty(chatItemText);
                EditingItem.Text = value;
            }
        }

    }
}

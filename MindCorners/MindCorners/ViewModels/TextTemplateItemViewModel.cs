using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.CustomControls;
using MindCorners.DAL;
using MindCorners.Models;
using Xamarin.Forms;

namespace MindCorners.ViewModels
{
    public class TextTemplateItemViewModel : ChatItemAttachmentViewModel
    {
        private TextChatItemViewModel textChatItemViewModel;
        public TextChatItemViewModel TextChatItemViewModel
        {
            get { return textChatItemViewModel; }
            set
            {
                textChatItemViewModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectTemplateCommand { get; set; }

        public TextTemplateItemViewModel()
        {
            SelectTemplateCommand = new Command(ChooseSelectedTemplate);
            Task.Run(LoadTextTempates).Wait();

        }
        private ObservableCollection<TextTemplate> templates;
        public ObservableCollection<TextTemplate> Templates
        {
            get { return templates; }
            set
            {
                templates = value;
                OnPropertyChanged();
            }
        }

        private TextTemplate selectedItem;
        public TextTemplate SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (value != null)
                {
                    value.IsSelected = !value.IsSelected;
                    selectedItem = null;
                    //UpdateListSelection(Templates, value.Id);
                    foreach (var textTemplate in Templates)
                    {
                        if (textTemplate.Id.ToString() != value.Id.ToString())
                        {
                            textTemplate.IsSelected = false;
                        }
                    }
                    // UpdateListSelection(Templates, value.Id);
                }
                OnPropertyChanged();
            }
        }
		public delegate void MyEventHandler(object sender, TextChangedEventArgs e);

		//public event TextChangedEventArgs MyEvent;

        private async void ChooseSelectedTemplate()
        {
            var selected = Templates.FirstOrDefault(p => p.IsSelected);
			EditingItem.Text = selected.Text;
			//TextChatItemViewModel.EditorChatItemText.TextChanged+=SubGraphButton_Click;
			TextChatItemViewModel.ChatItemText = selected.Text;
            
			//TextChatItemViewModel.EditorChatItemText.ClearValue(CustomEditor.TextProperty);
            await App.NavigationPage.PopAsync();
        }
        private async Task LoadTextTempates()
        {
            TextTemplateRepository textTemplateRepository = new TextTemplateRepository();
            Templates = new ObservableCollection<TextTemplate>(await textTemplateRepository.GetAll());
        }
    }
}

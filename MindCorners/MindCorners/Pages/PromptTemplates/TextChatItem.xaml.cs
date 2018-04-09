using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.CustomControls;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.Pages.PromptTemplates
{
    public partial class TextChatItem : ContentPage
    {
		public void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			((CustomEditor)sender).TextChangeAction();
		}

		public TextChatItem(TextChatItemViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
			vm.EditorChatItemText = EditorChatItemText;
        }
    }
}

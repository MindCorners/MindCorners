using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.ViewModels;
using Xamarin.Forms;

namespace MindCorners.Pages.PromptTemplates
{
    public partial class AudioChatItem : ContentPage
    {
        public AudioChatItem(AudioChatItemViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}

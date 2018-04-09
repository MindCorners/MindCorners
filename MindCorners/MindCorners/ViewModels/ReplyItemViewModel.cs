using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.DAL;
using MindCorners.Models;
using MindCorners.Models.Enums;
using MindCorners.Pages;
using MindCorners.Pages.PromptTemplates;
using MindCorners.Pages.UserControls;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
namespace MindCorners.ViewModels
{
    public class ReplyItemViewModel : ChatItemBaseViewModel
    {  
        protected async override Task<Post> CreateNewPost()
        {   
            PostRepository postRepository = new PostRepository();
            var editingItem = EditingItem;
            
            // editingItem.Type = (int)PostTypes.Prompt;
            var result = await postRepository.Submit(editingItem);

            if (result != null)
            {
                if (result.IsOk && result.Id.HasValue)
                {
                   // await Application.Current.MainPage.DisplayAlert("Success", "Prompt was saved", "OK");
                    editingItem.Id = result.Id.Value;
                    EditingItem = editingItem;
                }
                else
                {
                    await Navigation.PushPopupAsync(new CustomAlertDialog("Error", result.ErrorMessage, "OK"));
                }
            }
            else
            {
                await Navigation.PushPopupAsync(new CustomAlertDialog("Warning", "Error", "OK"));
            }

            return editingItem;
        }
    }
}

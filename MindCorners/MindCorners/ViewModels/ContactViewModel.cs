using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.DAL;
using MindCorners.Models;
using Xamarin.Forms;

namespace MindCorners.ViewModels
{
    public class ContactViewModel : BaseViewModel
    {
        public ICommand AcceptContactCommand { protected set; get; }
        public ICommand RejectContactCommand { protected set; get; }
        //public ObservableCollection<ContactViewModel> Contacts { get; set; }
        public ContactViewModel()
        {
            AcceptContactCommand = new Command(AcceptContact);
            RejectContactCommand = new Command(RejectContact);
        }

        private Contact viewItem;
        public Contact ViewItem
        {
            get { return viewItem; }
            set
            {
                viewItem = value;
                if (viewItem != null)
                {
                    Task.Run(LoadContact).Wait();
                }
            }
        }
        
        private void AcceptContact()
        {
            ViewItem.State = 1;
            UpdateState();
        }
        private async Task LoadContact()
        {
            ContactRepository contactRepository = new ContactRepository();
            var viewItemstate = await contactRepository.GetByIdWithState(ViewItem.Id);
            if(viewItemstate != null)
            {
                ViewItem.State = viewItemstate.State;
            }
        }
        private void RejectContact()
        {
            ViewItem.State = 2;
            UpdateState();
        }

        private async void UpdateState()
        {
            InvitationRepository  invitationRepository  = new InvitationRepository();
            await invitationRepository.ChangeInvitationState(ViewItem);

        }

        //private string firstName;
        //public string ContactFirstName
        //{
        //    get { return firstName; }
        //    set { firstName = value; OnPropertyChanged(); }
        //}
        //private string lastName;
        //public string ContactLastName
        //{
        //    get { return lastName; }
        //    set { lastName = value; OnPropertyChanged(); }
        //}
        //private string email;
        //public string ContactEmail
        //{
        //    get { return email; }
        //    set { email = value; OnPropertyChanged(); }
        //}

        //private Guid id;
        //public Guid ContactId
        //{
        //    get { return id; }
        //    set { id = value; OnPropertyChanged(); }
        //}

        //private Guid userContactId;
        //public Guid UserContactId
        //{
        //    get { return userContactId; }
        //    set { userContactId = value; OnPropertyChanged(); }
        //}

        //public string ContactFullName
        //{
        //    get { return $"{ContactFirstName} {ContactLastName}"; }
        //}

        //private bool isSelected;
        //public bool IsSelected
        //{
        //    get { return isSelected; }
        //    set
        //    {
        //        if (isSelected != value)
        //        {
        //            isSelected = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private bool isActivated;
        //public bool IsActivated
        //{
        //    get { return isActivated; }
        //    set
        //    {
        //        if (isActivated != value)
        //        {
        //            isActivated = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private string profileImageString;
        //public string ProfileImageString
        //{
        //    get { return profileImageString; }
        //    set
        //    {
        //        if (profileImageString != value)
        //        {
        //            profileImageString = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
    }
}

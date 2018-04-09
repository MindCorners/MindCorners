using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.Models
{
	public class User: BaseModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
       // public byte[] ProfileImage { get; set; }
        //public string ProfileImageString { get; set; }
		private string profileImageString;
		public string ProfileImageString
		{
			get { return profileImageString; }
			set
			{
				profileImageString = value;
				OnPropertyChanged();
			}
		}
    }

    public class UserRegister : User
    {   
        public string ConfirmPassword { get; set; }
        public Guid InvitationId { get; set; }
    }

    public class UserProfileImage
    {
        public byte[] Image { get; set; }
        public string ImageString { get; set; }
        public Guid Id { get; set; }
    }
}

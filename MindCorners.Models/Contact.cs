using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.Models
{
    public class Contact : BaseSelectableModel
    {
      
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string FullName {
            get { return $"{FirstName} {LastName}"; }
        }
        public string ProfileImageString { get; set; }
        public bool IsActivated { get; set; }

		private byte? state;
		public byte? State
		{
			get { return state; }
			set
			{
				state = value;
				OnPropertyChanged();
			}
		}
		private bool isPending;
		public bool IsPending
		{
			get { return isPending; }
			set
			{
				isPending = value;
				OnPropertyChanged();
			}
		}
		    }
}

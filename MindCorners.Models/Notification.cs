using System;

using Xamarin.Forms;

namespace MindCorners.Models
{
	public class Notification : BaseModel
	{
		public Guid Id { get; set; }
		public int? Type { get; set; }
		public Guid? SourceId { get; set; }
		public string Text { get; set; }
		public Guid SenderId { get; set; }
		public string SenderFullName { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime RecieveDate { get; set; }
		public DateTime? ReadDate { get; set; }
		public string UserCreatorFullName { get; set; }

		private string userProfileImageName;
		public string UserProfileImageName
		{
			get { return userProfileImageName; }
			set
			{
				userProfileImageName = value;
				OnPropertyChanged();
			}
		}
		public bool CanSendTellMeMore;
	}
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MindCorners.Models.Enums;
using Xamarin.Forms;

namespace MindCorners.Models
{
    public class Post : BaseModel
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string MainPostTitle { get; set; }
        public string Title { get; set; }
        public int? Type { get; set; }
        public string TypeName { get; set; }

        public Guid? UserId { get; set; }
        public Guid? CircleId { get; set; }
        public string CircleName { get; set; }
        public Guid CreatorId { get; set; }
        public string CreatorFullName { get; set; }
        public Guid? LastUpdatedUserId { get; set; }
        public string LastUpdatedUserFullName { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string LastUpdatedDateValue
        {
            get
            {
                if (LastUpdatedDate.HasValue)
                {
                     return DateToString(LastUpdatedDate.Value); 
                }
                return null;
            }
        }

        public string DateCreatedValue
        {
            get { return DateToString(DateCreated); }
        }

        private string DateToString(DateTime datetime)
        {
			var lastUpdateDate = datetime.ToLocalTime();
            var timeAfterNow = DateTime.Now - lastUpdateDate;
            if (timeAfterNow.TotalDays >= 1)
            {
                if (timeAfterNow.TotalDays > 1)
                {
                    return lastUpdateDate.ToString("dd/MM/yyyy");
                }
                return "Yesterday";
            }
            if (timeAfterNow.TotalHours >= 1)
            {
                return string.Format("{0} hour{1} ago", Math.Round(timeAfterNow.TotalHours), timeAfterNow.TotalHours > 1 ? "s" : "");
            }
            if (timeAfterNow.TotalMinutes > 1)
            {
                return string.Format("{0} minute{1}  ago", Math.Round(timeAfterNow.TotalMinutes), timeAfterNow.TotalMinutes > 1 ? "s" : "");
            }
            return "Just now";
        }

        public int? LastAttachmentType { get; set; }
        public DateTime DateCreated { get; set; }
        public bool HasReplies { get; set; }
        public Guid? LastAttachmentId { get; set; }

        public Contact SelectedContact { get; set; }

        public Circle SelectedCircle { get; set; }

        private PostAttachment mainAttachment;
        public PostAttachment MainAttachment
        {
            get { return mainAttachment; }
            set
            {
                mainAttachment = value;
                OnPropertyChanged();
            }
        }

        public string LastAttachmentTypeImage
        {
            get
            {
                if (LastAttachmentType.HasValue)
                {
                    switch (LastAttachmentType.Value)
                    {
                        case (int)ChatType.Text:
                            return "text.png";
                        case (int)ChatType.Image:
                            return "picture.png";
                        case (int)ChatType.Audio:
                            return "audio.png";
                        case (int)ChatType.Video:
                            return "video.png";
                    }
                }
                return null;

            }
        }

        public string LastAttachmentTypeName
        {
            get
            {
                if (LastAttachmentType.HasValue)
                {
                    switch (LastAttachmentType.Value)
                    {
                        case (int)ChatType.Text:
                            return "Text Message";
                        case (int)ChatType.Image:
                            return "Image Message";
                        case (int)ChatType.Audio:
                            return "Audio Message";
                        case (int)ChatType.Video:
                            return "Video Message";
                    }
                }
                return null;

            }
        }
        public string FormatedFromText
        {
            get
            {
                //var htmlContents = "<span style=\"color: #95BB6B;\">" + Element.Text + "</span>";
                //return string.Format("<span style=\"color: #95BB6B;\"><span color='#697780'>From </span><b>{0} </b><span color='#697780'>for </span><b>{1}</b></span>", CreatorFullName, CircleName);
                return string.Format("From {0} for {1}", CreatorFullName, CircleName);

            }
        }

        public bool CanShowFormattedString = false;

        public FormattedString CustomFormattedTextForCardView
        {
            get
            {
                if(!CanShowFormattedString)
                    return null;
                return new FormattedString
                {
                    Spans = {
						new Span { Text = "From ", ForegroundColor = HasReplies ? Color.FromHex("#B5F1B9"): Color.FromHex("#C6DCFF")},
						new Span { Text = CreatorFullName, ForegroundColor = Color.FromHex("#FFFFFF")},
						new Span { Text = " for ", ForegroundColor = HasReplies ?  Color.FromHex("#B5F1B9"): Color.FromHex("#C6DCFF")},
						new Span { Text = CircleName, ForegroundColor = Color.FromHex("#FFFFFF")}}
                };
            }
            //set { }
            //get
            //{
            //    //var htmlContents = "<span style=\"color: #95BB6B;\">" + Element.Text + "</span>";
            //    //return string.Format("<span style=\"color: #95BB6B;\"><span color='#697780'>From </span><b>{0} </b><span color='#697780'>for </span><b>{1}</b></span>", CreatorFullName, CircleName);
            //    //return string.Format("From {0} for {1}", CreatorFullName, CircleName);

            //}
        }

        public FormattedString CustomFormattedTextForChatItem
        {
            get
            {
                if (!CanShowFormattedString)
                    return null;
                return new FormattedString
                {
                    Spans = {
                    new Span { Text = "From "},
                    new Span { Text = CreatorFullName},
                    new Span { Text = " for "},
                    new Span { Text = CircleName}}
                };
            }
            //set { }
            //get
            //{
            //    //var htmlContents = "<span style=\"color: #95BB6B;\">" + Element.Text + "</span>";
            //    //return string.Format("<span style=\"color: #95BB6B;\"><span color='#697780'>From </span><b>{0} </b><span color='#697780'>for </span><b>{1}</b></span>", CreatorFullName, CircleName);
            //    //return string.Format("From {0} for {1}", CreatorFullName, CircleName);

            //}
        }
        private List<PostAttachment> attachments;

        public List<PostAttachment> Attachments
        {
            get { return attachments; }
            set
            {
                attachments = value;
                OnPropertyChanged();
            }
        }

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
		private bool canSendTellMeMore;
		public bool CanSendTellMeMore
		{
			get { return canSendTellMeMore; }
			set
			{
				canSendTellMeMore = value;
				OnPropertyChanged();
			}
		}
    }
}

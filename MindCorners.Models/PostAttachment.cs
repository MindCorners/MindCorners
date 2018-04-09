using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.Models
{
    public class PostAttachment :BaseModel
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
       // public bool IsMainAttachment { get; set; }


        private bool isMainAttachment;
        public bool IsMainAttachment
        {
            get { return isMainAttachment; }
            set
            {
                isMainAttachment = value;
                OnPropertyChanged();
            }
        }

        public int? Type { get; set; }
        public string TypeName { get; set; }

        //public string Text { get; set; }
        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                text = value; 
                OnPropertyChanged();
            }
        }

        // public string FilePath { get; set; }
        private string filePath;
        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
                OnPropertyChanged();
            }
        }

        private string fileUrl;
        public string FileUrl
        {
            get { return fileUrl; }
            set
            {
                fileUrl = value;
                OnPropertyChanged();
            }
        }

        private string fileThumbnailUrl;
        public string FileThumbnailUrl
        {
            get { return fileThumbnailUrl; }
            set
            {
                fileThumbnailUrl = value;
                OnPropertyChanged();
            }
        }
        private double? fileDuration;
        public double? FileDuration
        {
            get { return fileDuration; }
            set
            {
                fileDuration = value;
                OnPropertyChanged();
            }
        }
    }

	public class PostAndPostAttachment
	{
		public Post Post { get; set; }   
		public bool IsFile { get; set; }
		public PostAttachment PostAttachment { get; set; }
		public AttachmentFile AttachmentFile { get; set; }
	}
}

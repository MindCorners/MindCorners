using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.Models
{
    public class AttachmentFile : BaseModel
    {
        private byte[] fileData;
        public byte[] FileData
        {
            get { return fileData; }
            set
            {
                fileData = value; 
                OnPropertyChanged();
            }
        }

        // public string FilePath { get; set; }
        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged();
            }
        }

        private string fileExtention;
        public string FileExtention
        {
            get { return fileExtention; }
            set
            {
                fileExtention = value;
                OnPropertyChanged();
            }
        }

        private string fileContentType;
        public string FileContentType
        {
            get { return fileContentType; }
            set
            {
                fileContentType = value;
                OnPropertyChanged();
            }
        }

        private int attachmentType;
        public int AttachmentType
        {
            get { return attachmentType; }
            set { attachmentType = value; }
        }

        private byte[] thumbnailFileData;
        public byte[] ThumbnailFileData
        {
            get { return thumbnailFileData; }
            set
            {
                thumbnailFileData = value;
                OnPropertyChanged();
            }
        }

    }
}

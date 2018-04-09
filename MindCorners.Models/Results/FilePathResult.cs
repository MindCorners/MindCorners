using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.Models.Results
{
    public class FilePathResult : IdResult
    {

		public Guid? PostId { get; set; }
        public string FileName { get; set; }
        public string ThumbnailUrl { get; set;}
        public string FileUrl { get; set; }
    }
}
 
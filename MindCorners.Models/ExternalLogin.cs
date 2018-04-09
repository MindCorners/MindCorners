using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.Models
{

    public class RegisterExternalBindingModel
    {
        public string UserName { get; set; }
        public string Provider { get; set; }
        public string ExternalAccessToken { get; set; }
        public Guid? InvitationId { get; set; }
    }


    public class FbData
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string middle_name { get; set; }
        public string id { get; set; }
        public string email { get; set; }
        public FBPicture picture { get; set; }
    }

    public class FBPicture
    {
        public FBPictureDate data { get; set; }
    }

    public class FBPictureDate
    {
        public int height { get; set; }
        public bool is_silhouette { get; set; }
        public string url { get; set; }
        public int width { get; set; }
    }
}

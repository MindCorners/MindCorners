using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.Code
{
    public class Constants
    {
        //public const string RestURL = "http://192.168.0.95:50000";
          public const string RestURL = "http://mindcornersweb.azurewebsites.net/rest/";
        //public const string RestURL = "http://5.152.68.48:3344";

        public const string FacebookAuthUrl =
            "https://www.facebook.com/v2.9/dialog/oauth?client_id={0}&redirect_uri={1}&response_type=token";

        public const string FacebookClientId = "286678745123277";
// public const string FacebookRedirectUrl = "https://www.facebook.com/connect/login_success.html";
public const string FacebookRedirectUrl = "http://mindcornersweb.azurewebsites.net/SuccessExternalLogin.html";
        //public const string LoginUrl =
    }
}

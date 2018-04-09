using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using MindCorners.Authentication;
using MindCorners.Common.Code.Enums;
using MindCorners.Common.Code.Helpers;
using MindCorners.Common.Model;
using MindCorners.Models;
using MindCorners.Models.Results;
using Newtonsoft.Json;
using System.Transactions;
using System.Web.UI.WebControls;
using MindCorners.Models.Enums;
using MindCorners.RestfullService.Code;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
using MindCorners.Common.Code;

namespace MindCorners.RestfullService.Controllers
{
    public class FilesController
    {
        public FilesController()
        {
            //_invitationRepository = new InvitationRepository(Context, DbUser, null);
        }
    }
}

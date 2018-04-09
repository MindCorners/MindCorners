using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MindCorners.Authentication;
using MindCorners.Common.Code;
using MindCorners.Common.Code.Enums;
using MindCorners.Common.Model;
using MindCorners.Web.Code.BaseController;

namespace MindCorners.Web.Controllers
{
    //[AllowAnonymous]
    public class DefaultController : ListControllerCustomBinding<ListFilter, Common.Model.UserProfile, Users_GetAll_Result, UserProfileRepository>
    {
        #region Constructors
        public DefaultController()
            : base((int)ModuleName.User)
        {
            //context = new ApplicationDbContext();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            //_applicationRepository = new UserProfileRepository(Context, CurrentUserId, CurrentUserOrganizationID);
            //_invitationRepository = new InvitationRepository(Context, CurrentUserId, CurrentUserOrganizationID);
            //_organizationRepository = new OrganizationRepository(Context, CurrentUserId, CurrentUserOrganizationID);
        }

        #endregion
        
    }
}
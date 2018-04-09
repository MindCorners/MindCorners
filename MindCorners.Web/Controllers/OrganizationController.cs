using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MindCorners.Authentication;
using MindCorners.Web.Code.BaseController;
using MindCorners.Common.Code;
using MindCorners.Common.Code.Enums;
using MindCorners.Common.Model;

namespace MindCorners.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrganizationController : ListControllerCustomBinding<ListFilter,  Organization, Organizations_GetAll_Result, OrganizationRepository>
    {
        #region Constructors
        public OrganizationController()
            : base((int)ModuleName.Organization)
        {

        }
        #endregion
        
    }
}
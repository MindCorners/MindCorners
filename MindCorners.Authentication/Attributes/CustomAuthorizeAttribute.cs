using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MindCorners.Authentication.Attributes
{
    public class CustomAuthorizeAttribute: AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //if (filterContext.HttpContext.Request.IsAjaxRequest() && !filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
            //{
            //    ASPxWebControl.RedirectOnCallback("/Account/Logon");

            //    //filterContext.Result = new JavaScriptResult { Script = "onauthorizationfailure('" + ResourceErrors.SessionHasExpired + "')" };

            //    //filterContext.HttpContext.Response.StatusCode = 403;
            //    //filterContext.Result = new JsonResult
            //    //{
            //    //    Data = new
            //    //    {
            //    //        Error = "NotAuthorized",
            //    //        LogOnUrl = "/Account/Logon" //urlHelper.Action("LogOn", "Account")
            //    //    },
            //    //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //    //};



            //    //filterContext.Result = new JsonResult
            //    //{
            //    //    Data = new
            //    //    {
            //    //        Error = "NotAuthorized",
            //    //        LogOnUrl = "/Account/Logon"//urlHelper.Action("LogOn", "Account")
            //    //    },
            //    //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //    //};
            //}
            //else
                base.HandleUnauthorizedRequest(filterContext);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) && filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(CustomAuthorizeAttribute), true))
            {
                base.OnAuthorization(filterContext);
            }
        }
    }


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AllowAnonymousAttribute : Attribute
    {

    }
}

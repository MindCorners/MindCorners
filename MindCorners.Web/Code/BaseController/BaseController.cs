using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using MindCorners.Authentication;
using MindCorners.Common.Code.Interfaces;
using MindCorners.Common.Model;
using UserProfile = MindCorners.Common.Model.UserProfile;

namespace MindCorners.Web.Code.BaseController
{
    [Authorize]
    public class BaseController : Controller
    {
        #region Properties

        public IPrincipal User
        {
            get
            {
                return HttpContext == null ? null : HttpContext.User;
            }
        }

        public HttpContextBase HttpContext
        {
            get
            {
                return ControllerContext == null ? null : ControllerContext.HttpContext;
            }
        }

        protected readonly MindCornersEntities Context;

        protected readonly JavaScriptSerializer Serializer;

        public string CurrentUserFullName
        {
            get { return CurrentUser.UserProfile.FullName; }
        }

        public Guid CurrentUserId
        {
            get { return CurrentUser.UserProfile.Id; }
        }

        public ApplicationUser CurrentUser
        {
            get
            {
                var manager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(new ApplicationDbContext()));
                return manager.FindById(User.Identity.GetUserId());
            }
        }
        public UserProfile CurrentUserProfile
        {
            get
            {
                return new UserProfile
                {
                    FirstName = CurrentUser.UserProfile.FirstName,
                    LastName = CurrentUser.UserProfile.LastName,
                    Id = CurrentUser.UserProfile.Id,
                    OrganizationId = CurrentUser.UserProfile.OrganizationId
                };
            }
        }
        public Guid? CurrentUserOrganizationID
        {
            get
            {
                var user = CurrentUser;
                return user?.UserProfile.OrganizationId;
            }
        }

        public string GlobalAlertMessage
        {
            get { return (string)TempData["GlobalAlertMessage"]; }
            set { TempData["GlobalAlertMessage"] = value; }
        }

        #endregion

        #region Constructor

        public BaseController()
        {
            Context = new MindCornersEntities();
            Serializer = new JavaScriptSerializer();
            //Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.PercentDecimalSeparator = ".";
            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.FullDateTimePattern = "dd/MM/yyyy HH:mm:ss";
            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator = "/";
        }
        public BaseController(MindCornersEntities context)
        {
            Context = context;
            Serializer = new JavaScriptSerializer();
            //Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.PercentDecimalSeparator = ".";
            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.FullDateTimePattern = "dd/MM/yyyy HH:mm:ss";
            //Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator = "/";
        }
        #endregion

        public void UpdateMultipleModels<TModel>(List<TModel> models, string prefix, FormCollection collection) where TModel : class, ISynchronizableEntity, new()
        {
            var indices = collection.GetValues(prefix + ".Index") ?? new string[0];
            foreach (var index in indices)
            {
                var modelId = Guid.Parse(collection[prefix + "[" + index + "].Id"]);
                if (modelId != Guid.Empty)
                {
                    var model = models.FirstOrDefault(x => x.Id == modelId);
                    if (model != null)
                    {
                        UpdateModel(model, prefix + "[" + index + "]");
                    }
                }
            }
        }

        public string SerializeControl(string controlPath, object model)
        {
            ViewData.Model = model;
            return SerializeControl(controlPath, ViewData);
        }

        public string SerializeControl(string controlPath, ViewDataDictionary viewData)
        {
            var control = new RazorView(ControllerContext, controlPath, null, false, null);
            using (var stringWriter = new StringWriter())
            using (var writer = new HtmlTextWriter(stringWriter))
            {
                control.Render(new ViewContext(ControllerContext, control, viewData, TempData, writer), writer);
                string value = writer.InnerWriter.ToString();
                return value;
            }
        }
    }
}
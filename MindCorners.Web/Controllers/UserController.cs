using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DevExpress.Data;
using DevExpress.Data.Filtering;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using MindCorners.Authentication;
using MindCorners.Common.Code;
using MindCorners.Common.Code.Enums;
using MindCorners.Common.Code.Helpers;
using MindCorners.Common.Model;
using MindCorners.Web.Code.BaseController;
using UserProfile = MindCorners.Common.Model.UserProfile;
using DevExpress.Web.Mvc;
using MindCorners.Authentication.Attributes;

namespace MindCorners.Web.Controllers
{

    //[CustomAuthorize(Roles = "Admin")]
    [Authorize(Roles = "Admin,OrganizationAdmin")]
    public class UserController : ListControllerCustomBinding<ListFilter, Common.Model.UserProfile, Users_GetAll_Result, UserProfileRepository>
    {
        ApplicationDbContext context;

        private InvitationRepository _invitationRepository;
        private OrganizationRepository _organizationRepository;
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        #region Private Properties

        private UserProfileRepository _applicationRepository;
        #endregion

        #region Constructors
        public UserController()
            : base((int)ModuleName.User)
        {
            context = new ApplicationDbContext();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                _applicationRepository = new UserProfileRepository(Context, CurrentUserId, CurrentUserOrganizationID);
                _invitationRepository = new InvitationRepository(Context, CurrentUserId, CurrentUserOrganizationID);
                _organizationRepository = new OrganizationRepository(Context, CurrentUserId, CurrentUserOrganizationID);
            }
        }

        #endregion
      
        public ActionResult Create()
        {
            LoadViewBags();
            return View("Form", new Invitation());
        }
        //public ActionResult Edit(string id)
        //{
        //    var user = context.Users.FirstOrDefault(p => p.Id == id);
        //    return View("Form", user);
        //}

        [HttpPost]
        public async Task<ActionResult> CreateInvitation(string email, Guid roleId, Guid? organizationId)
        {
            using (var txscope = new TransactionScope(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    var activationCode = string.Empty;
                    _invitationRepository.Create(roleId, organizationId, email, out activationCode);
                    Context.SaveChanges();

                    Dictionary<string, string> values = new Dictionary<string, string>();
                    values.Add("[ActivationCode]", activationCode);
                    MessageHelper.SendMessage(MessageTemplateTypes.RegistrationActivationCode, values, CurrentUserOrganizationID, email);
                    //The Transaction will be completed
                    txscope.Complete();
                }
                catch(Exception ex)
                {
                    LogHelper.WriteError(ex);
                }
            }
            
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<ActionResult> Update(ApplicationUser user, string password, string roleName)
        {
            var userDb = context.Users.FirstOrDefault(p => p.Id == user.Id);
            if (userDb == null)
            {
                var result = await UserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await this.UserManager.AddToRoleAsync(user.Id, roleName);
                }
            }
            else 
            {
                var result = await UserManager.UpdateAsync(user);
            }

            context.SaveChanges();
            return RedirectToAction("Index");
        }

        #region Actions
        
        public async Task<ActionResult> Delete(Guid id)
        {  
            var application = _applicationRepository.GetById(id);

            if (application != null)
            {
                _applicationRepository.Delete(application);
                Context.SaveChanges();

                var user = UserManager.FindByIdAsync(application.User_Id);

               // await UserManager.DeleteAsync(user);

                await UserManager.SetLockoutEnabledAsync(application.User_Id, true);
            }
            return RedirectToAction("Index");
        }
        #endregion


        #region CustomBinding

        protected override GridViewModel CreateGridViewModel()
        {
            var viewModel = new GridViewModel();
            viewModel.KeyFieldName = "Id";
            viewModel.Columns.Add("Name");
            viewModel.Columns.Add("Type");
            viewModel.Columns.Add("IsEmail");
            return viewModel;
        }

        protected override void LoadListHeaderFilters(CriteriaOperator where)
        {
            ViewBag.Roles = context.Roles.ToDictionary(k => k.Id, v=> v.Name);
            ViewBag.Organizations = _organizationRepository.GetAllForListFilter();
        }

        public override ReadOnlyCollection<GridViewColumnState> DefaultSortExpressionState()
        {
            GridViewColumnState gsState = new GridViewColumnState() { SortIndex = 0, FieldName = "DateCreated", SortOrder = ColumnSortOrder.Descending };
            return new ReadOnlyCollection<GridViewColumnState>(new List<GridViewColumnState>() { gsState });
        }


        #endregion


        #region Private Methods

        private void LoadViewBags()
        {
            ViewBag.Roles = context.Roles;
            ViewBag.Organizations = _organizationRepository.GetAllForListFilter();
        }

        #endregion
    }
}
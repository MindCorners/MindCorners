using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using MindCorners.Web.Code.BaseController;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Facebook;
using Microsoft.AspNet.Identity.EntityFramework;
using MindCorners.Authentication;
using MindCorners.Common.Code;
using MindCorners.Common.Code.Enums;
using MindCorners.Common.Code.Helpers;
using MindCorners.Common.Model;
using MindCorners.Models.Results;
using UserProfile = MindCorners.Authentication.UserProfile;

namespace MindCorners.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private InvitationRepository _invitationRepository;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext DbContext { get; set; }
        private bool IsAuthenticationCodeValid
        {
            get { return Session["IsAuthenticationCodeValid"] != null ? (bool)Session["IsAuthenticationCodeValid"] : false; }
            set { Session["IsAuthenticationCodeValid"] = value; }
        }
        private Guid? InvitationId
        {
            get { return Session["InvitationId"] != null ? (Guid)Session["InvitationId"] : (Guid?)null; }
            set { Session["InvitationId"] = value; }
        }


        public AccountController()
        {
            DbContext = new ApplicationDbContext();
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(DbContext));
            _invitationRepository = new InvitationRepository();
            //UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DbContext));
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult RegisterView(RegisterViewModel model)
        {
            if (IsAuthenticationCodeValid)
            {
                return View("Register");
            }
            else
            {
                return RedirectToAction("RegisterActivationCode");
            }
        }
        [AllowAnonymous]
        public ActionResult RegisterActivationCode()
        {
            return View(new Invitation() { IsExternalUserLogin = false });
        }
        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var txscope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        var user = new ApplicationUser
                        {
                            UserName = model.Email,
                            Email = model.Email,
                            UserProfile =
                                new UserProfile()
                                {
                                    Id = Guid.NewGuid(),
                                    FirstName = model.FirstName,
                                    LastName = model.LastName
                                }
                        };

                        var result = await UserManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            using (MindCornersEntities context = new MindCornersEntities())
                            {
                                var invitation = context.Invitations.FirstOrDefault(p => p.Id == InvitationId);
                                if (invitation != null)
                                {

                                    var role = DbContext.Roles.FirstOrDefault(p => p.Id == invitation.RoleId.ToString());
                                    await this.UserManager.AddToRoleAsync(user.Id, role.Name);

                                    invitation.State = (int) InvitationStates.Accepted;
                                    invitation.StateDate = DateTime.Now;
                                    try
                                    {
                                        context.SaveChanges();
                                        IsAuthenticationCodeValid = false;
                                        InvitationId = null;

                                        if (Request.Files.Count > 0)
                                        {
                                            var file = Request.Files[0];

                                            if (file != null && file.ContentLength > 0)
                                            {   
                                                var fileExtention = Path.GetExtension(file.FileName);

                                                var fileFullName = Utilities.UploadBlob("profile-images", string.Format("{0}{1}", Guid.NewGuid(), fileExtention),
                                                    file.InputStream);

                                                user.UserProfile.ProfileImageString = fileFullName;
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e);
                                    }

                                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                                    // Send an email with this link
                                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                                    txscope.Complete();
                                    return RedirectToAction("Index", "Home");
                                }
                            }
                        }

                        AddErrors(result);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterActivationCode(Invitation model)
        {
            if (ModelState.IsValid)
            {
                var invitation = _invitationRepository.GetInvitationByAuthenticationCode(model.ActivationCode,
                    model.Email);

                if (invitation.IsOk)
                {
                    IsAuthenticationCodeValid = invitation.IsOk;
                    InvitationId = invitation.Id;
                    //invitation.State = (int)InvitationStates.Valid;
                    //invitation.StateDate = DateTime.Now;
                    //context.SaveChanges();

                    if (model.IsExternalUserLogin)
                    {
                        return await ExternalLoginConfirmation(new ExternalLoginConfirmationViewModel() { Email = model.Email }, null);
                    }

                    return View("Register", new RegisterViewModel() { Email = model.Email });
                }
                //else{ }
            }

            // If we got this far, something failed, redisplay form
            return View("ExternalLoginFailure");
        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }


        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            string firstName = string.Empty;
            string lastName = string.Empty;
            string middleName = string.Empty;
            string email = string.Empty;
            string puctureUrl = string.Empty;
            if (loginInfo.Login.LoginProvider == "Facebook")
            {
                var identity = AuthenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
                var access_token = identity.FindFirstValue("FacebookAccessToken");
                var fb = new FacebookClient(access_token);
                dynamic myInfo = fb.Get("/me?fields=first_name,middle_name,last_name,id,email,picture.width(200)"); // specify the email field
                loginInfo.Email = myInfo.email;
                puctureUrl = GetFacebookImageUrl(myInfo);


            }
            var loginInf = new ExternalLoginInfo() {};

            //  var firstNameClaim = loginInfo.ExternalIdentity.Claims.First(c => c.Type == "urn:facebook:first_name");

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        var test = GetProfilePicture(puctureUrl);

                        //    using (MindCornersEntities context = new MindCornersEntities())
                        //{
                        //    var user = context.UserProfiles.FirstOrDefault(p => p.User_Id == User.Identity.GetUserId());
                        //    if (user != null)
                        //    {
                        //        user.ProfileImage = GetProfilePicture(puctureUrl);
                        //        context.SaveChanges();
                        //    }
                        //}

                        return RedirectToLocal(returnUrl);
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    {
                        using (MindCornersEntities context = new MindCornersEntities())
                        {
                            var invitation = context.Invitations.FirstOrDefault(p => p.Email == loginInfo.Email && p.ExpireDate > DateTime.Now && p.DateDeleted == null);
                            if (invitation != null)
                            {
                                return View("RegisterActivationCode", new Invitation() { Email = loginInfo.Email, IsExternalUserLogin = true });
                            }
                            else
                            {
                                return View("ExternalLoginFailure");
                            }
                        }

                        // If the user does not have an account, then prompt the user to create an account


                        //ViewBag.ReturnUrl = returnUrl;
                        //ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                        //return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
                    }
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                //var ext =
                //    await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
                //var email = ext.Claims.First(x => x.Type.EndsWith("emailaddress")).Value;
                //var name = ext.Claims.First(x => x.Type.EndsWith("name")).Value;
                //var nameIdentifier = ext.Claims.First(x => x.Type.EndsWith("nameidentifier")).Value;


                var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

                //var firstNameClaim = loginInfo.ExternalIdentity.Claims.First(c => c.Type == "urn:facebook:first_name");

                string firstName = string.Empty;
                string lastName = string.Empty;
                string middleName = string.Empty;
                string email = string.Empty;
                byte[] picture = null;

                if (loginInfo.Login.LoginProvider == "Facebook")
                {
                    var identity = AuthenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
                    var access_token = identity.FindFirstValue("FacebookAccessToken");
                    var fb = new FacebookClient(access_token);
                    dynamic myInfo = fb.Get("/me?fields=first_name,middle_name,last_name,id,email,picture.width(600)");
                        // specify the email field
                    loginInfo.Email = myInfo.email;
                    firstName = myInfo.first_name;
                    middleName = myInfo.middle_name;
                    lastName = myInfo.last_name;
                    picture = GetProfilePicture(GetFacebookImageUrl(myInfo));
                }

                bool registrationOk = false;
                if (InvitationId.HasValue)
                {
                    using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        try
                        {
                            var fileFullName = string.Empty;
                            if (picture != null)
                            {
                                fileFullName = Utilities.UploadBlob("profile-images", string.Format("{0}{1}", Guid.NewGuid(), ".jpg"), picture);
                            }
                            
                            var user = new ApplicationUser
                            {
                                UserName = model.Email,
                                Email = model.Email,
                                UserProfile = new UserProfile()
                                {
                                    Id = Guid.NewGuid(),
                                    FirstName = firstName,
                                    LastName = lastName,
                                    MiddleName = middleName,
                                    //ProfileImage = picture,
                                    ProfileImageString = fileFullName
                                }
                            };
                            var result = await UserManager.CreateAsync(user);
                            if (result.Succeeded)
                            {
                                result = await UserManager.AddLoginAsync(user.Id, info.Login);
                                if (result.Succeeded)
                                {
                                    using (InvitationRepository _invitationRepository = new InvitationRepository(Context,user.UserProfile.Id, null))
                                    using (UserContactRepository userContactRepository = new UserContactRepository(Context, user.UserProfile.Id, null))
                                    using (UserProfileRepository userProfileRepository = new UserProfileRepository(Context, user.UserProfile.Id, null))
                                    {
                                        var invitation = _invitationRepository.GetById(InvitationId.Value);
                                        if (invitation != null)
                                        {
                                            var role = DbContext.Roles.First(p => p.Id == invitation.RoleId.ToString());
                                            var addToRole = await this.UserManager.AddToRoleAsync(user.Id, role != null ? role.Name : MindCorners.Common.Code.Constants.GeneralUserRoleId);
                                            if (addToRole.Succeeded)
                                            {
                                                invitation.State = (int)InvitationStates.Accepted;
                                                invitation.StateDate = DateTime.Now;
                                                _invitationRepository.Update(invitation);


                                                //add userToUserContacts

                                                userContactRepository.Create(new UserContact()
                                                {
                                                    ContactName = user.UserProfile.FullName,
                                                    ContactUserId = user.UserProfile.Id,
                                                    MainUserId = invitation.CreatorId,
                                                    State = (int)InvitationStates.Accepted,
                                                    StateDate = DateTime.Now
                                                });

                                                var invitationCreator = userProfileRepository.GetById(invitation.CreatorId);
                                                userContactRepository.Create(new UserContact()
                                                {
                                                    ContactName = invitationCreator.FullName,
                                                    ContactUserId = invitation.CreatorId,
                                                    MainUserId = user.UserProfile.Id,
                                                    State = (int)InvitationStates.Accepted,
                                                    StateDate = DateTime.Now
                                                });


                                                //add create Circle of two users
                                                //var circle = new Common.Model.Circle() { IsGroup = false };
                                                //CircleRepository circleRepository = new CircleRepository(Context, dbUser, null);
                                                //circleRepository.Create(circle);
                                                //Context.SaveChanges();

                                                //circleRepository.AddMainPersonToCircleUser(circle.Id, dbUser);
                                                //circleRepository.SynchCircleUsers(circle.Id, new List<Guid>() { userRegistration.UserProfile.Id });

                                                Context.SaveChanges();

                                                //_invitationRepository.SaveChanges();
                                                transactionScope.Complete();

                                            }
                                            registrationOk = true;
                                        }
                                    }
                                }
                                if (registrationOk)
                                {
                                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                                    return RedirectToAction("Index", "Home");
                                }
                            }

                            AddErrors(result);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteError(ex);
                        }
                    }

                    //var user = new ApplicationUser
                    //{
                    //    UserName = model.Email,
                    //    Email = model.Email,
                    //    UserProfile = new UserProfile()
                    //    {
                    //        Id = Guid.NewGuid(),
                    //        FirstName = firstName,
                    //        LastName = lastName,
                    //        MiddleName = middleName,
                    //        ProfileImage = picture,
                    //    }
                    //};
                    //var result = await UserManager.CreateAsync(user);
                    //if (result.Succeeded)
                    //{
                    //    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    //    if (result.Succeeded)
                    //    {
                    //        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    //        return RedirectToLocal(returnUrl);
                    //    }
                    //}
                   
                }

                
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        private byte[] GetProfilePicture(string url)
        {
            var webClient = new WebClient();
            byte[] imageBytes = null;
            try
            {

                imageBytes = webClient.DownloadData(url);
            }
            catch (Exception c)
            {
                //MessageBox.Show(c.Message);
            }
            return imageBytes;

        }
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        private string GetFacebookImageUrl(dynamic myInfo)
        {
            var jsonObject = (myInfo as JsonObject);
            if (jsonObject != null && jsonObject.ContainsKey("picture"))
            {
                var pictureObject = jsonObject.FirstOrDefault(p => p.Key == "picture");
                if (pictureObject.Value != null)
                {
                    var jsonObjectData = (pictureObject.Value as JsonObject);
                    if (jsonObjectData != null && jsonObjectData.ContainsKey("data"))
                    {
                        var pictureObjectData = jsonObjectData.FirstOrDefault(p => p.Key == "data");
                        if (pictureObjectData.Value != null)
                        {
                            var jsonObjectDataUrl = (pictureObjectData.Value as JsonObject);
                            if (jsonObjectDataUrl != null && jsonObjectDataUrl.ContainsKey("url"))
                            {
                                var pictureObjectDataUrl = jsonObjectDataUrl.FirstOrDefault(p => p.Key == "url");
                                if (pictureObjectDataUrl.Value != null)
                                {
                                    return pictureObjectDataUrl.Value.ToString();
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        [AllowAnonymous]
        public ActionResult SuccessLoginFromExt()
        {   
            return View();
        }

    }


    public class FacebookProfileInfo
    {
        public string first_name { get; set; }
        public string last_name { get; set; }

        public string id { get; set; }

        public string email { get; set; }

        public FacebookProfilePicture picture { get; set; }
    }

    public class FacebookProfilePicture
    {
        public string height { get; set; }
        public string url { get; set; }
        public string width { get; set; }
        public string is_silhouette { get; set; }
    }
}
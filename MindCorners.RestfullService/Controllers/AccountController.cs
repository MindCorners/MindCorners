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
    public class AccountController : BaseController
    {
        //private InvitationRepository _invitationRepository;

        public AccountController()
        {
            //_invitationRepository = new InvitationRepository(Context, DbUser, null);
        }

        [HttpGet]
        public async Task<string> SendEmail()
        {
            MessageHelper.SendEmail(null, "nchogovadze@directsolutions.ge", "Test", "this is testMessage");
            LogHelper.WriteError(new Exception("error test"));
            LogHelper.WriteInfoLog("Info log test");
            return "testtest";
        }

        [HttpGet]
        public async Task<User> GetUser(string userName, string password)
        {
            ApplicationUser applicationUser = await UserManager.FindAsync(userName, password);

            if (applicationUser != null)
            {
                return new User()
                {
                    FirstName = applicationUser.UserProfile.FirstName,
                    LastName = applicationUser.UserProfile.LastName,
                    Email = applicationUser.Email,
                    FullName = applicationUser.UserProfile.FullName,
                    Id = applicationUser.UserProfile.Id,
                    //ProfileImage = applicationUser.UserProfile.ProfileImage,
                    ProfileImageString = Request.GetFileUrl((int)FileType.Profile, applicationUser.UserProfile.ProfileImageString)
                };
            }
            return null;
        }

        [HttpGet]
        public async Task<IdResult> CheckAuthenticationCode(string code, string email)
        {
            using (InvitationRepository _invitationRepository = new InvitationRepository())
            {
                var invitation = _invitationRepository.GetInvitationByAuthenticationCode(code, email);
                return invitation;
            }
        }

        [Route("api/Account/RegisterUser")]
        [HttpPost]
        public async Task<ObjectResult<User>> RegisterUser(UserRegister user)
        {
            if (user.InvitationId == Guid.Empty)
            {
                return new ObjectResult<User>()
                {
                    IsOk = false,
                    ErrorMessage = "No valid invitation exists"
                };
            }
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var userRegistration = new ApplicationUser
                    {
                        UserName = user.Email,
                        Email = user.Email,
                        UserProfile =
                            new Authentication.UserProfile()
                            {
                                Id = Guid.NewGuid(),
                                FirstName = user.FirstName,
                                LastName = user.LastName
                            }
                    };
                    var result = await UserManager.CreateAsync(userRegistration, user.Password);

                    if (result.Succeeded)
                    {
                        var dbUser = userRegistration.UserProfile.Id;
                        using (InvitationRepository _invitationRepository = new InvitationRepository(Context, dbUser, null))
                        using (UserContactRepository userContactRepository = new UserContactRepository(Context, dbUser, null))
                        using (UserProfileRepository userProfileRepository = new UserProfileRepository(Context, dbUser, null))
                        {
                            var invitation = _invitationRepository.GetById(user.InvitationId);
                            if (invitation != null)
                            {
                                var addToRole =
                                    await
                                        this.UserManager.AddToRoleAsync(userRegistration.Id,
                                            MindCorners.Common.Code.Constants.GeneralUserRoleId);
                                if (addToRole.Succeeded)
                                {
                                    invitation.State = (int)InvitationStates.Accepted;
                                    invitation.StateDate = DateTime.Now;
                                    _invitationRepository.Update(invitation);

                                    //add userToUserContacts

                                    userContactRepository.Create(new UserContact()
                                    {
                                        ContactName = userRegistration.UserProfile.FullName,
                                        ContactUserId = userRegistration.UserProfile.Id,
                                        MainUserId = invitation.CreatorId,
                                        State = (int)InvitationStates.Accepted,
                                        StateDate = DateTime.Now
                                    });

                                    var invitationCreator = userProfileRepository.GetById(invitation.CreatorId);
                                    userContactRepository.Create(new UserContact()
                                    {
                                        ContactName = invitationCreator.FullName,
                                        ContactUserId = invitation.CreatorId,
                                        MainUserId = userRegistration.UserProfile.Id,
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
                                    return new ObjectResult<User>()
                                    {
                                        IsOk = true,
                                        ReturnedObject = new User()
                {
                                            FirstName = userRegistration.UserProfile.FirstName,
                                            LastName = userRegistration.UserProfile.LastName,
                                            Email = userRegistration.Email,
                                            FullName = userRegistration.UserProfile.FullName,
                                            Id = userRegistration.UserProfile.Id,
                                            //ProfileImage = applicationUser.UserProfile.ProfileImage,
                                            ProfileImageString = Request.GetFileUrl((int)FileType.Profile, userRegistration.UserProfile.ProfileImageString)
                                        }
                                };
                                }
                                return new ObjectResult<User>()
                                {
                                    IsOk = false,
                                    ErrorMessage = string.Join(",", addToRole.Errors)
                                };
                                //   return RedirectToAction("Index", "Home");
                            }
                            return new ObjectResult<User>()
                            {
                                IsOk = false,
                                ErrorMessage = string.Join(",", result.Errors)
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex);
                    return new ObjectResult<User>()
                    {
                        IsOk = false,
                        ErrorMessage = ex.ToString()
                    };
                }
                //var user = JsonConvert.DeserializeObject<UserRegister>(userData);
            }

            return new ObjectResult<User>()
            {
                IsOk = false,
                ErrorMessage = "Error"
            };
        }



        [Route("api/Account/SaveProfilePhoto")]
        [HttpPost]
        public async Task<FilePathResult> SaveProfilePhoto(UserProfileImage profileImage)
        {
            if (profileImage.Id == Guid.Empty)
            {
                return new FilePathResult()
                {
                    IsOk = false,
                    ErrorMessage = "No valid invitation exists"
                };
            }
            if (profileImage.Image == null && string.IsNullOrEmpty(profileImage.ImageString))
            {
                return new FilePathResult()
                {
                    IsOk = false,
                    ErrorMessage = "No image"
                };
            }
            var dbUser = DbUser;
            using (UserProfileRepository _userProfileRepository = new UserProfileRepository(Context, dbUser, null))
            {
                var user = _userProfileRepository.GetByIdForUpdat(profileImage.Id);
                ;
                if (user != null)
                {
                    using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        try
                        {
                            //user.ProfileImage = profileImage.Image;
                            var fileName = string.Format("{0}.jpg", Guid.NewGuid());
                            var path = Common.Code.Utilities.UploadBlob(Common.Code.Utilities.GetAzureFolderByFileType((int)FileType.Profile), fileName, profileImage.Image);
                            user.ProfileImageString = fileName;
                            //Context.Entry(user).State = EntityState.Modified;

                            _userProfileRepository.Update(user);
                            Context.SaveChanges();
                            transactionScope.Complete();
                            return new FilePathResult()
                            {
                                IsOk = true,
                                FileUrl = Request.GetFileUrl((int)FileType.Profile, fileName)
                            };
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteError(ex);
                            return new FilePathResult()
                            {
                                IsOk = false,
                                ErrorMessage = ex.ToString()
                            };
                        }
                    }
                }
                //var user = JsonConvert.DeserializeObject<UserRegister>(userData);
                return new FilePathResult()
                {
                    IsOk = false,
                    ErrorMessage = "No User Found"
                };
            }
        }

        [HttpPost]
        public HttpResponseMessage PostTest(string value)
        {
            // Save Code will be here
            return new HttpResponseMessage(HttpStatusCode.OK);
        }




        //public Authentication.UserProfile GetUser(string userName, string password)
        //{
        //    //ApplicationUser applicationUser = await UserManager.FindAsync(userName, password);

        //    //if (applicationUser != null)
        //    //{
        //    //    return applicationUser.UserProfile;
        //    //}
        //    return null;
        //}


        //// GET: api/Account
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Account/5
        //public string Get(int id)
        //{
        //    return "valusdfsdfe";
        //}


        // POST: api/Account
        public void Post([FromBody]string userData)
        {
        }

        //// PUT: api/Account/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Account/5
        //public void Delete(int id)
        //{
        //}


        public bool AddEmpDetails([FromBody]string userData)
        {
            return true;
            //write insert logic  

        }

        [HttpPost]
        [Route("save")]
        public bool Save([FromBody]UserRegister userData)
        {
            return true;
        }

        private async Task<FbData> GetFacebookProfileAsync(string accessToken)
        {
            var requestUrl = "https://graph.facebook.com/v2.9/me/?fields=first_name,middle_name,last_name,id,email,picture.width(600)&access_token=" + accessToken;
            var httpClient = new HttpClient();
            try
            {
                var userJson = await httpClient.GetStringAsync(requestUrl);
                return JsonConvert.DeserializeObject<FbData>(userJson);
            }
            catch (Exception e)
            {
                string s = e.ToString();
            }

            return null;
        }

        [HttpGet]
        public async Task<User> LoginExternalUser(string provider, string accessToken, bool loadExtUser = false)
        {
            var userData = await GetFacebookProfileAsync(accessToken);
            ApplicationUser applicationUser = await UserManager.FindAsync(new UserLoginInfo(provider, userData.id));

            if (applicationUser != null)
            {
                return new User()
                {
                    FirstName = applicationUser.UserProfile.FirstName,
                    LastName = applicationUser.UserProfile.LastName,
                    Email = applicationUser.Email,
                    FullName = applicationUser.UserProfile.FullName,
                    Id = applicationUser.UserProfile.Id,
                    //ProfileImage = applicationUser.UserProfile.ProfileImage,
                    ProfileImageString = Request.GetFileUrl((int)FileType.Profile, applicationUser.UserProfile.ProfileImageString)
                };
            }
            if (loadExtUser)
            {
                return new User() {Email = userData.email};
            }

            return null;
        }

        [Route("api/Account/RegisterExternal")]
        [HttpPost]
        public async Task<ObjectResult<User>> RegisterExternal(RegisterExternalBindingModel model)
        {
            bool registrationOk = false;

            if (!model.InvitationId.HasValue || model.InvitationId == Guid.Empty)
            {
                return new ObjectResult<User>()
                {
                    IsOk = false,
                    ErrorMessage = "No valid invitation exists"
                };
            }


            // if (model.InvitationId.HasValue)
            {
                using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        var userData = await GetFacebookProfileAsync(model.ExternalAccessToken);
                        var fileFullName = string.Empty;
                        if (userData.picture != null)
                        {
                            var picture = GetProfilePicture(userData.picture.data.url);
                            if (picture != null)
                            {
                                fileFullName = Utilities.UploadBlob("profile-images", string.Format("{0}{1}", Guid.NewGuid(), ".jpg"), picture);
                            }
                        }

                        var user = new ApplicationUser
                        {
                            UserName = model.UserName,
                            Email = model.UserName,
                            UserProfile = new Authentication.UserProfile()
                            {
                                Id = Guid.NewGuid(),
                                FirstName = userData.first_name,
                                LastName = userData.last_name,
                                MiddleName = userData.middle_name,
                                //ProfileImage = picture,
                                ProfileImageString = fileFullName
                            }
                        };
                        var result = await UserManager.CreateAsync(user);
                        if (result.Succeeded)
                        {
                            var info = new ExternalLoginInfo()
                            {
                                DefaultUserName = model.UserName,
                                Login = new UserLoginInfo(model.Provider, userData.id)
                            };

                            result = await UserManager.AddLoginAsync(user.Id, info.Login);
                            if (result.Succeeded)
                            {
                                using (InvitationRepository _invitationRepository = new InvitationRepository(Context, user.UserProfile.Id, null))
                                using (UserContactRepository userContactRepository = new UserContactRepository(Context, user.UserProfile.Id, null))
                                using (UserProfileRepository userProfileRepository = new UserProfileRepository(Context, user.UserProfile.Id, null))
                                {
                                    var invitation = _invitationRepository.GetById(model.InvitationId.Value);
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
                                            return new ObjectResult<User>()
                                            {
                                                IsOk = true,
                                                ReturnedObject = new User()
                                                {
                                                    FirstName = user.UserProfile.FirstName,
                                                    LastName = user.UserProfile.LastName,
                                                    Email = user.Email,
                                                    FullName = user.UserProfile.FullName,
                                                    Id = user.UserProfile.Id,
                                                    //ProfileImage = applicationUser.UserProfile.ProfileImage,
                                                    ProfileImageString = Request.GetFileUrl((int)FileType.Profile, user.UserProfile.ProfileImageString)
                                                }
                                            };

                                        }

                                        return new ObjectResult<User>()
                                        {
                                            IsOk = false,
                                            ErrorMessage = string.Join(",", addToRole.Errors)
                                        };
                                        //   return RedirectToAction("Index", "Home");
                                    }
                                    return new ObjectResult<User>()
                                    {
                                        IsOk = false,
                                        ErrorMessage = "No valid Invitation"
                                    };
                                }
                            }
                            return new ObjectResult<User>()
                            {
                                IsOk = false,
                                ErrorMessage = string.Join(",", result.Errors)
                            };
                        }
                        return new ObjectResult<User>()
                        {
                            IsOk = false,
                            ErrorMessage = string.Join(",", result.Errors)
                        };
                        //AddErrors(result);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteError(ex);
                        return new ObjectResult<User>()
                        {
                            IsOk = false,
                            ErrorMessage = ex.ToString()
                        };
                    }
                }
            }

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
            //if (verifiedAccessToken == null)
            //{
            //    return BadRequest("Invalid Provider or External Access Token");
            //}

            //user = new IdentityUser() { UserName = model.UserName };
            //ApplicationUser user = await UserManager.CreateAsync(new UserLoginInfo(model.Provider, model.ExternalAccessToken));

            // bool hasRegistered = user != null;

            // if (hasRegistered)
            // {
            //     return BadRequest("External user is already registered");
            // }

            //// user = new IdentityUser() { UserName = model.UserName };

            // IdentityResult result = await UserManager.CreateAsync(user);
            // if (!result.Succeeded)
            // {
            //     return GetErrorResult(result);
            // }

            // var info = new ExternalLoginInfo()
            // {
            //     DefaultUserName = model.UserName,
            //     Login = new UserLoginInfo(model.Provider, verifiedAccessToken.user_id)
            // };

            // result = await _repo.AddLoginAsync(user.Id, info.Login);
            // if (!result.Succeeded)
            // {
            //     return GetErrorResult(result);
            // }

            // //generate access token response
            // var accessTokenResponse = GenerateLocalAccessTokenResponse(model.UserName);

            // return Ok(accessTokenResponse);



            return new ObjectResult<User>()
            {
                IsOk = false,
                ErrorMessage = "Error"
            };
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
    }

}

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;
using MindCorners.Authentication;
using MindCorners.Common.Code;
using MindCorners.Common.Code.Enums;
using MindCorners.Common.Code.Helpers;
using MindCorners.Common.Model;
using MindCorners.Models;
using MindCorners.Models.Enums;
using MindCorners.Models.Results;
using Invitation = MindCorners.Models.Invitation;
//using InvitationRepository = MindCorners.DAL.InvitationRepository;

namespace MindCorners.RestfullService.Controllers
{
    public class InvitationController : BaseController
    {
        //private InvitationRepository _invitationRepository;
        // private UserContactRepository _userContactRepository;
        public InvitationController()
        {
            //_invitationRepository = new InvitationRepository(Context, DbUser, null);
            //_userContactRepository = new UserContactRepository(Context, DbUser, null);
        }

        [Route("api/Invitation/Create")]
        [HttpPost]
        public async Task<BoolResult> InsertSystemInvitation(Invitation invitation)
        {
            if (invitation == null)
            {
                return new BoolResult()
                {
                    IsOk = false,
                    ErrorMessage = "No invitation info"
                };
            }
            var dbUser = DbUser;
            using (Common.Model.InvitationRepository _invitationRepository = new Common.Model.InvitationRepository(Context, dbUser, null))
            using (UserContactRepository _userContactRepository = new UserContactRepository(Context, dbUser, null))
            using (UserProfileRepository _userProfileRepository = new UserProfileRepository(Context, dbUser, null))
            {
                //checkIfPersonIsInUsers
                var user = await UserManager.FindByEmailAsync(invitation.Email);
                if (user == null)
                {
                    //create Invitation
                    var invitationByEmail = _invitationRepository.GetInvitationDoesNotExistsByEmail(invitation.Email);

                    if (invitationByEmail.IsOk)
                    {
                        using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                        {
                            try
                            {
                                //create new Invitation
                                var activationCode = string.Empty;
                                _invitationRepository.Create(new Guid("c51c4a63-8278-4013-9639-8f9ac6775b72"), null,
                                    invitation.Email, out activationCode);

                                Dictionary<string, string> values = new Dictionary<string, string>();
                                values.Add("[ActivationCode]", activationCode);
                                MessageHelper.SendMessage(MessageTemplateTypes.RegistrationActivationCode, values, null, invitation.Email,null, bccEmail:"nchogovadze@directsolutions.ge");

                                Context.SaveChanges();
                                transactionScope.Complete();
                                return new BoolResult()
                                {
                                    IsOk = true
                                };
                            }
                            catch (Exception e)
                            {
                                LogHelper.WriteError(e);
                                return new BoolResult()
                                {
                                    IsOk = false,
                                    ErrorMessage = e.ToString()
                                };
                            }
                        }
                    }
                    //check if Invitation exists
                    return new BoolResult()
                    {
                        IsOk = false,
                        ErrorMessage = invitationByEmail.ErrorMessage
                    };
                }


                //create contactInvitation
                var invitationToContactsByUserId = _userContactRepository.GetInvitationInContactsDoesNotExistsByUserId(dbUser, user.UserProfile.Id);

                if (invitationToContactsByUserId.IsOk)
                {
                    _userContactRepository.Create(new UserContact()
                    {
                        ContactName = user.UserProfile.FullName,
                        ContactUserId = user.UserProfile.Id,
                        MainUserId = DbUser,
                        State = (int)InvitationStates.Pending,
                        StateDate = DateTime.Now
                    });
                    try
                    {
                        Context.SaveChanges();
                        var senderUser = _userProfileRepository.GetById(dbUser);
                        Utilities.AddNotification(dbUser, user.UserProfile.Id, dbUser, (int)NotificationTypes.InvitationToContactsRecieved,
                            string.Format("You are added to {0}'s contacts. Please Accept or Reject invitation", senderUser.FullName));


                        return new BoolResult()
                        {
                            IsOk = true,
                        };
                    }
                    catch (Exception e)
                    {
                        LogHelper.WriteError(e);
                    }
                }

                return new BoolResult()
                {
                    IsOk = false,
                    ErrorMessage = invitationToContactsByUserId.ErrorMessage
                };
            }


            //var invitation = _invitationRepository.GetById(user.InvitationId); ;
            //if (invitation != null)
            //{
            //    using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            //    {
            //        try
            //        {
            //            var userRegistration = new ApplicationUser
            //            {
            //                UserName = user.Email,
            //                Email = user.Email,
            //                UserProfile = new Authentication.UserProfile() { Id = Guid.NewGuid(), FirstName = user.FirstName, LastName = user.LastName }
            //            };
            //            var result = await UserManager.CreateAsync(userRegistration, user.Password);

            //            if (result.Succeeded)
            //            {
            //                var addToRole = await this.UserManager.AddToRoleAsync(userRegistration.Id, MindCorners.Common.Code.Constants.GeneralUserRoleId);
            //                if (addToRole.Succeeded)
            //                {
            //                    invitation.State = (int)InvitationStates.Valid;
            //                    invitation.StateDate = DateTime.Now;
            //                    _invitationRepository.Update(invitation);
            //                    Context.SaveChanges();
            //                    //_invitationRepository.SaveChanges();
            //                    transactionScope.Complete();
            //                    return new IdResult()
            //                    {
            //                        IsOk = true,
            //                        Id = userRegistration.UserProfile.Id
            //                    };
            //                }
            //                return new IdResult()
            //                {
            //                    IsOk = false,
            //                    ErrorMessage = string.Join(",", addToRole.Errors)
            //                };
            //                //   return RedirectToAction("Index", "Home");
            //            }
            //            return new IdResult()
            //            {
            //                IsOk = false,
            //                ErrorMessage = string.Join(",", result.Errors)
            //            };

            //        }
            //        catch (Exception ex)
            //        {
            //            LogHelper.WriteError(ex);
            //            return new IdResult()
            //            {
            //                IsOk = false,
            //                ErrorMessage = ex.ToString()
            //            };
            //        }
            //    }
            //}
        }

        [Route("api/Invitation/ChangeInvitationState")]
        [HttpPost]
        public async Task<IdResult> ChangeInvitationState(Contact invitorUserProfile)
        {
            if (invitorUserProfile == null)
            {
                return new IdResult()
                {
                    IsOk = false,
                    ErrorMessage = "No user info"
                };
            }
            var dbUser = DbUser;
            using (UserContactRepository _userContactRepository = new UserContactRepository(Context, dbUser, null))
            using (UserProfileRepository _userProfileRepository = new UserProfileRepository(Context, dbUser, null))
            {
                var invitorUserProfileFromDb = _userProfileRepository.GetById(invitorUserProfile.Id);

                if (invitorUserProfile.Id != Guid.Empty && invitorUserProfileFromDb != null)
                {
                    var userContactDB = _userContactRepository.GetByContactUserId(invitorUserProfileFromDb.Id, dbUser);
                    if (userContactDB != null)
                    {
                        userContactDB.State = invitorUserProfile.State;
                        userContactDB.StateDate = DateTime.Now;

                        var senderUser = _userProfileRepository.GetById(userContactDB.MainUserId);
                        if (invitorUserProfile.State == (byte) InvitationStates.Accepted)
                        {   
                            _userContactRepository.Create(new UserContact()
                            {
                                ContactName = senderUser.FullName,
                                ContactUserId = senderUser.Id,
                                MainUserId = dbUser,
                                State = (int)InvitationStates.Accepted,
                                StateDate = DateTime.Now
                            });

                            Utilities.AddNotification(dbUser, userContactDB.MainUserId, dbUser, (int)NotificationTypes.InvitationToContactsConfirmed,
                                string.Format("You invitaion is accepted"));
                        }
                        Context.SaveChanges();
                        return new IdResult()
                        {
                            IsOk = true,
                            Id = userContactDB.Id,
                        };
                    }
                }
            }

            return new IdResult()
            {
                IsOk = false,
                ErrorMessage = "Error"
            };
            //var user = JsonConvert.DeserializeObject<UserRegister>(userData);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MindCorners.Common.Code.Enums;
using MindCorners.Common.Model;
using MindCorners.Models;
using MindCorners.Models.Enums;
using MindCorners.RestfullService.Code;

namespace MindCorners.RestfullService.Controllers
{
    public class ContactController : BaseController
    {
        //private UserContactRepository _userContactRepository;
        public ContactController()
        {
            //_userContactRepository = new UserContactRepository(Context, DbUser, null);
        }
        [HttpGet]
        public async Task<List<Contact>> GetAll(Guid userId)
        {
            var dbUser = DbUser;
            using (UserContactRepository _userContactRepository = new UserContactRepository(Context, dbUser, null))
            {
                var contacts = _userContactRepository.GetAllByUserId(dbUser);
                if (contacts != null)
                {
                    return contacts.Select(p => new Contact()
                    {
                        Id = p.Id,
                        Email = p.Email,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        ProfileImageString = Request.GetFileUrl((int)FileType.Profile, p.ProfileImageString),
                        IsActivated = p.ContactState == (int) InvitationStates.Accepted
                    }).ToList();
                }

                return new List<Contact>();
            }
        }

        [HttpGet]
        public async Task<List<Contact>> GetOnlySelectedForCircle(Guid circleId)
        {
            using (UserContactRepository _userContactRepository = new UserContactRepository(Context, DbUser, null))
            {
                var selectedUsers = _userContactRepository.GetAllCircleUsers(circleId);
                if (selectedUsers != null)
                {
                    return selectedUsers.Select(p => new Contact()
                    {
                        Id = p.Id,
                        Email = p.Email,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        ProfileImageString = Request.GetFileUrl((int)FileType.Profile, p.ProfileImageString),
                        IsSelected = true
                    }).ToList();
                }

                return new List<Contact>();
            }
        }

        [HttpGet]
        public async Task<List<Contact>> GetAllWithSelectedForCircle(Guid circleId)
        {
            var dbUser = DbUser;
            using (UserContactRepository _userContactRepository = new UserContactRepository(Context, dbUser, null))
            {
                var selectedUsers = _userContactRepository.GetAllByCircleIdAndUserId(dbUser, circleId);
                if (selectedUsers != null)
                {
                    return selectedUsers.Select(p => new Contact()
                    {
                        Id = p.Id,
                        //Email = p.Email,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        ProfileImageString = Request.GetFileUrl((int)FileType.Profile, p.ProfileImageString),
                        IsSelected = p.IsSelectedInCircle
                    }).ToList();
                }

                return new List<Contact>();
            }
        }

        [HttpGet]
        public async Task<Contact> GetById(Guid userId)
        {
            var dbUser = DbUser;
            using (UserProfileRepository _userProfileRepository = new UserProfileRepository(Context, dbUser, null))
            using (UserContactRepository _userContactRepository = new UserContactRepository(Context, dbUser, null))
            {
                var user = _userProfileRepository.GetById(userId);
                if (user != null)
                {
                    return new Contact()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        ProfileImageString = Request.GetFileUrl((int)FileType.Profile, user.ProfileImageString)
                    };
                }

                return new Contact();
            }
        }


        [HttpGet]
        public async Task<Contact> GetByIdWithState(Guid userId)
        {
            var dbUser = DbUser;
            using (UserProfileRepository _userProfileRepository = new UserProfileRepository(Context, dbUser, null))
            using (UserContactRepository _userContactRepository = new UserContactRepository(Context, dbUser, null))
            {
                
                var user = _userProfileRepository.GetById(userId);
                if (user != null)
                {
                    var userContact = _userContactRepository.GetByContactUserId(user.Id, dbUser);
                    return new Contact()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        State = userContact?.State,
                        ProfileImageString = Request.GetFileUrl((int)FileType.Profile, user.ProfileImageString)
                    };
                }

                return new Contact();
            }
        }
    }
}

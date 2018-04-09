using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Common.Code.CoreRepositories;
using MindCorners.Common.Code.Enums;
using MindCorners.Models.Results;

namespace MindCorners.Common.Model
{
    public class UserContactRepository : GenericRepository<UserContact>
    {
        private readonly MindCornersEntities _context;
        private readonly Guid _currentUserId;
        public UserContactRepository(MindCornersEntities context, Guid currentUserId, Guid? currentUserOrganizationId, bool createObjectSet = true) : base(context, currentUserId, currentUserOrganizationId, createObjectSet)
        {
            _context = context;
            _currentUserId = currentUserId;
        }
        public UserContactRepository()
        {
            _context = new MindCornersEntities();
        }

        public BoolResult GetInvitationInContactsDoesNotExistsByUserId(Guid invitorUserId, Guid userId)
        {
            var invitation = _context.UserContacts.FirstOrDefault(
                p => p.ContactUserId == userId && p.MainUserId == invitorUserId &&
                     //p.State == (int) InvitationStates.Unvalid && p.ExpireDate > DateTime.Now &&
                     p.DateDeleted == null);
            if (invitation != null)
            {
                if (invitation.State == (int)InvitationStates.Accepted)
                {
                    return new BoolResult()
                    {
                        ErrorMessage = "User is already invited to your contacts and accepted",
                        IsOk = false
                    };
                }
            }
            return new BoolResult()
            {
                IsOk = true
            };
        }

        public List<UserProfile> GetAllByUserId(Guid userId)
        {
            var list = (from userContact in _context.UserContacts
                        join userProfile in _context.UserProfiles on userContact.ContactUserId equals userProfile.Id
                        join user in Context.AspNetUsers on userProfile.User_Id equals user.Id
                        where userContact.DateDeleted == null && userProfile.DateDeleted == null && userContact.MainUserId == userId
                        select new
                        {
                            userProfile.Id,
                            user.Email,
                            userProfile.FirstName,
                            userProfile.LastName,
                            userProfile.ProfileImageString,
                            userContact.State,
                            userContact.ContactName
                        }
               ).ToList().Select(p => new UserProfile()
               {
                   Id = p.Id,
                   Email = p.Email,
                   ContactName = p.ContactName,
                   FirstName = p.FirstName,
                   LastName = p.LastName,
                   ProfileImageString = p.ProfileImageString,
                   ContactState = p.State
               }).ToList();

            return list;
        }
        public List<UserProfile> GetAllCircleUsers(Guid circleId)
        {
            var list = (from circleUser in _context.CircleUsers
                        join userProfile in _context.UserProfiles on circleUser.UserId equals userProfile.Id
                        where circleUser.DateDeleted == null && circleUser.CircleId == circleId && !circleUser.IsMainPerson
                        select userProfile
               ).ToList();

            return list;
        }

        public List<UserProfile> GetAllByCircleIdAndUserId(Guid userId, Guid circleId)
        {
            var resultList = (from userContact in _context.UserContacts
                              from userProfile in _context.UserProfiles
                              join user in Context.AspNetUsers on userProfile.User_Id equals user.Id
                              where
                                  userContact.DateDeleted == null && userProfile.DateDeleted == null &&
                                  userContact.MainUserId == userId &&
                                  userContact.ContactUserId == userProfile.Id
                              select new
                              {
                                  userProfile.Id,
                                  user.Email,
                                  userProfile.FirstName,
                                  userProfile.LastName,
                                  userProfile.ProfileImageString,
                                  userContact.State,
                                  IsSelectedInCircle = false
                              }).ToList();
            List<UserProfile> list = null;
            if (circleId == Guid.Empty)
            {
               list = resultList.Select(p => new UserProfile()
                    {
                        Id = p.Id,
                        Email = p.Email,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        ProfileImageString = p.ProfileImageString,
                        ContactState = p.State,
                        IsSelectedInCircle = p.IsSelectedInCircle
                    }).ToList();
            }
            else
            {
                var circleUsers =
                    _context.CircleUsers.Where(p => p.DateDeleted == null && p.CircleId == circleId && !p.IsMainPerson).Select(p =>p.UserId);
                list = resultList.Select(p => new UserProfile()
                    {
                        Id = p.Id,
                        Email = p.Email,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        ProfileImageString = p.ProfileImageString,
                        ContactState = p.State,
                        IsSelectedInCircle = circleUsers.Contains(p.Id)
                    }).ToList();
            }
            return list;
        }

        public UserContact GetByContactUserId(Guid mainUser, Guid userId)
        {
            var userContact = _context.UserContacts.FirstOrDefault(
                p => p.ContactUserId == userId && p.MainUserId == mainUser &&
                     //p.State == (int) InvitationStates.Unvalid && p.ExpireDate > DateTime.Now &&
                     p.DateDeleted == null);
            return userContact;
        }


        public Circle GetCircleByUsersPair(Guid userId, Guid userContactId)
        {
            var list = (from userCircle in _context.CircleUsers
                join circle in _context.Circles on userCircle.CircleId equals circle.Id
                // join userContact in _context.UserContacts on userContact.Id  = userCircle.
                where !circle.IsGroup &&
                      userCircle.DateDeleted == null && circle.DateDeleted == null
                      && (userCircle.UserId == userId || userCircle.UserId == userContactId)
                select circle
                ).Distinct().FirstOrDefault();

            return list;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using MindCorners.Common.Code;
using MindCorners.Common.Code.CoreRepositories;
using MindCorners.Common.Code.Interfaces;

namespace MindCorners.Common.Model
{
    public class UserProfileRepository : GenericRepository<UserProfile>, ICustomBindingListRepository<UserProfile, ListFilter, Users_GetAll_Result>
    {
        #region Constructors
        private readonly MindCornersEntities _context;
        private readonly Guid _currentUserId;

        public UserProfileRepository()
        {
            _context = new MindCornersEntities();
        }
        public UserProfileRepository(Guid currentUserId)
        {
            _currentUserId = currentUserId;
            _context = new MindCornersEntities();
        }
        public UserProfileRepository(MindCornersEntities context, Guid currentUserId, Guid? currentUserOrganizationId) 
            : base(context, currentUserId, currentUserOrganizationId)
        {
            _currentUserId = currentUserId;
            _context = context;
        }
        #endregion

        public ViewType GetDefaultViewType()
        {
            return ViewType.AllApplications;
        }

        public IQueryable GetFilteredApplications(CriteriaOperator @where)
        {
            return _context.Users_GetAll().AppendWhere(new CriteriaToEFExpressionConverter(), where);
        }

        public int GetFilteredApplicationsCount(CriteriaOperator @where)
        {
            return GetFilteredApplications(where).Count();
        }

        public override IQueryable<UserProfile> GetAll()
        {
            var userItems = (from userProfile in Context.UserProfiles
                            join user in Context.AspNetUsers on userProfile.User_Id equals user.Id
                            select new
                            {
                                userProfile.Id,
                                userProfile.FirstName,
                                userProfile.LastName,
                                userProfile.ProfileImageString,
                                user.Email
                            }).ToList().Select(userItem => new UserProfile()
                            {
                                Id = userItem.Id,
                                FirstName = userItem.FirstName,
                                LastName = userItem.LastName,
                                ProfileImageString = userItem.ProfileImageString,
                                Email = userItem.Email
                            }).AsQueryable();

            return userItems;
        }

        public override UserProfile GetById(Guid id)
        {
            var userItem = (from userProfile in Context.UserProfiles
                join user in Context.AspNetUsers on userProfile.User_Id equals user.Id
                where userProfile.Id == id
                select new
                {
                    userProfile.Id,
                    userProfile.FirstName,
                    userProfile.LastName,
                    userProfile.ProfileImageString,
                    user.Email
                    
                }).FirstOrDefault();

            if (userItem != null)
            {
                return new UserProfile()
                {
                    Id = userItem.Id,
                    FirstName = userItem.FirstName,
                    LastName = userItem.LastName,
                    ProfileImageString = userItem.ProfileImageString,
                    Email = userItem.Email,
                    FullName = string.Format("{0} {1}", userItem.FirstName, userItem.LastName)
                };
            }

            return null;
        }

        public UserProfile GetByIdForUpdat(Guid id)
        {
            return Context.UserProfiles.FirstOrDefault(p => p.DateDeleted == null && p.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Common.Code.CoreRepositories;

namespace MindCorners.Common.Model
{
    public class NotificationRepository : GenericRepository<Notification>
    {

        private readonly MindCornersEntities _context;
        private readonly Guid _currentUserId;
        public NotificationRepository(MindCornersEntities context, Guid currentUserId, Guid? currentUserOrganizationId, bool createObjectSet = true) : base(context, currentUserId, currentUserOrganizationId, createObjectSet)
        {
            _context = context;
            _currentUserId = currentUserId;
        }
        public NotificationRepository()
        {
            _context = new MindCornersEntities();
        }

        public List<Models.Notification> Get(Guid userId, int skip, int take)
        {
            var result = _context.Notifications_GetAllByUser(userId, take, skip);
            var list = result.Select(p => new Models.Notification()
            {
                Id = p.Id,
                SourceId= p.SourceId,
                Type = p.Type,
                Text = p.Body,
                DateCreated= p.DateCreated,
                ReadDate= p.ReadDate,
                UserCreatorFullName= p.UserCreatorFullName,
                UserProfileImageName= p.UserProfileImageName
            }).ToList();
            
            return list;
        }

        public int GetCountUnread(Guid userId)
        {
            return _context.Notifications.Count(p =>p.DateDeleted == null && p.UserId == userId && !p.ReadDate.HasValue);
        }
    }
}

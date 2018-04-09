using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Common.Code;
using MindCorners.Common.Code.CoreRepositories;
using MindCorners.Common.Code.Enums;

namespace MindCorners.Common.Model
{
    public class CircleRepository :  GenericRepository<Circle>
    {
        private readonly MindCornersEntities _context;
        private readonly Guid _currentUserId;
        public CircleRepository(MindCornersEntities context, Guid currentUserId, Guid? currentUserOrganizationId, bool createObjectSet = true) : base(context, currentUserId, currentUserOrganizationId, createObjectSet)
        {
            _context = context;
            _currentUserId = currentUserId;
        }
        public CircleRepository()
        {
            _context = new MindCornersEntities();
        }

        public void AddMainPersonToCircleUser(Guid circleId, Guid mainUserId)
        {
            _context.CircleUsers.Add(new CircleUser()
            {
                Id = Guid.NewGuid(),
                CircleId = circleId,
                IsMainPerson = true,
                UserId = mainUserId,
                State = (int)InvitationStates.Pending,
                StateDate = DateTime.Now,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                CreatorId = _currentUserId,
                ModifierId = _currentUserId
            });
        }

        public void SynchCircleUsers(Guid circleId, List<Guid> selectedUserIds, string circleName,  Guid circleCreatorId)
        {
            var alreadyselectedUsers =
                _context.CircleUsers.Where(p => p.DateDeleted == null && p.CircleId == circleId && !p.IsMainPerson).Select(p=> p.Id).ToList();

            var added = selectedUserIds.Except(alreadyselectedUsers).ToList(); 
            var deleted = alreadyselectedUsers.Except(selectedUserIds).ToList();

            foreach (var addedItem in added)
            {
                var newId = Guid.NewGuid();
                _context.CircleUsers.Add(new CircleUser()
                {
                    Id = newId,
                    CircleId = circleId,
                    IsMainPerson = false,
                    UserId = addedItem,
                    State = (int) InvitationStates.Pending,
                    StateDate = DateTime.Now,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    CreatorId = _currentUserId,
                    ModifierId = _currentUserId
                });

                var senderUser = _context.UserProfiles.FirstOrDefault(p =>p.DateDeleted == null && p.Id == circleCreatorId);
               // Utilities.AddNotification(circleCreatorId, addedItem, newId, (int)MindCorners.Models.Enums.NotificationTypes.InvitationToContactsRecieved,
               //     string.Format("You are added to {0} circle created by {1}.", circleName, senderUser.FullName));

            }
            foreach (var deletedItem in deleted)
            {
                var circleUser = _context.CircleUsers.FirstOrDefault(p => p.Id == deletedItem);
                if (circleUser != null)
                {
                    circleUser.DateDeleted = DateTime.Now;
                    circleUser.ModifierId = _currentUserId;
                }
            }
        }
        public void LeaveCircle(Guid circleId, Guid userId)
        {
            var circleUser =
                _context.CircleUsers.FirstOrDefault(
                    p => p.DateDeleted == null && p.CircleId == circleId && p.UserId == userId);
            if (circleUser != null)
            {
                circleUser.DateDeleted = DateTime.Now;
                circleUser.ModifierId = userId;
            }
        }
    }
}

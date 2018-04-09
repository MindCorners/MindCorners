using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Common.Code;
using MindCorners.Common.Code.CoreRepositories;
using MindCorners.Common.Code.Enums;
using MindCorners.Models.Results;

namespace MindCorners.Common.Model
{
    public class InvitationRepository : GenericRepository<Invitation>
    {
        private readonly MindCornersEntities _context;
        private readonly Guid _currentUserId;

        public InvitationRepository(MindCornersEntities context, Guid currentUserId, Guid? currnetUserOrganizationId, bool createObjectSet = true) : base(context, currentUserId, currnetUserOrganizationId, createObjectSet)
        {
            _context = context;
            _currentUserId = currentUserId;
        }
        public InvitationRepository()
        {
            _context = new MindCornersEntities();
        }
        public IdResult GetInvitationByAuthenticationCode(string code, string email)
        {
            var invitation = _context.Invitations.FirstOrDefault(
                p => p.ActivationCode.ToLower() == code.ToLower() &&
                     p.Email.ToLower() == email.ToLower() &&
                     //p.State == (int) InvitationStates.Unvalid && p.ExpireDate > DateTime.Now &&
                     p.DateDeleted == null);


            if (invitation != null)
            {
                if (invitation.State == (int) InvitationStates.Accepted)
                {
                    return new IdResult()
                    {
                        ErrorMessage = "Authentication code is already activated.",
                        IsOk = false
                    };
                }
                if(invitation.ExpireDate < DateTime.Now)
                {
                    return new IdResult()
                    {
                        ErrorMessage = "Authentication code is expired",
                        IsOk = false
                    };
                }
                return new IdResult()
                {   
                    Id = invitation.Id,
                    IsOk = true
                };
            }
            return new IdResult()
            {
                ErrorMessage = "No such authentication code or Email",
                IsOk = false
            };
        }

        public BoolResult GetInvitationDoesNotExistsByEmail(string email)
        {
            var invitation = _context.Invitations.FirstOrDefault(
                p => p.Email.ToLower() == email.ToLower() &&
                     //p.State == (int) InvitationStates.Unvalid && p.ExpireDate > DateTime.Now &&
                     p.DateDeleted == null);
            if (invitation != null)
            {
                if (invitation.State == (int)InvitationStates.Accepted)
                {
                    return new BoolResult()
                    {
                        ErrorMessage = "User is already invited and accepted",
                        IsOk = false
                    };
                }
                if (invitation.ExpireDate > DateTime.Now)
                {
                    return new BoolResult()
                    {
                        ErrorMessage = "User is already invited but did not accepted invitation",
                        IsOk = false
                    };
                }
            }
            return new BoolResult()
            {   
                IsOk = true
            };
        }

        public Invitation Create(Guid roleId, Guid? organizationId, string email, out string outActivationCode)
        {
            var activationCode = Utilities.GetActivationCode();
            var invitation = new Invitation()
            {
                Id = Guid.NewGuid(),
                RoleId = roleId,
                OrganizationId = organizationId,
                InviatorId = CurrentUserId,
                Email = email,
                State = (byte)InvitationStates.Pending,
                StateDate = DateTime.Now,
                ActivationCode = activationCode,
                ExpireDate = DateTime.Now.AddHours(48),
            };
            outActivationCode = activationCode;
            return base.Create(invitation);
        }
    }
}

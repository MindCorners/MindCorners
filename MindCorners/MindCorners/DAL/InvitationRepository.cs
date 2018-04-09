using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Code;
using MindCorners.Models;
using MindCorners.Models.Results;

namespace MindCorners.DAL
{
    public class InvitationRepository : RestService
    {
        //public async Task<IdResult> CheckAuthenticationCode(string email, string code)
        //{
        //    return await GetResult<IdResult>(string.Format("Account/CheckAuthenticationCode?email={0}&code={1}", email, code));
        //}

        public async Task<IdResult> CreateInvitation(Invitation invitation)
        {
            return await PostResult<IdResult, Invitation>("api/Invitation/Create", invitation);
        }
        public async Task<IdResult> ChangeInvitationState(Contact contact)
        {
            return await PostResult<IdResult, Contact>("api/Invitation/ChangeInvitationState", contact);
        }
    }
}

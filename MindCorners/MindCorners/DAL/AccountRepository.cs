using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Code;
using MindCorners.Models;
using MindCorners.Models.Results;
using Newtonsoft.Json;

namespace MindCorners.DAL
{
    public class AccountRepository : RestService
    {
        public async Task<User> LoginUser(string email, string password)
        {
            User currentUser = await GetResult<User>(string.Format("Account/GetUser?userName={0}&password={1}", email,password));
            return currentUser;
        }

        public async Task<IdResult> CheckAuthenticationCode(string email, string code)
        {
            return await GetResult<IdResult>(string.Format("Account/CheckAuthenticationCode?email={0}&code={1}", email, code));
        }

        public async Task<ObjectResult<User>> RegisterUser(UserRegister user)
        {
            return await PostResult<ObjectResult<User>, UserRegister>("api/Account/RegisterUser", user);
        }

        public async Task<FilePathResult> SaveProfilePhoto(UserProfileImage profileImage)
        {
            return await PostResult<FilePathResult, UserProfileImage>("api/Account/SaveProfilePhoto", profileImage);
        }

        public async Task<User> LoginExternalUser(string provider, string access_token, bool loadExrUser = false)
        {
			User currentUser = await GetResult<User>(string.Format("Account/LoginExternalUser?provider={0}&accessToken={1}{2}", provider, access_token, loadExrUser ? "&loadExtUser=true" : null));
            return currentUser;
        }
        public async Task<ObjectResult<User>> RegisterExternalUser(RegisterExternalBindingModel user)
        {
            return await PostResult<ObjectResult<User>, RegisterExternalBindingModel>("api/Account/RegisterExternal", user);
        }

    }
}

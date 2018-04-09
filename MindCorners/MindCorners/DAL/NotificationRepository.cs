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
    public class NotificationRepository : RestService
    {  
        public async Task<List<Notification>> Get(int skip, int take)
        {
            return await GetResult<List<Notification>>(string.Format("Notification/Get?skip={0}&take={1}",skip, take));
        }
        public async Task<int> GetUnreadCount()
        {
            return await GetResult<int>(string.Format("Notification/GetUnreadCount"));
        }
        public async Task<Notification> GetItem(Guid id)
        {
            return await GetResult<Notification>(string.Format("Notification/GetItem?id={0}", id));
        }
        public async Task<IdResult> Submit(Notification notification)
        {
            return await PostResult<IdResult, Notification>("api/Notification/Submit", notification);
        }
        public async Task<IdResult> UpdateRead(Notification notification)
        {
            return await PostResult<IdResult, Notification>("api/Notification/UpdateRead", notification);
        }
        public async Task<BoolResult> Delete(Notification notification)
        {
            return await PostResult<BoolResult, Notification>("api/Notification/Delete", notification);
        }
    }
}

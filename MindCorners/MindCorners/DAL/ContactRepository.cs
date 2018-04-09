using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Code;
using MindCorners.Helpers;
using MindCorners.Models;

namespace MindCorners.DAL
{
    public class ContactRepository : RestService
    {
        public async Task<List<Contact>> GetAll(Guid userId)
        {
            return await GetResult<List<Contact>>(string.Format("Contact/GetAll?userId={0}", userId));
        }

        public async Task<ObservableCollection<Contact>> GetAllObservableCollection(Guid userId)
        {
            var result = await GetAll(Settings.CurrentUserId);
            if (result != null)
            {
                return new ObservableCollection<Contact>(result.Select(p => new Contact
                {
                    Email = p.Email,
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    IsActivated = p.IsActivated,
                    ProfileImageString = p.ProfileImageString
                }));
            }
            return null;
        }

        public async Task<List<Contact>> GetAllWithSelectedForCircle(Guid userId, Guid circleId)
        {
            var result = await GetResult<List<Contact>>(string.Format("Contact/GetAllWithSelectedForCircle?circleId={0}", circleId));
            return result;
        }
        public async Task<List<Contact>> GetOnlySelectedForCircle(Guid circleId)
        {
            var result = await GetResult<List<Contact>>(string.Format("Contact/GetOnlySelectedForCircle?circleId={0}", circleId));
            return result;
        }

        public async Task<Contact> GetById(Guid userId)
        {
            return await GetResult<Contact>(string.Format("Contact/GetById?userId={0}", userId));
        }
        public async Task<Contact> GetByIdWithState(Guid userId)
        {
            return await GetResult<Contact>(string.Format("Contact/GetByIdWithState?userId={0}", userId));
        }
    }
}

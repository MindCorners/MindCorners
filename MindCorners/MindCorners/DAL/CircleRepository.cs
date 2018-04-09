using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Code;
using MindCorners.Helpers;
using MindCorners.Models;
using MindCorners.Models.Results;

namespace MindCorners.DAL
{
    public class CircleRepository : RestService
    {

        public async Task<Circle> GetItem(Guid circleId)
        {
            return await GetResult<Circle>(string.Format("Circle/GetById?circleId={0}", circleId));
        }
        public async Task<List<Circle>> GetAll(Guid userId)
        {
            return await GetResult<List<Circle>>(string.Format("Circle/GetAll?userId={0}", userId));
        }

        public async Task<ObservableCollection<Circle>> GetAllObservableCollection(Guid userId)
        {
            var result = await GetAll(Settings.CurrentUserId);
            if (result != null)
            {
                return new ObservableCollection<Circle>(result.Select(p => new Circle
                {
                    Id = p.Id,
                    Name = p.Name,
                    IsCreatedByUser = p.IsCreatedByUser
                }));
            }
            return null;
        }

        public async Task<IdResult> Submit(Circle circle)
        {
            return await PostResult<IdResult, Circle>("api/Circle/Submit", circle);
        }

        public async Task<BoolResult> Leave(Circle circle)
        {
            return await PostResult<BoolResult, Circle>("api/Circle/Leave", circle);
        }

        public async Task<BoolResult> Delete(Circle circle)
        {
            return await PostResult<BoolResult, Circle>("api/Circle/Delete", circle);
        }
    }
}

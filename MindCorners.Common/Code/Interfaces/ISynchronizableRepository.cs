using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Common.Code.CoreRepositories;

namespace MindCorners.Common.Code.Interfaces
{
    public interface ISynchronizableRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAllByParent(Guid parentObjectId, int parentObjectTypeId, int type);
        IQueryable<TEntity> GetAllByParent(Guid parentObjectId, int parentObjectTypeId);
        void DeleteAllByParent(Guid parentObjectId, int parentObjectTypeId, int type);

        List<TEntity> Synchronize(List<TEntity> modelsFromForm, Guid parentObjectId, int parentObjectTypeId, int type);
        List<TEntity> Synchronize<TEqualityComparer>(List<TEntity> modelsFromForm, Guid parentObjectId, int parentObjectTypeId, int type)
            where TEqualityComparer : IEqualityComparer<TEntity>, new();

        List<TEntity> Synchronize(List<TEntity> modelsFromForm, Guid parentObjectId, int parentObjectTypeId);
        List<TEntity> Synchronize<TEqualityComparer>(List<TEntity> modelsFromForm, Guid parentObjectId, int parentObjectTypeId)
            where TEqualityComparer : IEqualityComparer<TEntity>, new();
    }

}

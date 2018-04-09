using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Common.Code.Interfaces;
using MindCorners.Common.Model;

namespace MindCorners.Common.Code.CoreRepositories
{
    public class SynchronizableGenericRepository<TEntity> : GenericRepository<TEntity>, ISynchronizableRepository<TEntity> where TEntity : class, ISynchronizableEntity
    {
        public SynchronizableGenericRepository(MindCornersEntities context, Guid currentUserId, Guid? currentUserOrganizationId) : base(context, currentUserId, currentUserOrganizationId)
        {
        }
        public virtual IQueryable<TEntity> GetAllByParent(Guid parentObjectId, int parentObjectTypeId, int type)
        {
            return GetAll().Where(x => x.ParentObjectId == parentObjectId && x.ParentObjectTypeId == parentObjectTypeId && x.Type == type);
        }
        public virtual IQueryable<TEntity> GetAllByParent(Guid parentObjectId, int parentObjectTypeId)
        {
            return GetAll().Where(x => x.ParentObjectId == parentObjectId && x.ParentObjectTypeId == parentObjectTypeId);
        }
        public virtual void DeleteAllByParent(Guid parentObjectId, int parentObjectTypeId, int type)
        {
            var list =
                GetAll()
                    .Where(
                        x =>
                            x.ParentObjectId == parentObjectId && x.ParentObjectTypeId == parentObjectTypeId &&
                            x.Type == type);
            if (list != null)
            {
                foreach (var item in list)
                {
                    item.DateDeleted = DateTime.Now;
                    item.ModifierId = CurrentUserId;
                }
            }
        }
        public virtual IQueryable<TEntity> GetAllByParentType(int parentObjectTypeId, int type)
        {
            return GetAll().Where(x => x.ParentObjectTypeId == parentObjectTypeId && x.Type == type);
        }
        public virtual TEntity GetLastByIdAndParentType(Guid parentObjectId, int parentObjectTypeId, int type)
        {
            return GetAll().OrderByDescending(p => p.DateCreated).FirstOrDefault(x => x.ParentObjectTypeId == parentObjectTypeId && x.Type == type && x.ParentObjectId == parentObjectId);
        }
        public virtual IQueryable<TEntity> GetAllByParentType(int parentObjectTypeId)
        {
            return GetAll().Where(x => x.ParentObjectTypeId == parentObjectTypeId);
        }
        public void Create(TEntity model, Guid parentObjectId, int parentObjectTypeId, int type)
        {
            model.ParentObjectId = parentObjectId;
            model.ParentObjectTypeId = parentObjectTypeId;
            model.Type = type;
            Create(model);
        }
        public void Create(TEntity model, Guid parentObjectId, int parentObjectTypeId)
        {
            model.ParentObjectId = parentObjectId;
            model.ParentObjectTypeId = parentObjectTypeId;
            Create(model);
        }
        public void CreateMultiple(IEnumerable<TEntity> models, Guid parentObjectId, int parentObjectTypeId, int type)
        {
            foreach (var model in models)
            {
                Create(model, parentObjectId, parentObjectTypeId, type);
            }
        }
        public void CreateMultiple(IEnumerable<TEntity> models, Guid parentObjectId, int parentObjectTypeId)
        {
            foreach (var model in models)
            {
                Create(model, parentObjectId, parentObjectTypeId);
            }
        }
        public virtual List<TEntity> Synchronize(List<TEntity> modelsFromForm, Guid parentObjectId, int parentObjectTypeId, int type)
        {
            return Synchronize<EntityComparer<TEntity>>(modelsFromForm, parentObjectId, parentObjectTypeId, type);
        }
        public virtual List<TEntity> Synchronize<TEqualityComparer>(List<TEntity> modelsFromForm, Guid parentObjectId, int parentObjectTypeId, int type)
            where TEqualityComparer : IEqualityComparer<TEntity>, new()
        {
            modelsFromForm = modelsFromForm ?? new List<TEntity>();
            var modelsFromDatabase = GetAllByParent(parentObjectId, parentObjectTypeId, type).ToList();
            var comparer = new TEqualityComparer();

            modelsFromDatabase.Except(modelsFromForm, comparer).ToList().ForEach(Delete);
            modelsFromForm.Except(modelsFromDatabase, comparer).ToList().ForEach(model => Create(model, parentObjectId, parentObjectTypeId, type));

            return modelsFromDatabase.Intersect(modelsFromForm, comparer).ToList();
        }
        public virtual List<TEntity> Synchronize(List<TEntity> modelsFromForm, Guid parentObjectId, int parentObjectTypeId)
        {
            return Synchronize<EntityComparer<TEntity>>(modelsFromForm, parentObjectId, parentObjectTypeId);
        }
        public virtual List<TEntity> Synchronize<TEqualityComparer>(List<TEntity> modelsFromForm, Guid parentObjectId, int parentObjectTypeId)
            where TEqualityComparer : IEqualityComparer<TEntity>, new()
        {
            modelsFromForm = modelsFromForm ?? new List<TEntity>();
            var modelsFromDatabase = GetAllByParent(parentObjectId, parentObjectTypeId).ToList();
            var comparer = new TEqualityComparer(); modelsFromDatabase.Except(modelsFromForm, comparer).ToList().ForEach(Delete);
            modelsFromForm.Except(modelsFromDatabase, comparer).ToList().ForEach(model => Create(model, parentObjectId, parentObjectTypeId));

            return modelsFromDatabase.Intersect(modelsFromForm, comparer).ToList();
        }
    }
}

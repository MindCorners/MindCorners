using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Common.Code.Interfaces;
using MindCorners.Common.Model;

namespace MindCorners.Common.Code.CoreRepositories
{
    public class GenericRepository<TEntity> : DisposableRepository, IRepository<TEntity> where TEntity : class, IEntity
    {
        protected ObjectSet<TEntity> _objectSet;
        
        public GenericRepository(MindCornersEntities context, Guid currentUserId, Guid? currentUserOrganizationId, bool createObjectSet = true)
            : base(currentUserId, context, currentUserOrganizationId)
        {
            if (createObjectSet)
            {
                _objectSet = ((IObjectContextAdapter)Context).ObjectContext.CreateObjectSet<TEntity>();
            }

            ((IObjectContextAdapter)Context).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }
        public GenericRepository()
        {  
            ((IObjectContextAdapter)Context).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }
        public virtual IQueryable<TEntity> GetAll()
        {
            return _objectSet.Where(x => x.DateDeleted == null);
        }

        public virtual IQueryable<TEntity> GetAllIncludingDeleted()
        {
            return _objectSet;
        }
        
        public virtual TEntity GetById(Guid id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }
        public virtual TEntity Create(TEntity model)
        {
            model.Id = Guid.NewGuid();
            model.DateCreated = model.DateModified = DateTime.Now;
            model.CreatorId = model.ModifierId = CurrentUserId;
            _objectSet.AddObject(model);
            return model;
        }
        public virtual void Update(TEntity model)
        {
            //Context.Entry(model).State = EntityState.Modified;
            model.DateModified = DateTime.Now;
            model.ModifierId = CurrentUserId;
        }
        public virtual void Delete(Guid id)
        {
            var model = GetById(id);
            Delete(model);
        }
        public virtual void Delete(TEntity model)
        {
            model.DateModified = DateTime.Now;
            model.ModifierId = CurrentUserId;
            model.DateDeleted = DateTime.Now;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.Common.Code.CoreRepositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAllIncludingDeleted();
        TEntity GetById(Guid id);
        TEntity Create(TEntity model);
        void Update(TEntity model);
        void Delete(Guid id);
        void SaveChanges();
    }
}

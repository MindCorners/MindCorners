using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Common.Code.Interfaces;

namespace MindCorners.Common.Code.CoreRepositories
{
    public class EntityComparer<TEntity> : IEqualityComparer<TEntity> where TEntity : IEntity
    {
        public bool Equals(TEntity firstModel, TEntity secondModel)
        {
            if (firstModel.Id == Guid.Empty || secondModel.Id == Guid.Empty)
                return false;
            return firstModel.Id == secondModel.Id;
        }

        public int GetHashCode(TEntity model)
        {
            return model.Id.GetHashCode();
        }
    }
}

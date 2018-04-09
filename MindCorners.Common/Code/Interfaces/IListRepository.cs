using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using MindCorners.Common.Code.CoreRepositories;
using MindCorners.Common.Model;

namespace MindCorners.Common.Code.Interfaces
{
    public interface IListRepository<TApplication, TListFilter> : IRepository<TApplication>
       where TApplication : class, IEntity
       where TListFilter : ListFilter
    {
        ViewType GetDefaultViewType();
        IQueryable<TApplication> GetFilteredApplications(TListFilter filter, ViewType viewType, UserProfile currentUser);
        IQueryable GetFilteredApplications(CriteriaOperator where);
    }
}

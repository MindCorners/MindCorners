using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Internal;
using DevExpress.Web.Internal;
using MindCorners.Common.Code;
using MindCorners.Common.Code.CoreRepositories;
using MindCorners.Common.Code.Enums;
using MindCorners.Common.Code.Interfaces;

namespace MindCorners.Common.Model
{
    public class OrganizationRepository : GenericRepository<Organization>, ICustomBindingListRepository<Organization, ListFilter, Organizations_GetAll_Result>
    {
        #region Constructors
        private readonly MindCornersEntities _context;
        private readonly Guid _currentUserId;
        
        public OrganizationRepository()
        {
            _context = new MindCornersEntities();
        }
        public OrganizationRepository(Guid currentUserId)
        {
            _currentUserId = currentUserId;
            _context = new MindCornersEntities();
        }
        public OrganizationRepository(MindCornersEntities context, Guid currentUserId, Guid? currentUserOrganizationId)
            : base(context, currentUserId, currentUserOrganizationId)
        {
            _currentUserId = currentUserId;
            _context = context;
        }
        #endregion

        #region DB Methods
        public IQueryable<Organization> GetAllForList()
        {
            var result = _context.Organizations
                .Where(x => x.DateDeleted == null)
                .OrderBy(x => x.Name);
            return result;
        }
        public Dictionary<Guid, string> GetAllForListFilter()
        {
            var result = _context.Organizations
                .Where(x => x.DateDeleted == null)
                .OrderBy(x => x.Name).ToList().Select(p => new KeyValuePair<Guid, string>(p.Id, p.Name));
            return result.ToDictionary(k => k.Key, v => v.Value); 
        }

        public IQueryable<Organization> GetAllByType(int type, bool includeSystemEntity = false)
        {
            var result = _context.Organizations
                .Where(x => x.DateDeleted == null);

            return result.OrderBy(x => x.Name);
        }

        public Organization GetByUserId(Guid userId)
        {
            var user = _context.UserProfiles.Include("Organization").FirstOrDefault(p => p.Id == userId);
            return user == null ? null : user.Organization;
        }
        
        #endregion
        

        public IQueryable GetFilteredApplications(CriteriaOperator @where)
        {
            return _context.Organizations_GetAll().AppendWhere(new CriteriaToEFExpressionConverter(), where);
        }

        public int GetFilteredApplicationsCount(CriteriaOperator @where)
        {
            var result = GetFilteredApplications(where).Count();

            return result;
        }

        public ViewType GetDefaultViewType()
        {
            return ViewType.AllApplications;
        }
        
        #region Disposable

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.Common.Model
{
    public class GridStateRepository : IDisposable
    {
        private readonly MindCornersEntities _context;

        public GridStateRepository()
        {
            _context = new MindCornersEntities();
        }

        public GridStateRepository(MindCornersEntities context)
        {
            _context = context;
        }

        public GridState Load(Guid userId, int moduleNameId)
        {
            var result = _context.GridStates.FirstOrDefault(x => x.UserProfile.Id == userId && x.ModuleId == moduleNameId);
            return result;
        }

        public void Insert(Guid userId, int moduleNameId, string layoutData, bool showFilter)
        {
            Insert(userId, moduleNameId, layoutData,  showFilter, null);
        }
        
        public void Insert(Guid userId, int moduleNameId, string layoutData, bool showFilter, string serializedFilter)
        {
            var state = Load(userId, moduleNameId);
            if (state != null)
            {
                state.UserId = userId;
                state.LayoutData = layoutData;
                state.ModuleId = moduleNameId;
                state.ShowFilter = showFilter;
                state.Filter = serializedFilter;
            }
            else
            {
                var newState = new GridState
                {
                    UserId = userId,
                    LayoutData = layoutData,
                    ModuleId = moduleNameId,
                    ShowFilter = showFilter,
                    Filter = serializedFilter
                };
                _context.GridStates.Add(newState);
            }
        }

        public void Delete(Guid userId, int moduleNameId)
        {
            var state = Load(userId, moduleNameId);
            if (state != null)
                _context.GridStates.Remove(state);
        }

        public void Save()
        {
            _context.SaveChanges();
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

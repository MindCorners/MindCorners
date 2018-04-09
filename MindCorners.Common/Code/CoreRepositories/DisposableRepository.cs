using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Common.Model;

namespace MindCorners.Common.Code.CoreRepositories
{
    public class DisposableRepository : IDisposable
    {
        #region Protected Members

        /// <summary>
        /// The context object for the database
        /// </summary>
        protected MindCornersEntities Context;

        /// <summary>
        /// Current session user id
        /// </summary>
        protected readonly Guid CurrentUserId;

        protected readonly Guid? CurrentUserOrganizationId;
        #endregion

        #region Constructors

        public DisposableRepository(Guid currentUserId, MindCornersEntities context, Guid? currentUserOrganizationId)
        {
            CurrentUserId = currentUserId;
            Context = context;
            CurrentUserOrganizationId = currentUserOrganizationId;
        }

        public DisposableRepository(Guid currentUserId)
        {
            CurrentUserId = currentUserId;
            Context = new MindCornersEntities();
        }

        public DisposableRepository()
        {
            Context = new MindCornersEntities();
        }

        #endregion

        /// <summary>
        /// Saves all context changes
        /// </summary>
        public virtual void SaveChanges()
        {
            Context.SaveChanges();
        }

        #region Disposable

        private bool _disposed;

        /// <summary>
        /// Releases all resources used by the WarrantManagement.DataExtract.Dal.ReportDataBase
        /// </summary>
        /// <param name="disposing">A boolean value indicating whether or not to dispose managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        /// <summary>
        /// Releases all resources used by the WarrantManagement.DataExtract.Dal.ReportDataBase
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using MindCorners.Common.Code;
using MindCorners.Common.Code.CoreRepositories;
using MindCorners.Common.Code.Enums;
using MindCorners.Common.Code.Interfaces;

namespace MindCorners.Common.Model
{
    public class MessageTemplateRepository : GenericRepository<MessageTemplate>, ICustomBindingListRepository<MessageTemplate, ListFilter, MessageTemplates_GetAll_Result>
    {
        #region Constructors
        private readonly MindCornersEntities _context;
        private readonly Guid _currentUserId;

        public MessageTemplateRepository()
        {
            _context = new MindCornersEntities();
        }
        public MessageTemplateRepository(Guid currentUserId)
        {
            _currentUserId = currentUserId;
            _context = new MindCornersEntities();
        }
        public MessageTemplateRepository(MindCornersEntities context, Guid currentUserId, Guid? currentUserOrganizationId) 
            : base(context, currentUserId, currentUserOrganizationId)
        {
            _currentUserId = currentUserId;
            _context = context;
        }
        #endregion

        public MessageTemplate GetMessageTemplateByType(MessageTemplateTypes type)
        {
            return GetAll().FirstOrDefault(p => p.Type == (int) type);
        }

        public ViewType GetDefaultViewType()
        {
            return ViewType.AllApplications;
        }

        public IQueryable GetFilteredApplications(CriteriaOperator @where)
        {
            return _context.MessageTemplates_GetAll().AppendWhere(new CriteriaToEFExpressionConverter(), where);
        }

        public int GetFilteredApplicationsCount(CriteriaOperator @where)
        {
            return GetFilteredApplications(where).Count();
        }
    }
}

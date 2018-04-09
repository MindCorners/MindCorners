using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Common.Code.CoreRepositories;

namespace MindCorners.Common.Model
{
    public class TextTemplateRepository :   GenericRepository<TextTemplate>
    {
        private readonly MindCornersEntities _context;
        private readonly Guid _currentUserId;
        public TextTemplateRepository(MindCornersEntities context, Guid currentUserId, Guid? currentUserOrganizationId, bool createObjectSet = true) : base(context, currentUserId, currentUserOrganizationId, createObjectSet)
        {
            _context = context;
            _currentUserId = currentUserId;
        }
        public TextTemplateRepository()
        {
            _context = new MindCornersEntities();
        }
    }
}

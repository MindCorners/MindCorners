using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.Common.Code.Interfaces
{
    public interface IEntity
    {
        Guid Id { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }
        Guid CreatorId { get; set; }
        Guid ModifierId { get; set; }
        DateTime? DateDeleted { get; set; }
    }

}

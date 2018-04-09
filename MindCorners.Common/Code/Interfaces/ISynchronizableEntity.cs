using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.Common.Code.Interfaces
{
    public interface ISynchronizableEntity : IEntity
    {
        Guid ParentObjectId { get; set; }
        int ParentObjectTypeId { get; set; }
        int Type { get; set; }
    }
}

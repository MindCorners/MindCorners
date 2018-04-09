using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Common.Code.Interfaces;

namespace MindCorners.Common.Model
{
    public partial class UserProfile : IEntity
    {
        public string ContactName { get; set; }
        public string Email { get; set; }
        public byte? ContactState { get; set; }
        public bool IsSelectedInCircle { get; set; }
    }
}

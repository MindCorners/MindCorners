using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.Models
{
   public class Invitation
    {
        public Guid Id { get; set; }
        public Guid InvitorId { get; set; }
        public string Email { get; set; }
        public bool IsAccepted { get; set; }
    }
}

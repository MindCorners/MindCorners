using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.Models
{
    public class TextTemplate : BaseSelectableModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.Models.Results
{
    public class ObjectResult<T> : Result
    {
        public T ReturnedObject { get; set; }
    }
}

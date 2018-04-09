using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MindCorners.Models.Annotations;

namespace MindCorners.Models
{
    public class Circle  :BaseSelectableModel
    {
      
        public string Name { get; set; }
        public bool IsCreatedByUser { get; set; }
        public List<Contact> SelectedContacts { get; set; }
        //public bool IsSelected { get; set; }
      
    }
}

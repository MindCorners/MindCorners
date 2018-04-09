using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindCorners.Models
{
    public class BaseSelectableModel : BaseModel
    {
        public Guid Id { get; set; }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;

                OnPropertyChanged();
            }
        }
    }
}

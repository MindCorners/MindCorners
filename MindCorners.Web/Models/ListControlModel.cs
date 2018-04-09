using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.XtraExport.Xls;

namespace MindCorners.Web.Models
{
    public class ListControlModel
    {
        private Guid? id = Guid.Empty;
        public Guid? Id
        {
            get { return id; }
            set { id = value; }
        }

        private bool canAdd = true;
        public bool CanAdd
        {
            get { return canAdd; }
            set { canAdd = value; }
        }

        private bool canEdit = true;
        public bool CanEdit
        {
            get { return canEdit; }
            set { canEdit = value; }
        }

        private bool canDelete = true;
        public bool CanDelete
        {
            get { return canDelete; }
            set { canDelete = value; }
        }



    }
}
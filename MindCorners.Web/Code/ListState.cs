using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using MindCorners.Common.Code;
using ViewType = MindCorners.Common.Code.ViewType;

namespace MindCorners.Web.Code
{
    public class ListState<TListFilter> where TListFilter : ListFilter
    {
        public TListFilter ListFilter { get; set; }
        public string LayoutData { get; set; }
        public bool ShowFilter { get; set; }
        public ViewType? ViewType { get; set; }
        public ListState()
        {
            LayoutData = LayoutData ?? string.Empty;
            ShowFilter = true;
        }
        public string FilterExpression { get; set; }
        public ReadOnlyCollection<GridViewColumnState> SortExpression { get; set; }
    }
}
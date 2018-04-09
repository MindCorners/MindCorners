using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Export;
using DevExpress.Web.Mvc;
using DevExpress.XtraPrinting;

namespace MindCorners.Web.Code.Helpers
{
    public delegate ActionResult ExportMethod(GridViewSettings settings, object dataObject);

    public static class GridViewHelper
    {
        private static List<ExportType> _exportTypes;
        public static List<ExportType> ExportTypes
        {
            get { return _exportTypes ?? (_exportTypes = CreateExportTypes()); }
        }

        private static List<ExportType> CreateExportTypes()
        {
            var exportTypes = new List<ExportType>
                                  {   
                                      new ExportType {Format = ExportFormat.Xlsx, Method = GridViewExtension.ExportToXlsx}
                                  };

            return exportTypes;
        }
    }
}
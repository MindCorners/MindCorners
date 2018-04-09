using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.XtraPrinting;

namespace MindCorners.Web.Code.Helpers
{
    public class ExportType
    {
        public ExportFormat Format { get; set; }
        public ExportMethod Method { get; set; }
    }
}
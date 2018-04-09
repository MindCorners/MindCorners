using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace MindCorners.Common.Code.Helpers
{
    public class LogHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void WriteError(Exception ex)
        {
            Logger.Error(ex.InnerException ?? ex);
        }

        public static void WriteInfoLog(string message)
        {
            Logger.Info(message);
        }
    }
}

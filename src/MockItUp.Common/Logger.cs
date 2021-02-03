using log4net;
using System;

namespace MockItUp.Common
{
    public static class Logger
    {
        private static ILog _log = LogManager.GetLogger("MockItUp");

        public static void LogError(string message, Exception ex)
        {
            _log.Error(message, ex);
        }

        public static void LogInfo(string message)
        {
            _log.Info(message);
        }
    }
}

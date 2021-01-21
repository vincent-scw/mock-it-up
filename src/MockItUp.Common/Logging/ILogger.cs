using System;

namespace MockItUp.Common.Logging
{
    public interface ILogger
    {
        void Log(string content, LogLevel logLevel = LogLevel.Info);
    }
}

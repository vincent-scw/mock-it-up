using System;
using System.Collections.Generic;
using System.Text;

namespace MockItUp.Common.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string content, LogLevel logLevel = LogLevel.Info)
        {
            Console.WriteLine(content);
        }
    }
}

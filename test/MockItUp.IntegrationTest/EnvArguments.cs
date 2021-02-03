using System;
using System.Collections.Generic;
using System.Text;

namespace MockItUp.IntegrationTest
{
    static class EnvArguments
    {
        public static string GetServiceUrl(string serviceName)
        {
            return Environment.GetEnvironmentVariable($"SERVICE_{serviceName.ToUpper()}");
        }
    }
}

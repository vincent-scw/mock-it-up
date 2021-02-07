using System;

namespace MockItUp.IntegrationTest
{
    static class EnvArguments
    {
        public static string GetServiceUrl(string serviceName)
        {
            return Environment.GetEnvironmentVariable($"SERVICE_{serviceName.ToUpper()}");
        }

        public static string GetCtlService()
        {
            return Environment.GetEnvironmentVariable("CTL_SERVICE");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MockItUp.IntegrationTest
{
    static class EnvArguments
    {
        public static string GetServiceUrl(string serviceName)
        {
            if (serviceName == "order")
                return "http://localhost:5000";
            else
                return "http://localhost:5010";
        }
    }
}

using MockItUp.Common.Logging;
using MockItUp.HttpHost;
using System;
using System.Threading.Tasks;

namespace MockItUp.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpServer = new HttpServer(new ConsoleLogger());
            await httpServer.StartAsync(new System.Threading.CancellationToken());

            System.Console.WriteLine("Press any key to close");
            System.Console.ReadKey();
        }
    }
}

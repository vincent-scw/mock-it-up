using Microsoft.Extensions.DependencyInjection;
using MockItUp.Common.Logging;
using MockItUp.Core.Contracts;
using MockItUp.HttpHost;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MockItUp.Console
{
    class Program
    {
        private static IServiceProvider _serviceProvider;

        static async Task Main(string[] args)
        {
            try
            {
                System.Console.WriteLine("Server starting...");
                RegisterServices();

                var httpServer = _serviceProvider.GetService<HttpServer>();
                var cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;
                await httpServer.StartAsync(cancellationToken);

                System.Console.WriteLine("Press any key to close");
                System.Console.ReadKey();

                cancellationTokenSource.Cancel();
            }
            finally
            {
                DisposeServices();
            }
        }

        private static void RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<HttpServer>();
            services.AddSingleton<ILogger, ConsoleLogger>();
            services.AddSingleton<HostConfiguration>((s) => new HostConfiguration { Hosts = new Dictionary<string, string>
            {
                { "order", "http://localhost:5000/" },
                { "shipment", "http://localhost:5010/" }
            }});

            _serviceProvider = services.BuildServiceProvider(true);
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
                return;

            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}

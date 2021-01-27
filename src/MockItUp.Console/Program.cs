using log4net;
using log4net.Config;
using log4net.Core;
using Microsoft.Extensions.DependencyInjection;
using MockItUp.Common.Contracts;
using MockItUp.Common.Logging;
using MockItUp.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MockItUp.Console
{
    class Program
    {
        private static IServiceProvider _serviceProvider;

        static async Task Main(string[] args)
        {
            var logRepo = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepo, new FileInfo("log4net.config"));

            try
            {
                System.Console.WriteLine("Server starting...");
                RegisterServices();

                var registry = _serviceProvider.GetService<ISpecRegistry>();
                registry.RegisterDirectory(@"C:\Projects\My\mock-it-up\src\MockItUp.IntegrationTest\specs");

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
            services.AddSingleton<HostConfiguration>((s) => new HostConfiguration { Hosts = new Dictionary<string, string>
            {
                { "order", "http://localhost:5000/" },
                { "shipment", "http://localhost:5010/" }
            }});
            services.AddSingleton<ISpecLoader, SpecLoader>();
            services.AddSingleton<ISpecRegistry, SpecRegistry>();

            services.AddSingleton<IMockProvider, Restful.RestfulMockProvider>();

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

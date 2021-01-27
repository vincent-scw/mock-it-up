using log4net;
using log4net.Config;
using Microsoft.Extensions.DependencyInjection;
using MockItUp.Common.Contracts;
using MockItUp.Core;
using System;
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

            while (true)
            {
                System.Threading.Thread.Sleep(2000);
            }

            try
            {
                System.Console.WriteLine(Directory.Exists(Environment.GetEnvironmentVariable("SETTING_FILE")));

                System.Console.WriteLine("Server starting...");
                //RegisterServices();

                //var hostConfig = _serviceProvider.GetService<HostConfiguration>();

                //var registry = _serviceProvider.GetService<ISpecRegistry>();
                //registry.RegisterDirectory(hostConfig.SpecDirectory);

                //var httpServer = _serviceProvider.GetService<HttpServer>();
                //var cancellationTokenSource = new CancellationTokenSource();
                //var cancellationToken = cancellationTokenSource.Token;
                //await httpServer.StartAsync(cancellationToken);

                //System.Console.WriteLine("Press any key to close");
                System.Console.Read();

                //cancellationTokenSource.Cancel();
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
            services.AddSingleton((s) => Common.YamlSerializer.DeserializeFile<HostConfiguration>(Environment.GetEnvironmentVariable("SETTING_FILE")));
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

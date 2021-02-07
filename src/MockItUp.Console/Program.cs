using Grpc.Core;
using log4net;
using log4net.Config;
using Microsoft.Extensions.DependencyInjection;
using MockItUp.Common;
using MockItUp.Core;
using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MockItUp.Console
{
    class Program
    {
        private static string _configPath;
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            var logRepo = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepo, new FileInfo("log4net.config"));

            var log = LogManager.GetLogger("main");

            RegisterServices();

            _configPath = args.Length > 0 ? args[0] : Environment.GetEnvironmentVariable("SETTING_FILE");

            Logger.LogInfo($"Load settings from {_configPath}");

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            try
            {
                Task.Run(() => StartCtlServer(cancellationToken));

                StartRestfulServer(cancellationToken);

                // cancellationTokenSource.Cancel();
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.Message, ex);
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
            services.AddSingleton((s) => Common.Utilities.YamlSerializer.DeserializeFile<HostConfiguration>(_configPath));
            services.AddSingleton<ISpecLoader, SpecLoader>();
            services.AddSingleton<ISpecRegistry, SpecRegistry>();

            services.AddSingleton<IMockProvider, Core.Restful.RestfulMockProvider>();

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

        private static void StartRestfulServer(CancellationToken cancellationToken)
        {
            var hostConfig = _serviceProvider.GetService<HostConfiguration>();
            var registry = _serviceProvider.GetService<ISpecRegistry>();
            registry.RegisterDirectory(hostConfig.SpecDirectory);

            var httpServer = _serviceProvider.GetService<HttpServer>();
            
            httpServer.Start(cancellationToken);
        }

        private static async Task StartCtlServer(CancellationToken cancellationToken)
        {
            var server = new Server
            {
                Services = { MockController.BindService(new MockControllerImpl()) },
                Ports = { new ServerPort("localhost", 30000, ServerCredentials.Insecure) }
            };
            server.Start();

            Logger.LogInfo("Control server started...");
            while (!cancellationToken.IsCancellationRequested) { }
            await server.ShutdownAsync();
        }
    }
}

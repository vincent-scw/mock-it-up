using Grpc.Core;
using log4net;
using log4net.Config;
using Microsoft.Extensions.DependencyInjection;
using MockItUp.Common;
using MockItUp.Core;
using MockItUp.Core.Contracts;
using MockItUp.Core.Dynamic;
using MockItUp.Core.Models;
using MockItUp.Core.Static;
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

            // Try fix GRPC DNS error
            Envrionment.SetEnvironmentVariable("GRPC_DNS_RESOLVER", "native");
            
            RegisterServices();

            _configPath = args.Length > 0 ? args[0] : Environment.GetEnvironmentVariable("SETTING_FILE");

            if (string.IsNullOrEmpty(_configPath))
                Logger.LogInfo($"Setting file not provided, use default.");
            else
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
            services.AddSingleton((s) => {
                if (string.IsNullOrEmpty(_configPath))
                    return new HostConfiguration();
                else
                    return Common.Utilities.YamlSerializer.DeserializeFile<HostConfiguration>(_configPath);
            });

            services.AddSingleton<IRequestHandler, RestfulRequestHandler>();
            services.AddSingleton<StaticMockProvider>();
            services.AddSingleton<DynamicMockProvider>();
            services.AddSingleton<HitRecordCollection>();
            
            _serviceProvider = services.BuildServiceProvider(true);

            services.AddSingleton<IServiceProvider>(_serviceProvider);
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
            var httpServer = _serviceProvider.GetService<HttpServer>();
            
            httpServer.Start(cancellationToken);
        }

        private static async Task StartCtlServer(CancellationToken cancellationToken)
        {
            var config = _serviceProvider.GetService<HostConfiguration>();
            var server = new Server
            {
                Services = { Mockctl.MockController.BindService(new MockControllerImpl(_serviceProvider)) },
                Ports = { new ServerPort(config.Host, config.ControlPort, ServerCredentials.Insecure) }
            };
            server.Start();

            Logger.LogInfo("Control server started...");
            while (!cancellationToken.IsCancellationRequested) { }
            await server.ShutdownAsync();
        }
    }
}

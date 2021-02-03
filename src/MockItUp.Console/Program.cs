using log4net;
using log4net.Config;
using Microsoft.Extensions.DependencyInjection;
using MockItUp.Common;
using MockItUp.Core;
using System;
using System.IO;
using System.Reflection;
using System.Threading;

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

            try
            {
                var hostConfig = _serviceProvider.GetService<HostConfiguration>();
                var registry = _serviceProvider.GetService<ISpecRegistry>();
                registry.RegisterDirectory(hostConfig.SpecDirectory);

                var httpServer = _serviceProvider.GetService<HttpServer>();
                var cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;
                httpServer.Start(cancellationToken);

                cancellationTokenSource.Cancel();
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
            services.AddSingleton((s) => Core.Utilities.YamlSerializer.DeserializeFile<HostConfiguration>(_configPath));
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

using MockItUp.Common;
using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using System.Net;
using System.Threading;

namespace MockItUp.Core
{
    public class HttpServer
    {
        private readonly HostConfiguration _hostConfiguration;
        private readonly IRequestHandler _handler;
        public HttpServer(HostConfiguration hostConfiguration, IRequestHandler requestHandler)
        {
            _hostConfiguration = hostConfiguration;
            _handler = requestHandler;
        }

        public void Start(CancellationToken cancelToken)
        {
            var listener = new HttpListener();
            listener.IgnoreWriteExceptions = true;

            foreach (var v in _hostConfiguration.Services.Values)
            {
                var prefix = $"http://{_hostConfiguration.Host}:{v}/";
                listener.Prefixes.Add(prefix);
                Logger.LogInfo($"Listen at {prefix}");
            }

            try
            {
                listener.Start();
                Logger.LogInfo($"Start to listen...");
            }
            catch (HttpListenerException hlex)
            {
                Logger.LogError(hlex.Message, hlex);
                throw hlex;
            }

            var sem = new Semaphore(_hostConfiguration.AcceptConnections, _hostConfiguration.AcceptConnections);

            // When it is not cancelld
            while (!cancelToken.IsCancellationRequested)
            {
                sem.WaitOne();

#pragma warning disable 4014
                // Wait here until we hear from a connection
                listener.GetContextAsync().ContinueWith(async t =>
                {
                    sem.Release();

                    var ctx = await t;

                    await _handler.HandleAsync(ctx);
                });
#pragma warning restore 4014
            }
        }
    }
}

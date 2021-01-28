using MockItUp.Common.Contracts;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace MockItUp.Core
{
    public class HttpServer
    {
        private readonly ILog _logger;
        private readonly HostConfiguration _hostConfiguration;
        private readonly IRequestHandler _handler;
        public HttpServer(HostConfiguration hostConfiguration, IMockProvider provider)
        {
            _logger = LogManager.GetLogger(typeof(HttpServer));
            _hostConfiguration = hostConfiguration;
            _handler = provider.RequestHandler;
        }

        public void Start(CancellationToken cancelToken)
        {
            var listener = new HttpListener();
            listener.IgnoreWriteExceptions = true;

            foreach (var v in _hostConfiguration.Services.Values)
            {
                var prefix = $"http://{_hostConfiguration.Host}:{v}/";
                listener.Prefixes.Add(prefix);
                _logger.Info($"Listen at {prefix}");
            }

            try
            {
                listener.Start();
                _logger.Info($"Start to listen...");
            }
            catch (HttpListenerException hlex)
            {
                _logger.Error(hlex.Message, hlex);
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
                    var req = ctx.Request;

                    //_logger.Info($"Request #: {++requestCount}");
                    _logger.Info($"{req.HttpMethod} {req.Url}");
                    var reader = new System.IO.StreamReader(req.InputStream);
                    _logger.Info($"Body: {reader.ReadToEnd()}");

                    await _handler.HandleAsync(ctx);

                    _logger.Info($"Completed.");
                });
#pragma warning restore 4014
            }
        }
    }
}

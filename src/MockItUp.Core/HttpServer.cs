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

        public async Task StartAsync(CancellationToken cancelToken)
        {
            var listener = new HttpListener();
            foreach (var v in _hostConfiguration.Hosts.Values)
            {
                var prefix = $"http://*:{v}/";
                listener.Prefixes.Add(prefix);
                _logger.Info($"Listen at {prefix}");
            }
            
            listener.Start();

            _logger.Info($"Start to listen...");

            var requestCount = 0;
            // When it is not cancelld
            while (!cancelToken.IsCancellationRequested)
            {
                // Wait here until we hear from a connection
                var ctx = await listener.GetContextAsync();

                var req = ctx.Request;

                _logger.Info($"Request #: {++requestCount}");
                _logger.Info($"{req.HttpMethod} {req.Url}");
                var reader = new System.IO.StreamReader(req.InputStream);
                _logger.Info($"Body: {reader.ReadToEnd()}");

                await _handler.HandleAsync(ctx);

                _logger.Info($"Response sent.");
            }
        }
    }
}

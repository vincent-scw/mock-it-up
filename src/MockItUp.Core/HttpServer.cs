using MockItUp.Common.Logging;
using MockItUp.Common.Contracts;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MockItUp.Core
{
    public class HttpServer
    {
        private readonly ILogger _logger;
        private readonly HostConfiguration _hostConfiguration;
        private readonly IRequestHandler _handler;
        public HttpServer(ILogger logger, HostConfiguration hostConfiguration, IMockProvider provider)
        {
            _logger = logger;
            _hostConfiguration = hostConfiguration;
            _handler = provider.RequestHandler;
        }

        public async Task StartAsync(CancellationToken cancelToken)
        {
            var listener = new HttpListener();
            foreach (var v in _hostConfiguration.Hosts.Values)
            {
                listener.Prefixes.Add(v);
            }
            
            listener.Start();

            _logger.Log($"Start to listen...");

            var requestCount = 0;
            // When it is not cancelld
            while (!cancelToken.IsCancellationRequested)
            {
                // Wait here until we hear from a connection
                var ctx = await listener.GetContextAsync();

                var req = ctx.Request;

                _logger.Log($"Request #: {++requestCount}");
                _logger.Log($"{req.HttpMethod} {req.Url}");
                var reader = new System.IO.StreamReader(req.InputStream);
                _logger.Log(reader.ReadToEnd());

                await _handler.HandleAsync(ctx);

                _logger.Log($"Response sent.");
            }
        }
    }
}

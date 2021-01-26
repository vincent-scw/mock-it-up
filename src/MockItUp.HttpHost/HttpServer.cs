using MockItUp.Common.Logging;
using MockItUp.Common.Contracts;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MockItUp.HttpHost
{
    public class HttpServer
    {
        private readonly ILogger _logger;
        private readonly HostConfiguration _hostConfiguration;
        public HttpServer(ILogger logger, HostConfiguration hostConfiguration)
        {
            _logger = logger;
            _hostConfiguration = hostConfiguration;
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
                var resp = ctx.Response;

                _logger.Log($"Request #: {++requestCount}");
                _logger.Log($"{req.HttpMethod} {req.Url}");
                _logger.Log($"UserHostName: {req.UserHostName}");
                _logger.Log($"UserAgent: {req.UserAgent}");

                byte[] data = Encoding.UTF8.GetBytes("{\"msg\": \"Hello World\"}");
                resp.ContentType = "application/json";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();

                _logger.Log($"Response sent.");
            }
        }
    }
}

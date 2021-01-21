using MockItUp.Common.Logging;
using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MockItUp.HttpHost
{
    public class HttpServer
    {
        private readonly ILogger _logger;
        public HttpServer(ILogger logger)
        {
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancelToken)
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8000/");
            listener.Start();

            _logger.Log($"Start to listen at http://localhost:8000/");

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

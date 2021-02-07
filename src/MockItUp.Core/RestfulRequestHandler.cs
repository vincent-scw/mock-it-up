using MockItUp.Common;
using MockItUp.Core.Contracts;
using MockItUp.Core.Dynamic;
using MockItUp.Core.Models;
using MockItUp.Core.Static;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MockItUp.Core
{
    public class RestfulRequestHandler : IRequestHandler
    {
        private StaticMockProvider _staticMockProvider;
        private DynamicMockProvider _dynamicMockProvider;
        private readonly HostConfiguration _hostConfiguration;

        public RestfulRequestHandler(
            StaticMockProvider staticMockProvider,
            DynamicMockProvider dynamicMockProvider,
            HostConfiguration hostConfiguration)
        {
            _staticMockProvider = staticMockProvider;
            _dynamicMockProvider = dynamicMockProvider;
            _hostConfiguration = hostConfiguration;
        }

        public async Task HandleAsync(HttpListenerContext context)
        {
            var req = context.Request;
            var resp = context.Response;

            Logger.LogInfo($"{req.HttpMethod} {req.Url}");

            try
            {
                var host = _hostConfiguration.Services.FirstOrDefault(kv => kv.Value == context.Request.Url.Port);
                if (host.Key == null)
                    throw new NotSupportedException($"Cannot handle request {context.Request.Url}. Host not found.");

                IMockProvider provider = null;
                // Try match dynamic first
                var matchedTemplate = _dynamicMockProvider.StubMatcher.Match(context.Request, host.Key, out StubItem stub);
                if (matchedTemplate != null)
                    provider = _dynamicMockProvider;
                else
                {
                    // Try match static
                    matchedTemplate = _staticMockProvider.StubMatcher.Match(context.Request, host.Key, out stub);
                    provider = _staticMockProvider;
                }
                
                if (matchedTemplate == null)
                    throw new NotSupportedException($"Cannot find matched rule. Request ignored.");

                await provider.ResponseResolver.ResolveAsync(context, stub, matchedTemplate);
            }
            catch (Exception ex)
            {
                resp.StatusCode = (int) HttpStatusCode.BadRequest;
                byte[] data = Encoding.UTF8.GetBytes(ex.Message);
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                await resp.OutputStream.WriteAsync(data, 0, data.Length);

                Logger.LogError(ex.Message, ex);
            }
            finally
            {
                resp.Close();
            }
        }
    }
}

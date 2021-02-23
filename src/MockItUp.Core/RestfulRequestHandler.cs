using MockItUp.Common;
using MockItUp.Core.Contracts;
using MockItUp.Core.Dynamic;
using MockItUp.Core.Models;
using MockItUp.Core.Static;
using System;
using System.Collections.Generic;
using System.IO;
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
                var requestModel = BuildRequest(context.Request);
                Logger.LogInfo($"Request body: {requestModel.Body}");

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

                if (stub.Response.Delay > 0)
                {
                    System.Threading.Thread.Sleep(stub.Response.Delay);
                }

                var responseModel = provider.ResponseResolver.Resolve(requestModel, stub, matchedTemplate);
                await WriteToHttpResponse(resp, responseModel);

                Logger.LogInfo($"Response body: {responseModel.Body}");
            }
            catch (Exception ex)
            {
                resp.StatusCode = (int)HttpStatusCode.BadRequest;
                await RespondBodyAsync(resp, ex.Message);

                Logger.LogError(ex.Message, ex);
            }
            finally
            {
                resp.Close();
            }
        }

        private static async Task WriteToHttpResponse(HttpListenerResponse resp, ResponseModel responseModel)
        {
            resp.StatusCode = responseModel.StatusCode;
            resp.ContentType = responseModel.ContentType;
            if (responseModel.Headers?.Count > 0)
            {
                foreach (var kv in responseModel.Headers)
                {
                    resp.Headers.Add(kv.Key, kv.Value);
                }
            }

            if (responseModel.Body != null)
            {
                await RespondBodyAsync(resp, responseModel.Body);
            }
        }

        private static async Task RespondBodyAsync(HttpListenerResponse resp, string body)
        {
            byte[] data = Encoding.UTF8.GetBytes(body);
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = data.LongLength;

            await resp.OutputStream.WriteAsync(data, 0, data.Length);
        }

        private static RequestModel BuildRequest(HttpListenerRequest request)
        {
            var reader = new StreamReader(request.InputStream);
            var bodyStr = reader.ReadToEnd();

            var ret = new RequestModel
            {
                Method = request.HttpMethod,
                Path = request.RawUrl,
                Body = bodyStr,
                Headers = new Dictionary<string, string>()
            };

            foreach (var h in request.Headers.AllKeys)
            {
                ret.Headers.Add(h.ToLower(), request.Headers[h]);
            }

            return ret;
        }       
    }
}

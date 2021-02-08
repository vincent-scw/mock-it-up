using MockItUp.Common;
using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UriTemplate.Core;

namespace MockItUp.Core.Dynamic
{
    public class MockResponseResolver : IMockResponseResolver
    {
        public async Task ResolveAsync(HttpListenerContext context, StubItem stub, UriTemplateMatch match)
        {
            var req = context.Request;
            var res = context.Response;
            var responseModel = stub.Response;

            var reader = new StreamReader(req.InputStream);
            var bodyStr = reader.ReadToEnd();
            Logger.LogInfo($"Body: {bodyStr}");

            res.StatusCode = responseModel.StatusCode;
            res.ContentType = responseModel.ContentType;

            // No body, return
            if (string.IsNullOrEmpty(responseModel.Body))
                return;

            var body = responseModel.Body;
            Logger.LogInfo($"Response body: {body}");

            if (responseModel.Delay > 0)
            {
                System.Threading.Thread.Sleep(responseModel.Delay);
            }

            byte[] data = Encoding.UTF8.GetBytes(body);
            res.ContentEncoding = Encoding.UTF8;
            res.ContentLength64 = data.LongLength;

            await res.OutputStream.WriteAsync(data, 0, data.Length);
        }
    }
}

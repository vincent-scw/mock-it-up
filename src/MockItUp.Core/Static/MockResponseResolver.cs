using MockItUp.Common;
using MockItUp.Common.Utilities;
using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UriTemplate.Core;

namespace MockItUp.Core.Static
{
    public class MockResponseResolver : IMockResponseResolver
    {
        private readonly HostConfiguration _configuration;

        public MockResponseResolver(HostConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task ResolveAsync(HttpListenerContext context, StubItem stub, UriTemplateMatch match)
        {
            var req = context.Request;
            var res = context.Response;
            var responseModel = stub.Response;

            var reader = new StreamReader(req.InputStream);
            var bodyStr = reader.ReadToEnd();
            Logger.LogInfo($"Body: {bodyStr}");

            var requestDict = RequestDictionaryBuilder.Build(context.Request, match, bodyStr);

            res.StatusCode = responseModel.StatusCode;
            res.ContentType = responseModel.ContentType;

            // No body, return
            if (string.IsNullOrEmpty(responseModel.Body))
                return;

            var type = responseModel.BodyType;
            if (type == BodyType.Auto)
            {
                type = File.Exists(Path.Combine(_configuration.PayloadDirectory, responseModel.Body)) ? BodyType.File : BodyType.Direct;
            }

            var body = type == BodyType.Direct
                ? responseModel.Body :
                File.ReadAllText(Path.Combine(_configuration.PayloadDirectory, responseModel.Body));

            body = FormatBody(body, requestDict);
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

        private string FormatBody(string bodyStr, IDictionary<string, dynamic> requestDict)
        {
            return bodyStr.Format((ph) =>
            {
                var phValue = ph.Substring(2, ph.Length - 3);
                var formatted = GetRequestValue(phValue, requestDict, true);
                return formatted == null ? ph : formatted;
            });
        }

        private string GetRequestValue(string phValue, IDictionary<string, dynamic> dict, bool isFirstLevel)
        {
            var dotIndex = phValue.IndexOf('.');
            var current = phValue.Substring(0, dotIndex < 0 ? phValue.Length : dotIndex);

            if (isFirstLevel)
                current = GetFirstLevel(current);

            if (!dict.TryGetValue(current.ToLower(), out dynamic currentValue))
                return null;

            var currentDict = currentValue as IDictionary<string, dynamic>;
            if (currentDict == null)
            {
                Type t = currentValue.GetType();
                if (t.IsSimpleType())
                    return Convert.ToString(currentValue);

                return JsonConvert.SerializeObject(currentValue);
            }

            return GetRequestValue(phValue.Substring(dotIndex + 1), currentDict, false);
        }

        private string GetFirstLevel(string current)
        {
            switch (current)
            {
                case "b":
                case "body":
                    return "body";
                case "h":
                case "header":
                case "headers":
                    return "headers";
                case "p":
                case "path":
                    return "path";
                default:
                    return current;
            }
        }
    }
}

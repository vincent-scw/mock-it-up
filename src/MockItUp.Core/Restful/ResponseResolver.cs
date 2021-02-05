using MockItUp.Common;
using MockItUp.Common.Utilities;
using MockItUp.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MockItUp.Core.Restful
{
    public class ResponseResolver
    {
        private readonly HostConfiguration _configuration;
        public ResponseResolver(HostConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Resolve(HttpListenerResponse resp, ResponseModel responseModel, IDictionary<string, dynamic> requestDict)
        {
            resp.StatusCode = responseModel.StatusCode;
            resp.ContentType = responseModel.ContentType;

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
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = data.LongLength;

            await resp.OutputStream.WriteAsync(data, 0, data.Length);
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

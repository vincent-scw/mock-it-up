using MockItUp.Common.Utilities;
using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        public ResponseModel Resolve(RequestModel request, StubItem stub, UriTemplateMatch match)
        {
            var res = new ResponseModel();
            var stubResponse = stub.Response;

            var requestDict = RequestDictionaryBuilder.Build(request, match);

            res.StatusCode = stubResponse.StatusCode;
            res.ContentType = stubResponse.ContentType;
            res.Headers = stubResponse.Headers;

            // No body, return
            if (string.IsNullOrEmpty(stubResponse.Body))
                return res;

            var type = stubResponse.BodyType;
            if (type == BodyType.Auto)
            {
                type = File.Exists(Path.Combine(_configuration.PayloadDirectory, stubResponse.Body)) ? BodyType.File : BodyType.Direct;
            }

            var body = type == BodyType.Direct
                ? stubResponse.Body :
                File.ReadAllText(Path.Combine(_configuration.PayloadDirectory, stubResponse.Body));

            res.Body = FormatBody(body, requestDict);

            return res;
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

            if (!dict.TryGetValue(current, out dynamic currentValue))
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

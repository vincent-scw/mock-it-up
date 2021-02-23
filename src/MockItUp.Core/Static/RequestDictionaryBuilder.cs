using MockItUp.Core.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Net;
using UriTemplate.Core;

namespace MockItUp.Core.Static
{
    internal class RequestDictionaryBuilder
    {
        public static IDictionary<string, dynamic> Build(RequestModel request, UriTemplateMatch uriMatch)
        {
            var requestDict = new Dictionary<string, object>();

            requestDict["path"] = BuildPath(uriMatch);
            requestDict["headers"] = request.Headers;
            try
            {
                requestDict["body"] = BuildBody(request.Body);
            }
            catch (JsonReaderException)
            {
                requestDict["body"] = request.Body;
            }

            return requestDict;
        }

        private static IDictionary<string, dynamic> BuildPath(UriTemplateMatch uriMatch)
        {
            var ret = new Dictionary<string, object>();
            foreach (var b in uriMatch.Bindings)
            {
                ret.Add(b.Key.ToLower(), b.Value.Value);
            }

            return ret;
        }

        private static IDictionary<string, dynamic> BuildBody(string bodyStr)
        {
            if (string.IsNullOrEmpty(bodyStr))
                return null;

            var converter = new ExpandoObjectConverter();
            IDictionary<string, dynamic> bodyObj = JsonConvert.DeserializeObject<ExpandoObject>(bodyStr, converter);
            return bodyObj;
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Net;
using UriTemplate.Core;

namespace MockItUp.Core.Restful
{
    internal class RequestDictionaryBuilder
    {
        public IDictionary<string, dynamic> Build(HttpListenerRequest hlr, UriTemplateMatch uriMatch, string bodyStr)
        {
            var request = new Dictionary<string, object>();

            request["path"] = BuildPath(uriMatch);
            request["headers"] = BuildHeaders(hlr.Headers);
            try
            {
                request["body"] = BuildBody(bodyStr);
            }
            catch (JsonReaderException)
            {
                request["body"] = bodyStr;
            }

            return request;
        }

        private IDictionary<string, dynamic> BuildPath(UriTemplateMatch uriMatch)
        {
            var ret = new Dictionary<string, object>();
            foreach (var b in uriMatch.Bindings)
            {
                ret.Add(b.Key.ToLower(), b.Value.Value);
            }

            return ret;
        }

        private IDictionary<string, dynamic> BuildHeaders(NameValueCollection headers)
        {
            var ret = new Dictionary<string, object>();
            foreach (var h in headers.AllKeys)
            {
                ret.Add(h.ToLower(), headers[h]);
            }

            return ret;
        }

        private IDictionary<string, dynamic> BuildBody(string bodyStr)
        {
            if (string.IsNullOrEmpty(bodyStr))
                return null;

            var converter = new ExpandoObjectConverter();
            IDictionary<string, dynamic> bodyObj = JsonConvert.DeserializeObject<ExpandoObject>(bodyStr, converter);
            return bodyObj;
        }
    }
}

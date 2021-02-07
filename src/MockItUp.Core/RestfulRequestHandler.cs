using MockItUp.Common;
using MockItUp.Core.Contracts;
using MockItUp.Core.Models;
using MockItUp.Core.Restful;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UriTemplate.Core;

namespace MockItUp.Core
{
    public class RestfulRequestHandler : IRequestHandler
    {
        private const string WILDCARD = "*";

        private readonly IReadOnlyDictionary<string, IList<RuleItem>> _items;
        private readonly HostConfiguration _hostConfiguration;
        private readonly ResponseResolver _resolver;

        public RestfulRequestHandler(ISpecRegistry registry,
            ResponseResolver resolver,
            HostConfiguration hostConfiguration)
        {
            _items = BuildItemDictionary(registry.Specs);
            _hostConfiguration = hostConfiguration;
            _resolver = resolver;
        }

        public async Task HandleAsync(HttpListenerContext context)
        {
            var req = context.Request;
            var resp = context.Response;

            Logger.LogInfo($"{req.HttpMethod} {req.Url}");
            var reader = new System.IO.StreamReader(req.InputStream);
            var bodyStr = reader.ReadToEnd();
            Logger.LogInfo($"Body: {bodyStr}");

            try
            {
                var host = _hostConfiguration.Services.FirstOrDefault(kv => kv.Value == context.Request.Url.Port);
                if (host.Key == null)
                    throw new NotSupportedException($"Cannot handle request {context.Request.Url}. Host not found.");

                var candidates = _items.ContainsKey(host.Key) ?
                    _items[host.Key] :
                    _items.SelectMany(x => x.Value);

                RuleItem matchedItem = Match(context.Request, candidates, out UriTemplateMatch match);
                
                if (matchedItem == null)
                    throw new NotSupportedException($"Cannot find matched rule. Request ignored.");

                var requestDict = new RequestDictionaryBuilder().Build(context.Request, match, bodyStr);

                await _resolver.Resolve(context.Response, matchedItem.Response, requestDict);
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

        private static RuleItem Match(HttpListenerRequest request, IEnumerable<RuleItem> candidates, out UriTemplateMatch match)
        {
            match = null;
            foreach (var c in candidates)
            {
                match = c.Match(request.HttpMethod, request.Url);
                if (match != null)
                {
                    return c;
                }
            }

            return null;
        }

        private static IReadOnlyDictionary<string, IList<RuleItem>> BuildItemDictionary(IList<SpecDeclaration> specs) 
        {
            var ret = new Dictionary<string, IList<RuleItem>>();
            foreach (RestfulSpecDeclaration spec in specs)
            {
                var key = string.IsNullOrEmpty(spec.Service) ? WILDCARD : spec.Service;
                if (!ret.ContainsKey(key))
                    ret.Add(key, new List<RuleItem>());

                ret[key] = ret[key].Concat(spec.Rules).ToList();
            }

            return ret;
        }
    }
}

using log4net;
using MockItUp.Common;
using MockItUp.Restful.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MockItUp.Restful
{
    public class RestfulRequestHandler : IRequestHandler
    {
        private const string WILDCARD = "*";

        private readonly IReadOnlyDictionary<string, IList<RuleItem>> _items;
        private readonly IDictionary<string, int> _hosts;
        private readonly ResponseResolver _resolver;

        public RestfulRequestHandler(ISpecRegistry registry,
            ResponseResolver resolver,
            HostConfiguration hostConfiguration)
        {
            _items = BuildItemDictionary(registry.GetSpecs("restful"));
            _hosts = hostConfiguration.Services;
            _resolver = resolver;
        }

        public async Task HandleAsync(HttpListenerContext context)
        {
            var resp = context.Response;
            try
            {
                var host = _hosts.FirstOrDefault(kv => kv.Value == context.Request.Url.Port);
                if (host.Key == null)
                    throw new NotSupportedException($"Cannot handle request {context.Request.Url}. Host not found.");

                var candidates = _items.ContainsKey(host.Key) ?
                    _items[host.Key] :
                    _items.SelectMany(x => x.Value);

                var matched = candidates.FirstOrDefault(d => d.Match(context.Request.HttpMethod, context.Request.Url) != null);
                if (matched == null)
                    throw new NotSupportedException($"Cannot find matched rule. Request ignored.");

                if (matched.Response.Delay > 0)
                {
                    System.Threading.Thread.Sleep(matched.Response.Delay);
                }

                await _resolver.Resolve(context.Response, matched.Response);
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

        private static IReadOnlyDictionary<string, IList<RuleItem>> BuildItemDictionary(IReadOnlyCollection<SpecDeclaration> specs) 
        {
            var ret = new Dictionary<string, IList<RuleItem>>();
            foreach (RestfulSpecDeclaration spec in specs)
            {
                var key = string.IsNullOrEmpty(spec.ServerName) ? WILDCARD : spec.ServerName;
                if (!ret.ContainsKey(key))
                    ret.Add(key, new List<RuleItem>());

                ret[key] = ret[key].Concat(spec.Rules).ToList();
            }

            return ret;
        }
    }
}

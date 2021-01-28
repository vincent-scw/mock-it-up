using MockItUp.Common.Contracts;
using MockItUp.Restful.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Text;

namespace MockItUp.Restful
{
    public class RestfulRequestHandler : IRequestHandler
    {
        private const string WILDCARD = "*";

        private readonly IReadOnlyDictionary<string, IList<RuleItem>> _items;
        private readonly IDictionary<string, int> _hosts;
        public RestfulRequestHandler(ISpecRegistry registry, HostConfiguration hostConfiguration)
        {
            _items = BuildItemDictionary(registry.GetSpecs("restful"));
            _hosts = hostConfiguration.Hosts;
        }

        public async Task HandleAsync(HttpListenerContext context)
        {
            var host = _hosts.FirstOrDefault(kv => kv.Value == context.Request.Url.Port);
            if (host.Key == null)
                throw new NotSupportedException($"Cannot handle request {context.Request.Url}. Host not found.");

            var candidates = _items.ContainsKey(host.Key) ?
                _items[host.Key] :
                _items.SelectMany(x => x.Value);

            var matched = candidates.FirstOrDefault(d => d.Matches(context.Request) != null);
            if (matched == null)
                return;

            var resp = context.Response;

            resp.StatusCode = matched.Response.StatusCode;
            resp.ContentType = matched.Response.ContentType;

            byte[] data = Encoding.UTF8.GetBytes(matched.Response.BodyType == BodyType.Direct
                ? matched.Response.Body :
                System.IO.File.ReadAllText(matched.Response.Body));
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = data.LongLength;

            await resp.OutputStream.WriteAsync(data, 0, data.Length);
            resp.Close();
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

﻿using MockItUp.Common.Contracts;
using MockItUp.Restful.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using log4net;

namespace MockItUp.Restful
{
    public class RestfulRequestHandler : IRequestHandler
    {
        private const string WILDCARD = "*";

        private readonly IReadOnlyDictionary<string, IList<RuleItem>> _items;
        private readonly IDictionary<string, int> _hosts;
        private readonly ResponseResolver _resolver;
        private readonly ILog _log;
        public RestfulRequestHandler(ISpecRegistry registry,
            ResponseResolver resolver,
            HostConfiguration hostConfiguration)
        {
            _items = BuildItemDictionary(registry.GetSpecs("restful"));
            _hosts = hostConfiguration.Services;
            _resolver = resolver;
            _log = LogManager.GetLogger(typeof(RestfulRequestHandler));
        }

        public async Task HandleAsync(HttpListenerContext context)
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

            var resp = context.Response;
            await _resolver.Resolve(resp, matched.Response);

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
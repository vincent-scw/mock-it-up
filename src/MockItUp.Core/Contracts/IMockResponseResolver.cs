using MockItUp.Core.Models;
using System.Net;
using UriTemplate.Core;

namespace MockItUp.Core.Contracts
{
    public interface IMockResponseResolver
    {
        UriTemplateMatch Match(HttpListenerRequest request, string service, out RuleItem stub);
    }
}

using MockItUp.Core.Models;
using System.Net;
using UriTemplate.Core;

namespace MockItUp.Core.Contracts
{
    public interface IMockResponseResolver
    {
        ResponseModel Resolve(RequestModel request, StubItem stub, UriTemplateMatch match);
    }
}

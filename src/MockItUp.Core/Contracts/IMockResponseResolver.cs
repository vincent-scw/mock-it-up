using MockItUp.Core.Models;
using System.Net;
using System.Threading.Tasks;
using UriTemplate.Core;

namespace MockItUp.Core.Contracts
{
    public interface IMockResponseResolver
    {
        Task ResolveAsync(HttpListenerContext context, StubItem stub, UriTemplateMatch match);
    }
}

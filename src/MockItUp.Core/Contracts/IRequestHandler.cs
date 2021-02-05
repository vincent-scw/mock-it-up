using System.Net;

namespace MockItUp.Core.Contracts
{
    public interface IRequestHandler
    {
        System.Threading.Tasks.Task HandleAsync(HttpListenerContext context);
    }
}

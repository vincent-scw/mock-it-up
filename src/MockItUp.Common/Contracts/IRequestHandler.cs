using System.Net;

namespace MockItUp.Common.Contracts
{
    public interface IRequestHandler
    {
        System.Threading.Tasks.Task HandleAsync(HttpListenerContext context);
    }
}

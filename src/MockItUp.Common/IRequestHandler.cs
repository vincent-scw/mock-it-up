using System.Net;

namespace MockItUp.Common
{
    public interface IRequestHandler
    {
        System.Threading.Tasks.Task HandleAsync(HttpListenerContext context);
    }
}

using MockItUp.Common;
using MockItUp.Core.Contracts;
using MockItUp.Core.Models;

namespace MockItUp.Core.Restful
{
    public class RestfulMockProvider : IMockProvider
    {
        public RestfulMockProvider(ISpecRegistry registry, HostConfiguration hostConfiguration)
        {
            RequestHandler = new RestfulRequestHandler(registry, new ResponseResolver(hostConfiguration), hostConfiguration);
        }

        public IRequestHandler RequestHandler { get; }
    }
}

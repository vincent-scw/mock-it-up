using MockItUp.Common.Contracts;

namespace MockItUp.Restful
{
    public class RestfulMockProvider : IMockProvider
    {
        private readonly IRequestHandler _requestHandler;
        public RestfulMockProvider(ISpecRegistry registry, HostConfiguration hostConfiguration)
        {
            _requestHandler = new RestfulRequestHandler(registry, hostConfiguration);
        }

        public MockTypeEnum MockType => MockTypeEnum.Restful;
        public IRequestHandler RequestHandler => _requestHandler;
    }
}

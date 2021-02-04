using MockItUp.Common;

namespace MockItUp.Restful
{
    public class RestfulMockProvider : IMockProvider
    {
        public RestfulMockProvider(ISpecRegistry registry, HostConfiguration hostConfiguration)
        {
            RequestHandler = new RestfulRequestHandler(registry, new ResponseResolver(hostConfiguration), hostConfiguration);
        }

        public MockTypeEnum MockType => MockTypeEnum.Restful;
        public IRequestHandler RequestHandler { get; }
    }
}

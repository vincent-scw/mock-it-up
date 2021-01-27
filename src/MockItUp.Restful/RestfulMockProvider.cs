﻿using MockItUp.Common.Contracts;

namespace MockItUp.Restful
{
    public class RestfulMockProvider : IMockProvider
    {
        public RestfulMockProvider(ISpecRegistry registry, HostConfiguration hostConfiguration)
        {
            RequestHandler = new RestfulRequestHandler(registry, hostConfiguration);
        }

        public MockTypeEnum MockType => MockTypeEnum.Restful;
        public IRequestHandler RequestHandler { get; }
    }
}

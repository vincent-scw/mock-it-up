using MockItUp.Core.Contracts;
using System;

namespace MockItUp.Restful
{
    public class RestfulMockProvider : IMockProvider
    {
        public MockTypeEnum MockType => MockTypeEnum.Restful;
    }
}

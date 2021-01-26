using MockItUp.Common.Contracts;

namespace MockItUp.Restful
{
    public class RestfulMockProvider : IMockProvider
    {
        public MockTypeEnum MockType => MockTypeEnum.Restful;
    }
}

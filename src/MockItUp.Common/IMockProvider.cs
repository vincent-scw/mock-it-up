namespace MockItUp.Common
{
    public interface IMockProvider
    {
        MockTypeEnum MockType { get; }
        IRequestHandler RequestHandler { get; }
    }
}

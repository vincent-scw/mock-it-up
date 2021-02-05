namespace MockItUp.Core.Contracts
{
    public interface IMockProvider
    {
        IRequestHandler RequestHandler { get; }
    }
}

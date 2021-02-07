namespace MockItUp.Core.Contracts
{
    internal interface IMockProvider
    {
        IMockStubMatcher StubMatcher { get; }
        IMockResponseResolver ResponseResolver { get; }
    }
}

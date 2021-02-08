using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MockItUp.Client
{
    public class TestScenario : IDisposable
    {
        private readonly ConcurrentBag<string> _stubIds;
        private readonly MockController.MockControllerClient _client;
        public TestScenario(MockController.MockControllerClient client)
        {
            _client = client;
            _stubIds = new ConcurrentBag<string>();
        }

        public RegisterResult RegisterDynamicStub(DynamicStub stub)
        {
            var result = _client.RegisterDynamicStub(stub);
            if (result.Succeed)
                _stubIds.Add(result.StubID);
            return result;
        }

        public async Task<RegisterResult> RegisterDynamicStubAsync(DynamicStub stub)
        {
            var result = await _client.RegisterDynamicStubAsync(stub);
            if (result.Succeed)
                _stubIds.Add(result.StubID);
            return result;
        }

        public void RemoveDynamicStub(params string[] stubIds)
        {
            var ids = new StubIDs();
            ids.IdList.Add(stubIds);
            _client.RemoveDynamicStubs(ids);

            _stubIds.TryTake(out string _);
        }

        public async Task RemoveDynamicStubAsync(params string[] stubIds)
        {
            var ids = new StubIDs();
            ids.IdList.Add(stubIds);
            await _client.RemoveDynamicStubsAsync(ids);

            _stubIds.TryTake(out string _);
        }

        public void Close()
        {
            // Remove stubs created in current scenario
            RemoveDynamicStub(_stubIds.ToArray());
        }

        void IDisposable.Dispose()
        {
            Close();
        }
    }
}

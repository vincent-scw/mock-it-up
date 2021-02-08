using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockItUp.Client
{
    public class TestScenario : IDisposable
    {
        private List<string> _stubIds;
        private readonly MockController.MockControllerClient _client;
        public TestScenario(MockController.MockControllerClient client)
        {
            _client = client;
            _stubIds = new List<string>();
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

        public void RemoveDynamicStubs(params string[] stubIds)
        {
            var ids = PrepareStubIDs(stubIds);
            if (ids == null)
                return;

            if (_client.RemoveDynamicStubs(ids).Succeed)
                _stubIds = _stubIds.Where(x => !stubIds.Contains(x)).ToList();
        }

        public async Task RemoveDynamicStubsAsync(params string[] stubIds)
        {
            var ids = PrepareStubIDs(stubIds);
            if (ids == null)
                return;

            if ((await _client.RemoveDynamicStubsAsync(ids)).Succeed)
                _stubIds = _stubIds.Where(x => !stubIds.Contains(x)).ToList();
        }

        public void Close()
        {
            // Remove stubs created in current scenario
            RemoveDynamicStubs(_stubIds.ToArray());
        }

        void IDisposable.Dispose()
        {
            Close();
        }

        private StubIDs PrepareStubIDs(string[] stubIds)
        {
            if (stubIds == null || stubIds.Length == 0)
            {
                return null;
            }
            var ids = new StubIDs();
            ids.IdList.Add(stubIds);
            return ids;
        }
    }
}

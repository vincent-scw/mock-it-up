using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockItUp.Client
{
    /// <summary>
    /// A test scenario
    /// </summary>
    public class TestScenario : IDisposable
    {
        private List<string> _stubIds;
        private readonly Mockctl.MockController.MockControllerClient _client;
        internal TestScenario(Mockctl.MockController.MockControllerClient client)
        {
            _client = client;
            _stubIds = new List<string>();
        }

        /// <summary>
        /// Register a dynamic stub.
        /// </summary>
        /// <param name="stubAction">Build a stub with request and expected response</param>
        /// <returns>Register result</returns>
        public RegisterResult RegisterDynamicStub(Action<DynamicStub> stubAction)
        {
            var stub = new DynamicStub();
            stubAction(stub);
            var result = _client.RegisterDynamicStub(stub.InternalStub);
            if (result.Succeed)
                _stubIds.Add(result.StubID);
            return new RegisterResult
            {
                Succeed = result.Succeed,
                StubID = result.StubID
            };
        }

        /// <summary>
        /// Register a dynamic stub async.
        /// </summary>
        /// <param name="stubAction">Build a stub with request and expected response</param>
        /// <returns>Register result</returns>
        public async Task<RegisterResult> RegisterDynamicStubAsync(Action<DynamicStub> stubAction)
        {
            var stub = new DynamicStub();
            stubAction(stub);
            var result = await _client.RegisterDynamicStubAsync(stub.InternalStub);
            if (result.Succeed)
                _stubIds.Add(result.StubID);
            return new RegisterResult
            {
                Succeed = result.Succeed,
                StubID = result.StubID
            };
        }

        /// <summary>
        /// Remove dynamic stubs by stub id.
        /// </summary>
        /// <param name="stubIds">Stub id list</param>
        public void RemoveDynamicStubs(params string[] stubIds)
        {
            var ids = PrepareStubIDs(stubIds);
            if (ids == null)
                return;

            if (_client.RemoveDynamicStubs(ids).Succeed)
                _stubIds = _stubIds.Where(x => !stubIds.Contains(x)).ToList();
        }

        /// <summary>
        /// Remove dynamic stubs by stub id async.
        /// </summary>
        /// <param name="stubIds">Stub id list</param>
        public async Task RemoveDynamicStubsAsync(params string[] stubIds)
        {
            var ids = PrepareStubIDs(stubIds);
            if (ids == null)
                return;

            if ((await _client.RemoveDynamicStubsAsync(ids)).Succeed)
                _stubIds = _stubIds.Where(x => !stubIds.Contains(x)).ToList();
        }

        /// <summary>
        /// Close current test scenario.
        /// It removes stubs generated in current scenario.
        /// </summary>
        public void Close()
        {
            // Remove stubs created in current scenario
            RemoveDynamicStubs(_stubIds.ToArray());
        }

        void IDisposable.Dispose()
        {
            Close();
        }

        private Mockctl.StubIDs PrepareStubIDs(string[] stubIds)
        {
            if (stubIds == null || stubIds.Length == 0)
            {
                return null;
            }
            var ids = new Mockctl.StubIDs();
            ids.IdList.Add(stubIds);
            return ids;
        }
    }
}

using Grpc.Core;
using Mockctl;
using MockItUp.Common;
using MockItUp.Core;
using MockItUp.Core.Dynamic;
using MockItUp.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MockItUp.Console
{
    class MockControllerImpl : MockController.MockControllerBase
    {
        private readonly DynamicMockProvider _mockProvider;
        private readonly HitRecordCollection _hitRecords;
        public MockControllerImpl(IServiceProvider serviceProvider)
        {
            _mockProvider = serviceProvider.GetService(typeof(DynamicMockProvider)) as DynamicMockProvider;
            _hitRecords = serviceProvider.GetService(typeof(HitRecordCollection)) as HitRecordCollection;
        }

        public override Task<RegisterResult> RegisterDynamicStub(DynamicStub request, ServerCallContext context)
        {
            var req = request.Request;
            var res = request.Response;

            try
            {
                var stubId = _mockProvider.Registry.Register(new StubItem
                {
                    Request = new RequestModel
                    {
                        Method = req.Method,
                        Path = req.UriTemplate,
                        BodyType = BodyType.Direct
                    },
                    Response = new ResponseModel
                    {
                        Body = res.Body,
                        StatusCode = res.StatusCode,
                        ContentType = res.ContentType,
                        Headers = res.Headers
                    }
                }, req.Service);

                Logger.LogInfo("Dynamic stub registered");

                var result = new RegisterResult { Succeed = true, StubID = stubId.ToString() };
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                return Task.FromResult(new RegisterResult { Succeed = false });
            }
        }

        public override Task<RemoveResult> RemoveDynamicStubs(StubIDs request, ServerCallContext context)
        {
            try
            {
                _mockProvider.Registry.Remove(request.IdList);

                Logger.LogInfo($"Dynamic stubs removed");

                return Task.FromResult(new RemoveResult { Succeed = true });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                return Task.FromResult(new RemoveResult { Succeed = false });
            }
        }

        public override Task<Records> GetLastRecords(NRecords request, ServerCallContext context)
        {
            var records = _hitRecords.TakeLast(request.N).Select(x => 
            {
                var record = new Record
                {
                    Time = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(x.RecordTime),
                    Message = x.Message,
                    Request = new HttpRequest
                    {
                        HttpMethod = x.Request.Method,
                        Uri = x.Request.Path,
                        Body = x.Request.Body,
                        Headers = { x.Request.Headers }
                    },
                    Response = new ResponseDef
                    {
                        StatusCode = x.Response.StatusCode,
                        ContentType = x.Response.ContentType,
                        Body = x.Response.Body,
                        Headers = { x.Response.Headers }
                    }
                };

                return record;
            });

            return Task.FromResult(new Records { Items = { records } });
        }
    }
}

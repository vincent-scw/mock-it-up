using System;

namespace MockItUp.Core.Models
{
    public class HitRecord
    {
        public DateTimeOffset RecordTime { get; set; }
        public RequestModel Request { get; set; }
        public ResponseModel Response { get; set; }
        public StubItem Stub { get; set; }
    }
}

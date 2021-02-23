using System;

namespace MockItUp.Core.Models
{
    public class HitRecord
    {
        public DateTime RecordTime { get; set; }
        public RequestModel Request { get; set; }
        public ResponseModel Response { get; set; }
        public string Message { get; set; }
    }
}

using System.Collections.Generic;

namespace MockItUp.Restful.Models
{
    public class RequestModel
    {
        public string Method { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public string Path { get; set; }
        public string Body { get; set; }
        public string TargetHost { get; set; }
        
        public BodyType BodyType { get; set; } = BodyType.Direct;
    }
}

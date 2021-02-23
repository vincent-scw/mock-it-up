using System.Collections.Generic;

namespace MockItUp.Core.Models
{
    public class RequestModel
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public string Body { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        
        public BodyType BodyType { get; set; } = BodyType.Direct;
    }
}

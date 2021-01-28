using System.Collections.Generic;

namespace MockItUp.Restful.Models
{
    public class RequestModel
    {
        public string Method { get; set; }
        public string Path { get; set; }
        
        public BodyType BodyType { get; set; } = BodyType.Direct;
    }
}

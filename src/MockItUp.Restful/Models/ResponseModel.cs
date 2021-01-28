using System.Collections.Generic;

namespace MockItUp.Restful.Models
{
    public class ResponseModel
    {
        public IDictionary<string, string> Headers { get; set; }
        public string Body { get; set; }
        public int StatusCode { get; set; } = 200;
        public string ContentType { get; set; } = "application/json";
        public int Delay { get; set; }

        public BodyType BodyType { get; set; } = BodyType.Direct;
    }
}

using System;
using System.Collections.Generic;

namespace MockItUp.Client
{
    public class Record
    {
        public DateTimeOffset Time { get; private set; }
        public string Message { get; private set; }
        public HttpRequest Request { get; private set; }
        public HttpResponse Response { get; private set; }

        internal Record(Mockctl.Record rec)
        {
            Time = rec.Time.ToDateTimeOffset();
            Message = rec.Message;
            Request = new HttpRequest(rec.Request);
            Response = new HttpResponse(rec.Response);
        }
    }

    public class HttpRequest
    {
        public string HttpMethod { get; private set; }
        public string Uri { get; private set; }
        public string Body { get; private set; }
        public IDictionary<string, string> Headers { get; private set; }

        internal HttpRequest(Mockctl.HttpRequest req)
        {
            HttpMethod = req.HttpMethod;
            Uri = req.Uri;
            Body = req.Body;
            Headers = req.Headers;
        }
    }

    public class HttpResponse
    {
        public int StatusCode { get; private set; }
        public string ContentType { get; private set; }
        public string Body { get; private set; }
        public IDictionary<string, string> Headers { get; private set; }

        internal HttpResponse(Mockctl.ResponseDef res)
        {
            StatusCode = res.StatusCode;
            ContentType = res.ContentType;
            Body = res.Body;
            Headers = res.Headers;
        }
    }
}

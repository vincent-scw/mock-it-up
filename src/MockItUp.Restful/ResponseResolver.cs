using MockItUp.Common;
using MockItUp.Restful.Models;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MockItUp.Restful
{
    public class ResponseResolver
    {
        public async Task Resolve(HttpListenerResponse resp, ResponseModel responseModel, HostConfiguration configuration)
        {
            resp.StatusCode = responseModel.StatusCode;
            resp.ContentType = responseModel.ContentType;

            // No body, return
            if (string.IsNullOrEmpty(responseModel.Body))
                return;

            var type = responseModel.BodyType;
            if (type == BodyType.Auto)
            {
                type = File.Exists(Path.Combine(configuration.PayloadDirectory, responseModel.Body)) ? BodyType.File : BodyType.Direct;
            }

            var body = type == BodyType.Direct
                ? responseModel.Body :
                File.ReadAllText(Path.Combine(configuration.PayloadDirectory, responseModel.Body));
            Logger.LogInfo($"Response body: {body}");

            byte[] data = Encoding.UTF8.GetBytes(body);
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = data.LongLength;

            await resp.OutputStream.WriteAsync(data, 0, data.Length);
        }
    }
}

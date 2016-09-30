using System;
using System.Linq;
using System.Text;

namespace DemoServer
{
    public class ResponseData<T> : IResponseData<T>
    {
        public HeaderCollection Headers { get; internal set; }

        public T Payload { get; internal set; }

        public StatusCode StatusCode { get; internal set; }

        public ResponseData(){
            Headers = new HeaderCollection();
            StatusCode = StatusCode.OK;
        }

        public virtual byte[] GetBytes()
        {
            var stringPayload = Payload.ToString();
            var responseBytes = Encoding.UTF8.GetBytes(stringPayload);

            string headers = MakeHeader(Headers, responseBytes.Length);
            var headerBytes = Encoding.UTF8.GetBytes(headers);

            var resultBytes = headerBytes.ToList().Concat(responseBytes).ToArray();
            return resultBytes;
        }

        protected string MakeHeader(HeaderCollection headers, int contentLength)
        {
            var code = (int)StatusCode;
            StringBuilder sb = new StringBuilder($"HTTP/1.1 {code} {StatusCode}\r\nServer: SEDC Data Web Server\r\n");
            sb.AppendLine($"Content-Length: {contentLength}");
            foreach (var header in headers)
            {
                sb.AppendLine($"{header.Name}: {header.Value}");
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
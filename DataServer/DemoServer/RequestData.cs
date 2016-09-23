using System.Collections.Generic;
using System.Linq;

namespace DemoServer
{
    public class RequestData
    {
        public Method Method { get; set; }
        public string Location { get; set; }
        public Dictionary<string, string> QueryParameters { get; set; }
        public HeaderCollection Headers { get; set; }
        public string Body { get; set; }

        public RequestData()
        {
            Method = Method.Unknown;
            Headers = new HeaderCollection();
            Location = string.Empty;
            Body = string.Empty;
            QueryParameters = new Dictionary<string, string>();
        }

    }
}
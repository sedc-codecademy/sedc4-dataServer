using System;
using System.Text;

namespace DemoServer
{
    internal class StringResponseGenerator : IResponseGenerator
    {
        public StringResponseGenerator()
        {
        }
        public IResponseData GenerateResponse(RequestData request)
        {
            var result = new ResponseData<string>();
            result.Headers = new HeaderCollection();
            result.Headers.Add("Content-type", "text/html");
            StringBuilder sb = new StringBuilder();
            sb.Append(TagWriter.Create("h1","HELLO FROM SEDC SERVER"));
            sb.Append(TagWriter.Create("h2",$"You requested the location {request.Location}"));

            sb.Append(TagWriter.Create("h2", "Headers"));   
            foreach (var header in request.Headers)
            {
                sb.Append(TagWriter.Create("div", $"{header.Name}: {header.Value}"));
            }

            sb.Append(TagWriter.Create("h2", "Query Parameters"));
            foreach (var param in request.QueryParameters)
            {
                sb.Append(TagWriter.Create("div", $"{param.Key}: {param.Value}"));
            }
            result.Payload = sb.ToString();
            return result;
        }
    }
}
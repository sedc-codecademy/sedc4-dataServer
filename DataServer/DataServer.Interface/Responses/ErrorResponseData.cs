using DataServer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServer.Responses
{
    public class ErrorResponseData : ResponseData<string>
    {
        public ErrorResponseData(StatusCode statusCode)
        {
            StatusCode = statusCode;
            Payload = GetPayload();
        }

        private string GetPayload()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(TagWriter.Create("h1", "Error occured"));
            sb.AppendLine(TagWriter.Create("h2", StatusCode.ToString()));
            return sb.ToString();
        }

    }
}

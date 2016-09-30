using System;
using System.IO;

namespace DemoServer
{
    internal class FileResponseGenerator : IResponseGenerator
    {
        public IResponseData GenerateResponse(RequestData request)
        {
            var result = new BinaryResponseData();
            result.Payload = File.ReadAllBytes("favicon.ico");
            result.Headers.Add("Content-Type", "image/ico");
            return result;
        }
    }
}
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DemoServer
{
    internal class FileResponseGenerator : IResponseGenerator
    {
        public IResponseData GenerateResponse(RequestData request)
        {

            var filePattern = new Regex(@"^\/([a-z0-9]+\.[a-z0-9]+)$", RegexOptions.IgnoreCase);
            var match = filePattern.Match(request.Location);
            if (!match.Success)
            {
                throw new DataServerException("Invalid file request for file response generator");
            }
            var fileName = match.Groups[1].Value;

            if (!File.Exists(fileName))
            {
                return new FileNotFoundResponseData(fileName);
            }

            var result = new BinaryResponseData();
            result.Payload = File.ReadAllBytes(fileName);
            result.Headers.Add("Content-Type", GetContentType(fileName));
            return result;
        }

        private Dictionary<string, string> contentTypes = new Dictionary<string, string>
        {
            {".ico", "image/ico" },
            {".html" , "text/html"},
            {".jpg" , "image/jpeg"},
            {".txt", "text/plain" }
        };

        private string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName);
            if (contentTypes.ContainsKey(ext))
                return contentTypes[ext];
            return "text/plain";
        }
    }
}
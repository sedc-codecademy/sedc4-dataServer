using DataServer.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServer.Requests;
using System.Text.RegularExpressions;
using DataServer.Entities;
using System.IO;
using Newtonsoft.Json;

namespace DataServer.FileManager
{
    public class FileDetailsGenerator : IResponseGenerator
    {
        public IResponseData GenerateResponse(RequestData request)
        {
            var result = new ResponseData<string>();
            var path = GetPath(request.Location);

            var drive = path[0];
            if (path[1] != '/')
            {
                return new ErrorResponseData(StatusCode.BadRequest);
            }

            var fileName = path.Substring(2);
            var filePath = drive + @":\" + fileName;

            if (!File.Exists(filePath))
            {
                return new ErrorResponseData(StatusCode.NotFound);
            }

            var fileInfo = new FileInfo(filePath);

            var x = new
            {
                LastUsed = fileInfo.LastAccessTimeUtc,
                Size = fileInfo.Length,
                FileName = fileInfo.Name
            };

            result.Payload = JsonConvert.SerializeObject(x);
            result.Headers.Add("Content-type", "application/json");
            return result;
        }

        private string GetPath(string location)
        {
            var indexOfSlash = location.IndexOf("/", 1);
            var path = location.Substring(indexOfSlash + 1);
            return path;
        }
    }
}

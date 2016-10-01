using DataServer.Responses;
using DataServer.Entities;

namespace DataServer.Responses
{
    internal class FileNotFoundResponseData : ResponseData<string>
    {
        public FileNotFoundResponseData(string fileName)
        {
            StatusCode = StatusCode.NotFound;
            Payload = $"file {fileName} was not found";
        }
    }
}
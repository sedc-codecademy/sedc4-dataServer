namespace DemoServer
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
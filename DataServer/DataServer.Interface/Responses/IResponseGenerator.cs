using DataServer.Requests;

namespace DataServer.Responses
{
    public interface IResponseGenerator
    {
        IResponseData GenerateResponse(RequestData request);
    }
}
using DataServer.Entities;

namespace DataServer.Responses
{
    public interface IResponseData<T> : IResponseData
    {
        T Payload { get; }
    }

    public interface IResponseData
    {
        HeaderCollection Headers { get; }
        StatusCode StatusCode { get; }

        byte[] GetBytes();
    }
}
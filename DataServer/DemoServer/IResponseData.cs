namespace DemoServer
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
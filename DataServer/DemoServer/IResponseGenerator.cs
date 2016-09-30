namespace DemoServer
{
    internal interface IResponseGenerator
    {
        IResponseData GenerateResponse(RequestData request);
    }
}
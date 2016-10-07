using DataServer.Responses;
using log4net;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataServer.Requests
{
    internal class RequestHandler
    {
        private Socket socket;
        private GeneratorFactory factory;
        private static readonly ILog log = LogManager.GetLogger(typeof(RequestHandler));


        public RequestHandler(Socket socket, GeneratorFactory factory)
        {
            this.socket = socket;
            this.factory = factory;
        }

        internal async Task AcceptRequest()
        {
            var clientSocket = socket.Accept();
            log.Info("Accepted call");
            await Task.Run(() => HandleRequest(clientSocket));
        }

        private void HandleRequest(Socket clientSocket)
        {
            //get request
            var buffer = new byte[10240];
            var receivedCount = clientSocket.Receive(buffer);
            var request = Encoding.UTF8.GetString(buffer, 0, receivedCount);
            log.Debug(request);

            //parse request
            var requestParser = new RequestParser();
            var parsedRequest = requestParser.ParseRequest(request);

            var responseGenerator = factory.GetResponseGenerator(parsedRequest);
            var myResponse = responseGenerator.GenerateResponse(parsedRequest);
            var responseBytes = myResponse.GetBytes();

            log.Info($"response is {responseBytes.Length} bytes long");

            clientSocket.Send(responseBytes);
            clientSocket.Close();
        }

        
    }
}

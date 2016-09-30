using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemoServer
{
    internal class RequestHandler
    {
        private Socket socket;

        public RequestHandler(Socket socket)
        {
            this.socket = socket;
        }

        internal async Task AcceptRequest()
        {
            var clientSocket = socket.Accept();
            Console.WriteLine("Accepted call");
            await Task.Run(() => HandleRequest(clientSocket));
        }

        private void HandleRequest(Socket clientSocket)
        {
            //get request
            var buffer = new byte[10240];
            var receivedCount = clientSocket.Receive(buffer);
            var request = Encoding.UTF8.GetString(buffer, 0, receivedCount);
            Console.WriteLine(request);

            //parse request
            var requestParser = new RequestParser();
            var parsedRequest = requestParser.ParseRequest(request);

            IResponseGenerator responseGenerator;
            if ((parsedRequest.Method == Method.Get) && (parsedRequest.Location == "/favicon.ico"))
            {
                responseGenerator = new FileResponseGenerator();
            }
            else
            {
                responseGenerator = new StringResponseGenerator();
            }

            var myResponse = responseGenerator.GenerateResponse(parsedRequest);
            var responseBytes = myResponse.GetBytes();

            clientSocket.Send(responseBytes);
            clientSocket.Close();
        }

        
    }
}

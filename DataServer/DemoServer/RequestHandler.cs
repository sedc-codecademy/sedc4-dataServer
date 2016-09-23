using System;
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

            parsedRequest.Headers.Get("Host");


            //return response
            string myResponse = "<h1>HELLO FROM SEDC SERVER</h1>";
            var responseBytes = Encoding.UTF8.GetBytes(myResponse);

            string headers = MakeHeader(responseBytes.Length);
            var headerBytes = Encoding.UTF8.GetBytes(headers);

            clientSocket.Send(headerBytes);
            clientSocket.Send(responseBytes);

            clientSocket.Close();
        }

        private string MakeHeader(int contentLength)
        {
            return $@"HTTP/1.1 200 OK 
Server: SEDC Data Web Server
Content-Length: {contentLength}
Connection: close
Content-Type: text\plain

";
        }
    }
}

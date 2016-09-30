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

            //return response
            if ((parsedRequest.Method == Method.Get) && (parsedRequest.Location == "/favicon.ico"))
            {
                var responseBytes = File.ReadAllBytes("favicon.ico");
                var headers = new HeaderCollection();
                headers.Add("Content-Type", "image/ico");
                string headersString = MakeHeader(headers, responseBytes.Length);
                var headerBytes = Encoding.UTF8.GetBytes(headersString);
                clientSocket.Send(headerBytes);
                clientSocket.Send(responseBytes);
            }
            else
            {
                var responseGenerator = new ResponseGenerator();

                var myResponse = responseGenerator.GenerateResponse(parsedRequest);
                var responseBytes = Encoding.UTF8.GetBytes(myResponse.Body);

                string headers = MakeHeader(myResponse.Headers, responseBytes.Length);
                var headerBytes = Encoding.UTF8.GetBytes(headers);
                clientSocket.Send(headerBytes);
                clientSocket.Send(responseBytes);
            }
            clientSocket.Close();
        }

        private string MakeHeader(HeaderCollection headers, int contentLength)
        {
            StringBuilder sb = new StringBuilder("HTTP/1.1 200 OK\r\nServer: SEDC Data Web Server\r\n");
            sb.AppendLine($"Content-Length: {contentLength}");
            foreach (var header in headers)
            {
                sb.AppendLine($"{header.Name}: {header.Value}");
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }
}

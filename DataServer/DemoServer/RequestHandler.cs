using System;
using System.Net.Sockets;

namespace DemoServer
{
    internal class RequestHandler
    {
        private Socket socket;

        public RequestHandler(Socket socket)
        {
            this.socket = socket;
        }

        internal void AcceptRequest()
        {
            var clientSocket = socket.Accept();
            Console.WriteLine("Accepted call");
            //clientSocket.Send()
        }
    }
}
using DataServer.Requests;
using DataServer.Responses;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DataServer.Server
{
    public class WebServer
    {
        public int Port { get; private set; }
        public GeneratorFactory Factory { get; set; }

        public WebServer(int port)
        {
            Port = port;
            Factory = new GeneratorFactory();
        }

        public void Start()
        {
            Socket s = new Socket(SocketType.Stream, ProtocolType.Tcp);
            s.Bind(new IPEndPoint(IPAddress.Any, Port));
            s.Listen(10);

            int i = 0;
            while (true)
            {
                Console.WriteLine("looping " + i++);
                var requestHandler = new RequestHandler(s, Factory);
                requestHandler.AcceptRequest();
            }
        }
    }
}
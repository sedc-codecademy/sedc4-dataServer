using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DemoServer
{
    internal class WebServer
    {
        public int Port { get; private set; }

        public WebServer(int port)
        {
            Port = port;
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
                var requestHandler = new RequestHandler(s);
                requestHandler.AcceptRequest();
            }
        }
    }
}
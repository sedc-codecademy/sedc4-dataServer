using System;
using System.Net.Sockets;

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
            Socket s = new Socket();
            
        }
    }
}
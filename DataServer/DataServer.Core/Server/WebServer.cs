using DataServer.Requests;
using DataServer.Responses;
using log4net;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DataServer.Server
{
    public class WebServer
    {
        public int port;
        private GeneratorFactory factory;

        private Socket socket;

        private static readonly ILog log = LogManager.GetLogger(typeof(WebServer));

        public WebServer(WebServerConfig config)
        {


            port = config.Port;
            factory = config.Factory ?? new GeneratorFactory();
            if (Directory.Exists(config.Folder))
                Directory.SetCurrentDirectory(config.Folder);
        }

        public void Start()
        {
            log.Info("Starting Service");
            socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            log.Debug($"Bound socket to ipaddress.any and port {port}");
            socket.Listen(10);

            log.Debug("Started listening");

            int i = 0;
            while (true)
            {
                log.Debug("looping " + i++);
                var requestHandler = new RequestHandler(socket, factory);
                log.Debug("Instanciated handler");
                requestHandler.AcceptRequest();
                log.Info("Accepted request");
            }
        }

        public void Stop()
        {
            log.Info("Stopping");
            socket.Disconnect(true);
            socket.Dispose();
        }
    }
}
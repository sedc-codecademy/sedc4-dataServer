using DataServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServer.Responses;
using DataServer.Requests;
using DataServer.MsSql;
using DataServer.FileManager;
using System.Configuration;
using log4net.Config;

namespace DemoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();

            var port = int.Parse(ConfigurationManager.AppSettings["port"]);
            var folder = ConfigurationManager.AppSettings["folder"];

            var config = new WebServerConfig
            {
                Port = port,
                Folder = folder
            };

            config.Factory.RegisterGenerator<MsSqlResponseGenerator>("sql-server");
            config.Factory.RegisterGenerator<FileDetailsGenerator>("file");
            var ws = new WebServer(config);
            ws.Start();
        }
    }
}

using DataServer.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServer.Server
{
    public class WebServerConfig
    {
        public int Port { get; set; }
        public string Folder { get; set; }
        public GeneratorFactory Factory { get; set; }

        public WebServerConfig()
        {
            Factory = new GeneratorFactory();
        }

        public static WebServerConfig Default { get; private set; }

        static WebServerConfig()
        {
            Default = new WebServerConfig
            {
                Port = 8082,
                Factory = new GeneratorFactory(),
                Folder = Directory.GetCurrentDirectory()
            };
        }
    }
}

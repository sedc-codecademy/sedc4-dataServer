using DataServer.FileManager;
using DataServer.MsSql;
using DataServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DemoServerService
{
    public partial class DataServerService : ServiceBase
    {

        private WebServer webServer;

        public DataServerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var config = new WebServerConfig
            {
                Port = 8082,
                Folder = @"C:\Source\SEDC\sedc4-dataServer\DataServer\Site"
            };

            config.Factory.RegisterGenerator<MsSqlResponseGenerator>("sql-server");
            config.Factory.RegisterGenerator<FileDetailsGenerator>("file");

            webServer = new WebServer(config); 
            Task.Run(() => { webServer.Start(); });
        }

        protected override void OnStop()
        {
            webServer.Stop();
        }
    }
}

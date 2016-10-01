﻿using DataServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServer.Responses;
using DataServer.Requests;
using DataServer.MsSql;

namespace DemoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var port = 8082;
            WebServer ws = new WebServer(port);
            ws.Factory.RegisterGenerator<MsSqlResponseGenerator>("mssql");
            ws.Start();
        }
    }
}

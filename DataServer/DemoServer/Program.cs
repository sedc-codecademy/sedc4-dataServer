using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var port = 8082;
            WebServer ws = new WebServer(port);
            ws.Start();
        }
    }
}

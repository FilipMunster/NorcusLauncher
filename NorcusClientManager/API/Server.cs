using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grapevine;

namespace NorcusClientManager.API
{
    public class Server
    {
        public Server()
        {
            var server = RestServerBuilder.From<Startup>().Build();
            server.Start();
        }
    }
}

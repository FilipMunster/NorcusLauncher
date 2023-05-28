using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grapevine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NorcusLauncher;

namespace NorcusClientManager.API
{
    public class Server
    {
        public Server(ILauncher launcher)
        {
            RestServerBuilder serverBuilder = RestServerBuilder.UseDefaults();
            serverBuilder.Services.AddSingleton<ILauncher>(launcher);
            serverBuilder.Services.AddLogging(loggingBuilder => 
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddNLog("NLog.config");
                });
            
            IRestServer server = serverBuilder.Build();
            server.Prefixes.Clear();
            server.Prefixes.Add($"https://+:443/");
            server.AutoParseFormUrlEncodedData();
            server.Router.Options.SendExceptionMessages = true;
            
            server.Start();
        }
    }
}

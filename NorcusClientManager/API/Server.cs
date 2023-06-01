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
        private IRestServer _server;
        public Server(ILauncher launcher, int port, string secureKey)
        {
            RestServerBuilder serverBuilder = RestServerBuilder.UseDefaults();
            serverBuilder.Services.AddSingleton<ILauncher>(launcher);
            serverBuilder.Services.AddSingleton<ITokenAuthenticator>(new JWTAuthenticator(secureKey));
            serverBuilder.Services.AddLogging(loggingBuilder => 
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddNLog("NLog.config");
                });

            _server = serverBuilder.Build();
            _server.Prefixes.Clear();
            //_server.Prefixes.Add($"https://+:{port}/");
            _server.Prefixes.Add($"http://localhost:{port}/");
            _server.AutoParseFormUrlEncodedData();
            _server.Router.Options.SendExceptionMessages = true;
        }
        public void Start() => _server.Start();
        public void Stop() => _server.Stop();
    }
}

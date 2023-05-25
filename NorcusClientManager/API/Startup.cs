using System;
using System.IO;
using Grapevine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace NorcusClientManager.API
{
    internal class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddNLog(new NLogLoggingConfiguration(Configuration.GetSection("NLog")));
            });
        }

        public void ConfigureServer(IRestServer server)
        {
            server.Prefixes.Add($"https://+:443/");

            /* Configure server to auto parse application/x-www-for-urlencoded data*/
            server.AutoParseFormUrlEncodedData();

            /* Configure Router Options (if supported by your router implementation) */
            server.Router.Options.SendExceptionMessages = true;
        }
    }
}
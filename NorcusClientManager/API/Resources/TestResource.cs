using Grapevine;
using Microsoft.Extensions.Logging;
using NorcusLauncher;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorcusClientManager.API.Resources
{
    [RestResource]
    public class TestResource
    {
        private readonly ILauncher _launcher;
        private readonly ILogger _logger;
        public TestResource(ILauncher launcher, ILogger<TestResource> logger)
        {
            _launcher = launcher;
            _logger = logger;
        }
        [RestRoute("Get", "/api/test")] 
        public async Task Test(IHttpContext context)
        {
            StringBuilder clients = new StringBuilder();
            foreach (var client in _launcher.Clients)
            {
                clients.AppendLine(client.ToString());
            }
            await context.Response.SendResponseAsync(clients.ToString()).ConfigureAwait(false);
            _logger.LogDebug("API TEST OK");
        }
    }
}

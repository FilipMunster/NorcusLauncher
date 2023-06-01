using Grapevine;
using Microsoft.Extensions.Logging;
using NorcusClientManager.API.Models;
using NorcusLauncher;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorcusClientManager.API.Resources
{
    [RestResource(BasePath = "api/v1/machine")]
    public class MachineResource
    {
        private readonly ILauncher _launcher;
        private readonly ILogger _logger;
        private readonly ITokenAuthenticator _authenticator;
        public MachineResource(ILauncher launcher, ILogger<ClientsResource> logger, ITokenAuthenticator authenticator)
        {
            _launcher = launcher;
            _logger = logger;
            _authenticator = authenticator;
        }

        [RestRoute("Post", "shutdown")]
        public async Task Shutdown(IHttpContext context)
        {
            context.Response.StatusCode = HttpStatusCode.Ok;
            await context.Response.SendResponseAsync();

            _launcher.StopClients();
            Process.Start("shutdown", "/s /f /t 1");
        }
        [RestRoute("Post", "restart")]
        public async Task Restart(IHttpContext context)
        {
            context.Response.StatusCode = HttpStatusCode.Ok;
            await context.Response.SendResponseAsync();

            Process.Start("shutdown", "/r /f /t 1");
        }
    }
}

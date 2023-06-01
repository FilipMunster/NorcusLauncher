using Grapevine;
using Microsoft.Extensions.Logging;
using NorcusClientManager.API.Models;
using NorcusLauncher;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorcusClientManager.API.Resources
{
    [RestResource(BasePath ="api/v1/clients")]
    public class ClientsResource
    {
        private readonly ILauncher _launcher;
        private readonly ILogger _logger;
        private readonly ITokenAuthenticator _authenticator;
        private readonly ClientsModel _clientsModel;
        public ClientsResource(ILauncher launcher, ILogger<ClientsResource> logger, ITokenAuthenticator authenticator)
        {
            _launcher = launcher;
            _logger = logger;
            _authenticator = authenticator;
            _clientsModel = new ClientsModel(launcher);
        }

        [RestRoute("Get", "")]
        [RestRoute("Get", "{id:num}")]
        public async Task GetClients(IHttpContext context)
        {
            string jwtToken = context.Request.Headers.GetValue<string>("Authorization").Split(' ')[1];
            bool tokenValid = _authenticator.IsTokenValid(jwtToken);

            int? id = _clientsModel.GetIdFromPath(context.Request.PathParameters, out bool idOutOfRange);
            if (idOutOfRange)
            {
                context.Response.StatusCode = HttpStatusCode.BadRequest;
                await context.Response.SendResponseAsync();
                return;
            }

            string response = id.HasValue ? _clientsModel.GetClient(id.Value) : _clientsModel.GetClients();

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = HttpStatusCode.Ok;
            await context.Response.SendResponseAsync(response).ConfigureAwait(false);
        }

        [RestRoute("Post", "run")]
        [RestRoute("Post", "{id:num}/run")]
        public async Task RunClients(IHttpContext context)
        {
            int? id = _clientsModel.GetIdFromPath(context.Request.PathParameters, out bool idOutOfRange);
            if (idOutOfRange)
            {
                context.Response.StatusCode = HttpStatusCode.BadRequest;
                await context.Response.SendResponseAsync();
                return;
            }

            if (id.HasValue) _launcher.Clients[id.Value].Run();
            else _launcher.RunClients();

            context.Response.StatusCode = HttpStatusCode.Ok;
            await context.Response.SendResponseAsync();
        }

        [RestRoute("Post", "stop")]
        [RestRoute("Post", "{id:num}/stop")]
        public async Task StopClients(IHttpContext context)
        {
            int? id = _clientsModel.GetIdFromPath(context.Request.PathParameters, out bool idOutOfRange);
            if (idOutOfRange)
            {
                context.Response.StatusCode = HttpStatusCode.BadRequest;
                await context.Response.SendResponseAsync();
                return;
            }

            if (id.HasValue) _launcher.Clients[id.Value].Stop();
            else _launcher.StopClients();

            context.Response.StatusCode = HttpStatusCode.Ok;
            await context.Response.SendResponseAsync();
        }

        [RestRoute("Post", "restart")]
        [RestRoute("Post", "{id:num}/restart")]
        public async Task RestartClients(IHttpContext context)
        {
            int? id = _clientsModel.GetIdFromPath(context.Request.PathParameters, out bool idOutOfRange);
            if (idOutOfRange)
            {
                context.Response.StatusCode = HttpStatusCode.BadRequest;
                await context.Response.SendResponseAsync();
                return;
            }

            if (id.HasValue) _launcher.Clients[id.Value].Restart();
            else _launcher.RestartClients();

            context.Response.StatusCode = HttpStatusCode.Ok;
            await context.Response.SendResponseAsync();
        }

        [RestRoute("Post", "identify")]
        [RestRoute("Post", "{id:num}/identify")]
        public async Task IdentifyClients(IHttpContext context)
        {
            int? id = _clientsModel.GetIdFromPath(context.Request.PathParameters, out bool idOutOfRange);
            if (idOutOfRange)
            {
                context.Response.StatusCode = HttpStatusCode.BadRequest;
                await context.Response.SendResponseAsync();
                return;
            }

            if (id.HasValue) _launcher.Clients[id.Value].IdentifyDisplay();
            else _launcher.IdentifyDisplays();

            context.Response.StatusCode = HttpStatusCode.Ok;
            await context.Response.SendResponseAsync();
        }
    }
}

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
        public async Task GetClients(IHttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = HttpStatusCode.Ok;
            await context.Response.SendResponseAsync(_clientsModel.GetClients()).ConfigureAwait(false);
        }

        [RestRoute("Get", "{id:num}")]
        public async Task GetClient(IHttpContext context)
        {
            bool idOk = int.TryParse(context.Request.PathParameters["id"], out int id);
            if (!idOk || id > _launcher.Clients.Count)
            {
                context.Response.StatusCode = HttpStatusCode.BadRequest;
                await context.Response.SendResponseAsync();
                return;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = HttpStatusCode.Ok;
            await context.Response.SendResponseAsync(_clientsModel.GetClient(id)).ConfigureAwait(false);
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

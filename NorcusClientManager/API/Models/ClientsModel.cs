using NorcusLauncher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using NorcusLauncher.Clients;
using System.Reflection;

namespace NorcusClientManager.API.Models
{
    internal class ClientsModel
    {
        private ILauncher _launcher;
        public ClientsModel(ILauncher launcher)
        {
            _launcher = launcher;
        }
        public string GetClients()
        {
            List<Client> clients = new();
            for (int i = 0; i < _launcher.Clients.Count; i++)
            {
                clients.Add(new Client(i, _launcher.Clients[i]));
            }
            return JsonSerializer.Serialize<List<Client>>(clients);
        }
        public string GetClient(int index)
        {
            if (index >= _launcher.Clients.Count) return "";
            return JsonSerializer.Serialize<Client>(new Client(index, _launcher.Clients[index]));
        }

        public int? GetIdFromPath(IDictionary<string, string> pathParameters, out bool idOutOfRange)
        {
            idOutOfRange = false;
            if (!pathParameters.TryGetValue("id", out string? idString)) return null;
            if (!int.TryParse(idString, out int id)) return null;
            if (id >= _launcher.Clients.Count) idOutOfRange = true;
            return id;
        }

        public class Client
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Display { get; set; }
            public bool IsRunning { get; set; }
            public Client(int id, ClientProcess clientProcess)
            {
                Id = id;
                Name = clientProcess.ClientInfo.ToString();
                Display = clientProcess.Display?.ToString() ?? "";
                IsRunning = clientProcess.IsRunning;
            }
        }
    }
}

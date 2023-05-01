using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorcusLauncher.Clients
{
    public class Client : IClient
    {
        public string Name { get; set; } = "";
        public string AppId { get; set; } = "";
        public string DisplayDeviceKey { get; set; } = "";
        public Client() { }
        public Client(string name, string appId, string displayDeviceKey)
        {
            Name = name;
            AppId = appId;
            DisplayDeviceKey = displayDeviceKey;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorcusLauncher.Clients
{
    public class Client
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string DisplayDeviceKey { get; set; }
        public StartupMode StartMode { get; set; }
        public enum StartupMode
        {
            None = 0,
            FullScreen = 'F',
            Kiosk = 'K'
        }

        public Client() { }
        public Client(string name, string url, StartupMode startupMode, string displayDeviceKey)
        {
            Name = name;
            Url = url;
            StartMode = startupMode;
            DisplayDeviceKey = displayDeviceKey;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}

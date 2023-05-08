using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorcusLauncher.Clients
{
    public class ClientInfo : IClient
    {
        public string Name { get; set; } = "";
        public string DisplayDeviceKey { get; set; } = "";
        public Mode StartMode { get; set; } = Mode.None;

        public ClientInfo() { }
        public ClientInfo(string name, Mode startMode, string displayDeviceKey)
        {
            Name = name;
            StartMode = startMode;
            DisplayDeviceKey = displayDeviceKey;
        }
        public override string ToString()
        {
            return Name;
        }
        public enum Mode
        {
            None = 'N',
            FullScreen = 'F',
            Kiosk = 'K'
        }
    }
}

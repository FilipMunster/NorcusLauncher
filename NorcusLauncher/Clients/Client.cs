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
        public string DisplayDeviceKey { get; set; } = "";
        public Mode StartMode { get; set; }
        string IClient.StartMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Client() { }
        public Client(string name, Mode startMode, string displayDeviceKey)
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorcusLauncher.Clients
{
    public class ClientInfo
    {
        public string Name { get; set; } = "";
        public string DisplayID { get; set; } = "";
        public Mode StartMode { get; set; } = Mode.None;

        public ClientInfo() { }
        public ClientInfo(string name, Mode startMode, string displayID)
        {
            Name = name;
            StartMode = startMode;
            DisplayID = displayID;
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

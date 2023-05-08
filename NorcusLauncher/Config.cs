using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NorcusLauncher.Clients;

namespace NorcusLauncher
{
    public class Config : IConfig
    {
        public string ChromePath { get; set; }
            = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Google\Chrome\Application\chrome.exe";
        public string ProfilesPath { get; set; }
            = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\";
        public string ServerUrl { get; set; } = "";
        public int ProcessExitTimeout { get; set; } = 2000;
        public int WindowHandleTimeout { get; set; } = 10000;
        public int IdentifierTimeout { get; set; } = 5000;
        public List<ClientInfo> ClientInfos { get; set; } = new List<ClientInfo>();
    }
}

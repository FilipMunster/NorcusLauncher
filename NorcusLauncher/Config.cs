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
        public string ProfilesPath { get; set; }
            = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\";
        public string ServerUrl { get; set; } = "";
        public int ProcessExitTimeout { get; set; } = 2000;
        public int WindowHandleTimeout { get; set; } = 10000;
        public int IdentifierTimeout { get; set; } = 5000;
        public List<ClientInfo> ClientInfos { get; set; } = new List<ClientInfo>();
        public Config()
        {
            string chromeRelPath = @"\Google\Chrome\Application\chrome.exe";
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + chromeRelPath))
                ChromePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + chromeRelPath;
            else if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + chromeRelPath))
                ChromePath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + chromeRelPath;
            else
                ChromePath = "";
        }
    }
}

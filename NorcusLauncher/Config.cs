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
    public class Config
    {
        public string ChromePath { get; set; } 
            = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Google\Chrome\Application\chrome.exe";
        public string ProfilesPath { get; set; }
            = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\";
        public List<Client> Clients { get; set; } = new List<Client>();
        public Config() { }
        public static string GetDefaultConfigFilePath() =>
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" +
            Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location) + "Cfg.xml";

        public static Config Load() => Load(GetDefaultConfigFilePath());
        public static Config Load(string configFilePath)
        {
            if (!System.IO.File.Exists(configFilePath))
                return null;

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Config));
            System.IO.FileStream file = System.IO.File.OpenRead(configFilePath);
            object deserialized = serializer.Deserialize(file);

            file.Close();
            return deserialized as Config;
        }

        public static void Save(Config config) => Save(config, GetDefaultConfigFilePath());
        public static void Save(Config config, string configFilePath)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(Config));

            System.IO.FileStream file = System.IO.File.Create(configFilePath);

            serializer.Serialize(file, config);
            file.Close();
        }
        public void Save() => Save(GetDefaultConfigFilePath());
        public void Save(string configFilePath) => Save(this, configFilePath);
    }
}

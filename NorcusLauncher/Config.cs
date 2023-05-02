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
        public string ServerUrl { get; set; } = "";
        public int WaitForExit { get; set; } = 2000;
        public int WaitForWindowHandle { get; set; } = 10000;
        public int IdentifierTimeout { get; set; } = 5000;
        public List<Client> Clients { get; set; } = new List<Client>();
        public Config() { }
        public static string GetDefaultConfigFilePath() =>
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" +
            Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location) + "Cfg.xml";

        public static Config Load() => Load(GetDefaultConfigFilePath());
        public static Config Load(string configFilePath)
        {
            if (!System.IO.File.Exists(configFilePath))
            {
                if (System.IO.File.Exists(configFilePath + "_temp"))
                    return Load(configFilePath + "_temp");
                else
                    return null;
            }

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Config));
            System.IO.FileStream file = System.IO.File.OpenRead(configFilePath);
            Config deserialized = null;
            try { deserialized = serializer.Deserialize(file) as Config; }
            catch { }
            file.Close();

            if (deserialized is null)
                return Load(configFilePath + "_temp");

            if (configFilePath.EndsWith("_temp"))
            {
                System.IO.File.Copy(configFilePath, configFilePath.Substring(0, configFilePath.Length - 5), true);
                System.IO.File.Delete(configFilePath);
            }

            return deserialized;
        }

        public static void Save(Config config) => Save(config, GetDefaultConfigFilePath());
        public static void Save(Config config, string configFilePath)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(Config));

            System.IO.FileStream file = System.IO.File.Create(configFilePath + "_temp");

            serializer.Serialize(file, config);
            file.Close();

            System.IO.File.Copy(configFilePath + "_temp", configFilePath, true);
            System.IO.File.Delete(configFilePath + "_temp");
        }
        public void Save() => Save(GetDefaultConfigFilePath());
        public void Save(string configFilePath) => Save(this, configFilePath);
    }
}

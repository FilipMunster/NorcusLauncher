using NorcusLauncher;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NorcusClientManager
{
    /// <summary>
    /// Norcus Client Manager Config
    /// </summary>
    public class Config
    {
        [XmlElement(Namespace = nameof(NorcusLauncher))]
        public NorcusLauncher.Config LauncherConfig { get; set; } = new NorcusLauncher.Config();
        public bool RunOnStartup { get; set; }
        public bool StartInTray { get; set; }
        public bool AutoLaunch { get; set; }

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
                    return new Config();
            }

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Config));
            System.IO.FileStream file = System.IO.File.OpenRead(configFilePath);
            Config deserialized = null;
            try { deserialized = (Config)serializer.Deserialize(file); }
            catch { }
            file.Close();

            if (deserialized is null)
                return Load(configFilePath + "_temp");

            if (configFilePath.EndsWith("_temp"))
                System.IO.File.Move(configFilePath, configFilePath.Substring(0, configFilePath.Length - 5), true);

            return deserialized ?? new Config();
        }

        public void Save() => Save(GetDefaultConfigFilePath());
        public void Save(string configFilePath)
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(Config));

            System.IO.FileStream file = System.IO.File.Create(configFilePath + "_temp");

            serializer.Serialize(file, this);
            file.Close();

            System.IO.File.Move(configFilePath + "_temp", configFilePath, true);
        }

    }
}

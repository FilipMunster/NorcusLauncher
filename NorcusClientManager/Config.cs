using Microsoft.Win32;
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
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        [XmlElement(Namespace = nameof(NorcusLauncher))]
        public NorcusLauncher.Config LauncherConfig { get; set; } = new NorcusLauncher.Config();
        public bool RunOnStartup { get; set; }
        public bool StartInTray { get; set; }
        public bool AutoLaunch { get; set; }
        public bool AutoIdentify { get; set; }
        public bool APIAutoStart { get; set; }
        public int APIPort { get; set; } = 443;
        public string APIKey { get; set; } = "";

        public static string GetDefaultConfigFilePath() =>
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" +
            Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location) + "Cfg.xml";
        public static Config Load() => Load(GetDefaultConfigFilePath());
        public static Config Load(string configFilePath)
        {
            _logger.Info("Loading config file: " + configFilePath);
            if (!System.IO.File.Exists(configFilePath))
            {
                if (System.IO.File.Exists(configFilePath + "_temp"))
                    return Load(configFilePath + "_temp");
                else
                {
                    _logger.Warn("File was not found ({0}), returning new Config", configFilePath);
                    return new Config();
                }
            }

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Config));
            System.IO.FileStream file = System.IO.File.OpenRead(configFilePath);
            Config deserialized = null;
            try { deserialized = (Config)serializer.Deserialize(file); }
            catch (Exception e) { _logger.Warn(e, "Deserialization failed"); }
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
            _logger.Debug("Saving config file to {0}", configFilePath);
            _SaveRegistry();

            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(Config));

            System.IO.FileStream file = System.IO.File.Create(configFilePath + "_temp");

            serializer.Serialize(file, this);
            file.Close();

            System.IO.File.Move(configFilePath + "_temp", configFilePath, true);
        }

        private void _SaveRegistry()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (RunOnStartup)
                key?.SetValue("NorcusClientManager", 
                    "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location.Replace(".dll", ".exe") + "\"");
            else
                key?.DeleteValue("NorcusClientManager", false);
        }

    }
}

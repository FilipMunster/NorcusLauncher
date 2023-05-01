using NorcusLauncher;
using NorcusLauncher.Clients;
using NorcusLauncher.Displays;

// Načtení konfigurace a spuštění klientů
Config config = Config.Load();
if (config == null)
    config = CreateConfig();

Launcher launcher = new Launcher(config, false);
Console.ReadKey();
launcher.IdentifyDisplays();
Console.ReadKey();
launcher.StopClients();

Config CreateConfig()
{
    //// Vytvoření konfiguračního souboru
    Config config = new Config();
    DisplayHandler displayHandler = new DisplayHandler();
    Client client = new Client("Kytara", "unknown", displayHandler.Displays[0].DeviceKey);
    Client client2 = new Client("Zpevacka", "unknown", displayHandler.Displays[2].DeviceKey);
    config.Clients.Add(client);
    config.Clients.Add(client2);
    config.Save();
    return config;
}
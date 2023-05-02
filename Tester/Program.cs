using NorcusLauncher;
using NorcusLauncher.Clients;
using NorcusLauncher.Displays;
using System.Diagnostics;

// Načtení konfigurace a spuštění klientů
Config config = Config.Load();
if (config == null)
    config = CreateConfig();

Launcher launcher = new Launcher(config, true);
//Console.ReadKey();
//launcher.IdentifyDisplays();
//Console.ReadKey();
//launcher.RunClients();
while (true)
{
    //Console.ReadKey();
    //launcher.RestartClients();
    //launcher.Clients.ForEach(x => x.Process.Kill());
    //Console.ReadKey();
    //launcher.RunClients();
    //Console.ReadKey();
    //launcher.Config.Clients[0].StartMode = Client.Mode.FullScreen;
    //launcher.RefreshClients();
    Console.ReadKey();
    launcher.RestartClients();
}

Config CreateConfig()
{
    //// Vytvoření konfiguračního souboru
    Config config = new Config();
    config.ServerUrl = "https://norcus.local:59825";
    DisplayHandler displayHandler = new DisplayHandler();
    Client client = new Client("Kytara", Client.Mode.None, displayHandler.Displays[0].DeviceKey);
    Client client2 = new Client("Zpevacka", Client.Mode.None, displayHandler.Displays[2].DeviceKey);
    config.Clients.Add(client);
    config.Clients.Add(client2);
    config.Save();
    return config;
}
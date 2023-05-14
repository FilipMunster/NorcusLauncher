using NorcusLauncher;
using NorcusLauncher.Clients;
using NorcusLauncher.Displays;
using NorcusClientManager;
using System.Diagnostics;



NorcusLauncher.Config CreateConfig()
{
    //// Vytvoření konfiguračního souboru
    NorcusLauncher.Config config = new NorcusLauncher.Config();
    config.ServerUrl = "https://norcus.local:59825";
    DisplayHandler displayHandler = new DisplayHandler();
    ClientInfo client = new ClientInfo("Kytara", ClientInfo.Mode.None, displayHandler.Displays[0].DisplayID);
    ClientInfo client2 = new ClientInfo("Zpevacka", ClientInfo.Mode.None, displayHandler.Displays[2].DisplayID);
    config.ClientInfos.Add(client);
    config.ClientInfos.Add(client2);
    return config;
}
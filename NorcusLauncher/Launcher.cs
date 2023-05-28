using NorcusLauncher.Clients;
using NorcusLauncher.Displays;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorcusLauncher
{
    public class Launcher : ILauncher
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public List<ClientProcess> Clients { get; private set; } = new List<ClientProcess>();
        public DisplayHandler DisplayHandler { get; private set; } = new DisplayHandler();
        public IConfig Config { get; private set; }
        public bool ClientsAreRunning { get; private set; } = false;

        /// <summary>
        /// Stará se o vytvoření klientů a jejich přiřazení k displejům. Umožňuje hromadné akce nad klienty (Run, Restart, Stop).
        /// </summary>
        /// <param name="config"></param>
        public Launcher(Config config)
        {
            Config = config;
            _AddBlankDisplaysToHandler();
            RefreshClients();
            DisplayHandler.DisplayChanged += DisplayHandler_DisplayChanged;
        }

        private void DisplayHandler_DisplayChanged(object sender, DisplayHandler.DisplayChangeEventArgs e)
        {
            foreach (var disp in e.UpdatedDisplays)
            {
                Clients.ForEach(cli => { if (cli.Display == disp) cli.UpdateWindowPosition(); });
            }
            foreach (var disp in e.AddedDisplays)
            {
                Clients.ForEach(cli => { if (cli.Display == disp && ClientsAreRunning) cli.Run(); });
            }
            foreach (var disp in e.RemovedDisplays)
            {
                Clients.ForEach(cli => { if (cli.Display == disp) cli.Stop(); });
            }
        }

        /// <summary>
        /// Zastaví všechny klienty a načte aktuální konfiguraci. Znovu naplní List <see cref="Clients"/> a přiřadí ke klientům jejich displeje.
        /// </summary>
        public void RefreshClients()
        {
            _logger.Debug("Refreshing clients");
            Clients.ForEach(cli => cli.Stop());

            Clients.Clear();
            foreach (var client in Config.ClientInfos)
            {
                ClientProcess cliProc = new ClientProcess(client, Config);
                cliProc.Display = DisplayHandler.Displays.FirstOrDefault(x => x.DisplayID == cliProc.ClientInfo.DisplayID)
                    ?? new Display(cliProc.ClientInfo.DisplayID);
                Clients.Add(cliProc);
            }

            if (ClientsAreRunning) RunClients();
        }
        public void RunClients() { _logger.Debug("Starting all clients"); ClientsAreRunning = true; Clients.ForEach(cli => cli.Run()); }
        public void RestartClients() { _logger.Debug("Restarting all clients"); Clients.ForEach(cli => cli.Restart()); }
        public void StopClients() { _logger.Debug("Stopping all clients"); ClientsAreRunning = false; Clients.ForEach(cli => cli.Stop()); }
        public void IdentifyDisplays() { _logger.Debug("Identifying all displays"); Clients.ForEach(cli => cli.IdentifyDisplay()); }

        /// <summary>
        /// Přidá do kolekce displejů v Handleru displeje načtené z Configu. Zamezí duplicitním displejům.
        /// </summary>
        /// <param name="displayID"></param>
        private void _AddBlankDisplaysToHandler()
        {
            var currentIds = DisplayHandler.Displays.Select(d => d.DisplayID).ToList();
            var configIds = Config.ClientInfos.Select(x => x.DisplayID);

            foreach (var id in configIds)
            {
                if (!currentIds.Contains(id))
                {
                    DisplayHandler.Displays.Add(new Display(id));
                    _logger.Debug("Display {0} is not connected. It was added as blank display.", id);
                }
            }
        }
    }
}

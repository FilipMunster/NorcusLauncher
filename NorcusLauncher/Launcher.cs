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
    public class Launcher
    {
        public List<ClientProcess> Clients { get; private set; } = new List<ClientProcess>();
        public DisplayHandler DisplayHandler { get; private set; } = new DisplayHandler();
        public IConfig Config { get; private set; }

        /// <summary>
        /// Stará se o vytvoření klientů a jejich přiřazení k displejům. Umožňuje hromadné akce nad klienty (Run, Restart, Stop).
        /// </summary>
        /// <param name="config"></param>
        public Launcher(Config config)
        {
            Config = config;
            RefreshClients();
            DisplayHandler.DisplayChanged += DisplayHandler_DisplayChanged;
        }

        private void DisplayHandler_DisplayChanged(object sender, DisplayHandler.DisplayChangeEventArgs e)
        {
            foreach (var disp in e.AddedDisplays)
            {
                Clients.ForEach(x => { if (x.Display == disp) x.Run(); });
            }

            foreach (var disp in e.RemovedDisplays)
            {
                Clients.ForEach(x => { if (x.Display == disp) x.Stop(); });
            }
        }

        /// <summary>
        /// Zastaví všechny klienty a načte aktuální konfiguraci. Znovu naplní List <see cref="Clients"/> a přiřadí ke klientům jejich displeje.
        /// </summary>
        public void RefreshClients()
        {
            bool allRunning = Clients.Count > 0 && Clients.All(x => x.IsRunning);
            StopClients();

            List<ClientProcess> noDisplayClients = new List<ClientProcess>();
            Clients.Clear();
            foreach (var client in Config.ClientInfos)
            {
                ClientProcess cliProc = new ClientProcess(client, Config);
                cliProc.Display = DisplayHandler.Displays.FirstOrDefault(x => x.DeviceKey == cliProc.ClientInfo.DisplayDeviceKey);
                Clients.Add(cliProc);
                if (cliProc.Display is null)
                    noDisplayClients.Add(cliProc);
            }

            if (noDisplayClients.Count > 0)
            {
                string msg = "Někteří klienti nemají přiřazený displej:\n";
                foreach (var client in noDisplayClients)
                {
                    msg += $"{client.ClientInfo.Name} - {client.ClientInfo.DisplayDeviceKey}\n";
                }
                MessageBox.Show(msg, "Chybí displej", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (allRunning) RunClients();
        }
        public void RunClients() => Clients.ForEach(cli => cli.Run());
        public void RestartClients() => Clients.ForEach(cli => cli.Restart());
        public void StopClients() => Clients.ForEach(cli => cli.Stop());
        public void IdentifyDisplays() => Clients.ForEach(cli => cli.IdentifyDisplay());
    }
}

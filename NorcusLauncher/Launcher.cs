using NorcusLauncher.Clients;
using NorcusLauncher.Displays;
using System;
using System.Collections.Generic;
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
        public Config Config { get; private set; }
        private bool __autoRun;

        /// <summary>
        /// Pro všechny klienty nastaví, zda se budou automaticky spouštět po připojení displeje.
        /// </summary>
        public bool AutoRun
        {
            get => __autoRun;
            set 
            { 
                __autoRun = value;
                Clients.ForEach(client => client.AutoRun = value);
            }
        }

        /// <summary>
        /// Stará se o vytvoření klientů a jejich přiřazení k displejům. Umožňuje hromadné akce nad klienty (Run, Restart, Stop).
        /// </summary>
        /// <param name="config"></param>
        public Launcher(Config config, bool autoRun = false)
        {
            Config = config;
            RefreshClients();
            AutoRun = autoRun;
            DisplayHandler.DisplayChanged += DisplayHandler_DisplayChanged;
        }

        private void DisplayHandler_DisplayChanged(object sender, DisplayHandler.DisplayChangeEventArgs e)
        {
            foreach (var disp in e.AddedDisplays)
            {
                Clients.ForEach(x => { if (x.Display == disp && x.AutoRun) x.Run(); });
            }

            foreach (var disp in e.RemovedDisplays)
            {
                Clients.ForEach(x => { if (x.Display == disp) x.Stop(); });
            }
        }

        /// <summary>
        /// Zastaví všechny klienty a načte aktuální konfiguraci. Vytvoří nový List <see cref="Clients"/> a přiřadí ke klientům jejich displeje.
        /// </summary>
        public void RefreshClients()
        {
            foreach (var client in Clients)
            {
                client.Stop();
            }

            List<ClientProcess> noDisplayClients = new List<ClientProcess>();
            Clients.Clear();
            foreach (var client in Config.Clients)
            {
                ClientProcess cliProc = new ClientProcess(client, Config);
                cliProc.Display = DisplayHandler.Displays.FirstOrDefault(x => x.DeviceKey == cliProc.DisplayDeviceKey);
                Clients.Add(cliProc);
                if (cliProc.Display is null)
                    noDisplayClients.Add(cliProc);
            }

            if (noDisplayClients.Count > 0)
            {
                string msg = "Někteří klienti nemají přiřazený displej:\n";
                foreach (var client in noDisplayClients)
                {
                    msg += $"{client.Name} - {client.DisplayDeviceKey}\n";
                }
                MessageBox.Show(msg, "Chybí displej", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            AutoRun = AutoRun;
        }
        public void RunClients() => Clients.ForEach(cli => cli.Run());
        public void RestartClients() => Clients.ForEach(cli => cli.Restart());
        public void StopClients() => Clients.ForEach(cli => cli.Stop());
        public void IdentifyDisplays() => Clients.ForEach(cli => cli.IdentifyDisplay());
    }
}

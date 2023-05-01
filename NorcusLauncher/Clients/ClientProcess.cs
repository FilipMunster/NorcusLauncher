using NorcusLauncher.Displays;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorcusLauncher.Clients
{
    public class ClientProcess : Client
    {
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        const int SWP_SHOWWINDOW = 0x0040;
        public Process Process { get; private set; } = new Process();
        public Display Display { get; set; }
        public bool IsRunning => Process.IsRunning();
        public bool CanRun => Display != null && Display.IsConnected;
        private bool __autoRun = false;
        /// <summary>
        /// Určuje, zda při připojení obrazovky bude klient automaticky spuštěn
        /// </summary>
        public bool AutoRun
        {
            get => __autoRun;
            set
            {
                __autoRun = value;
                if (value) Run();
            }
        }
        private Config _Config { get; set; }
        public ClientProcess(Client client, Config config)
            : base(client.Name, client.AppId, client.DisplayDeviceKey) 
        {
            _Config = config;
        }
        public void Run()
        {
            if (Display is null || !Display.IsConnected || this.IsRunning) return;

            string arguments = $"--profile-directory=\"{Name}\" " +
                               $"--user-data-dir=\"{_Config.ProfilesPath}User Data - {Name}\" " +
                               $"--app-id={AppId}";

            Process.StartInfo = new ProcessStartInfo(_Config.ChromePath, arguments);
            _StartProcessOnPosition(Process, Display.ScreenBounds);
        }
        public void Stop()
        {
            if (!Process.IsRunning()) return;
            
            if (Process.Responding)
            {
                Process.CloseMainWindow();
                Process.WaitForExit(5000);
            }

            if (Process.IsRunning())
                Process.Kill();
        }
        public void Restart()
        {
            Stop();
            Run();
        }
        public void IdentifyDisplay(TimeSpan timeout = default)
        {
            if (!Display.IsConnected)
                return;
            if (timeout == default)
                timeout = new TimeSpan(0, 0, 5);
            Display.Identify(timeout, this.Name);
        }
        private void _StartProcessOnPosition(Process process, Rectangle windowPosition)
        {
            process.Start();
            DateTime startTime = DateTime.Now;
            while (process.MainWindowHandle == IntPtr.Zero && (DateTime.Now - startTime).TotalSeconds <= 10)
            {
                Thread.Sleep(100);
            }
            SetWindowPos(process.MainWindowHandle, 0, windowPosition.Left, windowPosition.Top, windowPosition.Width, windowPosition.Height, SWP_SHOWWINDOW);
        }
    }
}

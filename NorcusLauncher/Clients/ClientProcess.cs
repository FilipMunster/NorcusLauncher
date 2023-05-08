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
    public class ClientProcess
    {
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        const int SWP_SHOWWINDOW = 0x0040;
        public ClientInfo ClientInfo { get; set; }
        public Process Process { get; private set; } = new Process();
        public Display? Display { get; set; }
        public bool IsRunning => Process.IsRunning();
        public bool CanRun => Display != null && Display.IsConnected;
        private IConfig _Config { get; set; }
        public ClientProcess(ClientInfo clientInfo, IConfig config)
        {
            ClientInfo = clientInfo;
            _Config = config;
        }
        public void Run()
        {
            if (Display is null || !Display.IsConnected || this.IsRunning) return;
            Console.WriteLine("Starting client " + ClientInfo.Name);
            string startMode;
            switch (ClientInfo.StartMode)
            {
                case ClientInfo.Mode.FullScreen:
                    startMode = "--start-fullscreen";
                    break;
                case ClientInfo.Mode.Kiosk:
                    startMode = "--kiosk";
                    break;
                default:
                case ClientInfo.Mode.None:
                    startMode = "";
                    break;
            }

            string arguments = $"--profile-directory=\"{ClientInfo.Name}\" " +
                               $"--user-data-dir=\"{_Config.ProfilesPath}User Data - {ClientInfo.Name}\" " +
                               $"{startMode} \"{_Config.ServerUrl}\"";

            Process.StartInfo = new ProcessStartInfo(_Config.ChromePath, arguments);
            _StartProcessOnPosition(Process, Display.ScreenBounds);
            Console.WriteLine($"Client {ClientInfo.Name} started with arguments:\n{arguments}");
        }
        public void Stop()
        {
            if (!Process.IsRunning()) return;

            try
            {
                Console.WriteLine("Stopping client " + ClientInfo.Name);
                if (!Process.CloseMainWindow())
                {
                    Console.WriteLine("CloseMainWindow failed");
                    Process.Kill(true);
                    Console.WriteLine("Process killed");
                }
                else if (!Process.WaitForExit(_Config.ProcessExitTimeout))
                {
                    Console.WriteLine("WaitForExit failed");
                    Process.Kill(true);
                    Console.WriteLine("Process killed");
                }
                else
                    Console.WriteLine("Process stopped properly");
            }
            catch (Exception e) { Console.WriteLine(e); }
            finally { Process.Close(); }
        }
        public void Restart()
        {
            Stop();
            if (Process.IsRunning() && Process.HasExited)
                Process.Close();
            Run();
        }
        public void IdentifyDisplay(TimeSpan timeout = default)
        {
            if (Display is null || !Display.IsConnected)
                return;
            if (timeout == default)
                timeout = new TimeSpan(0, 0, 0, 0, _Config.IdentifierTimeout);
            Display.Identify(timeout, ClientInfo.Name);
        }
        private void _StartProcessOnPosition(Process process, Rectangle windowPosition)
        {
            process.Start();
            DateTime startTime = DateTime.Now;
            while (process.MainWindowHandle == IntPtr.Zero && (DateTime.Now - startTime).TotalMilliseconds <= _Config.WindowHandleTimeout)
            {
                Thread.Sleep(100);
            }
            SetWindowPos(process.MainWindowHandle, 0, windowPosition.Left, windowPosition.Top, windowPosition.Width, windowPosition.Height, SWP_SHOWWINDOW);
        }
    }
}

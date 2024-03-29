﻿using NorcusLauncher.Displays;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorcusLauncher.Clients
{
    public class ClientProcess
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        const int SWP_SHOWWINDOW = 0x0040;
        const int SWP_NOACTIVATE = 0x0010;
        public ClientInfo ClientInfo { get; set; }
        public Process Process { get; private set; } = new Process();
        public Display? Display { get; set; }
        public bool IsRunning => Process.IsRunning();
        private IConfig _Config { get; set; }
        public ClientProcess(ClientInfo clientInfo, IConfig config)
        {
            ClientInfo = clientInfo;
            _Config = config;
        }
        public void Run()
        {
            if (Display is null || !Display.IsConnected || this.IsRunning) return;
            Process.Close();

            _logger.Debug("Starting client {0}", ClientInfo.Name);
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
            _StartProcessOnPosition(Process, Display.WorkingArea);
            _logger.Debug("Client {0} (PID {1}) started with arguments: {2}", ClientInfo.Name, Process.Id, arguments);
        }
        public void Stop()
        {
            if (!Process.IsRunning()) return;

            try
            {
                _logger.Debug("Stopping client {0} (PID {1})", ClientInfo.Name, Process.Id);
                if (!Process.CloseMainWindow())
                {
                    _logger.Debug("CloseMainWindow failed");
                    Process.Kill(true);
                    _logger.Debug("Process killed");
                }
                else if (!Process.WaitForExit(_Config.ProcessExitTimeout))
                {
                    _logger.Debug("WaitForExit failed");
                    Process.Kill(true);
                    _logger.Debug("Process killed");
                }
                else
                    _logger.Debug("Process stopped properly");
            }
            catch (Exception e) { _logger.Warn(e, "Error occured while stopping the process"); }
            finally { Process.Close(); }
        }
        public void Restart()
        {
            _logger.Debug("Restarting client {0}", ClientInfo.Name);
            Stop();
            if (Process.IsRunning() && Process.HasExited)
                Process.Close();
            Run();
        }
        public void IdentifyDisplay(TimeSpan timeout = default)
        {
            if (Display is null)
                return;
            if (timeout == default)
                timeout = TimeSpan.FromMilliseconds(_Config.IdentifierTimeout);
            Display.Identify(timeout, ClientInfo.Name, GetLocalIPAddress());
        }
        public void UpdateWindowPosition()
        {
            if (Display is null || !IsRunning) return;
            _logger.Debug("Updating window position (client: {0}, new position: {1})", ClientInfo.Name, Display.WorkingArea);
            SetWindowPos(Process.MainWindowHandle, new IntPtr(-1), Display.WorkingArea.Left, Display.WorkingArea.Top, Display.WorkingArea.Width, Display.WorkingArea.Height, SWP_SHOWWINDOW);
        }
        public void SetWindowTopMost()
        {
            if (Display is null || !IsRunning) return;
            Rectangle windowPosition = Display.WorkingArea;
            SetWindowPos(Process.MainWindowHandle, HWND_TOPMOST, windowPosition.Left, windowPosition.Top, windowPosition.Width, windowPosition.Height, SWP_SHOWWINDOW);
        }
        public void SetWindowToBottom()
        {
            if (Display is null || !IsRunning) return;
            Rectangle windowPosition = Display.WorkingArea;
            SetWindowPos(Process.MainWindowHandle, HWND_BOTTOM, windowPosition.Left, windowPosition.Top, windowPosition.Width, windowPosition.Height, SWP_NOACTIVATE);
        }
        private void _StartProcessOnPosition(Process process, Rectangle windowPosition)
        {
            process.Start();
            DateTime startTime = DateTime.Now;
            while (process.MainWindowHandle == IntPtr.Zero && (DateTime.Now - startTime).TotalMilliseconds <= _Config.WindowHandleTimeout)
            {
                Thread.Sleep(100);
            }
            SetWindowPos(process.MainWindowHandle, new IntPtr(-1), windowPosition.Left, windowPosition.Top, windowPosition.Width, windowPosition.Height, SWP_SHOWWINDOW);
            SetForegroundWindow(process.MainWindowHandle);
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ips = host.AddressList
                .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString().StartsWith("192.168."))
                .Select(ip => ip.ToString());
            return String.Join(", ", ips);
        }
        public override string ToString()
        {
            return "Client " + ClientInfo.Name + (IsRunning ? $" (running on {Display})" : " (not running)"); 
        }
    }
}

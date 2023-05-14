using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using NorcusLauncher;
using NorcusLauncher.Clients;
using NorcusLauncher.Displays;

namespace NorcusClientManager
{
    internal class ClientView
    {
        public ClientProcess ClientProcess { get; set; }
        public Launcher Launcher { get; set; }

        public string Name 
        {
            get => ClientProcess.ClientInfo.Name;
            set => ClientProcess.ClientInfo.Name = value;
        }
        public ClientInfo.Mode StartMode
        {
            get => ClientProcess.ClientInfo.StartMode;
            set => ClientProcess.ClientInfo.StartMode = value;
        }
        private Display _display;
        public Display Display
        {
            get => _display;
            set
            {
                _display = value;
                ClientProcess.ClientInfo.DisplayID = value.DisplayID;
            }
        }
        private ICommand _runCommand;
        public ICommand RunCommand => _runCommand ??= new RelayCommand<object>(
            (o) => ClientProcess.Run(),
            (o) => !ClientProcess.IsRunning);
        private ICommand _stopCommand;
        public ICommand StopCommand => _stopCommand ??= new RelayCommand<object>(
            (o) => ClientProcess.Stop(),
            (o) => ClientProcess.IsRunning);
        private ICommand _restartCommand;
        public ICommand RestartCommand => _restartCommand ??= new RelayCommand<object>((o) => ClientProcess.Restart());
        public ClientView(ClientProcess clientProcess, Launcher launcher)
        {
            ClientProcess = clientProcess;
            Launcher = launcher;
            if (ClientProcess.Display is not null)
                Display = ClientProcess.Display;
        }
    }
}

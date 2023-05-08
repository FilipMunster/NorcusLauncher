using NorcusLauncher;
using NorcusLauncher.Clients;
using NorcusLauncher.Displays;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NorcusClientManager
{
    class MainViewModel : INotifyPropertyChanged
    {
        private Config _Config { get; set; }
        private Launcher _Launcher { get; set; }
        
        #region Commands
        private ICommand _loadConfigCommand;
        public ICommand LoadConfigCommand => _loadConfigCommand ??= new RelayCommand<object>((o) => _LoadConfig());
        private ICommand _saveConfigCommand;
        public ICommand SaveConfigCommand => _saveConfigCommand ??= new RelayCommand<object>((o) => _SaveConfig());
        private ICommand _addClientCommand;
        public ICommand AddClientCommand => _addClientCommand ??= new RelayCommand<object>((o) => _AddClient());
        private ICommand _removeClientCommand;
        public ICommand RemoveClientCommand => _removeClientCommand ??= new RelayCommand<object>((o) => _RemoveClient(SelectedClient));
        private ICommand _runClientsCommand;
        public ICommand RunClientsCommand => _runClientsCommand ??= new RelayCommand<object>((o) => _RunClients());
        private ICommand _stopClientsCommand;
        public ICommand StopClientsCommand => _stopClientsCommand ??= new RelayCommand<object>((o) => _StopClients());
        private ICommand _restartClientsCommand;
        public ICommand RestartClientsCommand => _restartClientsCommand ??= new RelayCommand<object>((o) => _RestartClients());
        private ICommand _identifyClientsCommand;
        public ICommand IdentifyClientsCommand => _identifyClientsCommand ??= new RelayCommand<object>((o) => _IdentifyClients());
        #endregion
        #region Properties
        public string ChromePath
        {
            get => _Config.LauncherConfig.ChromePath;
            set => _Config.LauncherConfig.ChromePath = value;
        }
        public string ProfilesPath
        {
            get => _Config.LauncherConfig.ProfilesPath;
            set => _Config.LauncherConfig.ProfilesPath = value;
        }
        public string ServerUrl
        {
            get => _Config.LauncherConfig.ServerUrl;
            set => _Config.LauncherConfig.ServerUrl = value;
        }
        public int WindowHandleTimeout
        {
            get => _Config.LauncherConfig.WindowHandleTimeout;
            set => _Config.LauncherConfig.WindowHandleTimeout = value;
        }
        public int ProcessExitTimeout
        {
            get => _Config.LauncherConfig.ProcessExitTimeout;
            set => _Config.LauncherConfig.ProcessExitTimeout = value;
        }
        public bool RunOnStartup
        {
            get => _Config.RunOnStartup;
            set => _Config.RunOnStartup = value;
        }
        public bool StartInTray
        {
            get => _Config.StartInTray;
            set => _Config.StartInTray = value;
        }
        public bool AutoLaunch
        {
            get => _Config.AutoLaunch;
            set => _Config.AutoLaunch = value;
        }
        public ObservableCollection<Display> DisplayList =>
            _Launcher.DisplayHandler?.Displays?.ToObservableCollection() ?? new ObservableCollection<Display>();
        public ObservableCollection<ClientView> ClientViews { get; } = new();
        public ClientView SelectedClient { get; set; }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? DataGridChanged;

        public MainViewModel()
        {
            _LoadConfig();
        }

        private void _LoadConfig()
        {
            _StopClients();
            _Config = Config.Load();
            _Launcher = new Launcher(_Config.LauncherConfig);
            _Launcher.DisplayHandler.DisplayChanged += DisplayHandler_DisplayChanged;
            _GenerateClientViews();
        }

        private void _SaveConfig()
        {
            _Config.Save();
            _Launcher.RefreshClients();
        }
        private void _AddClient()
        {
            ClientInfo clientInfo = new();
            _Config.LauncherConfig.ClientInfos.Add(clientInfo);
            ClientProcess clientProcess = new ClientProcess(clientInfo, _Config.LauncherConfig);
            _Launcher.Clients.Add(clientProcess);
            ClientViews.Add(new ClientView(clientProcess, _Launcher));
        }
        private void _RemoveClient(ClientView clientView)
        {
            if (clientView is null) return;

            clientView.ClientProcess.Stop();
            clientView.Launcher.Config.ClientInfos.Remove(clientView.ClientProcess.ClientInfo);
            clientView.Launcher.Clients.Remove(clientView.ClientProcess);
            ClientViews.Remove(clientView);
        }
        private void _RunClients()
        {
            _Launcher?.RunClients();
        }
        private void _StopClients()
        {
            _Launcher?.StopClients();
        }
        private void _RestartClients()
        {
            _Launcher?.RestartClients();
        }
        private void _IdentifyClients()
        {
            _Launcher?.IdentifyDisplays();
        }
        private void _GenerateClientViews()
        {
            ClientViews.Clear();
            foreach (var cliProc in _Launcher.Clients)
            {
                ClientViews.Add(new ClientView(cliProc, _Launcher));
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayList)));
        }
        private void DisplayHandler_DisplayChanged(object sender, NorcusLauncher.Displays.DisplayHandler.DisplayChangeEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayList)));
            DataGridChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

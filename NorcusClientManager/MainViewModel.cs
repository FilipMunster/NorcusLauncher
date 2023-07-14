using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
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
using System.Windows;
using System.Windows.Input;

namespace NorcusClientManager
{
    class MainViewModel : INotifyPropertyChanged
    {
        private Config _Config { get; set; }
        private Launcher _Launcher { get; set; }
        private API.Server? _APIServer { get; set; }
        
        #region Commands
        private ICommand _loadConfigCommand;
        public ICommand LoadConfigCommand => _loadConfigCommand ??= new RelayCommand<object>((o) => _LoadConfig());
        private ICommand _saveConfigCommand;
        public ICommand SaveConfigCommand => _saveConfigCommand ??= new RelayCommand<object>((o) => _SaveConfig());
        private ICommand _addClientCommand;
        public ICommand AddClientCommand => _addClientCommand ??= new RelayCommand<object>((o) => _AddClient());
        private ICommand _removeClientCommand;
        public ICommand RemoveClientCommand => _removeClientCommand ??= new RelayCommand<object>(
            (o) => _RemoveClient(SelectedClient),
            (o) => SelectedClient is not null);
        private ICommand _runClientsCommand;
        public ICommand RunClientsCommand => _runClientsCommand ??= new RelayCommand<object>(
            (o) => _RunClients(),
            (o) => ClientViews.Count > 0 && !_Launcher.ClientsAreRunning);
        private ICommand _stopClientsCommand;
        public ICommand StopClientsCommand => _stopClientsCommand ??= new RelayCommand<object>(
            (o) => _StopClients(),
            (o) => ClientViews.Count > 0 && _Launcher.ClientsAreRunning);
        private ICommand _restartClientsCommand;
        public ICommand RestartClientsCommand => _restartClientsCommand ??= new RelayCommand<object>(
            (o) => _RestartClients(),
            (o) => ClientViews.Count > 0);
        private ICommand _identifyClientsCommand;
        public ICommand IdentifyClientsCommand => _identifyClientsCommand ??= new RelayCommand<object>(
            (o) => _IdentifyClients(),
            (o) => true);
        private ICommand _apiServerStartCommand;
        public ICommand APIServerStartCommand => _apiServerStartCommand ??= new RelayCommand<object>(
            (o) => _APIServerStart(),
            (o) => !_APIServerIsRunning);
        private ICommand _apiServerStopCommand;
        public ICommand APIServerStopCommand => _apiServerStopCommand ??= new RelayCommand<object>(
            (o) => _APIServerStop(),
            (o) => _APIServerIsRunning);
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
            get => _Config.LauncherConfig.WindowHandleTimeout / 1000;
            set => _Config.LauncherConfig.WindowHandleTimeout = value * 1000;
        }
        public int ProcessExitTimeout
        {
            get => _Config.LauncherConfig.ProcessExitTimeout / 1000;
            set => _Config.LauncherConfig.ProcessExitTimeout = value * 1000;
        }
        public int IdentificationTimeout
        {
            get => _Config.LauncherConfig.IdentifierTimeout / 1000;
            set => _Config.LauncherConfig.IdentifierTimeout = value * 1000;
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
        public bool AutoIdentify
        {
            get => _Config.AutoIdentify;
            set => _Config.AutoIdentify = value;
        }
        public bool APIAutoStart
        {
            get => _Config.APIAutoStart;
            set => _Config.APIAutoStart = value;
        }
        public int APIPort
        {
            get => _Config.APIPort;
            set => _Config.APIPort = value;
        }
        public string APIKey
        {
            get => _Config.APIKey;
            set => _Config.APIKey = value;
        }
        public ObservableCollection<Display> DisplayList =>
            _Launcher.DisplayHandler.Displays.ToObservableCollection() ?? new ObservableCollection<Display>();
        public ObservableCollection<ClientView> ClientViews { get; } = new();
        private ClientView _selectedClient;
        public ClientView SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged(nameof(ClientViews));
            }
        }
        private bool _APIServerIsRunning { get; set; }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? DataGridChanged;

        public MainViewModel()
        {
            _LoadConfig();
            _StartupActions();
        }
        public void SetClientsTopMost()
        {
            _Launcher.Clients.ForEach(cli => cli.SetWindowTopMost());
        }
        public void SetClientsToBottom()
        {
            _Launcher.Clients.ForEach(cli => cli.SetWindowToBottom());
        }
        private void _LoadConfig()
        {
            _StopClients();
            _Config = Config.Load();
            _Launcher = new Launcher(_Config.LauncherConfig);
            _Launcher.DisplayHandler.DisplayChanged += DisplayHandler_DisplayChanged;
            _GenerateClientViews();
        }
        private void _StartupActions()
        {
            if (AutoIdentify) _Launcher.IdentifyDisplays();
            if (AutoLaunch) _Launcher.RunClients();
            if (APIAutoStart) _APIServerStart();
        }

        private void _SaveConfig()
        {
            _Config?.Save();
            _Launcher?.RefreshClients();
            _GenerateClientViews();
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
            OnPropertyChanged(nameof(ClientViews));
        }
        private void _StopClients()
        {
            _Launcher?.StopClients();
            OnPropertyChanged(nameof(ClientViews));
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
            OnPropertyChanged(nameof(DisplayList));
        }
        private void DisplayHandler_DisplayChanged(object sender, NorcusLauncher.Displays.DisplayHandler.DisplayChangeEventArgs e)
        {
            OnPropertyChanged(nameof(DisplayList));
            DataGridChanged?.Invoke(this, EventArgs.Empty);
        }
        private void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void _APIServerStart()
        {
            _APIServer?.Stop();
            _APIServer = new API.Server(_Launcher, APIPort, APIKey);
            _APIServer.Start();
            _APIServerIsRunning = true;
        }
        private void _APIServerStop()
        {
            _APIServer?.Stop();
            _APIServer = null;
            _APIServerIsRunning = false;
        }
    }
}

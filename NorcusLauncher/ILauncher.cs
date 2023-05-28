using NorcusLauncher.Clients;
using NorcusLauncher.Displays;

namespace NorcusLauncher
{
    public interface ILauncher
    {
        List<ClientProcess> Clients { get; }
        bool ClientsAreRunning { get; }
        IConfig Config { get; }
        DisplayHandler DisplayHandler { get; }

        void IdentifyDisplays();
        void RefreshClients();
        void RestartClients();
        void RunClients();
        void StopClients();
    }
}
using NorcusLauncher.Clients;

namespace NorcusLauncher
{
    public interface IConfig
    {
        List<ClientInfo> ClientInfos { get; set; }
        string ChromePath { get; set; }
        int IdentifierTimeout { get; set; }
        string ProfilesPath { get; set; }
        string ServerUrl { get; set; }
        int ProcessExitTimeout { get; set; }
        int WindowHandleTimeout { get; set; }
    }
}
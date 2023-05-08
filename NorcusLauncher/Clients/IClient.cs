namespace NorcusLauncher.Clients
{
    public interface IClient
    {
        string Name { get; set; }
        ClientInfo.Mode StartMode { get; set; }
        string DisplayDeviceKey { get; set; }
    }
}
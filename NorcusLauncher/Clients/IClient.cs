namespace NorcusLauncher.Clients
{
    public interface IClient
    {
        string Name { get; set; }
        string StartMode { get; set; }
        string DisplayDeviceKey { get; set; }
    }
}
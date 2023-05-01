namespace NorcusLauncher.Clients
{
    public interface IClient
    {
        string AppId { get; set; }
        string DisplayDeviceKey { get; set; }
        string Name { get; set; }
    }
}
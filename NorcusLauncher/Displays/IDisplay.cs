namespace NorcusLauncher.Displays
{
    public interface IDisplay
    {
        string DeviceKey { get; }
        int Id { get; }
        bool IsConnected { get; }
        string Name { get; }
        Rectangle ScreenBounds { get; }
    }
}
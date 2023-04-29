using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorcusLauncher.Displays
{
    public class Display
    {
        public WindowsDisplayAPI.DisplayAdapter Adapter { get; private set; }
        public WindowsDisplayAPI.DisplayScreen Screen { get; set; }
        public int Id { get; private set; }
        public string DeviceKey => Adapter.DeviceKey;
        public string Name => $"{Id} : {Adapter.DeviceName} - " + (Screen?.ScreenName ?? "not connected");
        public Rectangle ScreenBounds => Screen?.Bounds ?? default;
        public bool IsConnected => Screen != null;

        public Display(int id, WindowsDisplayAPI.DisplayAdapter adapter, WindowsDisplayAPI.DisplayScreen screen)
        {
            Id = id;
            Adapter = adapter;
            Screen = screen;
        }
        public void Identify(TimeSpan timeout, string additionalText = "")
        {
            IdentifierForm identifierForm = new IdentifierForm(timeout, this, additionalText);
            Task.Run(() => { Application.Run(identifierForm); });
        }

        public override string ToString() => Name;
    }
}

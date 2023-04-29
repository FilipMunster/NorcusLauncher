using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorcusLauncher.Displays
{
    public class DisplayHandler
    {
        public List<Display> Displays { get; }
        private DisplayChangeWatcher _Watcher { get; set; }
        public DisplayHandler()
        {
            Displays = new List<Display>();
            _InitDisplays();
            Refresh();
            _Watcher = new DisplayChangeWatcher();
            _Watcher.DisplayChanged += _Watcher_DisplayChanged;
            Task.Run(() => { Application.Run(_Watcher); });
        }

        private void _Watcher_DisplayChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        private void _InitDisplays()
        {
            var allAdapters = WindowsDisplayAPI.DisplayAdapter.GetDisplayAdapters(true);
            int i = 1;
            foreach (var adapter in allAdapters)
            {
                Displays.Add(new Display(i++, adapter, null));
            }
        }
        public void Refresh()
        {
            var activeDisplays = WindowsDisplayAPI.Display.GetDisplays();

            List<Display> addedDisplays = new List<Display>();
            List<Display> removedDisplays = new List<Display>();

            foreach (var display in Displays)
            {
                var screen = activeDisplays
                    .FirstOrDefault(x => x.Adapter.DeviceKey == display.DeviceKey)?
                    .DisplayScreen;

                if (display.IsConnected && screen is null)
                    removedDisplays.Add(display);
                else if (!display.IsConnected && screen != null)
                    addedDisplays.Add(display);

                display.Screen = screen;
            }

            if (addedDisplays.Count > 0 || removedDisplays.Count > 0)
                DisplayChanged?.Invoke(this, new DisplayChangeEventArgs(addedDisplays, removedDisplays));
        }

        public delegate void DisplayChangeEventHandler(object sender, DisplayChangeEventArgs e);
        public class DisplayChangeEventArgs
        {
            public IEnumerable<Display> AddedDisplays { get; }
            public IEnumerable<Display> RemovedDisplays { get; }
            public DisplayChangeEventArgs(IEnumerable<Display> addedDisplays, IEnumerable<Display> removedDisplays)
            {
                AddedDisplays = addedDisplays;
                RemovedDisplays = removedDisplays;
            }
        }
        public event DisplayChangeEventHandler DisplayChanged;
    }
}

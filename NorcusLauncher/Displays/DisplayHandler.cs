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
        public List<Display> Displays { get; } = new();
        private DisplayChangeWatcher _Watcher { get; } = new();
        public DisplayHandler()
        {
            Refresh();
            _Watcher.DisplayChanged += (_, _) => Refresh();
            Task.Run(() => { Application.Run(_Watcher); });
        }
        public void Refresh()
        {
            var activeDisplays = WindowsDisplayAPI.Display.GetDisplays();
            
            List<Display> addedDisplays = new();
            List<Display> removedDisplays = new();
            List<Display> updatedDisplays = new();

            List<Display> unHandledDisplays = new(Displays);

            foreach (var activeDisp in activeDisplays)
            {
                string displayId = Display.GetDisplayId(activeDisp);
                Display? currentDisplay = Displays.FirstOrDefault(d => d.DisplayID == displayId);

                if (currentDisplay is null)
                {
                    Display addedDisplay = new Display(activeDisp);
                    Displays.Add(addedDisplay);
                    addedDisplays.Add(addedDisplay);
                }
                else
                {
                    if (!currentDisplay.IsConnected)
                        addedDisplays.Add(currentDisplay);
                    else if (currentDisplay.WorkingArea != activeDisp.DisplayScreen.WorkingArea)
                        updatedDisplays.Add(currentDisplay);

                    currentDisplay.DisplayConnected(activeDisp);
                    unHandledDisplays.Remove(currentDisplay);
                }
            }
            unHandledDisplays.ForEach(d => { d.DisplayDisconnected(); removedDisplays.Add(d); });

            for (int i = 0; i < Displays.Count; i++)
            {
                Displays[i].Index = i + 1;
            }

            if (addedDisplays.Count > 0 || removedDisplays.Count > 0 || updatedDisplays.Count > 0)
                DisplayChanged?.Invoke(this, new DisplayChangeEventArgs(addedDisplays, removedDisplays, updatedDisplays));
        }      

        public delegate void DisplayChangeEventHandler(object sender, DisplayChangeEventArgs e);
        public class DisplayChangeEventArgs
        {
            public IEnumerable<Display> AddedDisplays { get; }
            public IEnumerable<Display> RemovedDisplays { get; }
            public IEnumerable<Display> UpdatedDisplays { get; }
            public DisplayChangeEventArgs(IEnumerable<Display> addedDisplays, IEnumerable<Display> removedDisplays, IEnumerable<Display> updatedDisplays)
            {
                AddedDisplays = addedDisplays;
                RemovedDisplays = removedDisplays;
                UpdatedDisplays = updatedDisplays;
            }
        }
        public event DisplayChangeEventHandler? DisplayChanged;
    }
}

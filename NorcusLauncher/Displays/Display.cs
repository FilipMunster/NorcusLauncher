using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using WindowsDisplayAPI;

namespace NorcusLauncher.Displays
{
    public class Display
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Pořadí displeje, hodnotu potřeba zadat ručně. Pouze pro snažší identifikaci v seznamu. Nezávisí na něm logika.
        /// </summary>
        public int Index { get; set; } = 0;
        public string DisplayID { get; }
        public string Name => $"{Index}: {_DisplayNameBase}" + (IsConnected ? "" : " (not connected)");
        public Rectangle WorkingArea { get; private set; }
        public bool IsConnected { get; private set; }
        private string _DisplayNameBase { get; set; } = "";
        /// <summary>
        /// Vytvoří prázdný displej, ke kterému znám jen DisplayID, ale zatím není připojený.
        /// </summary>
        /// <param name="displayID"></param>
        public Display(string displayID)
        {
            DisplayID = displayID;
            WorkingArea = Rectangle.Empty;
            IsConnected = false;
            _DisplayNameBase = displayID;
        }
        public Display(WindowsDisplayAPI.Display display)
        {
            (string vendor, DisplayID) = ParseDevicePath(display.DevicePath);
            _DisplayNameBase = _GetDisplayNameBase(vendor, display);
            WorkingArea = display.DisplayScreen.WorkingArea;
            IsConnected = true;
        }
        public void DisplayConnected(WindowsDisplayAPI.Display display)
        {
            _logger.Debug("Adding display {0} to client {1}", display, this);
            (string vendor, string displayID) = ParseDevicePath(display.DevicePath);
            if (displayID != DisplayID)
                throw new ArgumentException($"DisplayID se neshodují (původní: {DisplayID}, nové: {displayID})");
            _DisplayNameBase = _GetDisplayNameBase(vendor, display);
            WorkingArea = display.DisplayScreen.WorkingArea;
            IsConnected = true;
        }
        public void DisplayDisconnected()
        {
            _logger.Debug("Display disconnected (client {0})", this);
            WorkingArea = Rectangle.Empty;
            IsConnected = false;
        }
        public void Identify(TimeSpan timeout, string additionalText = "", string footer = "")
        {
            if (!IsConnected) return;
            IdentifierForm identifierForm = new IdentifierForm(timeout, this, additionalText, footer);
            Task.Run(() => { Application.Run(identifierForm); });
        }
        public override string ToString() => Name;

        private string _GetDisplayNameBase(string vendor, WindowsDisplayAPI.Display display)
        {
            char backslash = '\\';
            string[] dispNameArray = display.DisplayName
                .Split(backslash, StringSplitOptions.RemoveEmptyEntries)
                .Except(new[] { "." })
                .ToArray();
            return $"{vendor} - {dispNameArray[0]}\\{dispNameArray.Last()}";
        }
        /// <summary>
        /// Z property DevicePath získá výrobce a instanceId
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static (string vendor, string displayId) ParseDevicePath(string path)
        {
            string[] splitted = path.Split('#');
            if (splitted.Length != 4)
                throw new ArgumentException("Device path monitoru je v neočekávaném tvaru:\n" + path);
            return (splitted[1], splitted[2]);
        }
        public static string GetDisplayId(WindowsDisplayAPI.Display display)
        {
            (_, string displayId) = ParseDevicePath(display.DevicePath);
            return displayId;
        }
    }
}

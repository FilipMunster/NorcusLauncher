using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NorcusLauncher.Displays
{
    internal partial class DisplayChangeWatcher : Form
    {
        const uint WM_DISPLAYCHANGE = 0x007e;
        public DisplayChangeWatcher()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_DISPLAYCHANGE)
            {
                DisplayChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler DisplayChanged;
    }
}

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
    public partial class IdentifierForm : Form
    {
        private Display _Display { get; set; }
        private TimeSpan _TimeOut { get; set; }
        public IdentifierForm(TimeSpan timeOut, Display display, string additionalText = "")
        {
            InitializeComponent();
            ShowInTaskbar = false;

            DispId.Text = display.Id.ToString();
            DispName.Text = display.Name.ToString();
            AdditionalText.Text = additionalText;

            DispId.Left = (this.ClientSize.Width - DispId.Width) / 2;
            DispName.Left = (this.ClientSize.Width - DispName.Width) / 2;
            AdditionalText.Left = (this.ClientSize.Width - AdditionalText.Width) / 2;

            if (!display.IsConnected)
                return;
            _Display = display;
            _TimeOut = timeOut;
        }
        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (!_Display.IsConnected)
                return;

            int xCenter = (_Display.ScreenBounds.Right - _Display.ScreenBounds.Left) / 2 + _Display.ScreenBounds.Left;
            int yCenter = (_Display.ScreenBounds.Bottom - _Display.ScreenBounds.Top) / 2 + _Display.ScreenBounds.Top;
            int x = xCenter - (Width / 2);
            int y = yCenter - (Height / 2);
            SetDesktopLocation(x, y);
            await Task.Delay(_TimeOut);
            Close();
        }

        private void IdentifierForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void DispName_Click(object sender, EventArgs e)
        {

        }

        private void AdditionalText_Click(object sender, EventArgs e)
        {
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace NorcusLauncher.Displays
{
    public partial class IdentifierForm : Form
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private Display _Display { get; set; }
        private TimeSpan _TimeOut { get; set; }
        public IdentifierForm(TimeSpan timeOut, Display display, string additionalText = "", string footer = "")
        {
            InitializeComponent();
            HandleCreated += IdentifierForm_HandleCreated;
            ShowInTaskbar = false;

            DispId.Text = display.Index.ToString();
            DispName.Text = display.Name.ToString();
            AdditionalText.Text = additionalText;
            Footer.Text = footer;

            DispId.Left = (this.ClientSize.Width - DispId.Width) / 2;
            DispName.Left = (this.ClientSize.Width - DispName.Width) / 2;
            AdditionalText.Left = (this.ClientSize.Width - AdditionalText.Width) / 2;

            _Display = display;
            _TimeOut = timeOut;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            int xCenter = (_Display.WorkingArea.Right - _Display.WorkingArea.Left) / 2 + _Display.WorkingArea.Left;
            int yCenter = (_Display.WorkingArea.Bottom - _Display.WorkingArea.Top) / 2 + _Display.WorkingArea.Top;
            int x = xCenter - (Width / 2);
            int y = yCenter - (Height / 2);
            SetDesktopLocation(x, y);
            Timer timer = new Timer() { Interval = (int)_TimeOut.TotalMilliseconds };
            timer.Tick += (_, _) => Close();
            timer.Start();
        }
        private void IdentifierForm_HandleCreated(object? sender, EventArgs e)
        {
            SetForegroundWindow(this.Handle);
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

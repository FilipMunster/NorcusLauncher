﻿namespace NorcusLauncher.Displays
{
    partial class IdentifierForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DispId = new Label();
            DispName = new Label();
            AdditionalText = new Label();
            Footer = new Label();
            SuspendLayout();
            // 
            // DispId
            // 
            DispId.AutoSize = true;
            DispId.Font = new Font("Segoe UI", 72F, FontStyle.Bold, GraphicsUnit.Point);
            DispId.ImageAlign = ContentAlignment.TopLeft;
            DispId.Location = new Point(166, 52);
            DispId.Name = "DispId";
            DispId.Size = new Size(428, 159);
            DispId.TabIndex = 0;
            DispId.Text = "DispId";
            DispId.TextAlign = ContentAlignment.TopCenter;
            DispId.Click += label1_Click;
            // 
            // DispName
            // 
            DispName.AutoSize = true;
            DispName.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            DispName.ImageAlign = ContentAlignment.TopLeft;
            DispName.Location = new Point(302, 255);
            DispName.Name = "DispName";
            DispName.Size = new Size(156, 41);
            DispName.TabIndex = 1;
            DispName.Text = "DispName";
            // 
            // AdditionalText
            // 
            AdditionalText.AutoSize = true;
            AdditionalText.Font = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Point);
            AdditionalText.ImageAlign = ContentAlignment.TopLeft;
            AdditionalText.Location = new Point(334, 369);
            AdditionalText.Name = "AdditionalText";
            AdditionalText.Size = new Size(93, 54);
            AdditionalText.TabIndex = 2;
            AdditionalText.Text = "Text";
            // 
            // Footer
            // 
            Footer.AutoSize = true;
            Footer.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            Footer.Location = new Point(12, 463);
            Footer.Name = "Footer";
            Footer.Size = new Size(70, 28);
            Footer.TabIndex = 3;
            Footer.Text = "Footer";
            // 
            // IdentifierForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(760, 500);
            Controls.Add(Footer);
            Controls.Add(AdditionalText);
            Controls.Add(DispName);
            Controls.Add(DispId);
            FormBorderStyle = FormBorderStyle.None;
            Name = "IdentifierForm";
            StartPosition = FormStartPosition.Manual;
            Text = "IdentifierForm";
            TopMost = true;
            Load += IdentifierForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label DispId;
        private Label DispName;
        private Label AdditionalText;
        private Label Footer;
    }
}
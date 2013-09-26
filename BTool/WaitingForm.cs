﻿namespace BTool
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class WaitingForm : Form
    {
        private IContainer components;
        public ProgressBar pbProgressBar;

        public WaitingForm()
        {
            this.InitializeComponent();
            this.pbProgressBar.Visible = true;
            this.pbProgressBar.Step = 1;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pbProgressBar = new ProgressBar();
            base.SuspendLayout();
            this.pbProgressBar.Cursor = Cursors.Default;
            this.pbProgressBar.Location = new Point(12, 12);
            this.pbProgressBar.Name = "pbProgressBar";
            this.pbProgressBar.Size = new Size(0xed, 0x17);
            this.pbProgressBar.Step = 1;
            this.pbProgressBar.Style = ProgressBarStyle.Marquee;
            this.pbProgressBar.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0x106, 0x35);
            base.Controls.Add(this.pbProgressBar);
            base.Name = "WaitingForm";
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Please wait...";
            base.ResumeLayout(false);
        }
    }
}


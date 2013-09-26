﻿namespace BTool
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ListSelectForm : Form
    {
        private Button btnCancel;
        private Button btnOk;
        private IContainer components;
        private ListBox lbDataItems;

        public ListSelectForm()
        {
            this.InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public string GetUserSelection()
        {
            int selectedIndex = this.lbDataItems.SelectedIndex;
            return this.lbDataItems.Items[selectedIndex].ToString();
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ListSelectForm));
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.lbDataItems = new ListBox();
            base.SuspendLayout();
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new Point(0x18, 0x87);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(0x54, 0x1d);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.buttonOk_Click);
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x84, 0x87);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x54, 0x1d);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.buttonCancel_Click);
            this.lbDataItems.FormattingEnabled = true;
            this.lbDataItems.Items.AddRange(new object[] { "88:88:88:88:88:88:88:88", "88:88:88:88:88:88:88:88", "88:88:88:88:88:88:88:88" });
            this.lbDataItems.Location = new Point(0x2e, 0x1b);
            this.lbDataItems.Name = "lbDataItems";
            this.lbDataItems.ScrollAlwaysVisible = true;
            this.lbDataItems.Size = new Size(150, 0x52);
            this.lbDataItems.TabIndex = 3;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new Size(0xe3, 0xb6);
            base.Controls.Add(this.lbDataItems);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOk);
            base.Icon = (Icon) resources.GetObject("$this.Icon");
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0xf3, 220);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0xf3, 220);
            base.Name = "ListSelectForm";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Select Connection";
            base.FormClosing += new FormClosingEventHandler(this.sysFormClosing);
            base.ResumeLayout(false);
        }

        public bool LoadFormData(List<string> dataItems)
        {
            bool flag = true;
            if (dataItems != null)
            {
                this.lbDataItems.BeginUpdate();
                this.lbDataItems.Items.Clear();
                foreach (string str in dataItems)
                {
                    this.lbDataItems.Items.Add(str);
                }
                if (this.lbDataItems.Items.Count > 0)
                {
                    this.lbDataItems.SetSelected(0, true);
                }
                this.lbDataItems.EndUpdate();
                return flag;
            }
            return false;
        }

        private void sysFormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}

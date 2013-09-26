﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

public class DisplayUtils
{
    private const string moduleName = "DisplayUtils";
    private SharedObjects sharedObjs = new SharedObjects();
    private const uint SW_RESTORE = 9;

    public bool BringWindowToFront(int hWnd)
    {
        bool flag = false;
        if (!this.sharedObjs.IsMonoRunning())
        {
            ShowWindow((IntPtr) hWnd, 9);
            flag = SetForegroundWindow(hWnd);
        }
        return flag;
    }

    public bool BringWindowToFront(Form form)
    {
        form.Show();
        form.BringToFront();
        return true;
    }

    public void CenterForm(object form)
    {
        Form form2 = form as Form;
        Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
        form2.Top = workingArea.Height / 2;
        form2.Left = workingArea.Width / 2;
    }

    public void CenterFormOnForm(object form2CenterOn, object form2Center)
    {
        Form form = form2CenterOn as Form;
        Form form2 = form2Center as Form;
        form2.Top = (form.Top + (form.Height / 2)) - (form2.Height / 2);
        if (form2.Top < 0)
        {
            form2.Top = 0;
        }
        form2.Left = (form.Left + (form.Width / 2)) - (form2.Width / 2);
        if (form2.Left < 0)
        {
            form2.Left = 0;
        }
        form2.Location = new Point(form2.Left, form2.Top);
    }

    public void CheckHexKeyPress(object sender, KeyPressEventArgs e)
    {
        if (!Regex.IsMatch(e.KeyChar.ToString(), @"\b[0-9a-fA-F]+\b") && (e.KeyChar != '\b'))
        {
            e.Handled = true;
        }
    }

    public void CheckNumericKeyPress(object sender, KeyPressEventArgs e)
    {
        if (!Regex.IsMatch(e.KeyChar.ToString(), @"\b[0-9]+\b") && (e.KeyChar != '\b'))
        {
            e.Handled = true;
        }
    }

    public void ComboBoxHideCursor(object sender, EventArgs e)
    {
        try
        {
            if (!this.sharedObjs.IsMonoRunning())
            {
                ComboBox box = (ComboBox) sender;
                HideCaret(box.Handle);
            }
        }
        catch
        {
        }
    }

    [DllImport("User32.dll")]
    public static extern int FindWindow(string lpClassName, string lpWindowName);
    public int GetWindowId(string className, string windowName)
    {
        if (!this.sharedObjs.IsMonoRunning())
        {
            return FindWindow(className, windowName);
        }
        return 0;
    }

    [DllImport("User32.dll")]
    private static extern bool HideCaret(IntPtr hWnd);
    public void OpenFormWindow(Form form)
    {
        if ((form.WindowState == FormWindowState.Minimized) && !this.sharedObjs.IsMonoRunning())
        {
            ShowWindow(form.Handle, 9);
        }
        form.Show();
        form.BringToFront();
    }

    public void PreventAnyKeyPress(object sender, KeyEventArgs e)
    {
        e.Handled = true;
    }

    public void PreventAnyKeyPress(object sender, KeyPressEventArgs e)
    {
        e.Handled = true;
    }

    public bool RemovePathFromFilename(string fullPathFileName, ref string fileName, string defaultName)
    {
        bool flag = true;
        try
        {
            fileName = defaultName;
            if (fullPathFileName.Length <= 0)
            {
                return flag;
            }
            string str = fullPathFileName;
            int num = 0;
            while ((num = str.IndexOf(@"\")) != -1)
            {
                str = str.Remove(0, num + 1);
            }
            if (str == string.Empty)
            {
                fileName = defaultName;
                return flag;
            }
            fileName = str;
        }
        catch
        {
            flag = false;
        }
        return flag;
    }

    public void SetCbColor(object cbox, Color fore, Color back)
    {
        ComboBox box = cbox as ComboBox;
        box.ForeColor = fore;
        box.BackColor = back;
    }

    [DllImport("User32.dll")]
    public static extern bool SetForegroundWindow(int hWnd);
    public void SetTbColor(object tbox, Color fore, Color back)
    {
        TextBox box = tbox as TextBox;
        box.ForeColor = fore;
        box.BackColor = back;
    }

    [DllImport("User32.dll")]
    private static extern int ShowWindow(IntPtr hWnd, uint Msg);
    public void TextBoxHideCursor(object sender, EventArgs e)
    {
        try
        {
            if (!this.sharedObjs.IsMonoRunning())
            {
                TextBox box = (TextBox) sender;
                HideCaret(box.Handle);
            }
        }
        catch
        {
        }
    }
}

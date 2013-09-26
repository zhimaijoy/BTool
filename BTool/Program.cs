﻿namespace BTool
{
    using System;
    using System.Windows.Forms;

    internal static class Program
    {
        private static CmdLineArgs cmdLineArgs = new CmdLineArgs();

        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            cmdLineArgs.SetCmdLineArgs(args);
            Application.Run(new FormMain(cmdLineArgs));
        }
    }
}

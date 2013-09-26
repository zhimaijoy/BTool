﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

public class Logging
{
    private DataUtils dataUtils = new DataUtils();
    private static long logFileMaxLength = 0x1388000L;
    private static string logFileNameAndPath = "";
    private static long logFilePosition = 0L;
    private static FileStream logFileStream = null;
    private static Mutex logMutex = new Mutex();
    private const string moduleName = "Logging";
    private MsgBox msgBox = new MsgBox();
    private static MsgType msgLevel = MsgType.Info;
    private static FileStream posFileStream = null;
    private static int posLength = 0;
    private const string startMarker = "\r\n****************************************************\r\n* Log Start Marker                                 *\r\n****************************************************\r\n\r\n";
    private static bool useConsoleLogging = false;
    private static bool useFileLogging = false;
    private static bool useHighResTime = false;
    private static bool useMsgBox = false;

    public Logging()
    {
        posLength = Marshal.SizeOf(logFilePosition);
    }

    public bool CheckLogging(MsgType msgType)
    {
        bool flag = true;
        if (!useFileLogging && !useConsoleLogging)
        {
            return false;
        }
        if (msgType < msgLevel)
        {
            flag = false;
        }
        return flag;
    }

    public bool Close()
    {
        logMutex.WaitOne();
        useFileLogging = false;
        if (logFileStream != null)
        {
            logFileStream.Flush();
            logFileStream.Close();
        }
        logFileStream = null;
        logFilePosition = 0L;
        if (posFileStream != null)
        {
            posFileStream.Flush();
            posFileStream.Close();
        }
        posFileStream = null;
        logMutex.ReleaseMutex();
        return true;
    }

    private bool EraseFileLog()
    {
        bool flag = true;
        try
        {
            long num = logFileStream.Length - logFilePosition;
            if (num <= 0L)
            {
                return flag;
            }
            byte[] buffer = new byte[num];
            for (int i = 0; i < num; i++)
            {
                buffer[i] = 0x20;
            }
            logFileStream.Write(buffer, 0, buffer.Length);
        }
        catch (Exception exception)
        {
            if (useMsgBox)
            {
                string msg = "Cannot Erase Trailing Log File Data\n" + exception.Message + "\nLogging\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            }
            flag = false;
            this.Close();
        }
        return flag;
    }

    public bool GetConsoleLogging()
    {
        bool useConsoleLogging = true;
        logMutex.WaitOne();
        useConsoleLogging = Logging.useConsoleLogging;
        logMutex.ReleaseMutex();
        return useConsoleLogging;
    }

    public bool GetFileLogging()
    {
        bool useFileLogging = true;
        logMutex.WaitOne();
        useFileLogging = Logging.useFileLogging;
        logMutex.ReleaseMutex();
        return useFileLogging;
    }

    public bool GetHighResTime()
    {
        bool useHighResTime = true;
        logMutex.WaitOne();
        useHighResTime = Logging.useHighResTime;
        logMutex.ReleaseMutex();
        return useHighResTime;
    }

    public MsgType GetMsgLevel()
    {
        MsgType none = MsgType.None;
        logMutex.WaitOne();
        none = msgLevel;
        logMutex.ReleaseMutex();
        return none;
    }

    public string GetMsgTypeStr(MsgType msgType)
    {
        switch (msgType)
        {
            case MsgType.Debug:
                return "Debug  ";

            case MsgType.Info:
                return "Info   ";

            case MsgType.Warning:
                return "Warning";

            case MsgType.Error:
                return "Error  ";

            case MsgType.Fatal:
                return "Fatal  ";

            case MsgType.None:
                return "       ";
        }
        return "Unknown";
    }

    public bool GetUseMsgBox()
    {
        bool useMsgBox = true;
        logMutex.WaitOne();
        useMsgBox = Logging.useMsgBox;
        logMutex.ReleaseMutex();
        return useMsgBox;
    }

    private bool Open()
    {
        bool flag = true;
        if (logFileNameAndPath == "")
        {
            return false;
        }
        if (logFileStream != null)
        {
            return true;
        }
        bool flag2 = File.Exists(logFileNameAndPath);
        string path = logFileNameAndPath + ".pos";
        bool flag3 = File.Exists(path);
        try
        {
            if (flag2 && !flag3)
            {
                logFileStream = new FileStream(logFileNameAndPath, FileMode.Create);
            }
            else
            {
                logFileStream = new FileStream(logFileNameAndPath, FileMode.OpenOrCreate);
            }
        }
        catch (Exception exception)
        {
            if (useMsgBox)
            {
                string msg = "Cannot Open Log Message File\n" + exception.Message + "\nLogging\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            }
            flag = false;
            this.Close();
            return flag;
        }
        try
        {
            if (!flag2 && flag3)
            {
                posFileStream = new FileStream(path, FileMode.Create);
            }
            else
            {
                posFileStream = new FileStream(path, FileMode.OpenOrCreate);
            }
        }
        catch (Exception exception2)
        {
            if (useMsgBox)
            {
                string str3 = "Cannot Open Log Position File\n" + exception2.Message + "\nLogging\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str3);
            }
            flag = false;
            this.Close();
            return flag;
        }
        if (posFileStream.Length == posLength)
        {
            try
            {
                posFileStream.Seek(0L, SeekOrigin.Begin);
                byte[] buffer = new byte[posLength];
                if (posFileStream.Read(buffer, 0, posLength) != posLength)
                {
                    logFilePosition = 0L;
                }
                int index = 0;
                bool dataErr = false;
                logFilePosition = (long) this.dataUtils.Unload64Bits(buffer, ref index, ref dataErr, false);
                goto Label_01D0;
            }
            catch (Exception exception3)
            {
                if (useMsgBox)
                {
                    string str4 = "Cannot Read Log Position File\n" + exception3.Message + "\nLogging\n";
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, str4);
                }
                logFilePosition = 0L;
                goto Label_01D0;
            }
        }
        logFilePosition = 0L;
    Label_01D0:
        try
        {
            logFileStream.Seek(logFilePosition, SeekOrigin.Begin);
        }
        catch (Exception exception4)
        {
            if (useMsgBox)
            {
                string str5 = "Cannot Seek To Log Position File\n" + exception4.Message + "\nLogging\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str5);
            }
            flag = false;
            this.Close();
            return flag;
        }
        bool flag1 = flag = this.UpdateFileLog("\r\n****************************************************\r\n* Log Start Marker                                 *\r\n****************************************************\r\n\r\n");
        return flag;
    }

    public bool SetConsoleLogging(bool useConsole)
    {
        logMutex.WaitOne();
        useConsoleLogging = useConsole;
        logMutex.ReleaseMutex();
        return true;
    }

    public void SetFileLogging(bool fileLogging)
    {
        logMutex.WaitOne();
        useFileLogging = fileLogging;
        logMutex.ReleaseMutex();
    }

    public bool SetHighResTime(bool useHighRes)
    {
        logMutex.WaitOne();
        useHighResTime = useHighRes;
        logMutex.ReleaseMutex();
        return true;
    }

    public bool SetLogFileMaxLength(long maxLength)
    {
        logMutex.WaitOne();
        logFileMaxLength = maxLength;
        logMutex.ReleaseMutex();
        return true;
    }

    public bool SetMsgLevel(MsgType newMsgLevel)
    {
        logMutex.WaitOne();
        msgLevel = newMsgLevel;
        logMutex.ReleaseMutex();
        return true;
    }

    public bool SetPathAndLogFileName(string location)
    {
        bool flag = true;
        logMutex.WaitOne();
        logFileNameAndPath = location;
        this.Close();
        flag = this.Open();
        if (!flag)
        {
            this.Close();
        }
        logMutex.ReleaseMutex();
        return flag;
    }

    public bool SetUseMsgBox(bool useMBox)
    {
        logMutex.WaitOne();
        useMsgBox = useMBox;
        logMutex.ReleaseMutex();
        return true;
    }

    private bool UpdateFileLog(string newMessage)
    {
        bool flag = true;
        byte[] bytesFromAsciiString = null;
        try
        {
            bytesFromAsciiString = new byte[newMessage.Length];
            bytesFromAsciiString = this.dataUtils.GetBytesFromAsciiString(newMessage);
            if ((bytesFromAsciiString.Length + logFilePosition) > logFileMaxLength)
            {
                this.EraseFileLog();
                logFilePosition = 0L;
                logFileStream.Seek(logFilePosition, SeekOrigin.Begin);
            }
            logFileStream.Write(bytesFromAsciiString, 0, bytesFromAsciiString.Length);
            logFileStream.Flush(true);
        }
        catch (Exception exception)
        {
            if (useMsgBox)
            {
                string msg = "Cannot Write To Log File\n" + exception.Message + "\nLogging\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            }
            flag = false;
            this.Close();
            return flag;
        }
        this.UpdatePositionLog((long) bytesFromAsciiString.Length);
        return flag;
    }

    private bool UpdatePositionLog(long dataLength)
    {
        bool flag = true;
        logFilePosition += dataLength;
        try
        {
            byte[] data = new byte[posLength];
            int index = 0;
            bool dataErr = false;
            this.dataUtils.Load64Bits(ref data, ref index, (ulong) logFilePosition, ref dataErr, false);
            posFileStream.Seek(0L, SeekOrigin.Begin);
            posFileStream.Write(data, 0, data.Length);
            posFileStream.Flush(true);
        }
        catch (Exception exception)
        {
            if (useMsgBox)
            {
                string msg = "Cannot Write To Log Position File\n" + exception.Message + "\nLogging\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            }
            flag = false;
            this.Close();
        }
        return flag;
    }

    public bool Write(MsgType msgType, string extraInfo, string message)
    {
        bool flag = true;
        if (this.CheckLogging(msgType))
        {
            logMutex.WaitOne();
            try
            {
                string str = "";
                if (!useHighResTime)
                {
                    str = "[" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff") + "] ";
                }
                else
                {
                    DateTime time = new DateTime(0x7d1, 1, 1);
                    DateTime now = DateTime.Now;
                    str = str + "[" + now.ToString("MM/dd/yyyy hh:mm:ss.");
                    long ticks = now.Ticks - time.Ticks;
                    TimeSpan span = new TimeSpan(ticks);
                    long num2 = ticks / 10L;
                    long num3 = ((long) span.TotalSeconds) * 0xf4240L;
                    str = str + ((num2 - num3)).ToString("D6") + "] ";
                }
                string str2 = "<" + this.GetMsgTypeStr(msgType) + "> ";
                string str3 = "";
                if (message != null)
                {
                    str3 = message;
                }
                string str4 = "";
                if ((extraInfo != null) && (extraInfo.Length > 0))
                {
                    str4 = " {" + extraInfo + "}";
                }
                string str5 = "";
                str5 = str + str2 + str3 + str4 + "\r\n";
                if (useConsoleLogging)
                {
                    Console.Write(str5);
                }
                if (useFileLogging)
                {
                    flag = this.UpdateFileLog(str5);
                }
            }
            catch (Exception exception)
            {
                if (useMsgBox)
                {
                    string msg = "Cannot Write Log Data\n" + exception.Message + "\nLogging\n";
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                }
            }
            logMutex.ReleaseMutex();
        }
        return flag;
    }

    public bool Write(MsgType msgType, string extraInfo, string format, params object[] arg)
    {
        bool flag = true;
        if (this.CheckLogging(msgType))
        {
            logMutex.WaitOne();
            string message = string.Empty;
            try
            {
                message = string.Format(format, arg);
            }
            catch (Exception exception)
            {
                message = "Attempt To Write Log Failed\nFormat String Issue With...\n" + format + "\n" + exception.Message + "\n";
            }
            flag = this.Write(msgType, extraInfo, message);
            logMutex.ReleaseMutex();
        }
        return flag;
    }

    public enum MsgType
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal,
        None
    }
}

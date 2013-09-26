﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public class DataUtils
{
    public const int Base10 = 10;
    public const int Base16 = 0x10;
    private bool handleException = true;
    private const string moduleName = "DataUtils";
    private MsgBox msgBox = new MsgBox();
    private byte[] revByteLookup = new byte[] { 
        0, 0x80, 0x40, 0xc0, 0x20, 160, 0x60, 0xe0, 0x10, 0x90, 80, 0xd0, 0x30, 0xb0, 0x70, 240, 
        8, 0x88, 0x48, 200, 40, 0xa8, 0x68, 0xe8, 0x18, 0x98, 0x58, 0xd8, 0x38, 0xb8, 120, 0xf8, 
        4, 0x84, 0x44, 0xc4, 0x24, 0xa4, 100, 0xe4, 20, 0x94, 0x54, 0xd4, 0x34, 180, 0x74, 0xf4, 
        12, 140, 0x4c, 0xcc, 0x2c, 0xac, 0x6c, 0xec, 0x1c, 0x9c, 0x5c, 220, 60, 0xbc, 0x7c, 0xfc, 
        2, 130, 0x42, 0xc2, 0x22, 0xa2, 0x62, 0xe2, 0x12, 0x92, 0x52, 210, 50, 0xb2, 0x72, 0xf2, 
        10, 0x8a, 0x4a, 0xca, 0x2a, 170, 0x6a, 0xea, 0x1a, 0x9a, 90, 0xda, 0x3a, 0xba, 0x7a, 250, 
        6, 0x86, 70, 0xc6, 0x26, 0xa6, 0x66, 230, 0x16, 150, 0x56, 0xd6, 0x36, 0xb6, 0x76, 0xf6, 
        14, 0x8e, 0x4e, 0xce, 0x2e, 0xae, 110, 0xee, 30, 0x9e, 0x5e, 0xde, 0x3e, 190, 0x7e, 0xfe, 
        1, 0x81, 0x41, 0xc1, 0x21, 0xa1, 0x61, 0xe1, 0x11, 0x91, 0x51, 0xd1, 0x31, 0xb1, 0x71, 0xf1, 
        9, 0x89, 0x49, 0xc9, 0x29, 0xa9, 0x69, 0xe9, 0x19, 0x99, 0x59, 0xd9, 0x39, 0xb9, 0x79, 0xf9, 
        5, 0x85, 0x45, 0xc5, 0x25, 0xa5, 0x65, 0xe5, 0x15, 0x95, 0x55, 0xd5, 0x35, 0xb5, 0x75, 0xf5, 
        13, 0x8d, 0x4d, 0xcd, 0x2d, 0xad, 0x6d, 0xed, 0x1d, 0x9d, 0x5d, 0xdd, 0x3d, 0xbd, 0x7d, 0xfd, 
        3, 0x83, 0x43, 0xc3, 0x23, 0xa3, 0x63, 0xe3, 0x13, 0x93, 0x53, 0xd3, 0x33, 0xb3, 0x73, 0xf3, 
        11, 0x8b, 0x4b, 0xcb, 0x2b, 0xab, 0x6b, 0xeb, 0x1b, 0x9b, 0x5b, 0xdb, 0x3b, 0xbb, 0x7b, 0xfb, 
        7, 0x87, 0x47, 0xc7, 0x27, 0xa7, 0x67, 0xe7, 0x17, 0x97, 0x57, 0xd7, 0x37, 0xb7, 0x77, 0xf7, 
        15, 0x8f, 0x4f, 0xcf, 0x2f, 0xaf, 0x6f, 0xef, 0x1f, 0x9f, 0x5f, 0xdf, 0x3f, 0xbf, 0x7f, 0xff
     };
    public const int Size16Bits = 2;
    public const int Size32Bits = 4;
    public const int Size64Bits = 8;
    public const int Size8Bits = 1;

    public bool ArrayEquals<T>(T[] array1, T[] array2)
    {
        if ((array1 == null) && (array2 == null))
        {
            return true;
        }
        if ((array1 == null) || (array2 == null))
        {
            return false;
        }
        if (array1.Length != array2.Length)
        {
            return false;
        }
        return Enumerable.SequenceEqual<T>(array1, array2);
    }

    public bool Buffer(string strOut, ref MemoryStream mStream)
    {
        bool flag = true;
        try
        {
            byte[] bytesFromAsciiString = this.GetBytesFromAsciiString(strOut);
            mStream.Write(bytesFromAsciiString, 0, bytesFromAsciiString.Length);
        }
        catch (Exception exception)
        {
            string msg = "Output Buffer Error\n" + exception.Message + "\nDataUtils\n";
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            flag = false;
        }
        return flag;
    }

    public string BuildSimpleDataStr(byte[] data)
    {
        string str = string.Empty;
        if ((data.Length > 0) && (data != null))
        {
            for (uint i = 0; i < data.Length; i++)
            {
                str = str + string.Format("{0:X2}", data[i]);
            }
        }
        return str;
    }

    public void BuildSimpleDataStr(byte[] data, ref string msg, int length)
    {
        if ((length > 0) && (data != null))
        {
            string str = string.Empty;
            for (uint i = 0; i < length; i++)
            {
                str = str + string.Format("{0:X2}", data[i]);
            }
            msg = msg + str;
        }
    }

    public string BuildSimpleReverseDataStr(byte[] data)
    {
        string str = string.Empty;
        if ((data.Length > 0) && (data != null))
        {
            for (int i = data.Length - 1; i >= 0; i--)
            {
                str = str + string.Format("{0:X2}", data[i]);
            }
        }
        return str;
    }

    public string BuildTwoByteAddrDataStr(byte[] data)
    {
        return this.BuildTwoByteAddrDataStr(data, false);
    }

    public string BuildTwoByteAddrDataStr(string data)
    {
        string str = string.Empty;
        int length = data.Length;
        if (length <= 0)
        {
            return str;
        }
        string str2 = string.Empty;
        CharEnumerator enumerator = data.GetEnumerator();
        enumerator.MoveNext();
        uint num2 = 0;
        while (num2 < length)
        {
            str2 = str2 + string.Format("{0:X2}", enumerator.Current);
            if ((((num2 + 1) % 4) == 0) && (num2 != (length - 1)))
            {
                str2 = str2 + ":";
            }
            num2++;
            enumerator.MoveNext();
        }
        return (str + str2);
    }

    public string BuildTwoByteAddrDataStr(byte[] data, bool useLower)
    {
        string str = string.Empty;
        if (data == null)
        {
            return str;
        }
        int length = data.Length;
        if (length <= 0)
        {
            return str;
        }
        string str2 = string.Empty;
        for (uint i = 0; i < length; i++)
        {
            if (useLower)
            {
                str2 = str2 + string.Format("{0:x2}", data[i]);
            }
            else
            {
                str2 = str2 + string.Format("{0:X2}", data[i]);
            }
            if ((((i + 1) % 2) == 0) && (i != (data.Length - 1)))
            {
                str2 = str2 + ":";
            }
        }
        return (str + str2);
    }

    public void ByteArrayToStructure(byte[] byteArray, ref object dataStruct)
    {
        try
        {
            int cb = Marshal.SizeOf(dataStruct);
            IntPtr destination = Marshal.AllocHGlobal(cb);
            Marshal.Copy(byteArray, 0, destination, cb);
            dataStruct = Marshal.PtrToStructure(destination, dataStruct.GetType());
            Marshal.FreeHGlobal(destination);
        }
        catch
        {
            dataStruct = null;
        }
    }

    public bool CheckAsciiString(string str)
    {
        try
        {
            return (Encoding.UTF8.GetByteCount(str) == str.Length);
        }
        catch
        {
            return false;
        }
    }

    public bool CheckDCHashSet<T>(HashSet<T> newHashSet, HashSet<T> oldHashSet)
    {
        return ((newHashSet != null) && (oldHashSet != null));
    }

    public bool CheckDCList<T>(List<T> newList, List<T> oldList)
    {
        return ((newList != null) && (oldList != null));
    }

    public bool Compare8ByteArrays(byte[] data1, byte[] data2)
    {
        bool flag = false;
        try
        {
            if ((data1 == null) || (data2 == null))
            {
                return false;
            }
            if ((data1.Length == 0) && (data2.Length == 0))
            {
                return true;
            }
            if ((data1.Length == 0) || (data2.Length == 0))
            {
                return false;
            }
            byte[] data = new byte[8];
            byte[] buffer2 = new byte[8];
            int index = 0;
            for (int i = 8; i < 0x10; i++)
            {
                data[index] = data1[i];
                buffer2[index++] = data2[i];
            }
            if (this.BuildSimpleDataStr(data) == this.BuildSimpleDataStr(buffer2))
            {
                flag = true;
            }
        }
        catch (Exception exception)
        {
            flag = false;
            if (!this.handleException)
            {
                throw;
            }
            string msg = "Compare8ByteArrays\nCompare Data Issue\n" + exception.Message + "\nDataUtils\n";
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            return flag;
        }
        return flag;
    }

    public bool CompareByteArrays(byte[] data1, byte[] data2)
    {
        bool flag = false;
        try
        {
            if ((data1 == null) || (data2 == null))
            {
                return false;
            }
            if ((data1.Length == 0) && (data2.Length == 0))
            {
                return true;
            }
            if ((data1.Length == 0) || (data2.Length == 0))
            {
                return false;
            }
            if (this.BuildSimpleDataStr(data1) == this.BuildSimpleDataStr(data2))
            {
                flag = true;
            }
        }
        catch (Exception exception)
        {
            flag = false;
            if (!this.handleException)
            {
                throw;
            }
            string msg = "CompareByteArrays\nData Compare Issue\n" + exception.Message + "\nDataUtils\n";
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            return flag;
        }
        return flag;
    }

    public bool CompareDCListObjs<T>(List<T> newList, List<T> oldList)
    {
        if ((newList == null) && (oldList == null))
        {
            return true;
        }
        if ((newList != null) && (oldList == null))
        {
            return false;
        }
        return ((oldList != null) && (newList.Count == oldList.Count));
    }

    public static T DeepCopy<T>(T obj)
    {
        object obj2 = null;
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            stream.Position = 0L;
            obj2 = (T) formatter.Deserialize(stream);
            stream.Close();
        }
        return (T) obj2;
    }

    public string Get8BitsStr(byte data)
    {
        string str = "";
        try
        {
            for (int i = 0; i < 8; i++)
            {
                byte num2 = (byte) (data << i);
                if ((num2 & 0x80) == 0x80)
                {
                    str = str + "1";
                }
                else
                {
                    str = str + "0";
                }
            }
        }
        catch
        {
            str = "";
        }
        return str;
    }

    public string GetAsciiStringFromBytes(byte[] bytes)
    {
        string str = null;
        try
        {
            if (bytes != null)
            {
                str = new ASCIIEncoding().GetString(bytes);
            }
        }
        catch
        {
            str = null;
        }
        return str;
    }

    public byte GetByte16(ushort value, byte byteNumber)
    {
        return (byte) ((value >> (8 * byteNumber)) & 0xff);
    }

    public byte GetByte32(uint value, byte byteNumber)
    {
        return (byte) ((value >> (8 * byteNumber)) & 0xff);
    }

    public byte GetByte64(ulong value, byte byteNumber)
    {
        return (byte) ((value >> (8 * byteNumber)) & ((ulong) 0xffL));
    }

    public byte[] GetBytes(string str)
    {
        byte[] buffer = null;
        try
        {
            if (str != null)
            {
                byte[] dst = new byte[str.Length * 2];
                System.Buffer.BlockCopy(str.ToCharArray(), 0, dst, 0, dst.Length);
                buffer = dst;
            }
        }
        catch
        {
            buffer = null;
        }
        return buffer;
    }

    public byte[] GetBytesFromAsciiString(string str)
    {
        byte[] bytes = null;
        try
        {
            if (str != null)
            {
                bytes = new byte[str.Length];
                bytes = new ASCIIEncoding().GetBytes(str);
            }
        }
        catch
        {
            bytes = null;
        }
        return bytes;
    }

    public byte[] GetBytesFromString(string str)
    {
        byte[] buffer = null;
        try
        {
            int num = str.Length / 2;
            buffer = new byte[num];
            for (int i = 0; i < num; i++)
            {
                buffer[i] = Convert.ToByte(str.Substring(i * 2, 2), 0x10);
            }
        }
        catch
        {
            buffer = null;
        }
        return buffer;
    }

    public bool GetHandleException()
    {
        return this.handleException;
    }

    public byte[] GetHexBytes(string str, string[] delimiterStrs)
    {
        byte[] buffer = null;
        try
        {
            if (str == null)
            {
                return buffer;
            }
            string[] strArray = str.Split(delimiterStrs, StringSplitOptions.RemoveEmptyEntries);
            buffer = new byte[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                buffer[i] = Convert.ToByte(strArray[i], 0x10);
            }
        }
        catch
        {
            buffer = null;
        }
        return buffer;
    }

    public bool GetInt32(string strValue, string[] delimiterStrs, ref int value)
    {
        bool flag = true;
        try
        {
            string[] strArray = strValue.Split(delimiterStrs, StringSplitOptions.RemoveEmptyEntries);
            if ((strArray != null) && (strArray.Length > 0))
            {
                strArray[0] = strArray[0].Trim();
                value = Convert.ToInt32(strArray[0]);
                return flag;
            }
            value = 0;
        }
        catch (Exception exception)
        {
            string msg = "Get Index 32 Data Error\n" + exception.Message + "\nDataUtils\n";
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            flag = false;
            value = 0;
        }
        return flag;
    }

    public string GetIPAddrStr(byte[] ipAddr)
    {
        string str = "";
        try
        {
            if ((ipAddr != null) && (ipAddr.Length != 0))
            {
                str = new IPAddress(ipAddr).ToString();
            }
        }
        catch
        {
            str = "";
        }
        return str;
    }

    public byte[] GetReverseBytes(byte[] bytes)
    {
        byte[] buffer = new byte[0];
        try
        {
            if (bytes.Length <= 0)
            {
                return buffer;
            }
            buffer = new byte[bytes.Length];
            int num = bytes.Length - 1;
            foreach (byte num2 in bytes)
            {
                buffer[num--] = num2;
            }
        }
        catch
        {
            buffer = new byte[0];
        }
        return buffer;
    }

    public string GetString(byte[] bytes)
    {
        string str = null;
        try
        {
            if (bytes != null)
            {
                char[] dst = new char[bytes.Length / 2];
                System.Buffer.BlockCopy(bytes, 0, dst, 0, bytes.Length);
                str = new string(dst);
            }
        }
        catch
        {
            str = null;
        }
        return str;
    }

    public bool GetString(string strIn, string[] delimiterStrs, ref string strOut)
    {
        bool flag = true;
        try
        {
            string[] strArray = strIn.Split(delimiterStrs, StringSplitOptions.RemoveEmptyEntries);
            if ((strArray != null) && (strArray.Length > 0))
            {
                strArray[0] = strArray[0].Trim();
                strOut = strArray[0];
                return flag;
            }
            strOut = "";
        }
        catch (Exception exception)
        {
            string msg = "Get String Error\n" + exception.Message + "\nDataUtils\n";
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            flag = false;
            strOut = "";
        }
        return flag;
    }

    public string GetStringFromBytes(byte[] bytes)
    {
        return this.GetStringFromBytes(bytes, false);
    }

    public string GetStringFromBytes(byte[] bytes, bool reverseOrder)
    {
        try
        {
            StringBuilder builder = new StringBuilder(bytes.Length * 2);
            foreach (byte num in bytes)
            {
                if (!reverseOrder)
                {
                    builder.Append(num.ToString("X02"));
                }
                else
                {
                    builder.Insert(0, num.ToString("X02"));
                }
            }
            return builder.ToString();
        }
        catch
        {
            return "";
        }
    }

    public bool GetUInt32(string strValue, string[] delimiterStrs, ref uint value)
    {
        bool flag = true;
        int num = Convert.ToInt32((uint) value);
        flag = this.GetInt32(strValue, delimiterStrs, ref num);
        value = (uint) num;
        return flag;
    }

    public bool HashSetEquals<T>(HashSet<T> hashSet1, HashSet<T> hashSet2)
    {
        if ((hashSet1 == null) && (hashSet2 == null))
        {
            return true;
        }
        if ((hashSet1 == null) || (hashSet2 == null))
        {
            return false;
        }
        if (hashSet1.Count != hashSet2.Count)
        {
            return false;
        }
        return hashSet1.SetEquals(hashSet2);
    }

    public bool IsEmptyAddress(byte[] data)
    {
        if ((data != null) && (data.Length != 0))
        {
            for (uint i = 0; i < data.Length; i++)
            {
                if (data[i] != 0)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool ListEquals<T>(List<T> list1, List<T> list2)
    {
        if ((list1 == null) && (list2 == null))
        {
            return true;
        }
        if ((list1 == null) || (list2 == null))
        {
            return false;
        }
        if (list1.Count != list2.Count)
        {
            return false;
        }
        return Enumerable.SequenceEqual<T>(list1, list2);
    }

    public void Load16Bits(ref byte[] data, ref int index, ushort bits, ref bool dataErr, bool dataSwap)
    {
        try
        {
            if (data.Length < (index + 2))
            {
                dataErr = true;
                SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Load 16 Bits Failed -> Not Enough Destination Data Bytes For Load");
            }
            else if (dataSwap)
            {
                byte byteNumber = 1;
                for (int i = 0; i < 2; i++)
                {
                    byteNumber = (byte) (byteNumber - 1);
                    data[index++] = this.GetByte16(bits, byteNumber);
                }
            }
            else
            {
                byte num3 = 0;
                for (int j = 0; j < 2; j++)
                {
                    num3 = (byte) (num3 + 1);
                    data[index++] = this.GetByte16(bits, num3);
                }
            }
        }
        catch (Exception exception)
        {
            dataErr = true;
            if (!this.handleException)
            {
                throw;
            }
            string msg = "Load 16 Bits Failed\nData Transfer Issue\n" + exception.Message + "\nDataUtils\n";
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        }
    }

    public void Load32Bits(ref byte[] data, ref int index, uint bits, ref bool dataErr, bool dataSwap)
    {
        try
        {
            if (data.Length < (index + 4))
            {
                dataErr = true;
                SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Load 32 Bits Failed -> Not Enough Destination Data Bytes For Load");
            }
            else if (dataSwap)
            {
                byte byteNumber = 3;
                for (int i = 0; i < 4; i++)
                {
                    byteNumber = (byte) (byteNumber - 1);
                    data[index++] = this.GetByte32(bits, byteNumber);
                }
            }
            else
            {
                byte num3 = 0;
                for (int j = 0; j < 4; j++)
                {
                    num3 = (byte) (num3 + 1);
                    data[index++] = this.GetByte32(bits, num3);
                }
            }
        }
        catch (Exception exception)
        {
            dataErr = true;
            if (!this.handleException)
            {
                throw;
            }
            string msg = "Load 32 Bits Failed\nData Transfer Issue\n" + exception.Message + "\nDataUtils\n";
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        }
    }

    public void Load64Bits(ref byte[] data, ref int index, ulong bits, ref bool dataErr, bool dataSwap)
    {
        try
        {
            if (data.Length < (index + 8))
            {
                dataErr = true;
                SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Load 64 Bits Failed -> Not Enough Destination Data Bytes For Load");
            }
            else if (dataSwap)
            {
                byte byteNumber = 7;
                for (int i = 0; i < 8; i++)
                {
                    byteNumber = (byte) (byteNumber - 1);
                    data[index++] = this.GetByte64(bits, byteNumber);
                }
            }
            else
            {
                byte num3 = 0;
                for (int j = 0; j < 8; j++)
                {
                    num3 = (byte) (num3 + 1);
                    data[index++] = this.GetByte64(bits, num3);
                }
            }
        }
        catch (Exception exception)
        {
            dataErr = true;
            if (!this.handleException)
            {
                throw;
            }
            string msg = "Load 64 Bits Failed\nData Transfer Issue\n" + exception.Message + "\nDataUtils\n";
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        }
    }

    public void Load8Bits(ref byte[] data, ref int index, byte bits, ref bool dataErr)
    {
        try
        {
            if (data.Length < (index + 1))
            {
                dataErr = true;
                SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Load 8 Bits Failed -> Not Enough Destination Data Bytes For Load");
            }
            else
            {
                data[index++] = bits;
            }
        }
        catch (Exception exception)
        {
            dataErr = true;
            if (!this.handleException)
            {
                throw;
            }
            string msg = "Load 8 Bits Failed\nData Transfer Issue\n" + exception.Message + "\nDataUtils\n";
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        }
    }

    public void LoadDataBytes(ref byte[] data, ref int index, byte[] sourceData, ref bool dataErr)
    {
        try
        {
            if (data.Length < (index + sourceData.Length))
            {
                dataErr = true;
                SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Load Data Bytes Failed -> Not Enough Destination Data Bytes For Load");
            }
            else if (sourceData != null)
            {
                int num = index;
                for (int i = num; i < (num + sourceData.Length); i++)
                {
                    if (data.Length <= i)
                    {
                        return;
                    }
                    data[i] = sourceData[i - num];
                    index++;
                }
            }
        }
        catch (Exception exception)
        {
            dataErr = true;
            if (!this.handleException)
            {
                throw;
            }
            string msg = "Load Data Bytes\nData Transfer Issue\n" + exception.Message + "\nDataUtils\n";
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        }
    }

    public bool ReverseByteBits(ref byte[] bytes, int startIndex, int stopIndex)
    {
        bool flag = true;
        try
        {
            if (((bytes.Length > 0) || (startIndex > stopIndex)) || (stopIndex >= bytes.Length))
            {
                for (int i = startIndex; i <= stopIndex; i++)
                {
                    bytes[i] = this.revByteLookup[bytes[i]];
                }
                return flag;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    public ushort SetByte16(byte value, byte byteNumber)
    {
        return (ushort) (value << (8 * byteNumber));
    }

    public uint SetByte32(byte value, byte byteNumber)
    {
        return (uint) (value << (8 * byteNumber));
    }

    public ulong SetByte64(byte value, byte byteNumber)
    {
        return (ulong) (value << (8 * byteNumber));
    }

    public void SetHandleException(bool handleMyExceptions)
    {
        this.handleException = handleMyExceptions;
    }

    public byte[] StructureToByteArray(object dataStruct)
    {
        try
        {
            int cb = Marshal.SizeOf(dataStruct);
            byte[] destination = new byte[cb];
            IntPtr ptr = Marshal.AllocHGlobal(cb);
            Marshal.StructureToPtr(dataStruct, ptr, false);
            Marshal.Copy(ptr, destination, 0, cb);
            Marshal.FreeHGlobal(ptr);
            return destination;
        }
        catch
        {
            return null;
        }
    }

    public ushort Unload16Bits(byte[] data, ref int index, ref bool dataErr, bool dataSwap)
    {
        ushort bits = 0;
        return this.Unload16Bits(data, ref index, ref bits, ref dataErr, dataSwap);
    }

    public ushort Unload16Bits(byte[] data, ref int index, ref ushort bits, ref bool dataErr, bool dataSwap)
    {
        try
        {
            if (data.Length < (index + 2))
            {
                dataErr = true;
                SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Unload 16 Bits Failed -> Not Enough Source Data Bytes For Unload");
                return bits;
            }
            bits = 0;
            if (dataSwap)
            {
                byte num = 1;
                for (int j = 0; j < 2; j++)
                {
                    num = (byte) (num - 1);
                    bits = (ushort) (bits + this.SetByte16(data[index++], num));
                }
                return bits;
            }
            byte byteNumber = 0;
            for (int i = 0; i < 2; i++)
            {
                byteNumber = (byte) (byteNumber + 1);
                bits = (ushort) (bits + this.SetByte16(data[index++], byteNumber));
            }
        }
        catch (Exception exception)
        {
            dataErr = true;
            if (!this.handleException)
            {
                throw;
            }
            string msg = "Unload 16 Bits Failed\nData Transfer Issue\n" + exception.Message + "\nDataUtils\n";
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            return bits;
        }
        return bits;
    }

    public uint Unload32Bits(byte[] data, ref int index, ref bool dataErr, bool dataSwap)
    {
        uint bits = 0;
        return this.Unload32Bits(data, ref index, ref bits, ref dataErr, dataSwap);
    }

    public uint Unload32Bits(byte[] data, ref int index, ref uint bits, ref bool dataErr, bool dataSwap)
    {
        try
        {
            if (data.Length < (index + 4))
            {
                dataErr = true;
                SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Unload 32 Bits Failed -> Not Enough Source Data Bytes For Unload");
                return bits;
            }
            bits = 0;
            if (dataSwap)
            {
                byte num = 3;
                for (int j = 0; j < 4; j++)
                {
                    num = (byte) (num - 1);
                    bits += this.SetByte32(data[index++], num);
                }
                return bits;
            }
            byte byteNumber = 0;
            for (int i = 0; i < 4; i++)
            {
                byteNumber = (byte) (byteNumber + 1);
                bits += this.SetByte32(data[index++], byteNumber);
            }
        }
        catch (Exception exception)
        {
            dataErr = true;
            if (!this.handleException)
            {
                throw;
            }
            string msg = "Unload 32 Bits Failed\nData Transfer Issue\n" + exception.Message + "\nDataUtils\n";
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            return bits;
        }
        return bits;
    }

    public ulong Unload64Bits(byte[] data, ref int index, ref bool dataErr, bool dataSwap)
    {
        ulong bits = 0L;
        return this.Unload64Bits(data, ref index, ref bits, ref dataErr, dataSwap);
    }

    public ulong Unload64Bits(byte[] data, ref int index, ref ulong bits, ref bool dataErr, bool dataSwap)
    {
        try
        {
            if (data.Length < (index + 8))
            {
                dataErr = true;
                SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Unload 64 Bits Failed -> Not Enough Source Data Bytes For Unload");
                return bits;
            }
            bits = 0L;
            if (dataSwap)
            {
                byte num = 7;
                for (int j = 0; j < 8; j++)
                {
                    num = (byte) (num - 1);
                    bits += this.SetByte64(data[index++], num);
                }
                return bits;
            }
            byte byteNumber = 0;
            for (int i = 0; i < 8; i++)
            {
                byteNumber = (byte) (byteNumber + 1);
                bits += this.SetByte64(data[index++], byteNumber);
            }
        }
        catch (Exception exception)
        {
            dataErr = true;
            if (!this.handleException)
            {
                throw;
            }
            string msg = "Unload 64 Bits Failed\nData Transfer Issue\n" + exception.Message + "\nDataUtils\n";
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            return bits;
        }
        return bits;
    }

    public byte Unload8Bits(byte[] data, ref int index, ref bool dataErr)
    {
        byte bits = 0;
        return this.Unload8Bits(data, ref index, ref bits, ref dataErr);
    }

    public byte Unload8Bits(byte[] data, ref int index, ref byte bits, ref bool dataErr)
    {
        try
        {
            if (data.Length < (index + 1))
            {
                dataErr = true;
                SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Unload 8 Bits Failed -> Not Enough Source Data Bytes For Unload");
                return bits;
            }
            bits = data[index++];
        }
        catch (Exception exception)
        {
            dataErr = true;
            if (!this.handleException)
            {
                throw;
            }
            string msg = "Unload 8 Bits Failed\nData Transfer Issue\n" + exception.Message + "\nDataUtils\n";
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            return bits;
        }
        return bits;
    }

    public void UnloadDataBytes(byte[] data, ref int index, ref byte[] destData, ref bool dataErr)
    {
        this.UnloadDataBytes(data, data.Length, ref index, ref destData, ref dataErr);
    }

    public void UnloadDataBytes(byte[] data, int dataLength, ref int index, ref byte[] destData, ref bool dataErr)
    {
        try
        {
            if ((data.Length < (index + dataLength)) || (destData.Length < dataLength))
            {
                dataErr = true;
                SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Unload Data Bytes Failed -> Not Enough Source Data Bytes Or Destination Bytes For Unload");
            }
            else if (destData != null)
            {
                int num = index;
                int num2 = 0;
                for (int i = num; i < (num + dataLength); i++)
                {
                    if (destData.Length < num2)
                    {
                        dataErr = true;
                        return;
                    }
                    destData[num2] = data[index];
                    num2++;
                    index++;
                }
            }
        }
        catch (Exception exception)
        {
            dataErr = true;
            if (!this.handleException)
            {
                throw;
            }
            string msg = "Unload Data Bytes\nData Transfer Issue\n" + exception.Message + "\nDataUtils\n";
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
        }
    }
}


﻿namespace BTool
{
    using System;
    using System.Collections.Generic;

    public class DeviceFormUtils
    {
        private DataUtils dataUtils = new DataUtils();
        private const string moduleName = "DeviceFormUtils";
        private MsgBox msgBox = new MsgBox();

        public void BuildRawDataStr(byte[] data, ref string msg, int length)
        {
            if (length > 0)
            {
                string str = string.Empty;
                for (uint i = 0; i < length; i++)
                {
                    str = str + string.Format("{0:X2} ", data[i]);
                    this.CheckLineLength(ref str, i, true);
                }
                msg = msg + string.Format(" Raw\t\t: {0:S}\n", str);
            }
        }

        public void CheckLineLength(ref string msg, uint lineIndex, bool addTabs)
        {
            if (((lineIndex + 1) % 0x10) == 0)
            {
                if (addTabs)
                {
                    msg = msg + "\n\t\t  ";
                }
                else
                {
                    msg = msg + "\n";
                }
            }
        }

        public bool ConvertDisplayTypes(ValueDisplay inValueDisplay, string inStr, ref ValueDisplay outValueDisplay, ref string outStr, bool displayMsg)
        {
            bool flag = true;
            if ((inStr == null) || (inStr.Length == 0))
            {
                outStr = inStr;
                flag = true;
            }
            else if (outStr == null)
            {
                if (displayMsg)
                {
                    string msg = "Out String Cannot Be Null\nDeviceFormUtils\n";
                    this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                }
                flag = false;
            }
            else if (inValueDisplay == outValueDisplay)
            {
                outStr = inStr;
            }
            else
            {
                string str2 = "";
                switch (inValueDisplay)
                {
                    case ValueDisplay.Hex:
                        try
                        {
                            str2 = inStr;
                        }
                        catch (Exception exception)
                        {
                            if (displayMsg)
                            {
                                string str3 = string.Format("Cannot Convert The Incoming String Value From Hex\n\n{0}\n", exception.Message) + "DeviceFormUtils\n";
                                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str3);
                            }
                            flag = false;
                        }
                        break;

                    case ValueDisplay.Dec:
                        try
                        {
                            uint bits = Convert.ToUInt32(inStr, 10);
                            int index = 0;
                            bool dataErr = false;
                            byte[] data = new byte[4];
                            this.dataUtils.Load32Bits(ref data, ref index, bits, ref dataErr, false);
                            if (dataErr)
                            {
                                throw new ApplicationException("Error Loading 32 Bit Value");
                            }
                            int num3 = 0;
                            for (index = data.Length - 1; index >= 0; index--)
                            {
                                if (data[index] != 0)
                                {
                                    break;
                                }
                                num3++;
                            }
                            if (num3 == 4)
                            {
                                num3 = 3;
                            }
                            byte[] destinationArray = new byte[4 - num3];
                            Array.Copy(data, destinationArray, destinationArray.Length);
                            for (index = 0; index < destinationArray.Length; index++)
                            {
                                str2 = str2 + destinationArray[index].ToString("X2");
                                if (index < (destinationArray.Length - 1))
                                {
                                    str2 = str2 + ":";
                                }
                            }
                        }
                        catch (Exception exception2)
                        {
                            if (displayMsg)
                            {
                                string str4 = string.Format("Cannot Convert The Incoming String Value From Decimal\n\n{0}\n", exception2.Message) + "DeviceFormUtils\n";
                                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str4);
                            }
                            flag = false;
                        }
                        break;

                    case ValueDisplay.Ascii:
                        try
                        {
                            if (!this.dataUtils.CheckAsciiString(inStr))
                            {
                                throw new ApplicationException("Ascii String Value Contains Unprintable Characters");
                            }
                            byte[] bytesFromAsciiString = this.dataUtils.GetBytesFromAsciiString(inStr);
                            for (int i = 0; i < bytesFromAsciiString.Length; i++)
                            {
                                str2 = str2 + string.Format("{0:S}", bytesFromAsciiString[i].ToString("X2"));
                                if (i < (bytesFromAsciiString.Length - 1))
                                {
                                    str2 = str2 + ":";
                                }
                            }
                        }
                        catch (Exception exception3)
                        {
                            if (displayMsg)
                            {
                                string str5 = string.Format("Cannot Convert The Incoming String Value From Ascii\n\n{0}\n", exception3.Message) + "DeviceFormUtils\n";
                                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str5);
                            }
                            flag = false;
                        }
                        break;

                    default:
                        if (displayMsg)
                        {
                            string str6 = string.Format("Unknown Incoming String Type #{0}\n", inValueDisplay) + "DeviceFormUtils\n";
                            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str6);
                        }
                        flag = false;
                        break;
                }
                if (flag)
                {
                    string[] strArray = str2.Split(new char[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str7 in strArray)
                    {
                        if (str7.Length == 0)
                        {
                            if (displayMsg)
                            {
                                string str8 = "Incoming String Conversion Missing Byte In Delimited Format\nDeviceFormUtils\n";
                                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str8);
                            }
                            flag = false;
                            break;
                        }
                        if (str7.Length != 2)
                        {
                            if (displayMsg)
                            {
                                string str9 = "Incoming String Conversion Not In Single Byte Delimited Format\nDeviceFormUtils\n";
                                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str9);
                            }
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        switch (outValueDisplay)
                        {
                            case ValueDisplay.Hex:
                                try
                                {
                                    outStr = str2;
                                }
                                catch (Exception exception4)
                                {
                                    if (displayMsg)
                                    {
                                        string str10 = string.Format("Cannot Convert The Outgoing String Value To Hex\n\n{0}\n", exception4.Message) + "DeviceFormUtils\n";
                                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str10);
                                    }
                                    flag = false;
                                }
                                goto Label_0562;

                            case ValueDisplay.Dec:
                                try
                                {
                                    foreach (string str12 in strArray)
                                    {
                                        outStr = outStr + str12;
                                    }
                                    if (strArray.Length > 4)
                                    {
                                        throw new ApplicationException("Conversion String Exceeds Four Hex Bytes");
                                    }
                                    uint num5 = 0;
                                    for (int j = 0; j < strArray.Length; j++)
                                    {
                                        num5 += (uint) (Convert.ToByte(strArray[j], 0x10) << ((byte) (8 * j)));
                                    }
                                    outStr = string.Format("{0:D}", num5);
                                    Convert.ToUInt32(outStr, 10);
                                }
                                catch (Exception exception5)
                                {
                                    if (displayMsg)
                                    {
                                        string str13 = string.Format("Cannot Convert The Outgoing String Value To Decimal\n\n{0}\n", exception5.Message) + "DeviceFormUtils\n";
                                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str13);
                                    }
                                    flag = false;
                                }
                                goto Label_0562;

                            case ValueDisplay.Ascii:
                                try
                                {
                                    foreach (string str14 in strArray)
                                    {
                                        char ch = Convert.ToChar(Convert.ToByte(str14, 0x10));
                                        outStr = outStr + string.Format("{0:S}", ch.ToString());
                                    }
                                    if (!this.dataUtils.CheckAsciiString(outStr))
                                    {
                                        throw new ApplicationException("Ascii String Value Contains Unprintable Characters");
                                    }
                                }
                                catch (Exception exception6)
                                {
                                    if (displayMsg)
                                    {
                                        string str15 = string.Format("Cannot Convert The Outgoing String Value To Ascii\n\n{0}\n", exception6.Message) + "DeviceFormUtils\n";
                                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str15);
                                    }
                                    flag = false;
                                }
                                goto Label_0562;
                        }
                        if (displayMsg)
                        {
                            string str11 = string.Format("Unknown Out String Type #{0}\n", (ValueDisplay) outValueDisplay) + "DeviceFormUtils\n";
                            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str11);
                        }
                        flag = false;
                    }
                }
            }
        Label_0562:
            if ((outStr != null) && !flag)
            {
                outStr = inStr;
                outValueDisplay = inValueDisplay;
            }
            return flag;
        }

        public string GetAttExecuteWriteFlagsStr(byte executeWriteFlags)
        {
            switch (executeWriteFlags)
            {
                case 0:
                    return "Cancel All Prepared Writes";

                case 1:
                    return "Immediately Write All Pending Prepared Values";
            }
            return "Unknown Execute Write Flags";
        }

        public string GetErrorStatusStr(byte errorStatus)
        {
            string newLineSpacer = "\n       \t\t  ";
            return this.GetErrorStatusStr(errorStatus, newLineSpacer);
        }

        public string GetErrorStatusStr(byte errorStatus, string newLineSpacer)
        {
            switch (errorStatus)
            {
                case 1:
                    return ("The Attribute Handle Given Was Not " + newLineSpacer + "Valid On This Server.");

                case 2:
                    return "The Attribute Cannot Be Read.";

                case 3:
                    return "The Attribute Cannot Be Written.";

                case 4:
                    return "The Attribute PDU Was Invalid.";

                case 5:
                    return ("The attribute Requires Authentication " + newLineSpacer + "Before It Can Be Read Or Written.");

                case 6:
                    return ("Attribute Server Does Not Support The " + newLineSpacer + "Request Received From The Client.");

                case 7:
                    return ("Offset Specified Was Past The End Of " + newLineSpacer + "The Attribute.");

                case 8:
                    return ("The Attribute Requires Authorization " + newLineSpacer + "Before It Can Be Read Or Written.");

                case 9:
                    return "Too Many Prepare Writes Have Been Queued.";

                case 10:
                    return ("No Attribute Found Within The Given " + newLineSpacer + "Attribute Handle Range");

                case 11:
                    return ("The Attribute cannot Be Read Or Written " + newLineSpacer + "Using The Read Blob Request.");

                case 12:
                    return ("The Encryption Key Size Used For " + newLineSpacer + "Encrypting This Link Is Insufficient.");

                case 13:
                    return ("The Attribute Value Length Is Invalid " + newLineSpacer + "For The Operation.");

                case 14:
                    return ("The Attribute Request That Was Requested " + newLineSpacer + "Has Encountered An Error That Was Unlikely, " + newLineSpacer + "And Therefore Could Not Be Completed As Requested.");

                case 15:
                    return ("The Attribute Requires Encryption Before It " + newLineSpacer + "Can Be Read Or Written.");

                case 0x10:
                    return ("The attribute Type Is Not A supported Grouping " + newLineSpacer + "Attribute As Defined By A Higher Layer Specification.");

                case 0x11:
                    return "Insufficient Resources To Complete The Request.";

                case 0x80:
                    return "Invaild Value.";
            }
            return "Unknown Error Status";
        }

        public string GetFindFormatStr(byte findFormat)
        {
            switch (findFormat)
            {
                case 1:
                    return "A List Of 1 Or More Handles With Their 16-bit Bluetooth UUIDs";

                case 2:
                    return "A List Of 1 Or More Handles With Their 128-bit UUIDs";
            }
            return "Unknown Find Format";
        }

        public string GetGapAddrTypeStr(byte addrType)
        {
            switch (addrType)
            {
                case 0:
                    return "Public";

                case 1:
                    return "Static";

                case 2:
                    return "PrivateNonResolve";

                case 3:
                    return "PrivateResolve";
            }
            return "Unknown Addr Type";
        }

        public string GetGapAdTypesStr(byte adTypes)
        {
            string str = "\n       \t\t  ";
            switch (adTypes)
            {
                case 1:
                    return "Flags: Discovery Mode";

                case 2:
                    return "Service: More 16-bit UUIDs Available";

                case 3:
                    return "Service: Complete List Of 16-bit UUIDs";

                case 4:
                    return "Service: More 32-bit UUIDs Available";

                case 5:
                    return "Service: Complete List Of 32-bit UUIDs";

                case 6:
                    return "Service: More 128-bit UUIDs Available";

                case 7:
                    return "Service: Complete List Of 128-bit UUIDs";

                case 8:
                    return "Shortened Local Name";

                case 9:
                    return "Complete Local Name";

                case 10:
                    return "TX Power Level: 0xXX: -127 to +127 dBm";

                case 13:
                    return ("Simple Pairing OOB Tag: Class Of device" + str + " (3 octets)");

                case 14:
                    return ("Simple Pairing OOB Tag: Simple Pairing " + str + "Hash C (16 octets)");

                case 15:
                    return ("Simple Pairing OOB Tag: Simple Pairing " + str + "Randomizer R (16 octets)");

                case 0x10:
                    return "Security Manager TK Value";

                case 0x11:
                    return "Secutiry Manager OOB Flags";

                case 0x12:
                    return ("Min And Max Values Of The Connection Interval " + str + "(2 Octets Min, 2 Octets Max) (0xFFFF Indicates " + str + "No Conn Interval Min Or Max)");

                case 0x13:
                    return "Signed Data Field";

                case 20:
                    return ("Service Solicitation: List Of 16-bit " + str + "Service UUIDs");

                case 0x15:
                    return ("Service Solicitation: List Of 128-bit " + str + "Service UUIDs");

                case 0x16:
                    return "Service Data";

                case 0xff:
                    return ("Manufacturer Specific Data: First 2 Octets " + str + "Contain The Company Identifier Code " + str + "Followed By The Additional Manufacturer " + str + "Specific Data");
            }
            return "Unknown Gap Ad Types";
        }

        public string GetGapAdventAdTypeStr(byte adType)
        {
            switch (adType)
            {
                case 0:
                    return "SCAN_RSP data";

                case 1:
                    return "Advertisement data";
            }
            return "Unknown GAP Advent Ad Type";
        }

        public string GetGapAuthenticatedCsrkStr(byte authCsrk)
        {
            switch (authCsrk)
            {
                case 0:
                    return "CSRK Is Not Authenticated";

                case 1:
                    return "CSRK Is Authenticated";
            }
            return "Unknown GAP Authenticated Csrk";
        }

        public string GetGapAuthReqStr(byte authReq)
        {
            string str = "\n       \t\t  ";
            string str2 = string.Empty;
            if (authReq == 0)
            {
                return "Gap Auth Req Bit Mask Is Not Set";
            }
            byte num = 0;
            num = 1;
            if ((authReq & num) == num)
            {
                str2 = "Bonding - exchange and save key information";
            }
            num = 4;
            if ((authReq & num) == num)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + str;
                }
                str2 = str2 + "Man-In-The-Middle protection";
            }
            if (string.IsNullOrEmpty(str2))
            {
                str2 = "Unknown Gap Auth Req";
            }
            return str2;
        }

        public string GetGapBondParamIdStr(ushort bondParamId)
        {
            switch (bondParamId)
            {
                case 0x400:
                    return "GAPBOND_PAIRING_MODE";

                case 0x401:
                    return "GAPBOND_INITIATE_WAIT";

                case 0x402:
                    return "GAPBOND_MITM_PROTECTION";

                case 0x403:
                    return "GAPBOND_IO_CAPABILITIES";

                case 0x404:
                    return "GAPBOND_OOB_ENABLED";

                case 0x405:
                    return "GAPBOND_OOB_DATA";

                case 0x406:
                    return "GAPBOND_BONDING_ENABLED";

                case 0x407:
                    return "GAPBOND_KEY_DIST_LIST";

                case 0x408:
                    return "GAPBOND_DEFAULT_PASSCODE";

                case 0x409:
                    return "GAPBOND_ERASE_ALLBONDS";

                case 0x40a:
                    return "GAPBOND_AUTO_FAIL_PAIRING";

                case 0x40b:
                    return "GAPBOND_AUTO_FAIL_REASON";

                case 0x40c:
                    return "GAPBOND_KEYSIZE";

                case 0x40d:
                    return "GAPBOND_AUTO_SYNC_WL";

                case 0x40e:
                    return "GAPBOND_BOND_COUNT";
            }
            return "Unknown Gap Bond Param ID";
        }

        public string GetGapChannelMapStr(byte channelMap)
        {
            string str = "\n       \t\t  ";
            string str2 = string.Empty;
            if (channelMap == 0)
            {
                return "Channel Map Bit Mask Is Not Set";
            }
            byte num = 0;
            num = 0;
            if ((channelMap & num) == num)
            {
                str2 = "Channel 37";
            }
            num = 1;
            if ((channelMap & num) == num)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + str;
                }
                str2 = str2 + "Channel 38";
            }
            num = 2;
            if ((channelMap & num) == num)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + str;
                }
                str2 = str2 + "Channel 39";
            }
            if (string.IsNullOrEmpty(str2))
            {
                str2 = "Unknown Gap Channel Map";
            }
            return str2;
        }

        public string GetGapDiscoveryModeStr(byte discoveryMode)
        {
            switch (discoveryMode)
            {
                case 0:
                    return "Nondiscoverable";

                case 1:
                    return "General";

                case 2:
                    return "Limited";

                case 3:
                    return "All";
            }
            return "Unknown Discovery Mode";
        }

        public string GetGapEnableDisableStr(byte gapEnableDisable)
        {
            switch (gapEnableDisable)
            {
                case 0:
                    return "Disable";

                case 1:
                    return "Enable";
            }
            return "Unknown Gap EnableDisable";
        }

        public string GetGapEventTypeStr(byte eventType)
        {
            switch (eventType)
            {
                case 0:
                    return "Connectable Undirect Advertisement";

                case 1:
                    return "Connectable Direct Advertisement";

                case 2:
                    return "Scannable Undirect Advertisement";

                case 3:
                    return "Non-connectable Undirect Advertisement";

                case 4:
                    return "Scan Response";
            }
            return "Unknown Gap Event Type";
        }

        public string GetGapFilterPolicyStr(byte filterPolicy)
        {
            string str = "\n       \t\t  ";
            switch (filterPolicy)
            {
                case 0:
                    return ("Allow Scan Requests From Any, Allow " + str + "Connect Request From Any.");

                case 1:
                    return ("Allow Scan Requests From White List Only, " + str + "Allow Connect Request From Any.");

                case 2:
                    return ("Allow Scan Requests From Any, Allow " + str + "Connect Request From White List Only.");

                case 3:
                    return ("Allow Scan Requests From White List Only, " + str + "Allow Connect Requests From White List Only.");
            }
            return "Unknown Gap Filter Policy";
        }

        public string GetGapIOCapsStr(byte ioCaps)
        {
            switch (ioCaps)
            {
                case 0:
                    return "DisplayOnly";

                case 1:
                    return "DisplayYesNo";

                case 2:
                    return "KeyboardOnly";

                case 3:
                    return "NoInputNoOutput";

                case 4:
                    return "KeyboardDisplay";
            }
            return "Unknown Gap IO Caps";
        }

        public string GetGapKeyDiskStr(byte keyDisk)
        {
            string str = "\n       \t\t  ";
            string str2 = string.Empty;
            if (keyDisk == 0)
            {
                return "Gap Key Disk Bit Mask Is Not Set";
            }
            byte num = 0;
            num = 1;
            if ((keyDisk & num) == num)
            {
                str2 = "Slave Encryption Key";
            }
            num = 2;
            if ((keyDisk & num) == num)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + str;
                }
                str2 = str2 + "Slave Identification Key";
            }
            num = 4;
            if ((keyDisk & num) == num)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + str;
                }
                str2 = str2 + "Slave Signing Key";
            }
            num = 8;
            if ((keyDisk & num) == num)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + str;
                }
                str2 = str2 + "Master Encryption Key";
            }
            num = 0x10;
            if ((keyDisk & num) == num)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + str;
                }
                str2 = str2 + "Master Identification Key";
            }
            num = 0x20;
            if ((keyDisk & num) == num)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + str;
                }
                str2 = str2 + "Master Signing Key";
            }
            if (string.IsNullOrEmpty(str2))
            {
                str2 = "Unknown Gap Key Disk";
            }
            return str2;
        }

        public string GetGapOobDataFlagStr(byte dataFlag)
        {
            switch (dataFlag)
            {
                case 0:
                    return "Out-Of-Bounds (OOB) Data Is NOT Available";

                case 1:
                    return "Out-Of-Bounds (OOB) Data Is Available";
            }
            return "Unknown Gap Oob Data Flag";
        }

        public string GetGapParamIdStr(byte paramId)
        {
            string str = "\n       \t\t  ";
            switch (paramId)
            {
                case 0:
                    return ("Minimum Time To Remain Advertising When In , " + str + "Discoverable Mode (mSec). Setting This " + str + "Parameter To 0 Turns Off The Timer " + str + "(default). TGAP_GEN_DISC_ADV_MIN");

                case 1:
                    return ("Maximum Time To Remain Advertising, When In " + str + "Limited Discoverable Mode (mSec). TGAP_LIM_ADV_TIMEOUT");

                case 2:
                    return ("Minimum Time To Perform Scanning, When Performing " + str + "General Discovery Proc (mSec). TGAP_GEN_DISC_SCAN");

                case 3:
                    return ("Minimum Time To Perform Scanning, When Performing " + str + "Limited Discovery Proc (mSec). TGAP_LIM_DISC_SCAN");

                case 4:
                    return ("Advertising Timeout, When Performing " + str + "Connection Establishment Proc (mSec). " + str + "TGAP_CONN_EST_ADV_TIMEOUT");

                case 5:
                    return ("Link Layer Connection Parameter Update " + str + "Notification Timer, Connection Parameter " + str + "Update Proc (mSec). TGAP_CONN_PARAM_TIMEOUT");

                case 6:
                    return ("Minimum Advertising Interval, When In Limited " + str + "Discoverable Mode (mSec). TGAP_LIM_DISC_ADV_INT_MIN");

                case 7:
                    return ("Maximum Advertising Interval, When In Limited " + str + "Discoverable Mode (mSec). TGAP_LIM_DISC_ADV_INT_MAX");

                case 8:
                    return ("Minimum Advertising Interval, When In General " + str + "Discoverable Mode (mSec). TGAP_GEN_DISC_ADV_INT_MIN");

                case 9:
                    return ("Maximum Advertising Interval, When In General " + str + "Discoverable Mode (mSec). TGAP_GEN_DISC_ADV_INT_MAX");

                case 10:
                    return ("Minimum Advertising Interval, When In Connectable " + str + "Mode (mSec). TGAP_CONN_ADV_INT_MIN");

                case 11:
                    return ("Maximum Advertising Interval, When In Connectable " + str + "Mode (mSec). TGAP_CONN_ADV_INT_MAX");

                case 12:
                    return ("Scan Interval Used During Link Layer Initiating " + str + "State, When In Connectable Mode (mSec). TGAP_CONN_SCAN_INT");

                case 13:
                    return ("Scan Window Used During Link Layer Initiating " + str + "State, When In Connectable Mode (mSec). " + str + "TGAP_CONN_SCAN_WIND");

                case 14:
                    return ("Scan Interval Used During Link Layer Initiating " + str + "State, When In Connectable Mode, High Duty " + str + "Scan Cycle Scan Paramaters (mSec). TGAP_CONN_HIGH_SCAN_INT");

                case 15:
                    return ("Scan Window Used During Link Layer Initiating " + str + "State, When In Connectable Mode, High Duty " + str + "Scan Cycle Scan Paramaters (mSec). TGAP_CONN_HIGH_SCAN_WIND");

                case 0x10:
                    return ("Scan Interval Used During Link Layer Scanning " + str + "State, When In General Discovery " + str + "Proc (mSec). TGAP_GEN_DISC_SCAN_INT");

                case 0x11:
                    return ("Scan Window Used During Link Layer Scanning " + str + "State, When In General Discovery " + str + "Proc (mSec). TGAP_GEN_DISC_SCAN_WIND");

                case 0x12:
                    return ("Scan Interval Used During Link Layer Scanning " + str + "State, When In Limited Discovery " + str + "Proc (mSec). TGAP_LIM_DISC_SCAN_INT");

                case 0x13:
                    return ("Scan Window Used During Link Layer Scanning " + str + "State, When In Limited Discovery " + str + "Proc (mSec). TGAP_LIM_DISC_SCAN_WIND");

                case 20:
                    return ("Advertising Interval, When Using Connection " + str + "Establishment Proc (mSec). TGAP_CONN_EST_ADV");

                case 0x15:
                    return ("Minimum Link Layer Connection Interval, " + str + "When Using Connection Establishment " + str + "Proc (mSec). TGAP_CONN_EST_INT_MIN");

                case 0x16:
                    return ("Maximum Link Layer Connection Interval, " + str + "When Using Connection Establishment " + str + "Proc (mSec). TGAP_CONN_EST_INT_MAX");

                case 0x17:
                    return ("Scan Interval Used During Link Layer Initiating " + str + "State, When Using Connection Establishment " + str + "Proc (mSec). TGAP_CONN_EST_SCAN_INT");

                case 0x18:
                    return ("Scan window Used During Link Layer Initiating " + str + "State, When Using Connection Establishment " + str + "Proc (mSec). TGAP_CONN_EST_SCAN_WIND");

                case 0x19:
                    return ("Link Layer Connection Supervision Timeout, " + str + "When Using Connection Establishment " + str + "Proc (mSec). TGAP_CONN_EST_SUPERV_TIMEOUT");

                case 0x1a:
                    return ("Link Layer Connection Slave Latency, When Using " + str + "Connection Establishment Proc (mSec) TGAP_CONN_EST_LATENCY");

                case 0x1b:
                    return ("Local Informational Parameter About Min Len " + str + "Of Connection Needed, When Using Connection" + str + " Establishment Proc (mSec). TGAP_CONN_EST_MIN_CE_LEN");

                case 0x1c:
                    return ("Local Informational Parameter About Max Len " + str + "Of Connection Needed, When Using Connection " + str + "Establishment Proc (mSec). TGAP_CONN_EST_MAX_CE_LEN");

                case 0x1d:
                    return ("Minimum Time Interval Between Private " + str + "(Resolvable) Address Changes. In Minutes " + str + "(Default 15 Minutes) TGAP_PRIVATE_ADDR_INT");

                case 30:
                    return ("SM Message Timeout (Milliseconds). " + str + "(Default 30 Seconds). TGAP_SM_TIMEOUT");

                case 0x1f:
                    return ("SM Minimum Key Length Supported " + str + "(default 7). TGAP_SM_MIN_KEY_LEN");

                case 0x20:
                    return ("SM Maximum Key Length Supported " + str + "(Default 16). TGAP_SM_MAX_KEY_LEN");

                case 0x21:
                    return "TGAP_FILTER_ADV_REPORTS";

                case 0x22:
                    return "TGAP_SCAN_RSP_RSSI_MIN";

                case 0x23:
                    return ("GAP TestCodes - Puts GAP Into A " + str + "Test Mode TGAP_GAP_TESTCODE");

                case 0x24:
                    return ("SM TestCodes - Puts SM Into A " + str + "Test Mode TGAP_SM_TESTCODE");

                case 100:
                    return ("GATT TestCodes - Puts GATT Into A Test " + str + "Mode (ParamValue Maintained By GATT) " + str + "TGAP_GATT_TESTCODE");

                case 0x65:
                    return ("ATT TestCodes - Puts ATT Into A Test Mode " + str + "(ParamValue Maintained By ATT) TGAP_ATT_TESTCODE");

                case 0x66:
                    return "TGAP_GGS_TESTCODE";

                case 0xfe:
                    return "SET_RX_DEBUG";

                case 0xff:
                    return "GET_MEM_USED";
            }
            return "Unknown Gap Param Id";
        }

        public string GetGapProfileStr(byte gapProfile)
        {
            switch (gapProfile)
            {
                case 1:
                    return "Broadcaster";

                case 2:
                    return "Observer";

                case 4:
                    return "Peripheral";

                case 8:
                    return "Central";
            }
            return "Unknown Gap Profile";
        }

        public string GetGapSMPFailureTypesStr(byte failTypes)
        {
            switch (failTypes)
            {
                case 0:
                    return "SUCCESS";

                case 1:
                    return "SMP_PAIRING_FAILED_PASSKEY_ENTRY_FAILED";

                case 2:
                    return "SMP_PAIRING_FAILED_OOB_NOT_AVAIL";

                case 3:
                    return "SMP_PAIRING_FAILED_AUTH_REQ";

                case 4:
                    return "SMP_PAIRING_FAILED_CONFIRM_VALUE";

                case 5:
                    return "SMP_PAIRING_FAILED_NOT_SUPPORTED";

                case 6:
                    return "SMP_PAIRING_FAILED_ENC_KEY_SIZE";

                case 7:
                    return "SMP_PAIRING_FAILED_CMD_NOT_SUPPORTED";

                case 8:
                    return "SMP_PAIRING_FAILED_UNSPECIFIED";

                case 9:
                    return "SMP_PAIRING_FAILED_REPEATED_ATTEMPTS";

                case 0x17:
                    return "bleTimeout";
            }
            return "Unknown Gap SMP Failure Types";
        }

        public string GetGapTerminationReasonStr(byte termReason)
        {
            switch (termReason)
            {
                case 0x3b:
                    return "LSTO Violation";

                case 0x3d:
                    return "MIC Failure";

                case 0x3e:
                    return "Failed To Establish";

                case 0x3f:
                    return "MAC Connection Failed";

                case 40:
                    return "Control Packet Instant Passed";

                case 0x22:
                    return "Control Packet Timeout";

                case 8:
                    return "Supervisor Timeout";

                case 0x13:
                    return "Peer Requested";

                case 0x16:
                    return "Host Requested";
            }
            return "Unknown Gap Termination Reason";
        }

        public string GetGapTrueFalseStr(byte gapTrueFalse)
        {
            switch (gapTrueFalse)
            {
                case 0:
                    return "False";

                case 1:
                    return "True";
            }
            return "Unknown Gap TrueFalse";
        }

        public string GetGapUiInputStr(byte uiInput)
        {
            switch (uiInput)
            {
                case 0:
                    return "Don’t Ask User To Input A Passcode";

                case 1:
                    return "Ask User To Input A Passcode";
            }
            return "Unknown GAP UI Input";
        }

        public string GetGapUiOutputStr(byte uiOutput)
        {
            switch (uiOutput)
            {
                case 0:
                    return "Don’t Display Passcode";

                case 1:
                    return "Display A Passcode";
            }
            return "Unknown GAP UI Input";
        }

        public string GetGapYesNoStr(byte gapYesNo)
        {
            switch (gapYesNo)
            {
                case 0:
                    return "No";

                case 1:
                    return "Yes";
            }
            return "Unknown Gap Yes No";
        }

        public string GetGattCharProperties(byte properties, bool useShort)
        {
            string str = string.Empty;
            string str2 = " ";
            if (properties == 0)
            {
                return str;
            }
            byte num = 0;
            num = 1;
            if ((properties & num) == num)
            {
                if (useShort)
                {
                    str = str + "Bcst";
                }
                else
                {
                    str = str + GATT_CharProperties.Broadcast.ToString();
                }
                str = str + str2;
            }
            num = 2;
            if ((properties & num) == num)
            {
                if (useShort)
                {
                    str = str + "Rd";
                }
                else
                {
                    str = str + GATT_CharProperties.Read.ToString();
                }
                str = str + str2;
            }
            num = 4;
            if ((properties & num) == num)
            {
                if (useShort)
                {
                    str = str + "Wwr";
                }
                else
                {
                    str = str + GATT_CharProperties.WriteWithoutResponse.ToString();
                }
                str = str + str2;
            }
            num = 8;
            if ((properties & num) == num)
            {
                if (useShort)
                {
                    str = str + "Wr";
                }
                else
                {
                    str = str + GATT_CharProperties.Write.ToString();
                }
                str = str + str2;
            }
            num = 0x10;
            if ((properties & num) == num)
            {
                if (useShort)
                {
                    str = str + "Nfy";
                }
                else
                {
                    str = str + GATT_CharProperties.Notify.ToString();
                }
                str = str + str2;
            }
            num = 0x20;
            if ((properties & num) == num)
            {
                if (useShort)
                {
                    str = str + "Ind";
                }
                else
                {
                    str = str + GATT_CharProperties.Indicate.ToString();
                }
                str = str + str2;
            }
            num = 0x40;
            if ((properties & num) == num)
            {
                if (useShort)
                {
                    str = str + "Asw";
                }
                else
                {
                    str = str + GATT_CharProperties.AuthenticatedSignedWrites.ToString();
                }
                str = str + str2;
            }
            num = 0x80;
            if ((properties & num) != num)
            {
                return str;
            }
            if (useShort)
            {
                return (str + "Exp");
            }
            return (str + GATT_CharProperties.ExtendedProperties.ToString());
        }

        public string GetGattPermissionsStr(byte permissions)
        {
            string str = "\n       \t\t  ";
            string str2 = string.Empty;
            if (permissions == 0)
            {
                return "Gatt Permissions Bit Mask Is Not Set";
            }
            byte num = 0;
            num = 1;
            if ((permissions & num) == num)
            {
                str2 = "GATT_PERMIT_READ";
            }
            num = 2;
            if ((permissions & num) == num)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + str;
                }
                str2 = str2 + "GATT_PERMIT_WRITE";
            }
            num = 4;
            if ((permissions & num) == num)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + str;
                }
                str2 = str2 + "GATT_PERMIT_AUTHEN_READ";
            }
            num = 8;
            if ((permissions & num) == num)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + str;
                }
                str2 = str2 + "GATT_PERMIT_AUTHEN_WRITE";
            }
            num = 0x10;
            if ((permissions & num) == num)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + str;
                }
                str2 = str2 + "GATT_PERMIT_AUTHOR_READ";
            }
            num = 0x20;
            if ((permissions & num) == num)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str2 = str2 + str;
                }
                str2 = str2 + "GATT_PERMIT_AUTHOR_WRITE";
            }
            if (string.IsNullOrEmpty(str2))
            {
                str2 = "Unknown Gatt Permissions";
            }
            return str2;
        }

        public string GetGattServiceUUIDStr(ushort serviceUUID)
        {
            switch (serviceUUID)
            {
                case 0x2800:
                    return "PrimaryService";

                case 0x2801:
                    return "SecondaryService";
            }
            return "Unknown Gatt Service UUID";
        }

        public string GetHciExtClkDivideOnHaltCtrlStr(byte control)
        {
            switch (control)
            {
                case 0:
                    return "HCI_EXT_DISABLE_CLK_DIVIDE_ON_HALT";

                case 1:
                    return "HCI_EXT_ENABLE_CLK_DIVIDE_ON_HALT";
            }
            return "Unknown Clk Divide On Halt Ctrl";
        }

        public string GetHciExtCwModeStr(byte cwMode)
        {
            switch (cwMode)
            {
                case 0:
                    return "HCI_EXT_TX_MODULATED_CARRIER";

                case 1:
                    return "HCI_EXT_TX_UNMODULATED_CARRIER";
            }
            return "Unknown Cw Mode";
        }

        public string GetHciExtDeclareNvUsageModeStr(byte control)
        {
            switch (control)
            {
                case 0:
                    return "NV Not In Use";

                case 1:
                    return "NV In Use";
            }
            return "Unknown Declare Nv Usage Proc Mode";
        }

        public string GetHciExtMapPmIoPortStr(byte data)
        {
            switch (data)
            {
                case 0:
                    return "PM IO Port 0";

                case 1:
                    return "PM IO Port 1";

                case 2:
                    return "PM IO Port 2";

                case 0xff:
                    return "PM IO Port None";
            }
            return "Unknown HciExtMapPmIoPort Data";
        }

        public string GetHciExtOnePktPerEvtCtrlStr(byte control)
        {
            switch (control)
            {
                case 0:
                    return "HCI_EXT_DISABLE_ONE_PKT_PER_EVT";

                case 1:
                    return "HCI_EXT_ENABLE_ONE_PKT_PER_EVT";
            }
            return "Unknown One Pkt Per Evt Ctrl";
        }

        public string GetHciExtPERTestCommandStr(byte data)
        {
            switch (data)
            {
                case 0:
                    return "Reset PER Counters";

                case 1:
                    return "Read PER Counters";
            }
            return "Unknown HciExtPERTestCommand Data";
        }

        public string GetHciExtRxGainStr(byte rxGain)
        {
            switch (rxGain)
            {
                case 0:
                    return "HCI_EXT_RX_GAIN_STD";

                case 1:
                    return "HCI_EXT_RX_GAIN_HIGH";
            }
            return "Unknown Rx Gain";
        }

        public string GetHciExtSetFastTxRespTimeCtrlStr(byte control)
        {
            switch (control)
            {
                case 0:
                    return "HCI_EXT_DISABLE_FAST_TX_RESP_TIME";

                case 1:
                    return "HCI_EXT_ENABLE_FAST_TX_RESP_TIME";
            }
            return "Unknown Set Fast Tx Resp Time Ctrl";
        }

        public string GetHciExtSetFreqTuneStr(byte data)
        {
            switch (data)
            {
                case 0:
                    return "Tune Frequency Down";

                case 1:
                    return "Tune Frequency Up";
            }
            return "Unknown HciExtSetFreqTune Data";
        }

        public string GetHCIExtStatusStr(byte status)
        {
            switch (status)
            {
                case 0:
                    return "Success";

                case 1:
                    return "Unknown HCI Command";

                case 2:
                    return "Unknown Connection Identifier";

                case 3:
                    return "Hardware Failure";

                case 4:
                    return "Page Timeout";

                case 5:
                    return "Authentication Failure";

                case 6:
                    return "PIN/Key Missing";

                case 7:
                    return "Memory Capacity Exceeded";

                case 8:
                    return "Connection Timeout";

                case 9:
                    return "Connection Limit Exceeded";

                case 10:
                    return "Synchronous Connection Limit To A Device Exceeded";

                case 11:
                    return "ACL Connection Already Exists";

                case 12:
                    return "Command Disallowed";

                case 13:
                    return "Connection Rejected Due To Limited Resources";

                case 14:
                    return "Connection Rejected Due To Security Reasons";

                case 15:
                    return "Connection Rejected Due To Unacceptable BD_ADDR";

                case 0x10:
                    return "Connection Accept Timeout Exceeded";

                case 0x11:
                    return "Unsupported Feature Or Parameter Value";

                case 0x12:
                    return "Invalid HCI Command Parameters";

                case 0x13:
                    return "Remote User Terminated Connection";

                case 20:
                    return "Remote Device Terminated Connection Due To Low Resources";

                case 0x15:
                    return "Remote Device Terminated Connection Due To Power Off";

                case 0x16:
                    return "Connection Terminated By Local Host";

                case 0x17:
                    return "Repeated Attempts";

                case 0x18:
                    return "Pairing Not Allowed";

                case 0x19:
                    return "Unknown LMP PDU";

                case 0x1a:
                    return "Unsupported Remote or LMP Feature";

                case 0x1b:
                    return "SCO Offset Rejected";

                case 0x1c:
                    return "SCO Interval Rejected";

                case 0x1d:
                    return "SCO Air Mode Rejected";

                case 30:
                    return "Invalid LMP Parameters";

                case 0x1f:
                    return "Unspecified Error";

                case 0x20:
                    return "Unsupported LMP Parameter Value";

                case 0x21:
                    return "Role Change Not Allowed";

                case 0x22:
                    return "LMP/LL Response Timeout";

                case 0x23:
                    return "LMP Error Transaction Collision";

                case 0x24:
                    return "LMP PDU Not Allowed";

                case 0x25:
                    return "Encryption Mode Not Acceptable";

                case 0x26:
                    return "Link Key Can Not be Changed";

                case 0x27:
                    return "Requested QoS Not Supported";

                case 40:
                    return "Instant Passed";

                case 0x29:
                    return "Pairing With Unit Key Not Supported";

                case 0x2a:
                    return "Different Transaction Collision";

                case 0x2b:
                    return "Reserved";

                case 0x2c:
                    return "QoS Unacceptable Parameter";

                case 0x2d:
                    return "QoS Rejected";

                case 0x2e:
                    return "Channel Assessment Not Supported";

                case 0x2f:
                    return "Insufficient Security";

                case 0x30:
                    return "Parameter Out Of Mandatory Range";

                case 0x31:
                    return "Reserved";

                case 50:
                    return "Role Switch Pending";

                case 0x33:
                    return "Reserved";

                case 0x34:
                    return "Reserved Slot Violation";

                case 0x35:
                    return "Role Switch Failed";

                case 0x36:
                    return "Extended Inquiry Response Too Large";

                case 0x37:
                    return "Simple Pairing Not Supported By Host";

                case 0x38:
                    return "Host Busy - Pairing";

                case 0x39:
                    return "Connection Rejected Due To No Suitable Channel Found";

                case 0x3a:
                    return "Controller Busy";

                case 0x3b:
                    return "Unacceptable Connection Interval";

                case 60:
                    return "Directed Advertising Timeout";

                case 0x3d:
                    return "Connection Terminated Due To MIC Failure";

                case 0x3e:
                    return "Connection Failed To Be Established";

                case 0x3f:
                    return "MAC Connection Failed";
            }
            return "Unknown HCI EXT Status";
        }

        public string GetHciExtTxPowerStr(byte txPower)
        {
            switch (txPower)
            {
                case 0:
                    return "HCI_EXT_TX_POWER_MINUS_23_DBM";

                case 1:
                    return "HCI_EXT_TX_POWER_MINUS_6_DBM";

                case 2:
                    return "HCI_EXT_TX_POWER_0_DBM";

                case 3:
                    return "HCI_EXT_TX_POWER_4_DBM";
            }
            return "Unknown Tx Power";
        }

        public string GetHciReqOpCodeStr(byte data)
        {
            switch (data)
            {
                case 1:
                    return "ATT_ErrorRsp";

                case 2:
                    return "ATT_ExchangeMTUReq";

                case 3:
                    return "ATT_ExchangeMTURsp";

                case 4:
                    return "ATT_FindInfoReq";

                case 5:
                    return "ATT_FindInfoRsp";

                case 6:
                    return "ATT_FindByTypeValueReq";

                case 7:
                    return "ATT_FindByTypeValueRsp";

                case 8:
                    return "ATT_ReadByTypeReq";

                case 9:
                    return "ATT_ReadByTypeRsp";

                case 10:
                    return "ATT_ReadReq";

                case 11:
                    return "ATT_ReadRsp";

                case 12:
                    return "ATT_ReadBlobReq";

                case 13:
                    return "ATT_ReadBlobRsp";

                case 14:
                    return "ATT_ReadMultiReq";

                case 15:
                    return "ATT_ReadMultiRsp";

                case 0x10:
                    return "ATT_ReadByGrpTypeReq";

                case 0x11:
                    return "ATT_ReadByGrpTypeRsp";

                case 0x12:
                    return "ATT_WriteReq";

                case 0x13:
                    return "ATT_WriteRsp";

                case 0x16:
                    return "ATT_PrepareWriteReq";

                case 0x17:
                    return "ATT_PrepareWriteRsp";

                case 0x18:
                    return "ATT_ExecuteWriteReq";

                case 0x19:
                    return "ATT_ExecuteWriteRsp";

                case 0x1b:
                    return "ATT_HandleValueNotification";

                case 0x1d:
                    return "ATT_HandleValueIndication";

                case 30:
                    return "ATT_HandleValueConfirmation";
            }
            return "Unknown HCIReqOpcode Data";
        }

        public string GetL2CapConnParamUpdateResultStr(ushort updateResult)
        {
            switch (updateResult)
            {
                case 0:
                    return "CONN_PARAMS_ACCEPTED";

                case 1:
                    return "CONN_PARAMS_REJECTED";
            }
            return "Unknown L2Cap Conn Param Update Result";
        }

        public string GetL2CapInfoTypesStr(ushort infoTypes)
        {
            switch (infoTypes)
            {
                case 1:
                    return "CONNECTIONLESS_MTU";

                case 2:
                    return "EXTENDED_FEATURES";

                case 3:
                    return "FIXED_CHANNELS";
            }
            return "Unknown L2Cap Info Types";
        }

        public string GetL2CapRejectReasonsStr(ushort rejectReason)
        {
            switch (rejectReason)
            {
                case 0:
                    return "Command not understood";

                case 1:
                    return "Signaling MTU exceeded ";

                case 2:
                    return "Invalid CID in request";
            }
            return "Unknown L2Cap Reject Reason";
        }

        public string GetLEAddressTypeStr(byte dataFlag)
        {
            switch (dataFlag)
            {
                case 0:
                    return "Public Device Address";

                case 1:
                    return "Random Device Address";
            }
            return "Unknown LE Address Type";
        }

        public string GetOpCodeName(ushort opCode)
        {
            HCICmds cmds = new HCICmds();
            for (uint i = 0; i < (cmds.OpCodeLookupTable.Length / 2); i++)
            {
                if (cmds.OpCodeLookupTable[i, 0] == string.Format("0x{0:X4}", opCode))
                {
                    return cmds.OpCodeLookupTable[i, 1];
                }
            }
            return "Unknown Op Code";
        }

        public string GetPacketTypeStr(byte packetType)
        {
            switch (packetType)
            {
                case 1:
                    return "Command";

                case 2:
                    return "Async Data";

                case 3:
                    return "Sync Data";

                case 4:
                    return "Event";
            }
            return "Unknown Packet Type";
        }

        public string GetShortErrorStatusStr(byte errorStatus)
        {
            switch (errorStatus)
            {
                case 1:
                    return "INVALID_HANDLE";

                case 2:
                    return "READ_NOT_PERMITTED";

                case 3:
                    return "WRITE_NOT_PERMITTED";

                case 4:
                    return "INVALID_PDU";

                case 5:
                    return "INSUFFICIENT_AUTHEN";

                case 6:
                    return "UNSUPPORTED_REQ";

                case 7:
                    return "INVALID_OFFSET";

                case 8:
                    return "INSUFFICIENT_AUTHOR";

                case 9:
                    return "PREPARE_QUEUE_FULL";

                case 10:
                    return "ATTR_NOT_FOUND";

                case 11:
                    return "ATTR_NOT_LONG";

                case 12:
                    return "INSUFFICIENT_KEY_SIZE";

                case 13:
                    return "INVALID_SIZE";

                case 14:
                    return "UNLIKELY_ERROR";

                case 15:
                    return "INSUFFICIENT_ENCRYPTION";

                case 0x10:
                    return "UNSUPPORTED_GRP_TYPE";

                case 0x11:
                    return "INSUFFICIENT_RESOURCES";

                case 0x80:
                    return "INVALID_VALUE";
            }
            return "Unknown Error Status";
        }

        public string GetSigAuthStr(byte sigAuth)
        {
            switch (sigAuth)
            {
                case 0:
                    return "The Authentication Signature is not included with the Write PDU.";

                case 1:
                    return "The included Authentication Signature is valid.";

                case 2:
                    return "The included Authentication Signature is not valid.";
            }
            return "Unknown Signature Authorization";
        }

        public string GetStatusStr(byte status)
        {
            switch (status)
            {
                case 0:
                    return "Success";

                case 1:
                    return "Failure";

                case 2:
                    return "Invalid Parameter";

                case 3:
                    return "Invalid Task";

                case 4:
                    return "Msg Buffer Not Available";

                case 5:
                    return "Invalid Msg Pointer";

                case 6:
                    return "Invalid Event Id";

                case 7:
                    return "Invalid Interupt Id";

                case 8:
                    return "No Timer Avail";

                case 9:
                    return "NV Item UnInit";

                case 10:
                    return "NV Op Failed";

                case 11:
                    return "Invalid Mem Size";

                case 12:
                    return "Error Command Disallowed";

                case 0x10:
                    return "Not Ready To Perform Task";

                case 0x11:
                    return "Already Performing That Task";

                case 0x12:
                    return "Not Setup Properly To Perform That Task";

                case 0x13:
                    return "Memory Allocation Error Occurred";

                case 20:
                    return "Can't Perform Function When Not In A Connection";

                case 0x15:
                    return "There Are No Resources Available";

                case 0x16:
                    return "Waiting";

                case 0x17:
                    return "Timed Out Performing Function";

                case 0x18:
                    return "A Parameter Is Out Of Range";

                case 0x19:
                    return "The Link Is Already Encrypted";

                case 0x1a:
                    return "The Procedure Is Completed";

                case 0x30:
                    return "The User Canceled The Task";

                case 0x31:
                    return "The Connection Was Not Accepted";

                case 50:
                    return "The Bound Information Was Rejected.";

                case 0x40:
                    return "The Attribute PDU Is Invalid";

                case 0x41:
                    return "The Attribute Has Insufficient Authentication";

                case 0x42:
                    return "The Attribute Has Insufficient Encryption";

                case 0x43:
                    return "The Attribute Has Insufficient Encryption Key Size";

                case 0xff:
                    return "Task ID Isn't Setup Properly";
            }
            return "Unknown Status";
        }

        public string GetUtilResetTypeStr(byte resetType)
        {
            switch (resetType)
            {
                case 0:
                    return "Hard Reset";

                case 1:
                    return "Soft Reset";
            }
            return "Unknown Util Reset Type";
        }

        public int GetUuidLength(byte format, ref bool dataErr)
        {
            dataErr = false;
            switch (format)
            {
                case 1:
                    return 2;

                case 2:
                    return 0x10;
            }
            string msg = string.Format("Can Not Convert The UUID Format. [{0}]\n", (int) format);
            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
            dataErr = true;
            return 0;
        }

        public string HexStr2UserDefinedStr(string msg, SharedAppObjs.StringType strType)
        {
            string str = string.Empty;
            try
            {
                if (msg == null)
                {
                    return "";
                }
                uint index = 0;
                msg = msg.Trim();
                string[] strArray = msg.Split(new char[] { ' ', ':' });
                uint num2 = 0;
                if (strType == SharedAppObjs.StringType.HEX)
                {
                    str = msg;
                }
                else if (strType == SharedAppObjs.StringType.DEC)
                {
                    if (strArray.Length <= 4)
                    {
                        for (index = 0; index < strArray.Length; index++)
                        {
                            try
                            {
                                num2 += (uint) (Convert.ToByte(strArray[index], 0x10) << ((byte) (8 * index)));
                            }
                            catch (Exception exception)
                            {
                                string str2 = string.Format("Cannot Convert The Value Into Decimal.\n\n{0}\n", exception.Message);
                                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str2);
                            }
                        }
                        str = str + string.Format("{0:D} ", num2);
                    }
                    else
                    {
                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Cannot Convert The Value Into Decimal.\n");
                    }
                }
                else if (strType == SharedAppObjs.StringType.ASCII)
                {
                    for (index = 0; index < strArray.Length; index++)
                    {
                        try
                        {
                            char ch = Convert.ToChar(Convert.ToByte(strArray[index], 0x10));
                            str = str + string.Format("{0:S} ", ch.ToString());
                        }
                        catch (Exception exception2)
                        {
                            string str3 = string.Format("Can Not Convert The Value Into ASCII.\n\n{0}\n", exception2.Message);
                            this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str3);
                        }
                    }
                }
                else
                {
                    str = msg;
                }
                str = str.Trim();
            }
            catch
            {
                return str;
            }
            return str;
        }

        public bool LoadMsgHeader(ref byte[] data, ref int index, byte packetType, ushort opCode, byte dataLength)
        {
            bool flag = true;
            try
            {
                data[index++] = packetType;
                bool dataErr = false;
                this.dataUtils.Load16Bits(ref data, ref index, opCode, ref dataErr, false);
                data[index++] = dataLength;
                if (dataErr)
                {
                    flag = false;
                }
            }
            catch (Exception exception)
            {
                string msg = string.Format("Load Msg Header Failed\nMessage Data Transfer Issue.\n\n{0}\n", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                flag = false;
            }
            return flag;
        }

        public byte[] String2BDA_LSBMSB(string bdaStr)
        {
            byte[] buffer = new byte[6];
            try
            {
                string[] strArray = bdaStr.Split(new char[] { ' ', ':' });
                if (strArray.Length == 6)
                {
                    for (uint i = 0; i < 6; i++)
                    {
                        try
                        {
                            buffer[(int) ((IntPtr) (5 - i))] = Convert.ToByte(strArray[i], 0x10);
                        }
                        catch
                        {
                            return null;
                        }
                    }
                    return buffer;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public byte[] String2Bytes_LSBMSB(string str, byte radix)
        {
            byte[] buffer;
            uint index = 0;
            try
            {
                if (radix != 0xff)
                {
                    string[] strArray = str.Split(new char[] { ' ', ':' });
                    int num2 = 0;
                    for (index = 0; index < strArray.Length; index++)
                    {
                        if (strArray[index].Length > 0)
                        {
                            num2++;
                        }
                    }
                    buffer = new byte[num2];
                    int num3 = 0;
                    for (index = 0; index < strArray.Length; index++)
                    {
                        try
                        {
                            if (strArray[index].Length > 0)
                            {
                                buffer[num3++] = Convert.ToByte(strArray[index], radix);
                            }
                        }
                        catch
                        {
                            return null;
                        }
                    }
                    return buffer;
                }
                char[] chArray = str.ToCharArray();
                buffer = new byte[chArray.Length];
                for (index = 0; index < chArray.Length; index++)
                {
                    buffer[index] = (byte) chArray[index];
                }
            }
            catch
            {
                return null;
            }
            return buffer;
        }

        public ushort[] String2UInt16_LSBMSB(string str, byte radix)
        {
            ushort[] numArray;
			try
			{
				uint index = 0;
				if (radix != 0xff)
				{
					string[] strArray = str.Split(new char[] { ' ', ':', ';' });
					numArray = new ushort[strArray.Length];
					for (index = 0; index < strArray.Length; index++)
					{
						try
						{
							if (strArray[index] != string.Empty)
							{
								numArray[index] = Convert.ToUInt16(strArray[index], radix);
							}
							else
							{
								return null;
							}
						}
						catch
						{
							return null;
						}
					}
					return numArray;
				}
			}
			catch { }
			return null;
		}

        public byte UnloadAttMsgHeader(ref byte[] data, ref int index, ref string msg, ref bool dataErr)
        {
            ushort num = 0;
            byte num2 = 0;
            try
            {
                num = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                if (msg != null)
                {
                    msg = msg + string.Format(" ConnHandle\t: 0x{0:X4} ({1:D})\n", num, num);
                }
                num2 = this.dataUtils.Unload8Bits(data, ref index, ref dataErr);
                if (msg != null)
                {
                    msg = msg + string.Format(" PduLen\t\t: 0x{0:X2} ({1:D})\n", num2, num2);
                }
            }
            catch (Exception exception)
            {
                string str = string.Format("UnloadAttMsgHeader Failed\nMessage Data Transfer Issue.\n\n{0}\n", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str);
                dataErr = true;
            }
            return num2;
        }

        public string UnloadColonData(byte[] data, bool limitLen)
        {
            bool dataErr = false;
            int index = 0;
            return this.UnloadColonData(data, ref index, data.Length, ref dataErr, limitLen);
        }

        public string UnloadColonData(byte[] data, ref int index, int numBytes, ref bool dataErr)
        {
            return this.UnloadColonData(data, ref index, numBytes, ref dataErr, true);
        }

        public string UnloadColonData(byte[] data, ref int index, int numBytes, ref bool dataErr, bool limitLen)
        {
            string msg = string.Empty;
            byte bits = 0;
            dataErr = false;
            try
            {
                for (int i = 0; (i < numBytes) && !dataErr; i++)
                {
                    this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                    if (i != (numBytes - 1))
                    {
                        msg = msg + string.Format("{0:X2}:", bits);
                    }
                    else
                    {
                        msg = msg + string.Format("{0:X2}", bits);
                    }
                    if (limitLen && (i != (numBytes - 1)))
                    {
                        this.CheckLineLength(ref msg, (uint) i, true);
                    }
                }
            }
            catch (Exception exception)
            {
                string str2 = string.Format("Unload Colon Data Failed\nMessage Data Transfer Issue.\n\n{0}\n", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str2);
                dataErr = true;
            }
            return msg;
        }

        public string UnloadDeviceAddr(byte[] data, ref byte[] addr, ref int index, bool direction, ref bool dataErr)
        {
            string str = string.Empty;
            byte bits = 0;
            dataErr = false;
            try
            {
                for (int i = 0; (i < 6) && !dataErr; i++)
                {
                    this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                    if (dataErr)
                    {
                        return str;
                    }
                    addr[i] = bits;
                    if (direction)
                    {
                        if (i != 5)
                        {
                            str = str + string.Format("{0:X2}:", bits);
                        }
                        else
                        {
                            str = str + string.Format("{0:X2}", bits);
                        }
                    }
                    else if (i != 0)
                    {
                        str = string.Format("{0:X2}:", bits) + str;
                    }
                    else
                    {
                        str = string.Format("{0:X2}", bits) + str;
                    }
                }
            }
            catch (Exception exception)
            {
                string msg = string.Format("Unload Device Address Failed\nMessage Data Transfer Issue.\n\n{0}\n", exception.Message);
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                dataErr = true;
            }
            return str;
        }

        public string UnloadHandleHandleValueData(byte[] data, ref int index, int totalLength, int dataLength, ref bool dataErr)
        {
            List<HCIReplies.HandleHandleData> handleHandleData = new List<HCIReplies.HandleHandleData>();
            return this.UnloadHandleHandleValueData(data, ref index, totalLength, dataLength, ref dataErr, ref handleHandleData);
        }

        public string UnloadHandleHandleValueData(byte[] data, ref int index, int totalLength, int dataLength, ref bool dataErr, ref List<HCIReplies.HandleHandleData> handleHandleData)
        {
            string str = string.Empty;
            string msg = string.Empty;
            ushort num = 0xffff;
            ushort num2 = 0xffff;
            dataErr = false;
            int num3 = totalLength;
            byte bits = 0;
            if (dataLength != 0)
            {
                while (((num3 > 0) && !dataErr) && (num3 >= ((byte) dataLength)))
                {
                    try
                    {
                        HCIReplies.HandleHandleData item = new HCIReplies.HandleHandleData();
                        num = 0xffff;
                        num = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                        item.handle1 = num;
                        num2 = 0xffff;
                        num2 = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                        item.handle2 = num2;
                        int num5 = dataLength - 4;
                        item.data = new byte[num5];
                        for (int i = 0; (i < num5) && !dataErr; i++)
                        {
                            this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                            item.data[i] = bits;
                            if (i != (num5 - 1))
                            {
                                msg = msg + string.Format("{0:X2}:", bits);
                            }
                            else
                            {
                                msg = msg + string.Format("{0:X2}", bits);
                            }
                            this.CheckLineLength(ref msg, (uint) i, true);
                        }
                        handleHandleData.Add(item);
                    }
                    catch (Exception exception)
                    {
                        string str3 = string.Format("Unload Handle Value Data Failed\nMessage Data Transfer Issue.\n\n{0}\n", exception.Message);
                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str3);
                        dataErr = true;
                    }
                    str = (str + string.Format(" AttrHandle\t: 0x{0:X4}\n", num)) + string.Format(" EndGrpHandle\t: 0x{0:X4}\n", num2) + string.Format(" Value\t\t: {0:S}\n", msg);
                    msg = string.Empty;
                    num3 -= (byte) dataLength;
                }
            }
            return str;
        }

        public string UnloadHandleValueData(byte[] data, ref int index, int totalLength, int dataLength, ref bool dataErr)
        {
            string handleStr = string.Empty;
            string valueStr = string.Empty;
            List<HCIReplies.HandleData> handleData = new List<HCIReplies.HandleData>();
            return this.UnloadHandleValueData(data, ref index, totalLength, dataLength, ref handleStr, ref valueStr, ref dataErr, "Data", ref handleData);
        }

        public string UnloadHandleValueData(byte[] data, ref int index, int totalLength, int dataLength, ref bool dataErr, string strDataName)
        {
            string handleStr = string.Empty;
            string valueStr = string.Empty;
            return this.UnloadHandleValueData(data, ref index, totalLength, dataLength, ref handleStr, ref valueStr, ref dataErr, strDataName);
        }

        public string UnloadHandleValueData(byte[] data, ref int index, int totalLength, int dataLength, ref bool dataErr, string strDataName, ref List<HCIReplies.HandleData> handleData)
        {
            string handleStr = string.Empty;
            string valueStr = string.Empty;
            return this.UnloadHandleValueData(data, ref index, totalLength, dataLength, ref handleStr, ref valueStr, ref dataErr, strDataName, ref handleData);
        }

        public string UnloadHandleValueData(byte[] data, ref int index, int totalLength, int dataLength, ref string handleStr, ref string valueStr, ref bool dataErr, string strDataName)
        {
            List<HCIReplies.HandleData> handleData = new List<HCIReplies.HandleData>();
            return this.UnloadHandleValueData(data, ref index, totalLength, dataLength, ref handleStr, ref valueStr, ref dataErr, strDataName, ref handleData);
        }

        public string UnloadHandleValueData(byte[] data, ref int index, int totalLength, int dataLength, ref string handleStr, ref string valueStr, ref bool dataErr, string strDataName, ref List<HCIReplies.HandleData> handleData)
        {
            string str = string.Empty;
            string msg = string.Empty;
            valueStr = string.Empty;
            handleStr = string.Empty;
            ushort num = 0xffff;
            dataErr = false;
            int num2 = totalLength;
            byte bits = 0;
            if (dataLength != 0)
            {
                while (((num2 > 0) && !dataErr) && (num2 >= ((byte) dataLength)))
                {
                    try
                    {
                        HCIReplies.HandleData item = new HCIReplies.HandleData();
                        num = 0xffff;
                        num = this.dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
                        item.handle = num;
                        int num4 = dataLength - 2;
                        item.data = new byte[num4];
                        for (int i = 0; (i < num4) && !dataErr; i++)
                        {
                            this.dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
                            item.data[i] = bits;
                            valueStr = valueStr + string.Format("{0:X2} ", bits);
                            if (i != (num4 - 1))
                            {
                                msg = msg + string.Format("{0:X2}:", bits);
                            }
                            else
                            {
                                msg = msg + string.Format("{0:X2}", bits);
                            }
                            this.CheckLineLength(ref msg, (uint) i, true);
                        }
                        handleData.Add(item);
                    }
                    catch (Exception exception)
                    {
                        string str3 = string.Format("Unload Handle Value Data Failed\nMessage Data Transfer Issue.\n\n{0}\n", exception.Message);
                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str3);
                        dataErr = true;
                    }
                    handleStr = handleStr + string.Format("0x{0:X4} ", num);
                    str = str + string.Format(" Handle\t\t: 0x{0:X4}\n", num) + string.Format(" {0}\t\t: {1:S}\n", strDataName, msg);
                    msg = string.Empty;
                    num2 -= (byte) dataLength;
                }
            }
            return str;
        }

        private enum UuidFormat
        {
            SixteenBytes = 2,
            TwoBytes = 1
        }
    }
}


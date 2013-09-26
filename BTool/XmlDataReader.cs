﻿namespace BTool
{
    using System;
    using System.Drawing;
    using System.Xml;

    public class XmlDataReader
    {
        public const string moduleName = "XmlDataReader";
        private MsgBox msgBox = new MsgBox();
        private XmlDataReaderUtils xmlDataReaderUtils = new XmlDataReaderUtils();
        private const string xmlFormatVersion = "00.00.04";

        public bool Read(string xmlFileName)
        {
            bool flag = true;
            try
            {
                int num3;
                XmlDocument document = new XmlDocument();
                document.Load(xmlFileName);
                XmlNode documentElement = document.DocumentElement;
                XmlNodeList elementsByTagName = document.GetElementsByTagName("version");
                string tagName = "Version Number";
                if (elementsByTagName.Count == 0)
                {
                    return this.xmlDataReaderUtils.NoTagValueFound(tagName, xmlFileName, "XmlDataReader");
                }
                string fileVersion = elementsByTagName[0].InnerText.Trim();
                if ("00.00.04" != fileVersion)
                {
                    return this.xmlDataReaderUtils.FileVersionError("00.00.04", fileVersion, xmlFileName, "XmlDataReader");
                }
                XmlNodeList list3 = document.GetElementsByTagName("unknown_indl");
                tagName = "Unknown Indent Level";
                if (list3.Count == 0)
                {
                    return this.xmlDataReaderUtils.NoTagValueFound(tagName, xmlFileName, "XmlDataReader");
                }
                try
                {
                    AttrData.unknownIndentLevel = Convert.ToByte(list3[0].InnerText.Trim());
                }
                catch (Exception exception)
                {
                    flag = this.xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, list3[0].InnerText.Trim(), ((byte) 4).ToString(), exception.Message, "XmlDataReader");
                    AttrData.unknownIndentLevel = 4;
                }
                XmlNodeList list4 = document.GetElementsByTagName("key_width");
                tagName = "Key Width";
                if (list4.Count == 0)
                {
                    return this.xmlDataReaderUtils.NoTagValueFound(tagName, xmlFileName, "XmlDataReader");
                }
                try
                {
                    AttrData.columns.keyWidth = Convert.ToByte(list4[0].InnerText.Trim());
                }
                catch (Exception exception2)
                {
                    flag = this.xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, list4[0].InnerText.Trim(), 70.ToString(), exception2.Message, "XmlDataReader");
                    AttrData.columns.keyWidth = 70;
                }
                XmlNodeList list5 = document.GetElementsByTagName("con_hnd_width");
                tagName = "Connection Handle Width";
                if (list5.Count == 0)
                {
                    return this.xmlDataReaderUtils.NoTagValueFound(tagName, xmlFileName, "XmlDataReader");
                }
                try
                {
                    AttrData.columns.connHandleWidth = Convert.ToByte(list5[0].InnerText.Trim());
                }
                catch (Exception exception3)
                {
                    flag = this.xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, list5[0].InnerText.Trim(), 0x37.ToString(), exception3.Message, "XmlDataReader");
                    AttrData.columns.connHandleWidth = 0x37;
                }
                XmlNodeList list6 = document.GetElementsByTagName("handle_width");
                tagName = "Handle Width";
                if (list6.Count == 0)
                {
                    return this.xmlDataReaderUtils.NoTagValueFound(tagName, xmlFileName, "XmlDataReader");
                }
                try
                {
                    AttrData.columns.handleWidth = Convert.ToByte(list6[0].InnerText.Trim());
                }
                catch (Exception exception4)
                {
                    flag = this.xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, list6[0].InnerText.Trim(), 0x37.ToString(), exception4.Message, "XmlDataReader");
                    AttrData.columns.handleWidth = 0x37;
                }
                XmlNodeList list7 = document.GetElementsByTagName("uuid_width");
                tagName = "UUID Width";
                if (list7.Count == 0)
                {
                    return this.xmlDataReaderUtils.NoTagValueFound(tagName, xmlFileName, "XmlDataReader");
                }
                try
                {
                    AttrData.columns.uuidWidth = Convert.ToByte(list7[0].InnerText.Trim());
                }
                catch (Exception exception5)
                {
                    flag = this.xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, list7[0].InnerText.Trim(), 0x37.ToString(), exception5.Message, "XmlDataReader");
                    AttrData.columns.uuidWidth = 0x37;
                }
                XmlNodeList list8 = document.GetElementsByTagName("uuid_desc_width");
                tagName = "UUID Desc Width";
                if (list8.Count == 0)
                {
                    return this.xmlDataReaderUtils.NoTagValueFound(tagName, xmlFileName, "XmlDataReader");
                }
                try
                {
                    AttrData.columns.uuidDescWidth = Convert.ToByte(list8[0].InnerText.Trim());
                }
                catch (Exception exception6)
                {
                    flag = this.xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, list8[0].InnerText.Trim(), 0xe1.ToString(), exception6.Message, "XmlDataReader");
                    AttrData.columns.uuidDescWidth = 0xe1;
                }
                XmlNodeList list9 = document.GetElementsByTagName("value_width");
                tagName = "Value Width";
                if (list9.Count == 0)
                {
                    return this.xmlDataReaderUtils.NoTagValueFound(tagName, xmlFileName, "XmlDataReader");
                }
                try
                {
                    AttrData.columns.valueWidth = Convert.ToByte(list9[0].InnerText.Trim());
                }
                catch (Exception exception7)
                {
                    flag = this.xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, list9[0].InnerText.Trim(), 150.ToString(), exception7.Message, "XmlDataReader");
                    AttrData.columns.valueWidth = 150;
                }
                XmlNodeList list10 = document.GetElementsByTagName("value_desc_width");
                tagName = "Value Desc Width";
                if (list10.Count == 0)
                {
                    return this.xmlDataReaderUtils.NoTagValueFound(tagName, xmlFileName, "XmlDataReader");
                }
                try
                {
                    AttrData.columns.valueDescWidth = Convert.ToByte(list10[0].InnerText.Trim());
                }
                catch (Exception exception8)
                {
                    flag = this.xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, list10[0].InnerText.Trim(), 0xaf.ToString(), exception8.Message, "XmlDataReader");
                    AttrData.columns.valueDescWidth = 0xaf;
                }
                XmlNodeList list11 = document.GetElementsByTagName("properties_width");
                tagName = "Properties Width";
                if (list11.Count == 0)
                {
                    return this.xmlDataReaderUtils.NoTagValueFound(tagName, xmlFileName, "XmlDataReader");
                }
                try
                {
                    AttrData.columns.propertiesWidth = Convert.ToByte(list11[0].InnerText.Trim());
                }
                catch (Exception exception9)
                {
                    num3 = 0x90;
                    flag = this.xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, list11[0].InnerText.Trim(), num3.ToString(), exception9.Message, "XmlDataReader");
                    AttrData.columns.propertiesWidth = 0x90;
                }
                XmlNodeList list12 = document.GetElementsByTagName("max_packet_size");
                tagName = "Max Packet Size";
                if (list12.Count == 0)
                {
                    return this.xmlDataReaderUtils.NoTagValueFound(tagName, xmlFileName, "XmlDataReader");
                }
                try
                {
                    AttrData.writeLimits.maxPacketSize = Convert.ToInt16(list12[0].InnerText.Trim());
                    if ((AttrData.writeLimits.maxPacketSize < 0x10) || (AttrData.writeLimits.maxPacketSize > 0x200))
                    {
                        num3 = 0x7f;
                        flag = this.xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, list12[0].InnerText.Trim(), num3.ToString(), null, "XmlDataReader");
                        AttrData.writeLimits.maxPacketSize = 0x7f;
                    }
                }
                catch (Exception exception10)
                {
                    flag = this.xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, list12[0].InnerText.Trim(), AttrData.writeLimits.maxPacketSize.ToString(), exception10.Message, "XmlDataReader");
                    AttrData.writeLimits.maxPacketSize = 0x7f;
                }
                XmlNodeList list13 = document.GetElementsByTagName("max_num_prepare_writes");
                tagName = "Max Num Prepare Writes";
                if (list13.Count == 0)
                {
                    return this.xmlDataReaderUtils.NoTagValueFound(tagName, xmlFileName, "XmlDataReader");
                }
                try
                {
                    AttrData.writeLimits.maxNumPreparedWrites = Convert.ToInt16(list13[0].InnerText.Trim());
                    if ((AttrData.writeLimits.maxNumPreparedWrites < 1) || (AttrData.writeLimits.maxNumPreparedWrites > 0x1c))
                    {
                        num3 = 5;
                        flag = this.xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, list13[0].InnerText.Trim(), num3.ToString(), null, "XmlDataReader");
                        AttrData.writeLimits.maxNumPreparedWrites = 5;
                    }
                }
                catch (Exception exception11)
                {
                    flag = this.xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, list13[0].InnerText.Trim(), AttrData.writeLimits.maxNumPreparedWrites.ToString(), exception11.Message, "XmlDataReader");
                    AttrData.writeLimits.maxNumPreparedWrites = 5;
                }
                foreach (XmlNode node2 in documentElement.SelectNodes("descendant::data_set"))
                {
                    XmlNodeList list14 = node2.SelectNodes("data_set_name");
                    XmlNodeList list15 = node2.SelectNodes("uuid");
                    XmlNodeList list16 = node2.SelectNodes("indl");
                    XmlNodeList list17 = node2.SelectNodes("vdsp");
                    XmlNodeList list18 = node2.SelectNodes("vedt");
                    XmlNodeList list19 = node2.SelectNodes("udsc");
                    XmlNodeList list20 = node2.SelectNodes("vdsc");
                    XmlNodeList list21 = node2.SelectNodes("fore");
                    XmlNodeList list22 = node2.SelectNodes("back");
                    tagName = "Unknown";
                    try
                    {
                        for (int i = 0; i < list15.Count; i++)
                        {
                            UuidData data = new UuidData();
                            tagName = "Key";
                            string str3 = list15[i].InnerText.Replace("0x", "").Trim();
                            string key = str3;
                            tagName = "Uuid";
                            data.uuid = str3;
                            tagName = "Indent Level";
                            try
                            {
                                data.indentLevel = Convert.ToByte(list16[i].InnerText.Trim());
                            }
                            catch (Exception exception12)
                            {
                                flag = this.xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, list16[i].InnerText.Trim(), 0.ToString(), exception12.Message, "XmlDataReader");
                                data.indentLevel = 0;
                            }
                            tagName = "Value Display";
                            string str10 = list17[i].InnerText.Trim();
                            if (str10 == null)
                            {
                                goto Label_0A12;
                            }
                            if (!(str10 == "Hex"))
                            {
                                if (str10 == "Dec")
                                {
                                    goto Label_09FE;
                                }
                                if (str10 == "Asc")
                                {
                                    goto Label_0A08;
                                }
                                goto Label_0A12;
                            }
                            data.valueDsp = ValueDisplay.Hex;
                            goto Label_0A4D;
                        Label_09FE:
                            data.valueDsp = ValueDisplay.Dec;
                            goto Label_0A4D;
                        Label_0A08:
                            data.valueDsp = ValueDisplay.Ascii;
                            goto Label_0A4D;
                        Label_0A12:
                            flag = this.xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, list17[i].InnerText.Trim(), ValueDisplay.Hex.ToString(), null, "XmlDataReader");
                            data.valueDsp = ValueDisplay.Hex;
                        Label_0A4D:
                            tagName = "Value Edit";
                            str10 = list18[i].InnerText.Trim();
                            if (str10 == null)
                            {
                                goto Label_0AA2;
                            }
                            if (!(str10 == "Edit"))
                            {
                                if (str10 == "Read")
                                {
                                    goto Label_0A98;
                                }
                                goto Label_0AA2;
                            }
                            data.valueEdit = ValueEdit.Editable;
                            goto Label_0ADD;
                        Label_0A98:
                            data.valueEdit = ValueEdit.ReadOnly;
                            goto Label_0ADD;
                        Label_0AA2:
                            flag = this.xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, list18[i].InnerText.Trim(), ValueEdit.Editable.ToString(), null, "XmlDataReader");
                            data.valueEdit = ValueEdit.Editable;
                        Label_0ADD:
                            tagName = "Uuid Description";
                            data.uuidDesc = list19[i].InnerText.Trim();
                            tagName = "Value Description";
                            data.valueDesc = list20[i].InnerText.Trim();
                            tagName = "Foreground Color";
                            Color color = Color.FromArgb(0);
                            color = Color.FromName(list21[i].InnerText.Trim());
                            if (color.ToKnownColor() == ((KnownColor) 0))
                            {
                                flag = this.xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, list21[i].InnerText.Trim(), AttrData.defaultForeground.ToString(), null, "XmlDataReader");
                                data.foreColor = AttrData.defaultForeground;
                            }
                            else
                            {
                                data.foreColor = color;
                            }
                            tagName = "Background Color";
                            color = Color.FromName(list22[i].InnerText.Trim());
                            if (color.ToKnownColor() == ((KnownColor) 0))
                            {
                                flag = this.xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, list22[i].InnerText.Trim(), AttrData.defaultBackground.ToString(), null, "XmlDataReader");
                                data.backColor = AttrData.defaultBackground;
                            }
                            else
                            {
                                data.backColor = color;
                            }
                            tagName = "Store Data Item";
                            try
                            {
                                data.dataSetName = list14.Item(0).FirstChild.Value;
                            }
                            catch
                            {
                                data.dataSetName = "Unknown Data Set Name";
                            }
                            AttrUuid.uuidDictAccess.WaitOne();
                            try
                            {
                                AttrUuid.uuidDict.Add(key, data);
                            }
                            catch (Exception exception13)
                            {
                                string msg = "XML File Data Error\n" + exception13.Message + "\nUUID = 0x" + key + "\nData Set Name = " + data.dataSetName + "\nTag Field = " + tagName + "\n" + xmlFileName + "\nXmlDataReader\n";
                                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
                                flag = false;
                            }
                            AttrUuid.uuidDictAccess.ReleaseMutex();
                            if (!flag)
                            {
                                goto Label_0D79;
                            }
                        }
                    }
                    catch (Exception exception14)
                    {
                        string str8 = "Error Reading XML File\n" + exception14.Message + "\nTag Field = " + tagName + "\n" + xmlFileName + "\nXmlDataReader\n";
                        this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str8);
                        flag = false;
                    }
                Label_0D79:
                    if (!flag)
                    {
                        return flag;
                    }
                }
                return flag;
            }
            catch (Exception exception15)
            {
                string str9 = "Error Reading XML File\n" + exception15.Message + "\n" + xmlFileName + "\nXmlDataReader\n";
                this.msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, str9);
                flag = false;
            }
            return flag;
        }
    }
}

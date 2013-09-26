﻿namespace BTool
{
    using System;
    using System.Collections;
    using System.Threading;

    internal class CommParser
    {
        private Queue _dataBuffer = new Queue();
        private Mutex bufferMutex = new Mutex();
        private ParserStateEnum ParserState = ParserStateEnum.packet_type_token;

        public void DeQueueData(int length)
        {
            this.bufferMutex.WaitOne();
            if (length >= this._dataBuffer.Count)
            {
                this._dataBuffer.Clear();
            }
            else
            {
                for (uint i = 0; i < length; i++)
                {
                    this._dataBuffer.Dequeue();
                }
            }
            this.bufferMutex.ReleaseMutex();
        }

        public void EnQueueData(byte[] data)
        {
            this.bufferMutex.WaitOne();
            foreach (byte num in data)
            {
                this._dataBuffer.Enqueue(num);
            }
            this.bufferMutex.ReleaseMutex();
        }

        public int GetDataSize()
        {
            return this._dataBuffer.Count;
        }

        public bool ParseData(ref byte type, ref ushort opCode, ref ushort eventOpCode, ref byte length, ref byte[] data)
        {
            bool flag = false;
            this.bufferMutex.WaitOne();
            if (this._dataBuffer.Count != 0)
            {
                switch (this.ParserState)
                {
                    case ParserStateEnum.packet_type_token:
                        if (((byte) this._dataBuffer.Peek()) != 4)
                        {
                            this._dataBuffer.Dequeue();
                            break;
                        }
                        type = (byte) this._dataBuffer.Dequeue();
                        this.ParserState = ParserStateEnum.event_code_token;
                        break;

                    case ParserStateEnum.event_code_token:
                        opCode = (byte) this._dataBuffer.Dequeue();
                        this.ParserState = ParserStateEnum.length_token;
                        break;

                    case ParserStateEnum.eop0_token:
                        eventOpCode = (byte) this._dataBuffer.Dequeue();
                        this.ParserState = ParserStateEnum.eop1_token;
                        break;

                    case ParserStateEnum.eop1_token:
                        eventOpCode = (ushort) (eventOpCode | ((ushort) (((byte) this._dataBuffer.Dequeue()) << 8)));
                        this.ParserState = ParserStateEnum.data_token;
                        break;

                    case ParserStateEnum.length_token:
                        length = (byte) this._dataBuffer.Dequeue();
                        if ((opCode == 0x13) || (opCode == 0xff))
                        {
                            this.ParserState = ParserStateEnum.eop0_token;
                        }
                        else
                        {
                            this.ParserState = ParserStateEnum.data_token;
                        }
                        break;

                    case ParserStateEnum.data_token:
                        if (type == 4)
                        {
                            int num = 0;
                            if ((opCode == 0x13) || (opCode == 0xff))
                            {
                                num = length - 2;
                            }
                            else
                            {
                                num = length;
                            }
                            if (this._dataBuffer.Count >= num)
                            {
                                data = new byte[num];
                                for (uint i = 0; i < data.Length; i++)
                                {
                                    data[i] = (byte) this._dataBuffer.Dequeue();
                                }
                                flag = true;
                                this.ParserState = ParserStateEnum.packet_type_token;
                            }
                        }
                        else
                        {
                            flag = false;
                            this.ParserState = ParserStateEnum.packet_type_token;
                        }
                        break;
                }
            }
            this.bufferMutex.ReleaseMutex();
            return flag;
        }

        private enum ParserStateEnum
        {
            packet_type_token,
            event_code_token,
            eop0_token,
            eop1_token,
            length_token,
            data_token
        }
    }
}


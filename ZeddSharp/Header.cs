using System;
using System.Collections.Generic;
using System.Text;

namespace ZeddSharp
{
    public class Header
    {
        public enum Version
        {
            V0,
            V1,
            V2,
            V3,
            V4,
            V5,
            V6
        };

        internal byte[] headerData;

        public Header(byte[] data)
        {
            headerData = data;
        }

        public byte VersionNumber => headerData[0x00];

        public byte StatusLineType => (byte)((headerData[0x01] >> 1) & 0x01);

        private byte GetBitOfByte(byte val, byte bitPos) => (byte)((val >> bitPos) & 0x01);
    }
}

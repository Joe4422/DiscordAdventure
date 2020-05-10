using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace ZeddSharp
{
    public class MemoryMap
    {
        private byte[] memory;

        public byte[] Header
        {
            get
            {
                try
                {
                    return memory[0..40];
                }
                catch (Exception)
                {
                    return new byte[0];
                }
            }
        }
        public uint Dynamic { get; }
        public uint Static { get; }
        public uint High { get; }
    }
}

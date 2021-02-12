using System;
using System.Collections.Generic;
using System.Text;

namespace RingingBloom.NBNK
{
    interface IChunk
    {
        public char[] dwTag { get;}
        public uint dwChunkSize { get; set; }
    }
}

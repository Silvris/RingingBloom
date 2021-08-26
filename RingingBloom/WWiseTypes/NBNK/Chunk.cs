using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace RingingBloom.NBNK
{
    public abstract class Chunk : BNKItem
    {
        public char[] dwTag { get;}
        public uint dwChunkSize { get; set; }
        public virtual void Export(BinaryWriter bw)
        {
            MessageBox.Show("You shouldn't be seeing this message.");
        }
    }
}

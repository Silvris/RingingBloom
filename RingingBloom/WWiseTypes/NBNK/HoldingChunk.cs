using RingingBloom.NBNK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace RingingBloom.WWiseTypes.NBNK
{
    public class HoldingChunk : Chunk
    {
        public byte[] holding;
        private string _Header;
        public new string Header { get => _Header; set => _Header = value; }


        public HoldingChunk(string header, byte[] section)
        {
            holding = section;
            Header = header;
            //don't need the rest
        }

        public override void Export(BinaryWriter bw)
        {
            bw.Write(holding);
        }
    }
}

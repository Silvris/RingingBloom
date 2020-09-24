using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using RingingBloom.Common;

namespace RingingBloom
{
    public class NPCKHeader
    {
        public byte[] magic = { (byte)'A', (byte)'K', (byte)'P', (byte)'K' };
        public uint headerLength;
        uint unkn2 = 1;
        uint unkn3 = 20;
        uint unkn4 = 4;
        public uint wemTableLength;
        uint unkn6 = 4;
        uint unkn7 = 1;
        uint unkn8 = 12;
        uint unknCount = 0;
        uint unknValue = 0;
        uint unknA = 0;
        string audioLang;
        string SFX = "sfx";
        uint unkn10 = 0;
        public uint wemCount;

        public List<Wem> WemList = new List<Wem>();

        public void ExportFile(string aFilePath)
        {
            wemTableLength = wemCount * 20;
            headerLength = (wemTableLength) + 56;
            BinaryWriter bw = new BinaryWriter(File.Create(aFilePath));
            bw.Write(magic);
            bw.Write(headerLength);
            bw.Write(unkn2);
            bw.Write(unkn3);
            bw.Write(unkn4);
            bw.Write(wemTableLength);
            bw.Write(unkn6);
            bw.Write(unkn7);
            bw.Write(unkn8);
            bw.Write(unknCount);
            if(unknCount > 0)
            {
                bw.Write(unknValue);
                bw.Write(unknA);
                bw.Write(Encoding.Unicode.GetBytes(audioLang));
                bw.Write(false);
                bw.Write(false);
            }
            bw.Write(Encoding.Unicode.GetBytes(SFX));
            bw.Write(false);
            bw.Write(false);
            bw.Write(unkn10);
            bw.Write(wemCount);
            uint currentOffset = headerLength+4;
            uint workingOffset = 0;
            foreach(Wem wem in WemList)
            {
                bw.Write(wem.id);
                bw.Write(1);
                bw.Write(wem.length);
                bw.Write(currentOffset);
                workingOffset = (uint)bw.BaseStream.Position;
                bw.Seek((int)currentOffset, SeekOrigin.Begin);
                bw.Write(wem.file);
                bw.Seek((int)workingOffset,SeekOrigin.Begin);
                currentOffset += wem.length;
                bw.Write(0);
            }
            bw.Close();
        }
    }
}

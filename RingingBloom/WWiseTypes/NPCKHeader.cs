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

        //created constructor
        public NPCKHeader()
        {
            //nothing to do here either
        }
        //imported constructor
        public NPCKHeader(BinaryReader br)
        {
            char[] magicBytes = br.ReadChars(4);
            uint headerLen = br.ReadUInt32();
            unkn2 = br.ReadUInt32();
            unkn3 = br.ReadUInt32();
            unkn4 = br.ReadUInt32();
            wemTableLength = br.ReadUInt32();
            unkn6 = br.ReadUInt32();
            unkn7 = br.ReadUInt32();
            unkn8 = br.ReadUInt32();
            unknCount = br.ReadUInt32();
            if (unknCount > 0)
            {
                unknValue = br.ReadUInt32();
                unknA = br.ReadUInt32();
                audioLang = HelperFunctions.ReadUniNullTerminatedString(br);
            }
            string Asfx = HelperFunctions.ReadUniNullTerminatedString(br);
            unkn10 = br.ReadUInt32();
            uint wemACount = br.ReadUInt32();
            for(int i = 0; i < wemACount; i++)
            {
                uint id = br.ReadUInt32();
                uint one = br.ReadUInt32();
                uint length = br.ReadUInt32();
                uint offset = br.ReadUInt32();
                uint zero = br.ReadUInt32();
                int workingOffset = (int)br.BaseStream.Position;
                br.BaseStream.Seek(offset, SeekOrigin.Begin);
                byte[] file = br.ReadBytes((int)length);
                br.BaseStream.Seek(workingOffset, SeekOrigin.Begin);
                Wem newWem = new Wem("Imported Wem" + i, id, file);
                WemList.Add(newWem);
            }
        }

        public void ExportFile(string aFilePath)
        {
            wemTableLength = (uint)WemList.Count * 20;
            headerLength = (wemTableLength) + 56;
            if(unknCount > 0)
            {
                headerLength += (uint)(8 + (audioLang.Length * 2) + 2);
            }
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
                HelperFunctions.WriteUniNullTerminatedString(bw, audioLang);
            }
            HelperFunctions.WriteUniNullTerminatedString(bw, SFX);
            bw.Write(unkn10);
            bw.Write(WemList.Count);
            uint currentOffset = headerLength+4;
            foreach(Wem wem in WemList)
            {
                bw.Write(wem.id);
                bw.Write(1);
                bw.Write(wem.length);
                bw.Write(currentOffset);
                int workingOffset = (int)bw.BaseStream.Position;
                bw.Seek((int)currentOffset, SeekOrigin.Begin);
                bw.Write(wem.file);
                bw.Seek(workingOffset,SeekOrigin.Begin);
                currentOffset += wem.length;
                bw.Write(0);
            }
            bw.Close();
        }
    }
}

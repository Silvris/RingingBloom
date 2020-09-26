using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RingingBloom.Common;

namespace RingingBloom.NBNK
{
    class DIDX_DATA
    {
        char[] DIDX = new char[] { 'D', 'I', 'D', 'X' };
        uint wemCount;
        char[] DATA = new char[] { 'D', 'A', 'T', 'A' };
        List<Wem> wemList;
        //these are combined since they're both wem data, best to read it immediately into a wem
        public DIDX_DATA(BinaryReader br)
        {
            char[] didxRead = br.ReadChars(4);
            uint didxLength = br.ReadUInt32();
            wemCount = didxLength / 12;
            uint[] ids = new uint[wemCount];
            uint[] offsets = new uint[wemCount];
            uint[] lengths = new uint[wemCount];
            for(int i = 0; i < wemCount; i++)
            {
                ids[i] = br.ReadUInt32();
                offsets[i] = br.ReadUInt32();
                lengths[i] = br.ReadUInt32();
            }
            long DataOff = br.BaseStream.Position;
            char[] dataRead = br.ReadChars(4);
            uint dataLength = br.ReadUInt32();
            List<byte[]> wemDatas = new List<byte[]>();
            for(int i = 0; i < wemCount; i++)
            {
                br.BaseStream.Seek(DataOff + offsets[i],SeekOrigin.Begin);
                wemDatas[i] = br.ReadBytes((int)lengths[i]);
            }
            for(int i = 0; i < wemCount; i++)
            {
                Wem newWem = new Wem("Imported Wem " + i, ids[i], wemDatas[i]);
                wemList.Add(newWem);
            }
        }

        public void AddWem(string aName, uint aId, BinaryReader br)
        {
            Wem newWem = new Wem(aName, Convert.ToString(aId), br);
            wemList.Add(newWem);
        }

        public void ExportDIDXDATA(BinaryWriter bw)
        {
            bw.Write(DIDX);
            bw.Write(wemCount * 12);
            uint currentOffset = 0;
            for(int i = 0; i < wemCount; i++)
            {
                bw.Write(wemList[i].id);
                bw.Write(currentOffset);
                bw.Write(wemList[i].length);
                currentOffset += wemList[i].length;
            }
            bw.Write(DATA);
            bw.Write(currentOffset);
            for(int i = 0; i < wemCount; i++)
            {
                bw.Write(wemList[i].file);
            }
        }
    }
}

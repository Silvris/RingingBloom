using RingingBloom.Common;
using RingingBloom.WWiseTypes.NBNK.HIRC;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingingBloom.NBNK
{
    public class HIRC : Chunk
    {
        private char[] magic = { 'H', 'I', 'R', 'C' };
        private uint sectionLength;
        ObservableCollection<HIRCNode> wwiseObjects = new ObservableCollection<HIRCNode>();

        public new char[] dwTag => magic;

        public new uint dwChunkSize { get => sectionLength; set => sectionLength = value; }
        public new string Header { get => "Object Hierarchy"; set => throw new NotImplementedException(); }
        public ObservableCollection<HIRCNode> Items { get => wwiseObjects; set => wwiseObjects = value; }

        public HIRC(BinaryReader br)
        {
            dwChunkSize = br.ReadUInt32();
            uint objectCount = br.ReadUInt32();
            for(int i = 0; i < objectCount; i++)
            {
                HIRCTypes hirctype = (HIRCTypes)br.ReadByte();
                switch (hirctype)
                {
                    default:
                        uint length = br.ReadUInt32();
                        HIRCUnkn newUnkn = new HIRCUnkn(hirctype, length, br);
                        Items.Add(newUnkn);
                        break;
                }
                
            }
        }


        public int ReturnSectionLength()
        {
            int length = 4;
            for(int i = 0; i < wwiseObjects.Count; i++)
            {
                length += wwiseObjects[i].CalculateSectionLength();
            }
            return length;
        }

        public override void Export(BinaryWriter bw)
        {
            bw.Write(magic);
            bw.Write(ReturnSectionLength());
            bw.Write(wwiseObjects.Count);

        }

    }
}

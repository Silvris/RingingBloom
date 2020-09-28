using RingingBloom.WWiseTypes.NBNK.HIRC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingingBloom.NBNK
{
    class HIRC
    {
        char[] magic = { 'H', 'I', 'R', 'C' };
        uint sectionLength;
        uint objectCount;
        List<HIRCObject> wwiseObjects;
        public HIRC(BinaryReader br)
        {
            sectionLength = br.ReadUInt32();
            objectCount = br.ReadUInt32();
            for(int i = 0; i < objectCount; i++)
            {
                HIRCObject newObj = new HIRCObject(br);
                wwiseObjects.Add(newObj);
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

        public void ExportHIRC(BinaryWriter bw)
        {
            bw.Write(magic);
            bw.Write(ReturnSectionLength());
            bw.Write(wwiseObjects.Count);

        }
    }
}

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

        public void ExportHIRC(BinaryWriter bw)
        {

        }
    }
}

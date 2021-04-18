using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RingingBloom.Common;

namespace RingingBloom.WWiseTypes.NBNK.HIRC
{
    class HIRC4
    {
        HIRCTypes eHircType = HIRCTypes.Event;
        uint dwSectionSize;
        uint ulId;
        List<uint> ulActionIDs;

        public HIRC4(BinaryReader br)
        {
            ulId = br.ReadUInt32();
            uint ulActionListSize = br.ReadUInt32();
            for(int i = 0; i < ulActionListSize; i++)
            {
                ulActionIDs.Add(br.ReadUInt32());
            }
            dwSectionSize = (ulActionListSize * 4) + 8;
        }

        public void AddAction()
        {
            ulActionIDs.Add(0);
            dwSectionSize += 4;
        }

        public int GetLength()
        {
            return (int)dwSectionSize;
        }

        public void Export(BinaryWriter bw)
        {
            bw.Write((byte)eHircType);
            bw.Write(dwSectionSize);
            bw.Write(ulId);
            bw.Write(ulActionIDs.Count);
            for (int i = 0; i < ulActionIDs.Count; i++)
            {
                bw.Write(ulActionIDs[i]);
            }
        }
    }
}

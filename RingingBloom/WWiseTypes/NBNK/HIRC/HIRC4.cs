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
        HIRCTypes type = HIRCTypes.Event;
        uint length;
        uint objectId;
        uint actionCount;
        List<uint> actionIds;

        public HIRC4(BinaryReader br)
        {
            objectId = br.ReadUInt32();
            actionCount = br.ReadUInt32();
            for(int i = 0; i < actionCount; i++)
            {
                actionIds.Add(br.ReadUInt32());
            }
            length = (actionCount * 4) + 8;
        }

        public void AddAction()
        {
            actionCount++;
            actionIds.Add(0);
            length += 4;
        }

        public int GetLength()
        {
            return (int)length;
        }

        public void Export(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(length);
            bw.Write(objectId);
            bw.Write(actionCount);
            for (int i = 0; i < actionCount; i++)
            {
                bw.Write(actionIds[i]);
            }
        }
    }
}

using RingingBloom.Common;
using RingingBloom.NBNK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingingBloom.WWiseTypes.NBNK.HIRC
{
    class HIRC1 : HIRCNode
    {
        HIRCTypes eHircType = HIRCTypes.State;
        uint dwSectionSize { get; set; }
        uint ulId { get; set; }
        byte settingsCount;
        List<byte> settingsType;
        List<float> settingsValue;

        public HIRC1(BinaryReader br)
        {
            ulId = br.ReadUInt32();
            settingsCount = br.ReadByte();
            for (int i = 0; i < settingsCount; i++)
            {
                settingsType.Add(br.ReadByte());
            }
            for (int i = 0; i < settingsCount; i++)
            {
                settingsValue.Add(br.ReadSingle());
            }
        }

        public int GetLength()
        {
            return 5 + (settingsCount * 5);
        }

        public void AddSetting()
        {
            settingsCount++;
            settingsType.Add(0);
            settingsValue.Add(0);
        }

        public void Export(BinaryWriter bw)
        {
            bw.Write((byte)eHircType);
            bw.Write(dwSectionSize);
            bw.Write(ulId);
            bw.Write(settingsCount);
            for (int i = 0; i < settingsCount; i++)
            {
                bw.Write(settingsType[i]);
            }
            for(int i = 0; i < settingsCount; i++)
            {
                bw.Write(settingsValue[i]);
            }
        }
    }
}

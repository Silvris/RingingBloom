using RingingBloom.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingingBloom.WWiseTypes.NBNK.HIRC
{
    class HIRC1
    {
        HIRCTypes type = HIRCTypes.Settings;
        uint length;
        uint objectID;
        byte settingsCount;
        List<byte> settingsType;
        List<float> settingsValue;

        public HIRC1(BinaryReader br)
        {
            objectID = br.ReadUInt32();
            length = br.ReadUInt32();
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

        public void AddSetting()
        {
            settingsCount++;
            settingsType.Add(0);
            settingsValue.Add(0);
        }

        public void Export(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(length);
            bw.Write(objectID);
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

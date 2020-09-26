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
        byte[] settingsType;
        float[] settingsValue;

        HIRC1(uint aID, byte aCount, byte[] aTypeArray, float[] aValueArray)
        {
            objectID = aID;
            length = ((uint)aCount * 5 )+ 1;
            settingsCount = aCount;
            settingsType = aTypeArray;
            settingsValue = aValueArray;
        }

        public void Export(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(length);
            bw.Write(objectID);
            bw.Write(settingsCount);
            bw.Write(settingsType);
            for(int i = 0; i < settingsCount; i++)
            {
                bw.Write(settingsValue[i]);
            }
        }
    }
}

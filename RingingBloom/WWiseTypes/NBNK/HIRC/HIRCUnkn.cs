using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RingingBloom.WWiseTypes.NBNK.HIRC
{
    class HIRCUnkn
    {
        byte type;
        byte[] Data;

        public HIRCUnkn(byte aType,uint length, BinaryReader br)
        {
            type = aType;
            Data = br.ReadBytes((int)length);
        }

        public void Export(BinaryWriter bw)
        {
            bw.Write(type);
            bw.Write(Data.Length);
            bw.Write(Data);
        }
    }
}

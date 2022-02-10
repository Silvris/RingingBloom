using RingingBloom.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RingingBloom.WWiseTypes.NBNK.HIRC
{
    class HIRCUnkn : HIRCNode
    {
        HIRCTypes type;
        byte[] Data;

        public HIRCUnkn(HIRCTypes aType,uint length, BinaryReader br)
        {
            Header = "Unknown Type " + aType.ToString();
            type = aType;
            Data = br.ReadBytes((int)length);
        }

        public int GetLength()
        {
            return Data.Length;
        }

        public override void Export(BinaryWriter bw)
        {
            bw.Write((byte)type);
            bw.Write(Data.Length);
            bw.Write(Data);
        }

        public override int CalculateSectionLength()
        {
            return Data.Length;
        }
    }
}

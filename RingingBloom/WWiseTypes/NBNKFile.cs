using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RingingBloom.NBNK;
using RingingBloom.WWiseTypes.NBNK.HIRC;

namespace RingingBloom
{
    class NBNKFile
    {
        BKHD BankHeader = null;
        DIDX_DATA DataIndex = null;
        HIRC ObjectHierarchy = null;
        NBNKFile()
        {
            
        }

        public void ImportNBNK(BinaryReader br)
        {
            while(br.BaseStream.Position < br.BaseStream.Length)
            {
                string magic = Convert.ToString(br.ReadChars(4));
                switch (magic)
                {
                    case "BKHD":
                        uint SLength = br.ReadUInt32();
                        BankHeader = new BKHD(SLength, br);
                        break;
                    case "DIDX":
                        DataIndex = new DIDX_DATA(br);
                        break;
                    case "HIRC":
                        ObjectHierarchy = new HIRC(br);
                        break;
                    default:
                        break;
                }
            }
        }

        public void ExportNBNK(BinaryWriter bw)
        {

        }
    }
}

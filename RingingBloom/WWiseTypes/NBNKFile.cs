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


        //imported constructor
        public NBNKFile(BinaryReader br)
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

        //created constructor, this really shouldn't be used very often
        public NBNKFile()
        {
            BankHeader = new BKHD();
        }

        public void ExportNBNK(BinaryWriter bw)
        {
            if(BankHeader != null)
            {
                BankHeader.Export(bw);
            }
            if(DataIndex != null)
            {
                DataIndex.ExportDIDXDATA(bw);
            }
            if(ObjectHierarchy != null)
            {
                ObjectHierarchy.ExportHIRC(bw);
            }
        }
    }
}

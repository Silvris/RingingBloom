using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RingingBloom.Common;
using RingingBloom.NBNK;
using RingingBloom.WWiseTypes.NBNK.HIRC;

namespace RingingBloom
{
    class NBNKFile
    {
        public BKHD BankHeader = null;
        public DIDX DataIndex = null;
        //HIRC ObjectHierarchy = null;
        public List<byte[]> holding;//fun fact a list of lists is doable, but I don't really need it for this


        //imported constructor
        public NBNKFile(BinaryReader br, SupportedGames mode)
        {
            holding = new List<byte[]>();
            while(br.BaseStream.Position < br.BaseStream.Length)
            {
                byte[] magicArr = br.ReadBytes(4);
                string magic = Encoding.UTF8.GetString(magicArr);
                switch (magic)
                {
                    case "BKHD":
                        uint SLength = br.ReadUInt32();
                        BankHeader = new BKHD(SLength, br);
                        break;
                    case "DIDX":
                        DataIndex = new DIDX(br);
                        break;
                    /*case "HIRC":
                        ObjectHierarchy = new HIRC(br);
                        break;*/
                    default:
                        //this adds 
                        SLength = br.ReadUInt32();
                        byte[] tLength = BitConverter.GetBytes(SLength);
                        byte[] data = br.ReadBytes((int)SLength);
                        byte[] section = HelperFunctions.Combine(HelperFunctions.Combine(magicArr, tLength), data);
                        holding.Add(section);
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
                DataIndex.Export(bw);
            }
            /*if(ObjectHierarchy != null)
            {
                ObjectHierarchy.ExportHIRC(bw);
            }*/
            for(int i = 0; i < holding.Count; i++)
            {
                bw.Write(holding[i]);
            }
        }
    }
}

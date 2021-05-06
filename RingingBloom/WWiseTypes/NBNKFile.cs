using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using RingingBloom.Common;
using RingingBloom.NBNK;
using RingingBloom.WWiseTypes;
using RingingBloom.WWiseTypes.NBNK.HIRC;

namespace RingingBloom
{
    class NBNKFile
    {
        public BKHD BankHeader = null;
        public DIDX DataIndex = null;
        //HIRC ObjectHierarchy = null;
        public List<byte[]> holding;//fun fact a list of lists is doable, but I don't really need it for this
        public Labels labels =null;


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
                        uint bnkId = BankHeader.dwSoundbankID;
                        string path = Directory.GetCurrentDirectory() +"/" +mode.ToString() + "/BNK/" + bnkId.ToString() + ".lbl";
                        if (File.Exists(path))
                        {
                            MessageBoxResult labelRead = MessageBox.Show("Label file found. Read labels?","Labels", MessageBoxButton.YesNo);
                            if (labelRead == MessageBoxResult.Yes)
                            {
                                labels = new Labels(XmlReader.Create(path));
                            }
                            else
                            {
                                labels = new Labels();
                            }
                        }
                        else
                        {
                            labels = new Labels();
                        }
                        break;
                    case "DIDX":
                        DataIndex = new DIDX(br,labels);
                        break;
                    /*case "HIRC":
                        ObjectHierarchy = new HIRC(br);
                        break;*/
                    default:
                        //this adds support to not-immediately-interpreted versions of WWise, assuming that the main 3 (BKHD, DIDX, DATA) do not change in structure
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

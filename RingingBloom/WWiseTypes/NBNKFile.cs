using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using RingingBloom.Common;
using RingingBloom.NBNK;
using RingingBloom.WWiseTypes;
using RingingBloom.WWiseTypes.NBNK;
using RingingBloom.WWiseTypes.NBNK.HIRC;

namespace RingingBloom
{
    public class NBNKFile
    {
        public BKHD BankHeader = null;
        public DIDX DataIndex = null;
        public HIRC ObjectHierarchy = null;
        public List<HoldingChunk> holding = null;
        public Labels labels = null;

        public void ReadFile(BinaryReader br, SupportedGames mode)
        {
            labels = null;
            holding = new List<HoldingChunk>();
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                byte[] magicArr = br.ReadBytes(4);
                string magic = Encoding.UTF8.GetString(magicArr);
                switch (magic)
                {
                    case "BKHD":
                        uint SLength = br.ReadUInt32();
                        BankHeader = new BKHD(SLength, br);
                        uint bnkId = BankHeader.dwSoundbankID;
                        string path = Directory.GetCurrentDirectory() + "/" + mode.ToString() + "/BNK/" + bnkId.ToString() + ".lbl";
                        if (File.Exists(path))
                        {
                            MessageBoxResult labelRead = MessageBox.Show("Label file found. Read labels?", "Labels", MessageBoxButton.YesNo);
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
                        DataIndex = new DIDX(br, labels);
                        break;
                    case "HIRC":
                        if(mode == SupportedGames.MHWorld)
                        {
                            ObjectHierarchy = new HIRC(br);
                            break;
                        }
                        else
                        {
                            ReadDefault(br,magic,magicArr);
                            break;
                        }
                    default:
                        //this adds support to not-immediately-interpreted versions of WWise, assuming that the main 3 (BKHD, DIDX, DATA) do not change in structure
                        ReadDefault(br, magic, magicArr);
                        break;
                }
            }
            br.Close();
        }

        //created constructor, this really shouldn't be used very often
        public NBNKFile()
        {

        }
        public void ReadDefault(BinaryReader br,string magic, byte[] magicArr)
        {
            uint SLength = br.ReadUInt32();
            byte[] tLength = BitConverter.GetBytes(SLength);
            byte[] data = br.ReadBytes((int)SLength);
            byte[] section = HelperFunctions.Combine(HelperFunctions.Combine(magicArr, tLength), data);
            holding.Add(new HoldingChunk(magic, section));
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
            if(ObjectHierarchy != null)
            {
                ObjectHierarchy.Export(bw);
            }
            if(holding.Count > 0)
            {
                for(int i = 0; i < holding.Count; i++)
                {
                    holding[i].Export(bw);
                }
            }
            bw.Close();
        }
    }
}

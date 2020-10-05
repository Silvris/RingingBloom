using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingingBloom.WWiseTypes
{
    public class PKBKString
    {
        public string Path { get; set; }

        public PKBKString(string aName)
        {
            Path = aName;
        }
    }
    public class WWPKBKFile
    {
        char[] WWPK = { 'W', 'W', 'P', 'K' };
        char[] WWBK = { 'W', 'W', 'B', 'K' };
        uint PKVersion = 4;
        uint BKVersion = 3;
        public List<PKBKString> paths = new List<PKBKString>();

        //these are together since there is a very minute difference between them

        //imported constructor
        public WWPKBKFile(BinaryReader br)
        {
            char[] magic = br.ReadChars(4);
            uint version = br.ReadUInt32();
            uint pathCount = br.ReadUInt32();
            uint null1 = br.ReadUInt32();
            for(int i = 0; i < pathCount; i++)
            {
                if(magic.ToString() == WWPK.ToString())
                {
                    paths.Add(new PKBKString(HelperFunctions.ReadNullTerminatedString(br)));
                    byte null2 = br.ReadByte();
                }
                else
                {
                    paths.Add(new PKBKString(HelperFunctions.ReadNullTerminatedString(br)));
                    byte[] null2 = br.ReadBytes(2);
                }
            }
        }

        //created constructor
        public WWPKBKFile()
        {
            //nothing in here, since my list is already created prior
        }

        public void ExportFile(string type,BinaryWriter bw)
        {
            if(type == "wwpk")
            {
                bw.Write(WWPK);
                bw.Write(PKVersion);
                bw.Write((uint)paths.Count);
                bw.Write(0);
                for(int i = 0; i < paths.Count; i++)
                {
                    HelperFunctions.WriteNullTerminatedString(bw, paths[i].Path);
                    bw.Write(false);
                }
            }
            else
            {
                bw.Write(WWBK);
                bw.Write(BKVersion);
                bw.Write((uint)paths.Count);
                bw.Write(0);
                for (int i = 0; i < paths.Count; i++)
                {
                    HelperFunctions.WriteNullTerminatedString(bw, paths[i].Path);
                    bw.Write(false);
                    bw.Write(false);
                }
            }
        }
    }
}

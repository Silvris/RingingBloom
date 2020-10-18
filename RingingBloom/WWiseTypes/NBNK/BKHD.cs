using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RingingBloom.NBNK
{
    public class BKHD
    {
        char[] magic = new char[] { 'B', 'K', 'H', 'D' };
        uint sectionLength;
        uint unkn1;
        uint thisIsAHash;
        List<uint> unkns = new List<uint>();

        //imported constructor, the most common one
        public BKHD(uint SLength, BinaryReader br)
        {
            sectionLength = SLength;
            unkn1 = br.ReadUInt32();
            thisIsAHash = br.ReadUInt32();
            uint unknCount = (SLength / 4) - 2;
            for(int i = 0; i < unknCount; i++)
            {
                unkns.Add(br.ReadUInt32());
            }
        }

        //created constructor, shouldn't be used normally
        public BKHD()
        {
            sectionLength = 24;
            unkn1 = 120;
            thisIsAHash = 0; //make sure to implement some form of editing this value
            unkns.Add(0);
            unkns.Add(0);
            unkns.Add(1144);
            unkns.Add(0);
        }

        public void Export(BinaryWriter bw)
        {
            bw.Write(magic);
            bw.Write(8 + (unkns.Count * 4));
            bw.Write(unkn1);
            bw.Write(thisIsAHash);
            for(int i = 0; i < unkns.Count; i++)
            {
                bw.Write(unkns[i]);
            }
        }
    }
}

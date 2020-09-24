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
        uint[] unkns;

        BKHD(uint SLength, BinaryReader br)
        {
            sectionLength = SLength;
            unkn1 = br.ReadUInt32();
            thisIsAHash = br.ReadUInt32();
            uint unknCount = (SLength / 4) - 2;
            for(int i = 0; i < unknCount; i++)
            {
                unkns[i] = br.ReadUInt32();
            }
        }
    }
}

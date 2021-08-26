using RingingBloom.Common;
using RingingBloom.NBNK;
using System;
using System.Collections.Generic;
using System.Text;

namespace RingingBloom.WWiseTypes.NBNK.HIRC
{
    public abstract class HIRCNode : BNKItem
    {
        public abstract int CalculateSectionLength();
        public HIRCTypes eHircType { get; set; }
        public uint dwSectionSize { get; set; }
        public uint ulID { get; set; }
    }
}

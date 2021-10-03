using RingingBloom.Common;
using RingingBloom.NBNK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RingingBloom.WWiseTypes.NBNK.HIRC
{
    public abstract class HIRCNode : BNKItem
    {
        private HIRCTypes _eHircType;
        private uint _dwSectionSize;
        private uint _ulID;
        public abstract int CalculateSectionLength();
        public abstract void Export(BinaryWriter bw);
        public void ExportHeader(BinaryWriter bw)
        {
            bw.Write((byte)eHircType);
            bw.Write(dwSectionSize);
            bw.Write(ulID);
        }
        public HIRCTypes eHircType { get => _eHircType; private set => _eHircType = value; }
        public uint dwSectionSize { get => _dwSectionSize; set => _dwSectionSize = value; }
        public uint ulID { get => _ulID; set => _ulID = value; }
    }
}

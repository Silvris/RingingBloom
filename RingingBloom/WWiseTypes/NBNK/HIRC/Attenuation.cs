using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RingingBloom.WWiseTypes.NBNK.HIRC
{
    public class ConeSettings
    {
        public float fInsideDegrees { get; set; }
        public float fOutsideDegrees { get; set; }
        public float fOutsideVolume { get; set; }
        public float fLoPass { get; set; }
        public float fHiPass { get; set; }
    }
    public class Attenuation : HIRCNode
    {
        public bool _IsConeEnabled { get; set; }
        public bool bIsConeEnabled { get; set; }

        public override int CalculateSectionLength()
        {
            throw new NotImplementedException();
        }

        public override void Export(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }
    }
}

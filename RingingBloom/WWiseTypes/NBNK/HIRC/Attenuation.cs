using System;
using System.Collections.Generic;
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
    public class Attenuation
    {
        private bool _IsConeEnabled;
        public bool bIsConeEnabled { get; set; }
    }
}

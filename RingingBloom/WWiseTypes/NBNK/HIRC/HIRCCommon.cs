using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RingingBloom.WWiseTypes.NBNK.HIRC
{
    public class AkPropValue
    {
        private Dictionary<byte, float> _PropValues = new Dictionary<byte, float>();
        public Dictionary<byte, float> PropValues { get => _PropValues; set => _PropValues = value; }
        public AkPropValue()
        {

        }
        public AkPropValue(BinaryReader br)
        {
            byte cProps = br.ReadByte();
            List<byte> pIDs = new List<byte>();
            List<float> pValues = new List<float>();
            for(int i = 0; i < cProps; i++)
            {
                pIDs.Add(br.ReadByte());
            }
            for (int i = 0; i < cProps; i++)
            {
                pValues.Add(br.ReadSingle());
            }
            for (int i = 0; i < pIDs.Count; i++)
            {
                PropValues.Add(pIDs[i], pValues[i]);
            }
        }
    }
    public class AkPropRangedValue
    {
        private Dictionary<byte, float> _PropValues = new Dictionary<byte, float>();
        public Dictionary<byte, float> PropValues { get => _PropValues; set => _PropValues = value; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RingingBloom.Common;

namespace RingingBloom.WWiseTypes
{
    public class WWCTString
    {
        public WWCTType Type { get; set; }
        public string Path { get; set; }
        public uint Index { get; set; }

        public WWCTString(WWCTType aType, string aPath, uint aIndex)
        {
            Type = aType;
            Path = aPath;
            Index = aIndex;
        }
        public IList<WWCTType> WWCTTypes
        {
            get
            {
                return Enum.GetValues(typeof(WWCTType)).Cast<WWCTType>().ToList<WWCTType>();
            }
        }
    }

    public class WWCTFile
    {
        char[] WWCT = { 'W', 'W', 'C', 'T' };
        uint version = 6;
        public List<WWCTString> wwctStrings = new List<WWCTString>();
        //imported constructor
        public WWCTFile(BinaryReader br)
        {
            uint magic = br.ReadUInt32();
            uint version = br.ReadUInt32();
            uint WWBKCount = br.ReadUInt32();
            uint WWPKCount = br.ReadUInt32();
            uint WWEVCount = br.ReadUInt32();
            uint WWSWCount = br.ReadUInt32();
            uint WWSTCount = br.ReadUInt32();
            uint WWGPCount = br.ReadUInt32();
            uint WWENFCount = br.ReadUInt32();
            uint Unkn3Count = br.ReadUInt32();
            if (WWEVCount > 0)
            {
                for (int i = 0; i < WWEVCount; i++)
                {
                    uint newIndex = br.ReadUInt32();
                    string newString = HelperFunctions.ReadNullTerminatedString(br);
                    WWCTString newWWCTS = new WWCTString(WWCTType.WWEV, newString, newIndex);
                    wwctStrings.Add(newWWCTS);
                }
            }
            if (WWBKCount > 0)
            {
                for (int i = 0; i < WWBKCount; i++)
                {
                    uint newIndex = br.ReadUInt32();
                    string newString = HelperFunctions.ReadNullTerminatedString(br);
                    WWCTString newWWCTS = new WWCTString(WWCTType.WWBK, newString, newIndex);
                    wwctStrings.Add(newWWCTS);
                }
            }
            if (WWPKCount > 0)
            {
                for (int i = 0; i < WWPKCount; i++)
                {
                    uint newIndex = br.ReadUInt32();
                    string newString = HelperFunctions.ReadNullTerminatedString(br);
                    WWCTString newWWCTS = new WWCTString(WWCTType.WWPK, newString, newIndex);
                    wwctStrings.Add(newWWCTS);
                }
            }
            if (WWSWCount > 0)
            {
                for (int i = 0; i < WWSWCount; i++)
                {
                    uint newIndex = br.ReadUInt32();
                    string newString = HelperFunctions.ReadNullTerminatedString(br);
                    WWCTString newWWCTS = new WWCTString(WWCTType.WWSW, newString, newIndex);
                    wwctStrings.Add(newWWCTS);
                }
            }
            if (WWSTCount > 0)
            {
                for (int i = 0; i < WWSTCount; i++)
                {
                    uint newIndex = br.ReadUInt32();
                    string newString = HelperFunctions.ReadNullTerminatedString(br);
                    WWCTString newWWCTS = new WWCTString(WWCTType.WWST, newString, newIndex);
                    wwctStrings.Add(newWWCTS);
                }
            }
            if (WWGPCount > 0)
            {
                for (int i = 0; i < WWGPCount; i++)
                {
                    uint newIndex = br.ReadUInt32();
                    string newString = HelperFunctions.ReadNullTerminatedString(br);
                    WWCTString newWWCTS = new WWCTString(WWCTType.WWGP, newString, newIndex);
                    wwctStrings.Add(newWWCTS);
                }
            }
            if (WWENFCount > 0)
            {
                for (int i = 0; i < WWENFCount; i++)
                {
                    uint newIndex = br.ReadUInt32();
                    string newString = HelperFunctions.ReadNullTerminatedString(br);
                    WWCTString newWWCTS = new WWCTString(WWCTType.WWENF, newString, newIndex);
                    wwctStrings.Add(newWWCTS);
                }
            }
            if (Unkn3Count > 0)
            {
                for (int i = 0; i < Unkn3Count; i++)
                {
                    uint newIndex = br.ReadUInt32();
                    string newString = HelperFunctions.ReadNullTerminatedString(br);
                    WWCTString newWWCTS = new WWCTString(WWCTType.Unkn3, newString, newIndex);
                    wwctStrings.Add(newWWCTS);
                }
            }
        }

        //new constructor
        public WWCTFile()
        {
            //nothing needs to be done for this, since 0 value WWCTs exist
        }

        public bool CompareWWCTString(WWCTString string1, WWCTString string2)
        {
            if (string1.Index == string2.Index&&string1.Path==string2.Path&&string1.Type==string2.Type)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int CalculateTypeCount(WWCTType type)
        {
            int count = 0;
            for(int i =0; i < wwctStrings.Count; i++)
            {
                if(wwctStrings[i].Type == type)
                {
                    count++;
                }
            }
            return count;
        }

        public void ExportType(WWCTType aType,BinaryWriter bw)
        {
            for(int i = 0; i < wwctStrings.Count; i++)
            {
                if(wwctStrings[i].Type == aType)
                {
                    bw.Write(wwctStrings[i].Index);
                    HelperFunctions.WriteNullTerminatedString(bw, wwctStrings[i].Path);
                }
            }
        }

        //exporter function
        public void ExportFile(BinaryWriter bw)
        {
            bw.Write(WWCT);
            bw.Write(version);
            bw.Write(CalculateTypeCount(WWCTType.WWBK));
            bw.Write(CalculateTypeCount(WWCTType.WWPK));
            bw.Write(CalculateTypeCount(WWCTType.WWEV));
            bw.Write(CalculateTypeCount(WWCTType.WWSW));
            bw.Write(CalculateTypeCount(WWCTType.WWST));
            bw.Write(CalculateTypeCount(WWCTType.WWGP));
            bw.Write(CalculateTypeCount(WWCTType.WWENF));
            bw.Write(CalculateTypeCount(WWCTType.Unkn3));
            ExportType(WWCTType.WWEV, bw);
            ExportType(WWCTType.WWBK, bw);
            ExportType(WWCTType.WWPK, bw);
            ExportType(WWCTType.WWSW, bw);
            ExportType(WWCTType.WWST, bw);
            ExportType(WWCTType.WWGP, bw);
            ExportType(WWCTType.WWENF, bw);
            ExportType(WWCTType.Unkn3, bw);
        }
    }
}

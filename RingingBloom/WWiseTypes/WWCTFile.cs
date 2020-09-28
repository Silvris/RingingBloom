using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RingingBloom.WWiseTypes
{
    class WWCTFile
    {
        char[] WWCT = { 'W', 'W', 'C', 'T' };
        uint version = 6;
        List<uint> WWBKIndex;
        List<string> WWBKs; 
        List<uint> WWPKIndex;
        List<string> WWPKs;
        List<uint> WWEVIndex;
        List<string> WWEVs;
        List<uint> WWSWIndex;
        List<string> WWSWs;
        List<uint> WWSTIndex;
        List<string> WWSTs;
        List<uint> WWGPIndex;
        List<string> WWGPs;
        List<uint> WWENFIndex;
        List<string> WWENFs;
        List<uint> Unkn3Index;
        List<string> Unkn3s;

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
            for(int i = 0; i < WWEVCount; i++)
            {
                uint newIndex = br.ReadUInt32();
                WWEVIndex.Add(newIndex);
                string newString = br.ReadString();
                WWEVs.Add(newString);
            }
            for (int i = 0; i < WWBKCount; i++)
            {
                uint newIndex = br.ReadUInt32();
                WWBKIndex.Add(newIndex);
                string newString = br.ReadString();
                WWBKs.Add(newString);
            }
            for (int i = 0; i < WWPKCount; i++)
            {
                uint newIndex = br.ReadUInt32();
                WWPKIndex.Add(newIndex);
                string newString = br.ReadString();
                WWPKs.Add(newString);
            }
            for (int i = 0; i < WWSWCount; i++)
            {
                uint newIndex = br.ReadUInt32();
                WWSWIndex.Add(newIndex);
                string newString = br.ReadString();
                WWSWs.Add(newString);
            }
            for (int i = 0; i < WWSTCount; i++)
            {
                uint newIndex = br.ReadUInt32();
                WWSTIndex.Add(newIndex);
                string newString = br.ReadString();
                WWSTs.Add(newString);
            }
            for (int i = 0; i < WWGPCount; i++)
            {
                uint newIndex = br.ReadUInt32();
                WWGPIndex.Add(newIndex);
                string newString = br.ReadString();
                WWGPs.Add(newString);
            }
            for (int i = 0; i < WWENFCount; i++)
            {
                uint newIndex = br.ReadUInt32();
                WWENFIndex.Add(newIndex);
                string newString = br.ReadString();
                WWENFs.Add(newString);
            }
            for (int i = 0; i < Unkn3Count; i++)
            {
                uint newIndex = br.ReadUInt32();
                Unkn3Index.Add(newIndex);
                string newString = br.ReadString();
                Unkn3s.Add(newString);
            }
        }

        //new constructor
        public WWCTFile()
        {
            //nothing needs to be done for this, since 0 value WWCTs exist
        }

        //adds an extra path to the lists of specified type
        public void AddPath(string type)
        {
            switch (type)
            {
                case "WWEV":
                    WWEVIndex.Add((uint)WWEVIndex.Count);
                    WWEVs.Add("");
                    break;
                case "WWBK":
                    WWBKIndex.Add((uint)WWBKIndex.Count);
                    WWBKs.Add("");
                    break;
                case "WWPK":
                    WWPKIndex.Add((uint)WWPKIndex.Count);
                    WWPKs.Add("");
                    break;
                case "WWSW":
                    WWSWIndex.Add((uint)WWSWIndex.Count);
                    WWSWs.Add("");
                    break;
                case "WWST":
                    WWSTIndex.Add((uint)WWSTIndex.Count);
                    WWSTs.Add("");
                    break;
                case "WWGP":
                    WWGPIndex.Add((uint)WWGPIndex.Count);
                    WWGPs.Add("");
                    break;
                case "WWENF":
                    WWENFIndex.Add((uint)WWENFIndex.Count);
                    WWENFs.Add("");
                    break;
                case "Unkn3":
                    Unkn3Index.Add((uint)Unkn3Index.Count);
                    Unkn3s.Add("");
                    break;
                default:
                    MessageBox.Show("Invalid Type");
                    break;
            }
        }

        //exporter function
        public void ExportFile(BinaryWriter bw)
        {
            bw.Write(WWCT);
            bw.Write(version);
            bw.Write(WWBKs.Count);
            bw.Write(WWPKs.Count);
            bw.Write(WWEVs.Count);
            bw.Write(WWSWs.Count);
            bw.Write(WWSTs.Count);
            bw.Write(WWGPs.Count);
            bw.Write(WWENFs.Count);
            bw.Write(Unkn3s.Count);
            for(int i = 0; i < WWEVs.Count; i++)
            {
                bw.Write(WWEVIndex[i]);
                bw.Write(WWEVs[i]);
            }
            for (int i = 0; i < WWBKs.Count; i++)
            {
                bw.Write(WWBKIndex[i]);
                bw.Write(WWBKs[i]);
            }
            for (int i = 0; i < WWPKs.Count; i++)
            {
                bw.Write(WWPKIndex[i]);
                bw.Write(WWPKs[i]);
            }
            for (int i = 0; i < WWSWs.Count; i++)
            {
                bw.Write(WWSWIndex[i]);
                bw.Write(WWSWs[i]);
            }
            for (int i = 0; i < WWSTs.Count; i++)
            {
                bw.Write(WWSTIndex[i]);
                bw.Write(WWSTs[i]);
            }
            for (int i = 0; i < WWGPs.Count; i++)
            {
                bw.Write(WWGPIndex[i]);
                bw.Write(WWGPs[i]);
            }
            for (int i = 0; i < WWENFs.Count; i++)
            {
                bw.Write(WWENFIndex[i]);
                bw.Write(WWENFs[i]);
            }
            for (int i = 0; i < Unkn3s.Count; i++)
            {
                bw.Write(Unkn3Index[i]);
                bw.Write(Unkn3s[i]);
            }
        }
    }
}

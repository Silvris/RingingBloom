using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace RingingBloom.WWiseTypes
{
    public class RequestData
    {
        public int mElemNo { get; set; }
        public int mReqID { get; set; }
        public int mKillReqID { get; set; }
        public uint EpvElement { get; set; }
        public uint EpvIndex { get; set; }

        public RequestData()
        {
            mElemNo = 0;
            mReqID = -1;
            mKillReqID = 0;
            EpvElement = 0;
            EpvIndex = 0;
        }

        public RequestData(BinaryReader br)
        {
            mElemNo = br.ReadInt32();
            mReqID = br.ReadInt32();
            mKillReqID = br.ReadInt32();
            EpvElement = br.ReadUInt32();
            EpvIndex = br.ReadUInt32();
        }
        public void Export(BinaryWriter bw)
        {
            bw.Write(mElemNo);
            bw.Write(mReqID);
            bw.Write(mKillReqID);
            bw.Write(EpvElement);
            bw.Write(EpvIndex);
        }
    }
    public class TriggerData
    {
        public int mElemNo { get; set; }
        public int mReqID { get; set; }
        public uint mCallState { get; set; }
        public uint mOption { get; set; }
        public uint EpvElement { get; set; }
        public uint EpvIndex { get; set; }

        public TriggerData()
        {
            mElemNo = 0;
            mReqID = -1;
            mCallState = 0;
            mOption = 0;
            EpvElement = 0;
            EpvIndex = 0;
        }

        public TriggerData(BinaryReader br)
        {
            mElemNo = br.ReadInt32();
            mReqID = br.ReadInt32();
            mCallState = br.ReadUInt32();
            mOption = br.ReadUInt32();
            EpvElement = br.ReadUInt32();
            EpvIndex = br.ReadUInt32();
        }

        public void Export(BinaryWriter bw)
        {
            bw.Write(mElemNo);
            bw.Write(mReqID);
            bw.Write(mCallState);
            bw.Write(mOption);
            bw.Write(EpvElement);
            bw.Write(EpvIndex);
        }
    }
    public class EPVSPFile
    {
        uint IceborneMark = 0x18091001;
        char[] magic = { 'E', 'S', 'P', '\x00' };
        uint version = 8;
        uint hashType = 0x45185BED;//currently only two known values, just assume its the one with actual data
        public string WWCTPath { get; set; }//only exists based on hashtype
        public List<RequestData> requestDatas = new List<RequestData>();
        public List<TriggerData> triggerDatas = new List<TriggerData>();

        public EPVSPFile()
        {
            WWCTPath = "";
            requestDatas.Add(new RequestData());
            //no trigger data add because it is rarely used
        }

        public EPVSPFile(BinaryReader br)
        {
            uint IBMark = br.ReadUInt32();
            if(IBMark != IceborneMark)
            {
                MessageBox.Show("Iceborne Mark not present!");
                return;
            }
            char[] fMagic = br.ReadChars(4);
            if(new string(fMagic) != new string(magic))
            {
                MessageBox.Show("Magic bytes incorrect!");
                return;
            }
            uint ver = br.ReadUInt32();
            hashType = br.ReadUInt32();
            if(hashType == 0x45185BED)
            {
                WWCTPath = HelperFunctions.ReadNullTerminatedString(br);
            }
            uint reqDataCount = br.ReadUInt32();
            for(int i = 0; i < reqDataCount; i++)
            {
                requestDatas.Add(new RequestData(br));
            }
            uint tgrDataCount = br.ReadUInt32();
            for (int i = 0; i < tgrDataCount; i++)
            {
                triggerDatas.Add(new TriggerData(br));
            }
        }
        public void Export(BinaryWriter bw)
        {
            bw.Write(IceborneMark);
            bw.Write(magic);
            bw.Write(version);
            if(WWCTPath != null)
            {
                hashType = 0x45185BED;
            }
            bw.Write(hashType);
            if(hashType == 0x45185BED)
            {
                HelperFunctions.WriteNullTerminatedString(bw, WWCTPath);
            }
            bw.Write(requestDatas.Count);
            for(int i = 0; i < requestDatas.Count; i++)
            {
                requestDatas[i].Export(bw);
            }
            bw.Write(triggerDatas.Count);
            for(int i = 0; i < triggerDatas.Count; i++)
            {
                triggerDatas[i].Export(bw);
            }
            bw.Close();
        }
    }
}

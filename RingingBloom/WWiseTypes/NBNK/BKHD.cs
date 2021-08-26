﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RingingBloom.NBNK
{
    /// <summary>
    /// Bank Header
    /// This doesn't change between WWise versions, so it gets a single file
    /// </summary>
    public class BKHD : Chunk
    {
        private char[] magic = new char[] { 'B', 'K', 'H', 'D' };
        private uint sectionLength;
        public uint dwBankGeneratorVersion { get; set; }
        public uint dwSoundbankID { get; set; }
        public uint dwLanguageID { get; set; }
        public uint bFeedbackInBank { get; set; }
        public uint dwProjectID { get; set; }//MHW = 1114
        List<uint> gap = new List<uint>();

        public new char[] dwTag
        {
            get
            {
                return magic;
            }
        }
        public string dwTagStr
        {
            get
            {
                return new string(magic);
            }
        }
        public new uint dwChunkSize { get {
                if (sectionLength >= 20 + (gap.Count * 4)){
                    return sectionLength;
                }
                else
                {
                    sectionLength = (uint)(20 + (gap.Count * 4));
                    return sectionLength;
                }

            }  set => sectionLength = value; }

        public new string Header { get => "Bank Header"; set => throw new NotImplementedException(); }

        //imported constructor, the most common one
        public BKHD(uint SLength, BinaryReader br)
        {
            dwChunkSize = SLength;
            dwBankGeneratorVersion = br.ReadUInt32();
            dwSoundbankID = br.ReadUInt32();
            dwLanguageID = br.ReadUInt32();
            bFeedbackInBank = br.ReadUInt32();
            dwProjectID = br.ReadUInt32();
            uint unknCount = (SLength / 4) - 5;
            for(int i = 0; i < unknCount; i++)
            {
                gap.Add(br.ReadUInt32());
            }
        }

        //created constructor, shouldn't be used normally
        public BKHD()
        {
            dwChunkSize = 24;
            dwBankGeneratorVersion = 120;
            dwSoundbankID = 0; //make sure to implement some form of editing this value
            dwLanguageID = 0;
            bFeedbackInBank = 0;
            dwProjectID = 1114;
            gap.Add(0);
        }

        public override void Export(BinaryWriter bw)
        {
            bw.Write(dwTag);
            bw.Write(dwChunkSize);
            bw.Write(dwBankGeneratorVersion);
            bw.Write(dwSoundbankID);
            bw.Write(dwLanguageID);
            bw.Write(bFeedbackInBank);
            bw.Write(dwProjectID);
            for(int i = 0; i < gap.Count; i++)
            {
                bw.Write(gap[i]);
            }
        }
    }
}

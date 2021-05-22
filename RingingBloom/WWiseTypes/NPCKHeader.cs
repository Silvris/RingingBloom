using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;
using RingingBloom.Common;
using RingingBloom.WWiseTypes;

namespace RingingBloom
{
    public class PCKString
    {
        public string value;
        public uint index;

        public PCKString(BinaryReader br, SupportedGames mode, long start)
        {
            uint offset = br.ReadUInt32();
            index = br.ReadUInt32();
            long retval = br.BaseStream.Position;
            br.BaseStream.Seek(start+offset, SeekOrigin.Begin);
            if(mode == SupportedGames.MHRise)
            {
                value = HelperFunctions.ReadNullTerminatedString(br);
            }
            else
            {
                value = HelperFunctions.ReadUniNullTerminatedString(br);
            }
            br.BaseStream.Seek(retval, SeekOrigin.Begin);
            
        }
        public PCKString(uint Index, string Value)
        {
            index = Index;
            value = Value;
        }
    }
    public class NPCKHeader
    {
        public SupportedGames mode = SupportedGames.None;
        public Labels labels = new Labels();
        public byte[] magic = { (byte)'A', (byte)'K', (byte)'P', (byte)'K' };
        public uint headerLength;
        uint unkn2 = 1;
        uint languageLength;
        uint bnkTableLength = 4;
        public uint wemTableLength;
        uint unknStructLength = 4;//this and bnkTable are not at all interpreted, but they explain the two extra uint 0000 - I assume it's count and they're just 0
        public List<PCKString> pckStrings = new List<PCKString>();
        public List<Wem> WemList = new List<Wem>();

        //created constructor
        public NPCKHeader(SupportedGames Mode)
        {
            mode = Mode;
            pckStrings.Add(new PCKString(0, "sfx"));
        }
        //imported constructor
        public NPCKHeader(BinaryReader br,SupportedGames Mode,string fileName)
        {
            mode = Mode;
            string labelPath = Directory.GetCurrentDirectory() + "/" + mode.ToString() + "/PCK/" + fileName + ".lbl";
            if (File.Exists(labelPath))
            {
                MessageBoxResult labelRead = MessageBox.Show("Label file found. Read labels?", "Labels", MessageBoxButton.YesNo);
                if (labelRead == MessageBoxResult.Yes)
                {
                    labels = new Labels(XmlReader.Create(labelPath));
                }
            }
            char[] magicBytes = br.ReadChars(4);
            uint headerLen = br.ReadUInt32();
            unkn2 = br.ReadUInt32();
            languageLength = br.ReadUInt32();
            bnkTableLength = br.ReadUInt32();
            wemTableLength = br.ReadUInt32();
            unknStructLength = br.ReadUInt32();
            long stringHeaderStart = br.BaseStream.Position;
            uint stringCount = br.ReadUInt32();
            for(int i = 0; i < stringCount; i++)
            {

                PCKString stringData = new PCKString(br, Mode, stringHeaderStart);
                pckStrings.Add(stringData);
            }
            br.BaseStream.Seek(stringHeaderStart + languageLength, SeekOrigin.Begin);
            for (int i = 0; i < bnkTableLength/4; i++)
            {
                br.ReadUInt32();
            }
            //this is 4-aligned at least in RE Engine games
            uint wemACount = br.ReadUInt32();
            for(int i = 0; i < wemACount; i++)
            {
                uint id = br.ReadUInt32();
                uint one = br.ReadUInt32();
                uint length = br.ReadUInt32();
                uint offset = br.ReadUInt32();
                uint languageEnum = br.ReadUInt32();
                int workingOffset = (int)br.BaseStream.Position;
                br.BaseStream.Seek(offset, SeekOrigin.Begin);
                byte[] file = br.ReadBytes((int)length);
                br.BaseStream.Seek(workingOffset, SeekOrigin.Begin);
                string name;
                if (labels.wemLabels.ContainsKey(id))
                {
                    name = labels.wemLabels[id];
                }
                else
                {
                    name = "Imported Wem " + i;
                }
                Wem newWem = new Wem(name, id, file, languageEnum);
                WemList.Add(newWem);
            }
            //the unknStruct uint is right here, but we've already read what we need
        }

        public List<string> GetLanguages()
        {
            List<string> langList = new List<string>();
            for(int i = 0; i < pckStrings.Count; i++)
            {
                try
                {
                    langList.Insert((int)pckStrings[i].index, pckStrings[i].value);
                }
                catch (ArgumentOutOfRangeException)
                {
                    langList.Add(pckStrings[i].value);//thought is that should order be 2 0 1, the 2 is added, then 0 is inserted in front of it and then 1 is inserted in front of it
                }
            }
            return langList;
        }

        public List<byte> GenerateLanguageBytes(SupportedGames mode)
        {
            List<byte> languageBytes = new List<byte>();

            byte[] count = BitConverter.GetBytes(pckStrings.Count);
            for(int i = 0; i < count.Length; i++)
            {
                languageBytes.Add(count[i]);
            }
            //get "header" size for use later
            int headerSize = 4 + (pckStrings.Count * 8);
            //now generate string table and offsets
            List<byte> stringTable = new List<byte>();
            int[] offsets = new int[pckStrings.Count];
            List<string> strings = new List<string>();
            for(int i = 0; i < pckStrings.Count; i++)
            {
                strings.Add(pckStrings[i].value);
            }
            strings.Sort();//alphabetically
            for (int i = 0; i < strings.Count; i++)
            {
                byte[] asBytes;
                if (mode == SupportedGames.MHRise)
                {
                    asBytes = Encoding.UTF8.GetBytes(strings[i]);
                }
                else
                {
                    asBytes = Encoding.Unicode.GetBytes(strings[i]);
                }
                    
                for (int j = 0; j < pckStrings.Count; j++)
                {
                    if(pckStrings[j].value == strings[i])
                    {
                        offsets[j] = Math.Max(0, headerSize+stringTable.Count);
                        for(int k = 0;k < asBytes.Length; k++)
                        {
                            stringTable.Add(asBytes[k]);
                        }
                        if(mode == SupportedGames.MHRise)
                        {
                            stringTable.Add(0);
                        }
                        else
                        {
                            stringTable.Add(0);
                            stringTable.Add(0);
                        }
                    }
                }
            }
            //finally add pckString data to languageBytes
            for(int i = 0; i < pckStrings.Count; i++)
            {
                byte[] offset = BitConverter.GetBytes(offsets[i]);
                for(int j = 0; j < offset.Length; j++)
                {
                    languageBytes.Add(offset[j]);
                }
                byte[] index = BitConverter.GetBytes(pckStrings[i].index);
                for(int j = 0; j < index.Length; j++)
                {
                    languageBytes.Add(index[j]);
                }
            }
            //concat string table
            for(int i =0; i < stringTable.Count; i++)
            {
                languageBytes.Add(stringTable[i]);
            }
            //align 4
            while(languageBytes.Count%4 != 0)
            {
                languageBytes.Add(0);
            }

            return languageBytes;
        }

        public void ExportHeader(string aFilePath)
        {
            //TODO: rewrite this in a way that lets me use it for nonstream and stream
            wemTableLength = (uint)(WemList.Count * 20) + 4;
            List<byte> languageBytes = GenerateLanguageBytes(mode);
            headerLength = (uint)(wemTableLength + 20 + languageBytes.Count + bnkTableLength + unknStructLength);
            BinaryWriter bw = new BinaryWriter(File.Create(aFilePath));
            bw.Write(magic);
            bw.Write(headerLength);
            bw.Write(unkn2);
            bw.Write(languageBytes.Count);
            bw.Write(bnkTableLength);//const in Capcom pcks
            bw.Write(wemTableLength);
            bw.Write(unknStructLength);//const in Capcom pcks
            for(int i = 0; i < languageBytes.Count; i++)
            {
                bw.Write(languageBytes[i]);
            }
            bw.Write((int)0);
            bw.Write(WemList.Count);
            uint currentOffset = headerLength+8;//the +8 is because magic and header length are not included in header length
            foreach (Wem wem in WemList)
            {
                bw.Write(wem.id);
                bw.Write(1);
                bw.Write(wem.length);
                bw.Write(currentOffset);
                currentOffset += wem.length;
                bw.Write(wem.languageEnum);
            }
            bw.Write((int)0);
            bw.Close();
        }

        public void ExportFile(string aFilePath)
        {
            wemTableLength = (uint)(WemList.Count * 20)+4;
            List<byte> languageBytes = GenerateLanguageBytes(mode);
            headerLength = (uint)(wemTableLength + 20 + languageBytes.Count + bnkTableLength + unknStructLength);
            BinaryWriter bw = new BinaryWriter(File.Create(aFilePath));
            bw.Write(magic);
            bw.Write(headerLength);
            bw.Write(unkn2);
            bw.Write(languageBytes.Count);
            bw.Write(bnkTableLength);//const in Capcom pcks
            bw.Write(wemTableLength);
            bw.Write(unknStructLength);//const in Capcom pcks
            for (int i = 0; i < languageBytes.Count; i++)
            {
                bw.Write(languageBytes[i]);
            }
            bw.Write((int)0);
            bw.Write(WemList.Count);
            uint currentOffset = headerLength + 8;
            //later, may need to separate into an "offsets" list and write files afterwards rather than immediately
            foreach (Wem wem in WemList)
            {
                bw.Write(wem.id);
                bw.Write(1);
                bw.Write(wem.length);
                bw.Write(currentOffset);
                int workingOffset = (int)bw.BaseStream.Position;
                bw.Seek((int)currentOffset, SeekOrigin.Begin);
                bw.Write(wem.file);
                bw.Seek(workingOffset,SeekOrigin.Begin);
                currentOffset += wem.length;
                bw.Write(wem.languageEnum);
            }
            bw.Close();
        }
    }
}

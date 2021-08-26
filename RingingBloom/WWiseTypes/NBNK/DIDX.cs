using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using RingingBloom.Common;
using RingingBloom.WWiseTypes;

namespace RingingBloom.NBNK
{
    public class DIDX : Chunk
    {
        //this includes data chunk as well, because no point in separating them
        private char[] magic = new char[] { 'D', 'I', 'D', 'X' };
        private char[] DATA = new char[] { 'D', 'A', 'T', 'A' };

        private int pLoadedMedia { get => Items.Count; }
        public int didxSize { get => Items.Count * 12; }
        public int dataSize { get
            {
                int size = 0;
                for(int i = 0; i < Items.Count; i++)
                {
                    size += (int)Items[i].length;
                }
                return size;
            } }

        public DIDX(BinaryReader br, Labels labels)
        {
            Items = new ObservableCollection<Wem>();
            uint didxLength = br.ReadUInt32();
            uint wemCount = didxLength / 12;
            uint[] ids = new uint[wemCount];
            uint[] offsets = new uint[wemCount];
            uint[] lengths = new uint[wemCount];
            for (int i = 0; i < wemCount; i++)
            {
                ids[i] = br.ReadUInt32();
                offsets[i] = br.ReadUInt32();
                lengths[i] = br.ReadUInt32();
            }
            char[] dataRead = br.ReadChars(4);
            if(new string(dataRead) != "DATA")
            {
                MessageBox.Show("Error reading DATA section");
            }
            uint dataLength = br.ReadUInt32();
            long DataOff = br.BaseStream.Position;
            List<byte[]> wemDatas = new List<byte[]>();
            for (int i = 0; i < wemCount; i++)
            {
                br.BaseStream.Seek(DataOff + offsets[i], SeekOrigin.Begin);
                wemDatas.Add(br.ReadBytes((int)lengths[i]));
            }
            for (int i = 0; i < wemCount; i++)
            {
                string name = "Imported Wem " + i;
                if (labels.wemLabels.ContainsKey(ids[i]))
                {
                    name = labels.wemLabels[ids[i]];
                }
                Wem newWem = new Wem(name, ids[i], wemDatas[i]);
                Items.Add(newWem);
            }
            br.BaseStream.Seek(DataOff+dataLength,SeekOrigin.Begin);
            OnPropertyChanged("Items");
        }
        public new char[] dwTag { get => magic;}

        public new uint dwChunkSize { get => (uint)didxSize; set => throw new NotImplementedException(); }//not using dwChunkSize for this one since we have a combo
        public new string Header { get => "Data Index"; set => throw new NotImplementedException(); }
        public ObservableCollection<Wem> Items { get; set; }

        public void AddWem(string aName, uint aId, BinaryReader br)
        {
            Wem newWem = new Wem(aName, Convert.ToString(aId), br);
            Items.Add(newWem);
            OnPropertyChanged("Items");
        }
        public override void Export(BinaryWriter bw)
        {
            bw.Write(dwTag);
            bw.Write(didxSize);
            uint currentOffset = 0;
            for (int i = 0; i < pLoadedMedia; i++)
            {
                bw.Write(Items[i].id);
                bw.Write(currentOffset);
                bw.Write(Items[i].length);
                currentOffset += Items[i].length;
            }
            bw.Write(DATA);
            bw.Write(currentOffset);
            for (int i = 0; i < pLoadedMedia; i++)
            {
                bw.Write(Items[i].file);
            }
        }
    }
}

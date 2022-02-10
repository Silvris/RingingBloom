using RingingBloom.Common;
using RingingBloom.NBNK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace RingingBloom.WWiseTypes.ViewModels
{
    public class NBNKViewModel : BaseViewModel
    {
        public NBNKFile nbnk { get; set; }
        public ObservableCollection<Chunk> Chunks 
        {
            get 
            {
                return new ObservableCollection<Chunk>(nbnk.GetChunks());


            }        
        }
        public void SetNBNK(BinaryReader br, SupportedGames mode)
        {
            nbnk.ReadFile(br,mode);
            OnPropertyChanged("Chunks");
        }
        public void ExportNBNK(BinaryWriter bw)
        {
            nbnk.ExportNBNK(bw);
        }
        public DIDX GetDIDX()
        {
            return nbnk.DataIndex;
        } 
        public void AddWem(string aName, uint aID, BinaryReader br)
        {
            nbnk.DataIndex.AddWem(aName, aID, br);
            OnPropertyChanged("Chunks");
        }
        public void ExportLabels(string name, List<Wem> wems, List<uint> ids)
        {
            nbnk.labels.Export(name, wems, ids);
        }
        public HIRC GetHIRC()
        {
            return nbnk.ObjectHierarchy;
        }
    }
}

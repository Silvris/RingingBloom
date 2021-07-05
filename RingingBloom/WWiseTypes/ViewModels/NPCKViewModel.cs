using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using RingingBloom.Common;
using RingingBloom.Windows;

namespace RingingBloom
{
    public class NPCKViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public NPCKHeader npck = null;//shouldn't have to bind directly to this

        public ObservableCollection<Wem> wems { get { return new ObservableCollection<Wem>(npck.WemList); } set { npck.WemList = new List<Wem>(value); OnPropertyChanged("wems"); } }
        public ObservableCollection<string> languages { get { return new ObservableCollection<string>(npck.GetLanguages()); } set => throw new NotImplementedException(); }

        public NPCKViewModel(SupportedGames mode)
        {
            npck = new NPCKHeader(mode);
        }

        public void SetNPCK(NPCKHeader file)
        {
            npck = file;
            OnPropertyChanged("wems");
            OnPropertyChanged("languages");
        }

        public void AddWems(string[] fileNames)
        {
            foreach (string fileName in fileNames)
            {
                Wem newWem = HelperFunctions.MakeWems(fileName, new BinaryReader(File.Open(fileName, FileMode.Open)));
                npck.WemList.Add(newWem);
            }
            OnPropertyChanged("wems");
        }

        public void ReplaceWem(Wem newWem,int index)
        {
            newWem.id = npck.WemList[index].id;
            newWem.languageEnum = npck.WemList[index].languageEnum;
            npck.WemList[index] = newWem;
            OnPropertyChanged("wems");
        }
        public void DeleteWem(int index)
        {
            npck.WemList.RemoveAt(index);
            OnPropertyChanged("wems");
        }

        public void ExportWems(MessageBoxResult exportIds, string savePath)
        {
            foreach (Wem newWem in npck.WemList)
            {
                string name;
                if (exportIds == MessageBoxResult.Yes)
                {
                    name = savePath + "\\" + newWem.name + ".wem";
                }
                else
                {
                    name = savePath + "\\" + newWem.id + ".wem";
                }
                BinaryWriter bw = new BinaryWriter(new FileStream(name, FileMode.OpenOrCreate));
                bw.Write(newWem.file);
                bw.Close();
            }
        }

        public void ExportNPCK(string fileName, SupportedGames mode)
        {
            npck.ExportFile(fileName);
            if (mode == SupportedGames.RE2DMC5 || mode == SupportedGames.RE3R || mode == SupportedGames.MHRise || mode == SupportedGames.RE8)
            {
                npck.ExportHeader(fileName + ".nonstream");
            }
        }

        public void IDReplace(string[] id2)
        {
            for (int i = 0; i < id2.Length; i++)
            {
                try
                {
                    npck.WemList[i].id = Convert.ToUInt32(id2[i]);
                }
                catch (ArgumentOutOfRangeException)
                {
                    break;
                }
            }
            OnPropertyChanged("wems");
        }

        public void ExportLabels(SupportedGames mode, string currentFileName, List<uint> changedIds)
        {
            npck.labels.Export(Directory.GetCurrentDirectory() + "/" + mode.ToString() + "/PCK/" + currentFileName + ".lbl", npck.WemList, changedIds);
        }

        public List<uint> GetWemIds()
        {
            List<uint> wemIds = new List<uint>();
            for (int i = 0; i < npck.WemList.Count; i++)
            {
                wemIds.Add(npck.WemList[i].id);
            }
            return wemIds;
        }

        public void MassReplace(List<ReplacingWem> mass)
        {
            for (int i = 0; i < mass.Count; i++)
            {
                int index = npck.WemList.FindIndex(x => x.id == mass[i].replacingId);
                Wem newWem = mass[i].wem;
                if (index != -1)
                {
                    newWem.id = npck.WemList[index].id;
                    newWem.languageEnum = npck.WemList[index].languageEnum;
                    npck.WemList[index] = newWem;
                }
            }
            OnPropertyChanged("Wems");
        }
    }
}

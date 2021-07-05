using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RingingBloom.Common;

namespace RingingBloom.WWiseTypes.ViewModels
{
    public class WWCTViewModel : BaseViewModel
    {
        public WWCTFile wwct { get; set; }
        public ObservableCollection<WWCTString> entries 
        {
            get 
            { 
                if(wwct == null)
                {
                    return new ObservableCollection<WWCTString>();
                }
                else
                {
                    return new ObservableCollection<WWCTString>(wwct.wwctStrings);
                }
            }
            set 
            {
                wwct.wwctStrings = new List<WWCTString>(value);
                OnPropertyChanged("entries");
            }
        }

        public WWCTViewModel()
        {
            wwct = null;
        }

        public void SetWWCT(WWCTFile CT)
        {
            wwct = CT;
            OnPropertyChanged("entries");
        }

        public void AddString(WWCTString newStr)
        {
            wwct.wwctStrings.Add(newStr);
            OnPropertyChanged("entries");
        }

        public void AddNonDuplicate(WWCTFile import)
        {
            for (int i = 0; i < import.wwctStrings.Count; i++)
            {
                bool isDuplicate = false;
                for (int j = 0; j < wwct.wwctStrings.Count; j++)
                {
                    if (!isDuplicate)
                    {
                        isDuplicate = import.CompareWWCTString(import.wwctStrings[i], wwct.wwctStrings[j]);
                    }
                }
                if (!isDuplicate)
                {
                    wwct.wwctStrings.Add(import.wwctStrings[i]);
                }
            }
            OnPropertyChanged("entries");
        }

        public void RemoveString(int index)
        {
            wwct.wwctStrings.RemoveAt(index);
            OnPropertyChanged("entries");
        }

        public void Export(BinaryWriter exportFile)
        {
            wwct.ExportFile(exportFile);
        }

    }
}

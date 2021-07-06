using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingingBloom.WWiseTypes.ViewModels
{
    public class WWPKBKViewModel : BaseViewModel
    {
        public WWPKBKFile wwpkbk { get; set; }
        public ObservableCollection<PKBKString> pkbk
        {
            get
            {
                if (wwpkbk == null)
                {
                    return new ObservableCollection<PKBKString>();
                }
                else
                {
                    return new ObservableCollection<PKBKString>(wwpkbk.paths);
                }
            }
            set
            {
                wwpkbk.paths = new List<PKBKString>(value);
                OnPropertyChanged("pkbk");
            }
        }

        public WWPKBKViewModel()
        {
            wwpkbk = null;
        }

        public void SetWWPKBK(WWPKBKFile file)
        {
            wwpkbk = file;
        }
        public void AddPath(PKBKString path)
        {
            wwpkbk.paths.Add(path);
            OnPropertyChanged("pkbk");
        }
        public void RemovePath(int index)
        {
            wwpkbk.paths.RemoveAt(index);
        }
        public void Export(string type,BinaryWriter bw)
        {
            wwpkbk.ExportFile(type,bw);
        }
    }
}

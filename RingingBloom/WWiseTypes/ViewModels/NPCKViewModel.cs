using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RingingBloom.Common;

namespace RingingBloom
{
    public class NPCKViewModel
    {
        public ObservableCollection<Wem> wems;
        public ObservableCollection<string> languages { get; set; }

        public NPCKViewModel(List<Wem> npck, List<string> lang)
        {
            wems = new ObservableCollection<Wem>(npck);
            languages = new ObservableCollection<string>(lang);
        }

        public NPCKViewModel()
        {
            wems = new ObservableCollection<Wem>();
            languages = new ObservableCollection<string>();
            languages.Add("sfx");
        }

    }
}

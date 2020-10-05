using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RingingBloom.WWiseTypes.ViewModels
{
    public class WWPKBKViewModel
    {
        public ObservableCollection<PKBKString> pkbk;

        public WWPKBKViewModel()
        {
            pkbk = new ObservableCollection<PKBKString>();
        }
    }
}

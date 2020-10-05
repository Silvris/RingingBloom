using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RingingBloom.Common;

namespace RingingBloom.WWiseTypes.ViewModels
{
    public class WWCTViewModel
    {
        public ObservableCollection<WWCTString> wwct;

        public WWCTViewModel()
        {
            wwct = new ObservableCollection<WWCTString>();
        }

    }
}

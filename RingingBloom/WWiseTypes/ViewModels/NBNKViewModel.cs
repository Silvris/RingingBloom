using RingingBloom.NBNK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RingingBloom.WWiseTypes.ViewModels
{
    class NBNKViewModel : BaseViewModel
    {
        public NBNKFile nbnk { get; set; }
        public ObservableCollection<Chunk> Chunks 
        {
            get 
            {
                ObservableCollection<Chunk> chunks = new ObservableCollection<Chunk>();

            }        
        }
    }
}

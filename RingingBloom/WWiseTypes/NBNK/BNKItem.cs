using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace RingingBloom.NBNK
{
    public abstract class BNKItem : INotifyPropertyChanged
    {
        //they all need Items too, but they all need items of differing types, so it's left out of here
        private string _Header;
        public string Header { get => _Header; set { _Header = value; OnPropertyChanged("Header"); } }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

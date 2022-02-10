﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace RingingBloom.WWiseTypes.NBNK.HIRC
{
    //RTPC = Real Time Parameter Control, responsible for half of the crazy shit WWise does
    public class AkRTPCGraphPoint : INotifyPropertyChanged
    {
        private float _To;
        private float _From;
        private uint _Interpolation;

        public event PropertyChangedEventHandler PropertyChanged;

        public float To { get => _To; set { _To = value; OnPropertyChanged("To"); } }
        public float From { get; set; }
        public uint Interpolation { get; set; }//might be an enum? gonna look later
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
        public AkRTPCGraphPoint(BinaryReader br)
        {
            To = br.ReadSingle();
            From = br.ReadSingle();
            Interpolation = br.ReadUInt32();
        }
    }
}
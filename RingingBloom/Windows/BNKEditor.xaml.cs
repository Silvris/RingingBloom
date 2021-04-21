using Microsoft.Win32;
using RingingBloom.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RingingBloom.Windows
{
    /// <summary>
    /// Interaction logic for BNKEditor.xaml
    /// </summary>
    public partial class BNKEditor : Window
    {
        SupportedGames mode = SupportedGames.MHWorld;
        NBNKFile nbnk = null;
        public BNKEditor(SupportedGames Mode)
        {
            InitializeComponent();
            mode = Mode;
        }

        public void ImportBNK(object sender, RoutedEventArgs e)
        {
            OpenFileDialog importFile = new OpenFileDialog();
            importFile.Multiselect = false;
            importFile.Filter = "WWise Soundbank file (*.bnk)|*.bnk";
            switch (mode)
            {
                case SupportedGames.MHWorld:
                    importFile.Filter += "|Monster Hunter World WWise Soundbank (*.nbnk)|*.nbnk";
                    break;
                case SupportedGames.MHRise:
                    importFile.Filter += "|Monster Hunter Rise Switch WWise Soundbank (*.nsw)|*.nsw";
                    importFile.Filter += "|Monster Hunter Rise English WWise Soundbank (*.En)|*.En";
                    importFile.Filter += "|Monster Hunter Rise Japanese WWise Soundbank (*.Ja)|*.Ja";
                    importFile.Filter += "|Monster Hunter Rise Fictional WWise Soundbank (*.Fc)|*.Fc";
                    break;
                case SupportedGames.DMC5:
                    importFile.Filter += "|Devil May Cry 5 WWise Soundbank (*.x64)|*.x64";
                    importFile.Filter += "|Devil May Cry 5 English WWise Soundbank (*.En)|*.En";
                    importFile.Filter += "|Devil May Cry 5 Japanese WWise Soundbank (*.Ja)|*.Ja";
                    break;
                default:
                    break;
            }
            if (importFile.ShowDialog() == true)
            {
                BinaryReader readFile = new BinaryReader(new FileStream(importFile.FileName, FileMode.Open), Encoding.ASCII);
                nbnk = new NBNKFile(readFile, mode);
                readFile.Close();
            }
        }
        public void ExportBNK(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "WWise Package file (*.pck)|*.pck";
            switch (mode)
            {
                case SupportedGames.MHWorld:
                    saveFile.Filter += "|Monster Hunter World WWise Soundbank (*.nbnk)|*.nbnk";
                    break;
                case SupportedGames.MHRise:
                    saveFile.Filter += "|Monster Hunter Rise Switch WWise Soundbank (*.nsw)|*.nsw";
                    saveFile.Filter += "|Monster Hunter Rise English WWise Soundbank (*.En)|*.En";
                    saveFile.Filter += "|Monster Hunter Rise Japanese WWise Soundbank (*.Ja)|*.Ja";
                    saveFile.Filter += "|Monster Hunter Rise Fictional WWise Soundbank (*.Fc)|*.Fc";
                    break;
                case SupportedGames.DMC5:
                    saveFile.Filter += "|Devil May Cry 5 WWise Soundbank (*.x64)|*.x64";
                    saveFile.Filter += "|Devil May Cry 5 English WWise Soundbank (*.En)|*.En";
                    saveFile.Filter += "|Devil May Cry 5 Japanese WWise Soundbank (*.Ja)|*.Ja";
                    break;
                default:
                    break;
            }
            if (saveFile.ShowDialog() == true)
            {
                nbnk.ExportNBNK(new BinaryWriter(new FileStream(saveFile.FileName,FileMode.OpenOrCreate)));
            }
        }
        
        public void treeView1_SelectedItemChanged(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

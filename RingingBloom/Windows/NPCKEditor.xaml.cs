using Microsoft.Win32;
using RingingBloom.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for NPCKEditor.xaml
    /// </summary>
    public partial class NPCKEditor : Window
    {
        public static NPCKHeader npck = null;
        public NPCKViewModel viewModel = new NPCKViewModel();

        public NPCKEditor()
        {
            InitializeComponent();
            Window.Background = HelperFunctions.GetBrushFromHex("#505050");
            WemView.Background = HelperFunctions.GetBrushFromHex("#282828");
            WemView.ItemsSource = viewModel.wems;
        }

        private void Import_Wems(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = true;
            openFile.Filter = "WWise Wem files (*.wem)|*.wem";
            if (openFile.ShowDialog() == true)
            {
                foreach (string fileName in openFile.FileNames)
                {
                    Wem newWem = HelperFunctions.MakeWems(fileName, new BinaryReader(File.Open(fileName, FileMode.Open)));
                    npck.WemList.Add(newWem);
                    viewModel.wems.Add(newWem);
                }

            }


        }

        private void Export_Wems(object sender, RoutedEventArgs e)
        {
            OpenFileDialog exportFile = new OpenFileDialog();
            exportFile.CheckFileExists = false;
            exportFile.FileName = "Save Here";
            if (exportFile.ShowDialog() == true)
            {
                string fullPath = exportFile.FileName;
                string savePath = System.IO.Path.GetDirectoryName(fullPath);
                foreach (Wem newWem in npck.WemList)
                {
                    BinaryWriter bw = new BinaryWriter(new FileStream(savePath + "\\" + newWem.id + ".wem",FileMode.OpenOrCreate));
                    bw.Write(newWem.file);
                    bw.Close();
                }

            }


        }

        private void Delete_Wem(object sender, RoutedEventArgs e)
        {
            try
            {
                npck.WemList.RemoveAt(WemView.Items.IndexOf(WemView.SelectedItem));
                viewModel.wems.RemoveAt(WemView.Items.IndexOf(WemView.SelectedItem));
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("File not Loaded!");
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("No entry selected!");
            }
        }


        private void MakeNPCK(object sender, RoutedEventArgs e)
        {
            npck = new NPCKHeader();
            viewModel.wems.Clear();
            Import_Wems(sender, e);
        }

        private void ImportNPCK(object sender, RoutedEventArgs e)
        {
            OpenFileDialog importFile = new OpenFileDialog();
            importFile.Multiselect = false;
            importFile.Filter = "WWise Package file (*.npck, *.pck)|*.npck;*.pck";
            if (importFile.ShowDialog() == true)
            {
                viewModel.wems.Clear();
                BinaryReader readFile = new BinaryReader(new FileStream(importFile.FileName, FileMode.Open), Encoding.ASCII);
                npck = new NPCKHeader(readFile);
                for (int i = 0; i < npck.WemList.Count; i++)
                {
                    viewModel.wems.Add(npck.WemList[i]);
                }
                readFile.Close();
            }
        }

        private void ExportNPCK(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "WWise Package file (*.npck, *.pck)|*.npck;*.pck";
            if (saveFile.ShowDialog() == true)
            {
                npck.ExportFile(saveFile.FileName);
            }


        }

        private void HelpMenu(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("NPCK Editor: for editing WWise Package files.\n\n" +
                "[New] - Makes a new NPCK and prompts the user to import wems.\n" +
                "[Import] - Imports an NPCK/PCK of the user's choosing.\n" +
                "[Export] - Exports the currently open npck into a new file.\n" +
                "[Import Wems] - Imports the chosen wems into the currently opened NPCK/PCK.\n" +
                "[Export Wems] - Exports the wems from the currently open npck into a folder.\n" +
                "[Delete Wem] - Deletes the currently selected wem.");
        }

    }
}

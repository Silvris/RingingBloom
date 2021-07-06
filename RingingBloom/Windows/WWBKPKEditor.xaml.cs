using Microsoft.Win32;
using RingingBloom.WWiseTypes;
using RingingBloom.WWiseTypes.ViewModels;
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
    /// Interaction logic for WWBKPKEditor.xaml
    /// </summary>
    public partial class WWBKPKEditor : Window
    {
        public WWPKBKViewModel viewModel = new WWPKBKViewModel();
        private string ImportPath = null;
        private string ExportPath = null;
        public WWBKPKEditor(Options options)
        {
            InitializeComponent();
            PKBKView.ItemsSource = viewModel.pkbk;
            if (options.defaultImport != null)
            {
                ImportPath = options.defaultImport;
            }
            if (options.defaultExport != null)
            {
                ExportPath = options.defaultExport;
            }
        }

        public void MakeWWPKBK(object sender, RoutedEventArgs e)
        {
            viewModel.SetWWPKBK(new WWPKBKFile());
            PKBKString strin = new PKBKString("");
            viewModel.AddPath(strin);
        }
        public void ImportWWPKBK(object sender, RoutedEventArgs e)
        {
            OpenFileDialog importFile = new OpenFileDialog();
            if(ImportPath != null)
            {
                importFile.InitialDirectory = ImportPath;
            }
            importFile.Multiselect = false;
            importFile.Filter = "All readable files|*.wwbk;*.wwpk|WWise Soundbank Container files (*.wwbk)|*.wwbk|WWise Package Container files (*.wwpk)|*.wwpk";
            if (importFile.ShowDialog() == true)
            {
                BinaryReader readFile = HelperFunctions.OpenFile(importFile.FileName);
                viewModel.SetWWPKBK(new WWPKBKFile(readFile));
                readFile.Close();
            }


        }
        public void ExportWWPKBK(object sender, RoutedEventArgs e)
        {
            if(viewModel.wwpkbk == null)
            {
                MessageBox.Show("WWPK/WWBK file not currently loaded.");
                return;
            }
            SaveFileDialog saveFile = new SaveFileDialog();
            if (ExportPath != null)
            {
                saveFile.InitialDirectory = ExportPath;
            }
            saveFile.Filter = "WWise Soundbank Container files (*.wwbk)|*.wwbk|WWise Package Container files (*.wwpk)|*.wwpk";
            if (saveFile.ShowDialog() == true)
            {
                BinaryWriter exportFile = new BinaryWriter(new FileStream(saveFile.FileName, FileMode.OpenOrCreate));
                viewModel.Export(saveFile.FileName.Substring(saveFile.FileName.Length-4,4),exportFile);
                exportFile.Close();
            }
        }

        private void AddEntry(object sender, RoutedEventArgs e)
        {
            if(viewModel.wwpkbk != null)
            {
                PKBKString strin = new PKBKString("");
                viewModel.AddPath(strin);
            }
            else
            {
                MessageBox.Show("File not Loaded!");
            }
        }
        private void DeleteEntry(object sender, RoutedEventArgs e)
        {
            try
            {
                if (viewModel.wwpkbk != null)
                {
                    viewModel.RemovePath(PKBKView.SelectedIndex);
                }
                else
                {
                    MessageBox.Show("File not Loaded!");
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("No entry selected!");
            }
        }

        private void HelpMenu(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("WWBK/WWPK Editor: for editing WWise Package Container files and WWise Soundbank Container files\n\n" +
                "[New] - Creates a new file, type will be determined on export\n" +
                "[Import] - Imports a pre-existing WWBK/WWPK\n" +
                "[Export] - Exports the currently opened file, make sure to specify which type you want to export as.\n" +
                "[Add Entry] - Adds a new entry into the opened file.\n" +
                "[Delete Entry] - Removes the currently selected entry from the file");
        }
    }
}

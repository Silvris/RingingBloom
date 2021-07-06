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
using Microsoft.Win32;
using RingingBloom;
using RingingBloom.WWiseTypes;
using RingingBloom.WWiseTypes.ViewModels;

namespace RingingBloom.Windows
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class WWCTEditor : Window
    {
        public WWCTViewModel viewModel = new WWCTViewModel();
        private string ImportPath = null;
        private string ExportPath = null;
        public WWCTEditor(Options options)
        {
            InitializeComponent();
            WWCTView.DataContext = viewModel;
            if (options.defaultImport != null)
            {
                ImportPath = options.defaultImport;
            }
            if (options.defaultExport != null)
            {
                ExportPath = options.defaultExport;
            }
        }

        public void MakeWWCT(object sender, RoutedEventArgs e)
        {
            viewModel.SetWWCT(new WWCTFile());
            WWCTString strin = new WWCTString(Common.WWCTType.WWEV, "", 0);
            viewModel.AddString(strin);
        }
        public void ImportWWCT(object sender, RoutedEventArgs e)
        {
            OpenFileDialog importFile = new OpenFileDialog();
            if (ImportPath != null)
            {
                importFile.InitialDirectory = ImportPath;
            }
            importFile.Multiselect = false;
            importFile.Filter = "WWise Container files (*.wwct)|*.wwct";
            if(importFile.ShowDialog() == true)
            {
                BinaryReader readFile = HelperFunctions.OpenFile(importFile.FileName);
                viewModel.SetWWCT(new WWCTFile(readFile));
                readFile.Close();
            }
            

        }

        public void ImportNonDuplicate(object sender, RoutedEventArgs e)
        {
            if (viewModel.wwct != null) {
                OpenFileDialog importFile = new OpenFileDialog();
                if (ImportPath != null)
                {
                    importFile.InitialDirectory = ImportPath;
                }
                importFile.Multiselect = false;
                importFile.Filter = "WWise Container files (*.wwct)|*.wwct";
                WWCTFile import;
                List<WWCTString> newStrings = new List<WWCTString>();
                if (importFile.ShowDialog() == true)
                {
                    BinaryReader readFile = HelperFunctions.OpenFile(importFile.FileName);
                    import = new WWCTFile(readFile);
                    //look for non-duplicates
                    viewModel.AddNonDuplicate(import);
                    readFile.Close();
                }
            }
            else
            {
                ImportWWCT(sender, e);
            }
        }

        public void ExportWWCT(object sender, RoutedEventArgs e)
        {
            if(viewModel.wwct == null)
            {
                MessageBox.Show("WWCT not currently loaded.");
                return;
            }
            WWCTView.Focus();
            SaveFileDialog saveFile = new SaveFileDialog();
            if (ExportPath != null)
            {
                saveFile.InitialDirectory = ExportPath;
            }
            saveFile.Filter = "WWise Container files (*.wwct)|*.wwct";
            if (saveFile.ShowDialog() == true)
            {
                BinaryWriter exportFile = new BinaryWriter(new FileStream(saveFile.FileName, FileMode.OpenOrCreate));
                viewModel.Export(exportFile);
                exportFile.Close();
            }
        }

        private void AddEntry(object sender, RoutedEventArgs e)
        {
            try
            {
                WWCTString strin = new WWCTString(Common.WWCTType.WWEV, "", 0);
                viewModel.AddString(strin);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("WWCT File not Loaded!");
            }
        }
        private void DeleteEntry(object sender, RoutedEventArgs e)
        {
            try
            {
                viewModel.RemoveString(WWCTView.SelectedIndex);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("WWCT File not Loaded!");
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("No entry selected!");
            }
        }
    }
}

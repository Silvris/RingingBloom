using Microsoft.Win32;
using RingingBloom.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        public SupportedGames mode = SupportedGames.MHWorld;
        public static NPCKHeader npck = null;
        public NPCKViewModel viewModel = new NPCKViewModel();
        private string ImportPath = null;
        private string ExportPath = null;
        private string currentFileName = null;
        private bool LabelsChanged = false;
        public List<uint> changedIds = new List<uint>();

        public NPCKEditor(SupportedGames Mode,Options options)
        {
            InitializeComponent();
            mode = Mode;
            WemView.ItemsSource = viewModel.wems;
            if(options.defaultImport != null)
            {
                ImportPath = options.defaultImport;
            }
            if (options.defaultExport != null)
            {
                ExportPath = options.defaultExport;
            }
        }

        private void Import_Wems(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if(ImportPath != null)
            {
                openFile.InitialDirectory = ImportPath;
            }
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

        private void Replace_Wem(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (ImportPath != null)
            {
                openFile.InitialDirectory = ImportPath;
            }
            openFile.Multiselect = false;
            openFile.Filter = "WWise Wem files (*.wem)|*.wem";
            if (openFile.ShowDialog() == true)
            {
                foreach (string fileName in openFile.FileNames)
                {
                    Wem newWem = HelperFunctions.MakeWems(fileName, new BinaryReader(File.Open(fileName, FileMode.Open)));
                    newWem.id = npck.WemList[WemView.SelectedIndex].id;
                    newWem.languageBool = npck.WemList[WemView.SelectedIndex].languageBool;
                    npck.WemList[WemView.SelectedIndex] = newWem;
                    viewModel.wems[WemView.SelectedIndex] = newWem;
                    WemView.ItemsSource = viewModel.wems;
                }

            }
        }

        private void Export_Wems(object sender, RoutedEventArgs e)
        {
            OpenFileDialog exportFile = new OpenFileDialog();
            if (ExportPath != null)
            {
                exportFile.InitialDirectory = ExportPath;
            }
            exportFile.CheckFileExists = false;
            exportFile.FileName = "Save Here";
            if (exportFile.ShowDialog() == true)
            {
                string fullPath = exportFile.FileName;
                string savePath = System.IO.Path.GetDirectoryName(fullPath);
                MessageBoxResult exportIds = MessageBox.Show("Export with names?", "Export", MessageBoxButton.YesNo);
                foreach (Wem newWem in npck.WemList)
                {
                    string name;
                    if (exportIds == MessageBoxResult.Yes)
                    {
                        name = savePath + "\\" + newWem.name + ".wem";
                    }
                    else
                    {
                        name = savePath + "\\" + newWem.id + ".wem";
                    }
                    BinaryWriter bw = new BinaryWriter(new FileStream(name, FileMode.OpenOrCreate));
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
            SaveLabels(sender, new CancelEventArgs());
            npck = new NPCKHeader(mode);
            viewModel.wems.Clear();
            Import_Wems(sender, e);
        }

        private void ImportNPCK(object sender, RoutedEventArgs e)
        {
            SaveLabels(sender, new CancelEventArgs());
            OpenFileDialog importFile = new OpenFileDialog();
            if (ImportPath != null)
            {
                importFile.InitialDirectory = ImportPath;
            }
            importFile.Multiselect = false;
            importFile.Filter = "WWise Package file (*.pck)|*.pck";
            switch (mode)
            {
                case SupportedGames.MHWorld:
                    importFile.Filter += "|Monster Hunter World WWise Package (*.npck)|*.npck";
                    importFile.Filter = "All supported files (*.pck,*.npck)|*.pck;*.npck|" + importFile.Filter;
                    break;
                case SupportedGames.MHRise:
                    importFile.Filter += "|Monster Hunter Rise Switch WWise Package (*.nsw)|*.nsw";
                    importFile.Filter += "|Monster Hunter Rise English WWise Package (*.En)|*.En";
                    importFile.Filter += "|Monster Hunter Rise Japanese WWise Package (*.Ja)|*.Ja";
                    importFile.Filter += "|Monster Hunter Rise Fictional WWise Package (*.Fc)|*.Fc";
                    importFile.Filter = "All supported files (*.pck,*.nsw,*.En,*.Ja,*.Fc)|*.pck;*.nsw;*.En;*.Ja;*.Fc|" + importFile.Filter;
                    break;
                case SupportedGames.RE2DMC5:
                    importFile.Filter += "|RE Engine WWise Package (*.x64)|*.x64";
                    importFile.Filter += "|RE Engine English WWise Package (*.En)|*.En";
                    importFile.Filter += "|RE Engine Japanese WWise Package (*.Ja)|*.Ja";
                    importFile.Filter = "All supported files (*.pck,*.x64,*.En,*.Ja)|*.pck;*.x64;*.En;*.Ja|" + importFile.Filter;
                    break;
                case SupportedGames.RE3R:
                    importFile.Filter += "|RE Engine WWise Package (*.stm)|*.stm";
                    importFile.Filter += "|RE Engine German WWise Package (*.De)|*.De";
                    importFile.Filter += "|RE Engine English WWise Package (*.En)|*.En";
                    importFile.Filter += "|RE Engine Spanish WWise Package (*.Es)|*.Es";
                    importFile.Filter += "|RE Engine French WWise Package (*.Fr)|*.Fr";
                    importFile.Filter += "|RE Engine Italian WWise Package (*.It)|*.It";
                    importFile.Filter += "|RE Engine Japanese WWise Package (*.Ja)|*.Ja";
                    importFile.Filter += "|RE Engine Chinese WWise Package (*.ZhCN)|*.ZhCN";
                    importFile.Filter = "All supported files (*.pck,*.stm,*.En,*.Ja,...)|*.pck;*.stm;*.De;*.En;*.Es;*.Fr;*.It;*.Ja;*.ZhCN|" + importFile.Filter;
                    break;
                case SupportedGames.RE8:
                    importFile.Filter += "|RE Engine WWise Package (*.x64)|*.x64";
                    importFile.Filter += "|RE Engine WWise Package (*.stm)|*.stm";
                    importFile.Filter += "|RE Engine German WWise Package (*.De)|*.De";
                    importFile.Filter += "|RE Engine English WWise Package (*.En)|*.En";
                    importFile.Filter += "|RE Engine Spanish WWise Package (*.Es)|*.Es";
                    importFile.Filter += "|RE Engine French WWise Package (*.Fr)|*.Fr";
                    importFile.Filter += "|RE Engine Italian WWise Package (*.It)|*.It";
                    importFile.Filter += "|RE Engine Japanese WWise Package (*.Ja)|*.Ja";
                    importFile.Filter += "|RE Engine Russian WWise Package (*.Ru)|*.Ru";
                    importFile.Filter += "|RE Engine Chinese WWise Package (*.ZhCN)|*.ZhCN";
                    importFile.Filter = "All supported files (*.pck,*.x64,*.stm,*.En,...)|*.pck;*.x64;*.stm;*.De;*.En;*.Es;*.Fr;*.It;*.Ja;*.Ru;*.ZhCN|" + importFile.Filter;
                    break;
                default:
                    break;
            }
            if (importFile.ShowDialog() == true)
            {
                viewModel.wems.Clear();
                BinaryReader readFile = new BinaryReader(new FileStream(importFile.FileName, FileMode.Open), Encoding.ASCII);
                currentFileName = importFile.FileName.Split("\\").Last().Split(".")[0];
                npck = new NPCKHeader(readFile,mode,currentFileName);
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
            if (ExportPath != null)
            {
                saveFile.InitialDirectory = ExportPath;
            }
            saveFile.Filter = "WWise Package file (*.pck)|*.pck";
            switch (mode)
            {
                case SupportedGames.MHWorld:
                    saveFile.Filter += "|Monster Hunter World WWise Package (*.npck)|*.npck";
                    saveFile.Filter = "All supported files (*.pck,*.npck)|*.pck;*.npck|" + saveFile.Filter;
                    break;
                case SupportedGames.MHRise:
                    saveFile.Filter += "|Monster Hunter Rise Switch WWise Package (*.nsw)|*.nsw";
                    saveFile.Filter += "|Monster Hunter Rise English WWise Package (*.En)|*.En";
                    saveFile.Filter += "|Monster Hunter Rise Japanese WWise Package (*.Ja)|*.Ja";
                    saveFile.Filter += "|Monster Hunter Rise Fictional WWise Package (*.Fc)|*.Fc";
                    saveFile.Filter = "All supported files (*.pck,*.nsw,*.En,*.Ja,*.Fc)|*.pck;*.nsw;*.En;*.Ja;*.Fc|" + saveFile.Filter;
                    break;
                case SupportedGames.RE2DMC5:
                    saveFile.Filter += "|Devil May Cry 5 WWise Package (*.x64)|*.x64";
                    saveFile.Filter += "|Devil May Cry 5 English WWise Package (*.En)|*.En";
                    saveFile.Filter += "|Devil May Cry 5 Japanese WWise Package (*.Ja)|*.Ja";
                    saveFile.Filter = "All supported files (*.pck,*.x64,*.En,*.Ja)|*.pck;*.x64;*.En;*.Ja|" + saveFile.Filter;
                    break;
                case SupportedGames.RE3R:
                    saveFile.Filter += "|RE Engine WWise Package (*.stm)|*.stm";
                    saveFile.Filter += "|RE Engine German WWise Package (*.De)|*.De";
                    saveFile.Filter += "|RE Engine English WWise Package (*.En)|*.En";
                    saveFile.Filter += "|RE Engine Spanish WWise Package (*.Es)|*.Es";
                    saveFile.Filter += "|RE Engine French WWise Package (*.Fr)|*.Fr";
                    saveFile.Filter += "|RE Engine Italian WWise Package (*.It)|*.It";
                    saveFile.Filter += "|RE Engine Japanese WWise Package (*.Ja)|*.Ja";
                    saveFile.Filter += "|RE Engine Chinese WWise Package (*.ZhCN)|*.ZhCN";
                    saveFile.Filter = "All supported files (*.pck,*.stm,*.En,*.Ja,...)|*.pck;*.stm;*.De;*.En;*.Es;*.Fr;*.It;*.Ja;*.ZhCN|" + saveFile.Filter;
                    break;
                case SupportedGames.RE8:
                    saveFile.Filter += "|RE Engine WWise Package (*.x64)|*.x64";
                    saveFile.Filter += "|RE Engine WWise Package (*.stm)|*.stm";
                    saveFile.Filter += "|RE Engine German WWise Package (*.De)|*.De";
                    saveFile.Filter += "|RE Engine English WWise Package (*.En)|*.En";
                    saveFile.Filter += "|RE Engine Spanish WWise Package (*.Es)|*.Es";
                    saveFile.Filter += "|RE Engine French WWise Package (*.Fr)|*.Fr";
                    saveFile.Filter += "|RE Engine Italian WWise Package (*.It)|*.It";
                    saveFile.Filter += "|RE Engine Japanese WWise Package (*.Ja)|*.Ja";
                    saveFile.Filter += "|RE Engine Russian WWise Package (*.Ru)|*.Ru";
                    saveFile.Filter += "|RE Engine Chinese WWise Package (*.ZhCN)|*.ZhCN";
                    saveFile.Filter = "All supported files (*.pck,*.x64,*.stm,*.En,...)|*.pck;*.x64;*.stm;*.De;*.En;*.Es;*.Fr;*.It;*.Ja;*.Ru;*.ZhCN|" + saveFile.Filter;
                    break;
                default:
                    break;
            }
            if (saveFile.ShowDialog() == true)
            {
                npck.ExportFile(saveFile.FileName);
                if(mode == SupportedGames.RE2DMC5||mode == SupportedGames.RE3R || mode == SupportedGames.MHRise || mode == SupportedGames.RE8)
                {
                    npck.ExportHeader(saveFile.FileName + ".nonstream");
                }
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

        private void IDReplace(object sender, RoutedEventArgs e)
        {
            InputDialog input = new InputDialog();
            input.LabelA.Content = "Input Wem IDs separated by a comma (,).";
            if(input.ShowDialog() == true)
            {
                input.Close();
                string IDs = input.Input.Text;
                string[] id2 = IDs.Split(',');
                for (int i = 0; i < id2.Length; i++)
                {
                    try
                    {
                        npck.WemList[i].id = Convert.ToUInt32(id2[i]);
                        viewModel.wems = new ObservableCollection<Wem>(npck.WemList);
                        WemView.ItemsSource = viewModel.wems;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        break;
                    }
                }
                
            }
        }
        private void LabelChanged(object sender, RoutedEventArgs e)
        {
            if (LabelsChanged == false)
            {
                LabelsChanged = true;
            }
            TextBox textbox = (TextBox)sender;
            Wem nWem = (Wem)textbox.DataContext;
            if (!changedIds.Contains(nWem.id))
            {
                changedIds.Add(nWem.id);
            }

        }
        private void SaveLabels(object sender, CancelEventArgs e)
        {
            if (LabelsChanged)
            {
                //prompt user
                MessageBoxResult saveLabels = MessageBox.Show("Save changed labels?", "", MessageBoxButton.YesNo);
                if (saveLabels == MessageBoxResult.Yes)
                {
                    if (currentFileName == null)
                    {
                        InputDialog input = new InputDialog();
                        input.LabelA.Content = "Input a filename for the label file.";
                        if (input.ShowDialog() == true)
                        {
                            input.Close();
                            currentFileName = input.Input.Text;
                        }
                    }
                    npck.labels.Export(Directory.GetCurrentDirectory() + "/" + mode.ToString() + "/PCK/" + currentFileName + ".lbl", npck.WemList, changedIds);
                }
            }
            LabelsChanged = false;
        }
    }
}

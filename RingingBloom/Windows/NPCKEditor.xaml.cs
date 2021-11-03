﻿using Microsoft.Win32;
using RingingBloom.Common;
using RingingBloom.WWiseTypes.ViewModels;
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
        public NPCKViewModel viewModel { get; set; }
        private string ImportPath = null;
        private string ExportPath = null;
        private string currentFileName = null;
        private bool LabelsChanged = false;
        public List<uint> changedIds = new List<uint>();

        public NPCKEditor(SupportedGames Mode,Options options)
        {
            InitializeComponent();
            mode = Mode;
            viewModel = new NPCKViewModel();
            WemView.DataContext = viewModel;
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
            if (viewModel.npck == null)
            {
                MessageBox.Show("NPCK not loaded.");
                return;
            }
            OpenFileDialog openFile = new OpenFileDialog();
            if(ImportPath != null)
            {
                openFile.InitialDirectory = ImportPath;
            }
            openFile.Multiselect = true;
            openFile.Filter = "WWise Wem files (*.wem)|*.wem";
            if (openFile.ShowDialog() == true)
            {
                viewModel.AddWems(openFile.FileNames);
            }


        }

        private void Replace_Wem(object sender, RoutedEventArgs e)
        {
            if (viewModel.npck == null)
            {
                MessageBox.Show("NPCK not loaded.");
                return;
            }
            OpenFileDialog openFile = new OpenFileDialog();
            if (ImportPath != null)
            {
                openFile.InitialDirectory = ImportPath;
            }
            openFile.Multiselect = false;
            openFile.Filter = "WWise Wem files (*.wem)|*.wem";
            if (openFile.ShowDialog() == true)
            {
                Wem newWem = HelperFunctions.MakeWems(openFile.FileName, HelperFunctions.OpenFile(openFile.FileName));
                viewModel.ReplaceWem(newWem, WemView.SelectedIndex);

            }
        }

        private void Export_Wems(object sender, RoutedEventArgs e)
        {
            if (viewModel.npck == null)
            {
                MessageBox.Show("NPCK not loaded.");
                return;
            }
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
                viewModel.ExportWems(exportIds, savePath);
            }


        }

        private void Delete_Wem(object sender, RoutedEventArgs e)
        {
            if (viewModel.npck == null)
            {
                MessageBox.Show("NPCK not loaded.");
                return;
            }
            try
            {
                viewModel.DeleteWem(WemView.SelectedIndex);
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
            viewModel.SetNPCK(new NPCKHeader(mode));
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
                    importFile.Filter += "|Monster Hunter Rise Switch WWise Package (*.x64)|*.pck.3.x64";
                    importFile.Filter += "|Monster Hunter Rise English WWise Package (*.En)|*.pck.3.x64.En";
                    importFile.Filter += "|Monster Hunter Rise Japanese WWise Package (*.Ja)|*.pck.3.x64.Ja";
                    importFile.Filter += "|Monster Hunter Rise Fictional WWise Package (*.Fc)|*.pck.3.x64.Fc";
                    importFile.Filter = "All supported files (*.pck,*.x64,*.En,*.Ja,*.Fc)|*.pck;*.pck.3.x64;*.pck.3.x64.En;*.pck.3.x64.Ja;*.pck.3.x64.Fc|" + importFile.Filter;
                    break;
                case SupportedGames.MHRiseSwitch:
                    importFile.Filter += "|Monster Hunter Rise Switch WWise Package (*.nsw)|*.pck.3.nsw";
                    importFile.Filter += "|Monster Hunter Rise English WWise Package (*.En)|*.pck.3.nsw.En";
                    importFile.Filter += "|Monster Hunter Rise Japanese WWise Package (*.Ja)|*.pck.3.nsw.Ja";
                    importFile.Filter += "|Monster Hunter Rise Fictional WWise Package (*.Fc)|*.pck.3.nsw.Fc";
                    importFile.Filter = "All supported files (*.pck,*.nsw,*.En,*.Ja,*.Fc)|*.pck;*.pck.3.nsw;*.pck.3.nsw.En;*.pck.3.nsw.Ja;*.pck.3.nsw.Fc|" + importFile.Filter;
                    break;
                case SupportedGames.RE2DMC5:
                    importFile.Filter += "|RE Engine WWise Package (*.x64)|*.pck.3.x64";
                    importFile.Filter += "|RE Engine English WWise Package (*.En)|*.pck.3.x64.En";
                    importFile.Filter += "|RE Engine Japanese WWise Package (*.Ja)|*.pck.3.x64.Ja";
                    importFile.Filter = "All supported files (*.pck,*.x64,*.En,*.Ja)|*.pck;*.pck.3.x64;*.pck.3.x64.En;*.pck.3.x64.Ja|" + importFile.Filter;
                    break;
                case SupportedGames.RE3R:
                    importFile.Filter += "|RE Engine WWise Package (*.stm)|*.pck.3.stm";
                    importFile.Filter += "|RE Engine German WWise Package (*.De)|*.pck.3.stm.De";
                    importFile.Filter += "|RE Engine English WWise Package (*.En)|*.pck.3.stm.En";
                    importFile.Filter += "|RE Engine Spanish WWise Package (*.Es)|*.pck.3.stm.Es";
                    importFile.Filter += "|RE Engine French WWise Package (*.Fr)|*.pck.3.stm.Fr";
                    importFile.Filter += "|RE Engine Italian WWise Package (*.It)|*.pck.3.stm.It";
                    importFile.Filter += "|RE Engine Japanese WWise Package (*.Ja)|*.pck.3.stm.Ja";
                    importFile.Filter += "|RE Engine Chinese WWise Package (*.ZhCN)|*.pck.3.stm.ZhCN";
                    importFile.Filter = "All supported files (*.pck,*.stm,*.En,*.Ja,...)|*.pck;*.pck.3.stm;*.pck.3.stm.De;*.pck.3.stm.En;*.pck.3.stm.Es;*.pck.3.stm.Fr;*.pck.3.stm.It;*.pck.3.stm.Ja;*.pck.3.stm.ZhCN|" + importFile.Filter;
                    break;
                case SupportedGames.RE8:
                    importFile.Filter += "|RE Engine WWise Package (*.x64)|*.pck.3.x64";
                    importFile.Filter += "|RE Engine WWise Package (*.stm)|*.pck.3.stm";
                    importFile.Filter += "|RE Engine German WWise Package (*.De)|*.pck.3.x64.De;*.pck.3.stm.De";
                    importFile.Filter += "|RE Engine English WWise Package (*.En)|*.pck.3.x64.En;*.pck.3.stm.En";
                    importFile.Filter += "|RE Engine Spanish WWise Package (*.Es)|*.pck.3.x64.Es;*.pck.3.stm.Es";
                    importFile.Filter += "|RE Engine French WWise Package (*.Fr)|*.pck.3.x64.Fr;*.pck.3.stm.Fr";
                    importFile.Filter += "|RE Engine Italian WWise Package (*.It)|*.pck.3.x64.It;*.pck.3.stm.It";
                    importFile.Filter += "|RE Engine Japanese WWise Package (*.Ja)|*.pck.3.x64.Ja;*.pck.3.stm.Ja";
                    importFile.Filter += "|RE Engine Russian WWise Package (*.Ru)|*.pck.3.x64.Ru;*.pck.3.stm.Ru";
                    importFile.Filter += "|RE Engine Chinese WWise Package (*.ZhCN)|*.pck.3.x64.ZhCN;*.pck.3.stm.ZhCN";
                    importFile.Filter = "All supported files (*.pck,*.x64,*.stm,*.En,...)|*.pck;*.pck.3.x64;*.pck.3.stm;*.pck.3.x64.De;*.pck.3.stm.De;*.pck.3.x64.En;*.pck.3.stm.En;*.pck.3.x64.Es;*.pck.3.stm.Es;*.pck.3.x64.Fr;*.pck.3.stm.Fr;*.pck.3.x64.It;*.pck.3.stm.It;*.pck.3.x64.Ja;*.pck.3.stm.Ja;*.pck.3.x64.Ru;*.pck.3.stm.Ru;*.pck.3.x64.ZhCN;*.pck.3.stm.ZhCN|" + importFile.Filter;
                    break;
                default:
                    break;
            }
            if (importFile.ShowDialog() == true)
            {
                BinaryReader readFile = HelperFunctions.OpenFile(importFile.FileName);
                currentFileName = importFile.FileName.Split("\\").Last().Split(".")[0];
                viewModel.SetNPCK(new NPCKHeader(readFile,mode,currentFileName));
                readFile.Close();
            }
        }

        private void ExportNPCK(object sender, RoutedEventArgs e)
        {
            if (viewModel.npck == null)
            {
                MessageBox.Show("NPCK not loaded.");
                return;
            }
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
                    saveFile.Filter += "|Monster Hunter Rise Switch WWise Package (*.x64)|*.pck.3.x64";
                    saveFile.Filter += "|Monster Hunter Rise English WWise Package (*.En)|*.pck.3.x64.En";
                    saveFile.Filter += "|Monster Hunter Rise Japanese WWise Package (*.Ja)|*.pck.3.x64.Ja";
                    saveFile.Filter += "|Monster Hunter Rise Fictional WWise Package (*.Fc)|*.pck.3.x64.Fc";
                    saveFile.Filter = "All supported files (*.pck,*.x64,*.En,*.Ja,*.Fc)|*.pck;*.pck.3.x64;*.pck.3.x64.En;*.pck.3.x64.Ja;*.pck.3.x64.Fc|" + saveFile.Filter;
                    break;
                case SupportedGames.MHRiseSwitch:
                    saveFile.Filter += "|Monster Hunter Rise Switch WWise Package (*.nsw)|*.pck.3.nsw";
                    saveFile.Filter += "|Monster Hunter Rise English WWise Package (*.En)|*.pck.3.nsw.En";
                    saveFile.Filter += "|Monster Hunter Rise Japanese WWise Package (*.Ja)|*.pck.3.nsw.Ja";
                    saveFile.Filter += "|Monster Hunter Rise Fictional WWise Package (*.Fc)|*.pck.3.nsw.Fc";
                    saveFile.Filter = "All supported files (*.pck,*.nsw,*.En,*.Ja,*.Fc)|*.pck;*.pck.3.nsw;*.pck.3.nsw.En;*.pck.3.nsw.Ja;*.pck.3.nsw.Fc|" + saveFile.Filter;
                    break;
                case SupportedGames.RE2DMC5:
                    saveFile.Filter += "|RE Engine WWise Package (*.x64)|*.pck.3.x64";
                    saveFile.Filter += "|RE Engine English WWise Package (*.En)|*.pck.3.x64.En";
                    saveFile.Filter += "|RE Engine Japanese WWise Package (*.Ja)|*.pck.3.x64.Ja";
                    saveFile.Filter = "All supported files (*.pck,*.x64,*.En,*.Ja)|*.pck;*.pck.3.x64;*.pck.3.x64.En;*.pck.3.x64.Ja|" + saveFile.Filter;
                    break;
                case SupportedGames.RE3R:
                    saveFile.Filter += "|RE Engine WWise Package (*.stm)|*.pck.3.stm";
                    saveFile.Filter += "|RE Engine German WWise Package (*.De)|*.pck.3.stm.De";
                    saveFile.Filter += "|RE Engine English WWise Package (*.En)|*.pck.3.stm.En";
                    saveFile.Filter += "|RE Engine Spanish WWise Package (*.Es)|*.pck.3.stm.Es";
                    saveFile.Filter += "|RE Engine French WWise Package (*.Fr)|*.pck.3.stm.Fr";
                    saveFile.Filter += "|RE Engine Italian WWise Package (*.It)|*.pck.3.stm.It";
                    saveFile.Filter += "|RE Engine Japanese WWise Package (*.Ja)|*.pck.3.stm.Ja";
                    saveFile.Filter += "|RE Engine Chinese WWise Package (*.ZhCN)|*.pck.3.stm.ZhCN";
                    saveFile.Filter = "All supported files (*.pck,*.stm,*.En,*.Ja,...)|*.pck;*.pck.3.stm;*.pck.3.stm.De;*.pck.3.stm.En;*.pck.3.stm.Es;*.pck.3.stm.Fr;*.pck.3.stm.It;*.pck.3.stm.Ja;*.pck.3.stm.ZhCN|" + saveFile.Filter;
                    break;
                case SupportedGames.RE8:
                    saveFile.Filter += "|RE Engine WWise Package (*.x64)|*.pck.3.x64";
                    saveFile.Filter += "|RE Engine WWise Package (*.stm)|*.pck.3.stm";
                    saveFile.Filter += "|RE Engine German WWise Package (*.De)|*.pck.3.x64.De;*.pck.3.stm.De";
                    saveFile.Filter += "|RE Engine English WWise Package (*.En)|*.pck.3.x64.En;*.pck.3.stm.En";
                    saveFile.Filter += "|RE Engine Spanish WWise Package (*.Es)|*.pck.3.x64.Es;*.pck.3.stm.Es";
                    saveFile.Filter += "|RE Engine French WWise Package (*.Fr)|*.pck.3.x64.Fr;*.pck.3.stm.Fr";
                    saveFile.Filter += "|RE Engine Italian WWise Package (*.It)|*.pck.3.x64.It;*.pck.3.stm.It";
                    saveFile.Filter += "|RE Engine Japanese WWise Package (*.Ja)|*.pck.3.x64.Ja;*.pck.3.stm.Ja";
                    saveFile.Filter += "|RE Engine Russian WWise Package (*.Ru)|*.pck.3.x64.Ru;*.pck.3.stm.Ru";
                    saveFile.Filter += "|RE Engine Chinese WWise Package (*.ZhCN)|*.pck.3.x64.ZhCN;*.pck.3.stm.ZhCN";
                    saveFile.Filter = "All supported files (*.pck,*.x64,*.stm,*.En,...)|*.pck;*.pck.3.x64;*.pck.3.stm;*.pck.3.x64.De;*.pck.3.stm.De;*.pck.3.x64.En;*.pck.3.stm.En;*.pck.3.x64.Es;*.pck.3.stm.Es;*.pck.3.x64.Fr;*.pck.3.stm.Fr;*.pck.3.x64.It;*.pck.3.stm.It;*.pck.3.x64.Ja;*.pck.3.stm.Ja;*.pck.3.x64.Ru;*.pck.3.stm.Ru;*.pck.3.x64.ZhCN;*.pck.3.stm.ZhCN|" + saveFile.Filter;
                    break;
                default:
                    break;
            }
            if (saveFile.ShowDialog() == true)
            {
                viewModel.ExportNPCK(saveFile.FileName, mode);
            }


        }

        private void HelpMenu(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("NPCK Editor: for editing WWise Package files.\n\n" +
                "[New] - Makes a new NPCK and prompts the user to import wems.\n" +
                "[Import] - Imports an NPCK/PCK of the user's choosing.\n" +
                "[Export] - Exports the currently open npck into a new file.\n" +
                "[Import Wems] - Imports the chosen wems into the currently opened NPCK/PCK.\n" +
                "[Replace Wem] - Replaces the selected wem in the view with the one chosen.\n" +
                "[Mass Replace] - Opens a window to import many wems and select which wems they should replace.\n" +
                "[Export Wems] - Exports the wems from the currently open npck into a folder.\n" +
                "[Delete Wem] - Deletes the currently selected wem.\n"+
                "[ID Replace] - Takes a list of wem IDs (separated by commas) and applies them to wems sequentially.");
        }

        private void IDReplace(object sender, RoutedEventArgs e)
        {
            if (viewModel.npck == null)
            {
                MessageBox.Show("NPCK not loaded.");
                return;
            }
            InputDialog input = new InputDialog();
            input.LabelA.Content = "Input Wem IDs separated by a comma (,).";
            if(input.ShowDialog() == true)
            {
                input.Close();
                string IDs = input.Input.Text;
                string[] id2 = IDs.Split(',');
                viewModel.IDReplace(id2);
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
                    viewModel.ExportLabels(mode, currentFileName, changedIds);
                }
            }
            LabelsChanged = false;
        }

        private void Mass_Replace(object sender, RoutedEventArgs e)
        {
            if(viewModel.npck == null)
            {
                MessageBox.Show("NPCK not loaded.");
                return;
            }
            List<uint> wemIds = viewModel.GetWemIds();
            MassReplace mass = new MassReplace(wemIds, ImportPath);
            if(mass.ShowDialog() == true)
            {
                viewModel.MassReplace(new List<ReplacingWem>(mass.holder.wems));
            }
        }
    }
}

using Microsoft.Win32;
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
using RingingBloom.WWiseTypes;

namespace RingingBloom.Windows
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class WemCreator : Window
    {
        public string WWiseCLIPath { get; set; }
        public string ProjectPath { get; set; }
        private string ImportPath = null;
        public string ExportPath { get; set; }
        public List<string> wavs { get; set; }
        public WemCreator(Options options)
        {
            InitializeComponent();
            wavs = new List<string>();
            if(options.defaultExport != null)
            {
                ExportPath = options.defaultExport;
            }
            else
            {
                ExportPath = Directory.GetCurrentDirectory();
            }
            ExportPath += "/Wems/";
            if(options.defaultImport != null)
            {
                ImportPath = options.defaultImport;
            }
            if(options.wwisePath != null)
            {
                WWiseCLIPath = options.wwisePath;
            }
            if(options.defaultProjectPath != null)
            {
                ProjectPath = options.defaultProjectPath;
            }
        }
        public void SetWWisePath(object sender, RoutedEventArgs e)
        {
            OpenFileDialog wwisePath = new OpenFileDialog();
            wwisePath.Title = "Set to your WWise installation's command-line (WWiseCLI) version";
            wwisePath.Filter = "Executable file (*.exe)| *.exe";
            if (wwisePath.ShowDialog() == true)
            {
                WWiseCLIPath = wwisePath.FileName;

            }
        }

        public void SetExportPath(object sender, RoutedEventArgs e)
        {
            OpenFileDialog exportFile = new OpenFileDialog();
            exportFile.CheckFileExists = false;
            exportFile.FileName = "Save Here";
            if (exportFile.ShowDialog() == true)
            {
                string fullPath = exportFile.FileName;
                ExportPath = System.IO.Path.GetDirectoryName(fullPath);
            }
        }

        public void SetProjectPath(object sender, RoutedEventArgs e)
        {
            OpenFileDialog projectFile = new OpenFileDialog();
            projectFile.Filter = "WWise Project (*.wproj)|*.wproj";
            projectFile.Title = "Select a valid WWise project for use:";
            if (projectFile.ShowDialog() == true)
            {
                ProjectPath = projectFile.FileName;
            }
        }

        public void ImportWavs(object sender, RoutedEventArgs e)
        {
            OpenFileDialog wavOpen = new OpenFileDialog();
            if(ImportPath != null)
            {
                wavOpen.InitialDirectory = ImportPath;
            }
            wavOpen.Filter = "Waveform Audio (*.wav) | *.wav";
            wavOpen.Multiselect = true;
            wavOpen.Title = "Select All Wavs to Convert";
            if (wavOpen.ShowDialog() == true)
            {
                for(int i = 0; i < wavOpen.FileNames.Length; i++)
                {
                    wavs.Add(wavOpen.FileNames[i]);
                    WavView.Items.Add(wavOpen.FileNames[i]);
                }
                RemovWav.IsEnabled = true;
                ExportWav.IsEnabled = true;
            }
        }

        public void RemoveWav(object sender, RoutedEventArgs e)
        {
            int index = WavView.SelectedIndex;
            if (index != -1)
            {
                wavs.RemoveAt(index);
                WavView.Items.RemoveAt(index);
            }
        }

        public void ExportWems(object sender, RoutedEventArgs e)
        {
            //check project path first, it's the only one that guaranteed needs to be set before exporting
            if(ProjectPath == null)
            {
                SetProjectPath(sender, e);
            }
            //make the wsources
            WSource.MakeWSource(wavs);
            //set up parameters to feed into our process
            string wwPath = "";
            if (WWiseCLIPath != null)
            {
                wwPath = WWiseCLIPath;
            }
            else
            {
                wwPath = Environment.GetEnvironmentVariable("wwiseroot");
                //this utilizes the path variable that WWise adds to your machine when you install it
                //now error proofing for if the variable does not exist on your machine
                if(wwPath == null)
                {
                    MessageBox.Show("WWise Root not present in path. Please manually give the WWise CLI path.");
                    return;
                }
                else
                {
                    wwPath += "Authoring\\x64\\Release\\bin\\WwiseCLI.exe";
                }
            }
            string args = "";
            //lots to do with the args
            args += "\""+ ProjectPath + "\"" + " ";
            args += "-ConvertExternalSources \"" + Directory.GetCurrentDirectory() + "\\Wems.wsources\" ";
            args += "-ExternalSourcesOutput " + "\"" + ExportPath + "\"";
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = wwPath;
            process.StartInfo.Arguments = args;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            //so that the CMD text can be displayed in the GUI
            process.StartInfo.RedirectStandardOutput = true;
            //start the process
            try
            {
                process.Start();
                SysOutput.Text = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show("WWise CLI Path not found. Either path variable is incorrect or WWise CLI path is incorrect.");
                return;
            }

        }

        private void HelpMenu(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Wem Creator: for simplifying the process of creating wems from .wav\n" +
                "(NOTE: An installation of WWise is required for this program to function properly.)\n\n" +
                "[Set WWise Path] - Allows you to input the path to WWiseCLI.exe. This can normally be found in {WWiseInstallationPath}/Audiokinetic/{WWiseVersion}/Authoring/x64/Release/bin. This is not completely necessary, but needed for unusual WWise installations.\n" +
                "[Set Project Path] - Allows you to input the path to your WWise project. One needs to be given to WWise for wems to be created.\n" +
                "[Set Export Path] - Allows you to choose where your created wems will be placed. This will default to the RingingBloom folder, in a subfolder called \"Wems\"\n" +
                "[Import Wavs] - Select the wavs you want to convert into wems. These do not need to be in the same folder.\n" +
                "[Remove Selected Wav] - Removes the selected wav file from the list to be created.\n" +
                "[Create Wems] - Runs WWise to create the wems in the directory provided.");
        }
    }
}

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

namespace RingingBloom.Windows
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class WemCreator : Window
    {
        public string WWiseCLIPath { get; set; }
        public string ExportPath { get; set; }
        public WemCreator()
        {
            InitializeComponent();
            
        }
        public void SetWWisePath(object sender, RoutedEventArgs e)
        {
            OpenFileDialog wwisePath = new OpenFileDialog();
            wwisePath.Title = "Set to your WWise installation's command-line version";
            if (wwisePath.ShowDialog() == true)
            {
                WWiseCLIPath = wwisePath.FileName;

            }
            else
            {
                MessageBox.Show("WWise CLI Path not set/changed. If not properly set, the program will fail.");
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

        private void HelpMenu(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Wem Creator: for simplifying the process of creating wems from .wav\n" +
                "(NOTE: An installation of WWise is required for this program to function properly.)\n\n" +
                "[Set WWise Path] - Allows you to input the path to WWiseCLI.exe. This can normally be found in {WWiseInstallationPath}/Audiokinetic/{WWiseVersion}/Authoring/x64/Release/bin .\n" +
                "[Set Project Path] - Allows you to input the path to your WWise project. You'll need to change some settings from default for wems to work in MHW.\n" +
                "[Set Export Path] - Allows you to choose where your created wems will be placed. This will default to the RingingBloom folder, in a subfolder called \"Wems\"\n" +
                "[Import Wavs] - Select the wavs you want to convert into wems. Specific filepaths are not necessary.");
        }
    }
}

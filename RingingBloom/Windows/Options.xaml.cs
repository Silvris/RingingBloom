using System;
using System.Collections.Generic;
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
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public OptionsWindow(Options options)
        {
            InitializeComponent();
            DefaultImport.Text = options.defaultImport;
            DefaultExport.Text = options.defaultExport;
            DefaultGame.SelectedIndex = (int)options.defaultGame;
            WWiseExePath.Text = options.wwisePath;
            DefaultProjectPath.Text = options.defaultProjectPath;

        }
        public void Confirm(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}

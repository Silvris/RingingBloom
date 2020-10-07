using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using RingingBloom.Windows;

namespace RingingBloom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WWCTEditor wwctEditor = null;
        WWBKPKEditor wwbkpkEditor = null;
        NPCKEditor npckEditor = null;
        LoopCalculator loopCalculator = null;
        public MainWindow()
        {
            InitializeComponent();
            Window.Background = HelperFunctions.GetBrushFromHex("#505050");
        }

        private void WWCTEdit(object sender, RoutedEventArgs e)
        {
            wwctEditor = new WWCTEditor();
            wwctEditor.Show();
        }
        private void WWPKBKEdit(object sender, RoutedEventArgs e)
        {
            wwbkpkEditor = new WWBKPKEditor();
            wwbkpkEditor.Show();
        }
        private void NPCKEdit(object sender, RoutedEventArgs e)
        {
            npckEditor = new NPCKEditor();
            npckEditor.Show();
        }
        private void LoopCalculator(object sender, RoutedEventArgs e)
        {
            loopCalculator = new LoopCalculator();
            loopCalculator.Show();
        }
    }
}

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
using RingingBloom.Common;

namespace RingingBloom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SupportedGames mode = SupportedGames.MHWorld;
        //Common programs
        //NBNKEditor nbnkEditor = null;
        NPCKEditor npckEditor = null;
        LoopCalculator loopCalculator = null;
        WemCreator wemCreator = null;
        //MHWorld programs
        WWCTEditor wwctEditor = null;
        WWBKPKEditor wwbkpkEditor = null;
        //EPVSPEditor epvspEditor = null;
        //WWEVEditor wwevEditor = null;
        //RE Engine programs
        //RSZEditor rszEditor = null;
        public MainWindow()
        {
            InitializeComponent();
            ChangeView();
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
            npckEditor = new NPCKEditor(mode);
            npckEditor.Show();
        }
        private void LoopCalculator(object sender, RoutedEventArgs e)
        {
            loopCalculator = new LoopCalculator();
            loopCalculator.Show();
        }
        private void WemCreate(object sender, RoutedEventArgs e)
        {
            wemCreator = new WemCreator();
            wemCreator.Show();
        }

        private void NullAllWindows()
        {
            /*if (nbnkEditor != null)
            {
                nbnkEditor.Close();
                nbnkEditor = null;
            }*/
            if (npckEditor != null)
            {
                npckEditor.Close();
                npckEditor = null;
            }
            if (loopCalculator != null)
            {
                loopCalculator.Close();
                loopCalculator = null;
            }
            if (wemCreator != null)
            {
                wemCreator.Close();
                wemCreator = null;
            }
            if (wwctEditor != null)
            {
                wwctEditor.Close();
                wwctEditor = null;
            }
            if (wwbkpkEditor != null)
            {
                wwbkpkEditor.Close();
                wwbkpkEditor = null;
            }
            /*if (epvspEditor != null)
            {
                epvspEditor.Close();
                epvspEditor = null;
            }*/
            /*if (wwevEditor != null)
            {
                wwevEditor.Close();
                wwevEditor = null;
            }*/
            /*if (rszEditor != null)
            {
                rszEditor.Close();
                rszEditor = null;
            }*/
        }

        private void ChangeView()
        {
            GameSelect.Header = "Mode: " + mode.ToString();
        }

        private void ChangeMode(object sender, RoutedEventArgs e)
        {
            //should only be called by MenuItems
            MenuItem item = (MenuItem)sender;
            mode = (SupportedGames)Convert.ToInt32(item.Tag);
            ChangeView();
            NullAllWindows();
        }
    }
}

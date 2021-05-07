using System;
using System.IO;
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
using System.Xml;

namespace RingingBloom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SupportedGames mode = SupportedGames.None;
        Options options;
        //Common programs
        BNKEditor bnkEditor = null;
        NPCKEditor npckEditor = null;
        LoopCalculator loopCalculator = null;
        WemCreator wemCreator = null;
        //MHWorld programs
        WWCTEditor wwctEditor = null;
        WWBKPKEditor wwbkpkEditor = null;
        EPVSPEditor epvspEditor = null;
        //WWEVEditor wwevEditor = null;
        //RE Engine programs
        //RSZEditor rszEditor = null;
        //WELEditor welEditor = null;
        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists("Options.xml"))
            {
                options = new Options(XmlReader.Create(new FileStream("Options.xml",FileMode.Open)));
            }
            else
            {
                options = new Options();
            }
            mode = options.defaultGame;
            ChangeView();
        }

        private void WWCTEdit(object sender, RoutedEventArgs e)
        {
            wwctEditor = new WWCTEditor(options);
            wwctEditor.Show();
        }
        private void WWPKBKEdit(object sender, RoutedEventArgs e)
        {
            wwbkpkEditor = new WWBKPKEditor(options);
            wwbkpkEditor.Show();
        }
        private void NPCKEdit(object sender, RoutedEventArgs e)
        {
            npckEditor = new NPCKEditor(mode, options);
            npckEditor.Show();
        }
        private void LoopCalculator(object sender, RoutedEventArgs e)
        {
            loopCalculator = new LoopCalculator();
            loopCalculator.Show();
        }
        private void WemCreate(object sender, RoutedEventArgs e)
        {
            wemCreator = new WemCreator(options);
            wemCreator.Show();
        }
        private void BNKEdit(object sender, RoutedEventArgs e)
        {
            bnkEditor = new BNKEditor(mode, options);
            bnkEditor.Show();
        }
        private void EPVSPEdit(object sender, RoutedEventArgs e)
        {
            epvspEditor = new EPVSPEditor(options);
            epvspEditor.Show();
        }

        private void SetOptions(object sender, RoutedEventArgs e)
        {
            OptionsWindow optionWindow = new OptionsWindow(options);
            optionWindow.ShowDialog();
            if(optionWindow.DialogResult == true)
            {
                ComboBoxItem DefaultGame = (ComboBoxItem)optionWindow.DefaultGame.SelectedItem;
                int defaultGame = Convert.ToInt32(DefaultGame.Tag);
                options = new Options(optionWindow.DefaultImport.Text, optionWindow.DefaultExport.Text, (SupportedGames)defaultGame, optionWindow.WWiseExePath.Text, optionWindow.DefaultProjectPath.Text);
                options.WriteOptions();
                mode = options.defaultGame;
                ChangeView();
            }
        }

        private void NullAllWindows()
        {
            if (bnkEditor != null)
            {
                bnkEditor.Close();
                bnkEditor = null;
            }
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
            /*if (welEditor != null)
            {
                welEditor.Close();
                welEditor = null;
            }*/
        }

        private void ChangeView()
        {
            GameSelect.Header = "Mode: " + mode.ToString();
            MainControl.Content = mode;
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
    public class ModeSelect : DataTemplateSelector
    {
        public DataTemplate NoneType { get; set; }
        public DataTemplate MHWorld { get; set; }
        public DataTemplate REEngine { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                if (item is SupportedGames.MHWorld)
                {
                    return MHWorld;
                }
                else if (item is SupportedGames.MHRise||item is SupportedGames.RE2DMC5||item is SupportedGames.RE3R||item is SupportedGames.RE8)
                {
                    return REEngine;
                }
                else
                {
                    return NoneType;
                }
            }
            else
            {
                return NoneType;
            }
        }
    }
}


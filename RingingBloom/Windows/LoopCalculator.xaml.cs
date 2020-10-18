using RingingBloom.WWiseTypes;
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
using System.Windows.Shapes;
using RingingBloom;

namespace RingingBloom.Windows
{
    /// <summary>
    /// Interaction logic for LoopCalculator.xaml
    /// </summary>
    public partial class LoopCalculator : Window
    {
        LoopCalculate loop = null;
        public LoopCalculator()
        { 
            InitializeComponent();
            loop = new LoopCalculate();
            DataContext = loop;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoopView.Text = loop.Calculate(IntroCount.SelectedIndex,LoopCount.SelectedIndex,(bool)IntroDiff.IsChecked,(bool)LoopDiff.IsChecked);
        }
    }
}

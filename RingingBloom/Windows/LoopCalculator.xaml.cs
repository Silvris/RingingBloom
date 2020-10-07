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

namespace RingingBloom.Windows
{
    /// <summary>
    /// Interaction logic for LoopCalculator.xaml
    /// </summary>
    public partial class LoopCalculator : Window
    {
        public LoopCalculator()
        { 
            LoopView.Background = HelperFunctions.GetBrushFromHex("#282828");
            InitializeComponent();
        }
    }
}

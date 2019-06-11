using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisualProfilerUI.ViewModel;

namespace VisualProfilerUI.View
{
    /// <summary>
    /// Interaction logic for ContainingUnitView.xaml
    /// </summary>
    public partial class ContainingUnitView : UserControl
    {
        public ContainingUnitView()
        {
            InitializeComponent();
        }

        public IEnumerable<MethodViewModel> ContainedMethods { get; set; }
    }
}

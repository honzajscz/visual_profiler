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

namespace VisualProfilerUI.View
{
    public partial class DetailView : UserControl
    {
        public static readonly DependencyProperty MetricsProperty =
            DependencyProperty.Register("Metrics", typeof (string), typeof (DetailView), new PropertyMetadata(default(string)));

        public string Metrics
        {
            get { return (string) GetValue(MetricsProperty); }
            set { SetValue(MetricsProperty, value); }
        }

        public static readonly DependencyProperty MethodNameProperty =
            DependencyProperty.Register("MethodName", typeof (string), typeof (DetailView), new PropertyMetadata(default(string)));

        public string MethodName
        {
            get { return (string) GetValue(MethodNameProperty); }
            set { SetValue(MethodNameProperty, value); }
        }
        

        public DetailView()
        {
            InitializeComponent();
        }


    }
}

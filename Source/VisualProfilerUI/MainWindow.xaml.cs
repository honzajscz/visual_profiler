using System.Windows;
using VisualProfilerAccess.ProfilingData;

namespace VisualProfilerUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            visualProfilerUI.Profile(ProfilerTypes.TracingProfiler, @"..\..\..\Mandelbrot\Bin\Mandelbrot.exe");
            //visualProfilerUI.Profile(ProfilerTypes.SamplingProfiler, @"..\..\..\Mandelbrot\Bin\Mandelbrot.exe");
        }
    }
}

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Mandelbrot
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationMessageLoop();
            stopwatch.Stop();
            string format = string.Format("---------Elapsed time = {0}", stopwatch.ElapsedMilliseconds/1000.0);
            Console.WriteLine();
            //MessageBox.Show(format);
        }

        private static void ApplicationMessageLoop()
        {
            Form f = new Form1();
            Application.Run(f);
        }
    }
}
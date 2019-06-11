using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using VisualProfilerAccess.ProfilingData;
using VisualProfilerAccess.ProfilingData.CallTrees;

namespace VisualProfilerAccess
{
    internal class Program
    {
        static ManualResetEvent ManualResetEvent = new ManualResetEvent(false);
        private static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-us");
            var processStartInfo = new ProcessStartInfo{ FileName = @"..\..\..\Mandelbrot\Bin\Mandelbrot.exe" };

            if (false)
            {
                var profilerAccess = new TracingProfilerAccess(
                    processStartInfo,
                    TimeSpan.FromMilliseconds(1000),
                    OnUpdateCallback);
                

                profilerAccess.StartProfiler();
             profilerAccess.Wait();
                //ManualResetEvent
                Console.WriteLine("bye bye");
            }
            else
            {
                var profilerAccess = new SamplingProfilerAccess(
                    processStartInfo,
                    TimeSpan.FromMilliseconds(1000),
                    OnUpdateCallback2);

                profilerAccess.StartProfiler();
                profilerAccess.Wait();
                Console.WriteLine("bye bye");
            }
        }

        private static void OnUpdateCallback(object sender, ProfilingDataUpdateEventArgs<TracingCallTree> eventArgs)
        {
         //   Console.Clear();
            foreach (TracingCallTree callTree in
                eventArgs.CallTrees)
            {
                ulong totalCycleTime = callTree.TotalCycleTime();
                ulong totalUserKernelTime = callTree.TotalUserKernelTime();
                string callTreeString = callTree.ToString((sb, cte) =>
                                                              {
                                                                  decimal cycleTime = totalCycleTime == 0
                                                                                          ? 0
                                                                                          : cte.CycleTime*
                                                                                            (decimal)
                                                                                            totalUserKernelTime/
                                                                                            totalCycleTime/1e7M;

                                                                  sb.AppendFormat(",Tact={0:0.00000}s", cycleTime);
                                                              });
                Console.WriteLine(callTreeString);
                Console.WriteLine();
            }
        }

        private static void OnUpdateCallback2(object sender, ProfilingDataUpdateEventArgs<SamplingCallTree> eventArgs)
        {
        //    Console.Clear();
            foreach (SamplingCallTree callTree in
                eventArgs.CallTrees)
            {
                Console.WriteLine(callTree.ToString(null));
                Console.WriteLine();
            }
        }
    }
}
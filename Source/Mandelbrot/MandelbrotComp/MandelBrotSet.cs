using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace Mandelbrot
{
    public class MandelBrotSet
    {
        //acquires the monitor's lock
        public static object threadLock = new object();

        public bool isCanceled = false;
        //a GDI taking care for rendering
        private IMandelBrotCompatible GDI;

        //implicit capacity
        private const int localBufferCap = 5;

        //capacity when the counted points will be moved to GDI
        private int flushCapacity = localBufferCap;
        //points
        private List<MandelBrotPoint> localBuffer;

        int scrWidth;
        int scrHeight;
        long translX;
        long translY;
        double zoom;
        int start;
        int end;

        //souradnice mysi pricteme k soucasnym, zvetsenim (podil strany panelu ku strane vyberu) vynasobime nove vznikla cisla a i zoom


        /// <summary>
        /// Consturcts a object that is responsible for counting a Mandelbrot's set 
        /// </summary>
        /// <param name="GDI">device responsible for rendering</param>
        /// <param name="workLoad">the area of the computation</param>
        /// <param name="clientRectangle">the area where the workLoad is situated</param>
        public MandelBrotSet(IMandelBrotCompatible GDI, int screenWidth, int screenHeight, long translateXInNewWorld, long translateYInNewWorld, double zoom, int start, int end)
        {
            this.GDI = GDI;
            localBuffer = new List<MandelBrotPoint>(2 * localBufferCap);

            scrWidth = screenWidth;
            scrHeight = screenHeight;
            translX = translateXInNewWorld;
            translY = translateYInNewWorld;
            this.zoom = zoom;
            this.start = start;
            this.end = end;
        }

        //int counter = 0;

        /// <summary>
        /// starts working on the workload
        /// </summary>
        public void Start()
        {
            //try - waiting for abort exception
            try
            {
                double denominator = scrHeight * zoom;
                double screenRation = scrWidth / (double)scrHeight;
                for (int i = 0; i < scrWidth; i++)
                {
                    for (int j = start; j < end; j++)     
                    {

                        double x = (i + translX * screenRation) / (denominator);
                        double y = -(j + translY / screenRation) / (denominator);

                        localBuffer.Add(new MandelBrotPoint(i, j, Mandelbrot(x, y)));


                        //if (isCanceled)
                        //{
                        //    return;
                        //}

                        if (localBuffer.Count >= flushCapacity)
                        {
                            FlushToGDIBuffer(false);

                        }
                    }
                }

                FlushToGDIBuffer(true);
            }

            finally
            {
                
                localBuffer.Clear();
                localBuffer.Capacity = 1;
                
            }

        }



        /// <summary>
        /// flushes the already computed points to the GDI, if the GDI is locked by another thread continues counting;
        /// </summary>
        /// <param name="requireWait">true if the thread should wait for the access to the GDIbuffer in order to flush, useful if you need to ensure the flush (end of work)</param>
        private void FlushToGDIBuffer(bool requireWait)
        {
            if (!requireWait)
            {

                bool goodToGo = false;
                try
                {
                    goodToGo = Monitor.TryEnter(threadLock);
                    if (goodToGo)
                    {   //Could enter the crital section

                        foreach (MandelBrotPoint item in localBuffer)
                        {
                            GDI.GDIBuffer.SetPixel(item.i, item.j, item.c);
                        }

                        GDI.Progress += localBuffer.Count;

                    }
                    else
                    {//Could not enter the crital section, never mind do more work, try later
                        flushCapacity += localBufferCap;
                    }
                }
                finally
                {
                    if (goodToGo)
                    {
                        Monitor.Exit(threadLock);
                        localBuffer.Clear();
                        flushCapacity = localBufferCap;
                    }
                }

            }
            else
            {
                lock (threadLock)
                {
                    GDI.Progress += localBuffer.Count;
                    foreach (MandelBrotPoint item in localBuffer)
                    {
                        GDI.GDIBuffer.SetPixel(item.i, item.j, item.c);
                    }
                    // Console.WriteLine(System.Threading.Thread.CurrentThread.Name + " ends");
                }
            }
        }

        /// <summary>
        /// perform the computation for a complex point
        /// </summary>
        /// <param name="x">real component</param>
        /// <param name="y">imaginary component </param>
        /// <returns>color for the particular complex point</returns>
        public static Color Mandelbrot(double realOfC, double imaginaryOfC)
        {


            /* //julia
            double realOfZ = realOfC;  //n-th z real - realna ze zet n
            double imaginaryOfZ = imaginaryOfC;  //n-th z imaginary - imaginarni ze zet n
            realOfC = 0.45;
            imaginaryOfC = 0.1428; //*/

            //mandelbrot
            double realOfZ = 0;  //n-th z real - realna ze zet n
            double imaginaryOfZ = 0;  //n-th z imaginary - imaginarni ze zet n
            //*/

            
            int maxIterations = 600;
            int iterationsCounter = 0;

            double realOfZPower = realOfZ * realOfZ;
            double imaginaryOfZPower = imaginaryOfZ * imaginaryOfZ;

            while ((realOfZPower + imaginaryOfZPower) < 2 * 2 && iterationsCounter <= maxIterations)
            {
                realOfZPower = realOfZ * realOfZ; //umocneni realne slozky
                imaginaryOfZPower = imaginaryOfZ * imaginaryOfZ; //umozneni imaginarni

                double tmprealOfZ = realOfZPower - imaginaryOfZPower + realOfC;  //    vypocet nasledujici z - realne slozky
                imaginaryOfZ = 2 * realOfZ * imaginaryOfZ + imaginaryOfC;                  //   vypocet nasledujici z - imaginarni slozky
                realOfZ = tmprealOfZ;
                iterationsCounter++;
            }
            

            if (iterationsCounter == maxIterations + 1)
            {
                return Color.Black;
            }
            else
            {
                //          double tmpColor = iterationsCounter + 1 - Math.Log(Math.Log(Math.Sqrt(realOfZPower + imaginaryOfZPower), Math.E), Math.E) / Math.Log(2, Math.E);
                //double tmpColor = iterationsCounter;
                //return Barvy.barvy[iterationsCounter*5 % 427];//Color.FromArgb(0, 0, (int)tmpColor * 9 % 255);
                return Color.FromArgb(0, iterationsCounter*10 % 255, iterationsCounter * 10 % 255);
            }
        }
    }
}
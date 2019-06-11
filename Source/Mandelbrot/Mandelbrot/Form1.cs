using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Mandelbrot
{
    public partial class Form1 : Form, IMandelBrotCompatible
    {
        //an exchange buffer, where the computed item are stored
        private Bitmap myGDIBuffer;//= new List<MandelBrotPoint>();

        private int progress = 0;

        public int Progress
        {
            get { return progress; }
            set { progress = value; }
        }
        Thread[] workerThreads;
        Stopwatch stopwatch = new Stopwatch();

        System.Windows.Forms.Timer timer;

        int threadCount;
        int scrWidth;
        int scrHeight;
        long translX;
        long translY;
        decimal zoom;

        const decimal INITIALZOOM = 1 / (decimal)3.0;

        public Form1()
        {
           InitializeComponent();

            MaximizeBox = false;
            
            Bitmap stopBMP = new Bitmap(Properties.Resources.stop);
            stopBMP.MakeTransparent();
            btnStop.Image = stopBMP;

            Bitmap zoomOutBMP = new Bitmap(Properties.Resources.zoom_out);
            zoomOutBMP.MakeTransparent();
            btnZoomOut.Image = zoomOutBMP;


            Bitmap saveBMP = new Bitmap(Properties.Resources.save);
            saveBMP.MakeTransparent();
            btnSave.Image = saveBMP;

            scrWidth = panel1.Width;
            scrHeight = panel1.Height;
            myGDIBuffer = new Bitmap(scrWidth, scrHeight, panel1.CreateGraphics());
            translX = -scrHeight * 11 / 16;// moves to right slighty
            translY = -scrWidth / 2;
            this.zoom = INITIALZOOM;
            threadCount = 4;//Environment.ProcessorCount ;
            Go(panel1, threadCount);

        }

    
        private void Go(Control GDI, int threadCount)
        {

            ProgressBar1.Maximum = scrWidth * scrHeight;
            ProgressBar1.Value = 0;
            progress = 0;
            workerThreads = new Thread[threadCount];
            MandelBrotSet[] workers = new MandelBrotSet[threadCount];
            int workLoad = scrHeight / threadCount;
            for (int i = 0; i < threadCount; i++)
            {
                workers[i] = new MandelBrotSet(this, scrWidth, scrHeight, translX, translY, (double)zoom, i * workLoad, (i + 1) * workLoad);
                workerThreads[i] = new Thread(workers[i].Start);
                workerThreads[i].Name = "worker " + i;
            }

            
            stopwatch.Reset();
            stopwatch.Start();
            //Stopwatch stopwatch1 = new Stopwatch();
            //stopwatch1.Start();
            foreach (Thread item in workerThreads)
            {
                item.Start();
            }

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 50;
            timer.Tick += OnTick;
            timer.Start();

            //foreach (var workerThread in workerThreads)
            //{
            //    workerThread.Join();
            //}
            //stopwatch1.Stop();
            //MessageBox.Show("Thread time was = " + stopwatch1.ElapsedTicks);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Monitor.TryEnter(MandelBrotSet.threadLock))
            {
                panel1.CreateGraphics().DrawImage(myGDIBuffer, 0, 0);
                Monitor.Exit(MandelBrotSet.threadLock);
                //e.Graphics.DrawImage(myGDIBuffer,panel1.Location.x,panel1);
            }
        }

        protected override bool IsInputKey(System.Windows.Forms.Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Up:
                    return true;
                case Keys.Down:
                    goto case Keys.Up;
                case Keys.Left:
                    goto case Keys.Up;
                case Keys.Right:
                    goto case Keys.Up;

            }
            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Up:
                    Finish();
                    translY -= scrHeight / 10;
                    Go(panel1, threadCount);
                    break;
                case Keys.Down:
                    Finish();
                    translY += scrHeight / 10;
                    Go(panel1, threadCount);
                    break;
                case Keys.Left:
                    Finish();
                    translX -= scrWidth / 10;
                    Go(panel1, threadCount);
                    break;
                case Keys.Right:
                    Finish();
                    translX += scrWidth / 10;
                    Go(panel1, threadCount);
                    break;

            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Finish();
            base.OnClosing(e);
        }

        /// <summary>
        /// when the timer elapses, repaint
        /// </summary>
        private void OnTick(object sender, EventArgs e)
        {
            this.Invalidate();
            if (Monitor.TryEnter(MandelBrotSet.threadLock))
            {
                try
                {
                    ProgressBar1.Value = progress;
                }
                catch (Exception)
                {

                    ProgressBar1.Value = ProgressBar1.Maximum;
                }
                percent.Text = String.Format("{0:f2}%, Time = {1} s", progress / (double)(scrHeight * scrWidth) * 100, stopwatch.ElapsedMilliseconds/1000.0);
                Monitor.Exit(MandelBrotSet.threadLock);
            }
            bool isAnyAlive = false;
            foreach (Thread item in workerThreads)
            {
                if (item.IsAlive)
                {
                    isAnyAlive = true;
                    break;
                }
            }
            if (!isAnyAlive)
            {
                
                timer.Stop();
                stopwatch.Stop();
                ProgressBar1.Value = ProgressBar1.Maximum;
                percent.Text = String.Format("{0:f2}%, Time = {1} s", 100, stopwatch.ElapsedMilliseconds/1000.0);
            }
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            this.Invalidate();
        }

        #region IMandelBrotCompatible Members

        public Bitmap GDIBuffer
        {
            get { return myGDIBuffer; }
        }
        #endregion

        private void Finish()
        {
            if (workerThreads != null)
            {
                foreach (Thread item in workerThreads)
                {
                    item.Abort();
                }
                timer.Stop();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Finish();
            //return the focus to panel, so the key events can be handeled
            panel1.Focus();
        }

        #region Mouse selection

        bool isDrag = false;
        Rectangle theRectangle = new Rectangle(new Point(0, 0), new Size(0, 0));
        Point startPoint;

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                timer.Stop();
                isDrag = true;
                Control control = (Control)sender;
                startPoint = control.PointToScreen(new Point(e.X, e.Y));
                ControlPaint.DrawReversibleFrame(theRectangle, Color.White, FrameStyle.Dashed);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            decimal screenRation = scrWidth / (decimal)scrHeight;

            //draw selection rectangle
            if (isDrag)
            {
                ControlPaint.DrawReversibleFrame(theRectangle, Color.White, FrameStyle.Dashed);

                Point endPoint = ((Control)sender).PointToScreen(new Point(e.X, e.Y));

                int height = endPoint.Y - startPoint.Y;
                int width = 0;
                if (endPoint.X < startPoint.X && endPoint.Y > startPoint.Y || endPoint.X > startPoint.X && endPoint.Y < startPoint.Y)
                {

                    width = -height;
                }
                else
                {
                    width = height;
                }

                width = (int)(width * screenRation);
                theRectangle = new Rectangle(startPoint.X, startPoint.Y, width, height);

                ControlPaint.DrawReversibleFrame(theRectangle, Color.White, FrameStyle.Dashed);
            }

            //computer complex coordinates
            decimal denominator = scrHeight * zoom;
            decimal x = (e.X + translX * screenRation) / (denominator);
            decimal y = -(e.Y + translY / screenRation) / (denominator);

            //show complex coords
            ComplexCoords.Text = string.Format("[{0:f4}, {1:f4}] zoom: {2:f2}x ", x, y, zoom / INITIALZOOM);
        }

        private void Form1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            if (isDrag)
            {
                timer.Start();

                isDrag = false;

                ControlPaint.DrawReversibleFrame(theRectangle, Color.White, FrameStyle.Dashed);

                Finish();
                Rectangle tmpRect = panel1.RectangleToClient(theRectangle);

                //if the rectangle is dragged backwards
                if (tmpRect.Width < 0)
                {
                    tmpRect.X += tmpRect.Width;
                    tmpRect.Width = -tmpRect.Width;
                }
                if (tmpRect.Height < 0)
                {
                    tmpRect.Y += tmpRect.Height;
                    tmpRect.Height = -tmpRect.Height;
                }
                if (tmpRect.Height == 0)
                {
                    tmpRect.Height = panel1.Height;
                }


                decimal screenRatio = scrWidth / (decimal)scrHeight;

                decimal ratio = panel1.Height / (decimal)tmpRect.Height;
                //ratio = ratio;
                translX = (long)((translX + tmpRect.X / screenRatio) * ratio);
                translY = (long)((translY + tmpRect.Y * screenRatio) * ratio);
                zoom = zoom * ratio;

                panel1.Invalidate();
                Go(panel1, threadCount);

                // Reset the rectangle.
                theRectangle = new Rectangle(0, 0, 0, 0);
            }
        }
        #endregion


        private void btnZoom_Click(object sender, EventArgs e)
        {
            decimal screenRatio = scrWidth / (decimal)scrHeight;
            Finish();
            decimal ratio = 0.3m;
            translX = (long)((translX - scrWidth / screenRatio) * ratio);
            translY = (long)((translY - scrHeight * screenRatio) * ratio);
            zoom = zoom * ratio;
            Go(panel1, threadCount);

            //computer complex coordinates
            decimal denominator = scrHeight * zoom;
            decimal x = (translX * screenRatio) / (denominator);
            decimal y = -(translY / screenRatio) / (denominator);

            //show complex coords
            ComplexCoords.Text = string.Format("[{0:f4}, {1:f4}] zoom: {2:f2}x ", x, y, zoom/INITIALZOOM);

            //return the focus to panel, so the key events can be handeled
            panel1.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Finish();
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "png files (*.png)|*.png|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 0;
            saveFileDialog1.RestoreDirectory = true;
          //  saveFileDialog1.DefaultExt = ".png";
            saveFileDialog1.FileName = ("mandelbrot - " + ComplexCoords).Replace(':','~');

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
               
                GDIBuffer.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
            //return the focus to panel, so the key events can be handeled
            panel1.Focus();
        }






    }


}

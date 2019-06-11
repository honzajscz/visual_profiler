//using System.Linq;

using System.Drawing;

namespace Mandelbrot
{
    public struct MandelBrotPoint
    {
        public int i, j;
        public Color c;
        public MandelBrotPoint(int i, int j, Color c)
        {
            this.i = i;
            this.j = j;
            this.c = c;
        }
    }


}

using System.Drawing;

namespace Mandelbrot
{
    public interface IMandelBrotCompatible
    {
        Bitmap GDIBuffer { get; }
        int Progress
        {
            get;
            set;
        }
    }
}
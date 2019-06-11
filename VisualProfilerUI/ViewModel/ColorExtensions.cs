using System.Windows.Media;

namespace VisualProfilerUI.ViewModel
{
    public static class ColorExtensions
    {
        public static Brush ToBrush(this Color color)
        {
            var solidColorBrush = new SolidColorBrush(color);
            return solidColorBrush;
        }
    }
}
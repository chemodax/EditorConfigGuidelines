using System.Windows.Media;

namespace EditorConfigGuidelines
{
    internal class Guideline
    {
        public Guideline(int column, DoubleCollection? dashArray)
        {
            Column = column;
            DashArray = dashArray;
        }

        public int Column { get; }
        public DoubleCollection? DashArray { get;  }
    }
}

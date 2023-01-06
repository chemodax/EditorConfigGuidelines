using System.Windows.Media;

namespace EditorConfigGuidelines
{
    internal class Guideline
    {
        private readonly int column;
        private readonly DoubleCollection dashArray;

        public Guideline(int column, DoubleCollection dashArray)
        {
            this.column = column;
            this.dashArray = dashArray;
        }

        public int Column { get { return column; } }
        public DoubleCollection DashArray { get { return dashArray; } }
    }
}

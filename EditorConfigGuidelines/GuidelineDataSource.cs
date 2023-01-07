using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.Text.Editor;

namespace EditorConfigGuidelines
{
    internal class GuidelineDataSource : INotifyPropertyChanged
    {
        public double X
        {
            get
            {
                return x;
            }

            private set
            {
                SetProperty(ref x, value);
            }
        }

        public double Y1
        {
            get
            {
                return y1;
            }
            private set
            {
                SetProperty(ref y1, value);
            }
        }

        public double Y2
        {
            get
            {
                return y2;
            }

            private set
            {
                SetProperty(ref y2, value);
            }
        }

        private readonly IWpfTextView view;
        private int guideline;
        private double x;
        private double y1;
        private double y2;

        public event PropertyChangedEventHandler PropertyChanged;

        public GuidelineDataSource(IWpfTextView view, int guideline)
        {
            this.guideline = guideline;
            this.view = view;

            Update();
        }

        public void Update()
        {
            double newX =
                view.FormattedLineSource.BaseIndentation +
                view.FormattedLineSource.ColumnWidth * guideline;

            X = newX;
            Y1 = view.ViewportTop;
            Y2 = view.ViewportBottom;
        }

        private void SetProperty(ref double v, double newVal, [CallerMemberName] string name = null)
        {
            if (v != newVal)
            {
                v = newVal;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(name));
                }
            }
        }
    }
}

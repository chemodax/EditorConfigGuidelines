using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EditorConfigGuidelines
{
    internal sealed class GuidelinesAdornment
    {
        private readonly IAdornmentLayer layer;
        private int[] guidelines;

        private readonly IWpfTextView view;
        private readonly Brush brush;
        private readonly List<GuidelineDataSource> guidelinesDataSource;

        public GuidelinesAdornment(IWpfTextView view)
        {
            this.guidelinesDataSource = new List<GuidelineDataSource>();
            this.view = view;
            this.layer = view.GetAdornmentLayer("EditorConfigGuidelinesAdornment");

            this.guidelines = ParseOptions(view.Options);
            this.view.Options.OptionChanged += TextView_OptionChanged;
            this.view.LayoutChanged += TextView_LayoutChanged;

            this.brush = new SolidColorBrush(Color.FromArgb(0x20, 0x00, 0x00, 0xff));
            this.brush.Freeze();
        }

        private void TextView_LayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            foreach(GuidelineDataSource dataSource in guidelinesDataSource)
            {
                dataSource.Update();
            }
        }

        private void TextView_OptionChanged(object sender, EditorOptionChangedEventArgs e)
        {
            guidelines = ParseOptions(view.Options);
            CreateVisuals();
        }

        private static int[] ParseOptions(IEditorOptions options)
        {
            try
            {
                IReadOnlyDictionary<string, object> conventions =
                    options.GetOptionValue(DefaultOptions.RawCodingConventionsSnapshotOptionId); 
                object guidelines;
                if (conventions != null &&
                    conventions.TryGetValue("guidelines", out guidelines) &&
                    guidelines is string)
                {
                    List<int> result = new List<int>();
                    foreach (string str in guidelines.ToString().Split(new char[] {' ', ',', ';'}, StringSplitOptions.RemoveEmptyEntries))
                    {
                        int guideline;
                        if (int.TryParse(str, out guideline))
                        {
                            result.Add(guideline);
                        }
                    }

                    return result.ToArray();
                }

                return new int[] {};
            }
            catch (Exception)
            {
                return new int[] { };
            }
        }

        private void CreateVisuals()
        {
            layer.RemoveAllAdornments();
            guidelinesDataSource.Clear();
            foreach (int guideline in guidelines)
            {
                GuidelineDataSource dataContext = new GuidelineDataSource(view, guideline);
                guidelinesDataSource.Add(dataContext);

                Line line = new Line();
                line.DataContext = dataContext;
                line.SetBinding(Line.X1Property, new Binding(nameof(GuidelineDataSource.X)));
                line.SetBinding(Line.Y1Property, new Binding(nameof(GuidelineDataSource.Y1)));
                line.SetBinding(Line.X2Property, new Binding(nameof(GuidelineDataSource.X)));
                line.SetBinding(Line.Y2Property, new Binding(nameof(GuidelineDataSource.Y2)));
                line.Stroke = brush;

                layer.AddAdornment(
                    AdornmentPositioningBehavior.OwnerControlled,
                    null,
                    null,
                    line,
                    null);
            }
        }
    }
}

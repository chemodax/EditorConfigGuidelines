using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using TextEditorGuidelines;

namespace EditorConfigGuidelines
{
    internal sealed class GuidelinesAdornment
    {
        private static Guid colorCategoryGuid = new Guid("{54FAC166-299A-4D70-9F43-F79E9A867B80}");
        private static ThemeResourceKey guidelineColorKey =
            new ThemeResourceKey(colorCategoryGuid, "ColumnGuidelineColor", 
                                 ThemeResourceKeyType.BackgroundBrush);

        private readonly IAdornmentLayer layer;
        private int[] guidelines;

        private readonly IWpfTextView view;
        private readonly List<GuidelineDataSource> guidelinesDataSource;
        private readonly IEditorFormatMap formatMap;

        public GuidelinesAdornment(IWpfTextView view, IEditorFormatMapService formatMapService)
        {
            this.guidelinesDataSource = new List<GuidelineDataSource>();
            this.view = view;
            this.layer = view.GetAdornmentLayer("EditorConfigGuidelinesAdornment");

            this.guidelines = ParseOptions(view.Options);
            this.view.Options.OptionChanged += TextView_OptionChanged;
            this.view.LayoutChanged += TextView_LayoutChanged;
            this.formatMap = formatMapService.GetEditorFormatMap(view);
            this.formatMap.FormatMappingChanged += FormatMap_FormatMappingChanged;
        }

        private void FormatMap_FormatMappingChanged(object sender, FormatItemsEventArgs e)
        {
            if (e.ChangedItems.Contains(GuidelineFormatDefinition.FormatName))
            {
                CreateVisuals();
            }
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

                ResourceDictionary resDict = formatMap.GetProperties(GuidelineFormatDefinition.FormatName);
                line.Stroke = (Brush)resDict[EditorFormatDefinition.BackgroundBrushId];

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

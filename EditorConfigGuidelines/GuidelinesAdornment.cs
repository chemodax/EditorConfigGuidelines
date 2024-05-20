using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;

namespace EditorConfigGuidelines
{
    internal sealed class GuidelinesAdornment
    {
        private static Guid colorCategoryGuid = new Guid("{54FAC166-299A-4D70-9F43-F79E9A867B80}");
        private static ThemeResourceKey guidelineColorKey =
            new ThemeResourceKey(colorCategoryGuid, "ColumnGuidelineColor",
                                 ThemeResourceKeyType.BackgroundBrush);

        private readonly IAdornmentLayer layer;
        private Guideline[] guidelines;

        private readonly IWpfTextView view;
        private readonly List<GuidelineDataSource> guidelinesDataSource;
        private readonly IEditorFormatMap formatMap;

        public GuidelinesAdornment(IWpfTextView view, IEditorFormatMapService formatMapService)
        {
            this.guidelinesDataSource = new List<GuidelineDataSource>();
            this.view = view;
            this.layer = view.GetAdornmentLayer("EditorConfigGuidelinesAdornment");

            this.guidelines = EditorConfigOptionsParser.Parse(view.Options);
            this.view.Options.OptionChanged += TextView_OptionChanged;
            this.view.LayoutChanged += TextView_LayoutChanged;
            this.formatMap = formatMapService.GetEditorFormatMap(view);
            this.formatMap.FormatMappingChanged += FormatMap_FormatMappingChanged;

            if (guidelines.Length > 0)
            {
                RegisterUsage();
            }
        }

        private void RegisterUsage()
        {
            ThreadHelper.JoinableTaskFactory.StartOnIdle(async () =>
            {
                RatingPrompt rating = new RatingPrompt(
                    "Ivan.EditorConfigGuidelines",
                    "EditorConfig Guidelines",
                    await GeneralOptions.GetLiveInstanceAsync(),
                    50);
                rating.RegisterSuccessfulUsage();
            }, VsTaskRunContext.UIThreadIdlePriority).FireAndForget();
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
            foreach (GuidelineDataSource dataSource in guidelinesDataSource)
            {
                dataSource.Update();
            }
        }

        private void TextView_OptionChanged(object sender, EditorOptionChangedEventArgs e)
        {
            if (e.OptionId == DefaultOptions.RawCodingConventionsSnapshotOptionId.Name)
            {
                guidelines = EditorConfigOptionsParser.Parse(view.Options);

                if (guidelines.Length > 0)
                {
                    RegisterUsage();
                }

                CreateVisuals();
            }
        }

        private void CreateVisuals()
        {
            layer.RemoveAllAdornments();
            guidelinesDataSource.Clear();
            foreach (Guideline guideline in guidelines)
            {
                GuidelineDataSource dataContext = new GuidelineDataSource(view, guideline.Column);
                guidelinesDataSource.Add(dataContext);

                Line line = new Line();
                line.DataContext = dataContext;
                line.SetBinding(Line.X1Property, new Binding(nameof(GuidelineDataSource.X)));
                line.SetBinding(Line.Y1Property, new Binding(nameof(GuidelineDataSource.Y1)));
                line.SetBinding(Line.X2Property, new Binding(nameof(GuidelineDataSource.X)));
                line.SetBinding(Line.Y2Property, new Binding(nameof(GuidelineDataSource.Y2)));

                ResourceDictionary resDict = formatMap.GetProperties(GuidelineFormatDefinition.FormatName);
                line.Stroke = (Brush)resDict[EditorFormatDefinition.BackgroundBrushId];
                line.StrokeDashArray = guideline.DashArray;

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

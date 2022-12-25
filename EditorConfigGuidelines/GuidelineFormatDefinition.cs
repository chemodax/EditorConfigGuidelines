using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace TextEditorGuidelines
{
    [UserVisible(true)]
    [Export(typeof(EditorFormatDefinition))]
    [Name(GuidelineFormatDefinition.FormatName)]
    public class GuidelineFormatDefinition : EditorFormatDefinition
    {
        public const string FormatName = "EditorConfigGuidelines/ColumnGuideline";

        public GuidelineFormatDefinition()
        {
            DisplayName = "Column guideline color";
            BackgroundCustomizable = true;
            BackgroundColor = Color.FromArgb(0x35, 0xD0, 0xD0, 0xD0);
            ForegroundCustomizable = false;
        }
    }
}

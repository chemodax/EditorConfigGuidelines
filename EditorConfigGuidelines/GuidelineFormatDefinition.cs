using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace TextEditorGuidelines
{
    // TODO: Not used yet.
    [UserVisible(true)]
    [Export(typeof(EditorFormatDefinition))]
    [Name("ColumnGuideline")]
    public class GuidelineFormatDefinition : EditorFormatDefinition
    {
        public GuidelineFormatDefinition()
        {
            DisplayName = "Column guideline color";
            BackgroundCustomizable = true;
            BackgroundColor = Colors.Gray;
            ForegroundCustomizable = false;
        }
    }
}

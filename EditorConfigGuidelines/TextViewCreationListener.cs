using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace EditorConfigGuidelines
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class TextViewCreationListener : IWpfTextViewCreationListener
    {
        private readonly IEditorFormatMapService formatMapService;

        /// <summary>
        /// Defines the adornment layer for the adornment. This layer is ordered
        /// after the selection layer in the Z-order
        /// </summary>
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("EditorConfigGuidelinesAdornment")]
        [Order(After = PredefinedAdornmentLayers.Selection, Before = PredefinedAdornmentLayers.Text)]
        internal AdornmentLayerDefinition editorAdornmentLayer;

        [ImportingConstructor]
        public TextViewCreationListener(
            [Import] IEditorFormatMapService formatMapService)
        {
            this.formatMapService = formatMapService;
        }

        public void TextViewCreated(IWpfTextView textView)
        {
            new GuidelinesAdornment(textView, formatMapService);
        }
    }
}

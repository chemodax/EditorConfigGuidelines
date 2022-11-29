using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace EditorConfigGuidelines
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class TextViewCreationListener : IWpfTextViewCreationListener
    {
        /// <summary>
        /// Defines the adornment layer for the adornment. This layer is ordered
        /// after the selection layer in the Z-order
        /// </summary>
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("EditorConfigGuidelinesAdornment")]
        [Order(After = PredefinedAdornmentLayers.Selection, Before = PredefinedAdornmentLayers.Text)]
        private AdornmentLayerDefinition editorAdornmentLayer;

        public void TextViewCreated(IWpfTextView textView)
        {
            new GuidelinesAdornment(textView);
        }
    }
}

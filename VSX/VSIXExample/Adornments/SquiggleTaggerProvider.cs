using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace VSIXExample.Adornments
{
    [Export(typeof(ITaggerProvider))]
    [ContentType("csharp")]
    [TagType(typeof(UnderscoreSquiggleTag))]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    public class SquiggleTaggerProvider : ITaggerProvider
    {
        public const string UnderscoreErrorType = "Underscore Error";

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
        {
            return buffer.Properties.GetOrCreateSingletonProperty(() => new SquiggleTagger(buffer)) as ITagger<T>;
        }
    }
}

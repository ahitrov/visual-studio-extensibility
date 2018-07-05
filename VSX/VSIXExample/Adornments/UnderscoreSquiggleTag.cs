using Microsoft.VisualStudio.Text.Tagging;

namespace VSIXExample.Adornments
{
    public class UnderscoreSquiggleTag: ErrorTag
    {
        public UnderscoreSquiggleTag() : base(SquiggleTaggerProvider.UnderscoreErrorType) { }
    }
}

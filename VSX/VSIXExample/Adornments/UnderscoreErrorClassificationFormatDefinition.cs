using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Windows.Media;

namespace VSIXExample.Adornments
{
    [Export(typeof(EditorFormatDefinition))]
    [Name(SquiggleTaggerProvider.UnderscoreErrorType)]
    [Order(After = Priority.High)]
    [UserVisible(true)]
    class UnderscoreErrorClassificationFormatDefinition: EditorFormatDefinition
    {
        public UnderscoreErrorClassificationFormatDefinition()
        {
            this.ForegroundColor = Colors.Red;
            this.BackgroundCustomizable = false;
            this.DisplayName = SquiggleTaggerProvider.UnderscoreErrorType;
        }
    }
}

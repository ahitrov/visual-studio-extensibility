using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;

namespace VSIXExample.Adornments
{
    public class SquiggleTagger : ITagger<UnderscoreSquiggleTag>
    {
        private ITextBuffer buffer;
        private Regex regularExpression = new Regex(RegexConstants.MatchLeadingUnderscoreInPrivateFieldDeclarations, RegexOptions.Compiled);

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        [Export(typeof(ErrorTypeDefinition))]
        [Name(SquiggleTaggerProvider.UnderscoreErrorType)]
        [DisplayName(SquiggleTaggerProvider.UnderscoreErrorType)]
        ErrorTypeDefinition UnderscoreErrortypeDefinition = null;

        public SquiggleTagger(ITextBuffer buffer)
        {
            this.buffer = buffer;

            this.buffer.Changed += (sender, args) => this.HandleBufferChanged(args);
        }

        public IEnumerable<ITagSpan<UnderscoreSquiggleTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach (var line in this.GetIntersectingLines(spans))
            {
                var text = line.GetText();

                var match = this.regularExpression.Match(text);

                if (match.Success)
                {
                    var underscoreGroup = match.Groups[RegexConstants.GroupNames.Underscore];
                    var matchSpan = new SnapshotSpan(line.Start + underscoreGroup.Index - 1, underscoreGroup.Length + 1);

                    yield return new TagSpan<UnderscoreSquiggleTag>(matchSpan, new UnderscoreSquiggleTag());
                }
            }
        }

        private IEnumerable<ITextSnapshotLine> GetIntersectingLines(NormalizedSnapshotSpanCollection spans)
        {
            if (spans.Count == 0)
            {
                yield break;
            }

            var lastVisitedLineNumber = 1;
            var snapshot = spans[0].Snapshot;
            foreach (var span in spans)
            {
                var firstLine = snapshot.GetLineNumberFromPosition(span.Start);
                var lastline = snapshot.GetLineNumberFromPosition(span.End);

                for (var i = Math.Max(lastVisitedLineNumber, firstLine); i <= lastline; i++)
                {
                    yield return snapshot.GetLineFromLineNumber(i);
                }

                lastVisitedLineNumber = lastline;
            }
        }

        private void HandleBufferChanged(TextContentChangedEventArgs args)
        {
            if (args.Changes.Count == 0)
            {
                return;
            }

            var snapshot = args.After;
            var start = args.Changes[0].NewPosition;
            var end = args.Changes[args.Changes.Count - 1].NewEnd;

            var totalAffectedSpan = new SnapshotSpan(snapshot.GetLineFromPosition(start).Start, snapshot.GetLineFromPosition(end).End);

            this.TagsChanged?.Invoke(this, new SnapshotSpanEventArgs(totalAffectedSpan));
        }
    }
}

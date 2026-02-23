using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.ExtractorTool.Hints;

/// <summary>Handles extracting enumerables for <see cref="IExtractor"/>.</summary>
public sealed class EnumerableExtractHint : ExtractHint<IEnumerable>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ExtractPriority.EnumerableHint;

    /// <inheritdoc/>
    protected override bool Extract(IEnumerable source, IExtractorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        if (chainer.AddFoundValue(source))
        {
            int i = 0;
            foreach (object item in source)
            {
                ArgumentGuard.ThrowUponIterationLimit(
                    i++,
                    chainer.Options.Valuer.Options.IterationLimit
                );
                _ = chainer.InnerExtract(item);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}

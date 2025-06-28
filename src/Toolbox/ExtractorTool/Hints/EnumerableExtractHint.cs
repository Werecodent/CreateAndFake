using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.ExtractorTool.Hints;

/// <summary>Handles extracting enumerables for <see cref="IExtractor"/>.</summary>
public sealed class EnumerableExtractHint : ExtractHint<IEnumerable>
{
    /// <inheritdoc/>
    protected override bool Extract(IEnumerable value, IExtractorChainer extractor)
    {
        ArgumentGuard.ThrowIfNull(extractor, nameof(extractor));

        if (extractor.AddFoundValue(value))
        {
            IEnumerator gen = value.GetEnumerator();
            while (gen.MoveNext())
            {
                _ = extractor.InnerExtract(gen.Current);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}

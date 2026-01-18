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
        ArgumentGuard.ThrowIfNull(extractor);

        if (extractor.AddFoundValue(value))
        {
            foreach (object item in value)
            {
                _ = extractor.InnerExtract(item);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}

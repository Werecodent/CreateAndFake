using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.ExtractorTool.Hints;

/// <summary>Handles extracting dictionaries for <see cref="IExtractor"/>.</summary>
public sealed class DictionaryExtractHint : ExtractHint<IDictionary>
{
    /// <inheritdoc/>
    protected override bool Extract(IDictionary value, IExtractorChainer extractor)
    {
        ArgumentGuard.ThrowIfNull(extractor);

        if (extractor.AddFoundValue(value))
        {
            foreach (DictionaryEntry item in value)
            {
                _ = extractor.InnerExtract(item.Key);
                _ = extractor.InnerExtract(item.Value);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}

using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.ExtractorTool.Hints;

/// <summary>Handles extracting dictionaries for <see cref="IExtractor"/>.</summary>
public sealed class DictionaryExtractHint : ExtractHint<IDictionary>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ExtractPriority.DictionaryHint;

    /// <inheritdoc/>
    protected override bool Extract(IDictionary source, IExtractorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        if (chainer.AddFoundValue(source))
        {
            foreach (DictionaryEntry item in source)
            {
                _ = chainer.InnerExtract(item.Key);
                _ = chainer.InnerExtract(item.Value);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}

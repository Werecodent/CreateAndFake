using System.Collections;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.ExtractorTool.Hints;

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

    /// <inheritdoc/>
    protected override async Task<bool> ExtractAsync(
        IDictionary source,
        IExtractorChainer chainer,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(chainer);

        if (await chainer.AddFoundValueAsync(source, canceler).ConfigureAwait(false))
        {
            foreach (DictionaryEntry item in source)
            {
                _ = await chainer.InnerExtractAsync(item.Key, canceler).ConfigureAwait(false);
                _ = await chainer.InnerExtractAsync(item.Value, canceler).ConfigureAwait(false);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}

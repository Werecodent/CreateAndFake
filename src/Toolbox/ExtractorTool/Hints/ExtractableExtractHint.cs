using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Comparisons;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.ExtractorTool.Hints;

/// <inheritdoc/>
public sealed class ExtractableExtractHint : ExtractHint<IExtractable>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ExtractPriority.ExtractableHint;

    /// <inheritdoc/>
    protected override bool Extract(IExtractable source, IExtractorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        if (chainer.AddFoundValue(source))
        {
            foreach (object? item in source.ExtractContents())
            {
                if (item != null)
                {
                    _ = chainer.InnerExtract(item);
                }
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
        IExtractable source,
        IExtractorChainer chainer,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(chainer);

        if (await chainer.AddFoundValueAsync(source, canceler).ConfigureAwait(false))
        {
            foreach (object? item in source.ExtractContents())
            {
                if (item != null)
                {
                    _ = await chainer.InnerExtractAsync(item, canceler).ConfigureAwait(false);
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}

using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.ExtractorTool.Engine;
using Werecodent.CreateAndFake.FakerTool.Proxy;

namespace Werecodent.CreateAndFake.ExtractorTool.Hints;

/// <inheritdoc/>
public sealed class FakedExtractHint : ExtractHint<IFaked>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ExtractPriority.FakedHint;

    /// <inheritdoc/>
    protected override bool Extract(IFaked source, IExtractorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        if (chainer.AddFoundValue(source))
        {
            _ = chainer.InnerExtract(source.FakeMeta);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <inheritdoc/>
    protected override async Task<bool> ExtractAsync(
        IFaked source,
        IExtractorChainer chainer,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(chainer);

        if (await chainer.AddFoundValueAsync(source, canceler).ConfigureAwait(false))
        {
            _ = await chainer.InnerExtractAsync(source.FakeMeta, canceler).ConfigureAwait(false);
            return true;
        }
        else
        {
            return false;
        }
    }
}

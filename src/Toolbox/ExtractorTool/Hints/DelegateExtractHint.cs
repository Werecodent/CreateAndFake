using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.ExtractorTool.Hints;

/// <summary>Handles extracting delegates for <see cref="IExtractor"/>.</summary>
public sealed class DelegateExtractHint : ExtractHint<Delegate>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ExtractPriority.DelegateHint;

    /// <inheritdoc/>
    protected override bool Extract(Delegate source, IExtractorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        return chainer.AddFoundValue(source);
    }

    /// <inheritdoc/>
    protected override Task<bool> ExtractAsync(
        Delegate source,
        IExtractorChainer chainer,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(chainer);

        return chainer.AddFoundValueAsync(source, canceler);
    }
}

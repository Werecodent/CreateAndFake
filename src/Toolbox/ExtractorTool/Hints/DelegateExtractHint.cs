using CreateAndFake.Design;
using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.ExtractorTool.Hints;

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
}

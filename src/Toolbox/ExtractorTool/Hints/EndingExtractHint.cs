using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.ExtractorTool.Hints;

#pragma warning disable MA0042 // Using sync behavior for async versions.

/// <summary>Handles extracting final values for <see cref="IExtractor"/>.</summary>
public sealed class EndingExtractHint : ExtractHint
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ExtractPriority.EndingHint;

    /// <inheritdoc/>
    public override ExtractHintResult TryToExtract(object? source, IExtractorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        Type? type = source?.GetType();
        if (
            source != null
            && type != null
            && (
                ValueRandom.SupportedTypes.Contains(type)
                || type.IsEnum
                || chainer.Options.ContentEndTypes.Contains(type)
            )
        )
        {
            return new(chainer.AddFoundValue(source));
        }
        else
        {
            return ExtractHintResult.None;
        }
    }

    /// <inheritdoc/>
    public override async Task<ExtractHintResult> TryToExtractAsync(
        object? source,
        IExtractorChainer chainer,
        CancellationToken canceler
    )
    {
        ArgumentGuard.ThrowIfNull(chainer);

        Type? type = source?.GetType();
        if (
            source != null
            && type != null
            && (
                ValueRandom.SupportedTypes.Contains(type)
                || type.IsEnum
                || chainer.Options.ContentEndTypes.Contains(type)
            )
        )
        {
            return new(await chainer.AddFoundValueAsync(source, canceler).ConfigureAwait(false));
        }
        else
        {
            return ExtractHintResult.None;
        }
    }
}

#pragma warning restore

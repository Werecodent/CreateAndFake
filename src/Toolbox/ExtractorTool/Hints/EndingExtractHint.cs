using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.Design.Randomization;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.ExtractorTool.Hints;

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
}

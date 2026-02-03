using System.Collections.Frozen;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.Design.Content;
using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.ExtractorTool.Hints;

/// <summary>Handles extracting final values for <see cref="IExtractor"/>.</summary>
public sealed class EndingExtractHint : ExtractHint
{
    /// <inheritdoc cref="ExtractorOptions.ContentEndTypes"/>
    private static readonly FrozenSet<Type> _ContentEndTypes = RuntimeDetails
        .RuntimeTypes.Concat([
            Assembly.GetExecutingAssembly().GetType(),
            typeof(string),
            typeof(Type),
            typeof(Lock),
        ])
        .ToFrozenSet();

    /// <inheritdoc/>
    public override ExtractHintResult TryExtract(object? value, IExtractorChainer extractor)
    {
        ArgumentGuard.ThrowIfNull(extractor);

        Type? type = value?.GetType();
        if (
            value != null
            && type != null
            && (
                type.IsValueType
                || _ContentEndTypes.Contains(type)
                || extractor.Options.ContentEndTypes.Contains(type)
            )
        )
        {
            return new(extractor.AddFoundValue(value));
        }
        else
        {
            return ExtractHintResult.None;
        }
    }
}

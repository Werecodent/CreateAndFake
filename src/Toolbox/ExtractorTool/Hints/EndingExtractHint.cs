using System.Collections.Frozen;
using System.Reflection;
using CreateAndFake.Design;
using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.ExtractorTool.Hints;

/// <summary>Handles extracting final values for <see cref="IExtractor"/>.</summary>
public sealed class EndingExtractHint : ExtractHint
{
    /// <inheritdoc cref="ExtractorOptions.ContentEndTypes"/>
    private static readonly FrozenSet<Type> _ContentEndTypes = FrozenSet.ToFrozenSet(
        [
            Assembly.GetExecutingAssembly().GetType(),
            typeof(Type).GetType(),
            typeof(ParameterInfo),
            typeof(PropertyInfo),
            typeof(MemberInfo),
            typeof(MethodInfo),
            typeof(FieldInfo),
            typeof(Assembly),
            typeof(string),
            typeof(Type),
        ]
    );

    /// <inheritdoc/>
    public override ExtractHintResult TryExtract(object? value, IExtractorChainer extractor)
    {
        ArgumentGuard.ThrowIfNull(extractor, nameof(extractor));

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

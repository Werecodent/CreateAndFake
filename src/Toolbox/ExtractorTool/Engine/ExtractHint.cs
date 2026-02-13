using CreateAndFake.Design.Content;

namespace CreateAndFake.ExtractorTool.Engine;

/// <inheritdoc cref="IExtractHint"/>
public abstract class ExtractHint : IExtractHint
{
    /// <inheritdoc/>
    public abstract int EnginePriority { get; }

    /// <inheritdoc/>
    public virtual IEnumerable<Type> SupportedTypes { get; } = [];

    /// <inheritdoc/>
    public abstract ExtractHintResult TryExtract(object? source, IExtractorChainer chainer);

    /// <inheritdoc/>
    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}

using CreateAndFake.Design.Types;

namespace CreateAndFake.ExtractorTool.Engine;

/// <inheritdoc cref="IExtractHint"/>
public abstract class ExtractHint : IExtractHint
{
    /// <inheritdoc/>
    public abstract int EnginePriority { get; }

    /// <inheritdoc/>
    public virtual IEnumerable<Type> SupportedTypes { get; } = [];

    /// <inheritdoc/>
    public abstract ExtractHintResult TryToExtract(object? source, IExtractorChainer chainer);

    /// <inheritdoc/>
    public override string ToString()
    {
        return GenericConverter.ExpandName(GetType());
    }
}

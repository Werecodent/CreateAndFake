using CreateAndFake.Design.Content;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ExtractorTool.Engine;

/// <summary>Handles extracting specific types for <see cref="IExtractor"/>.</summary>
public abstract class ExtractHint : IToolHint
{
    /// <inheritdoc/>
    public virtual IEnumerable<Type> SupportedTypes { get; } = [];

    /// <summary>Tries to extract content.</summary>
    /// <param name="value"></param>
    /// <param name="extractor"></param>
    /// <returns></returns>
    public abstract ExtractHintResult TryExtract(object? value, IExtractorChainer extractor);

    /// <inheritdoc/>
    public override string ToString()
    {
        return TypeDescriber.ExpandedName(GetType());
    }
}

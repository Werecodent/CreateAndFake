using CreateAndFake.Design;
using CreateAndFake.Design.Tooling;

namespace CreateAndFake.ExtractorTool.Engine;

/// <inheritdoc cref="IExtractor"/>
/// <param name="defaultHints">Generators used to extract specific types.</param>
public sealed class ExtractorEngine(IEnumerable<ExtractHint> defaultHints)
    : ToolEngine<ExtractHint>(defaultHints),
        IExtractorEngine
{
    /// <inheritdoc/>
    public bool Extract(object? value, IExtractorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        ExtractHintResult? result = SelectHints(chainer)
            .Select(h => h.TryExtract(value, chainer))
            .FirstOrDefault(r => r.HasData);

        if (result != null)
        {
            return result.Data;
        }
        else
        {
            throw new NotSupportedException(
                $"Type '{value?.GetType().FullName}' not supported by the extractor. "
                    + "Create a hint to extract the type and pass it to the extractor."
            );
        }
    }
}

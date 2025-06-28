using CreateAndFake.Design;
using CreateAndFake.ExtractorTool.Engine;

namespace CreateAndFake.ExtractorTool.Hints;

/// <summary>Handles extracting tasks for <see cref="IExtractor"/>.</summary>
public sealed class TaskExtractHint : ExtractHint<Task>
{
    /// <inheritdoc/>
    protected override bool Extract(Task value, IExtractorChainer extractor)
    {
        ArgumentGuard.ThrowIfNull(extractor, nameof(extractor));

        return extractor.AddFoundValue(value);
    }
}

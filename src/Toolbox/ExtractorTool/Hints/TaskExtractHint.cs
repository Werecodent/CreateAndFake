using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.ExtractorTool.Hints;

/// <summary>Handles extracting tasks for <see cref="IExtractor"/>.</summary>
public sealed class TaskExtractHint : ExtractHint<Task>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ExtractPriority.TaskHint;

    /// <inheritdoc/>
    protected override bool Extract(Task source, IExtractorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        return chainer.AddFoundValue(source);
    }
}

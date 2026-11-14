using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.DuplicatorTool.Engine;

namespace Werecodent.CreateAndFake.DuplicatorTool.Hints;

/// <inheritdoc/>
public sealed class StreamCopyHint : CopyHint<Stream>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)CopyPriority.StreamHint;

    /// <inheritdoc/>
    protected override Stream Copy(Stream source, IDuplicatorChainer duplicator)
    {
        ArgumentGuard.ThrowIfNull(source);

        source.Flush();

        long startingPosition = source.Position;
        source.Position = 0;

        MemoryStream stream = new();
        source.CopyTo(stream);
        stream.Flush();

        source.Position = startingPosition;
        stream.Position = startingPosition;
        return stream;
    }
}

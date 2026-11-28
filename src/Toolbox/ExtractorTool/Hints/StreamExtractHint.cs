using System.Text;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.ExtractorTool.Engine;

namespace Werecodent.CreateAndFake.ExtractorTool.Hints;

/// <summary>Handles extracting enumerables for <see cref="IExtractor"/>.</summary>
public sealed class StreamExtractHint : ExtractHint<Stream>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ExtractPriority.StreamHint;

    /// <inheritdoc/>
    protected override bool Extract(Stream source, IExtractorChainer chainer)
    {
        return chainer.InnerExtract(ReadContents(source, chainer));
    }

    /// <inheritdoc/>
    protected override Task<bool> ExtractAsync(
        Stream source,
        IExtractorChainer chainer,
        CancellationToken canceler
    )
    {
        return chainer.InnerExtractAsync(ReadContents(source, chainer), canceler);
    }

    /// <summary>Extracts the <paramref name="source"/> by reading it line by line.</summary>
    /// <returns>The read content.</returns>
    /// <inheritdoc cref="ExtractAsync"/>
    private static IEnumerable<string?> ReadContents(Stream source, IExtractorChainer chainer)
    {
        source.Flush();

        long startingPosition = source.Position;
        source.Position = 0;

        using StreamReader reader = new(source, Encoding.UTF8, true, 1024, true);
        for (int i = 0; !reader.EndOfStream; i++)
        {
            ArgumentGuard.ThrowUponIterationLimit(i, chainer.Options.Design.IterationLimit);

            string? line = reader.ReadLine();
            if (!string.IsNullOrWhiteSpace(line))
            {
                yield return line;
            }
        }

        source.Position = startingPosition;
    }
}

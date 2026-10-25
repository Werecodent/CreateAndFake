using System.Text;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.ValuerTool.Engine;

namespace Werecodent.CreateAndFake.ValuerTool.Hints;

/// <inheritdoc/>
public sealed class StreamCompareHint : CompareHint<Stream>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)ComparePriority.StreamHint;

    /// <inheritdoc/>
    protected override IEnumerable<Difference> Compare(
        Stream expected,
        Stream actual,
        IValuerChainer chainer
    )
    {
        return chainer.Compare(ReadContents(expected, chainer), ReadContents(actual, chainer));
    }

    /// <inheritdoc/>
    protected override int GetHashCode(Stream item, IValuerChainer chainer)
    {
        return chainer.GetHashCode(ReadContents(item, chainer));
    }

    private static List<string?> ReadContents(Stream stream, IValuerChainer chainer)
    {
        stream.Flush();

        long startingPosition = stream.Position;
        stream.Position = 0;

        List<string?> results = [];

        using StreamReader reader = new(stream, Encoding.UTF8, true, -1, true);
        for (int i = 0; !reader.EndOfStream; i++)
        {
            ArgumentGuard.ThrowUponIterationLimit(i, chainer.Options.Design.IterationLimit);

            results.Add(reader.ReadLine());
        }

        stream.Position = startingPosition;

        return results;
    }
}

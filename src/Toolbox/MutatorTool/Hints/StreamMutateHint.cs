using System.Text;
using Werecodent.CreateAndFake.Design;
using Werecodent.CreateAndFake.MutatorTool.Engine;

namespace Werecodent.CreateAndFake.MutatorTool.Hints;

/// <inheritdoc/>
public class StreamMutateHint : MutateHint<Stream>
{
    /// <inheritdoc/>
    public override int EnginePriority => (int)MutatePriority.StreamHint;

    /// <inheritdoc/>
    protected override bool Modify(Stream instance, IMutatorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(instance, chainer);

        instance.Flush();

        using StreamWriter writer = new(instance, Encoding.UTF8, leaveOpen: true);
        writer.WriteLine(chainer.Options.Randomizer.Create<string>());
        writer.Flush();

        return true;
    }
}

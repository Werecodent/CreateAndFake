using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.MutatorTool.Engine;

namespace CreateAndFake.MutatorTool.Hints;

/// <inheritdoc/>
public sealed class LegacyListMutateHint : IMutateHint
{
    /// <inheritdoc/>
    public int EnginePriority => (int)MutatePriority.LegacyListHint;

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes { get; } = [typeof(IList)];

    /// <inheritdoc/>
    public MutateHintResult TryModifying(object instance, IMutatorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        if (instance is IList list)
        {
            if (list.IsReadOnly || (list.Count == 0 && list.IsFixedSize))
            {
                return new(false);
            }

            if (
                list.IsFixedSize
                || chainer.Options.Gen.Next<bool>()
                || list.Add(chainer.Options.Randomizer.Create<string>()) == -1
            )
            {
                list[chainer.Options.Gen.Next(list.Count)] =
                    chainer.Options.Randomizer.Create<string>();
            }
            return new(true);
        }
        else
        {
            return MutateHintResult.None;
        }
    }
}

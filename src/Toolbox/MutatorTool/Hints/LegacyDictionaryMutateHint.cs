using System.Collections;
using CreateAndFake.Design;
using CreateAndFake.MutatorTool.Engine;

namespace CreateAndFake.MutatorTool.Hints;

/// <inheritdoc/>
public sealed class LegacyDictionaryMutateHint : IMutateHint
{
    /// <inheritdoc/>
    public int EnginePriority => (int)MutatePriority.LegacyDictionaryHint;

    /// <inheritdoc/>
    public IEnumerable<Type> SupportedTypes { get; } = [typeof(IDictionary)];

    /// <inheritdoc/>
    public MutateHintResult TryModifying(object instance, IMutatorChainer chainer)
    {
        ArgumentGuard.ThrowIfNull(chainer);

        if (instance is IDictionary dict)
        {
            if (dict.IsReadOnly || (dict.Count == 0 && dict.IsFixedSize))
            {
                return new(false);
            }

            string key = (string)chainer.VariantOf(typeof(string), dict.Keys.Cast<object>());
            if (dict.IsFixedSize || chainer.Options.Gen.Next<bool>())
            {
                dict[key] = chainer.Options.Randomizer.Create<string>();
            }
            else
            {
                dict.Add(key, chainer.Options.Randomizer.Create<string>());
            }
            return new(true);
        }
        else
        {
            return MutateHintResult.None;
        }
    }
}

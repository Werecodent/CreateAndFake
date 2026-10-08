using System.Collections.Specialized;
using Werecodent.CreateAndFake.MutatorTool.Engine;

namespace Werecodent.CreateAndFake.MutatorTool.Handlers;

/// <inheritdoc cref="IMutateHandler"/>
internal sealed class StringDictionaryMutateHandler : IMutateHandler
{
    /// <inheritdoc/>
    public Type? SupportedType => typeof(StringDictionary);

    /// <inheritdoc/>
    public bool ModifySupported(object instance, IMutatorChainer chainer)
    {
        StringDictionary dict = (StringDictionary)instance;

        IEnumerable<string> keys = dict.Keys.Cast<string>();
        if (dict.Count > 0)
        {
            dict[chainer.Options.Gen.NextItem(keys)] = chainer.Options.Randomizer.Create<string>();
        }

        dict.Add(chainer.VariantOf(keys), chainer.Options.Randomizer.Create<string>());
        return true;
    }
}

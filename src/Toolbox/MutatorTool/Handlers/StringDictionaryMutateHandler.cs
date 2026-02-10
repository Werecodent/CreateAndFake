using System.Collections.Specialized;
using CreateAndFake.MutatorTool.Engine;

namespace CreateAndFake.MutatorTool.Handlers;

/// <inheritdoc cref="IMutateHandler"/>
internal sealed class StringDictionaryMutateHandler : IMutateHandler
{
    /// <inheritdoc/>
    public Type? SupportedType => typeof(StringDictionary);

    /// <inheritdoc/>
    public bool ModifySupported(object instance, IMutatorChainer chainer)
    {
        StringDictionary dict = (StringDictionary)instance;

        string key = chainer.Options.Gen.Next<bool>()
            ? chainer.Options.Gen.NextItem(dict.Keys.Cast<string>())
            : chainer.VariantOf(dict.Keys.Cast<string>());

        dict[key] = chainer.Options.Randomizer.Create<string>();
        return true;
    }
}

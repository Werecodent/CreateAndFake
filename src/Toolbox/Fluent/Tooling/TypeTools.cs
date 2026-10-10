using Werecodent.CreateAndFake.RandomizerTool;

namespace Werecodent.CreateAndFake.Fluent.Tooling;

/// <inheritdoc/>
public sealed class TypeTools(Type source, ToolSet? tools) : ObjectTools<Type>(source, tools)
{
    /// <inheritdoc cref="IRandomizer.Create(Type,RandomizerMod)"/>
    public object CreateRandomInstance(RandomizerMod? optionConfiguration = null)
    {
        return Tools.Randomizer.Create(Source, optionConfiguration);
    }

    /// <inheritdoc cref="IRandomizer.CreateSized(int, Type,RandomizerMod)"/>
    public object CreateRandomSizedInstance(int size, RandomizerMod? optionConfiguration = null)
    {
        return Tools.Randomizer.CreateSized(size, Source, optionConfiguration);
    }
}

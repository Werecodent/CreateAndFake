using CreateAndFake.RandomizerTool;

namespace CreateAndFake.Fluent.Tooling;

/// <inheritdoc/>
public sealed class TypeTools(Type source, ToolSet? tools) : ObjectTools<Type>(source, tools)
{
    /// <inheritdoc cref="IRandomizer.Create(Type,RandomizerMod)"/>
    public object CreateRandomInstance(RandomizerMod? optionConfiguration = null)
    {
        return Tools.Randomizer.Create(Source, optionConfiguration);
    }
}
